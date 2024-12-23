﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.QQ.Internal;

public class Req
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("data")]
    public JToken? Data { get; set; }
}