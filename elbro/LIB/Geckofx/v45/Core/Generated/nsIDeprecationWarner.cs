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
// Generated by IDLImporter from file nsIDeprecationWarner.idl
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
    /// Interface for warning about deprecated operations.  Consumers should
    /// attach this interface to the channel's notification callbacks/loadgroup.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("665c5124-2c52-41ba-ae72-2393f8e76c25")]
	public interface nsIDeprecationWarner
	{
		
		/// <summary>
        /// Issue a deprecation warning.
        ///
        /// @param aWarning a warning code as declared in nsDeprecatedOperationList.h.
        /// @param aAsError optional boolean flag indicating whether the warning
        /// should be treated as an error.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void IssueWarning(uint aWarning, [MarshalAs(UnmanagedType.U1)] bool aAsError);
	}
}
