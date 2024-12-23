using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using Music.QQ.Internal;
using Music.QQ.Internal.MusicToken;
using Music.QQ.Internal.Playlists;
using Music.QQ.Internal.QuerSong;
using Music.QQ.Internal.Search;
using Music.QQ.Internal.User;
using Music.QQ.Internal.Search.Song;
using Music.QQ.Enums;
using System.IO.Compression;

namespace Music.QQ;

public class Music_QQ
{
    internal const string QQMusicApi = "https://u.y.qq.com/cgi-bin/musicu.fcg";

    private readonly CookieContainer Cookie;

    private readonly HttpClientHandler httpClientHandler;

    private readonly HttpClient client;

    private TokenInfo Token;

    public Music_QQ(TokenInfo token)
    {
        Cookie = new();
        foreach (var item in token.Cookie)
        {
            Cookie.Add(new Cookie(item.Key, item.Value, "/", "y.qq.com"));
        }
        httpClientHandler = new()
        {
            CookieContainer = Cookie,
            UseCookies = true
        };
        client = new(httpClientHandler)
        {
            DefaultRequestHeaders = { { "Referer", "y.qq.com" }, { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36 Edg/131.0.0.0" } }
        };
        Token = token;
    }


    public async Task<HomePageData> GetUserInfo()
    {
        var req = new
        {
            module = "music.UnifiedHomepage.UnifiedHomepageSrv",
            method = "GetHomepageHeader",
            param = new
            {
                uin = Token.Musicid.ToString(),
                IsQueryTabDetail = 1
            }
        };
        return (await Send(req)).Req.Data?.ToObject<HomePageData>() ?? throw new Exception("获取信息失败!");
    }

    public async Task<TokenInfo> RefreshToken()
    {
        var req = new
        {
            module = "QQConnectLogin.LoginServer",
            method = "QQLogin",
            param = new
            {
                refresh_key = Token.RefreshToken,
                refresh_token = Token.RefreshToken,
                musickey = Token.Musickey,
                musicid = Token.Musicid,
            }
        };
        var response = await Send(req);
        var obj = response.Req.Data?.ToObject<TokenInfo>() ?? throw new Exception("token刷新失败!");
        var cookie = Cookie.GetCookies(new Uri(QQMusicApi)).ToDictionary(c => c.Name, c => c.Value);
        if (response.Req.Code != 0)
            return Token;
        obj.Cookie = cookie;
        Token = obj;
        return obj;
    }

    public async Task<bool> CheckExpired()
    {
        var req = new
        {
            module = "music.UserInfo.userInfoServer",
            method = "GetLoginUserInfo",
            param = new { }
        };
        var res = (await Send(req)).Req.Code;
        return res == 0;
    }

    public async Task<Response> Send(object req)
    {
        var param = new
        {
            comm = new
            {
                ct = "11",
                cv = "13020508",
                v = "13020508",
                tmeAppID = "qqmusic",
                uid = "3931641530",
                format = "json",
                inCharset = "utf-8",
                outCharset = "utf-8",
                QIMEI36 = (await QimeiService.GetQimeiAsync(new Device(), "13.2.5.8")).Q36,
                qq = Token.Musicid.ToString(),
                authst = Token.Musickey,
                tmeLoginType = Token.LoginType.ToString(),
            },
            req
        };
        var content = new StringContent(JsonConvert.SerializeObject(param));
        var res = await client.PostAsync(QQMusicApi, content);
        var json = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response>(json) ?? throw new Exception("获取返回数据失败!!");
    }

    public async Task<List<SongData>> SearchSong(string name)
    {
        var req = new
        {
            module = "music.search.SearchCgiService",
            method = "DoSearchForQQMusicDesktop",
            platform = "desktop",
            param = new
            {
                search_id = Utils.GetSearchID(),
                search_type = 0,
                query = name,
                page_num = 1,
                page_id = 1,
                highlight = 0,
                num_per_page = 10,
                grp = 1
            }
        };
        return ((await Send(req)).Req.Data?.ToObject<ReqData>())?.Body.Songs.List ?? throw new Exception("搜索失败!");
    }



    public async Task<byte[]> DownloadPlaylists(long id)
    {
        var playlist = await GetDetail(id);
        var list = playlist.Songlist.ToDictionary(i => i.Mid, i => i);
        var zipName = $"歌单[{id}].zip";
        //分割mid
        var SpitMid = list.Keys.ToArray().Chunk(100).Select(s => s.ToList()).ToList();
        //将url全部保存
        var urls = new Dictionary<string, string>();
        foreach (var item in SpitMid)
        {
            var url = await GetSongData(SongFileType.MP3_128, [.. item]);
            urls = urls.Concat(url.ToDictionary(i => i.Key, i => i.Value.PlayUrl)).ToDictionary(k => k.Key, v => v.Value);
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

    public async Task<PlayData> GetDetail(long id, int dirid = 0)
    {
        var req = new
        {
            module = "music.srfDissInfo.DissInfo",
            method = "CgiGetDiss",
            param = new
            {
                disstid = id,
                dirid,
                tag = 0,
                song_num = 0,
                userinfo = 1,
                orderlist = 1
            }
        };
        return (await Send(req)).Req.Data?.ToObject<PlayData>() ?? throw new Exception("获取歌单详情失败!");
    }

    public async Task<Dictionary<string, string>> GetTrySongUrl(Dictionary<string, string> midVs)
    {
        var urls = new Dictionary<string, string>();
        var domain = "https://ws.stream.qqmusic.qq.com/";
        var req = new
        {
            module = "music.vkey.GetVkey",
            method = "UrlGetVkey",
            param = new
            {
                filename = midVs.Select(s => $"RS02{s.Value}.mp3"),
                songmid = midVs.Select(s => s.Key),
                guid = Guid.NewGuid().ToString("N"),
                songtype = midVs.Select(_ => 0),
            }
        };

        var res = await Send(req);
        var array = res.Req.Data?["midurlinfo"]?.Value<JArray>();
        if (array is not null)
        {
            foreach (var item in array)
            {
                var songmid = item["songmid"]!.Value<string>()!;
                var url = item["wifiurl"]!.Value<string>()!;
                urls[songmid] = domain + url;
            }
        }
        return urls;
    }


    public async Task<List<SongData>> QuerySongData(params string[] mid)
    {
        var req = new
        {
            module = "music.trackInfo.UniformRuleCtrl",
            method = "CgiGetTrackInfo",
            param = new
            {
                ids = new List<string>(),
                mids = mid,
                types = mid.Select(_ => 0),
                modify_stamp = mid.Select(_ => 0),
                client = 1,
                ctx = 0
            }
        };
        var res = (await Send(req)).Req.Data?.ToObject<QuerySongData>() ?? throw new Exception("歌曲信息获取失败!");
        return res.Tracks;
    }

    public async Task<Dictionary<string, SongData>> GetSongData(SongFileType type, params string[] mid)
    {
        var domain = "https://ws.stream.qqmusic.qq.com/";
        var songs = await QuerySongData(mid);
        var songDic = songs.ToDictionary(i => i.Mid, i => i);
        var req = new
        {
            module = "music.vkey.GetVkey",
            method = "UrlGetVkey",
            param = new
            {
                filename = mid.Select(s => $"{type.GetSongFormat()}{type}{type.GetSongExtension()}"),
                songmid = mid,
                guid = Guid.NewGuid().ToString("N"),
                songtype = mid.Select(_ => 0),
            }
        };
        var res = await Send(req);
        var array = res.Req.Data?["midurlinfo"]?.Value<JArray>();
        var trysong = new List<string>();
        if (array is not null)
        {
            foreach (var item in array)
            {
                var songmid = item["songmid"]!.Value<string>()!;
                var url = item["wifiurl"]!.Value<string>()!;
                if (string.IsNullOrEmpty(url))
                {
                    trysong.Add(songmid);
                    continue;
                }
                songDic[songmid].PlayUrl = domain + url;
            }
        }
        if (trysong.Count > 0)
        {
            var trysongs = songDic.Where(i => trysong.Contains(i.Key));
            var tryUrls = await GetTrySongUrl(trysongs.ToDictionary(i => i.Key, i => i.Value.Vs[0]));
            foreach (var (key, value) in tryUrls)
            {
                songDic[key].PlayUrl = value;
            }
        }
        return songDic;
    }

    public async Task<SongData> GetSong(string musicName, int index)
    {
        var data = await SearchSong(musicName);
        if (index < 1 || index > data.Count)
            index = 1;
        return (await GetSongData(SongFileType.MP3_128, data[index - 1].Mid)).First().Value;
    }
}