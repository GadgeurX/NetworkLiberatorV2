using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using LukeSkywalker.IPNetwork;
using SharpPcap;

namespace NetworkLiberator.Core
{
	public static class NetworkUtils
	{
		/*NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(AddressChangedCallback);*/

		public static List<string> GetAllIps()
		{
			List<string> l_Ips = new List<string>();
			IPNetwork ipn = IPNetwork.Parse(GetLocalAddr() + "/" + GetCidr(GetNetmask()));
			LukeSkywalker.IPNetwork.IPAddressCollection ips = IPNetwork.ListIPAddress(ipn);

			foreach (IPAddress ip in ips)
			{
				l_Ips.Add(ip.ToString());
			}
			return l_Ips;
		}

		public static IPAddress GetLocalAddr()
		{
			foreach (UnicastIPAddressInformation unicastIPAddressInformation in GetInterface().GetIPProperties().UnicastAddresses)
			{
				if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
				{
					return unicastIPAddressInformation.Address;
				}
			}
			return null;
		}

		public static IPAddress GetGatewayAddr()
		{
			foreach (var l_Ips in GetInterface().GetIPProperties().GatewayAddresses)
			{
				if (l_Ips.Address.AddressFamily == AddressFamily.InterNetwork)
				{
					return l_Ips.Address;
				}
			}
			return null;
		}

		public static PhysicalAddress GetLocalMac()
		{
			return GetInterface().GetPhysicalAddress();
		}

		public static IPAddress GetNetmask()
		{
			foreach (UnicastIPAddressInformation unicastIPAddressInformation in GetInterface().GetIPProperties().UnicastAddresses)
			{
				if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
				{
					return unicastIPAddressInformation.IPv4Mask;
				}
			}
			return null;
		}

		public static string GetCidr(IPAddress p_Netmask)
		{
			long l_NetmaskL = IpToLong(p_Netmask);

			int count = 0;

			while (l_NetmaskL != 0)
			{
				if ((l_NetmaskL & 0x1) == 0x1) count++;
				l_NetmaskL >>= 1;
			}
			return count.ToString();
		}

		public static long IpToLong(IPAddress p_Ip)
		{
			Byte[] l_IpB = p_Ip.GetAddressBytes();
			long l_Addr = 0;
			for (var i = 0; i <= 3; i++)
			{
				l_Addr = l_Addr | ((long)(l_IpB[i]) << (3 - i) * 8);
			}
			return l_Addr;
		}

		public static string GetInterfaceName()
		{
			foreach (NetworkInterface interf in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (interf.GetIPProperties().GatewayAddresses.Count > 0 && interf.NetworkInterfaceType != NetworkInterfaceType.Unknown && interf.NetworkInterfaceType != NetworkInterfaceType.Loopback)
					return interf.Name;
			}
			throw new Exception("No connected to the net");
		}

		public static NetworkInterface GetInterface()
		{
			foreach (NetworkInterface interf in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (interf.GetIPProperties().GatewayAddresses.Count > 0 && interf.NetworkInterfaceType != NetworkInterfaceType.Unknown && interf.NetworkInterfaceType != NetworkInterfaceType.Loopback)
					return interf;
			}
			throw new Exception("No connected to the net");
		}

		public static PhysicalAddress GetMacFromIp(IPAddress p_Ip)
		{
			PhysicalAddress l_Mac = null;
			lock (LibPcapHandler.Instance.lockObj)
			{
				ARP l_Arp = new ARP(LibPcapHandler.Instance.Device);
				l_Mac = l_Arp.Resolve(p_Ip, GetLocalAddr(), GetLocalMac());
			}
			return l_Mac;
		}
	}
}
