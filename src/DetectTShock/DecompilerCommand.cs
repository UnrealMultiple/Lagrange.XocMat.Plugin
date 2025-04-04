using System.IO.Compression;
using System.Text;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.XocMat.Command;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Utility;
using Microsoft.Extensions.Logging;

namespace DetectTShock;
public class DecompilerCommand : Command
{
    public override string[] Alias => ["反"];

    public override string HelpText => "反编译dll!";

    public override string[] Permissions => ["onebot.csharp.decompiler"];

    public static byte[] GenerateCompressed(List<(string fileName, byte[] buffer)> data)
    {
        using var ms = new MemoryStream();
        using var zip = new ZipArchive(ms, ZipArchiveMode.Create);
        foreach (var (filename, buffer) in data)
        {
            if (buffer is null || buffer.Length == 0)
                continue;
            var entry = zip.CreateEntry(filename, CompressionLevel.Fastest);
            using var stream = entry.Open();
            stream.Write(buffer);
            stream.Flush();
        }
        ms.Flush();
        zip.Dispose();
        return ms.ToArray();
    }

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        var forwards = args.Event.Chain.GetMsg<ForwardEntity>();
        if (forwards.FirstOrDefault() is not ForwardEntity forward || !Plugin.FileCache.TryGetValue(forward.MessageId, out var file))
        {
            await args.Event.Reply("未找到文件，请重新上传后反编译!", true);
            return;
        }
        var dirInfo = new DirectoryInfo(Path.Combine(Config.Instance.DetectPath));
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }
        var buffer = await HttpUtils.GetByteAsync(file.FileUrl!);
        var targetFile = Path.Combine(dirInfo.FullName, file.FileName);
        await File.WriteAllBytesAsync(targetFile, buffer);
        using var decompiler = new DllDecompiler();
        if (!decompiler.LoadFromFile(targetFile))
        {
            await args.Event.Reply($"加载程序集失败: {decompiler.LastError}", true);
            return;
        }
        if (!decompiler.DecompileAll())
        {
            await args.Event.Reply($"反编译失败: {decompiler.LastError}", true);
            return;
        }
        File.Delete(targetFile);
        if (args.Parameters.Count > 0 && args.Parameters[0].ToLower() == "-f")
        {
            var zipContents = decompiler.DecompiledFiles.Select(x => (x.Key, Encoding.UTF8.GetBytes(x.Value))).ToList();
            var zipFileName =  Path.GetFileNameWithoutExtension(file.FileName) + "(Source).zip";
            var zipBuffer = GenerateCompressed(zipContents);
            await args.Bot.GroupFSUpload(args.GroupUin, new FileEntity(zipBuffer, zipFileName));
        }
        else
        { 
            var markdownMultiple = new List<MessageChain>();
            foreach (var (fileName, code) in decompiler.DecompiledFiles)
            {
                var msg = MessageBuilder.Friend(args.MemberUin)
                    .Markdown(new MarkdownData()
                    {
                        Content = $""""
                        # {fileName}
                        ```csharp
                        {FormatCodeBlock(code)}
                        ```
                        """"
                    });
                markdownMultiple.Add(msg.Build());
            }
            await args.MessageBuilder.MultiMsg(MessageBuilder.Friend(args.MemberUin).Text("反编译成功!").MultiMsg([.. markdownMultiple])).Reply();
        }
            
    }
    
    
    /// <summary>
    /// 修复代码块的缩进问题
    /// </summary>
    /// <param name="rawCode">原始代码</param>
    /// <param name="spacesPerIndent">每个缩进级别的空格数（默认4）</param>
    /// <param name="codeLanguage">代码语言标识（如 "csharp"）</param>
    /// <returns>格式化后的 Markdown 代码块</returns>
    public static string FormatCodeBlock(string rawCode, int spacesPerIndent = 4)
    {
        var sb = new StringBuilder();
        
        // Step 1: 替换所有制表符为空格
        string normalizedCode = rawCode.Replace("\t", new string(' ', spacesPerIndent));

        // Step 2: 分割代码行
        string[] lines = normalizedCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        // Step 4: 处理每一行
        foreach (string line in lines)
        {
            // 自动移除行尾空白（可选）
            string trimmedLine = line.TrimEnd();
            
            // 保留行首缩进
            sb.AppendLine(trimmedLine);
        }

        return sb.ToString();
    }
}
