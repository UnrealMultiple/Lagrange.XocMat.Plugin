using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.QQ.Internal.Search;

internal class ReqData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("body")]
    public Body Body { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("feedbackURL")]
    public string FeedbackURL { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("meta")]
    public JToken? Meta { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("ver")]
    public int Ver { get; set; }
}
