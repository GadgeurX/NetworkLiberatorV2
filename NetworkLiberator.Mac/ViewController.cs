using System;
using System.Net.NetworkInformation;
using System.Threading;
using AppKit;
using Foundation;
using NetworkLiberator.Core;

namespace NetworkLiberator
{
	public partial class ViewController : NSViewController
	{
		private Thread m_UpdateThread;

		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			HostTableDataSource l_DataSource = new HostTableDataSource(NetworkLiberator.Core.Core.Instance.Hosts);
			HostTable.DataSource = l_DataSource;
			HostTable.Delegate = new HostTableDelegate(l_DataSource);
			m_UpdateThread = new Thread(UpdateTable);
			m_UpdateThread.Start();
		}

		partial void onAttackClicked(NSObject sender)
		{
			Core.Core.Instance.ArpAttackStatut = (AttackSwitch.SelectedSegment == 1);
		}

		partial void onTableSelection(NSObject sender)
		{
			for (var i = 0; i < HostTable.DataSource.GetRowCount(HostTable); i++)
				((HostTableDataSource)HostTable.DataSource).Hosts[i].IsSelected = HostTable.IsRowSelected(i);
		}

		partial void onRefresh(NSObject sender)
		{
			int l_PendingThread = Core.Core.Instance.PendingScanThread;
			if (l_PendingThread != 0)
			{
				new NSAlert()
				{
					AlertStyle = NSAlertStyle.Critical,
					InformativeText = l_PendingThread + " replies left",
					MessageText = "Scan not finished",
				}.RunModal();
			}
			else
				Core.Core.Instance.ScanRefresh();
		}

		private void UpdateTable()
		{
			while (true)
			{
				InvokeOnMainThread(() =>
				{
					if (Core.Core.Instance.PendingScanThread > 0)
						RefreshButton.Title = Core.Core.Instance.PendingScanThread + " replies left";
					else
						RefreshButton.Title = "Refresh";
					if (HostTable.SelectedRowCount == 0)
						HostTable.ReloadData();
				});
				Thread.Sleep(2000);
			}
		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
			}
		}
	}
}
