using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography;

namespace Music.QQ.Internal;
public class Device
{
    private static Random random = new();

    [JsonProperty("display")]
    public string Display { get; set; } = $"QMAPI.{random.Next(100000, 999999)}.001";

    [JsonProperty("product")]
    public string Product { get; set; } = "iarim";

    [JsonProperty("device")]
    public string DeviceModel { get; set; } = "sagit";

    [JsonProperty("board")]
    public string Board { get; set; } = "eomam";

    [JsonProperty("model")]
    public string Model { get; set; } = "MI 6";

    [JsonProperty("fingerprint")]
    public string Fingerprint { get; set; } =
        $"xiaomi/iarim/sagit:10/eomam.200122.001/{random.Next(1000000, 9999999)}:user/release-keys";

    [JsonProperty("boot_id")]
    public string BootId { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("proc_version")]
    public string ProcVersion { get; set; } =
        $"Linux 5.4.0-54-generic-{GenerateRandomString(8)} (android-build@google.com)";

    [JsonProperty("imei")]
    public string Imei { get; set; } = GenerateRandomImei();

    [JsonProperty("brand")]
    public string Brand { get; set; } = "Xiaomi";

    [JsonProperty("bootloader")]
    public string Bootloader { get; set; } = "U-boot";

    [JsonProperty("base_band")]
    public string BaseBand { get; set; } = "";

    [JsonProperty("version")]
    public OSVersion Version { get; set; } = new OSVersion();

    [JsonProperty("sim_info")]
    public string SimInfo { get; set; } = "T-Mobile";

    [JsonProperty("os_type")]
    public string OsType { get; set; } = "android";

    [JsonProperty("mac_address")]
    public string MacAddress { get; set; } = "00:50:56:C0:00:08";

    [JsonProperty("ip_address")]
    public List<int> IpAddress { get; } = new List<int> { 10, 0, 1, 3 };

    [JsonProperty("wifi_bssid")]
    public string WifiBssid { get; set; } = "00:50:56:C0:00:08";

    [JsonProperty("wifi_ssid")]
    public string WifiSsid { get; set; } = "<unknown ssid>";

    [JsonProperty("imsi_md5")]
    public List<byte> ImsiMd5 { get; set; } = GenerateRandomMd5().ToList();

    [JsonProperty("android_id")]
    public string AndroidId { get; set; } = BitConverter.ToString(GenerateRandomBytes(8)).Replace("-", "").ToLower();

    [JsonProperty("apn")]
    public string Apn { get; set; } = "wifi";

    [JsonProperty("vendor_name")]
    public string VendorName { get; set; } = "MIUI";

    [JsonProperty("vendor_os_name")]
    public string VendorOsName { get; set; } = "qmapi";

    [JsonProperty("qimei")]
    public QimeiService.QimeiResult Qimei { get; set; } = new QimeiService.QimeiResult();


    private static string GenerateRandomString(int length) =>
        new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".OrderBy(s => Guid.NewGuid()).Take(length).ToArray());

    private static byte[] GenerateRandomBytes(int count)
    {
        var bytes = new byte[count];
        RandomNumberGenerator.Fill(bytes);
        return bytes;
    }

    private static byte[] GenerateRandomMd5()
    {
        return MD5.HashData(GenerateRandomBytes(16));
    }

    public static string GenerateRandomImei()
    {
        int[] imei = new int[14];
        int sum = 0;

        for (int i = 0; i < 14; i++)
        {
            imei[i] = random.Next(0, 10);
            if ((i + 2) % 2 == 0)
            {
                int doubled = imei[i] * 2;
                imei[i] = doubled >= 10 ? doubled - 9 : doubled;
            }
            sum += imei[i];
        }

        int checkDigit = (10 - sum % 10) % 10;
        return string.Concat(imei) + checkDigit;
    }
}




public class OSVersion
{
    private static Random random = new Random();

    [JsonProperty("incremental")]
    public string Incremental { get; set; } = $"QMAPI{random.Next(100000, 999999)}.001";

    [JsonProperty("codename")]
    public string Codename { get; set; } = "REL";

    [JsonProperty("release")]
    public string Release { get; set; } = "10";

    [JsonProperty("sdk")]
    public int Sdk { get; set; } = 29;
}
