using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
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
            }
            return _MusicQQ;
        }
    }

    public void SetToken(TokenInfo? token)
    {
        if (token == null)
            return;
        _MusicQQ = new Music_QQ(token);
        Token = token;
    }
}
