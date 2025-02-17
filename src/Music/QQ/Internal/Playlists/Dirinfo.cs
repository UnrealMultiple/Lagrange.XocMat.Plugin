using Newtonsoft.Json;

namespace Music.QQ.Internal.Playlists;

public class Dirinfo
{

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("id")]
    public long Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("host_uin")]
    public long HostUin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("dirid")]
    public int Dirid { get; set; }

    /// <summary>
    /// 伤感DJ：你何必假装快乐
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("picurl")]
    public string Picurl { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("picid")]
    public int Picid { get; set; }

    /// <summary>
    /// 你何必假装快乐?我又何必真心难过.
    /// </summary>
    [JsonProperty("desc")]
    public string Desc { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("vec_tagid")]
    public List<int> VecTagid { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("vec_tagname")]
    public List<string> VecTagname { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ctime")]
    public int Ctime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("mtime")]
    public int Mtime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("listennum")]
    public int Listennum { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ordernum")]
    public int Ordernum { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("picmid")]
    public string Picmid { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("dirtype")]
    public int Dirtype { get; set; }

    /// <summary>
    /// 人间忧伤
    /// </summary>
    [JsonProperty("host_nick")]
    public string HostNick { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("songnum")]
    public int Songnum { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ordertime")]
    public int Ordertime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("show")]
    public int Show { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("picurl2")]
    public string Picurl2 { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("song_update_time")]
    public int SongUpdateTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("song_update_num")]
    public int SongUpdateNum { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("disstype")]
    public int Disstype { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ai_uin")]
    public long AiUin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("dv2")]
    public int Dv2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("dir_show")]
    public int DirShow { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("encrypt_uin")]
    public string EncryptUin { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("encrypt_ai_uin")]
    public string EncryptAiUin { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("owndir")]
    public int Owndir { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("headurl")]
    public string Headurl { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("tag")]
    public List<string> Tag { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("creator")]
    public Creator Creator { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("status")]
    public int Status { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("edge_mark")]
    public string EdgeMark { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("layer_url")]
    public string LayerUrl { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ext1")]
    public string Ext1 { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ext2")]
    public string Ext2 { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("origin_title")]
    public string OriginTitle { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ad_tag")]
    public bool AdTag { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("aiToast")]
    public string AiToast { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("role")]
    public int Role { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("rl2")]
    public int Rl2 { get; set; }
}