using System;
using System.Collections.Generic;
using PacketDotNet;
using SharpPcap.LibPcap;

namespace NetworkLiberator.Core
{
	public class LibPcapHandler
	{
		private string m_DeviceName;
		private LibPcapLiveDevice m_Device;
		private static LibPcapHandler m_Instance;
		public readonly object lockObj = new object();

		public LibPcapHandler()
		{
			m_Instance = this;
			m_DeviceName = NetworkUtils.GetInterfaceName();
			LibPcapLiveDeviceList devices = LibPcapLiveDeviceList.Instance;
			foreach (LibPcapLiveDevice dev in devices)
			{
				if (dev.Name.Equals(m_DeviceName))
					m_Device = dev;
			}
			if (m_Device == null)
				throw new Exception("No Device found");
			m_Device.Open();
			m_Device.Close();
		}

		public void SendPacket(IList<Packet> p_Packets)
		{
			lock(lockObj)
			{
				m_Device.Open();
				foreach (Packet l_Packet in p_Packets)
				{
					if (l_Packet != null)
						m_Device.SendPacket(l_Packet);
				}
				m_Device.Close();
			}
		}

		public LibPcapLiveDevice Device
		{
			get { return m_Device; }
		}

		public static LibPcapHandler Instance
		{
			get { return m_Instance; }
		}
	}
}
