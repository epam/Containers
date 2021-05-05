using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Allow to copy Type to
	/// </summary>
	/// <typeparam name="Type">Type</typeparam>
	public interface ICopyable<Type>
	{
		/// <summary>
		/// Copy object to another object
		/// </summary>
		/// <param name="copy">another</param>
		void CopyTo(Type copy);
	}
}
