using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Utility;

namespace Music.QQ;

public class MusicQQ
{
    private const string Uri = "https://oiapi.net/API/QQ_Music/";
    public static async Task<List<MusicItem>> GetMusicList(string name, string key)
    {
        var ret = new List<MusicItem>();
        var param = new Dictionary<string, string>()
        {
            { "msg", name },
            { "key", key }
        };
        var res = await HttpUtils.HttpGetString(Uri, param);
        var data = res.ToObject<ApiRespone>();
        if (data != null && data.Code == 1)
        {
            return data.Data?.ToObject<List<MusicItem>>() ?? [];
        }
        return ret;
    }

    public static async Task<MusicData?> GetMusic(string name, int id, string key)
    {
        var param = new Dictionary<string, string>()
        {
            { "msg", name },
            { "n", id.ToString()},
            { "key", key }
        };
        var res = await HttpUtils.HttpGetString(Uri, param);
        var data = res.ToObject<ApiRespone>();
        if (data != null && data.Code == 1)
        {
            return data.Data?.ToObject<MusicData>();
        }
        return null;
    }
}
