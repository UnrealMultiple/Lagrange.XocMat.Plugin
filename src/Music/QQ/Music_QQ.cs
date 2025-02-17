using Lagrange.XocMat.Extensions;
using Music.QQ.Enums;
using Music.QQ.Internal;
using Music.QQ.Internal.MusicToken;
using Music.QQ.Internal.Playlists;
using Music.QQ.Internal.QuerSong;
using Music.QQ.Internal.Search;
using Music.QQ.Internal.Search.Song;
using Music.QQ.Internal.User;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Music.QQ;

public class Music_QQ : IDisposable
{
    internal const string QQMusicApi = "https://u.y.qq.com/cgi-bin/musicu.fcg";

    private readonly CookieContainer Cookie;

    private readonly HttpClientHandler httpClientHandler;

    private readonly HttpClient client;

    private TokenInfo Token;

    private System.Timers.Timer timer;

    public event Action<TokenInfo>? TokenUpdated;

    public Music_QQ(TokenInfo token)
    {
        Cookie = new();
        foreach (var item in token.Cookie)
        {
            Cookie.Add(new Cookie(item.Key, item.Value, "/", "u.y.qq.com"));
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
        timer = new System.Timers.Timer
        {
            AutoReset = true,
            Interval = 60 * 10 * 1000,
            Enabled = true
        };
        timer.Elapsed += Timer_Elapsed;
        timer.Start();
    }

    private async void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        if (Token.KeyExpiresIn < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - Token.MusickeyCreateTime)
        {
            var (token, success) = await RefreshToken();
            if (success)
            {
                ChangeToken(token);
            }
        }
    }

    public void ChangeToken(TokenInfo token)
    { 
        Token = token;
        TokenUpdated?.Invoke(token);
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

    public async Task<(TokenInfo, bool)> RefreshToken()
    {
        var req = new
        {
            module = "QQConnectLogin.LoginServer",
            method = "QQLogin",
            ignore_code = 1,
            param = new
            {
                refresh_key = Token.RefreshKey,
                refresh_token = Token.RefreshToken,
                musickey = Token.Musickey,
                musicid = Token.Musicid,

            },
            extra_common = new
            {
                tmeLoginType = Token.LoginType.ToString()
            }
        };
        var response = await Send(req);
        var token = response.Req.Data?.ToObject<TokenInfo>() ?? throw new Exception("token刷新失败!");
        var cookie = Cookie.GetCookies(new Uri(QQMusicApi)).ToDictionary(c => c.Name, c => c.Value);
        token.Cookie = cookie;
        return (response.Req.Code == 0 ? token : Token, response.Req.Code == 0);
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
        var content = new StringContent(param.ToJson());
        var res = await client.PostAsync(QQMusicApi, content);
        var json = await res.Content.ReadAsStringAsync();
        return json.ToObject<Response>() ?? throw new Exception("获取返回数据失败!!");
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
        //分割mid
        var SpitMid = list.Keys.ToArray().Chunk(100).Select(s => s.ToList()).ToList();
        //将url全部保存
        var songurls = new Dictionary<string, string>();
        foreach (var item in SpitMid)
        {
            var url = await GetSongData(SongFileType.MP3_128, [.. item]);
            songurls = songurls.Concat(url.ToDictionary(i => i.Mid, i => i.PlayUrl)).ToDictionary(k => k.Key, v => v.Value);
        }
        SemaphoreSlim SemaphoreSlim = new(7);
        var tasks = list.Select(async i =>
        {
            await SemaphoreSlim.WaitAsync();
            try
            {
                var url = songurls[i.Key];
                var buffer = await client.GetByteArrayAsync(url);
                return (i.Value.Name + ".mp3", buffer);
            }
            catch
            {
                return (i.Value.Name + ".mp3", Array.Empty<byte>());
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        });
        var taskres = await Task.WhenAll(tasks);
        return Utils.GenerateCompressed(taskres.ToDictionary(i => i.Item1, i => i.Item2));
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

    public async Task<List<SongData>> GetSongData(SongFileType type, params string[] mid)
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
                filename = mid.Select(s => $"{type.GetSongFormat()}{type}{type.GetSongExtension()}").Concat(songs.Select(s => $"RS02{s.Vs[0]}.mp3")),
                songmid = mid.Concat(songs.Select(s => s.Mid)),
                guid = Guid.NewGuid().ToString("N"),
                songtype = mid.Select(_ => 0).Concat(songs.Select(s => 0)),
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
                if (string.IsNullOrEmpty(url))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(songDic[songmid].PlayUrl))
                    songDic[songmid].PlayUrl = domain + url;
            }
        }
        return [.. songDic.Values];
    }

    public async Task<SongData> GetSong(string musicName, int index)
    {
        var data = await SearchSong(musicName);
        if (index < 1 || index > data.Count)
            index = 1;
        return (await GetSongData(SongFileType.MP3_128, data[index - 1].Mid)).First();
    }

    public void Dispose()
    {
        timer.Stop();
        timer.Dispose();
        client.Dispose();
        httpClientHandler.Dispose();
        GC.SuppressFinalize(this);
    }
}