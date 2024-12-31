using Lagrange.XocMat;
using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Plugin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Music.QQ;
using Music.QQ.Internal.MusicToken;
using Newtonsoft.Json;

namespace Music;

[ConfigSeries]
public class Config : JsonConfigBase<Config>
{
    protected override string Filename => "Music";

    protected override string? ReloadMsg => "[Music] config reload successfully!\n";

    [JsonProperty("QQ音乐令牌信息")]
    public TokenInfo? Token { get; set; }

    [JsonIgnore]
    private Music_QQ? _MusicQQ = null;

    [JsonIgnore]
    private static ILogger? Logger => XocMatApp.Instance.Services.GetRequiredService<PluginLoader>().PluginContext.Plugins.FirstOrDefault(i => i is Music)?.Logger;

    [JsonIgnore]
    public Music_QQ MusicQQ
    {
        get
        {
            if (_MusicQQ == null)
            {
                if (Token == null)
                {
                    throw new Exception("未设置QQ音乐令牌信息");
                }
                _MusicQQ = new Music_QQ(Token);
                _MusicQQ.TokenUpdated += MusicQQ_TokenUpdated;
            }
            return _MusicQQ;
        }
    }

    private void MusicQQ_TokenUpdated(TokenInfo obj)
    {
        Token = obj;
        Save();
        Logger?.LogInformation("[Music] QQ音乐令牌信息已更新");
    }

    public void SetToken(TokenInfo? token)
    {
        if (token == null)
            return;
        _MusicQQ?.ChangeToken(token);
        Token = token;
    }
}
