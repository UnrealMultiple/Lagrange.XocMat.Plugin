
using System.Text;
using Lagrange.XocMat.Utility.Images;
using Music.QQ.Internal.Search.Song;
using Music.WangYi;
using SixLabors.ImageSharp.PixelFormats;

namespace Music;


public class MusicTool
{
    private static readonly Dictionary<long, string> MusicLocal = [];
    private static readonly Dictionary<long, string> MusicName = [];

    public static async Task<List<SongData>> GetMusicQQList(string musicName)
    {
        return await Config.Instance.MusicQQ.SearchSong(musicName);
    }

    public static async Task<List<MusicData>> GetMusic163List(string musicName)
    {
        return await Music_163.GetMusicListByName(musicName);
    }

    public static async Task<SongData> GetMusicQQ(string musicName, int index)
    {
        return await Config.Instance.MusicQQ.GetSong(musicName, index);
    }

    public static async Task<MusicData?> GetMusic163(string musicName, int index)
    {
        return await Music_163.GetMusic(musicName, index);
    }

    public static void ChangeLocal(string local, long uin)
    {
        MusicLocal[uin] = local;
    }
    public static void ChangeName(string name, long uin)
    {
        MusicName[uin] = name;
    }

    public static string GetLocal(long uin)
    {
        if (MusicLocal.TryGetValue(uin, out var music))
        {
            return music;
        }
        return "QQ";
    }

    public static string? GetName(long uin)
    {
        if (MusicName.TryGetValue(uin, out var music))
        {
            return music;
        }
        return null;
    }
}
