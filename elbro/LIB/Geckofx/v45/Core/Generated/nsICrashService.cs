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
// Generated by IDLImporter from file nsICrashService.idl
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
	[Guid("f60d76e5-62c3-4f58-89f6-b726c2b7bc20")]
	public interface nsICrashService
	{
		
		/// <summary>
        /// Records the occurrence of a crash.
        ///
        /// @param processType
        /// One of the PROCESS_TYPE constants defined below.
        /// @param crashType
        /// One of the CRASH_TYPE constants defined below.
        /// @param id
        /// Crash ID. Likely a UUID.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddCrash(int processType, int crashType, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase id);
	}
	
	/// <summary>nsICrashServiceConsts </summary>
	public class nsICrashServiceConsts
	{
		
		// 
		public const long PROCESS_TYPE_MAIN = 0;
		
		// 
		public const long PROCESS_TYPE_CONTENT = 1;
		
		// 
		public const long PROCESS_TYPE_PLUGIN = 2;
		
		// 
		public const long PROCESS_TYPE_GMPLUGIN = 3;
		
		// 
		public const long CRASH_TYPE_CRASH = 0;
		
		// 
		public const long CRASH_TYPE_HANG = 1;
	}
}
