// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace GPSSampleDecoder
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton configurationBrowseButton { get; set; }

		[Outlet]
		AppKit.NSTextField configurationPath { get; set; }

		[Outlet]
		AppKit.NSButton decodeButton { get; set; }

		[Outlet]
		AppKit.NSProgressIndicator decodeIndicator { get; set; }

		[Outlet]
		AppKit.NSTextField description { get; set; }

		[Outlet]
		AppKit.NSTextField errorMsg { get; set; }

		[Outlet]
		AppKit.NSButton outputBrowseButton { get; set; }

		[Outlet]
		AppKit.NSTextField outputDescription { get; set; }

		[Outlet]
		AppKit.NSTextField outputPath { get; set; }

		[Outlet]
		AppKit.NSSecureTextField passcode { get; set; }

		[Outlet]
		AppKit.NSButton saveButton { get; set; }

		[Outlet]
		AppKit.NSButton saveCSVButton { get; set; }

		[Outlet]
		AppKit.NSButton saveExcelButton { get; set; }

		[Outlet]
		AppKit.NSProgressIndicator saveIndicator { get; set; }

		[Outlet]
		AppKit.NSTextField theTitle { get; set; }

		[Outlet]
		AppKit.NSImageView titleImage { get; set; }

		[Action ("configurationBrowseClicked:")]
		partial void configurationBrowseClicked (Foundation.NSObject sender);

		[Action ("csvChecked:")]
		partial void csvChecked (Foundation.NSObject sender);

		[Action ("decodeClicked:")]
		partial void decodeClicked (Foundation.NSObject sender);

		[Action ("outputBrowseClicked:")]
		partial void outputBrowseClicked (Foundation.NSObject sender);

		[Action ("saveClicked:")]
		partial void saveClicked (Foundation.NSObject sender);

		[Action ("xlsChecked:")]
		partial void xlsChecked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (configurationBrowseButton != null) {
				configurationBrowseButton.Dispose ();
				configurationBrowseButton = null;
			}

			if (configurationPath != null) {
				configurationPath.Dispose ();
				configurationPath = null;
			}

			if (decodeButton != null) {
				decodeButton.Dispose ();
				decodeButton = null;
			}

			if (decodeIndicator != null) {
				decodeIndicator.Dispose ();
				decodeIndicator = null;
			}

			if (description != null) {
				description.Dispose ();
				description = null;
			}

			if (errorMsg != null) {
				errorMsg.Dispose ();
				errorMsg = null;
			}

			if (outputBrowseButton != null) {
				outputBrowseButton.Dispose ();
				outputBrowseButton = null;
			}

			if (outputDescription != null) {
				outputDescription.Dispose ();
				outputDescription = null;
			}

			if (outputPath != null) {
				outputPath.Dispose ();
				outputPath = null;
			}

			if (saveButton != null) {
				saveButton.Dispose ();
				saveButton = null;
			}

			if (saveCSVButton != null) {
				saveCSVButton.Dispose ();
				saveCSVButton = null;
			}

			if (saveExcelButton != null) {
				saveExcelButton.Dispose ();
				saveExcelButton = null;
			}

			if (saveIndicator != null) {
				saveIndicator.Dispose ();
				saveIndicator = null;
			}

			if (theTitle != null) {
				theTitle.Dispose ();
				theTitle = null;
			}

			if (titleImage != null) {
				titleImage.Dispose ();
				titleImage = null;
			}

			if (passcode != null) {
				passcode.Dispose ();
				passcode = null;
			}
		}
	}
}
