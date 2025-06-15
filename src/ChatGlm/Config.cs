using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGlm;

[ConfigSeries]
public class Config : JsonConfigBase<Config>
{
    [JsonProperty("api_key")]
    public string ApiKey { get; set; } = string.Empty;

    [JsonProperty("api_secret")]
    public string ApiSecret { get; set; } = string.Empty;

    [JsonProperty("assistant_id")]
    public string AssistantID { get; set; } = string.Empty;
}
