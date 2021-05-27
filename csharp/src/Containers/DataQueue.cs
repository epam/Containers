/*
  Copyright 2021 EPAM Systems, Inc

  See the NOTICE file distributed with this work for additional information
  regarding copyright ownership. Licensed under the Apache License,
  Version 2.0 (the "License"); you may not use this file except in compliance
  with the License.  You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
  WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the
  License for the specific language governing permissions and limitations under
  the License.
 */
﻿using System;
using System.Collections;
using System.Collections.Generic;
using EPAM.Deltix.DFP;

namespace EPAM.Deltix.Containers {
	/// <summary>
	/// Represents a dynamic circled queue, specialized to work with Decimal.
	/// 	Additionally it optionally can calculate simple statistical values.
	/// 	It has four methods of work and it can store elements in sliding window
	/// 	specified by time period or number of elements.
	///
	/// 	Also it support direct access to element.Using two indexers: by index (int) and by time (DateTime).
	/// 	All method has two versions, with and without time parameter.Please do not use both for the same class instance.
	/// Using method with time is mandatory if you want to use time indexer or time period sliding window(AutoDynamic type).
	/// You can specify number of listener to do some additional work when queue store or drop element.
	/// </summary>
	public class DataQueue : IEnumerable
	{
		/// <summary>
		/// 	SingleElement: Queue store only one single element. (Useful as default method for indicators history storing)
		/// 	ManualControl: Support all typical operations specific to Dequeue. (Useful for some custom work).
		/// 	AutoStatic: Store all elements in sliding window specified by number elements to store. Automatically drop element when it out of the window.
		/// 	AutoDynamic: Store all elements in sliding window specified by time period. Automatically drop element when it out of the window.
		/// <summary>
		public enum QueueType
		{
			SingleElement,
			AutoStatic,
			ManualControl,
			AutoDynamic
		}

		/// <summary>
		/// OnPush event
		/// </summary>
		public event Action<Decimal64, DateTime> OnPush;

		/// <summary>
		/// OnPop?.Invoke event
		/// </summary>
		public event Action<Decimal64, DateTime> OnPop;

		// We store our data and DateTimes in simple arrays
		private Decimal64[] queueBuffer;
		private DateTime[] queueDates;

		/// <summary>
		/// Default value, which will be returned for invalid request or request for empty queue.
		/// </summary>
		public static Decimal64 DefaultValue = Decimal64.NaN;

		/// <summary>
		/// Default DateTime, which will be assumed as default date value.
		/// </summary>
		public static readonly DateTime DefaultDateTime = DateTime.MinValue;

		// In case when we use single element, we use this two fields, instead of allocating arrays
		private Decimal64 previousElement = DefaultValue;
		private DateTime previousElementDate = DefaultDateTime;
		private Decimal64 singleElement = DefaultValue;
		private DateTime singleElementDate = DefaultDateTime;

		// current first valid element position
		private int first = 0;
		// current first free position (not normalized)
		private int last = 0;
		// valid elements count
		private int count = 0;
		// time period window size
		private TimeSpan period;

		// Indicates that queue need DateTime method to be called.
		private bool needDateTime;
		// Indicates current method of work.
		private QueueType queueType = QueueType.ManualControl;


		public struct StatisticsData
		{
			// Sum of added elements
			public Decimal64 sum;

			// Sum of absolute values of added elements
			public Decimal64 abssum;

			// Sum of squares of added elements
			public Decimal64 sqsum;

			// The number of elements in the stat
			public int count;
			public void Init()
			{
				sum = Decimal64.Zero;
				abssum = Decimal64.Zero;
				sqsum = Decimal64.Zero;
				count = 0;
			}

			public void Set(Decimal64 data)
			{
				sum = data;
				abssum = data.Abs();
				sqsum = data * data;
				count = 1;
			}

			public void Push(Decimal64 data)
			{
				sum += data;
				abssum += data.Abs();
				sqsum += data * data;
				count++;
			}

			public void Pop(Decimal64 data)
			{
				sum -= data;
				abssum -= data.Abs();
				sqsum -= data * data;
				count--;
			}
		}

		private StatisticsData stat;

		// Indicates that queue need calculate stat.
		bool calcStatistics = false;

		/// <summary>
		/// Initializes a new instance of the FinAnalysis.Base.DataQueue
		/// class that contains circle queue of Data and DateTime (optional). Default method of use is ManualControl.
		/// Queue capacity will be dynamic. You can manage it manually Put\Remove Last or First elements as in Dequeue.
		/// </summary>
		/// <param name="needDateTime"> Is working with dates needed </param>
		public DataQueue(bool needDateTime)
		{
			queueBuffer = new Decimal64[10];
			queueDates = new DateTime[10];

			first = last = 0;
			queueType = QueueType.ManualControl;
			this.needDateTime = needDateTime;
		}

		/// <summary>
		/// Initializes a new instance of the FinAnalysis.Base.DataQueue
		/// class that contains circle queue of Data and DateTime (optional). In this case method of use will be AutoStatic,
		/// or singleElement if you specify capacity equal to one. Queue capacity will be 
		/// fixed and it will be enough for exact "capacity" elements.
		/// </summary>
		/// <param name="capacity"> Queue capacity will be fixed and it will be enough for exact "capacity" elements. </param>
		/// <param name="needDateTime"> Is working with dates needed </param>
		/// <exception cref="ArgumentOutOfRangeException">Capacity should be at least 1.</exception>
		public DataQueue(int capacity, bool needDateTime)
		{
			if (capacity < 1)
				throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity should be positive.");

			if (capacity == 1)
				queueType = QueueType.SingleElement;
			else
			{
				queueType = QueueType.AutoStatic;
				queueBuffer = new Decimal64[capacity];
				queueDates = new DateTime[capacity];
			}
			first = last = 0;
			this.needDateTime = needDateTime;
		}

		/// <summary>
		/// Initializes a new instance of the FinAnalysis.Base.DataQueue
		/// class that contains circle queue of Data and DateTime (optional). In this case method of use will be AutoDynamic,
		/// Queue capacity will be dynamic and it will be enough for contain all elements on specified time period. 
		/// Using Put method with DateTime is mandatory in this case.
		/// Note: You can give queue a hint, what initial arrays size it should use, by setting Capacity field.
		/// </summary>
		/// <param name="period"> Queue capacity will be dynamic and it will be enough for contain all elements of particular time period. </param>
		public DataQueue(TimeSpan period)
		{
			if (period <= TimeSpan.Zero)
				throw new ArgumentOutOfRangeException(nameof(period), period, "The period must be positive.");

			queueType = QueueType.AutoDynamic;
			this.period = period;
			queueBuffer = new Decimal64[64];
			queueDates = new DateTime[64];
			first = last = 0;
			needDateTime = true;
		}

		/// <summary>
		/// Initializes a new instance of the FinAnalysis.Base.DataQueue
		/// class that contains circle queue of Data and DateTime (optional). In this case method of use will be AutoDynamic,
		/// Queue capacity will be dynamic and it will be enough for contain all elements on specified time period. 
		/// Using Put method with DateTime is mandatory in this case.
		/// Note: You can give queue a hint, what initial arrays size it should use, by setting Capacity field.
		/// </summary>
		/// <param name="capacity"> Queue capacity will be fixed and it will be enough for exact "capacity" elements. </param>
		/// <param name="period"> Queue capacity will be dynamic and it will be enough for contain all elements of particular time period. </param>
		public DataQueue(int capacity, TimeSpan period)
		{
			if (period <= TimeSpan.Zero)
				throw new ArgumentOutOfRangeException(nameof(period), period, "The period must be positive.");

			queueType = QueueType.AutoDynamic;
			this.period = period;
			queueBuffer = new Decimal64[capacity];
			queueDates = new DateTime[capacity];
			first = last = 0;
			needDateTime = true;
		}

		/// <summary>
		/// Initializes a new instance of the FinAnalysis.Base.DataQueue
		/// class that contains circle queue of Data and DateTime. Default method of use is ManualControl.
		/// Queue capacity will be dynamic. You can manage it manually Put\Remove Last or First elements as in Dequeue.
		/// </summary>
		/// <param name="calcStatistics"> Specify it true if you want queue to calculate simple stat. </param>
		/// <param name="needDateTime"> Is working with dates needed </param>
		public DataQueue(bool calcStatistics, bool needDateTime)
			: this(needDateTime)
		{
			if (calcStatistics)
			{
				stat = new StatisticsData();
				this.calcStatistics = true;
			}
		}

		/// <summary>
		/// Initializes a new instance of the FinAnalysis.Base.DataQueue
		/// class that contains circle queue of Data and DateTime (optional). In this case method of use will be AutoStatic,
		/// or singleElement if you specify capacity equal to one. Queue capacity will be 
		/// fixed and it will be enough for exact "capacity" elements.
		/// </summary>
		/// <param name="capacity"> Queue capacity will be fixed and it will be enough for exact "capacity" elements. </param>
		/// <param name="calcStatistics"> Specify it true if you want queue to calculate simple stat. </param>
		/// <param name="needDateTime"> Is working with dates needed </param>
		/// <exception cref="ArgumentOutOfRangeException">Capacity should be at least 1.</exception>
		public DataQueue(int capacity, bool calcStatistics, bool needDateTime)
			: this(capacity, needDateTime)
		{
			if (calcStatistics)
			{
				stat = new StatisticsData();
				this.calcStatistics = true;
			}
		}

		/// <summary>
		/// Initializes a new instance of the FinAnalysis.Base.DataQueue
		/// class that contains circle queue of Data and DateTime. In this case method of use will be AutoDynamic,
		/// Queue capacity will be dynamic and it will be enough for contain all elements on specified time period. 
		/// Using Put method with DateTime is mandatory in this case.
		/// Note: You can give queue a hint, what initial arrays size it should use, by setting Capacity field.
		/// </summary>
		/// <param name="capacity"> Queue capacity will be fixed and it will be enough for exact "capacity" elements. </param>
		/// <param name="period"> Queue capacity will be dynamic and it will be enough for contain all elements of particular time period. </param>
		/// <param name="calcStatistics"> Specify it true if you want queue to calculate simple stat. </param>
		public DataQueue(TimeSpan period, bool calcStatistics)
			: this(period)
		{
			if (calcStatistics)
			{
				stat = new StatisticsData();
				this.calcStatistics = true;
			}
		}

		/// <summary>
		/// Initializes a new instance of the FinAnalysis.Base.DataQueue
		/// class that contains circle queue of Data and DateTime. In this case method of use will be AutoDynamic,
		/// Queue capacity will be dynamic and it will be enough for contain all elements on specified time period. 
		/// Using Put method with DateTime is mandatory in this case.
		/// Note: You can give queue a hint, what initial arrays size it should use, by setting Capacity field.
		/// </summary>
		/// <param name="period"> Queue capacity will be dynamic and it will be enough for contain all elements of particular time period. </param>
		/// <param name="calcStatistics"> Specify it true if you want queue to calculate simple stat. </param>
		public DataQueue(int capacity, TimeSpan period, bool calcStatistics)
			: this(capacity, period)
		{
			if (calcStatistics)
			{
				stat = new StatisticsData();
				this.calcStatistics = true;
			}
		}

		/// <summary>
		/// This method expands queue. It doubles its capacity, reallocates buffers and restores elements.
		/// </summary>
		private void Expand()
		{
			Decimal64[] newArray = new Decimal64[queueBuffer.Length << 1];

			Array.Copy(queueBuffer, first, newArray, 0, queueBuffer.Length - first);
			if (first > 0)
				Array.Copy(queueBuffer, 0, newArray, queueBuffer.Length - first, first);

			queueBuffer = newArray;

			if (needDateTime)
			{
				DateTime[] newLongArray = new DateTime[queueDates.Length << 1];

				Array.Copy(queueDates, first, newLongArray, 0, queueDates.Length - first);
				if (first > 0)
					Array.Copy(queueDates, 0, newLongArray, queueDates.Length - first, first);
				first = 0;
				queueDates = newLongArray;
			}
			first = 0;
			last = first + count;
		}

		/// <summary>
		/// Store new data element in queue. This method for static automatic managed or single queue. 
		/// Do not call it for manually managed queue or dynamic automatic queue (ManualControl, AutoDynamic types). 
		/// Note: Please do not use both with and without DateTime method for one queue instance.
		/// </summary>
		/// <param name="data"> Data element to store. </param>
		/// <returns> Returns true if and only if we succeed to add element. </returns>
		/// <exception cref="InvalidOperationException">Thrown when you trying to use this method for ManualControl or AutoDynamic queue. 
		/// Or try to use this method after you have already used method with DateTime.</exception>
		public void Put(Decimal64 data)
		{
			if (queueType == QueueType.ManualControl)
				throw new InvalidOperationException("DataQueue: Use Manual methods PutLast, PutFirst.");
			if (needDateTime)
				throw new InvalidOperationException("DataQueue: Do not use this method for AutoDynamic, and do not use both with and without DateTime method for one queue instance.");

			if (queueType == QueueType.ManualControl)
				throw new InvalidOperationException("DataQueue: Use Manual methods putLast, putFirst.");

			if (needDateTime)
				throw new InvalidOperationException("DataQueue: Do not use this method for AutoDynamic, and do not use both with and without DateTime method for one queue instance.");

			if (queueType == QueueType.SingleElement)
			{
				if (calcStatistics)
					stat.Init();

				if (count > 0)
				{
					count = 0;
					OnPop?.Invoke(singleElement, DefaultDateTime);
				}

				previousElement = singleElement;
				singleElement = data;
				count = 1;

				if (calcStatistics)
					stat.Set(data);

				OnPush?.Invoke(singleElement, DefaultDateTime);
			}
			else
			{
				if (count == queueBuffer.Length) // Need to drop something ?
				{
					count--;

					Decimal64 data_ = queueBuffer[first++];
					if (first == queueBuffer.Length)
						first = 0;
					if (calcStatistics)
						stat.Pop(data_);
					OnPop?.Invoke(data_, DefaultDateTime);
				}


				count++;
				queueBuffer[last++] = data;
				if (last == queueBuffer.Length)
					last = 0;

				OnPush?.Invoke(data, DefaultDateTime);

				if (calcStatistics)
					stat.Push(data);

			}
		}

		/// <summary>
		/// Stores new data element in queue. This method for automatic managed or single element queue. 
		/// Do not call it for manually managed queue or dynamic automatic queue (ManualControl type). 
		/// Note: Please do not use both with and without DateTime method for one queue instance.
		/// </summary>
		/// <param name="data"> Data element to store. </param>
		/// <param name="time"> User defined time. Note: if time is earlier than previous, queue will ignore this call. </param>
		/// <returns> Returns true if and only if we succeed to add element. </returns>
		/// <exception cref="InvalidOperationException">Thrown when you trying to use this method for ManualControl queue. 
		/// Or try to use this method after you have already used method without DateTime.</exception>
		public void Put(Decimal64 data, DateTime time)
		{
			if (time == DefaultDateTime)
			{
				Put(data);
				return;
			}
			if (queueType == QueueType.ManualControl)
				throw new InvalidOperationException("DataQueue: Use Manual methods putLast, putFirst.");

			if (!needDateTime)
				throw new InvalidOperationException("DataQueue: Do not use both with and without DateTime method for one queue instance.");

			if (queueType == QueueType.SingleElement)
			{
				if (calcStatistics)
					stat.Init();

				if (count > 0)
				{
					count = 0;
					OnPop?.Invoke(singleElement, singleElementDate);
				}

				count = 1;
				previousElementDate = singleElementDate;
				previousElement = singleElement;
				singleElement = data;
				singleElementDate = time;

				if (calcStatistics)
					stat.Set(data);
				OnPush?.Invoke(singleElement, singleElementDate);
			}
			else
			{
				int prevIndex = last == 0 ? queueDates.Length - 1 : last - 1;

				if (count > 0 && time < queueDates[prevIndex])
					throw new ArgumentOutOfRangeException("DataQueue: New element has time less then last element.");

				if (queueType == QueueType.AutoDynamic)
				{
					// Lets delete outdated data
					// if our queue not empty and if first element have to older date then drop it
					while (count > 0 && queueDates[first] + period < time)
					{
						count--;
						int tempIndex = first++;
						if (first == queueDates.Length)
							first = 0;

						Decimal64 data2 = queueBuffer[tempIndex];
						if (calcStatistics)
							stat.Pop(data2);
						
						OnPop?.Invoke(data2, queueDates[tempIndex]);
					}

					if (count > 0 && first == last)
						Expand();


					count++;

					queueBuffer[last] = data;
					queueDates[last++] = time;
					if (last == queueBuffer.Length)
						last = 0;

					if (calcStatistics)
						stat.Push(data);
				}
				else
				{
					if (count == queueBuffer.Length) // Need to drop something ?
					{
						count--;
						int tempIndex = first++;
						if (first == queueBuffer.Length)
							first = 0;

						Decimal64 data_ = queueBuffer[tempIndex];
						if (calcStatistics)
							stat.Pop(data_);

						OnPop?.Invoke(data_, queueDates[tempIndex]);
					}

					count++;
					queueBuffer[last] = data;
					queueDates[last++] = time;
					if (last == queueBuffer.Length)
						last = 0;

					if (calcStatistics)
						stat.Push(data);

				}
			}
			OnPush?.Invoke(data, time);
		}

		/// <summary>
		/// Store new data element on last position in queue. This method for manual queue. 
		/// Do not call it for automatic managed queue or single element queue (singleElement, AutoDynamic, AutoStatic type). 
		/// Note: Please do not use both with and without DateTime method for one queue instance.
		/// </summary>
		/// <param name="data"> Data element to store. </param>
		/// <param name="time"> User defined time. Note: if time is earlier than last, queue will ignore this call. </param>
		/// <returns> Returns true if and only if we succeed to add element. </returns>
		/// <exception cref="InvalidOperationException">Thrown when you trying to use this method for non ManualControl queue. 
		/// Or try to use this method after you have already used method without DateTime.</exception>
		public void PutLast(Decimal64 data, DateTime time)
		{
			if (time == DefaultDateTime)
			{
				PutLast(data);
				return;
			}

			if (queueType != QueueType.ManualControl)
				throw new InvalidOperationException("DataQueue:  Use Auto method put.");

			if (!needDateTime)
				throw new InvalidOperationException("DataQueue: Do not use both with and without DateTime method for one queue instance.");

			if (count > 0 && time < queueDates[last == 0 ? queueBuffer.Length - 1 : last - 1])
				throw new ArgumentOutOfRangeException("DataQueue: New element has time less then last element.");

			if (count > 0 && first == last)
				Expand();

			count++;
			queueBuffer[last] = data;
			queueDates[last++] = time;
			if (last == queueBuffer.Length)
				last = 0;

			OnPush?.Invoke(data, time);

			if (calcStatistics)
				stat.Push(data);
		}

		/// <summary>
		/// Store new data element on first position in queue. This method for manual queue. 
		/// Do not call it for automatic managed queue or single element queue (singleElement, AutoDynamic, AutoStatic type). 
		/// Note: Please do not use both with and without DateTime method for one queue instance.
		/// </summary>
		/// <param name="data"> Data element to store. </param>
		/// <param name="time"> User defined time. Note: if first time is earlier than specified, queue will ignore this call. </param>
		/// <returns> Returns true if and only if we succeed to add element. </returns>
		/// <exception cref="InvalidOperationException">Thrown when you trying to use this method for non ManualControl queue. 
		/// Or try to use this method after you have already used method without DateTime.</exception>
		public void PutFirst(Decimal64 data, DateTime time)
		{
			if (time == DefaultDateTime)
			{
				PutFirst(data);
				return;
			}

			if (queueType != QueueType.ManualControl)
				throw new InvalidOperationException("DataQueue:  Use Auto method Put.");

			if (!needDateTime)
				throw new InvalidOperationException("DataQueue: Do not use both with and without DateTime method for one queue instance.");

			if (count > 0 && time > queueDates[first])
				throw new ArgumentOutOfRangeException("DataQueue: New element has time more then first element.");

			if (count > 0 && first == last)
				Expand();

			if (first == 0)
				first = queueBuffer.Length - 1;
			else
				first--;

			queueBuffer[first] = data;
			queueDates[first] = time;
			count++;

			OnPush?.Invoke(data, time);

			if (calcStatistics)
				stat.Push(data);
		}

		/// <summary>
		/// Store new data element on last position in queue. This method for manual queue. 
		/// Do not call it for automatic managed queue or single element queue (singleElement, AutoDynamic, AutoStatic type). 
		/// Note: Please do not use both with and without DateTime method for one queue instance.
		/// </summary>
		/// <param name="data"> Data element to store. </param>
		/// <returns> Returns true if and only if we succeed to add element. </returns>
		/// <exception cref="InvalidOperationException">Thrown when you trying to use this method for non ManualControl queue. 
		/// Or try to use this method after you have already used method with DateTime.</exception>
		public void PutLast(Decimal64 data)
		{
			if (queueType != QueueType.ManualControl)
				throw new InvalidOperationException("DataQueue:  Use Auto method put.");

			if (needDateTime)
				throw new InvalidOperationException("DataQueue: Do not use both with and without DateTime method for one queue instance.");

			if (count > 0 && first == last)
				Expand();

			count++;
			queueBuffer[last++] = data;
			if (last == queueBuffer.Length)
				last = 0;

			OnPush?.Invoke(data, DefaultDateTime);

			if (calcStatistics)
				stat.Push(data);
		}

		/// <summary>
		/// Store new data element on first position in queue. This method for manual queue. 
		/// Do not call it for automatic managed queue or single element queue (singleElement, AutoDynamic, AutoStatic type). 
		/// Note: Please do not use both with and without DateTime method for one queue instance.
		/// </summary>
		/// <param name="data"> Data element to store. </param>
		/// <returns> Returns true if and only if we succeed to add element. </returns>
		/// <exception cref="InvalidOperationException">Thrown when you trying to use this method for non ManualControl queue. 
		/// Or try to use this method after you have already used method with DateTime.</exception>
		public void PutFirst(Decimal64 data)
		{
			if (queueType != QueueType.ManualControl)
				throw new InvalidOperationException("DataQueue:  Use Auto method put.");

			if (needDateTime)
				throw new InvalidOperationException("DataQueue: Do not use both with and without DateTime method for one queue instance.");

			if (count > 0 && first == last)
				Expand();

			if (first == 0)
				first = queueBuffer.Length - 1;
			else
				first--;

			queueBuffer[first] = data;
			count++;

			OnPush?.Invoke(data, DefaultDateTime);

			if (calcStatistics)
				stat.Push(data);
		}

		/// <summary>
		/// Remove data element from first position in queue. This method for manual queue. 
		/// Do not call it for automatic managed queue or single element queue (singleElement, AutoDynamic, AutoStatic type). 
		/// Note: Please do not use both with and without DateTime method for one queue instance.
		/// </summary>
		/// <returns> Returns true if and only if we succeed to drop element. </returns>
		/// <exception cref="InvalidOperationException">Thrown when you trying to use this method for non ManualControl queue. 
		/// Or try to use this method before adding at least one element.</exception>
		public void RemoveFirst()
		{
			if (queueType != QueueType.ManualControl)
				throw new InvalidOperationException("DataQueue: Do not call this method for non MANUAL_CONTROL queue.");
			if (count == 0)
				throw new InvalidOperationException("DataQueue: Queue is not initialized. It seems that you hadn't put anything to it.");

			count--;
			int tempIndex = first++;
			if (first == queueBuffer.Length)
				first = 0;

			Decimal64 data = queueBuffer[tempIndex];
			if (calcStatistics)
				stat.Pop(data);

			OnPop?.Invoke(data, needDateTime ? queueDates[tempIndex] : DefaultDateTime);
		}

		/// <summary>
		/// Remove data element from last position in queue. This method for manual queue. 
		/// Do not call it for automatic managed queue or single element queue (singleElement, AutoDynamic, AutoStatic type). 
		/// Note: Please do not use both with and without DateTime method for one queue instance.
		/// </summary>
		/// <returns> Returns true if and only if we succeed to drop element. </returns>
		/// <exception cref="InvalidOperationException">Thrown when you trying to use this method for non ManualControl queue. 
		/// Or try to use this method before adding at least one element.</exception>
		public void RemoveLast()
		{
			if (queueType != QueueType.ManualControl)
				throw new InvalidOperationException("DataQueue: Do not call this method for non MANUAL_CONTROL queue.");
			if (count == 0)
				throw new InvalidOperationException("DataQueue: Queue is not initialized. It seems that you hadn't put anything to it.");

			last--;
			if (last < 0)
				last = queueBuffer.Length - 1;

			Decimal64 data = queueBuffer[last];
			if (calcStatistics)
				stat.Pop(data);

			OnPop?.Invoke(data, needDateTime ? queueDates[last] : DefaultDateTime);
		}

		/// <summary>
		/// Returns element which was store "step" steps ago.
		/// </summary>
		/// <param name="step"> Get time which was stored with element "step" steps ago. </param>
		/// <returns> Value associated with specified point. </returns>
		public Decimal64 GetPointsAgo(int step)
		{
			if (queueType == QueueType.SingleElement)
			{
				if (step == 0)
					return singleElement;
				if (step == 1)
					return previousElement;
				throw new InvalidOperationException("DataQueue: Do not use this method for single element queue.");
			}


			if (step < 0 || step >= count)
				throw new IndexOutOfRangeException();

			step = (last - 1) - step;
			if (step < 0)
				step += queueBuffer.Length;

			return queueBuffer[step];
		}

		/// <summary>
		/// Return time associated with specified point.
		/// </summary>
		/// <param name="step"> Get time which was stored with element "step" steps ago. </param>
		/// <returns> Time associated with specified point. </returns>
		public DateTime GetTimeByIndexAgo(int step)
		{
			if (!needDateTime)
				throw new InvalidOperationException("DataQueue: It seems that you didn't specify DateTime for data elements, so this call is incorrect.");

			if (queueType == QueueType.SingleElement)
			{
				if (step == 0)
					return singleElementDate;
				if (step == 1)
					return previousElementDate;
				throw new InvalidOperationException("DataQueue: Do not use this method for single element queue.");
			}

			if (step < 0 || step >= count)
				return DefaultDateTime;

			step = (last - 1) - step;
			if (step < 0)
				step += queueBuffer.Length;

			return queueDates[step];
		}

		private int leftBinarySearch(DateTime time)
		{
			int index = first + count;
			index = index >= queueBuffer.Length ? index - queueBuffer.Length : index;

			// We use binary search for it, O( log N ) complexity
			// Define borders
			int left = first; // starting from the first

			int right = index == 0 ? queueBuffer.Length - 1 : index - 1; // to cursor - 1, last valid element
			int center = 0;

			if (left > right) // normalize
				right += queueBuffer.Length;

			while (right - left > 1) // Binary search
			{
				center = (left + right) >> 1;

				if (queueDates[(center >= queueBuffer.Length ? center - queueBuffer.Length : center)] > time)
					right = center;
				else
					left = center;
			}
			left = left >= queueBuffer.Length ? left - queueBuffer.Length : left;
			return left;
		}

		/// <summary>
		/// Get offset index of element by element time.
		/// If there no element with such time, we return latest element with time smaller than specified.
		/// If we can't find valid index, we return -1.
		/// </summary>
		/// <param name="time"> Element time. </param>
		/// <returns> Zero-based index of desired point starting from latest one. </returns>
		public int GetIndexByTime(DateTime time)
		{
			if (queueType == QueueType.SingleElement)
				throw new InvalidOperationException("DataQueue: Do not use this method for single element queue.");
			if (!needDateTime)
				throw new InvalidOperationException("DataQueue: It seems that you didn't specify DateTime for data elements, so this call is incorrect.");

			if (queueDates[first] > time) // if our first valid element not older enough
				return -1;

			if (count == 0) // if queue is empty there is nothing to search
				return -1;


			int left = leftBinarySearch(time);
			int right = left + 1;

			// we return historically latest element with smaller or equal DateTime
			if (right < queueDates.Length && queueDates[right] <= time)
				return (last - right - 1 + queueBuffer.Length) % queueBuffer.Length;
			else
				return (last - left - 1 + queueBuffer.Length) % queueBuffer.Length;
		}

		/// <summary>
		/// Returns historically latest element with smaller or equal DateTime
		/// </summary>
		public Decimal64 GetByTime(DateTime time)
		{
			if (queueType == QueueType.SingleElement)
				throw new InvalidOperationException("DataQueue: Do not use this method for single element queue.");
			if (!needDateTime)
				throw new InvalidOperationException("DataQueue: It seems that you didn't specify DateTime for data elements, so this call is incorrect.");

			if (queueDates[first] > time) // if our very first valid element not older enough
				return DefaultValue;
			if (count == 0) // if queue is empty there is nothing to search
				return DefaultValue;

			int left = leftBinarySearch(time);
			int right = left + 1;

			if (queueDates[right] <= time)
				return queueBuffer[right];
			return queueBuffer[left];
		}

		/// <summary>
		/// Sets element which was stored at "step" steps ago.
		/// </summary>
		public void SetPointsAgo(int step, Decimal64 t)
		{
			if (queueType == QueueType.SingleElement)
			{
				if (step == 0)
				{
					OnPop?.Invoke(singleElement, singleElementDate);
					singleElement = t;

					if (calcStatistics)
						stat.Set(t);

					OnPush?.Invoke(singleElement, singleElementDate);
					return;
				}
				else if (step == 1)
				{
					OnPop?.Invoke(previousElement, previousElementDate);
					previousElement = t;
					OnPush?.Invoke(previousElement, previousElementDate);
					return;
				}
				throw new InvalidOperationException("TimeSeriesDataQueue: Do not use this method for single element queue.");
			}

			if (step < 0 || step >= count)
				throw new IndexOutOfRangeException("TimeSeriesDataQueue: Queue does not contain data with specified index.");

			step = (last - 1) - step;
			if (step < 0)
				step += queueBuffer.Length;

			OnPop?.Invoke(queueBuffer[last], needDateTime ? queueDates[last] : DefaultDateTime);

			if (calcStatistics)
			{
				stat.Pop(queueBuffer[step]);
				stat.Push(t);
			}

			queueBuffer[step] = t;

			OnPush?.Invoke(queueBuffer[step], needDateTime ? queueDates[step] : DefaultDateTime);
		}

		/// <summary>
		/// Sets element with given DateTime.
		/// </summary>
		public void SetByTime(DateTime time, Decimal64 t)
		{
			if (queueType == QueueType.SingleElement)
				throw new InvalidOperationException("TimeSeriesDataQueue: Do not use this method for single element queue.");

			if (!needDateTime)
				throw new InvalidOperationException("TimeSeriesDataQueue: It seems that you didn't specify DateTime for data elements, so this call is incorrect.");

			if (queueDates[first] > time)
				throw new ArgumentOutOfRangeException("TimeSeriesDataQueue: First valid element is older than specified DateTime.");

			if (count == 0)
				throw new InvalidOperationException("TimeSeriesDataQueue: Queue is empty.");

			int index = leftBinarySearch(time);

			if (queueDates[index] != time)
				if (queueDates[++index] != time)
					throw new ArgumentNullException("TimeSeriesDataQueue: There is no element with specified DateTime.");

			OnPop?.Invoke(queueBuffer[index], queueDates[index]);

			if (calcStatistics)
			{
				stat.Pop(queueBuffer[index]);
				stat.Push(t);
			}

			queueBuffer[index] = t;
			OnPush?.Invoke(queueBuffer[index], queueDates[index]);
		}

		/// <summary>
		/// Get data which was store "index" steps ago.
		/// </summary>
		/// <param name="index"> Get data which was store "index" steps ago. </param>
		/// <exception cref="InvalidOperationException">Thrown when you trying to use this method for single element queue. 
		/// Or try to use this method before adding at least one element.</exception>
		public Decimal64 this[int index]
		{
			get => GetPointsAgo(index);
			set => SetPointsAgo(index, value);
		}

		/// <summary>
		/// Return historically latest element with smaller or equal DateTime.
		/// Note: do not try to use this method if you didn't specify DateTime to data elements.
		/// </summary>
		/// <param name="time"> Returns historically latest element with smaller or equal "time". </param>
		/// <exception cref="InvalidOperationException">Thrown when you trying to use this method for single element queue. 
		/// Or try to use this method before adding at least one element. 
		/// Or if you didn't specify DateTime for data elements.</exception>
		public Decimal64 this[DateTime time]
		{
			get => GetByTime(time);
			set => SetByTime(time, value);
		}

		/// <summary>
		/// Get first(oldest) data element stored in container.
		/// Note: if there is not elements in queue it returns DefaultValue.
		/// </summary>
		public Decimal64 First
		{
			get
			{
				if (queueType == QueueType.SingleElement)
					return singleElement;

				if (count == 0)
					return DefaultValue;

				return queueBuffer[first];
			}
		}

		/// <summary>
		/// Get last data element stored in container.
		/// Note: if there is not elements in queue it returns DefaultValue.
		/// </summary>
		public Decimal64 Last
		{
			get
			{
				if (queueType == QueueType.SingleElement)
					return singleElement;

				if (count == 0)
					return DefaultValue;

				int index = last == 0 ? queueBuffer.Length - 1 : last - 1;
				return queueBuffer[index];
			}
		}

		/// <summary>
		/// Get first(oldest) data element DateTime stored in container.
		/// Note: if there is not elements in queue it returns DefaltDateTime.
		/// </summary>
		public virtual DateTime FirstDateTime
		{
			get
			{
				if (!needDateTime)
					throw new InvalidOperationException("HistoryDataQueue: DateTime not stored in. Use method put with DateTime.");

				if (queueType == QueueType.SingleElement)
					return singleElementDate;

				if (count == 0)
					return DefaultDateTime;

				return queueDates[first];
			}
		}

		/// <summary>
		/// Get last data element DateTime stored in container.
		/// Note: if there is not elements in queue it returns DefaltDateTime.
		/// </summary>
		public virtual DateTime LastDateTime
		{
			get
			{
				if (!needDateTime)
					throw new InvalidOperationException(
						"HistoryDataQueue: DateTime not stored in. Use method put with DateTime.");

				if (queueType == QueueType.SingleElement)
					return singleElementDate;

				if (count == 0)
					return DefaultDateTime;

				int index;
				if (last == 0)
					index = queueBuffer.Length - 1;
				else index = last - 1;

				return queueDates[index];
			}
		}

		/// <summary>
		/// Get previous data element stored in container.
		/// Note: if there is such not elements in queue it returns DefaltValue.
		/// </summary>
		public Decimal64 Previous
		{
			get
			{
				if (queueType == QueueType.SingleElement)
					return previousElement;

				if (count < 2)
					return DefaultValue;

				int index = last < 2 ? queueBuffer.Length + last - 2 : last - 2;
				return queueBuffer[index];
			}
		}


		/// <summary>
		/// Get the period of AutoDynamic queue.
		/// </summary>
		public TimeSpan Period => period;


		/// <summary>
		/// Get the number of elements stored in container.
		/// </summary>
		public int Count => count;

		/// <summary>
		/// Ready flag is set if and only if queue contain something.
		/// </summary>
		public bool Ready => count > 0;

		/// <summary>
		/// Return type of queue:
		/// singleElement: Queue store only one single element. (Useful as default method for indicators history storing)
		/// ManualControl: Support all typical operations specific to Dequeue. (Useful for some custom work).
		/// AutoStatic: Store all elements in sliding window specified by number elements to store. Automatically drop element when it out of the window.
		/// AutoDynamic: Store all elements in sliding window specified by time period. Automatically drop element when it out of the window.
		/// </summary>
		public QueueType Type => queueType;

		/// <summary>
		/// This flag is set if and only if queue calculates stat.
		/// </summary>
		public bool CalcStatistics => calcStatistics;

		/// <summary>
		/// This flag is set if and only if queue need DateTime to be specified for data elements, which you intend to add.
		/// </summary>
		public bool NeedDateTime => needDateTime;

		#region stat
		/// <summary>
		/// Get sum of values stored in the set.
		/// </summary>
		public Decimal64 Sum => stat.sum;

		/// <summary>
		/// Returns sum of squares.
		/// </summary>
		public Decimal64 SumOfSquares => stat.sqsum;

		/// <summary>
		/// Get sum of absolute values stored in the set.
		/// </summary>
		public Decimal64 SumOfAbsoluteValues => stat.abssum;

		/// <summary>
		/// Get estimation of distribution's First Raw Moment. 
		/// http://en.wikipedia.org/wiki/Moment_(mathematics)
		/// </summary>
		public Decimal64 FirstRawMoment => stat.sum / count;

		/// <summary>
		/// Get estimation of distribution's Second Raw Moment. 
		/// http://en.wikipedia.org/wiki/Moment_(mathematics)
		/// </summary>
		public Decimal64 SecondRawMoment => stat.sqsum / count;

		/// <summary>
		/// Get estimation of distribution's Second Central Moment.
		/// http://en.wikipedia.org/wiki/Central_moment
		/// </summary>
		public Decimal64 SecondCentralMoment
		{
			get
			{
				Decimal64 firstRawMoment = (stat.sum / count);
				return (stat.sqsum / count) - firstRawMoment * firstRawMoment;
			}
		}

		/// <summary>
		/// Get Arithmetic Mean. 
		/// http://en.wikipedia.org/wiki/Average#Arithmetic_mean
		/// </summary>
		public Decimal64 ArithmeticMean => stat.sum / count;

		/// <summary>
		/// Get Quadratic Mean. 
		/// http://en.wikipedia.org/wiki/Quadratic_mean
		/// </summary>
		public Decimal64 QuadraticMean => SqrtDecimal64(stat.sqsum / count);

		/// <summary>
		/// Get estimation of distribution's Expected Value. 
		/// http://en.wikipedia.org/wiki/Expected_value
		/// </summary>
		public Decimal64 ExpectedValue => stat.sum / count;

		/// <summary>
		/// Get distribution's Variance, same as VariancePopulation. 
		/// http://en.wikipedia.org/wiki/Variance
		/// </summary>
		public Decimal64 Variance => ((stat.sqsum - stat.sum * stat.sum / count) / count).Abs();

		/// <summary>
		/// Get distribution's Standard Deviation, same as StandardDeviatiOnPop?.Invokeulation. 
		/// http://en.wikipedia.org/wiki/Standard_Deviation
		/// </summary>
		public Decimal64 StandardDeviation => SqrtDecimal64((stat.sqsum - stat.sum * stat.sum / count).Abs() / count);

		/// <summary>
		/// Get distribution's Variance. 
		/// http://en.wikipedia.org/wiki/Variance
		/// </summary>
		public Decimal64 VariancePopulation => ((stat.sqsum - stat.sum * stat.sum / count) / count).Abs();

		/// <summary>
		/// Get estimation distribution's Variance. 
		/// http://en.wikipedia.org/wiki/Variance
		/// </summary>
		public Decimal64 VarianceSample => ((stat.sqsum - stat.sum * stat.sum / count) / (count - 1)).Abs();

		/// <summary>
		/// Get distribution's Standard Deviation. 
		/// http://en.wikipedia.org/wiki/Standard_Deviation
		/// </summary>
		public Decimal64 StandardDeviatiOnPopulation => SqrtDecimal64(((stat.sqsum - stat.sum * stat.sum / count) / count).Abs());

		/// <summary>
		/// Get estimation of distribution's Standard Deviation. 
		/// http://en.wikipedia.org/wiki/Standard_Deviation
		/// </summary>
		public Decimal64 StandardDeviationSample => SqrtDecimal64(((stat.sqsum - stat.sum * stat.sum / count) / (count - 1)).Abs());

		/// <summary>
		/// Get Coefficient Of Variation of stored set of values. 
		/// http://en.wikipedia.org/wiki/Coefficient_of_variation
		/// </summary>
		public Decimal64 CoefficientOfVariation => SqrtDecimal64(((stat.sqsum - stat.sum * stat.sum / count) / count).Abs()) / (stat.sum / count);

		#endregion stat

		/// <summary>
		/// Copies the elements of the DataQueue to a new array.
		/// </summary>
		/// <returns> An array containing copies of the elements of the MultipleValue. </returns>
		public Decimal64[] ToArray()
		{
			if (queueType == QueueType.SingleElement)
			{
				if (count > 0)
					return new Decimal64[] { singleElement };
				else
					return new Decimal64[0];
			}

			Decimal64[] array = new Decimal64[count];

			for (int i = first, j = 0; j < count; i++, j++)
			{
				if (i == queueBuffer.Length)
					i = 0;

				array[j] = queueBuffer[i];
			}
			return array;
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns> An System.Collections.IEnumerator object that can be used to iterate through the collection. </returns>
		public IEnumerator GetEnumerator()
		{
			return new DataQueueEnumerator(this);
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns> An System.Collections.IEnumerator object that can be used to iterate through the collection. </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DataQueueEnumerator(this);
		}

		/// <summary>
		/// Makes full clear of this instance.
		/// </summary>
		public void Clear()
		{
			first = last = count = 0;
		}

		/// <summary>
		/// Makes full copy of this instance.
		/// </summary>
		/// <returns> Copy of this instance. </returns>
		public DataQueue Clone()
		{
			DataQueue queue = new DataQueue(queueType == QueueType.SingleElement ? 1 : queueBuffer.Length, calcStatistics, needDateTime);
			queue.queueType = queueType;
			CopyTo(queue);

			return queue;
		}

		/// <summary>
		/// Sets internal state to destination object.
		/// </summary>
		/// <param name="destination"> Destination object to put contents. </param>
		public void CopyTo(DataQueue destination)
		{
			if (OnPush != null)
				destination.OnPush = (Action<Decimal64, DateTime>)OnPush.Clone();

			if (OnPop != null)
				destination.OnPop = (Action<Decimal64, DateTime>)OnPop.Clone();

			if (queueBuffer != null)
			{
				if (destination.queueBuffer.Length < queueBuffer.Length)
				{
					destination.queueBuffer = new Decimal64[queueBuffer.Length << 1];
				}
				Array.Copy(queueBuffer, 0, destination.queueBuffer, 0, queueBuffer.Length);
			}
			if (queueDates != null)
			{
				if (destination.queueDates == null)
					destination.queueDates = (DateTime[])queueDates.Clone();
				else
					Array.Copy(queueDates, 0, destination.queueDates, 0, queueDates.Length);
			}

			destination.previousElement = previousElement;
			destination.previousElementDate = previousElementDate;
			destination.singleElement = singleElement;
			destination.singleElementDate = singleElementDate;

			destination.first = first;
			destination.last = last;
			destination.count = count;

			destination.period = period;
			destination.needDateTime = needDateTime;
			destination.queueType = queueType;

			destination.stat.sqsum = stat.sqsum;
			destination.stat.abssum = stat.abssum;
			destination.stat.sum = stat.sum;
		}


		private Decimal64 SqrtDecimal64(Decimal64 value)
		{
			double temp = value.ToDouble();
			temp = Math.Sqrt(temp);
			return Decimal64.FromDouble(temp);
		}

		private struct DataQueueEnumerator : IEnumerator
		{
			int offset;
			DataQueue queue;

			public DataQueueEnumerator(DataQueue queue)
			{
				this.queue = queue;
				offset = -1;
			}

			public Decimal64 Current
			{
				get
				{
					return queue.queueBuffer[(queue.first + offset) % queue.queueBuffer.Length];
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return queue.queueBuffer[(queue.first + offset) % queue.queueBuffer.Length];
				}
			}

			public bool MoveNext()
			{
				if (offset < queue.count - 1)
				{
					offset++;
					return true;
				}
				return false;
			}

			public void Reset()
			{
				offset = -1;
			}
		}
	}
}