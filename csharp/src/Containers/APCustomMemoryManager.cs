using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Arithmetic progression memory manager with custom creator.
	/// </summary>
	/// <typeparam name="T">Type of object to manage.</typeparam>
	public class APCustomMemoryManager<T> : IMemoryManager<T> where T : class
	{
		T[] freeObjects;
		Int32 numberOfFreeObjects;
		Int32 increment;
		Int32 totalObjectsCount;
		Func<T> creator;

		/// <summary>
		/// Creates new arithmetic progression memory manager with custom creator
		/// </summary>
		/// <param name="creator">Custom creator</param>
		/// <param name="capacity">Initial pool capacity.</param>
		/// <param name="increment">Pool size increment.</param>
		public APCustomMemoryManager(Func<T> creator, Int32 capacity, Int32 increment)
		{
			if (increment > capacity)
				throw new ArgumentException("incrment");
			this.creator = creator;
			totalObjectsCount = numberOfFreeObjects = capacity;
			this.increment = increment;
			freeObjects = new T[capacity];
			for (int i = 0; i < capacity; ++i)
				freeObjects[i] = creator();
		}

		/// <summary>
		/// Creates new arithmetic progression memory manager with custom creator.
		/// </summary>
		/// <param name="creator">Custom creator</param>
		public APCustomMemoryManager(Func<T> creator)
			: this(creator, 10, 5)
		{
		}

		/// <summary>
		/// Reserve new instance of an object.
		/// </summary>
		/// <returns>Instance of an object</returns>
		public T New()
		{
			if (numberOfFreeObjects == 0)
			{
				for (int i = 0; i < increment; ++i)
					freeObjects[i] = creator();
				numberOfFreeObjects = increment;
				totalObjectsCount += increment;
			}
			numberOfFreeObjects--;
			return freeObjects[numberOfFreeObjects];
		}

		/// <summary>
		/// Free instance of an object and return it to the pool.
		/// </summary>
		/// <param name="obj">Instance of an object to free.</param>
		public void Delete(T obj)
		{
			if (numberOfFreeObjects == freeObjects.Length)
			{
				Array.Resize(ref freeObjects, freeObjects.Length << 1);
			}
			freeObjects[numberOfFreeObjects] = obj;
			numberOfFreeObjects++;
		}

		/// <summary>
		/// Number of free objects in pool.
		/// </summary>
		public Int32 FreeObjectsCount { get { return numberOfFreeObjects; } }

		/// <summary>
		/// Total number of objects in pool free + reserverd.
		/// </summary>
		public Int32 TotalObjectsCount { get { return totalObjectsCount; } }
	}
}
