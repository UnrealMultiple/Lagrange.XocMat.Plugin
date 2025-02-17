#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。


namespace Music.WangYi;

using Newtonsoft.Json;
using System;

public partial class PlaylistsData
{
    [JsonProperty("code")]
    public long Code { get; set; }

    [JsonProperty("relatedVideos")]

    public object RelatedVideos { get; set; }

    [JsonProperty("playlist")]
    public Playlist Playlist { get; set; }

    [JsonProperty("urls")]
    public object Urls { get; set; }

    [JsonProperty("privileges")]
    public Privilege[] Privileges { get; set; }

    [JsonProperty("sharedPrivilege")]
    public object SharedPrivilege { get; set; }

    [JsonProperty("resEntrance")]
    public object ResEntrance { get; set; }

    [JsonProperty("fromUsers")]
    public object FromUsers { get; set; }

    [JsonProperty("fromUserCount")]
    public long FromUserCount { get; set; }

    [JsonProperty("songFromUsers")]
    public object SongFromUsers { get; set; }
}

public partial class Playlist
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("coverImgId")]
    public double CoverImgId { get; set; }

    [JsonProperty("coverImgUrl")]
    public Uri CoverImgUrl { get; set; }

    [JsonProperty("coverImgId_str")]
    public string CoverImgIdStr { get; set; }

    [JsonProperty("adType")]
    public long AdType { get; set; }

    [JsonProperty("userId")]
    public long UserId { get; set; }

    [JsonProperty("createTime")]
    public long CreateTime { get; set; }

    [JsonProperty("status")]
    public long Status { get; set; }

    [JsonProperty("opRecommend")]
    public bool OpRecommend { get; set; }

    [JsonProperty("highQuality")]
    public bool HighQuality { get; set; }

    [JsonProperty("newImported")]
    public bool NewImported { get; set; }

    [JsonProperty("updateTime")]
    public long UpdateTime { get; set; }

    [JsonProperty("trackCount")]
    public long TrackCount { get; set; }

    [JsonProperty("specialType")]
    public long SpecialType { get; set; }

    [JsonProperty("privacy")]
    public long Privacy { get; set; }

    [JsonProperty("trackUpdateTime")]
    public long TrackUpdateTime { get; set; }

    [JsonProperty("commentThreadId")]
    public string CommentThreadId { get; set; }

    [JsonProperty("playCount")]
    public long PlayCount { get; set; }

    [JsonProperty("trackNumberUpdateTime")]
    public long TrackNumberUpdateTime { get; set; }

    [JsonProperty("subscribedCount")]
    public long SubscribedCount { get; set; }

    [JsonProperty("cloudTrackCount")]
    public long CloudTrackCount { get; set; }

    [JsonProperty("ordered")]
    public bool Ordered { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("tags")]
    public object[] Tags { get; set; }

    [JsonProperty("updateFrequency")]
    public string UpdateFrequency { get; set; }

    [JsonProperty("backgroundCoverId")]
    public double BackgroundCoverId { get; set; }

    [JsonProperty("backgroundCoverUrl")]
    public Uri BackgroundCoverUrl { get; set; }

    [JsonProperty("titleImage")]
    public double TitleImage { get; set; }

    [JsonProperty("titleImageUrl")]
    public Uri TitleImageUrl { get; set; }

    [JsonProperty("detailPageTitle")]
    public string DetailPageTitle { get; set; }

    [JsonProperty("englishTitle")]
    public string EnglishTitle { get; set; }

    [JsonProperty("officialPlaylistType")]
    public string OfficialPlaylistType { get; set; }

    [JsonProperty("copied")]
    public bool Copied { get; set; }

    [JsonProperty("relateResType")]
    public string RelateResType { get; set; }

    [JsonProperty("coverStatus")]
    public long CoverStatus { get; set; }

    [JsonProperty("subscribers")]
    public object[] Subscribers { get; set; }

    [JsonProperty("subscribed")]
    public bool Subscribed { get; set; }

    [JsonProperty("creator")]
    public Creator Creator { get; set; }

    [JsonProperty("tracks")]
    public Track[] Tracks { get; set; }

    [JsonProperty("videoIds")]
    public object VideoIds { get; set; }

    [JsonProperty("videos")]
    public object Videos { get; set; }

    [JsonProperty("trackIds")]
    public TrackId[] TrackIds { get; set; }

    [JsonProperty("bannedTrackIds")]
    public object BannedTrackIds { get; set; }

    [JsonProperty("mvResourceInfos")]
    public object MvResourceInfos { get; set; }

    [JsonProperty("shareCount")]
    public long ShareCount { get; set; }

    [JsonProperty("commentCount")]
    public long CommentCount { get; set; }

    [JsonProperty("remixVideo")]
    public object RemixVideo { get; set; }

    [JsonProperty("newDetailPageRemixVideo")]
    public object NewDetailPageRemixVideo { get; set; }

    [JsonProperty("sharedUsers")]
    public object SharedUsers { get; set; }

    [JsonProperty("historySharedUsers")]
    public object HistorySharedUsers { get; set; }

    [JsonProperty("gradeStatus")]
    public string GradeStatus { get; set; }

    [JsonProperty("score")]
    public string Score { get; set; }

    [JsonProperty("algTags")]
    public object AlgTags { get; set; }

    [JsonProperty("distributeTags")]
    public DistributeTag[] DistributeTags { get; set; }

    [JsonProperty("trialMode")]
    public long TrialMode { get; set; }

    [JsonProperty("displayTags")]
    public object DisplayTags { get; set; }

    [JsonProperty("displayUserInfoAsTagOnly")]
    public bool DisplayUserInfoAsTagOnly { get; set; }

    [JsonProperty("playlistType")]
    public string PlaylistType { get; set; }

    [JsonProperty("bizExtInfo")]
    public BizExtInfo BizExtInfo { get; set; }
}

public partial class BizExtInfo
{
}

public partial class Creator
{
    [JsonProperty("defaultAvatar")]
    public bool DefaultAvatar { get; set; }

    [JsonProperty("province")]
    public long Province { get; set; }

    [JsonProperty("authStatus")]
    public long AuthStatus { get; set; }

    [JsonProperty("followed")]
    public bool Followed { get; set; }

    [JsonProperty("avatarUrl")]
    public Uri AvatarUrl { get; set; }

    [JsonProperty("accountStatus")]
    public long AccountStatus { get; set; }

    [JsonProperty("gender")]
    public long Gender { get; set; }

    [JsonProperty("city")]
    public long City { get; set; }

    [JsonProperty("birthday")]
    public long Birthday { get; set; }

    [JsonProperty("userId")]
    public long UserId { get; set; }

    [JsonProperty("userType")]
    public long UserType { get; set; }

    [JsonProperty("nickname")]
    public string Nickname { get; set; }

    [JsonProperty("signature")]
    public string Signature { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("detailDescription")]
    public string DetailDescription { get; set; }

    [JsonProperty("avatarImgId")]
    public double AvatarImgId { get; set; }

    [JsonProperty("backgroundImgId")]
    public double BackgroundImgId { get; set; }

    [JsonProperty("backgroundUrl")]
    public Uri BackgroundUrl { get; set; }

    [JsonProperty("authority")]
    public long Authority { get; set; }

    [JsonProperty("mutual")]
    public bool Mutual { get; set; }

    [JsonProperty("expertTags")]
    public object ExpertTags { get; set; }

    [JsonProperty("experts")]
    public object Experts { get; set; }

    [JsonProperty("djStatus")]
    public long DjStatus { get; set; }

    [JsonProperty("vipType")]
    public long VipType { get; set; }

    [JsonProperty("remarkName")]
    public object RemarkName { get; set; }

    [JsonProperty("authenticationTypes")]
    public long AuthenticationTypes { get; set; }

    [JsonProperty("avatarDetail")]
    public AvatarDetail AvatarDetail { get; set; }

    [JsonProperty("backgroundImgIdStr")]
    public string BackgroundImgIdStr { get; set; }

    [JsonProperty("avatarImgIdStr")]
    public string AvatarImgIdStr { get; set; }

    [JsonProperty("anchor")]
    public bool Anchor { get; set; }

    [JsonProperty("avatarImgId_str")]
    public string CreatorAvatarImgIdStr { get; set; }
}

public partial class AvatarDetail
{
    [JsonProperty("userType")]
    public long UserType { get; set; }

    [JsonProperty("identityLevel")]
    public long IdentityLevel { get; set; }

    [JsonProperty("identityIconUrl")]
    public Uri IdentityIconUrl { get; set; }
}

public partial class DistributeTag
{
    [JsonProperty("tagName")]
    public string TagName { get; set; }

    [JsonProperty("algTagCategory")]
    public string AlgTagCategory { get; set; }

    [JsonProperty("tagLink")]
    public string TagLink { get; set; }
}

public partial class TrackId
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("v")]
    public long V { get; set; }

    [JsonProperty("t")]
    public long T { get; set; }

    [JsonProperty("at")]
    public long At { get; set; }

    [JsonProperty("alg")]
    public object Alg { get; set; }

    [JsonProperty("uid")]
    public long Uid { get; set; }

    [JsonProperty("rcmdReason")]
    public string RcmdReason { get; set; }

    [JsonProperty("sc")]
    public object Sc { get; set; }

    [JsonProperty("f")]
    public object F { get; set; }

    [JsonProperty("sr")]
    public object Sr { get; set; }

    [JsonProperty("dpr")]
    public object Dpr { get; set; }
}

public partial class Track
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("pst")]
    public long Pst { get; set; }

    [JsonProperty("t")]
    public long T { get; set; }

    [JsonProperty("ar")]
    public Ar[] Ar { get; set; }

    [JsonProperty("alia")]
    public string[] Alia { get; set; }

    [JsonProperty("pop")]
    public long Pop { get; set; }

    [JsonProperty("st")]
    public long St { get; set; }

    [JsonProperty("rt")]
    public string Rt { get; set; }

    [JsonProperty("fee")]
    public long Fee { get; set; }

    [JsonProperty("v")]
    public long V { get; set; }

    [JsonProperty("crbt")]
    public object Crbt { get; set; }

    [JsonProperty("cf")]
    public string Cf { get; set; }

    [JsonProperty("al")]
    public Al Al { get; set; }

    [JsonProperty("dt")]
    public long Dt { get; set; }

    [JsonProperty("h")]
    public H H { get; set; }

    [JsonProperty("m")]
    public H M { get; set; }

    [JsonProperty("l")]
    public H L { get; set; }

    [JsonProperty("sq")]
    public H Sq { get; set; }

    [JsonProperty("hr")]
    public object Hr { get; set; }

    [JsonProperty("a")]
    public object A { get; set; }

    [JsonProperty("cd")]
    public string Cd { get; set; }

    [JsonProperty("no")]
    public long No { get; set; }

    [JsonProperty("rtUrl")]
    public object RtUrl { get; set; }

    [JsonProperty("ftype")]
    public long Ftype { get; set; }

    [JsonProperty("rtUrls")]
    public object[] RtUrls { get; set; }

    [JsonProperty("djId")]
    public long DjId { get; set; }

    [JsonProperty("copyright")]
    public long Copyright { get; set; }

    [JsonProperty("s_id")]
    public long SId { get; set; }

    [JsonProperty("mark")]
    public long Mark { get; set; }

    [JsonProperty("originCoverType")]
    public long OriginCoverType { get; set; }

    [JsonProperty("originSongSimpleData")]
    public object OriginSongSimpleData { get; set; }

    [JsonProperty("tagPicList")]
    public object TagPicList { get; set; }

    [JsonProperty("resourceState")]
    public bool ResourceState { get; set; }

    [JsonProperty("version")]
    public long Version { get; set; }

    [JsonProperty("songJumpInfo")]
    public object SongJumpInfo { get; set; }

    [JsonProperty("entertainmentTags")]
    public object EntertainmentTags { get; set; }

    [JsonProperty("awardTags")]
    public object AwardTags { get; set; }

    [JsonProperty("single")]
    public long Single { get; set; }

    [JsonProperty("noCopyrightRcmd")]
    public object NoCopyrightRcmd { get; set; }

    [JsonProperty("alg")]
    public object Alg { get; set; }

    [JsonProperty("displayReason")]
    public object DisplayReason { get; set; }

    [JsonProperty("rtype")]
    public long Rtype { get; set; }

    [JsonProperty("rurl")]
    public object Rurl { get; set; }

    [JsonProperty("mst")]
    public long Mst { get; set; }

    [JsonProperty("cp")]
    public long Cp { get; set; }

    [JsonProperty("mv")]
    public long Mv { get; set; }

    [JsonProperty("publishTime")]
    public long PublishTime { get; set; }

    [JsonProperty("videoInfo")]
    public VideoInfo VideoInfo { get; set; }
}

public partial class Al
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("picUrl")]
    public Uri PicUrl { get; set; }

    [JsonProperty("tns")]
    public object[] Tns { get; set; }

    [JsonProperty("pic_str")]
    public string PicStr { get; set; }

    [JsonProperty("pic")]
    public double Pic { get; set; }
}

public partial class Ar
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("tns")]
    public object[] Tns { get; set; }

    [JsonProperty("alias")]
    public object[] Alias { get; set; }
}

public partial class H
{
    [JsonProperty("br")]
    public long Br { get; set; }

    [JsonProperty("fid")]
    public long Fid { get; set; }

    [JsonProperty("size")]
    public long Size { get; set; }

    [JsonProperty("vd")]
    public long Vd { get; set; }
}

public partial class VideoInfo
{
    [JsonProperty("moreThanOne")]
    public bool MoreThanOne { get; set; }

    [JsonProperty("video")]
    public Video Video { get; set; }
}

public partial class Video
{
    [JsonProperty("vid")]
    public string Vid { get; set; }

    [JsonProperty("type")]
    public long Type { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("playTime")]
    public long PlayTime { get; set; }

    [JsonProperty("coverUrl")]
    public Uri CoverUrl { get; set; }

    [JsonProperty("publishTime")]
    public long PublishTime { get; set; }

    [JsonProperty("artists")]
    public object Artists { get; set; }

    [JsonProperty("alias")]
    public object Alias { get; set; }
}

public partial class Privilege
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("fee")]
    public long Fee { get; set; }

    [JsonProperty("payed")]
    public long Payed { get; set; }

    [JsonProperty("realPayed")]
    public long RealPayed { get; set; }

    [JsonProperty("st")]
    public long St { get; set; }

    [JsonProperty("pl")]
    public long Pl { get; set; }

    [JsonProperty("dl")]
    public long Dl { get; set; }

    [JsonProperty("sp")]
    public long Sp { get; set; }

    [JsonProperty("cp")]
    public long Cp { get; set; }

    [JsonProperty("subp")]
    public long Subp { get; set; }

    [JsonProperty("cs")]
    public bool Cs { get; set; }

    [JsonProperty("maxbr")]
    public long Maxbr { get; set; }

    [JsonProperty("fl")]
    public long Fl { get; set; }

    [JsonProperty("pc")]
    public object Pc { get; set; }

    [JsonProperty("toast")]
    public bool Toast { get; set; }

    [JsonProperty("flag")]
    public long Flag { get; set; }

    [JsonProperty("paidBigBang")]
    public bool PaidBigBang { get; set; }

    [JsonProperty("preSell")]
    public bool PreSell { get; set; }

    [JsonProperty("playMaxbr")]
    public long PlayMaxbr { get; set; }

    [JsonProperty("downloadMaxbr")]
    public long DownloadMaxbr { get; set; }

    [JsonProperty("maxBrLevel")]
    public string MaxBrLevel { get; set; }

    [JsonProperty("playMaxBrLevel")]
    public string PlayMaxBrLevel { get; set; }

    [JsonProperty("downloadMaxBrLevel")]
    public string DownloadMaxBrLevel { get; set; }

    [JsonProperty("plLevel")]
    public string PlLevel { get; set; }

    [JsonProperty("dlLevel")]
    public string DlLevel { get; set; }

    [JsonProperty("flLevel")]
    public string FlLevel { get; set; }

    [JsonProperty("rscl")]
    public object Rscl { get; set; }

    [JsonProperty("freeTrialPrivilege")]
    public FreeTrialPrivilege FreeTrialPrivilege { get; set; }

    [JsonProperty("rightSource")]
    public long RightSource { get; set; }

    [JsonProperty("chargeInfoList")]
    public ChargeInfoList[] ChargeInfoList { get; set; }

    [JsonProperty("code")]
    public long Code { get; set; }

    [JsonProperty("message")]
    public object Message { get; set; }
}

public partial class ChargeInfoList
{
    [JsonProperty("rate")]
    public long Rate { get; set; }

    [JsonProperty("chargeUrl")]
    public object ChargeUrl { get; set; }

    [JsonProperty("chargeMessage")]
    public object ChargeMessage { get; set; }

    [JsonProperty("chargeType")]
    public long ChargeType { get; set; }
}

public partial class FreeTrialPrivilege
{
    [JsonProperty("resConsumable")]
    public bool ResConsumable { get; set; }

    [JsonProperty("userConsumable")]
    public bool UserConsumable { get; set; }

    [JsonProperty("listenType")]
    public object ListenType { get; set; }

    [JsonProperty("cannotListenReason")]
    public object CannotListenReason { get; set; }

    [JsonProperty("playReason")]
    public object PlayReason { get; set; }

    [JsonProperty("freeLimitTagType")]
    public object FreeLimitTagType { get; set; }
}




