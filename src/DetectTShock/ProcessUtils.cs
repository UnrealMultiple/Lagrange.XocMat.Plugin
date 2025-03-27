using System;
using System.Diagnostics;
using System.Text;

namespace DetectTShock;

public class ProcessUtils
{
    public static ProcessResult StartProcess(string fileName, string arguments = "")
    {
        var output = new StringBuilder();
        var error = new StringBuilder();

        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            WorkingDirectory = Path.GetDirectoryName(fileName),
            UseShellExecute = false,          // 必须设为 false 才能重定向 IO
            RedirectStandardInput = true,     // 新增：重定向标准输入
            RedirectStandardOutput = true,    // 重定向标准输出
            RedirectStandardError = true,     // 重定向错误输出
            CreateNoWindow = true,            // 不创建窗口
            StandardOutputEncoding = Encoding.UTF8,  // 设置输出编码
            StandardErrorEncoding = Encoding.UTF8    // 设置错误编码
        };

        using var process = new Process { StartInfo = startInfo };
        
        // 注册输出数据接收事件
        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null) output.AppendLine(e.Data);
        };

        // 注册错误数据接收事件
        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data != null) error.AppendLine(e.Data);
        };

        process.Start();

        // 新增：发送回车并关闭输入流
        try 
        {
            process.StandardInput.WriteLine(); // 发送回车
            process.StandardInput.Close();      // 关闭输入流
        }
        catch 
        { 
            // 处理可能出现的异常（如进程已退出）
        }

        // 开始异步读取输出
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        // 等待进程退出
        process.WaitForExit();

        return new ProcessResult
        {
            ExitCode = process.ExitCode,
            StandardOutput = output.ToString(),
            ErrorOutput = error.ToString()
        };
    }
}

public class ProcessResult
{
    public int ExitCode { get; set; }
    public string StandardOutput { get; set; } = string.Empty;
    public string ErrorOutput { get; set; } = string.Empty;
}