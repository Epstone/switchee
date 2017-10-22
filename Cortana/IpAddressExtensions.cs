using System;
using System.Linq;
using System.Net;
using Windows.Networking;
using Windows.Networking.Connectivity;

static internal class IpAddressExtensions
{
    public static IPAddress GetSubnetMask(IPAddress hostAddress)
    {
        var addressBytes = hostAddress.GetAddressBytes();
        if (addressBytes[0] >= 1 && addressBytes[0] <= 126)
            return IPAddress.Parse("255.0.0.0");
        else if (addressBytes[0] >= 128 && addressBytes[0] <= 191)
            return IPAddress.Parse("255.255.255.0");
        else if (addressBytes[0] >= 192 && addressBytes[0] <= 223)
            return IPAddress.Parse("255.255.255.0");
        else
            throw new ArgumentOutOfRangeException();
    }

    public static IPAddress GetBroadastAddress(IPAddress hostIPAddress)
    {
        var subnetAddress = GetSubnetMask(hostIPAddress);
        var deviceAddressBytes = hostIPAddress.GetAddressBytes();
        var subnetAddressBytes = subnetAddress.GetAddressBytes();
        if (deviceAddressBytes.Length != subnetAddressBytes.Length)
            throw new ArgumentOutOfRangeException();
        var broadcastAddressBytes = new byte[deviceAddressBytes.Length];
        for (var i = 0; i < broadcastAddressBytes.Length; i++)
            broadcastAddressBytes[i] = (byte)(deviceAddressBytes[i] | subnetAddressBytes[i] ^ 255);
        return new IPAddress(broadcastAddressBytes);
    }

    public static IPAddress GetLocalIp(HostNameType hostNameType)
    {
        var internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

        if (internetConnectionProfile?.NetworkAdapter == null) return null;
        var hostname =
            NetworkInformation.GetHostNames()
                .FirstOrDefault(
                    hostName =>
                        hostName.Type == hostNameType &&
                        hostName.IPInformation?.NetworkAdapter != null &&
                        hostName.IPInformation.NetworkAdapter.NetworkAdapterId == internetConnectionProfile.NetworkAdapter.NetworkAdapterId);

        // the ip address
        return IPAddress.Parse(hostname?.CanonicalName);
    }
}