using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Represents memory manager pool.
	/// </summary>
	/// <typeparam name="T">Class type to manage.</typeparam>
	public interface IMemoryManager<T>
	{
		/// <summary>
		/// Number of free objects in pool.
		/// </summary>
		Int32 FreeObjectsCount { get; }
		/// <summary>
		/// Total number of objects in pool free + reserverd.
		/// </summary>
		Int32 TotalObjectsCount { get; }
		/// <summary>
		/// Reserve new instance of an object.
		/// </summary>
		/// <returns>Instance of an object</returns>
		T New();
		/// <summary>
		/// Free instance of an object and return it to the pool.
		/// </summary>
		/// <param name="obj">Instance of an object to free.</param>
		void Delete(T obj);
	}
}
