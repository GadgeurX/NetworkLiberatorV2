using System;
using System.Net;
using System.Threading;
using NetworkLiberator.Core;
using PacketDotNet;

namespace NetworkLiberator
{
	public class Host
	{
		#region Computed Propoperties
		private string m_Ip = "";
		private string m_Mac = "...";
		private bool m_IsSelected = false;
		private Packet m_ArpPacket = null;
		private Thread m_UpdateThread;
		#endregion

		#region Constructors
		public Host(string p_Ip)
		{
			this.m_Ip = p_Ip;
			m_UpdateThread = new Thread(Run);
			m_UpdateThread.Start();
		}
		#endregion

		private void Run()
		{
			var l_Mac = NetworkUtils.GetMacFromIp(IPAddress.Parse(m_Ip));
			if (l_Mac != null)
			{
				m_Mac = l_Mac.ToString();
				var l_ArpPacket = new ARPPacket(ARPOperation.Response, l_Mac, IPAddress.Parse(m_Ip), NetworkUtils.GetLocalMac(), NetworkUtils.GetGatewayAddr());
				m_ArpPacket = new EthernetPacket(NetworkUtils.GetLocalMac(), l_Mac, EthernetPacketType.Arp);
				m_ArpPacket.PayloadPacket = l_ArpPacket;
			}
		}

		public Packet ArpPacket
		{
			get { return m_ArpPacket; }
		}

		public string Ip
		{
			get { return m_Ip; }
			set { m_Ip = value; }
		}

		public string Mac
		{
			get { return m_Mac; }
			set { m_Mac = value; }
		}

		public bool IsSelected
		{
			get { return m_IsSelected; }
			set { m_IsSelected = value; }
		}
	}
}
