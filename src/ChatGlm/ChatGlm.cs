using ChatGlm.JContent;
using System.Text;
using System.Text.Json;
using System.Timers;

namespace ChatGlm;

public class ChatGlm
{
    private string Access_Token;

    private readonly HttpClient _client;

    public int TokenExpires;

    private readonly string Apikey;

    private readonly string ApiSecret;

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly string AssistantID;

    private readonly System.Timers.Timer _timer;
    public ChatGlm(string key, string secret, string assistant_id)
    {
        Apikey = key;
        ApiSecret = secret;
        AssistantID = assistant_id;
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://chatglm.cn/chatglm/assistant-api/v1/")
        };
        var result = GetAccessToken().GetAwaiter().GetResult();
        Access_Token = result.AccessToken;
        TokenExpires = result.ExpiresOut;
        _client.DefaultRequestHeaders.Authorization = new("Bearer", Access_Token);
        _timer = new();
        _timer.Enabled = true;
        _timer.AutoReset = true;
        _timer.Interval = 1000;
        _timer.Elapsed += _timer_Elapsed;
        _timer.Start();

    }

    private void _timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (DateTimeOffset.Now.ToUnixTimeMilliseconds() > TokenExpires - 100)
        {
            var result = GetAccessToken().GetAwaiter().GetResult();
            Access_Token = result.AccessToken;
            TokenExpires = result.ExpiresOut;
            _client.DefaultRequestHeaders.Authorization = new("Bearer", Access_Token);
        }
    }

    public async Task<StreamSyncResult> ChatAsync(string text)
    {
        var body = new Dictionary<string, string>
        {
            { "assistant_id", AssistantID },
            { "prompt", text }
        };
        return await Request<StreamSyncResult>("stream_sync", body);
    }

    private async Task<AccessTokenResult> GetAccessToken()
    {
        var body = new Dictionary<string, string>
        {
            { "api_key", Apikey },
            { "api_secret", ApiSecret }
        };
        var result = await Request<AccessTokenResult>("get_token", body);
        return result;
    }



    private async Task<T> Request<T>(string action, Dictionary<string, string> body) where T : class, new()
    {
        var requestBody = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var responseContent = await _client.PostAsync(action, requestBody);
        if (responseContent.IsSuccessStatusCode)
        {
            var json = await responseContent.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BasicStruct<T>>(json, _serializerOptions);
            return result?.Result ?? throw new HttpRequestException("Deserialization returned null!");
        }
        throw new HttpRequestException($"Response error! status code: {responseContent.StatusCode}!");
    }
}
