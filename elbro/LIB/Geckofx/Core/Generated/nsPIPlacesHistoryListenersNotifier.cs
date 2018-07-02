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
// Generated by IDLImporter from file nsPIPlacesHistoryListenersNotifier.idl
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
    /// This is a private interface used by Places components to notify history
    /// listeners about important notifications.  These should not be used by any
    /// code that is not part of core.
    ///
    /// @note See also: nsINavHistoryObserver
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("808cf36c-4c9a-4bdb-91a4-d60a6fc25add")]
	public interface nsPIPlacesHistoryListenersNotifier
	{
		
		/// <summary>
        /// Calls onDeleteVisits and onDeleteURI notifications on registered listeners
        /// with the history service.
        ///
        /// @param aURI
        /// The nsIURI object representing the URI of the page being expired.
        /// @param aVisitTime
        /// The time, in microseconds, that the page being expired was visited.
        /// @param aWholeEntry
        /// Indicates if this is the last visit for this URI.
        /// @param aGUID
        /// The unique ID associated with the page.
        /// @param aReason
        /// Indicates the reason for the removal.
        /// See nsINavHistoryObserver::REASON_* constants.
        /// @param aTransitionType
        /// If it's a valid TRANSITION_* value, all visits of the specified type
        /// have been removed.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyOnPageExpired([MarshalAs(UnmanagedType.Interface)] nsIURI aURI, long aVisitTime, [MarshalAs(UnmanagedType.U1)] bool aWholeEntry, [MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aGUID, ushort aReason, uint aTransitionType);
	}
}
