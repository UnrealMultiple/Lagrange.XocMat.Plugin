using Music.QQ.Internal.Search.Song;
using Newtonsoft.Json;

namespace Music.QQ.Internal.Playlists;

public class PlayData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("subcode")]
    public int Subcode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("msg")]
    public string Msg { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("from_gedan_plaza")]
    public int FromGedanPlaza { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("accessed_plaza_cache")]
    public int AccessedPlazaCache { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("accessed_byfav")]
    public int AccessedByfav { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("optype")]
    public int Optype { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("filter_song_num")]
    public int FilterSongNum { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("sac_forbid")]
    public List<string> SacForbid { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("dirinfo")]
    public Dirinfo Dirinfo { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("songlist_size")]
    public int SonglistSize { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("songlist")]
    public List<SongData> Songlist { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("songtag")]
    public List<SongtagItem> Songtag { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("toplist_song")]
    public List<string> ToplistSong { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("toplist_nolimit")]
    public bool ToplistNolimit { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("login_uin")]
    public int LoginUin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("invalid_song")]
    public List<string> InvalidSong { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("filtered_song")]
    public List<string> FilteredSong { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ad_list")]
    public List<string> AdList { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("total_song_num")]
    public int TotalSongNum { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("encrypt_login")]
    public string EncryptLogin { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ct")]
    public int Ct { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("cv")]
    public int Cv { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ip")]
    public string Ip { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("orderlist")]
    public List<OrderlistItem> Orderlist { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("birthday")]
    public List<string> Birthday { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("aiExt")]
    public AiExt AiExt { get; set; } = new();

    ///// <summary>
    ///// 
    ///// </summary>
    //[JsonProperty("quickListenVid")]
    //public List<string> QuickListenVid { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("bitflag")]
    public int Bitflag { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("cmtURL_bykey")]
    public CmtURLBykey CmtURLBykey { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("srf_ip")]
    public string SrfIp { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("referer")]
    public string Referer { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("namedflag")]
    public int Namedflag { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("isAd")]
    public int IsAd { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("adTitle")]
    public string AdTitle { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("adUrl")]
    public string AdUrl { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("recomUgcValid")]
    public int RecomUgcValid { get; set; }
}
