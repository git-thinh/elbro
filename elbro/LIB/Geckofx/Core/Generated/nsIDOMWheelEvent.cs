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
// Generated by IDLImporter from file nsIDOMWheelEvent.idl
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
    /// License, v. 2.0. If a copy of the MPL was not distributed with this file,
    /// You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("86e2b577-7e61-4ed5-8ddd-c1533bf07137")]
	public interface nsIDOMWheelEvent : nsIDOMMouseEvent
	{
		
		/// <summary>
        /// The name of the event (case-insensitive). The name must be an XML
        /// name.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetTypeAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aType);
		
		/// <summary>
        /// Used to indicate the EventTarget to which the event was originally
        /// dispatched.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMEventTarget GetTargetAttribute();
		
		/// <summary>
        /// Used to indicate the EventTarget whose EventListeners are currently
        /// being processed. This is particularly useful during capturing and
        /// bubbling.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMEventTarget GetCurrentTargetAttribute();
		
		/// <summary>
        /// Used to indicate which phase of event flow is currently being
        /// evaluated.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new ushort GetEventPhaseAttribute();
		
		/// <summary>
        /// Used to indicate whether or not an event is a bubbling event. If the
        /// event can bubble the value is true, else the value is false.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetBubblesAttribute();
		
		/// <summary>
        /// Used to indicate whether or not an event can have its default action
        /// prevented. If the default action can be prevented the value is true,
        /// else the value is false.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetCancelableAttribute();
		
		/// <summary>
        /// Used to specify the time (in milliseconds relative to the epoch) at
        /// which the event was created. Due to the fact that some systems may
        /// not provide this information the value of timeStamp may be not
        /// available for all events. When not available, a value of 0 will be
        /// returned. Examples of epoch time are the time of the system start or
        /// 0:0:0 UTC 1st January 1970.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new long GetTimeStampAttribute();
		
		/// <summary>
        /// The stopPropagation method is used prevent further propagation of an
        /// event during event flow. If this method is called by any
        /// EventListener the event will cease propagating through the tree. The
        /// event will complete dispatch to all listeners on the current
        /// EventTarget before event flow stops. This method may be used during
        /// any stage of event flow.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void StopPropagation();
		
		/// <summary>
        /// If an event is cancelable, the preventDefault method is used to
        /// signify that the event is to be canceled, meaning any default action
        /// normally taken by the implementation as a result of the event will
        /// not occur. If, during any stage of event flow, the preventDefault
        /// method is called the event is canceled. Any default action associated
        /// with the event will not occur. Calling this method for a
        /// non-cancelable event has no effect. Once preventDefault has been
        /// called it will remain in effect throughout the remainder of the
        /// event's propagation. This method may be used during any stage of
        /// event flow.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void PreventDefault();
		
		/// <summary>
        /// The initEvent method is used to initialize the value of an Event
        /// created through the DocumentEvent interface. This method may only be
        /// called before the Event has been dispatched via the dispatchEvent
        /// method, though it may be called multiple times during that phase if
        /// necessary. If called multiple times the final invocation takes
        /// precedence. If called from a subclass of Event interface only the
        /// values specified in the initEvent method are modified, all other
        /// attributes are left unchanged.
        ///
        /// @param   eventTypeArg Specifies the event type. This type may be
        /// any event type currently defined in this
        /// specification or a new event type.. The string
        /// must be an XML name.
        /// Any new event type must not begin with any
        /// upper, lower, or mixed case version of the
        /// string "DOM". This prefix is reserved for
        /// future DOM event sets. It is also strongly
        /// recommended that third parties adding their
        /// own events use their own prefix to avoid
        /// confusion and lessen the probability of
        /// conflicts with other new events.
        /// @param   canBubbleArg Specifies whether or not the event can bubble.
        /// @param   cancelableArg Specifies whether or not the event's default
        /// action can be prevented.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void InitEvent([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase eventTypeArg, [MarshalAs(UnmanagedType.U1)] bool canBubbleArg, [MarshalAs(UnmanagedType.U1)] bool cancelableArg);
		
		/// <summary>
        /// Used to indicate whether preventDefault() has been called for this event.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetDefaultPreventedAttribute();
		
		/// <summary>
        /// Prevents other event listeners from being triggered and,
        /// unlike Event.stopPropagation() its effect is immediate.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void StopImmediatePropagation();
		
		/// <summary>
        ///The original target of the event, before any retargetings. </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMEventTarget GetOriginalTargetAttribute();
		
		/// <summary>
        /// The explicit original target of the event.  If the event was retargeted
        /// for some reason other than an anonymous boundary crossing, this will be set
        /// to the target before the retargeting occurs.  For example, mouse events
        /// are retargeted to their parent node when they happen over text nodes (bug
        /// 185889), and in that case .target will show the parent and
        /// .explicitOriginalTarget will show the text node.
        /// .explicitOriginalTarget differs from .originalTarget in that it will never
        /// contain anonymous content.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMEventTarget GetExplicitOriginalTargetAttribute();
		
		/// <summary>
        /// @deprecated Use nsIDOMEvent::defaultPrevented.
        /// To be removed in bug 691151.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetPreventDefault();
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetIsTrustedAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void DuplicatePrivateData();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SetTarget([MarshalAs(UnmanagedType.Interface)] nsIDOMEventTarget aTarget);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool IsDispatchStopped();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new System.IntPtr GetInternalNSEvent();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SetTrusted([MarshalAs(UnmanagedType.U1)] bool aTrusted);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void Serialize(System.IntPtr aMsg, [MarshalAs(UnmanagedType.U1)] bool aSerializeInterfaceType);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool Deserialize(System.IntPtr aMsg, ref System.IntPtr aIter);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SetOwner(nsIDOMEventTarget aOwner);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new System.IntPtr InternalDOMEvent();
		
		/// <summary>
        /// The nsIDOMUIEvent interface is the datatype for all UI events in the
        /// Document Object Model.
        ///
        /// For more information on this interface please see
        /// http://www.w3.org/TR/DOM-Level-2-Events/
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMWindow GetViewAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetDetailAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void InitUIEvent([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase typeArg, [MarshalAs(UnmanagedType.U1)] bool canBubbleArg, [MarshalAs(UnmanagedType.U1)] bool cancelableArg, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow viewArg, int detailArg);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetLayerXAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetLayerYAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetPageXAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetPageYAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new uint GetWhichAttribute();
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMNode GetRangeParentAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetRangeOffsetAttribute();
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetCancelBubbleAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SetCancelBubbleAttribute([MarshalAs(UnmanagedType.U1)] bool aCancelBubble);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetIsCharAttribute();
		
		/// <summary>
        /// The nsIDOMMouseEvent interface is the datatype for all mouse events
        /// in the Document Object Model.
        ///
        /// For more information on this interface please see
        /// http://www.w3.org/TR/DOM-Level-2-Events/
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetScreenXAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetScreenYAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetMozMovementXAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetMozMovementYAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetClientXAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetClientYAttribute();
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetCtrlKeyAttribute();
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetShiftKeyAttribute();
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetAltKeyAttribute();
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetMetaKeyAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new short GetButtonAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new ushort GetButtonsAttribute();
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIDOMEventTarget GetRelatedTargetAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void InitMouseEvent([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase typeArg, [MarshalAs(UnmanagedType.U1)] bool canBubbleArg, [MarshalAs(UnmanagedType.U1)] bool cancelableArg, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow viewArg, int detailArg, int screenXArg, int screenYArg, int clientXArg, int clientYArg, [MarshalAs(UnmanagedType.U1)] bool ctrlKeyArg, [MarshalAs(UnmanagedType.U1)] bool altKeyArg, [MarshalAs(UnmanagedType.U1)] bool shiftKeyArg, [MarshalAs(UnmanagedType.U1)] bool metaKeyArg, ushort buttonArg, [MarshalAs(UnmanagedType.Interface)] nsIDOMEventTarget relatedTargetArg);
		
		/// <summary>
        /// ranges between 0.0 and 1.0
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new float GetMozPressureAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new ushort GetMozInputSourceAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void InitNSMouseEvent(
					[MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase typeArg, 
					[MarshalAs(UnmanagedType.U1)] bool canBubbleArg, 
					[MarshalAs(UnmanagedType.U1)] bool cancelableArg, 
					[MarshalAs(UnmanagedType.Interface)] nsIDOMWindow viewArg, 
					int detailArg, 
					int screenXArg, 
					int screenYArg, 
					int clientXArg, 
					int clientYArg, 
					[MarshalAs(UnmanagedType.U1)] bool ctrlKeyArg, 
					[MarshalAs(UnmanagedType.U1)] bool altKeyArg, 
					[MarshalAs(UnmanagedType.U1)] bool shiftKeyArg, 
					[MarshalAs(UnmanagedType.U1)] bool metaKeyArg, 
					ushort buttonArg, 
					[MarshalAs(UnmanagedType.Interface)] nsIDOMEventTarget relatedTargetArg, 
					float pressure, 
					ushort inputSourceArg);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool GetModifierState([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase keyArg);
		
		/// <summary>
        /// }
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetDeltaXAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetDeltaYAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetDeltaZAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetDeltaModeAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InitWheelEvent(
					[MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase typeArg, 
					[MarshalAs(UnmanagedType.U1)] bool canBubbleArg, 
					[MarshalAs(UnmanagedType.U1)] bool cancelableArg, 
					[MarshalAs(UnmanagedType.Interface)] nsIDOMWindow viewArg, 
					int detailArg, 
					int screenXArg, 
					int screenYArg, 
					int clientXArg, 
					int clientYArg, 
					ushort buttonArg, 
					[MarshalAs(UnmanagedType.Interface)] nsIDOMEventTarget relatedTargetArg, 
					[MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase modifiersListArg, 
					double deltaXArg, 
					double deltaYArg, 
					double deltaZArg, 
					uint deltaMode);
	}
	
	/// <summary>nsIDOMWheelEventConsts </summary>
	public class nsIDOMWheelEventConsts
	{
		
		// <summary>
        //This Source Code Form is subject to the terms of the Mozilla Public
        // License, v. 2.0. If a copy of the MPL was not distributed with this file,
        // You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		public const ulong DOM_DELTA_PIXEL = 0x00;
		
		// 
		public const ulong DOM_DELTA_LINE = 0x01;
		
		// 
		public const ulong DOM_DELTA_PAGE = 0x02;
	}
}
