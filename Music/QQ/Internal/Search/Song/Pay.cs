using Newtonsoft.Json;

namespace Music.QQ.Internal.Search.Song;

public class Pay
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("pay_down")]
    public int PayDown { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("pay_month")]
    public int PayMonth { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("pay_play")]
    public int PayPlay { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("pay_status")]
    public int PayStatus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("price_album")]
    public int PriceAlbum { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("price_track")]
    public int PriceTrack { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("time_free")]
    public int TimeFree { get; set; }
}
