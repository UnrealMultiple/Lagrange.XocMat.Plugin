using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace Music.QQ.Internal.MusicToken;

public class TokenInfo
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("openid")]
    public string Openid { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("expired_at")]
    public int ExpiredAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("musicid")]
    public long Musicid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("musickey")]
    public string Musickey { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("musickeyCreateTime")]
    public int MusickeyCreateTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("first_login")]
    public int FirstLogin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("errMsg")]
    public string ErrMsg { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("sessionKey")]
    public string SessionKey { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("unionid")]
    public string Unionid { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("str_musicid")]
    public string StrMusicid { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("errtip")]
    public string Errtip { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("nick")]
    public string Nick { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("logo")]
    public string Logo { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("feedbackURL")]
    public string FeedbackURL { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("encryptUin")]
    public string EncryptUin { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("userip")]
    public string Userip { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("lastLoginTime")]
    public int LastLoginTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("keyExpiresIn")]
    public int KeyExpiresIn { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("refresh_key")]
    public string RefreshKey { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("loginType")]
    public int LoginType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("prompt2bind")]
    public int Prompt2bind { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("logoffStatus")]
    public int LogoffStatus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("otherAccounts")]
    public List<string> OtherAccounts { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("otherPhoneNo")]
    public string OtherPhoneNo { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("token")]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("isPrized")]
    public int IsPrized { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("isShowDevManage")]
    public int IsShowDevManage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("errTip2")]
    public string ErrTip2 { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("tip3")]
    public string Tip3 { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("encryptedPhoneNo")]
    public string EncryptedPhoneNo { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("phoneNo")]
    public string PhoneNo { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("bindAccountType")]
    public int BindAccountType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("needRefreshKeyIn")]
    public int NeedRefreshKeyIn { get; set; }

    [JsonProperty("p_skey")]
    public string P_Skey { get; set; } = string.Empty;

    [JsonProperty("cookie")]
    public Dictionary<string, string> Cookie { get; set; } = [];
}
