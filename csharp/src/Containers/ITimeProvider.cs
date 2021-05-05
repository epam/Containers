using EPAM.Deltix.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPAM.Deltix.Containers{
	interface IBreakPoint : IComparable<IBreakPoint>
	{
		Type Type { get; set; }
		HdDateTime Time { get; set; }
		long Key { get; set; }
		long Priority { get; set; }
	}

	interface IBreakPoint<T> : IBreakPoint
	{
		T data { get; set; }
	}

	/// <summary>
	/// Interface for TimeProvider(TimeProvider work with time. Allow us to get current time, to set breakpoint and etc).
	/// We suppose that implementation of this TimeProvider does'nt calls callbacks in addBreakpoint method.
	/// </summary>
	public interface ITimeProvider
	{
		/// <summary>
		/// Current time from this provider.
		/// </summary>
		HdDateTime CurrentTime { get; }
		/// <summary>
		/// Add new breakpoint to this provider.
		/// </summary>
		/// <typeparam name="T">Type of context.</typeparam>
		/// <param name="time">Time of new breakpoint.</param>
		/// <param name="context"> Context of new breakpoint.</param>
		/// <param name="action">Action which will be called on new breakpoint.</param>
		/// <param name="priority">Priority of new breakpoint (breakpoints with lesser priority with same time called earlier).</param>
		/// <returns>Key of this breakpoint or -1 if we can't add breakpoint(time less than current time).</returns>
		long AddBreakPoint<T>(HdDateTime time, T context, Action<HdDateTime, object> action, int priority = 0);
		/// <summary>
		/// Delete breakpoint with key from this provider.
		/// </summary>
		/// <param name="key">Key of breakpoint.</param>
		void DeleteBreakPoint(long key);
	}
}
