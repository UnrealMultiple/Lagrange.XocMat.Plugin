using System.Reflection;
using Lagrange.XocMat;
using PuppeteerSharp;

namespace GitHook;

internal class GithubPageUtils
{
    private static IBrowser browser = (IBrowser)typeof(XocMatAPI).Assembly.GetType("Lagrange.XocMat.Utility.MarkdownHelper")
        ?.GetField("browser", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)?.GetValue(null)!;

    public static async Task<byte[]> ScreenPage(string url, string? click = null)
    {
        if (browser == null)
        {
            await new BrowserFetcher().DownloadAsync();
            browser = await Puppeteer.LaunchAsync(new LaunchOptions()
            {
                Headless = true,
            });
            typeof(XocMatAPI).Assembly.GetType("Lagrange.XocMat.Utility.MarkdownHelper")
        ?.GetField("browser", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)?.SetValue(null, browser);
        }
        var page = await browser.NewPageAsync();
        await page.GoToAsync(url, WaitUntilNavigation.Load).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(click))
            // 隐藏左侧文件列表（可能需要根据实际DOM结构调整选择器）
            await page.ClickAsync(click);

        var ret = await page.ScreenshotDataAsync(new()
        {
            Type = ScreenshotType.Png,
            FullPage = true
        });
        await page.CloseAsync();
        return ret;
    }
}
