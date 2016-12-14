using System;
using System.Collections.Generic;

namespace NetworkLiberator.Core
{
	public class Core
	{
		private static Core m_Instance;
		private HostManager m_HostManager;
		private NetworkScanner m_NetworkScanner;
		private ArpAttack m_ArpAttack;
		private LibPcapHandler m_LibPcapHandler;

		public Core()
		{
			m_Instance = this;
			m_LibPcapHandler = new LibPcapHandler();
			m_HostManager = new HostManager();
			m_NetworkScanner = new NetworkScanner(m_HostManager);
			m_NetworkScanner.Start();
			m_ArpAttack = new ArpAttack(m_HostManager);
			m_ArpAttack.Start();
		}

		public bool ScanRefresh()
		{
			var l_PendingThread = m_NetworkScanner.PendingThread;
			if (l_PendingThread != 0)
				return false;
			m_NetworkScanner.Start();
			m_HostManager.Refresh();
			return true;
		}

		public int PendingScanThread
		{
			get { return m_NetworkScanner.PendingThread; }
		}

		public bool ArpAttackStatut
		{
			get { return m_ArpAttack.IsAttacking; }
			set { m_ArpAttack.IsAttacking = value; }
		}

		public static Core Instance
		{
			get { return m_Instance; }
		}

		public IList<Host> Hosts
		{
			get { return m_HostManager.Hosts; }
		}
	}
}
