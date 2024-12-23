using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.QQ.Internal.Search.Song;

public class SongFile
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("b_30s")]
    public int B30s { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("e_30s")]
    public int E30s { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("hires_bitdepth")]
    public int HiresBitdepth { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("hires_sample")]
    public int HiresSample { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("media_mid")]
    public string MediaMid { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_128mp3")]
    public int Size128mp3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_192aac")]
    public int Size192aac { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_192ogg")]
    public int Size192ogg { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_24aac")]
    public int Size24aac { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_320mp3")]
    public int Size320mp3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_360ra")]
    public List<string> Size360ra { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_48aac")]
    public int Size48aac { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_96aac")]
    public int Size96aac { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_96ogg")]
    public int Size96ogg { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_ape")]
    public int SizeApe { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_dolby")]
    public int SizeDolby { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_dts")]
    public int SizeDts { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_flac")]
    public int SizeFlac { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_hires")]
    public int SizeHires { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_new")]
    public List<int> SizeNew { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size_try")]
    public int SizeTry { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("try_begin")]
    public int TryBegin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("try_end")]
    public int TryEnd { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}
