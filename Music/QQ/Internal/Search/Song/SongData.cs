using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.QQ.Internal.Search.Song;

public class SongData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("act")]
    public int Act { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("album")]
    public Album Album { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("bpm")]
    public int Bpm { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("desc")]
    public string Desc { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("desc_hilight")]
    public string DescHilight { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("docid")]
    public string Docid { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("eq")]
    public int Eq { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("es")]
    public string Es { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("file")]
    public SongFile File { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("fnote")]
    public int Fnote { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("genre")]
    public int Genre { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("grp")]
    public List<string> Grp { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("hotness")]
    public Hotness Hotness { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("href3")]
    public string Href3 { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("index_album")]
    public int IndexAlbum { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("index_cd")]
    public int IndexCd { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("interval")]
    public int Interval { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("isonly")]
    public int Isonly { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ksong")]
    public KSong Ksong { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("label")]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("language")]
    public int Language { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("lyric")]
    public string Lyric { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("lyric_hilight")]
    public string LyricHilight { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("mid")]
    public string Mid { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("mv")]
    public MV Mv { get; set; } = new();

    /// <summary>
    /// 搁浅
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("newStatus")]
    public int NewStatus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ov")]
    public int Ov { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("pay")]
    public Pay Pay { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("protect")]
    public int Protect { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("sa")]
    public int Sa { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("singer")]
    public List<SingerItem> Singer { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("status")]
    public int Status { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("subtitle")]
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("tag")]
    public int Tag { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("tid")]
    public int Tid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("time_public")]
    public string TimePublic { get; set; } = string.Empty;

    /// <summary>
    /// 搁浅
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 搁浅
    /// </summary>
    [JsonProperty("title_hilight")]
    public string TitleHilight { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("type")]
    public int Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("version")]
    public int Version { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("vf")]
    public List<double> Vf { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("vi")]
    public List<int> Vi { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("volume")]
    public Volume Volume { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("vs")]
    public List<string> Vs { get; set; } = [];

    [JsonIgnore]
    public string PlayUrl { get; set; } = string.Empty;

    [JsonIgnore]
    public string PageUrl => $"https://y.qq.com/n/yqq/song/{Mid}.html";
}
