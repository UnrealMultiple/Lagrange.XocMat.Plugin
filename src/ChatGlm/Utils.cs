using ChatGlm.JContent.Segment;
using Lagrange.XocMat.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGlm;

public class Utils
{
    private static readonly ChatGlm _glm = new(Config.Instance.ApiKey, Config.Instance.ApiSecret, Config.Instance.AssistantID);

    public static async Task<string> Chat(string text)
    { 
        var response = await _glm.ChatAsync(text);
        return response.GetEntity<TextSegment>().JoinToString("\n\n", x => x.Text);
    }
}
