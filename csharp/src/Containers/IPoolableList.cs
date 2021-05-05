using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Interface for poolable indexed list.
	/// </summary>
	/// <typeparam name="T">List elements type</typeparam>
	public interface IPoolableIndexedList<T> : IReadOnlyList<T>
	{
		/// <summary>
		/// Capacity of list.
		/// </summary>
		int Capacity { get; }

		/// <summary>
		/// Clear this list.
		/// </summary>
		void Clear();

		/// <summary>
		/// Change capacity of list.
		/// </summary>
		/// <param name="capacity">New capacity of list</param>
		/// <param name="type">Type of new elements (should be inheritor of T)</param>
		void HintCapacity<Q>(int capacity) where Q : T;

		/// <summary>
		/// Change size of list.
		/// </summary>
		/// <param name="count">New count of list</param>
		/// <param name="type">Type of new elements (should be inheritor of T)</param>
		void SetSize<Q>(int count) where Q : T;

		/// <summary>
		/// Append new element to list.
		/// </summary>
		/// <param name="type">Type of new element (should be inheritor of T)</param>
		/// <returns>Appended element</returns>
		Q Append<Q>() where Q : T;
	}

}
