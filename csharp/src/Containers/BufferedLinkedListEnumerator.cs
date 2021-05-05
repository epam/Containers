using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPAM.Deltix.Containers{
	internal class BufferedLinkedListEnumerator<T> : IEnumerator<T>
	{
		int key;
		BufferedLinkedList<T> list;
		bool isStartedEnumeration;
		internal BufferedLinkedListEnumerator<T> Init(BufferedLinkedList<T> list) {
			this.list = list;
			isStartedEnumeration = false;
			return this;
		} 

		public T Current
		{
			get { return list[key]; }
		}

		public void Dispose()
		{
			list.enumerators.Delete(this);
		}

		object System.Collections.IEnumerator.Current
		{
			get { return list[key]; }
		}

		public bool MoveNext()
		{
			if (!isStartedEnumeration)
			{
				key = list.FirstKey;
				isStartedEnumeration = true;
				return key >= 0;
			}
			if (list.Next(key) >= 0)
			{
				key = list.Next(key);
				return true;
			}
			else return false;
		}

		public void Reset()
		{
			key = list.FirstKey;
		}
	}
}
