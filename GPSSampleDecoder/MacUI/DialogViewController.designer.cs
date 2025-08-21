// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace GPSSampleDecoder.MacUI
{
	[Register ("DialogViewController")]
	partial class DialogViewController
	{
		[Outlet]
		AppKit.NSTextField message { get; set; }

		[Outlet]
		AppKit.NSButton okButton { get; set; }

		[Outlet]
		AppKit.NSTextField title { get; set; }

		[Action ("AcceptDialog:")]
		partial void AcceptDialog (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (title != null) {
				title.Dispose ();
				title = null;
			}

			if (message != null) {
				message.Dispose ();
				message = null;
			}

			if (okButton != null) {
				okButton.Dispose ();
				okButton = null;
			}
		}
	}
}
