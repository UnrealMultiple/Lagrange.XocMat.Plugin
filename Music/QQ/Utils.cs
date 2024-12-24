using System.IO.Compression;
using System.Web;

namespace Music.QQ;

public class Utils
{
    public static byte[] GenerateCompressed(Dictionary<string, byte[]> data)
    {
        using var ms = new MemoryStream();
        using var zip = new ZipArchive(ms, ZipArchiveMode.Create);
        foreach (var (filename, buffer) in data)
        {
            if(buffer is null || buffer.Length == 0)
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

    public static string QueryUri(string url, Dictionary<string, string>? @params = null)
    {
        var uri = new UriBuilder(url);
        var args = HttpUtility.ParseQueryString(uri.Query);
        if (@params is not null)
            foreach (var (key, value) in @params)
                args[key] = value;
        uri.Query = args.ToString();
        return uri.ToString();
    }

    /// <summary>
    /// 获取随机的 searchID
    /// </summary>
    /// <returns>随机生成的 searchID 字符串</returns>
    public static long GetSearchID()
    {
        Random random = new Random();
        int e = random.Next(1, 21); // 注意：C# 的 Next 方法上限是开区间，所以用21
        long t = e * 18014398509481984L;
        long n = random.Next(0, 4194305) * 4294967296L; // 同样，上限是开区间
        long a = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        long r = a % (24L * 60 * 60 * 1000);

        return t + n + r;
    }

    public static int Hash33(string s, int h = 0)
    {
        unchecked // 允许溢出而不抛出异常
        {
            foreach (char c in s)
            {
                h = (h << 5) + h + c; // 使用字符的int值代替ord(c)
            }
            return 2147483647 & h; // 按位与操作，确保结果在32位有符号整数范围内
        }
    }
}
