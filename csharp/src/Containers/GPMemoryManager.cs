using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Geometric progression memory manager.
	/// </summary>
	/// <typeparam name="T">Thpe of object to manage.</typeparam>
	public class GPMemoryManager<T> : IMemoryManager<T> where T : new()
	{
		T[] freeObjects;
		Int32 numberOfFreeObjects = 0;
		Int32 totalObjectsCount = 0;
		Double coefficient;

		/// <summary>
		/// Default maximal value of number object increment.
		/// </summary>
		public Int32 MaxIncrement
		{
			get; set;
		} = 1000000;

		/// <summary>
		/// Creates new geometric progression memory manager.
		/// </summary>
		/// <param name="capacity">Initial pool capacity.</param>
		/// <param name="coefficient">Scale factor of geometric progression.</param>
		public GPMemoryManager(Int32 capacity, Double coefficient)
		{
			if (coefficient <= 1.0)
				throw new ArgumentException("coefficient");

			this.coefficient = coefficient;
			freeObjects = new T[capacity];
			totalObjectsCount = capacity;
			for (int i = 0; i < capacity; ++i)
				freeObjects[i] = new T();
		}

		/// <summary>
		/// Creates new geometric progression memory manager.
		/// </summary>
		public GPMemoryManager()
			: this(0x10, 2)
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
					freeObjects[i] = new T();

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
