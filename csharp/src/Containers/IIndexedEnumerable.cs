﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Allow to enumerate and indexing
	/// </summary>
	/// <typeparam name="T">Type</typeparam>
	public interface IIndexedEnumerable<T> : IEnumerable<T>, ICopyable<T>, ICloneable
	{
		/// <summary>
		/// Indexing
		/// </summary>
		/// <param name="index">index</param>
		/// <returns>object with index</returns>
		T this[int index] { get; }
		/// <summary>
		/// Number of element
		/// </summary>
		int Count { get; }
	}
}