using System.Net.NetworkInformation;
using Ran.Core.Utils.Logging;

namespace Ran.Core.Utils.HardwareInfos;

/// <summary>
/// 网卡信息帮助类
/// </summary>
public static class NetworkHelper
{
    /// <summary>
    /// 网卡信息
    /// </summary>
    public static List<NetworkInfo> NetworkInfos => GetNetworkInfos();

    /// <summary>
    /// 获取网卡信息
    /// </summary>
    /// <returns></returns>
    private static List<NetworkInfo> GetNetworkInfos()
    {
        List<NetworkInfo> networkInfos = [];

        try
        {
            // 获取可用网卡
            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up)
                .Where(ni =>
                    ni.NetworkInterfaceType is NetworkInterfaceType.Ethernet or NetworkInterfaceType.Wireless80211)
                .ToList();

            networkInfos.AddRange(from ni in interfaces
                let properties = ni.GetIPProperties()
                select new NetworkInfo
                {
                    Name = ni.Name,
                    Description = ni.Description,
                    Type = ni.NetworkInterfaceType.ToString(),
                    Speed = ni.Speed.ToString("#,##0") + " bps",
                    PhysicalAddress = BitConverter.ToString(ni.GetPhysicalAddress().GetAddressBytes()),
                    DnsAddresses = properties.DnsAddresses.Select(ip => ip.ToString()).ToList(),
                    IpAddresses = properties.UnicastAddresses.Select(ip => ip.Address + " / " + ip.IPv4Mask)
                        .ToList()
                });
        }
        catch (Exception ex)
        {
            LogHelper.Error("获取网卡信息出错，" + ex.Message);
        }

        return networkInfos;
    }
}

/// <summary>
/// 网卡信息
/// </summary>
public record NetworkInfo
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 速度
    /// </summary>
    public string Speed { get; set; } = string.Empty;

    /// <summary>
    /// 物理地址(mac 地址)
    /// </summary>
    public string PhysicalAddress { get; set; } = string.Empty;

    /// <summary>
    /// DNS 地址
    /// </summary>
    public List<string> DnsAddresses { get; set; } = [];

    /// <summary>
    /// IP 地址
    /// </summary>
    public List<string> IpAddresses { get; set; } = [];
}
