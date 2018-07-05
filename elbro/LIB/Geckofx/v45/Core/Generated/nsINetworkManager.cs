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
// Generated by IDLImporter from file nsINetworkManager.idl
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
    /// Manage network interfaces.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("e5ffe335-078e-4b25-87f1-02429bd2e458")]
	public interface nsINetworkManager
	{
		
		/// <summary>
        /// Register the given network interface with the network manager.
        ///
        /// Consumers will be notified with the 'network-interface-registered'
        /// observer notification.
        ///
        /// Throws if there's already an interface registered with the same network id.
        ///
        /// @param network
        /// Network interface to register.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RegisterNetworkInterface([MarshalAs(UnmanagedType.Interface)] nsINetworkInterface network);
		
		/// <summary>
        /// Update the routes and DNSes according the state of the given network.
        ///
        /// Consumers will be notified with the 'network-connection-state-changed'
        /// observer notification.
        ///
        /// Throws an exception if the specified network interface object isn't
        /// registered.
        ///
        /// @param network
        /// Network interface to update.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UpdateNetworkInterface([MarshalAs(UnmanagedType.Interface)] nsINetworkInterface network);
		
		/// <summary>
        /// Unregister the given network interface from the network manager.
        ///
        /// Consumers will be notified with the 'network-interface-unregistered'
        /// observer notification.
        ///
        /// Throws an exception if the specified network interface object isn't
        /// registered.
        ///
        /// @param network
        /// Network interface to unregister.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnregisterNetworkInterface([MarshalAs(UnmanagedType.Interface)] nsINetworkInterface network);
		
		/// <summary>
        /// Object containing all known network information, keyed by their
        /// network id. Network id is composed of a sub-id + '-' + network
        /// type. For mobile network types, sub-id is 'ril' + service id; for
        /// non-mobile network types, sub-id is always 'device'.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		Gecko.JsVal GetAllNetworkInfoAttribute();
		
		/// <summary>
        /// The preferred network type. One of the
        /// nsINetworkInterface::NETWORK_TYPE_* constants.
        ///
        /// This attribute is used for setting default route to favor
        /// interfaces with given type.  This can be overriden by calling
        /// overrideDefaultRoute().
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetPreferredNetworkTypeAttribute();
		
		/// <summary>
        /// The preferred network type. One of the
        /// nsINetworkInterface::NETWORK_TYPE_* constants.
        ///
        /// This attribute is used for setting default route to favor
        /// interfaces with given type.  This can be overriden by calling
        /// overrideDefaultRoute().
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPreferredNetworkTypeAttribute(int aPreferredNetworkType);
		
		/// <summary>
        /// The network information of the network interface handling all network
        /// traffic.
        ///
        /// When this changes, the 'network-active-changed' observer
        /// notification is dispatched.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsINetworkInfo GetActiveNetworkInfoAttribute();
		
		/// <summary>
        /// Override the default behaviour for preferredNetworkType and route
        /// all network traffic through the the specified interface.
        ///
        /// Consumers can observe changes to the active network by subscribing to
        /// the 'network-active-changed' observer notification.
        ///
        /// @param network
        /// Network to route all network traffic to. If this is null,
        /// a previous override is canceled.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int OverrideActive([MarshalAs(UnmanagedType.Interface)] nsINetworkInterface network);
		
		/// <summary>
        /// Add host route to the specified network into routing table.
        ///
        /// @param network
        /// The network information for the host to be routed to.
        /// @param host
        /// The host to be added.
        /// The host will be resolved in advance if it's not an ip-address.
        ///
        /// @return a Promise
        /// resolved if added; rejected, otherwise.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		Gecko.JsVal AddHostRoute([MarshalAs(UnmanagedType.Interface)] nsINetworkInfo network, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase host);
		
		/// <summary>
        /// Remove host route to the specified network from routing table.
        ///
        /// @param network
        /// The network information for the routing to be removed from.
        /// @param host
        /// The host routed to the network.
        /// The host will be resolved in advance if it's not an ip-address.
        ///
        /// @return a Promise
        /// resolved if removed; rejected, otherwise.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		Gecko.JsVal RemoveHostRoute([MarshalAs(UnmanagedType.Interface)] nsINetworkInfo network, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase host);
	}
}
