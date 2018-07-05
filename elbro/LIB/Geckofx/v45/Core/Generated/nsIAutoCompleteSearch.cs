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
// Generated by IDLImporter from file nsIAutoCompleteSearch.idl
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
	[Guid("DE8DB85F-C1DE-4d87-94BA-7844890F91FE")]
	public interface nsIAutoCompleteSearch
	{
		
		/// <summary>
        /// Search for a given string and notify a listener (either synchronously
        /// or asynchronously) of the result
        ///
        /// @param searchString - The string to search for
        /// @param searchParam - An extra parameter
        /// @param previousResult - A previous result to use for faster searching
        /// @param listener - A listener to notify when the search is complete
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void StartSearch([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase searchString, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase searchParam, [MarshalAs(UnmanagedType.Interface)] nsIAutoCompleteResult previousResult, [MarshalAs(UnmanagedType.Interface)] nsIAutoCompleteObserver listener);
		
		/// <summary>
        /// Stop all searches that are in progress
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void StopSearch();
	}
	
	/// <summary>nsIAutoCompleteObserver </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8bd1dbbc-dcce-4007-9afa-b551eb687b61")]
	public interface nsIAutoCompleteObserver
	{
		
		/// <summary>
        /// Called when a search is complete and the results are ready
        ///
        /// @param search - The search object that processed this search
        /// @param result - The search result object
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnSearchResult([MarshalAs(UnmanagedType.Interface)] nsIAutoCompleteSearch search, [MarshalAs(UnmanagedType.Interface)] nsIAutoCompleteResult result);
		
		/// <summary>
        /// Called to update with new results
        ///
        /// @param search - The search object that processed this search
        /// @param result - The search result object
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnUpdateSearchResult([MarshalAs(UnmanagedType.Interface)] nsIAutoCompleteSearch search, [MarshalAs(UnmanagedType.Interface)] nsIAutoCompleteResult result);
	}
	
	/// <summary>nsIAutoCompleteSearchDescriptor </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4c3e7462-fbfb-4310-8f4b-239238392b75")]
	public interface nsIAutoCompleteSearchDescriptor
	{
		
		/// <summary>
        /// Identifies the search behavior.
        /// Should be one of the SEARCH_TYPE_* constants above.
        /// Defaults to SEARCH_TYPE_DELAYED.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		ushort GetSearchTypeAttribute();
		
		/// <summary>
        /// Whether a new search should be triggered when the user deletes the
        /// autofilled part.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetClearingAutoFillSearchesAgainAttribute();
	}
	
	/// <summary>nsIAutoCompleteSearchDescriptorConsts </summary>
	public class nsIAutoCompleteSearchDescriptorConsts
	{
		
		// <summary>
        // nsIAutoCompleteInput implementation.
        // </summary>
		public const ushort SEARCH_TYPE_DELAYED = 0;
		
		// <summary>
        // The search is started synchronously, before any delayed searches.
        // </summary>
		public const ushort SEARCH_TYPE_IMMEDIATE = 1;
	}
}
