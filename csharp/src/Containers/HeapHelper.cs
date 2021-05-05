using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Helper for work with internal fields of heaps.
	/// </summary>
	public class HeapHelper
	{

		/// <summary>
		/// Get isHeap field from heap(true if it is real heap, not sorted list).
		/// </summary>
		/// <typeparam name="TValue">TValue.</typeparam>
		/// <param name="heap">Heap</param>
		/// <returns>true if it is real heap, not sorted list</returns>
		public static bool IsHeap<TValue>(Heap<TValue> heap)
		{
			return heap.isHeap;
		}

		/// <summary>
		/// Get isHeap field from heap(true if it is real heap, not sorted list).
		/// </summary>
		/// <typeparam name="TValue">TValue.</typeparam>
		/// <param name="heap">Heap</param>
		/// <returns>true if it is real heap, not sorted list</returns>
		public static bool IsHeap<TValue>(HeapWithIndices<TValue> heap)
		{
			return heap.isHeap;
		}

		/// <summary>
		/// Get isHeap field from heap(true if it is real heap, not sorted list).
		/// </summary>
		/// <typeparam name="TValue">TValue.</typeparam>
		/// <typeparam name="TKey">TKey</typeparam>
		/// <param name="heap">Heap</param>
		/// <returns>true if it is real heap, not sorted list</returns>
		public static bool IsHeap<TValue, TKey>(HeapWithIndices<TValue, TKey> heap)
		{
			return heap.isHeap;
		}

		/// <summary>
		/// Get isHeap field from heap(true if it is real heap, not sorted list).
		/// </summary>
		/// <typeparam name="TValue">TValue.</typeparam>
		/// <typeparam name="TAttachments">TAttachments</typeparam>
		/// <param name="heap">Heap</param>
		/// <returns>true if it is real heap, not sorted list</returns>
		public static bool IsHeap<TValue, TAttachments>(HeapWithAttachments<TValue, TAttachments> heap)
		{
			return heap.isHeap;
		}

		/// <summary>
		/// Get internal array with heap's entries
		/// </summary>
		/// <typeparam name="TValue">TValue</typeparam>
		/// <typeparam name="TKey">TKey</typeparam>
		/// <param name="heap">Heap</param>
		/// <returns>Internal array with heap's entries</returns>
		public static HeapWithIndices<TValue, TKey>.HeapEntry[] GetEntries<TValue, TKey>(HeapWithIndices<TValue, TKey> heap)
		{
			return heap.elements;
		}

		/// <summary>
		/// Get internal array with heap's entries
		/// </summary>
		/// <typeparam name="TValue">TValue</typeparam>
		/// <param name="heap">Heap</param>
		/// <returns>Internal array with heap's entries</returns>
		public static HeapWithIndices<TValue>.HeapEntry[] GetEntries<TValue>(HeapWithIndices<TValue> heap)
		{
			return heap.elements;
		}

		/// <summary>
		/// Get internal array with heap's entries
		/// </summary>
		/// <typeparam name="TValue">TValue</typeparam>
		/// <typeparam name="TAttachments">TAttachments</typeparam>
		/// <param name="heap">Heap</param>
		/// <returns>Internal array with heap's entries</returns>
		public static HeapWithAttachments<TValue, TAttachments>.HeapEntry[] GetEntries<TValue, TAttachments>(HeapWithAttachments<TValue, TAttachments> heap)
		{
			return heap.elements;
		}

		/// <summary>
		/// Get internal array with heap's entries
		/// </summary>
		/// <typeparam name="TValue">TValue</typeparam>
		/// <param name="heap">Heap</param>
		/// <returns>Internal array with heap's entries</returns>
		public static TValue[] GetEntries<TValue>(Heap<TValue> heap)
		{
			return heap.elements;
		}

	}
}
