using System;
using System.Collections.Generic;

namespace NetworkLiberator.Core
{
	public class HostManager
	{
		private ConcurrentList<Host> m_Hosts = new ConcurrentList<Host>();

		public HostManager()
		{
		}

		public IList<Host> Hosts
		{
			get { return m_Hosts; }
		}

		public void Refresh()
		{
			for (var i = 0; i < Hosts.Count; i++)
			{
				while (i < Hosts.Count && Hosts[i].Mac.Equals("..."))
					Hosts.RemoveAt(i);
			}
		}

		public void AddHost(string p_Ip)
		{
			for (var i = 0; i < Hosts.Count; i++)
				if (p_Ip.Equals(Hosts[i].Ip))
					return;
			Host l_Host = new Host(p_Ip);
			m_Hosts.Add(l_Host);
		}
	}
}
