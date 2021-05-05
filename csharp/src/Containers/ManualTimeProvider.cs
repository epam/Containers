using EPAM.Deltix.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPAM.Deltix.Containers{
    /// <summary>
    /// Implementation of breakpoint.
    /// </summary>
    class BreakPoint : IBreakPoint
    {
        /// <summary>
        /// Type of object in breakpoint
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// Context of breakpoint.
        /// </summary>
        public object CustomData { get; set; }
        /// <summary>
        /// Time of breakpoint.
        /// </summary>
        public HdDateTime Time { get; set; }
        /// <summary>
        /// Key of breakpoint in TimeProvider.
        /// </summary>
        public long Key { get; set; }
        /// <summary>
        /// Priority of breakpoint in TimeProvider.
        /// </summary>
        public long Priority { get; set; }
        /// <summary>
        /// Comparator for Breakpoint.
        /// </summary>
        /// <param name="other">Another breakpoint.</param>
        /// <returns>Return negative integer if this is less than other breakpoint, zero if equals, positive integer if more.</returns>
        public int CompareTo(IBreakPoint other)
        {
            int x = Time.CompareTo(other.Time);
            int y = Priority.CompareTo(other.Priority);
            int z;
            if (other is BreakPoint)
            {
                z = NumberOfMessage.CompareTo(((BreakPoint)other).NumberOfMessage);
            }
            else
            {
                z = Key.CompareTo(other.Key);
            }
            if (x != 0) return x;
            else if (y != 0) return y;
            else return z;
        }
        /// <summary>
        /// Number of this breakpoint in TimeProvider.
        /// </summary>
        public long NumberOfMessage { get; set; }
        /// <summary>
        /// Action which called for this breakpoint.
        /// </summary>
        public Action<HdDateTime, object> BreakPointAction { get; set; }
    }

    internal class BreakPointComparer : Comparer<BreakPoint>
    {
        public override int Compare(BreakPoint a, BreakPoint b)
        {
            int x = a.Time.CompareTo(b.Time);
			if (x != 0) return x;
			int y = a.Priority.CompareTo(b.Priority);
			if (y != 0) return y;
			int z = a.NumberOfMessage.CompareTo(b.NumberOfMessage);
            return z;
        }
    }
    /// <summary>
    /// Simple implementation of TimeProvider interface.
    /// </summary>
    public class ManualTimeProvider : ITimeProvider
    {
        static BreakPointComparer comparer = new BreakPointComparer();
        HeapWithIndices<BreakPoint> breakPoints = new HeapWithIndices<BreakPoint>(comparer);
        APMemoryManager<BreakPoint> memoryManager = new APMemoryManager<BreakPoint>();
        private long key = 0;

        /// <summary>
        /// Current time from this provider.
        /// </summary>
        public HdDateTime CurrentTime { get; set; }

        /// <summary>
        /// Move current time to new time.
        /// </summary>
        /// <param name="time">New time of time provider.</param>
        public void GoToTime(HdDateTime time)
        {
            while (breakPoints.Count > 0 && breakPoints.Peek.Value.Time <= time)
            {
                GoToNextBreakPoint(time);
            }

            CurrentTime = time;
        }

        bool needPop = false;

        /// <summary>
        /// Move current time to time of next breakpoint.
        /// </summary>
        public void GoToNextBreakPoint()
        {
            GoToNextBreakPoint(HdDateTime.MaxValue);
        }

        void GoToNextBreakPoint(HdDateTime destinationTime)
        {
            if (needPop)
            {
                memoryManager.Delete(breakPoints.Pop().Value);
            }
            if (breakPoints.Count == 0)
            {
                needPop = false;
                return;
            }
            BreakPoint breakPoint = breakPoints.Peek.Value;
            if (breakPoint.Time > destinationTime)
            {
                needPop = false;
                return;
            }
            needPop = true;
            long numberOfMessage = breakPoint.NumberOfMessage;
            CurrentTime = breakPoint.Time;
            int key = (int)breakPoint.Key;
            breakPoint.BreakPointAction(CurrentTime, breakPoint.CustomData);
        }
        /// <summary>
        /// Add new breakpoint to this provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="time">Time of new breakpoint.</param>
        /// <param name="context">Context of new breakpoint.</param>
        /// <param name="action">Action which will be called on new breakpoint.</param>
        /// <param name="priority">Priority of new breakpoint(breakpoints with lesser priority with same time called earlier).</param>
        /// <returns>Key of this breakpoint(positive ingeger number) or -1(if we can't add this breakpoint(For example time less than currentTime))</returns>
        public long AddBreakPoint<T>(HdDateTime time, T context, Action<HdDateTime, object> action, int priority = 0)
        {
            if (time < CurrentTime) return -1;
            key++;
            BreakPoint breakPoint = memoryManager.New();
            breakPoint.Time = time;
            breakPoint.CustomData = context;
            breakPoint.Priority = priority;
            breakPoint.BreakPointAction = action;
            breakPoint.NumberOfMessage = key;
            int _key;
            if (needPop)
            {
                memoryManager.Delete(breakPoints.Peek.Value);
                _key = breakPoints.ModifyTop(breakPoint).Key;
                needPop = false;
            }
            else
            {
                _key = breakPoints.Add(breakPoint);
            }
            breakPoint.Key = _key;
            return _key;
        }
        /// <summary>
        /// Delete breakpoint with key from this provider.
        /// </summary>
        /// <param name="key">Key of breakpoint.</param>
        public void DeleteBreakPoint(long key)
        {
            if (needPop)
            {
                memoryManager.Delete(breakPoints.Pop().Value);
                needPop = false;
            }
            BreakPoint breakPoint;
            if (breakPoints.TryGetValue((int)key, out breakPoint))
            {
                breakPoints.Remove((int)key);
                memoryManager.Delete(breakPoint);
            }
        }
    }

}
