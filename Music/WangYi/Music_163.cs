using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace Music.WangYi;
public class Music_163
{
    private const string encSecKey = "5cab2b2073b315b420815a56c81f57f62b396655bd04d5005fb1de166811c631551b1da456fdc6eb45f87ae926b19c74a06b3fc6a04c5018a52d5960a44cae7eeafd543a2b1735f4a60f74c23ef487ed15c1f40f35af40fdb66ec4fc67fe991fdefd3266ea8f55cddbaabb0f75f8b19c1c7eca6447dec6938969045e04851929";

    private const string iv = "ABxeF5MspBB0AbUJ";

    private const string ig = "0CoJUm6Qyw8W8jud";

    private const string key = "0102030405060708";

    private const string musicList = "https://music.163.com/weapi/cloudsearch/get/web?csrf_token=";

    private const string playMusic = "https://music.163.com/weapi/song/enhance/player/url/v1?csrf_token=";

    private const string musicinfo = "https://music.163.com/weapi/v3/song/detail?csrf_token=";

    private const string musicPlaylists = "https://music.163.com/weapi/v3/playlist/detail?csrf_token=";

    private static readonly HttpClient Client = new(new HttpClientHandler()
    {
        AutomaticDecompression = DecompressionMethods.GZip
    })
    {
        DefaultRequestHeaders =
        {
            { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.79" },
            { "Origin", "https://music.163.com" },
            { "Referer", "https://music.163.com/" },
            { "Cookie", "osver=undefined; deviceId=undefined; appver=8.10.05; versioncode=140; mobilename=undefined; buildver=1699788225; resolution=1920x1080; __csrf=; os=android; channel=undefined; requestId=1699788225483_0541;MUSIC_U=00DCE90E4414477F6CEB1D678986B3E798756DC0B3789AC24E863D0F1CDA8392E2191A215A72C87DD76D33AC15963F1D4581ADFAD71698E9CEE1F59205B30465327BD608B9A4C03E907A43561CC8BD9A21C0D400237A879F6E5CDEFED2B7ADD78FD44F6402E41100966CD15F655BE0C37A18D1103134FAAE42BE3D77AEB60D300BE1A2789E1B4F7EB956E1969D2CED89D57D629398263FB44214E8BF12D201B368A9DFF0B1AE062C24A80C57953E8D42B4FBDA2B11ADD2E8C87F230727EAB2D75DC85C3A8D033CF2ABD045131969431DF3BCC689B902402FF9A683CDF5C96EFF1FBFD2563BF50EDAFB2200C887A51F4FF10B4D14A5AA745BBD62DD7DB1C5EA183E3FE575795096A830BF3FA91D685B96E981718C1568BF95E2D9A146509FE4430570AF16B22DC144D77C61D654F90046F61DC210814E63661061EFA80136272A0DF51F97529AC412523D009391B77DAF29" }
        }
    };

    /// <summary>
    /// AES加密
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    private static string EncryptByAES(string plainText, string key, string iv)
    {
        var Key = Encoding.UTF8.GetBytes(key);
        var IV = Encoding.UTF8.GetBytes(iv);
        byte[] encrypted;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
            encrypted = msEncrypt.ToArray();
        }
        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// 生成表单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    private static async Task<string> GetPostForm(string url, object param)
    {
        var str = EncryptByAES(JsonConvert.SerializeObject(param), ig, key);
        var args = EncryptByAES(str, iv, key);
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "params", args },
            { "encSecKey", encSecKey }

        });
        var response = await Client.PostAsync(url, content);
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 通过id获取music链接
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<string> GetMusicUrlByid(long id)
    {
        try
        {
            var urls = await GetMusicUrlByid([id]);
            if (urls.TryGetValue(id, out var url))
                return url;
            else
                throw new Exception("无法获取歌曲url链接");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 通过id列表获取音乐链接
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static async Task<Dictionary<long, string>> GetMusicUrlByid(IEnumerable<long> ids)
    {
        var param = new
        {
            ids = $"[{string.Join(",", ids)}]",
            level = "standard",
            encodeType = "acc",
            csrf_token = ""
        };
        var result = await GetPostForm(playMusic, param);
        var musicData = JsonNode.Parse(result)!;
        if (string.IsNullOrEmpty(result) || musicData == null)
            throw new Exception("无法获取到数据，请检查Music。加密算法是否更改!");
        if (musicData["code"]?.GetValue<int>() != 200)
        {
            throw new Exception("API请求失败，无法获取音乐信息!");
        }
        var songs = musicData["data"]!.AsArray()!;
        var res = new Dictionary<long, string>();

        for (int i = 0; i < songs.Count; i++)
        {
            if (songs[i]?["url"] != null)
            { 
                var id = songs[i]!["id"]!.GetValue<long>()!;
                var url = songs[i]!["url"]!.GetValue<string>()!;
                res.Add(id, url);
            }
        }
        return res;
    }

    /// <summary>
    /// 通过音乐名称和序号获取音乐
    /// </summary>
    /// <param name="name"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<MusicData> GetMusic(string name, int index)
    {
        index = index <= 0 ? 0 : index - 1;
        var musicList = await GetMusicListByName(name);
        if (musicList.Count == 0)
        {
            throw new Exception("无法获取音乐列表!");
        }

        if (index >= musicList.Count)
        {
            throw new Exception("不存在次序号的音乐!");
        }
        var music = musicList[index];
        music.SetMusicUrl(await GetMusicUrlByid(music.ID));
        await GetMusicByID(music.ID);
        return music;
    }

    /// <summary>
    /// 通过音乐id获取音乐
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<MusicData> GetMusicByID(long id)
    {
        try
        {
            var list = await GetMusicByID([id]);
            if (list.Count == 0)
            {
                throw new Exception("歌曲信息获取失败");
            }
            return list[0];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static async Task<PlaylistsData> GetPlaylists(long id)
    {
        var param = new
        {
            id,
            n = 1000,
            csrf_token = ""
        };
        var result = await GetPostForm(musicPlaylists, param);
        return JObject.Parse(result).ToObject<PlaylistsData>() ?? throw new Exception("歌单获取失败!");
    }

    public static async Task<byte[]> DownloadPlaylists(long id)
    {
        var playlist = await GetPlaylists(id);
        Console.WriteLine(playlist.Playlist.TrackCount);
        if (playlist.Code != 200 || playlist.Playlist.Tracks.Length == 0)
            throw new Exception("获取歌单失败!");
        var list = playlist.Playlist.Tracks.ToDictionary(i => i.Id, i => i);
        var zipName = $"网易歌单[{id}].zip";
        //分割mid
        var SpitMid = list.Keys.ToArray().Chunk(100).Select(s => s.ToList()).ToList();
        //将url全部保存
        var urls = new Dictionary<long, string>();
        foreach (var item in SpitMid)
        {
            var url = await GetMusicUrlByid([.. item]);
            urls = urls.Concat(url).ToDictionary(k => k.Key, v => v.Value);
        }
        using var ms = new MemoryStream();
        using var zip = new ZipArchive(ms, ZipArchiveMode.Create);
        SemaphoreSlim SemaphoreSlim = new(1);
        using var client = new HttpClient();
        foreach (var (mid, song) in list)
        {
            await SemaphoreSlim.WaitAsync();
            try
            {
                var url = urls[mid];
                var buffer = await client.GetByteArrayAsync(url);
                var entry = zip.CreateEntry(song.Name + ".mp3", CompressionLevel.Fastest);
                using var stream = entry.Open();
                stream.Write(buffer);
                stream.Flush();
            }
            catch
            {

            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
        ms.Flush();
        zip.Dispose();
        return ms.ToArray();
    }

    /// <summary>
    /// 通过音乐id列表获取音乐
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<List<MusicData>> GetMusicByID(IEnumerable<long> ids)
    {
        var args = new List<object>();
        foreach (var id in ids)
        {
            args.Add(new { id });
        }
        var param = new
        {
            id = ids.First(),
            c = JsonConvert.SerializeObject(args),
            csrf_token = ""
        };
        var result = await GetPostForm(musicinfo, param);
        var data = JsonNode.Parse(result)!;
        if (string.IsNullOrEmpty(result) || data == null)
            throw new Exception("无法获取到数据，请检查Music。加密算法是否更改!");
        var res = new List<MusicData>();
        if (data["code"]?.GetValue<int>() == 200)
        {
            var musicList = data["songs"]!.AsArray()!;
            for (int i = 0; i < musicList.Count; i++)
            {
                var musicName = musicList[i]?["name"]?.GetValue<string>()!;
                var musicPicurl = musicList[i]?["al"]?["picUrl"]?.GetValue<string>()!;
                var musicID = musicList[i]?["id"]?.GetValue<long>()!;
                var musicSingers = musicList[i]?["ar"]?.AsArray()!;
                var singers = new List<string>();
                var url = await GetMusicUrlByid(musicID!.Value);
                for (int n = 0; n < musicSingers.Count; n++)
                {
                    singers.Add(musicSingers[n]?["name"]?.GetValue<string>()!);
                }
                res.Add(new(musicName, musicID!.Value, musicPicurl, singers, url));
            }
        }
        else
        {
            throw new Exception("无法获取API数据!");
        }
        return res;
    }

    /// <summary>
    /// 通过名称获取音乐列表
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static async Task<List<MusicData>> GetMusicListByName(string name)
    {
        var param = new
        {
            hlpretag = "<span class=\"s-fc7\">",
            hlposttag = "</span>",
            s = name,
            type = 1,
            offset = 0,
            total = "true",
            limit = 30,
            csrf_token = ""
        };
        var result = await GetPostForm(musicList, param);
        var data = JsonNode.Parse(result)!;
        if (string.IsNullOrEmpty(result) || data == null)
            throw new Exception("无法获取到数据，请检查Music_163。加密算法是否更改!");
        var res = new List<MusicData>();
        if (data["code"]?.GetValue<int>() == 200)
        {
            var musicList = data["result"]?["songs"]?.AsArray()!;
            for (int i = 0; i < musicList.Count; i++)
            {
                var musicName = musicList[i]?["name"]?.GetValue<string>()!;
                var musicPicurl = musicList[i]?["al"]?["picUrl"]?.GetValue<string>()!;
                var musicID = musicList[i]?["id"]?.GetValue<long>()!;
                var musicSingers = musicList[i]?["ar"]?.AsArray()!;
                var singers = new List<string>();
                for (int n = 0; n < musicSingers.Count; n++)
                {
                    singers.Add(musicSingers[n]?["name"]?.GetValue<string>()!);
                }
                res.Add(new(musicName, musicID!.Value, musicPicurl, singers));
            }
        }
        else
        {
            throw new Exception("无法获取API数据!");
        }
        return res;
    }
}

