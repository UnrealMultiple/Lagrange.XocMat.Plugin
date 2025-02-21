using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Configuration;
using Newtonsoft.Json;

namespace CommandUtils;

[ConfigSeries]
public class Config : JsonConfigBase<Config>
{
    [JsonProperty("启用")]
    public bool Enabled { get; set; } = true;

    [JsonProperty("群禁指令")]
    public Dictionary<long, CommandBody> GroupDisabledCommands { get; set; } = [];

    public bool AddDisabledCommand(long groupId, string command)
    {
        if (!GroupDisabledCommands.TryGetValue(groupId, out CommandBody? value))
        {
            value = new CommandBody();
            GroupDisabledCommands[groupId] = value;
        }
        return value.DisabledCommands.Add(command);
    }

    public bool RemoveDisabledCommand(long groupId, string command)
    {
        if (GroupDisabledCommands.TryGetValue(groupId, out CommandBody? value))
        {
            return value.DisabledCommands.Remove(command);
        }
        return false;
    }

    public bool AddAllowedCommand(long groupId, string command)
    {
        if (!GroupDisabledCommands.TryGetValue(groupId, out CommandBody? value))
        {
            value = new CommandBody();
            GroupDisabledCommands[groupId] = value;
        }
        return value.AllowedCommands.Add(command);
    }

    public bool RemoveAllowedCommand(long groupId, string command)
    {
        if (GroupDisabledCommands.TryGetValue(groupId, out CommandBody? value))
        {
            return value.AllowedCommands.Remove(command);
        }
        return false;
    }
}

public class CommandBody
{
    [JsonProperty("禁用指令")]
    public HashSet<string> DisabledCommands { get; set; } = [];

    [JsonProperty("允许指令")]
    public HashSet<string> AllowedCommands { get; set; } = [];
}