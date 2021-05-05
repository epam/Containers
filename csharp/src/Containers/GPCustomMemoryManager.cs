using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Geometric progression memory manager with custom creator.
	/// </summary>
	/// <typeparam name="T">Thpe of object to manage.</typeparam>
	public class GPCustomMemoryManager<T> : IMemoryManager<T> where T : class
	{
		T[] freeObjects;
		Int32 numberOfFreeObjects = 0;
		Int32 totalObjectsCount = 0;
		Double coefficient;
		Func<T> creator;

		/// <summary>
		/// Default maximal value of number object increment.
		/// </summary>
		public Int32 MaxIncrement
		{
			get; set;
		} = 1000000;

		/// <summary>
		/// Creates new geometric progression memory manager with custom creator
		/// </summary>
		/// <param name="creator">Custom creator</param>
		/// <param name="capacity">Initial pool capacity.</param>
		/// <param name="coefficient">Scale factor of geometric progression.</param>
		public GPCustomMemoryManager(Func<T> creator, Int32 capacity, Double coefficient)
		{
			if (coefficient <= 1.0)
				throw new ArgumentException("coefficient");
			this.creator = creator;
			this.coefficient = coefficient;
			freeObjects = new T[capacity];
			totalObjectsCount = capacity;
			for (int i = 0; i < capacity; ++i)
				freeObjects[i] = creator();
		}

		/// <summary>
		/// Creates new geometric progression memory manager.
		/// </summary>
		/// <param name="creator">Custom creator</param>
		public GPCustomMemoryManager(Func<T> creator)
			: this(creator, 0x10, 2)
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
				Int32 length = freeObjects.Length;
				Int32 newLength = Math.Min(MaxIncrement, (Int32)(freeObjects.Length * coefficient));
				freeObjects = new T[newLength];
				numberOfFreeObjects = newLength;

				for (int i = 0; i < numberOfFreeObjects; ++i)
					freeObjects[i] = creator();

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
		public Int32 TotalObjectsCount { get { return freeObjects.Length; } }
	}
}
