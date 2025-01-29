using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lagrange.XocMat;
using PuppeteerSharp;

namespace GitHook;

internal class GithubPageUtils
{
    private static IBrowser browser = (IBrowser)typeof(XocMatAPI).Assembly.GetType("Lagrange.XocMat.Utility.MarkdownHelper")
        ?.GetField("browser", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)?.GetValue(null)!;

    public static async Task<byte[]> ScreenPage(string url)
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
        await page.GoToAsync(url, 60 * 1000).ConfigureAwait(false);
        var ret = await page.ScreenshotDataAsync(new()
        {
            Type = ScreenshotType.Png,
            FullPage = true
        });
        await page.CloseAsync();
        return ret;
    }
}
