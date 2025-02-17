#pragma warning disable CS8618   // Naming Styles
namespace Music.QQ.Internal.User;

using Newtonsoft.Json;
using System;
public partial class HomePageData
{
    [JsonProperty("Info")]
    public DataInfo Info { get; set; }

    [JsonProperty("Status")]
    public long Status { get; set; }

    [JsonProperty("Prompt")]
    public Prompt Prompt { get; set; }

    [JsonProperty("TabDetail")]
    public TabDetail TabDetail { get; set; }
}

public partial class DataInfo
{
    [JsonProperty("BaseInfo")]
    public BaseInfo BaseInfo { get; set; }

    [JsonProperty("Singer")]
    public Singer Singer { get; set; }

    [JsonProperty("Pet")]
    public Pet Pet { get; set; }

    [JsonProperty("Putoo")]
    public Putoo Putoo { get; set; }

    [JsonProperty("SuperSubscription")]
    public SuperSubscription SuperSubscription { get; set; }

    [JsonProperty("Certificate")]
    public Certificate Certificate { get; set; }

    [JsonProperty("WxVideoChannel")]
    public WxVideoChannel WxVideoChannel { get; set; }

    [JsonProperty("Icons")]
    public InfoElement[] Icons { get; set; }

    [JsonProperty("Share")]
    public Share Share { get; set; }

    [JsonProperty("VisitorNum")]
    public Num VisitorNum { get; set; }

    [JsonProperty("FriendsNum")]
    public Num FriendsNum { get; set; }

    [JsonProperty("FansNum")]
    public Num FansNum { get; set; }

    [JsonProperty("FollowNum")]
    public Num FollowNum { get; set; }

    [JsonProperty("IsFollowed")]
    public long IsFollowed { get; set; }

    [JsonProperty("Setting")]
    public Setting Setting { get; set; }

    [JsonProperty("NewIconInfo")]
    public NewIconInfo NewIconInfo { get; set; }

    [JsonProperty("ButtonList")]
    public object ButtonList { get; set; }

    [JsonProperty("MusicWorldEntry")]
    public MusicWorldEntry MusicWorldEntry { get; set; }

    [JsonProperty("IP")]
    public Ip Ip { get; set; }

    [JsonProperty("UrgeUpdate")]
    public Black UrgeUpdate { get; set; }

    [JsonProperty("MedalList")]
    public MedalList[] MedalList { get; set; }

    [JsonProperty("Constellation")]
    public Constellation Constellation { get; set; }

    [JsonProperty("Gender")]
    public Gender Gender { get; set; }

    [JsonProperty("BgScheme")]
    public string BgScheme { get; set; }

    [JsonProperty("NumButtonList")]
    public object NumButtonList { get; set; }

    [JsonProperty("Black")]
    public Black Black { get; set; }

    [JsonProperty("CertificateList")]
    public object[] CertificateList { get; set; }

    [JsonProperty("BGInfoList")]
    public object BgInfoList { get; set; }

    [JsonProperty("DzNum")]
    public Num DzNum { get; set; }
}

public partial class BaseInfo
{
    [JsonProperty("IsHost")]
    public long IsHost { get; set; }

    [JsonProperty("IsSinger")]
    public long IsSinger { get; set; }

    [JsonProperty("EncryptedUin")]
    public string EncryptedUin { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("Avatar")]
    public Uri Avatar { get; set; }

    [JsonProperty("BackgroundImage")]
    public Uri BackgroundImage { get; set; }

    [JsonProperty("BackgroundImageType")]
    public long BackgroundImageType { get; set; }

    [JsonProperty("Pendant")]
    public Pendant Pendant { get; set; }

    [JsonProperty("BigAvatar")]
    public Uri BigAvatar { get; set; }

    [JsonProperty("UserType")]
    public long UserType { get; set; }

    [JsonProperty("BgImgExt")]
    public BgImgExt BgImgExt { get; set; }
}

public partial class BgImgExt
{
    [JsonProperty("PSCoverList")]
    public object[] PsCoverList { get; set; }

    [JsonProperty("ModuleID")]
    public string ModuleId { get; set; }

    [JsonProperty("RecordWallScheme")]
    public string RecordWallScheme { get; set; }

    [JsonProperty("RecordWallStyle")]
    public string RecordWallStyle { get; set; }

    [JsonProperty("RecordWallBlurBg")]
    public string RecordWallBlurBg { get; set; }

    [JsonProperty("RecordWallFrom")]
    public long RecordWallFrom { get; set; }
}

public partial class Pendant
{
    [JsonProperty("HasEntry")]
    public long HasEntry { get; set; }

    [JsonProperty("DynamicImg")]
    public Uri DynamicImg { get; set; }

    [JsonProperty("StaticImg")]
    public Uri StaticImg { get; set; }

    [JsonProperty("PromptText")]
    public string PromptText { get; set; }

    [JsonProperty("Scheme")]
    public Uri Scheme { get; set; }

    [JsonProperty("ID")]
    public long Id { get; set; }

    [JsonProperty("IsDefaultImg")]
    public long IsDefaultImg { get; set; }
}

public partial class Black
{
    [JsonProperty("HasEntry")]
    public long HasEntry { get; set; }

    [JsonProperty("Status")]
    public long Status { get; set; }
}

public partial class Certificate
{
    [JsonProperty("HasEntry")]
    public long HasEntry { get; set; }

    [JsonProperty("Info")]
    public InfoElement Info { get; set; }

    [JsonProperty("ArrowIcon")]
    public string ArrowIcon { get; set; }
}

public partial class InfoElement
{
    [JsonProperty("Title")]
    public string Title { get; set; }

    [JsonProperty("IconURL")]
    public string IconUrl { get; set; }

    [JsonProperty("Jump")]
    public Jump Jump { get; set; }

    [JsonProperty("CarouselList")]
    public object CarouselList { get; set; }

    [JsonProperty("CarouselDur")]
    public long CarouselDur { get; set; }
}

public partial class Jump
{
    [JsonProperty("JumpType")]
    public long JumpType { get; set; }

    [JsonProperty("JumpURL")]
    public string JumpUrl { get; set; }

    [JsonProperty("IsNeedLogin")]
    public long IsNeedLogin { get; set; }
}

public partial class Constellation
{
    [JsonProperty("Constellation")]
    public string ConstellationConstellation { get; set; }
}

public partial class Num
{
    [JsonProperty("HasEntry")]
    public long HasEntry { get; set; }

    [JsonProperty("Num")]
    public long NumNum { get; set; }

    [JsonProperty("Add")]
    public string Add { get; set; }
}

public partial class Gender
{
    [JsonProperty("Gender")]
    public string GenderGender { get; set; }
}

public partial class Ip
{
    [JsonProperty("Location")]
    public string Location { get; set; }
}

public partial class MedalList
{
    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("Icon")]
    public Uri Icon { get; set; }

    [JsonProperty("Scheme")]
    public Uri Scheme { get; set; }

    [JsonProperty("Type")]
    public long Type { get; set; }

    [JsonProperty("Level")]
    public long Level { get; set; }

    [JsonProperty("MedalID")]
    public string MedalId { get; set; }
}

public partial class MusicWorldEntry
{
    [JsonProperty("HasEntry")]
    public long HasEntry { get; set; }

    [JsonProperty("BackgroundImage")]
    public string BackgroundImage { get; set; }

    [JsonProperty("BackgroundImageType")]
    public long BackgroundImageType { get; set; }

    [JsonProperty("MusicWorldBtn")]
    public MusicWorldBtn MusicWorldBtn { get; set; }
}

public partial class MusicWorldBtn
{
    [JsonProperty("ButtonType")]
    public long ButtonType { get; set; }

    [JsonProperty("ButtonStyle")]
    public long ButtonStyle { get; set; }

    [JsonProperty("ButtonInfo")]
    public InfoElement ButtonInfo { get; set; }

    [JsonProperty("ArrowIcon")]
    public string ArrowIcon { get; set; }

    [JsonProperty("ExtraInfo")]
    public object ExtraInfo { get; set; }

    [JsonProperty("ExposureID")]
    public long ExposureId { get; set; }

    [JsonProperty("ClickID")]
    public long ClickId { get; set; }
}

public partial class NewIconInfo
{
    [JsonProperty("nickname")]
    public Nickname Nickname { get; set; }

    [JsonProperty("iconlist")]
    public Iconlist[] Iconlist { get; set; }
}

public partial class Iconlist
{
    [JsonProperty("width")]
    public long Width { get; set; }

    [JsonProperty("height")]
    public long Height { get; set; }

    [JsonProperty("srcUrl")]
    public Uri SrcUrl { get; set; }

    [JsonProperty("style")]
    public string Style { get; set; }

    [JsonProperty("ext")]
    public string Ext { get; set; }

    [JsonProperty("desc")]
    public string Desc { get; set; }

    [JsonProperty("Tips")]
    public string Tips { get; set; }

    [JsonProperty("Helptxt")]
    public string Helptxt { get; set; }

    [JsonProperty("Title")]
    public Title Title { get; set; }

    [JsonProperty("GifURL")]
    public string GifUrl { get; set; }

    [JsonProperty("GifTimes")]
    public long GifTimes { get; set; }

    [JsonProperty("GifWidth")]
    public long GifWidth { get; set; }

    [JsonProperty("GifHeight")]
    public long GifHeight { get; set; }

    [JsonProperty("GreyURL")]
    public string GreyUrl { get; set; }

    [JsonProperty("GreyWidth")]
    public long GreyWidth { get; set; }

    [JsonProperty("GreyHeight")]
    public long GreyHeight { get; set; }

    [JsonProperty("GreyLeft")]
    public long GreyLeft { get; set; }

    [JsonProperty("GreyRight")]
    public long GreyRight { get; set; }

    [JsonProperty("GreyText")]
    public string GreyText { get; set; }

    [JsonProperty("Flag")]
    public long Flag { get; set; }

    [JsonProperty("Hash")]
    public long Hash { get; set; }

    [JsonProperty("UIStyle")]
    public long UiStyle { get; set; }

    [JsonProperty("Segment")]
    public Segment Segment { get; set; }

    [JsonProperty("Animation")]
    public string Animation { get; set; }

    [JsonProperty("AmnInfo")]
    public AmnInfo AmnInfo { get; set; }

    [JsonProperty("GreyTextColor")]
    public string GreyTextColor { get; set; }
}

public partial class AmnInfo
{
    [JsonProperty("GifTimes")]
    public long GifTimes { get; set; }

    [JsonProperty("TextColor")]
    public string TextColor { get; set; }

    [JsonProperty("BackgroundURL")]
    public string BackgroundUrl { get; set; }
}

public partial class Segment
{
    [JsonProperty("Width")]
    public long Width { get; set; }

    [JsonProperty("Height")]
    public long Height { get; set; }

    [JsonProperty("DarkIconURL")]
    public string DarkIconUrl { get; set; }

    [JsonProperty("LightIconURL")]
    public string LightIconUrl { get; set; }

    [JsonProperty("StretchLeft")]
    public long StretchLeft { get; set; }

    [JsonProperty("StretchRight")]
    public long StretchRight { get; set; }

    [JsonProperty("Contents")]
    public Content[] Contents { get; set; }

    [JsonProperty("MaxDisplayLen")]
    public long MaxDisplayLen { get; set; }

    [JsonProperty("PaddingLeft")]
    public long PaddingLeft { get; set; }

    [JsonProperty("PaddingRight")]
    public long PaddingRight { get; set; }

    [JsonProperty("JumpURL")]
    public string JumpUrl { get; set; }
}

public partial class Content
{
    [JsonProperty("PrefixPixel")]
    public long PrefixPixel { get; set; }

    [JsonProperty("Content")]
    public string ContentContent { get; set; }

    [JsonProperty("Font")]
    public string Font { get; set; }

    [JsonProperty("FontSize")]
    public long FontSize { get; set; }

    [JsonProperty("FontColor")]
    public string FontColor { get; set; }

    [JsonProperty("MaxDisplayLen")]
    public long MaxDisplayLen { get; set; }
}

public partial class Title
{
    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("color")]
    public string Color { get; set; }
}

public partial class Nickname
{
    [JsonProperty("lightColor")]
    public string LightColor { get; set; }

    [JsonProperty("darkColor")]
    public string DarkColor { get; set; }
}

public partial class Pet
{
    [JsonProperty("HasEntry")]
    public long HasEntry { get; set; }

    [JsonProperty("PetURL")]
    public string PetUrl { get; set; }

    [JsonProperty("PetID")]
    public long PetId { get; set; }
}

public partial class Putoo
{
    [JsonProperty("HasEntry")]
    public long HasEntry { get; set; }

    [JsonProperty("Type")]
    public long Type { get; set; }

    [JsonProperty("Info")]
    public InfoElement Info { get; set; }

    [JsonProperty("ArrowIcon")]
    public string ArrowIcon { get; set; }

    [JsonProperty("ExtraInfo")]
    public object ExtraInfo { get; set; }
}

public partial class Setting
{
    [JsonProperty("HasEntry")]
    public long HasEntry { get; set; }

    [JsonProperty("SetBoot")]
    public SetBoot SetBoot { get; set; }
}

public partial class SetBoot
{
    [JsonProperty("HasEntry")]
    public long HasEntry { get; set; }

    [JsonProperty("ShowTime")]
    public long ShowTime { get; set; }

    [JsonProperty("Title")]
    public string Title { get; set; }
}

public partial class Share
{
    [JsonProperty("JumpURL")]
    public Uri JumpUrl { get; set; }
}

public partial class Singer
{
    [JsonProperty("SingerID")]
    public long SingerId { get; set; }

    [JsonProperty("SingerMid")]
    public string SingerMid { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("SingerPMid")]
    public string SingerPMid { get; set; }

    [JsonProperty("ForeignName")]
    public string ForeignName { get; set; }

    [JsonProperty("SingerType")]
    public long SingerType { get; set; }

    [JsonProperty("IsDead")]
    public long IsDead { get; set; }

    [JsonProperty("SingerPic")]
    public string SingerPic { get; set; }

    [JsonProperty("BgMagicColor")]
    public string BgMagicColor { get; set; }
}

public partial class SuperSubscription
{
    [JsonProperty("HasEntry")]
    public long HasEntry { get; set; }

    [JsonProperty("Type")]
    public long Type { get; set; }

    [JsonProperty("Info")]
    public InfoElement Info { get; set; }
}

public partial class WxVideoChannel
{
    [JsonProperty("HasEntry")]
    public long HasEntry { get; set; }

    [JsonProperty("Info")]
    public InfoElement Info { get; set; }
}

public partial class Prompt
{
    [JsonProperty("Msg")]
    public string Msg { get; set; }

    [JsonProperty("URL")]
    public string Url { get; set; }
}

public partial class TabDetail
{
    [JsonProperty("TabList")]
    public TabList[] TabList { get; set; }

    [JsonProperty("HasMore")]
    public long HasMore { get; set; }

    [JsonProperty("Order")]
    public long Order { get; set; }

    [JsonProperty("TabID")]
    public string TabId { get; set; }

    [JsonProperty("NeedShowTab")]
    public long NeedShowTab { get; set; }

    [JsonProperty("SongTab")]
    public SongTab SongTab { get; set; }

    [JsonProperty("AlbumTab")]
    public AlbumTab AlbumTab { get; set; }

    [JsonProperty("MomentTab")]
    public MomentTab MomentTab { get; set; }

    [JsonProperty("VideoTab")]
    public VideoTab VideoTab { get; set; }

    [JsonProperty("DiscTab")]
    public DiscTabClass DiscTab { get; set; }

    [JsonProperty("IntroductionTab")]
    public DiscTabClass IntroductionTab { get; set; }

    [JsonProperty("ArtistWorksTab")]
    public ArtistWorksTab ArtistWorksTab { get; set; }

    [JsonProperty("PutaoProductTab")]
    public PutaoProductTabClass PutaoProductTab { get; set; }

    [JsonProperty("ShowTab")]
    public PutaoProductTabClass ShowTab { get; set; }
}

public partial class AlbumTab
{
    [JsonProperty("TypeList")]
    public TypeList TypeList { get; set; }

    [JsonProperty("AlbumList")]
    public object AlbumList { get; set; }
}

public partial class TypeList
{
    [JsonProperty("DefaultID")]
    public long DefaultId { get; set; }

    [JsonProperty("ItemList")]
    public object ItemList { get; set; }
}

public partial class ArtistWorksTab
{
    [JsonProperty("PeriodTag")]
    public TypeList PeriodTag { get; set; }

    [JsonProperty("GenreTag")]
    public TypeList GenreTag { get; set; }

    [JsonProperty("WorksList")]
    public object WorksList { get; set; }
}

public partial class DiscTabClass
{
    [JsonProperty("List")]
    public List[] List { get; set; }
}

public partial class List
{
    [JsonProperty("ItemType")]
    public long ItemType { get; set; }

    [JsonProperty("AboutList")]
    public object AboutList { get; set; }

    [JsonProperty("SingerInfoList")]
    public object SingerInfoList { get; set; }

    [JsonProperty("ChoiceSongList")]
    public object ChoiceSongList { get; set; }

    [JsonProperty("ChoiceVideoList")]
    public object ChoiceVideoList { get; set; }

    [JsonProperty("NewestCommentList")]
    public object NewestCommentList { get; set; }

    [JsonProperty("MyMusicList")]
    public MyMusicList[] MyMusicList { get; set; }

    [JsonProperty("SimilarArtistsList")]
    public object SimilarArtistsList { get; set; }

    [JsonProperty("ArtistAchievementList")]
    public object ArtistAchievementList { get; set; }

    [JsonProperty("WormholeList")]
    public WormholeList[] WormholeList { get; set; }

    [JsonProperty("DissList")]
    public DissList[] DissList { get; set; }

    [JsonProperty("AIVenus")]
    public object AiVenus { get; set; }
}

public partial class DissList
{
    [JsonProperty("list")]
    public ListElement[] List { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }
}

public partial class ListElement
{
    [JsonProperty("dissid")]
    public long Dissid { get; set; }

    [JsonProperty("dirid")]
    public long Dirid { get; set; }

    [JsonProperty("picurl")]
    public Uri Picurl { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("subtitle")]
    public string Subtitle { get; set; }

    [JsonProperty("icontype")]
    public long Icontype { get; set; }

    [JsonProperty("iconurl")]
    public string Iconurl { get; set; }

    [JsonProperty("isshow")]
    public long Isshow { get; set; }

    [JsonProperty("dir_show")]
    public long DirShow { get; set; }

    [JsonProperty("layer_url")]
    public string LayerUrl { get; set; }
}

public partial class MyMusicList
{
    [JsonProperty("MyMusic")]
    public MyMusic MyMusic { get; set; }
}

public partial class MyMusic
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("infos")]
    public InfoClass[] Infos { get; set; }

    [JsonProperty("more")]
    public More More { get; set; }
}

public partial class InfoClass
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("picurl")]
    public string Picurl { get; set; }

    [JsonProperty("subtitle")]
    public string Subtitle { get; set; }

    [JsonProperty("jumpurl")]
    public string Jumpurl { get; set; }

    [JsonProperty("type")]
    public long Type { get; set; }

    [JsonProperty("laypic")]
    public Uri Laypic { get; set; }

    [JsonProperty("disslist")]
    public object Disslist { get; set; }
}

public partial class More
{
    [JsonProperty("jumpType")]
    public long JumpType { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }
}

public partial class WormholeList
{
    [JsonProperty("WormholeType")]
    public long WormholeType { get; set; }

    [JsonProperty("MusicGeneList")]
    public MusicGeneList[] MusicGeneList { get; set; }

    [JsonProperty("MusicLibList")]
    public object MusicLibList { get; set; }
}

public partial class MusicGeneList
{
    [JsonProperty("Info")]
    public MusicGeneListInfo Info { get; set; }

    [JsonProperty("ReportData")]
    public ReportData ReportData { get; set; }
}

public partial class MusicGeneListInfo
{
    [JsonProperty("ShowName")]
    public string ShowName { get; set; }

    [JsonProperty("Subtitle")]
    public string Subtitle { get; set; }

    [JsonProperty("MoreJumpUrl")]
    public string MoreJumpUrl { get; set; }
}

public partial class ReportData
{
    [JsonProperty("ListeningReport")]
    public ListeningReport ListeningReport { get; set; }

    [JsonProperty("Singers")]
    public SlowDegrees[] Singers { get; set; }

    [JsonProperty("Ages")]
    public object[] Ages { get; set; }

    [JsonProperty("Genres")]
    public object[] Genres { get; set; }

    [JsonProperty("BPM")]
    public Bpm Bpm { get; set; }

    [JsonProperty("SlowDegrees")]
    public SlowDegrees SlowDegrees { get; set; }

    [JsonProperty("MusicTastes")]
    public object[] MusicTastes { get; set; }

    [JsonProperty("Grooving")]
    public Grooving Grooving { get; set; }

    [JsonProperty("Personality")]
    public Personality Personality { get; set; }

    [JsonProperty("SortArray")]
    public long[] SortArray { get; set; }

    [JsonProperty("IsVisitAccount")]
    public long IsVisitAccount { get; set; }

    [JsonProperty("TimePreference")]
    public SlowDegrees TimePreference { get; set; }
}

public partial class Bpm
{
    [JsonProperty("Base")]
    public Base Base { get; set; }

    [JsonProperty("MinScore")]
    public long MinScore { get; set; }

    [JsonProperty("MaxScore")]
    public long MaxScore { get; set; }

    [JsonProperty("CardBPMActExt")]
    public object[] CardBpmActExt { get; set; }
}

public partial class Base
{
    [JsonProperty("TypeTitle")]
    public string TypeTitle { get; set; }

    [JsonProperty("KeyWord")]
    public string KeyWord { get; set; }

    [JsonProperty("Slogan")]
    public string Slogan { get; set; }

    [JsonProperty("Pic")]
    public string Pic { get; set; }

    [JsonProperty("EnglishName")]
    public string EnglishName { get; set; }

    [JsonProperty("Id")]
    public string Id { get; set; }

    [JsonProperty("ShowType")]
    public long ShowType { get; set; }
}

public partial class Grooving
{
    [JsonProperty("Base")]
    public Base Base { get; set; }

    [JsonProperty("Level")]
    public long Level { get; set; }
}

public partial class ListeningReport
{
    [JsonProperty("Report")]
    public object[] Report { get; set; }

    [JsonProperty("ShowType")]
    public long ShowType { get; set; }

    [JsonProperty("CurrentMonth")]
    public long CurrentMonth { get; set; }
}

public partial class Personality
{
    [JsonProperty("Base")]
    public Base Base { get; set; }

    [JsonProperty("RealMBTI")]
    public Base RealMbti { get; set; }

    [JsonProperty("GuideTxt")]
    public string GuideTxt { get; set; }

    [JsonProperty("GuideScheme")]
    public string GuideScheme { get; set; }
}

public partial class SlowDegrees
{
    [JsonProperty("Base")]
    public Base Base { get; set; }
}

public partial class MomentTab
{
    [JsonProperty("List")]
    public object List { get; set; }

    [JsonProperty("CarouselList")]
    public object CarouselList { get; set; }
}

public partial class PutaoProductTabClass
{
    [JsonProperty("WebViewURL")]
    public string WebViewUrl { get; set; }
}

public partial class SongTab
{
    [JsonProperty("List")]
    public object List { get; set; }

    [JsonProperty("SongTagInfoList")]
    public object SongTagInfoList { get; set; }

    [JsonProperty("SearchText")]
    public string SearchText { get; set; }

    [JsonProperty("IsShowQLIcon")]
    public long IsShowQlIcon { get; set; }
}

public partial class TabList
{
    [JsonProperty("TabGroup")]
    public long TabGroup { get; set; }

    [JsonProperty("TabID")]
    public string TabId { get; set; }

    [JsonProperty("TabName")]
    public string TabName { get; set; }

    [JsonProperty("Count")]
    public long Count { get; set; }

    [JsonProperty("PageSize")]
    public long PageSize { get; set; }

    [JsonProperty("FirstPageMax")]
    public long FirstPageMax { get; set; }

    [JsonProperty("WebViewURL")]
    public string WebViewUrl { get; set; }

    [JsonProperty("extra")]
    public object Extra { get; set; }
}

public partial class VideoTab
{
    [JsonProperty("VideoList")]
    public object VideoList { get; set; }

    [JsonProperty("TagList")]
    public object TagList { get; set; }
}
