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
// Generated by IDLImporter from file nsIDataCallInterfaceService.idl
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
    ///This Source Code Form is subject to the terms of the Mozilla Public
    /// License, v. 2.0. If a copy of the MPL was not distributed with this
    /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6b66446a-7000-438f-8e1b-b56b4cbf4fa9")]
	public interface nsIDataCall
	{
		
		/// <summary>
        /// Data call fail cause. One of the nsIDataCallInterface.DATACALL_FAIL_*
        /// values.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetFailCauseAttribute();
		
		/// <summary>
        /// If failCause != nsIDataCallInterface.DATACALL_FAIL_NONE, this field
        /// indicates the suggested retry back-off timer. The unit is milliseconds.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetSuggestedRetryTimeAttribute();
		
		/// <summary>
        /// Context ID, uniquely identifies this call.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetCidAttribute();
		
		/// <summary>
        /// Data call network state. One of the nsIDataCallInterface.DATACALL_STATE_*
        /// values.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetActiveAttribute();
		
		/// <summary>
        /// Data call connection type. One of the
        /// nsIDataCallInterface.DATACALL_PDP_TYPE_* values.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetPdpTypeAttribute();
		
		/// <summary>
        /// The network interface name.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetIfnameAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aIfname);
		
		/// <summary>
        /// A space-delimited list of addresses with optional "/" prefix length.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAddressesAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aAddresses);
		
		/// <summary>
        /// A space-delimited list of DNS server addresses.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetDnsesAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aDnses);
		
		/// <summary>
        /// A space-delimited list of default gateway addresses.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetGatewaysAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aGateways);
		
		/// <summary>
        /// A space-delimited list of Proxy Call State Control Function addresses for
        /// IMS client.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetPcscfAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aPcscf);
		
		/// <summary>
        /// MTU received from network, -1 if not set or invalid.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetMtuAttribute();
	}
	
	/// <summary>nsIDataCallInterfaceListener </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("e119c54b-9354-4ad6-a1ee-18608bde9320")]
	public interface nsIDataCallInterfaceListener
	{
		
		/// <summary>
        /// Notify data call interface listeners about unsolicited data call state
        /// changes.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyDataCallListChanged(uint count, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] nsIDataCall[] dataCalls);
	}
	
	/// <summary>nsIDataCallCallback </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("db0b640a-3b3a-4f48-84dc-256e176876c2")]
	public interface nsIDataCallCallback
	{
		
		/// <summary>
        /// Called when setupDataCall() returns succesfully.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifySetupDataCallSuccess([MarshalAs(UnmanagedType.Interface)] nsIDataCall dataCall);
		
		/// <summary>
        /// Called when getDataCallList() returns succesfully.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyGetDataCallListSuccess(uint count, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] nsIDataCall[] dataCalls);
		
		/// <summary>
        /// Called when request returns succesfully.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifySuccess();
		
		/// <summary>
        /// Called when request returns error.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyError([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase errorMsg);
	}
	
	/// <summary>nsIDataCallInterface </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ec219021-8623-4b9f-aba5-4db58c60684f")]
	public interface nsIDataCallInterface
	{
		
		/// <summary>
        /// Setup data call.
        ///
        /// @param apn
        /// Apn to connect to.
        /// @param username
        /// Username for apn.
        /// @param password
        /// Password for apn.
        /// @param authType
        /// Authentication type. One of the DATACALL_AUTH_* values.
        /// @param pdpType
        /// Connection type. One of the DATACALL_PDP_TYPE_* values.
        /// @param nsIDataCallCallback
        /// Called when request is finished.
        ///
        /// If successful, the notifySetupDataCallSuccess() will be called with the
        /// new nsIDataCall.
        ///
        /// Otherwise, the notifyError() will be called, and the error will be either
        /// 'RadioNotAvailable', 'OpNotAllowedBeforeRegToNw',
        /// 'OpNotAllowedDuringVoiceCall', 'RequestNotSupported' or 'GenericFailure'.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetupDataCall([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase apn, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase username, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase password, int authType, int pdpType, [MarshalAs(UnmanagedType.Interface)] nsIDataCallCallback callback);
		
		/// <summary>
        /// Deactivate data call.
        ///
        /// @param cid
        /// Context id.
        /// @param reason
        /// Disconnect Reason. One of the DATACALL_DEACTIVATE_* values.
        /// @param nsIDataCallCallback
        /// Called when request is finished.
        ///
        /// If successful, the notifySuccess() will be called.
        ///
        /// Otherwise, the notifyError() will be called, and the error will be either
        /// 'RadioNotAvailable' or 'GenericFailure'.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DeactivateDataCall(int cid, int reason, [MarshalAs(UnmanagedType.Interface)] nsIDataCallCallback callback);
		
		/// <summary>
        /// Get current data call list.
        ///
        /// @param nsIDataCallCallback
        /// Called when request is finished.
        ///
        /// If successful, the notifyGetDataCallListSuccess() will be called with the
        /// list of nsIDataCall(s).
        ///
        /// Otherwise, the notifyError() will be called, and the error will be either
        /// 'RadioNotAvailable' or 'GenericFailure'.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetDataCallList([MarshalAs(UnmanagedType.Interface)] nsIDataCallCallback callback);
		
		/// <summary>
        /// Set data registration state.
        ///
        /// @param attach
        /// whether to attach data registration or not.
        /// @param nsIDataCallCallback
        /// Called when request is finished.
        ///
        /// If successful, the notifySuccess() will be called.
        ///
        /// Otherwise, the notifyError() will be called, and the error will be either
        /// 'RadioNotAvailable', 'SubscriptionNotAvailable' or 'GenericFailure'.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetDataRegistration([MarshalAs(UnmanagedType.U1)] bool attach, [MarshalAs(UnmanagedType.Interface)] nsIDataCallCallback callback);
		
		/// <summary>
        /// Register to receive unsolicited events from this nsIDataCallInterface.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RegisterListener([MarshalAs(UnmanagedType.Interface)] nsIDataCallInterfaceListener listener);
		
		/// <summary>
        /// Unregister to stop receiving unsolicited events from this
        /// nsIDataCallInterface.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnregisterListener([MarshalAs(UnmanagedType.Interface)] nsIDataCallInterfaceListener listener);
	}
	
	/// <summary>nsIDataCallInterfaceConsts </summary>
	public class nsIDataCallInterfaceConsts
	{
		
		// <summary>
        // Data fail causes, defined in TS 24.008.
        // </summary>
		public const long DATACALL_FAIL_NONE = 0;
		
		// 
		public const long DATACALL_FAIL_OPERATOR_BARRED = 0x08;
		
		// 
		public const long DATACALL_FAIL_INSUFFICIENT_RESOURCES = 0x1A;
		
		// 
		public const long DATACALL_FAIL_MISSING_UKNOWN_APN = 0x1B;
		
		// 
		public const long DATACALL_FAIL_UNKNOWN_PDP_ADDRESS_TYPE = 0x1C;
		
		// 
		public const long DATACALL_FAIL_USER_AUTHENTICATION = 0x1D;
		
		// 
		public const long DATACALL_FAIL_ACTIVATION_REJECT_GGSN = 0x1E;
		
		// 
		public const long DATACALL_FAIL_ACTIVATION_REJECT_UNSPECIFIED = 0x1F;
		
		// 
		public const long DATACALL_FAIL_SERVICE_OPTION_NOT_SUPPORTED = 0x20;
		
		// 
		public const long DATACALL_FAIL_SERVICE_OPTION_NOT_SUBSCRIBED = 0x21;
		
		// 
		public const long DATACALL_FAIL_SERVICE_OPTION_OUT_OF_ORDER = 0x22;
		
		// 
		public const long DATACALL_FAIL_NSAPI_IN_USE = 0x23;
		
		// 
		public const long DATACALL_FAIL_ONLY_IPV4_ALLOWED = 0x32;
		
		// 
		public const long DATACALL_FAIL_ONLY_IPV6_ALLOWED = 0x33;
		
		// 
		public const long DATACALL_FAIL_ONLY_SINGLE_BEARER_ALLOWED = 0x34;
		
		// 
		public const long DATACALL_FAIL_PROTOCOL_ERRORS = 0x6F;
		
		// <summary>
        //Not mentioned in the specification </summary>
		public const long DATACALL_FAIL_VOICE_REGISTRATION_FAIL = -1;
		
		// 
		public const long DATACALL_FAIL_DATA_REGISTRATION_FAIL = -2;
		
		// 
		public const long DATACALL_FAIL_SIGNAL_LOST = -3;
		
		// 
		public const long DATACALL_FAIL_PREF_RADIO_TECH_CHANGED = -4;
		
		// 
		public const long DATACALL_FAIL_RADIO_POWER_OFF = -5;
		
		// 
		public const long DATACALL_FAIL_TETHERED_CALL_ACTIVE = -6;
		
		// 
		public const long DATACALL_FAIL_ERROR_UNSPECIFIED = 0xFFFF;
		
		// <summary>
        // Data call network state.
        // </summary>
		public const long DATACALL_STATE_INACTIVE = 0;
		
		// 
		public const long DATACALL_STATE_ACTIVE_DOWN = 1;
		
		// 
		public const long DATACALL_STATE_ACTIVE_UP = 2;
		
		// <summary>
        // Data call authentication type. Must match the values in ril_consts
        // RIL_DATACALL_AUTH_TO_GECKO array.
        // </summary>
		public const long DATACALL_AUTH_NONE = 0;
		
		// 
		public const long DATACALL_AUTH_PAP = 1;
		
		// 
		public const long DATACALL_AUTH_CHAP = 2;
		
		// 
		public const long DATACALL_AUTH_PAP_OR_CHAP = 3;
		
		// <summary>
        // Data call protocol type. Must match the values in ril_consts
        // RIL_DATACALL_PDP_TYPES array.
        // </summary>
		public const long DATACALL_PDP_TYPE_IPV4 = 0;
		
		// 
		public const long DATACALL_PDP_TYPE_IPV4V6 = 1;
		
		// 
		public const long DATACALL_PDP_TYPE_IPV6 = 2;
		
		// <summary>
        // Reason for deactivating data call.
        // </summary>
		public const long DATACALL_DEACTIVATE_NO_REASON = 0;
		
		// 
		public const long DATACALL_DEACTIVATE_RADIO_SHUTDOWN = 1;
	}
	
	/// <summary>nsIDataCallInterfaceService </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("64700406-7429-4743-a6ae-f82e9877fd0d")]
	public interface nsIDataCallInterfaceService
	{
		
		/// <summary>
        /// Get the corresponding data call interface.
        ///
        /// @param clientId
        /// clientId of the data call interface to get.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDataCallInterface GetDataCallInterface(int clientId);
	}
}