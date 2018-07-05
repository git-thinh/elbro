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
// Generated by IDLImporter from file nsIWindowProvider.idl
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
    /// The nsIWindowProvider interface exists so that the window watcher's default
    /// behavior of opening a new window can be easly modified.  When the window
    /// watcher needs to open a new window, it will first check with the
    /// nsIWindowProvider it gets from the parent window.  If there is no provider
    /// or the provider does not provide a window, the window watcher will proceed
    /// to actually open a new window.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("f607bd66-08e5-4d2e-ad83-9f9f3ca17658")]
	public interface nsIWindowProvider
	{
		
		/// <summary>
        /// A method to request that this provider provide a window.  The window
        /// returned need not to have the right name or parent set on it; setting
        /// those is the caller's responsibility.  The provider can always return null
        /// to have the caller create a brand-new window.
        ///
        /// @param aParent Must not be null.  This is the window that the caller wants
        /// to use as the parent for the new window.  Generally,
        /// nsIWindowProvider implementors can expect to be somehow related to
        /// aParent; the relationship may depend on the nsIWindowProvider
        /// implementation.
        ///
        /// @param aChromeFlags The chrome flags the caller will use to create a new
        /// window if this provider returns null.  See nsIWebBrowserChrome for
        /// the possible values of this field.
        ///
        /// @param aPositionSpecified Whether the attempt to create a window is trying
        /// to specify a position for the new window.
        ///
        /// @param aSizeSpecified Whether the attempt to create a window is trying to
        /// specify a size for the new window.
        ///
        /// @param aURI The URI to be loaded in the new window (may be NULL).  The
        /// nsIWindowProvider implementation must not load this URI into the
        /// window it returns.  This URI is provided solely to help the
        /// nsIWindowProvider implementation make decisions; the caller will
        /// handle loading the URI in the window returned if provideWindow
        /// returns a window.
        ///
        /// When making decisions based on aURI, note that even when it's not
        /// null, aURI may not represent all relevant information about the
        /// load.  For example, the load may have extra load flags, POST data,
        /// etc.
        ///
        /// @param aName The name of the window being opened.  Setting the name on the
        /// return value of provideWindow will be handled by the caller; aName
        /// is provided solely to help the nsIWindowProvider implementation
        /// make decisions.
        ///
        /// @param aFeatures The feature string for the window being opened.  This may
        /// be empty.  The nsIWindowProvider implementation is allowed to apply
        /// the feature string to the window it returns in any way it sees fit.
        /// See the nsIWindowWatcher interface for details on feature strings.
        ///
        /// @param aWindowIsNew [out] Whether the window being returned was just
        /// created by the window provider implementation.  This can be used by
        /// callers to keep track of which windows were opened by the user as
        /// opposed to being opened programmatically.  This should be set to
        /// false if the window being returned existed before the
        /// provideWindow() call.  The value of this out parameter is
        /// meaningless if provideWindow() returns null.
        /// @return A window the caller should use or null if the caller should just
        /// create a new window.  The returned window may be newly opened by
        /// the nsIWindowProvider implementation or may be a window that
        /// already existed.
        ///
        /// @throw NS_ERROR_ABORT if the caller should cease its attempt to open a new
        /// window.
        ///
        /// @see nsIWindowWatcher for more information on aFeatures.
        /// @see nsIWebBrowserChrome for more information on aChromeFlags.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMWindow ProvideWindow([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow aParent, uint aChromeFlags, [MarshalAs(UnmanagedType.U1)] bool aCalledFromJS, [MarshalAs(UnmanagedType.U1)] bool aPositionSpecified, [MarshalAs(UnmanagedType.U1)] bool aSizeSpecified, [MarshalAs(UnmanagedType.Interface)] nsIURI aURI, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aName, [MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aFeatures, [MarshalAs(UnmanagedType.U1)] ref bool aWindowIsNew);
	}
}
