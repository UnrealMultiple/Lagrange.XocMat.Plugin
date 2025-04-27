using System.Collections.Concurrent;
using DeepSeek.Core;
using DeepSeek.Core.Models;

namespace DeepSeek;

internal class Utils
{
    public static readonly Utils Instance = new();

    public DeepSeekClient _client = new(Config.Instance.APIKey);

    private readonly ConcurrentDictionary<ulong, ChatRequest> MemberChats = [];
    public async Task<string> ChatContent(uint userId, string query)
    {
        var request = GetChatRequest(userId);
        request.Messages.Add(Message.NewUserMessage(query));
        var chatResponse = await _client.ChatAsync(request, CancellationToken.None);
        return chatResponse?.Choices.First().Message?.Content ?? throw new Exception($"No response from DeepSeek {_client.ErrorMsg}");
    }

    public async Task<string> Chat(string query)
    {
        var request = new ChatRequest
        {
            Messages = [
               Message.NewSystemMessage(Config.Instance.SystemMessage),
               Message.NewUserMessage(query)
            ],
            // Specify the model
            Model = Config.Instance.Model
        };
        var chatResponse = await _client.ChatAsync(request, CancellationToken.None);
        return chatResponse?.Choices.First().Message?.Content ?? throw new Exception($"No response from DeepSeek {_client.ErrorMsg}");
    }

    public void ClearChat(ulong userId)
    {
        MemberChats.Remove(userId, out _);
    }

    private ChatRequest GetChatRequest(ulong userId)
    {
        if (MemberChats.TryGetValue(userId, out ChatRequest? value))
        {
            return value;
        }
        var request = new ChatRequest
        {
            Messages = [
                Message.NewSystemMessage(Config.Instance.SystemMessage),
            ],
            // Specify the model
            Model = Config.Instance.Model
        };
        MemberChats[userId] = request;
        return request;
    }
}
