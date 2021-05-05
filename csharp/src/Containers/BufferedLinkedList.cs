using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace EPAM.Deltix.Containers{

	internal struct ListEntry<T>
	{
		internal int prev;
		internal int next;
		internal T value;
	}

	/// <summary>
	/// Implementation of LinkedList analog. 
	/// But our data structure use it's own memory manager to optimize performance.
	/// </summary>
	/// <typeparam name="T">Type of elements stored in list.</typeparam>
	public class BufferedLinkedList<T> : ILinkedList<T>, ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
	{
		internal APMemoryManager<BufferedLinkedListEnumerator<T>> enumerators;
		const int ReferenceToNothing = -2;

		const int EmptyListPointers = -1;


		/// <summary>
		/// Array of stored elements and links
		/// </summary>
		ListEntry<T>[] listEntries;

		/// <summary>
		/// Stack of free positions.
		/// </summary>
		protected int[] _stackFree;
		/// <summary>
		/// First element in list.
		/// </summary>
		protected int _first;
		/// <summary>
		/// Last element in list.
		/// </summary>
		protected int _last;
		/// <summary>
		/// Number of elements sored in list.
		/// </summary>
		protected int _count;
		/// <summary>
		/// Capacity of internal buffers.
		/// </summary>
		protected int _capacity;

		/// <summary>
		/// Indicates whether list can collapse it's capacity or not.
		/// </summary>
		protected bool _collapsable = false;

		/// <summary>
		/// Creates new buffered list with default capacity (16 elements).
		/// </summary>
		public BufferedLinkedList()
			: this(0x10)
		{
		}

		/// <summary>
		/// Creates new buffered list with user specfied capacity.
		/// </summary>
		/// <param name="capacity">Initial capacity of internal buffers.</param>
		public BufferedLinkedList(int capacity)
		{
			_capacity = capacity;
			listEntries = new ListEntry<T>[capacity];
			_stackFree = new int[capacity];

			for (int i = 0; i < capacity; i++)
			{
				_stackFree[i] = i;
				listEntries[i].prev = ReferenceToNothing;
			}

			_first = EmptyListPointers;
			_last = EmptyListPointers;
			_count = 0;
		}

		/// <summary>
		/// This method double size of all internal buffers.
		/// </summary>
		protected void IncreaseSize()
		{
			int previousCapacity = _capacity;

			_capacity = _capacity << 1;
			Array.Resize(ref _stackFree, _capacity);
			Array.Resize(ref listEntries, _capacity);

			for (int i = previousCapacity; i < _capacity; i++)
			{
				_stackFree[i] = i;
				listEntries[i].prev = ReferenceToNothing;
			}
		}

		/// <summary>
		/// This method decrease size of all internal buffers.
		/// </summary>
		protected void DecreaseSize()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Add new first element in linked list.
		/// Note: unique index are reusable.
		/// </summary>
		/// <param name="obj">New data element to store in list.</param>
		/// <returns>Unique index (for random access) of stored element.</returns>
		public int AddFirst(T obj)
		{
			if (_count == _capacity)
				IncreaseSize();

			int index = _stackFree[_count++];
			listEntries[index].value = obj;
			listEntries[index].next = _first;
			listEntries[index].prev = EmptyListPointers;

			if (_first != EmptyListPointers)
				listEntries[_first].prev = index;

			_first = index;
			if (_last == EmptyListPointers)
				_last = index;

			return index;
		}

		/// <summary>
		/// Add new last element in linked list.
		/// Note: unique index are reusable.
		/// </summary>
		/// <param name="obj">New data element to store in list.</param>
		/// <returns>Unique index (for random access) of stored element.</returns>
		public int AddLast(T obj)
		{
			if (_count == _capacity)
				IncreaseSize();

			int index = _stackFree[_count++];
			listEntries[index].value = obj;
			listEntries[index].next = EmptyListPointers;
			listEntries[index].prev = _last;

			if (_last != EmptyListPointers)
				listEntries[_last].next = index;

			_last = index;
			if (_first == EmptyListPointers)
				_first = index;

			return index;
		}

		/// <summary>
		/// Remove first element from list.
		/// </summary>
		public void RemoveFirst()
		{
			if (_first != EmptyListPointers)
			{
				int newFirst = listEntries[_first].next;
				listEntries[_first].prev = ReferenceToNothing;

				_stackFree[--_count] = _first;

				if (newFirst != EmptyListPointers)
					listEntries[newFirst].prev = EmptyListPointers;

				_first = newFirst;

				if (_count == 0)
					_last = EmptyListPointers;
			}
		}

		/// <summary>
		/// Remove last element from list.
		/// </summary>
		public void RemoveLast()
		{
			if (_last != EmptyListPointers)
			{
				int newLast = listEntries[_last].prev;
				listEntries[_last].prev = ReferenceToNothing;

				_stackFree[--_count] = _last;

				if (newLast != EmptyListPointers)
					listEntries[newLast].next = EmptyListPointers;

				_last = newLast;

				if (_count == 0)
					_first = EmptyListPointers;
			}
		}

		/// <summary>
		/// Remove element with specified unique index (key).
		/// Note: unique index are reusable.
		/// </summary>
		/// <param name="key">Unique index of element to remove.</param>
		public void Remove(int key)
		{
			if (listEntries[key].prev == ReferenceToNothing)
				throw new ArgumentException("There is no such element!");

			_stackFree[--_count] = key;

			int prev = listEntries[key].prev;
			int next = listEntries[key].next;

			if (prev != EmptyListPointers)
				listEntries[prev].next = next;
			else
				_first = next;

			if (next != EmptyListPointers)
				listEntries[next].prev = prev;
			else
				_last = prev;

			listEntries[key].prev = ReferenceToNothing;
		}

		/// <summary>
		/// Add new element after element with specified index.
		/// Note: unique index are reusable.
		/// </summary>
		/// <param name="key">Index of previous for new element.</param>
		/// <param name="obj">New data element to store in list.</param>
		/// <returns>Unique index (for random access) of stored element.</returns>
		public int AddAfter(int key, T obj)
		{
			if (listEntries[key].prev == ReferenceToNothing)
				throw new ArgumentException("There is no such element!");

			if (_count == _capacity)
				IncreaseSize();

			int index = _stackFree[_count++];
			listEntries[index].value = obj;

			int next = listEntries[index].next = listEntries[key].next;
			listEntries[index].prev = key;
			listEntries[key].next = index;

			if (next != EmptyListPointers)
				listEntries[next].prev = index;
			else
				_last = index;

			return index;
		}

		/// <summary>
		/// Add new element before element with specified index.
		/// Note: unique index are reusable.
		/// </summary>
		/// <param name="key">Index of next for new element.</param>
		/// <param name="obj">New data element to store in list.</param>
		/// <returns>Unique index (for random access) of stored element.</returns>
		public int AddBefore(int key, T obj)
		{
			if (listEntries[key].prev == ReferenceToNothing)
				throw new ArgumentException("There is no such element!");

			if (_count == _capacity)
				IncreaseSize();

			int index = _stackFree[_count++];
			listEntries[index].value = obj;

			int prev = listEntries[index].prev = listEntries[key].prev;
			listEntries[index].next = key;
			listEntries[key].prev = index;

			if (prev != EmptyListPointers)
				listEntries[prev].next = index;
			else
				_first = index;

			return index;
		}

		/// <summary>
		/// Get element previous to element which unique index specified.
		/// With index validation.
		/// </summary>
		/// <param name="key">Index of element which previous are desired.</param>
		/// <returns>Index of previous element.</returns>
		public int PrevProtected(int key)
		{
			if (key == EmptyListPointers)
				return _last;

			if (listEntries[key].prev == ReferenceToNothing)
				throw new ArgumentException("There is no such element!");

			return listEntries[key].prev;
		}

		/// <summary>
		/// Get element next to element which unique index specified.
		/// With index validation.
		/// </summary>
		/// <param name="key">Index of element which next are desired.</param>
		/// <returns>Index of previous element.</returns>
		public int NextProtected(int key)
		{
			if (key == EmptyListPointers)
				return _first;

			if (listEntries[key].prev == ReferenceToNothing)
				throw new ArgumentException("There is no such element!");

			return listEntries[key].next;
		}

		/// <summary>
		/// Returns true if and only if there is element with specified key.
		/// </summary>
		/// <param name="key">Key to verify.</param>
		/// <returns>True if and only if there is element with specified key.</returns>
		public Boolean ContainsKey(Int32 key)
		{
			return listEntries[key].prev != ReferenceToNothing;
		}

		/// <summary>
		/// Get element previous to element which unique index specified.
		/// Without index validation.
		/// </summary>
		/// <param name="key">Index of element which previous are desired.</param>
		/// <returns>Index of previous element.</returns>
		public int Prev(int key)
		{
			return listEntries[key].prev;
		}

		/// <summary>
		/// Get element next to element which unique index specified.
		/// Without index validation.
		/// </summary>
		/// <param name="key">Index of element which next are desired.</param>
		/// <returns>Index of previous element.</returns>
		public int Next(int key)
		{
			return listEntries[key].next;
		}

		/// <summary>
		/// Unique index of first element in list.
		/// </summary>
		public int FirstKey
		{
			get
			{
				return _first;
			}
		}

		/// <summary>
		/// Unique index of last element in list.
		/// </summary>
		public int LastKey
		{
			get
			{
				return _last;
			}
		}

		/// <summary>
		/// Get first data element stored in list.
		/// </summary>
		public T First
		{
			get
			{
				if (_first == EmptyListPointers)
					throw new InvalidOperationException("List is empty!");

				return listEntries[_first].value;
			}
			set
			{
				if (_first == EmptyListPointers)
					throw new InvalidOperationException("List is empty!");

				listEntries[_first].value = value;
			}
		}

		/// <summary>
		/// Get last data element stored in list.
		/// </summary>
		public T Last
		{
			get
			{
				if (_last == EmptyListPointers)
					throw new InvalidOperationException("List is empty!");

				return listEntries[_last].value;
			}
			set
			{
				if (_last == EmptyListPointers)
					throw new InvalidOperationException("List is empty!");

				listEntries[_last].value = value;
			}
		}

		/// <summary>
		/// Get data element by associated unque index.
		/// Without index validatioin.
		/// </summary>
		/// <param name="key">Unique index of data element.</param>
		/// <returns>Desired data element.</returns>
		public T this[int key]
		{
			get
			{
				return listEntries[key].value;
			}
			set
			{
				listEntries[key].value = value;
			}
		}

		/// <summary>
		/// Get data element by associated unque index.
		/// With index validatioin.
		/// </summary>
		/// <param name="key">Unique index of data element.</param>
		/// <returns>Desired data element.</returns>
		public T GetProtected(int key)
		{
			if (listEntries[key].prev == ReferenceToNothing)
				throw new ArgumentException("There is no such element!");

			return listEntries[key].value;
		}

		/// <summary>
		/// Number of elements stored in list.
		/// </summary>
		public int Count
		{
			get
			{
				return _count;
			}
		}

		/// <summary>
		/// Indicates whether list can collapse it's capacity or not.
		/// </summary>
		public bool Collapsable
		{
			get
			{
				return _collapsable;
			}
			set
			{
				_collapsable = value;
			}
		}

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			if (enumerators == null) enumerators = new APMemoryManager<BufferedLinkedListEnumerator<T>>();
			return enumerators.New().Init(this);
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return enumerators.New().Init(this);
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// Copies the elements of the System.Collections.ICollection to an System.Array,
		/// starting at a particular System.Array index.
		/// </summary>
		/// <param name="array">
		///  The one-dimensional System.Array that is the destination of the elements
		///  copied from System.Collections.ICollection. The System.Array must have zero-based
		///  indexing.
		/// </param>
		/// <param name="index">
		/// The zero-based index in array at which copying begins.
		/// </param>
		public void CopyTo(Array array, int index)
		{
			if (array.Length - index < _count)
				throw new ArgumentException("Array is too small!");

			int current = _first;
			while (current != EmptyListPointers)
			{
				array.SetValue(listEntries[current].value, index++);
				current = listEntries[current].next;
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the System.Collections.ICollection
		/// is synchronized (thread safe).
		/// </summary>
		public bool IsSynchronized
		{
			get { return false; }
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the System.Collections.ICollection.
		/// </summary>
		public object SyncRoot
		{
			get { return this; }
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Adds an item to the System.Collections.Generic.ICollection.
		/// </summary>
		/// <param name="item">
		/// The object to add to the System.Collections.Generic.ICollection.
		/// </param>
		void ICollection<T>.Add(T item)
		{
			AddLast(item);
		}

		/// <summary>
		/// Determines whether the System.Collections.Generic.ICollection contains
		/// a specific value.
		/// </summary>
		/// <param name="item">
		/// The object to locate in the System.Collections.Generic.ICollection.
		/// </param>
		/// <returns>
		/// true if item is found in the System.Collections.Generic.ICollection; otherwise,
		/// false.
		/// </returns>
		public bool Contains(T item)
		{
			IEquatable<T> equatable = item as IEquatable<T>;

			if (equatable == null)
				throw new ArgumentException("Can not complete Contains operation. Type is not Equatable!");

			int current = _first;
			while (current != EmptyListPointers)
			{
				if (equatable.Equals(listEntries[current].value))
					return true;

				current = listEntries[current].next;
			}

			return false;
		}

		/// <summary>
		/// Copies the elements of the System.Collections. Generic.ICollection to an
		/// System.Array, starting at a particular System. Array index.
		/// </summary>
		/// <param name="array">
		/// The one-dimensional System.Array that is the destination of the elements
		/// copied from System.Collections.Generic.ICollection. The System.Array must
		/// have zero-based indexing.
		/// </param>
		/// <param name="arrayIndex">
		/// The zero-based index in array at which copying begins.
		/// </param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array.Length - arrayIndex < _count)
				throw new ArgumentException("Array is too small!");

			int current = _first;
			while (current != EmptyListPointers)
			{
				array.SetValue(listEntries[current].value, arrayIndex++);
				current = listEntries[current].next;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the System.Collections.Generic.ICollection
		/// is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection.
		/// </summary>
		/// <param name="item">
		/// The object to remove from the System.Collections.Generic.ICollection.
		/// </param>
		/// <returns>
		/// True if item was successfully removed from the System.Collections.Generic.ICollection;
		/// otherwise, false. This method also returns false if item is not found in
		/// the original System.Collections.Generic.ICollection.
		/// </returns>
		bool ICollection<T>.Remove(T item)
		{
			IEquatable<T> equatable = item as IEquatable<T>;

			if (equatable == null)
				throw new ArgumentException("Can not complete Contains operation. Type is not Equatable!");

			int current = _first;
			while (current != EmptyListPointers)
			{
				if (equatable.Equals(listEntries[current].value))
				{
					Remove(current);
					return true;
				}

				current = listEntries[current].next;
			}

			return false;
		}

		/// <summary>
		/// Removes all items from the System.Collections.Generic.ICollection.
		/// </summary>
		public void Clear()
		{
			_count = 0;
			_first = _last = EmptyListPointers;

			for (int i = 0; i < _capacity; i++)
			{
				_stackFree[i] = i;
				listEntries[i].prev = ReferenceToNothing;
			}
		}

		#endregion

		/// <summary>
		/// Stores elements of a list into array (in exactly the same order) and returns it.
		/// </summary>
		/// <returns>Array with elements of a list.</returns>
		public T[] ToArray()
		{
			T[] array = new T[_count];
			Int32 index = 0;
			foreach (T value in this)
				array[index++] = value;

			return array;
		}
	}
}
