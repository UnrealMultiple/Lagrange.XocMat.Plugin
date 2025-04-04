using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;

namespace DetectTShock;

public class DllDecompiler : IDisposable
{
    #region 公共属性
    public Dictionary<string, string> DecompiledFiles { get; } = new();
    public Exception LastError { get; private set; }
    #endregion

    #region 私有字段
    private PEFile _peFile;
    private CSharpDecompiler _decompiler;
    private UniversalAssemblyResolver _resolver;
    #endregion

    #region 加载方法
    /// <summary>
    /// 从文件路径加载DLL
    /// </summary>
    public bool LoadFromFile(string filePath)
    {
        try
        {
            // 将文件完全读入内存
            byte[] buffer = File.ReadAllBytes(filePath);
            using var stream = new MemoryStream(buffer);
        
            // 使用内存流创建PEFile
            _peFile = new PEFile(
                filePath, 
                stream, 
                PEStreamOptions.PrefetchMetadata | PEStreamOptions.PrefetchEntireImage
            );
        
            InitializeResolver(filePath);
            return true;
        }
        catch (Exception ex)
        {
            LastError = ex;
            return false;
        }
    }

    /// <summary>
    /// 从内存字节流加载DLL
    /// </summary>
    /// <param name="buffer">DLL字节数据</param>
    /// <param name="virtualName">虚拟文件名（用于依赖解析）</param>
    public bool LoadFromBuffer(byte[] buffer, string virtualName = "MemoryAssembly.dll")
    {
        try
        {
            using var stream = new MemoryStream(buffer);
            _peFile = new PEFile(virtualName, stream);
            InitializeResolver(virtualName);
            return true;
        }
        catch (Exception ex)
        {
            LastError = ex;
            return false;
        }
    }
    #endregion

    #region 核心反编译逻辑
    /// <summary>
    /// 执行完整反编译
    /// </summary>
    public bool DecompileAll()
    {
        try
        {
            if (_decompiler == null) return false;

            var mainModule = _decompiler.TypeSystem.MainModule;
            foreach (var typeDef in mainModule.TypeDefinitions)
            {
                if (ShouldSkipType(typeDef)) continue;

                var result = ProcessType(typeDef);
                if (result.HasValue)
                {
                    DecompiledFiles[result.Value.FileName] = result.Value.Code;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            LastError = ex;
            return false;
        }
    }
    #endregion

    #region 辅助方法
    private void InitializeResolver(string assemblyPath)
    {
        _resolver = new UniversalAssemblyResolver(
            assemblyPath,
            false,
            _peFile.DetectTargetFrameworkId(),
            _peFile.DetectRuntimePack(),
            PEStreamOptions.PrefetchEntireImage
        );

        _decompiler = new CSharpDecompiler(
            _peFile,
            _resolver,
            new DecompilerSettings { ThrowOnAssemblyResolveErrors = false }
        );
    }

    private (string FileName, string Code)? ProcessType(ITypeDefinition typeDef)
    {
        try
        {
            var fileName = GenerateFileName(typeDef);
            var code = GenerateCode(typeDef);
            return (fileName, code);
        }
        catch (Exception ex)
        {
            return ($"Error_{Guid.NewGuid()}.txt", $"/* Decompilation Error: {ex.Message} */");
        }
    }

    private string GenerateFileName(ITypeDefinition typeDef)
    {
        // 处理命名空间路径
        var nsPath = !string.IsNullOrEmpty(typeDef.Namespace)
            ? typeDef.Namespace.Replace('.', Path.DirectorySeparatorChar)
            : "GlobalTypes";

        // 处理类型名称
        var typeName = typeDef.FullTypeName.ToString();
        if (!string.IsNullOrEmpty(typeDef.Namespace) && typeName.StartsWith(typeDef.Namespace))
        {
            typeName = typeName.Substring(typeDef.Namespace.Length + 1);
        }

        // 清洗非法字符
        var safeName = SanitizeFileName(typeName
            .Replace('+', '.')    // 嵌套类型
            .Replace('<', '[')    // 泛型
            .Replace('>', ']'));

        return Path.Combine(nsPath, $"{safeName}.cs");
    }

    private string GenerateCode(ITypeDefinition typeDef)
    {
        var code = _decompiler.DecompileTypeAsString(typeDef.FullTypeName);
        return $"// Decompiled from: {typeDef.FullTypeName}\n" +
               $"// Assembly: {_peFile.Name}\n\n" +
               code;
    }

    private static string SanitizeFileName(string name)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return string.Join("_", name.Split(invalidChars))
            .Replace("..", ".")  // 防止多个点
            .Trim('.');
    }

    private static bool ShouldSkipType(ITypeDefinition typeDef)
    {
        return typeDef.IsCompilerGenerated() ||  // 跳过编译器生成的类型
               typeDef.Name.Contains("<");       // 跳过匿名类型
    }
    #endregion

    #region 依赖管理
    /// <summary>
    /// 添加依赖搜索路径
    /// </summary>
    public void AddReferencePath(string directory)
    {
        _resolver?.AddSearchDirectory(directory);
    }
    #endregion

    #region 资源清理
    public void Dispose()
    {
        _peFile?.Dispose();
        _resolver = null;
        _decompiler = null;
        GC.SuppressFinalize(this);
    }

    ~DllDecompiler() => Dispose();
    #endregion
}