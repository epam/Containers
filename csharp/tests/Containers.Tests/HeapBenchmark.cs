using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;
namespace EPAM.Deltix.Containers.Tests{
	public class LongComparer : Comparer<long> {
		public override int Compare(long x, long y)
		{
			if (x < y) return -1;
			if (x > y) return 1;
			return 0;
		}

	}


	public class ListHeapWithIndices<TValue>
	{

	//	HeapEntry[] elements;
		TValue[] elementsValue;
		int[] elementsKey;
		Comparer<TValue> comparer;
		ReverseComparer reverseComparer;
		int Count, head;
		int[] keysPosition;
		/// <summary>
		/// Heap's entry.
		/// </summary>
		public struct HeapEntry
		{
			/// <summary>
			/// Our key of entry.
			/// </summary>
			public int Key { get; internal set; }
			/// <summary>
			/// Entry's value.
			/// </summary>
			public TValue Value
			{
				get; internal set;
			}
			internal HeapEntry(int key, TValue value)
			{
				this.Key = key;
				this.Value = value;
			}
		}


		public int GetNextFreeKey()
		{
			int key = Math.Abs(head);
			if (key >= keysPosition.Length)
			{
				int oldLength = keysPosition.Length;
				Array.Resize(ref keysPosition, keysPosition.Length << 1);
				for (int i = oldLength; i < keysPosition.Length; ++i) keysPosition[i] = -i - 2;
			}
			head = -(keysPosition[head] + 1);
			return key;

		}

		public ListHeapWithIndices(Comparer<TValue> comparer = null)
		{
			if (comparer == null) this.comparer = new DefaultComparer<TValue>();
			else this.comparer = comparer;
			Array.Resize(ref elementsKey, 100);
			Array.Resize(ref elementsValue, 100);

			Array.Resize(ref keysPosition, 100);
			for (int i = 0; i < 100; ++i) keysPosition[i] = -i - 2;
			reverseComparer = new ReverseComparer(comparer);
			
			
		}

		public class ReverseComparer : Comparer<TValue>
		{
			public Comparer<TValue> comparer;

			public ReverseComparer(Comparer<TValue> comparer)
			{
				this.comparer = comparer;
			}

			public override int Compare(TValue x, TValue y)
			{
				return -comparer.Compare(x, y);
			}
		}


		public int Add(TValue x)
		{
			if (Count == elementsKey.Length)
			{
				Array.Resize(ref elementsKey, elementsKey.Length * 2);
				Array.Resize(ref elementsValue, elementsValue.Length * 2);
			}
	
			int l = 0;
			int r = Count - 1;			
			while (l < r)
			{
				int c = (l + r) / 2;
				if (comparer.Compare(x, elementsValue[c]) < 0) l = c + 1; else r = c;
			}
			if (comparer.Compare(x, elementsValue[l]) < 0) l++;
			
			for (int k = Count; k > l; k--)
			{
				elementsValue[k] = elementsValue[k - 1];
				elementsKey[k] = elementsKey[k - 1];
				keysPosition[elementsKey[k]] = k;
			}
			int key = GetNextFreeKey();
			elementsKey[l] = key;
			elementsValue[l] = x;
			keysPosition[key] = l;
			Count++;
			return key;
		}

		public bool Remove(int key)
		{
			int index = keysPosition[key];
			if (index < 0) return false;
			int last = head;
			head = elementsKey[index];
			keysPosition[head] = -last - 1;
			for (int i = index; i < Count; i++)
			{
				elementsKey[i] = elementsKey[i + 1];
				elementsValue[i] = elementsValue[i + 1];
				keysPosition[elementsKey[i]] = i; 
			}
			Count--;
			return true;

		}

		public HeapEntry Pop()
		{
			if (Count == 0) throw new Exception("");
			Count--;
			int last = head;
			head = elementsKey[Count];
			keysPosition[head] = -last - 1;
			return new HeapEntry(elementsKey[Count], elementsValue[Count]);
		}

		public HeapEntry Peek()
		{
			if (Count == 0) throw new Exception("");
			return new HeapEntry(elementsKey[Count - 1], elementsValue[Count - 1]);
		}


	}

	public class ListHeap<TValue>
	{
		TValue[] elements;
		Comparer<TValue> comparer;
		int Count;
		public ListHeap(Comparer<TValue> comparer = null) {
			if (comparer == null) this.comparer = new DefaultComparer<TValue>();
			else this.comparer = comparer;
			Array.Resize(ref elements, 100);
			Count = 0;
		}


		public void Add(TValue x)
		{
			if (Count == elements.Length) Array.Resize(ref elements, elements.Length * 2);
			int l = 0;
			int r = Count - 1;
			while (l < r)
			{
				int c = (l + r) / 2;
				if (comparer.Compare(x, elements[c]) < 0) l = c + 1; else r = c;
			}
			if (comparer.Compare(x, elements[l]) < 0) l++;
			for (int k = Count; k > l; k--) elements[k] = elements[k - 1];
			elements[l] = x;
			Count++;
		}

		public TValue Pop()
		{
			if (Count == 0) throw new Exception("");
			Count--;
			return elements[Count];
		}

		public TValue ModifyTop(TValue value)
		{
			TValue returnValue = elements[Count - 1];
			Count--;
			Add(value);
			return returnValue;
		}

		public TValue Peek()
		{
			if (Count == 0) throw new Exception("");
			return elements[Count - 1];
		}

	}
	[TestFixture]
	class HeapBenchmark
	{
		[TestCase(1000000, 10000)]
		[TestCase(1000000, 1000)]
		[TestCase(1000000, 800)]
		[TestCase(1000000, 25)]
		[TestCase(1000000, 10)]
		[TestCase(1000000, 5)]
		[TestCase(1000000, 2)]
		public void DoubleHeapTest(int numberOfIterations, int heapSize)
		{
			bool needPop = false;
			List<long> actions = new List<long>();
			Random rand = new Random(55);
			Heap<long> heap = new Heap<long>(new LongComparer());
			ListHeap<long> list = new ListHeap<long>(new LongComparer());
			for (int i = 0; i < heapSize; ++i)
			{
				int x = rand.Next() % 100;
				list.Add(x);
				heap.Add(x);
				actions.Add(x);
			}
			for (int i = 0; i < numberOfIterations; ++i)
			{
				if (i < heapSize) actions[i] = actions[i] + rand.Next() % 10000000;
				else actions.Add(actions[i - heapSize] + rand.Next() % 10000000);
			}
			long heapSum = 0;
			long listSum = 0;
			Stopwatch timer = new Stopwatch();
			timer.Start();
			for (int i = 0; i < numberOfIterations; ++i)
			{
				list.Add(actions[i]);
				listSum += list.Pop();
		
			}
			timer.Stop();
			System.Console.WriteLine(listSum + " " + timer.ElapsedMilliseconds);
			timer.Restart();
			for (int i = 0; i < numberOfIterations; ++i)
			{
				
				if (!needPop) heap.Add(actions[i]); else
				{
					needPop = false;
					heap.ModifyTop(actions[i]);
				}
				if (needPop) heap.Pop();
				needPop = true;
				heapSum += heap.Peek;
			}

			timer.Stop();
			System.Console.WriteLine(heapSum + " " + timer.ElapsedMilliseconds);
			Assert.AreEqual(heapSum, listSum);
		}


		[TestCase(1000000, 10000)]
		[TestCase(1000000, 1000)]
		[TestCase(1000000, 800)]
		[TestCase(1000000, 25)]
		[TestCase(1000000, 10)]
		[TestCase(1000000, 5)]
		[TestCase(1000000, 2)]
		public void DoubleHeapModifyTopTest(int numberOfIterations, int heapSize)
		{
			List<long> actions = new List<long>();
			Random rand = new Random(55);
			Heap<long> heap = new Heap<long>(new LongComparer());
			ListHeap<long> list = new ListHeap<long>(new LongComparer());
			for (int i = 0; i < heapSize; ++i)
			{
				int x = rand.Next() % 100;
				list.Add(x);
				heap.Add(x);
				actions.Add(x);
			}
			for (int i = 0; i < numberOfIterations; ++i)
			{
				if (i < heapSize) actions[i] = actions[i] + rand.Next() % 10000000;
				else actions.Add(actions[i - heapSize] + rand.Next() % 10000000);
			}
			long heapSum = 0;
			long listSum = 0;
			Stopwatch timer = new Stopwatch();
			timer.Start();
			for (int i = 0; i < numberOfIterations; ++i)
			{
				listSum += list.ModifyTop(actions[i]);

			}
			timer.Stop();
			System.Console.WriteLine(listSum + " " + timer.ElapsedMilliseconds);
			timer.Restart();
			for (int i = 0; i < numberOfIterations; ++i)
			{
				heapSum += heap.ModifyTop(actions[i]);
			}

			timer.Stop();
			System.Console.WriteLine(heapSum + " " + timer.ElapsedMilliseconds);
			Assert.AreEqual(heapSum, listSum);
		}


		[TestCase(1000000, 10000)]
		[TestCase(1000000, 1000)]
		[TestCase(1000000, 700)]
		[TestCase(1000000, 25)]
		[TestCase(1000000, 10)]
		[TestCase(1000000, 5)]
		[TestCase(1000000, 2)]
		public void HeapWithIndicesTest(int numberOfIterations, int heapSize)
		{
			List<int> actions = new List<int>();
			List<int> actions1 = new List<int>();
			Random rand = new Random(55);
			HeapWithIndices<long> heap = new HeapWithIndices<long>(new LongComparer());
			ListHeapWithIndices<long> list = new ListHeapWithIndices<long>(new LongComparer());
			System.Collections.Generic.HashSet<int> listKeys = new System.Collections.Generic.HashSet<int>();
			System.Collections.Generic.HashSet<int> heapKeys = new System.Collections.Generic.HashSet<int>();
			for (int i = 0; i < heapSize; ++i)
			{
				int x = rand.Next() % 100;
				list.Add(x);
				heap.Add(x);
				actions.Add(x);
				actions1.Add(x);
			}
			for (int i = 0; i < numberOfIterations; ++i)
			{
				if (i < heapSize) actions[i] = actions[i] + rand.Next() % 10000000;
				else actions.Add(actions[i - heapSize] + rand.Next() % 10000000);
				if (i < heapSize) actions1[i] = actions1[i] + rand.Next() % 10000000;
				else actions1.Add(actions1[i - heapSize] + rand.Next() % 10000000);

			}
			long heapSum = 0;
			long listSum = 0;
			Stopwatch timer = new Stopwatch();
			timer.Start();
			for (int i = 0; i < actions.Count; ++i)
			{
				ListHeapWithIndices<long>.HeapEntry heapEntry = list.Pop();
				listSum += heapEntry.Value;
				listKeys.Remove(heapEntry.Key);
				listKeys.Add(list.Add(actions[i]));
				if (i % 10 == 0)
				{
					int key = listKeys.First();
					listKeys.Remove(key);
					list.Remove(key);
					listKeys.Add(list.Add(actions1[i]));					
				}
			}

			timer.Stop();

			System.Console.WriteLine(timer.ElapsedMilliseconds);

			timer.Reset();
			timer.Start();
			for (int i = 0; i < actions.Count; ++i)
			{
				HeapWithIndices<long>.HeapEntry heapEntry = heap.Pop();
				heapSum += heapEntry.Value;
				heapKeys.Remove(heapEntry.Key);
				heapKeys.Add(heap.Add(actions[i]));
				if (i % 10 == 0)
				{
					int key = heapKeys.First();
					heapKeys.Remove(key);
					heap.Remove(key);
					heapKeys.Add(heap.Add(actions1[i]));
				}
			}

			timer.Stop();
			System.Console.WriteLine(timer.ElapsedMilliseconds);

			Assert.AreEqual(heapSum, listSum);
		}


	}
}
