// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NetworkLiberator
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSSegmentedControl AttackSwitch { get; set; }

		[Outlet]
		AppKit.NSTableView HostTable { get; set; }

		[Outlet]
		AppKit.NSTableColumn IpColumn { get; set; }

		[Outlet]
		AppKit.NSTableColumn MacColumn { get; set; }

		[Outlet]
		AppKit.NSButtonCell RefreshButton { get; set; }

		[Action ("onAttackClicked:")]
		partial void onAttackClicked (Foundation.NSObject sender);

		[Action ("onRefresh:")]
		partial void onRefresh (Foundation.NSObject sender);

		[Action ("onTableSelection:")]
		partial void onTableSelection (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AttackSwitch != null) {
				AttackSwitch.Dispose ();
				AttackSwitch = null;
			}

			if (HostTable != null) {
				HostTable.Dispose ();
				HostTable = null;
			}

			if (IpColumn != null) {
				IpColumn.Dispose ();
				IpColumn = null;
			}

			if (MacColumn != null) {
				MacColumn.Dispose ();
				MacColumn = null;
			}

			if (RefreshButton != null) {
				RefreshButton.Dispose ();
				RefreshButton = null;
			}
		}
	}
}
