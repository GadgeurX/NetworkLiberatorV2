using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AppKit;

namespace NetworkLiberator
{
	public class HostTableDataSource : NSTableViewDataSource
	{
		#region Public Variables
		private IList<Host> m_Hosts;
		#endregion

		#region Constructors
		public HostTableDataSource(IList<Host> p_Hosts)
		{
			m_Hosts = p_Hosts;
		}
		#endregion

		#region Override Methods
		public override nint GetRowCount(NSTableView tableView)
		{
			return m_Hosts.Count;
		}
		#endregion

		public IList<Host> Hosts
		{
			get { return m_Hosts; }
		}
	}
}
