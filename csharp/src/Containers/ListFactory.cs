using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers{




	/// <summary>
	/// Factory of generic lists.  
	/// The main feature of this collection - common memory pool for all lists and elements.
	///
	/// This collection provides functionallity to register (and unregister) new linked lists (methods <see cref="CreateList"> and <see cref="DeleteList(int)">).
	/// <see cref="CreateList"> returns <see cref="List"/> of new list to you. This method takes O(1) time.
	/// <see cref="DeleteList"> remove all elements of list and unregister list. This method takes O(n) time, where n - number of elements in list.
	/// </summary>
	/// <typeparam name="T">Type of elements of lists.</typeparam>
	public class ListFactory<T>
	{
		/// <summary>
		/// Node of list.
		/// </summary>
		public struct ListNode
		{
			internal int indexInData;
			internal ListFactory<T> factory;

			/// <summary>
			/// Return data in this Node.
			/// </summary>
			public T Data
			{
				get
				{
					return (T)factory.elements[indexInData].data;
				}
			}

			/// <summary>
			/// Remove this node from list.
			/// </summary>
			/// <returns>The node follows this.</returns>
			public ListNode? Remove()
			{
				if (factory.elements[indexInData].nextOrLast == NoElement)
				{
					factory.DeleteElement(indexInData);
					return null;
				}
				ListNode newNode = new ListNode();
				newNode.indexInData = factory.elements[indexInData].nextOrLast;
				newNode.factory = factory;
				factory.DeleteElement(indexInData);
				return newNode;
			}

			/// <summary>
			/// Add node with data before this.
			/// </summary>
			/// <param name="data">Data in inserted Node.</param>
			/// <returns></returns>
			public ListNode AddBefore(T data)
			{
				int index = factory.AddBefore(indexInData, data);
				ListNode newNode = new ListNode();
				newNode.indexInData = index;
				newNode.factory = factory;
				return newNode;
			}

			/// <summary>
			/// Add node with data after this.
			/// </summary>
			/// <param name="data">Data in inserted Node.</param>
			/// <returns></returns>
			public ListNode AddAfter(T data)
			{
				int index = factory.AddAfter(indexInData, data);
				ListNode newNode = new ListNode();
				newNode.indexInData = index;
				newNode.factory = factory;
				return newNode;
			}

			/// <summary>
			/// Return node follows this.
			/// </summary>
			public ListNode? Next
			{
				get
				{
					if (factory.elements[indexInData].nextOrLast == NoElement) return null;
					ListNode newNode = new ListNode();
					newNode.indexInData = factory.elements[indexInData].nextOrLast;
					newNode.factory = factory;
					return newNode;

				}
			}
			
			/// <summary>
			/// Return node before this. 
			/// </summary>
			public ListNode? Prev
			{
				get
				{
					if (factory.elements[indexInData].previousOrFirst == NoElement) return null;
					ListNode newNode = new ListNode();
					newNode.indexInData = factory.elements[indexInData].previousOrFirst;
					newNode.factory = factory;
					return newNode;
				}
			}
		}

		internal struct ListEnumerator : IEnumerator<T>
		{
			int listId;
			int index;
			ListFactory<T> factory;

			internal ListEnumerator(ListFactory<T> factory, int listId)
			{
				this.factory = factory;
				this.listId = listId;
				this.index = -1;
			}

			public T Current
			{
				get
				{
					return (T)factory.elements[index].data;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return factory.elements[index].data;
				}
			}
			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				if (index == -1)
				{
					if (factory.GetCount(listId) == 0) return false;
					index = factory.GetFirstUnsafe(listId);
				}
				else
				if (index != factory.GetLastUnsafe(listId))
				{
					index = factory.GetNextUnsafe(index);
					return true;
				}
				return false;
			}

			public void Reset()
			{
				index = -1;
			}
		}

		/// <summary>
		/// It's just API for work with list from factory as with usual list.
		/// </summary>
		public struct List : IEnumerable<T>
		{
			internal ListFactory<T> factory;
			/// <summary>
			/// List id of this list in ListFactory.
			/// </summary>
			public int ListId
			{
				get;
				internal set;
			}

			public int Count
			{
				get
				{
					return factory.GetCount(ListId);
				}
			}

			/// <summary>
			/// Add first node with data to list.
			/// </summary>
			/// <param name="data">Data of first element.</param>
			/// <returns>Inserted node.</returns>
			public ListNode AddFirst(T data)
			{
				ListNode node = new ListNode();
				node.factory = factory;
				node.indexInData = factory.AddFirst(ListId, data);
				return node;
			}

			/// <summary>
			/// Add last node with data to list.
			/// </summary>
			/// <param name="data">Data of last element.</param>
			/// <returns>Inserted node</returns>
			public ListNode AddLast(T data)
			{
				ListNode node = new ListNode();
				node.factory = factory;
				node.indexInData = factory.AddLast(ListId, data);
				return node;
			}

			/// <summary>
			/// Return first node of list.
			/// </summary>
			public ListNode First
			{
				get
				{
					ListNode node = new ListNode();
					node.factory = factory;
					node.indexInData = factory.GetFirst(ListId);
					return node;
				}
			}

			/// <summary>
			/// Return last node of list.
			/// </summary>
			public ListNode Last
			{
				get
				{
					ListNode node = new ListNode();
					node.factory = factory;
					node.indexInData = factory.GetLast(ListId);
					return node;
				}
			}

			/// <summary>
			/// Return enumerator for this list.
			/// </summary>
			/// <returns>Enumerator for this list.</returns>
			public IEnumerator<T> GetEnumerator()
			{
				return new ListEnumerator(factory, ListId);
			}

			/// <summary>
			/// Return enumerator for this list.
			/// </summary>
			/// <returns>Enumerator for this list.</returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new ListEnumerator(factory, ListId);
			}
		}


		struct ListEntry
		{
			internal int previousOrFirst;
			internal int nextOrLast;
			internal int listOrCount;
			internal Object data;
		}

		const int NoElement = -1;
		private Object ListObject = new object();

		private ListEntry[] elements;
		private int free = NoElement;




		/// <summary>
		/// Create new factory with capacity.
		/// </summary>
		/// <param name="capacity">Capacity of factory.</param>
		public ListFactory(int capacity)
		{
			Resize(capacity);
		}

		/// <summary>
		/// Create new factory with default capacity.
		/// </summary>
		public ListFactory()
		{
			Resize(4);
		}

		private void Resize()
		{
			Resize(elements.Length << 1);
		}

		private void Resize(int capacity)
		{
			int currentCapacity;
			int newCapacity;
			if (elements == null)
			{
				currentCapacity = 0;
				newCapacity = 4;
			}
			else
			{
				currentCapacity = newCapacity = elements.Length;
			}
			while (newCapacity < capacity)
				newCapacity <<= 1;

			if (elements == null)
			{
				elements = new ListEntry[newCapacity];
			}
			else
			{
				Array.Resize(ref elements, newCapacity);
			}

			for (int i = newCapacity - 1; i >= currentCapacity; --i)
				RemoveToPool(i);
		}

		private void RemoveToPool(int index)
		{
			elements[index].data = null;
			elements[index].nextOrLast = free;
			elements[index].listOrCount = NoElement;
			free = index;
		}

		private void CheckElementContract(int elementId)
		{
			if (elements[elementId].listOrCount == NoElement)
			{
				throw new InvalidOperationException("Id " + elementId + " is empty.");
			}

			if (elements[elementId].data == ListObject)
			{
				throw new InvalidOperationException("Id " + elementId + " is a list");
			}
		}

		private void CheckListContract(int listId)
		{
			if (elements[listId].listOrCount == NoElement)
			{
				throw new InvalidOperationException("Id " + listId + " is empty.");
			}

			if (elements[listId].data != ListObject)
			{
				throw new InvalidOperationException("Id " + listId + " is not a list");
			}
		}




		/// <summary>
		/// Create new list.
		/// </summary>
		/// <returns>Created list with id of it.</returns>
		public List CreateList()
		{

			if (free == -1)
				Resize();
			int index = free;
			free = elements[free].nextOrLast;
			elements[index].data = ListObject;
			elements[index].nextOrLast = NoElement;
			elements[index].previousOrFirst = NoElement;
			elements[index].listOrCount = 0;
			List returnedList = new List();
			returnedList.factory = this;
			returnedList.ListId = index;
			return returnedList;
		}


		/// <summary>
		/// Insert element to position before element with id. 
		/// </summary>
		/// <param name="beforeId">Id of element after inserted.</param>
		/// <param name="value">Inserted value.</param>
		/// <returns>Id of inserted element.</returns>
		internal int AddBefore(int beforeId, T value)
		{
			CheckElementContract(beforeId);
			if (free == -1)
				Resize();

			int listId = elements[beforeId].listOrCount;
			int index = free;
			free = elements[free].nextOrLast;

			elements[index].data = value;
			elements[index].listOrCount = listId;

			elements[index].nextOrLast = beforeId;
			elements[index].previousOrFirst = elements[beforeId].previousOrFirst;

			if (elements[beforeId].previousOrFirst != NoElement)
				elements[elements[beforeId].previousOrFirst].nextOrLast = index;
			elements[beforeId].previousOrFirst = index;

			if (elements[listId].previousOrFirst == beforeId)
				elements[listId].previousOrFirst = index;
			++elements[listId].listOrCount;

			return index;
		}

		/// <summary>
		/// Insert element to position after element with id.
		/// </summary>
		/// <param name="afterId">Id of element before inserted.</param>
		/// <param name="value">Inserted value.</param>
		/// <returns>Id of inserted element.</returns>
		internal int AddAfter(int afterId, T value)
		{
			CheckElementContract(afterId);
			if (free == -1)
				Resize();

			int listId = elements[afterId].listOrCount;
			int index = free;
			free = elements[free].nextOrLast;

			elements[index].data = value;
			elements[index].listOrCount = listId;

			elements[index].previousOrFirst = afterId;
			elements[index].nextOrLast = elements[afterId].nextOrLast;

			if (elements[afterId].nextOrLast != NoElement)
				elements[elements[afterId].nextOrLast].previousOrFirst = index;
			elements[afterId].nextOrLast = index;

			if (elements[listId].nextOrLast == afterId)
				elements[listId].nextOrLast = index;
			++elements[listId].listOrCount;

			return index;
		}



		/// <summary>
		///  Add first element to list with id. 
		/// </summary>
		/// <param name="listId">Id of list.</param>
		/// <param name="value">Value of inserted element.</param>
		/// <returns>Id of inserted element.</returns>
		internal int AddFirst(int listId, T value)
		{
			CheckListContract(listId);
			if (free == -1)
				Resize();

			int index = free;
			free = elements[free].nextOrLast;

			elements[index].data = value;
			elements[index].nextOrLast = elements[listId].previousOrFirst;
			elements[index].previousOrFirst = NoElement;
			elements[index].listOrCount = listId;
			if (elements[listId].nextOrLast == NoElement)
				elements[listId].nextOrLast = index;
			else
				elements[elements[listId].previousOrFirst].previousOrFirst = index;
			elements[listId].previousOrFirst = index;
			++elements[listId].listOrCount;

			return index;
		}

		/// <summary>
		///  Add last element to list with id. 
		/// </summary>
		/// <param name="listId">Id of list.</param>
		/// <param name="value">Value of inserted element.</param>
		/// <returns>Id of inserted element.</returns>
		internal int AddLast(int listId, T value)
		{
			CheckListContract(listId);
			if (free == -1)
				Resize();

			int index = free;
			free = elements[free].nextOrLast;

			elements[index].data = value;
			elements[index].nextOrLast = NoElement;
			elements[index].previousOrFirst = elements[listId].nextOrLast;
			elements[index].listOrCount = listId;
			if (elements[listId].previousOrFirst == NoElement)
				elements[listId].previousOrFirst = index;
			else
				elements[elements[listId].nextOrLast].nextOrLast = index;
			elements[listId].nextOrLast = index;
			++elements[listId].listOrCount;

			return index;
		}


		/// <summary>
		/// Remove element with id from list factory.
		/// </summary>
		/// <param name="elementId">Id of element.</param>
		/// <returns>Removed element.</returns>
		internal T DeleteElement(int elementId)
		{
			CheckElementContract(elementId);

			Object d = elements[elementId].data;
			int listId = elements[elementId].listOrCount;
			--elements[listId].listOrCount;
			if (elements[listId].previousOrFirst == elementId)
				elements[listId].previousOrFirst = elements[elementId].nextOrLast;
			else
			{
				elements[elements[elementId].previousOrFirst].nextOrLast = elements[elementId].nextOrLast;
			}

			if (elements[listId].nextOrLast == elementId)
				elements[listId].nextOrLast = elements[elementId].previousOrFirst;
			else
			{
				elements[elements[elementId].nextOrLast].previousOrFirst = elements[elementId].previousOrFirst;
			}

			RemoveToPool(elementId);
			return (T)d;
		}



		/// <summary>
		/// Convert list with id to array.
		/// </summary>
		/// <param name="listId">Id of list.</param>
		/// <returns>Array with data from list.</returns>
		internal T[] ToArray(int listId)
		{
			CheckListContract(listId);
			T[] array = new T[elements[listId].listOrCount];
			for (int i = elements[listId].previousOrFirst, j = 0; i != NoElement; i = elements[i].nextOrLast, ++j)
			{
				array[j] = (T)elements[i].data;
			}

			return array;
		}


		private int GetFirstUnsafe(int listId)
		{
			return elements[listId].previousOrFirst;
		}

		/// <summary>
		/// Get id of first element of list.
		/// </summary>
		/// <param name="listId">Id of list.</param>
		/// <returns>Id of first element of list.</returns>
		internal int GetFirst(int listId)
		{
			CheckListContract(listId);
			return elements[listId].previousOrFirst;
		}


		internal int GetLastUnsafe(int listId)
		{
			return elements[listId].nextOrLast;
		}

		/// <summary>
		/// Get id of last element of list.
		/// </summary>
		/// <param name="listId">Id of list.</param>
		/// <returns>Id of last element of list.</returns>
		internal int GetLast(int listId)
		{
			CheckListContract(listId);
			return elements[listId].nextOrLast;
		}


		/// <summary>
		/// Get count of list.
		/// </summary>
		/// <param name="listId">Id of list.</param>
		/// <returns>Count of list</returns>
		internal int GetCount(int listId)
		{
			CheckListContract(listId);
			return elements[listId].listOrCount;
		}



		/// <summary>
		/// Return element with id.
		/// </summary>
		/// <param name="key">Id of element.</param>
		/// <returns></returns>
		internal T this[int key]
		{

			get
			{
				CheckElementContract(key);
				return (T)elements[key].data;
			}

			set
			{
				CheckElementContract(key);
				elements[key].data = value;
			}
		}

		/// <summary>
		/// Get id of next element follows element with id.
		/// </summary>
		/// <param name="elementId">Id of element.</param>
		/// <returns>Id of element follows element with id.</returns>
		internal int GetNext(int elementId)
		{
			CheckElementContract(elementId);
			return elements[elementId].nextOrLast;
		}

		internal int GetNextUnsafe(int elementId)
		{
			return elements[elementId].nextOrLast;
		}



		/// <summary>
		/// Get id of previous element. 
		/// </summary>
		/// <param name="elementId">Id of element.</param>
		/// <returns>Id of previous element.</returns>
		internal int GetPrevious(int elementId)
		{
			CheckElementContract(elementId);
			return elements[elementId].previousOrFirst;
		}


		/// <summary>
		/// Delete List from ListFactory.
		/// </summary>
		/// <param name="listId">Id of removed list.</param>
		public void DeleteList(List list)
		{
			CheckListContract(list.ListId);
			int next;
			for (int i = elements[list.ListId].previousOrFirst, j = 0; i != NoElement; i = next, ++j)
			{
				next = elements[i].nextOrLast;
				RemoveToPool(i);
			}
			RemoveToPool(list.ListId);
		}
	}

}
