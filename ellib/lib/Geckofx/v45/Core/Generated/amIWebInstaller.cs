// --------------------------------------------------------------------------------------------
// Version: MPL 1.1/GPL 2.0/LGPL 2.1
// 
// The contents of this file are subject to the Mozilla Public License Version
// 1.1 (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
// for the specific language governing rights and limitations under the
// License.
// 
// <remarks>
// Generated by IDLImporter from file amIWebInstaller.idl
// 
// You should use these interfaces when you access the COM objects defined in the mentioned
// IDL/IDH file.
// </remarks>
// --------------------------------------------------------------------------------------------
namespace Gecko
{
	using System;
	using System.Runtime.InteropServices;
	using System.Runtime.InteropServices.ComTypes;
	using System.Runtime.CompilerServices;
	
	
	/// <summary>
    /// A callback function used to notify webpages when a requested install has
    /// ended.
    ///
    /// NOTE: This is *not* the same as InstallListener.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("bb22f5c0-3ca1-48f6-873c-54e87987700f")]
	public interface amIInstallCallback
	{
		
		/// <summary>
        /// Called when an install completes or fails.
        ///
        /// @param  aUrl
        /// The url of the add-on being installed
        /// @param  aStatus
        /// 0 if the install was successful or negative if not
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnInstallEnded([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aUrl, int aStatus);
	}
	
	/// <summary>
    /// This interface is used to allow webpages to start installing add-ons.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("658d6c09-15e0-4688-bee8-8551030472a9")]
	public interface amIWebInstaller
	{
		
		/// <summary>
        /// Checks if installation is enabled for a webpage.
        ///
        /// @param  aMimetype
        /// The mimetype for the add-on to be installed
        /// @param  referer
        /// The URL of the webpage trying to install an add-on
        /// @return true if installation is enabled
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsInstallEnabled([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aMimetype, [MarshalAs(UnmanagedType.Interface)] nsIURI aReferer);
		
		/// <summary>
        /// Installs an array of add-ons at the request of a webpage
        ///
        /// @param  aMimetype
        /// The mimetype for the add-ons
        /// @param  aBrowser
        /// The browser installing the add-ons.
        /// @param  aReferer
        /// The URI for the webpage installing the add-ons
        /// @param  aUris
        /// The URIs of add-ons to be installed
        /// @param  aHashes
        /// The hashes for the add-ons to be installed
        /// @param  aNames
        /// The names for the add-ons to be installed
        /// @param  aIcons
        /// The icons for the add-ons to be installed
        /// @param  aCallback
        /// An optional callback to notify about installation success and
        /// failure
        /// @param  aInstallCount
        /// An optional argument including the number of add-ons to install
        /// @return true if the installation was successfully started
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool InstallAddonsFromWebpage([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aMimetype, [MarshalAs(UnmanagedType.Interface)] nsIDOMElement aBrowser, [MarshalAs(UnmanagedType.Interface)] nsIURI aReferer, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=8)] System.IntPtr[] aUris, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=8)] System.IntPtr[] aHashes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=8)] System.IntPtr[] aNames, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=8)] System.IntPtr[] aIcons, amIInstallCallback aCallback, uint aInstallCount);
	}
}
