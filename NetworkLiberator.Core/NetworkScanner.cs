using System;
using System.Net.NetworkInformation;
using System.Threading;

namespace NetworkLiberator.Core
{
	public class NetworkScanner
	{
		private HostManager m_HostManager;
		private Thread m_MainThread;
		private bool m_IsScanning = false;
		private static int s_PendingThread = 0;
		private static readonly object lockObj = new object();

		public NetworkScanner(HostManager p_HostManager)
		{
			m_HostManager = p_HostManager;
		}

		public bool Start()
		{
			lock (lockObj)
			{
				if (s_PendingThread != 0)
					return false;
			}
			m_IsScanning = false;
			if (m_MainThread != null && m_MainThread.IsAlive)
				m_MainThread.Join();
			m_MainThread = new Thread(Run);
			m_IsScanning = true;
			m_MainThread.Start();
			return false;
		}

		private void Run()
		{
			for (var i = 0; i < NetworkUtils.GetAllIps().Count && m_IsScanning; i++)
				ThreadPool.QueueUserWorkItem(PingIp, new object[] { NetworkUtils.GetAllIps()[i], m_HostManager });
		}

		private static void PingIp(Object p_Object)
		{
			lock (lockObj)
				s_PendingThread++;
			object[] l_Params = p_Object as object[];
			String l_Ip = (String)l_Params[0];
			HostManager l_HostManager = (HostManager)l_Params[1];
			PingReply rep = new Ping().Send(l_Ip);
			if (rep.Status == IPStatus.Success)
				l_HostManager.AddHost(l_Ip);
			lock (lockObj)
				s_PendingThread--;
		}

		public int PendingThread
		{
			get { return s_PendingThread; }
		}
	}
}
