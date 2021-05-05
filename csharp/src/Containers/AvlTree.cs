using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;



namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Represents an AVL tree.
	/// http://en.wikipedia.org/wiki/AVL_tree
	/// </summary>
	/// <typeparam name="TKey"> The type of the keys in the tree.</typeparam>
	/// <typeparam name="TValue"> The type of the values in the tree. </typeparam>
	public class AvlTree<TKey, TValue> where TKey : IComparable
	{
		/// <summary>
		/// Represents AVL tree node.
		/// </summary>
		public struct Entry
		{
			/// <summary>
			/// Search key of this node.
			/// </summary>
			public TKey Key;

			/// <summary>
			/// Value associated with this node.
			/// </summary>
			public TValue Value;

			/// <summary>
			/// Reference to parent node of this node.
			/// </summary>
			public Int32 _parent;

			/// <summary>
			/// Reference to left child of this node.
			/// </summary>
			public Int32 _left;

			/// <summary>
			/// Reference to right child of this node.
			/// </summary>
			public Int32 _right;

			/// <summary>
			/// Height of this node.
			/// </summary>
			public Int32 _height;

			/// <summary>
			/// Number of children in the subtree rooted at this node.
			/// </summary>
			public Int32 _count;

			/// <summary>
			/// Create new AVL tree node.
			/// </summary>
			/// <param name="Key"> Search key of this node. </param>
			/// <param name="Value"> Value associated with this key. </param>
			/// <param name="_parent"> Reference to parent node. </param>
			/// <param name="_left"> Reference to left child of this node. </param>
			/// <param name="_right"> Reference to right child of this node. </param>
			/// <param name="_height"> Height of this node. </param>
			/// <param name="_count"> Number of children in the subtree rooted at this node. </param>
			public Entry(TKey Key, TValue Value, Int32 _parent, Int32 _left, Int32 _right, Int32 _height, Int32 _count)
			{
				this.Key = Key;
				this.Value = Value;
				this._parent = _parent;
				this._left = _left;
				this._right = _right;
				this._height = _height;
				this._count = _count;
			}
		}

		/// <summary>
		/// Represents AVL tree iterator.
		/// </summary>
		public struct AvlTreeIterator
		{
			/// <summary>
			/// Position of the current node.
			/// </summary>
			Int32 _current;

			/// <summary>
			/// Position of the current node in sorted order.
			/// </summary>
			Int32 _position;

			/// <summary>
			/// Tree data array.
			/// </summary>
			Entry[] _data;

			/// <summary>
			/// Represents null iterator.
			/// </summary>
			public static AvlTreeIterator EmptyIterator = new AvlTreeIterator(null, _NO_ELEMENT, 0);

			/// <summary>
			/// Creates tree iterator.
			/// </summary>
			/// <param name="data">Tree data array. </param>
			/// <param name="current">Position of the current node. </param>
			/// <param name="position">Position of the current node in sorted order. </param>
			public AvlTreeIterator(Entry[] data, Int32 current, Int32 position)
			{

				_data = data;
				_current = current;
				_position = position;
			}

			/// <summary>
			/// Search key of the current node.
			/// </summary>
			public TKey Key
			{
				get { return _data[_current].Key; }
			}

			/// <summary>
			/// Value associated with the current node.
			/// </summary>
			public TValue Value
			{
				get { return _data[_current].Value; }
			}

			/// <summary>
			/// Returns true if iterator points to valid node.
			/// </summary>
			public bool Valid
			{
				get { return _current != _NO_ELEMENT; }
			}

			/// <summary>
			/// Returns position of the current node in sorted order.
			/// </summary>
			public int Position
			{
				get { return _position; }
			}

			/// <summary>
			/// Gets the next element in sorted order.
			/// </summary>
			/// <returns> True if the next element exists. </returns>
			public bool Next()
			{
				if (_current == _NO_ELEMENT)
					return false;

				if (_data[_current]._right != _NO_ELEMENT)
				{
					_current = _data[_current]._right;

					while (_data[_current]._left != _NO_ELEMENT)
						_current = _data[_current]._left;

					++_position;
					return true;
				}
				else
				{
					Int32 previous = _current;
					_current = _data[_current]._parent;
					while (_current != _NO_ELEMENT && _data[_current]._left != previous)
					{
						previous = _current;
						_current = _data[_current]._parent;
					}
					if (_current == _NO_ELEMENT)
					{
						return false;
					}
					else
					{
						++_position;
						return true;
					}
				}
			}

			/// <summary>
			/// Gets the previous element in sorted order.
			/// </summary>
			/// <returns> True if the previous element exists. </returns>
			public bool Previous()
			{
				if (_current == _NO_ELEMENT)
					return false;

				if (_data[_current]._left != _NO_ELEMENT)
				{
					_current = _data[_current]._left;

					while (_data[_current]._right != _NO_ELEMENT)
						_current = _data[_current]._right;

					--_position;
					return true;
				}
				else
				{
					Int32 previous = _current;
					_current = _data[_current]._parent;
					while (_current != _NO_ELEMENT && _data[_current]._right != previous)
					{
						previous = _current;
						_current = _data[_current]._parent;
					}
					if (_current == _NO_ELEMENT)
					{
						return false;
					}
					else
					{
						--_position;
						return true;
					}
				}
			}
		}

		/// <summary>
		/// Reference to null element.
		/// </summary>
		public static Int32 _NO_ELEMENT = -1;

		Entry[] _data = null;
		Int32 _capacity;
		Int32 _root = _NO_ELEMENT;
		Int32 _count = 0;
		Int32 _free = 0;

		/// <summary>
		/// Creates AVL tree.
		/// </summary>
		public AvlTree()
			: this(32)
		{
		}

		/// <summary>
		/// Creates AVL tree with required number of preallocated elements.
		/// </summary>
		/// <param name="capacity"> Number of nodes to preallocate. </param>
		public AvlTree(Int32 capacity)
		{
			_capacity = capacity;

			_data = new Entry[_capacity];

			for (int i = 0; i < _capacity; ++i)
			{
				_data[i]._count = 1;
				_data[i]._height = 1;
				_data[i]._left = _NO_ELEMENT;
				_data[i]._parent = _NO_ELEMENT;
				_data[i]._right = i + 1;
			}

			_data[_capacity - 1]._right = _NO_ELEMENT;
		}

		/// <summary>
		/// Searches an element with certain key.
		/// </summary>
		/// <param name="key"> Search key. </param>
		/// <param name="value"> Result value. </param>
		/// <returns>Returns true if item with required key is found. </returns>
		bool TryGetValue(TKey key, out TValue value)
		{
			value = default(TValue);

			if (_root == _NO_ELEMENT)
				return false;

			Int32 current = _root;
			while (true)
			{
				Int32 compareResult = key.CompareTo(_data[current].Key);
				if (compareResult < 0)
				{
					if (_data[current]._left != _NO_ELEMENT)
						current = _data[current]._left;
					else
					{
						break;
					}
				}
				else if (compareResult == 0)
				{
					value = _data[current].Value;
					return true;
				}
				else
				{
					if (_data[current]._right != _NO_ELEMENT)
						current = _data[current]._right;
					else
					{
						break;
					}
				}
			}

			value = default(TValue);
			return false;
		}

		/// <summary>
		/// Finds whether tree contains required key.
		/// </summary>
		/// <param name="key">Search key. </param>
		/// <returns> Returns true if tree contains required key. </returns>
		public bool ContainsKey(TKey key)
		{
			TValue value;
			return TryGetValue(key, out value);
		}

		/// <summary>
		/// Number of elements in tree.
		/// </summary>
		public Int32 Count
		{
			get { return _count; }
		}

		/// <summary>
		/// Adds element into the tree.
		/// </summary>
		/// <param name="key"> Search key. </param>
		/// <param name="value"> Value associated with the key. </param>
		public void Add(TKey key, TValue value)
		{
			if (_root == _NO_ELEMENT)
			{
				_root = _free;
				_free = _data[_root]._right;
				_data[_root].Key = key;
				_data[_root].Value = value;
				_data[_root]._parent = _data[_root]._left = _data[_root]._right = _NO_ELEMENT;
			}
			else
			{
				Int32 node = Add(_root, key, value);

				while (node != _NO_ELEMENT)
				{
					if (node == _root)
					{
						if (key.CompareTo(_data[node].Key) < 0)
							LeftFix(node, _NO_ELEMENT);
						else
							RightFix(node, _NO_ELEMENT);
					}
					else
						Fix(node);

					node = _data[node]._parent;
				}
			}

			_count = _data[_root]._count;
		}

		private Int32 Add(Int32 node, TKey key, TValue value)
		{
			while (true)
			{
				if (key.CompareTo(_data[node].Key) < 0)
				{
					if (_data[node]._left == _NO_ELEMENT)
					{
						if (_count == _capacity)
						{
							_capacity <<= 1;

							Array.Resize<Entry>(ref _data, _capacity);

							for (int i = _count; i < _capacity; ++i)
							{
								_data[i]._count = 1;
								_data[i]._height = 1;
								_data[i]._left = _NO_ELEMENT;
								_data[i]._parent = _NO_ELEMENT;
								_data[i]._right = i + 1;
							}
							_data[_capacity - 1]._right = _NO_ELEMENT;
							_free = _count;
						}

						int created = _free;
						_data[node]._left = created;

						_data[created].Key = key;
						_data[created].Value = value;
						_data[created]._parent = node;
						_data[created]._count = 1;

						_free = _data[created]._right;

						_data[created]._right = _data[created]._left = _NO_ELEMENT;

						return created;
					}
					else
					{
						node = _data[node]._left;
						continue;
					}
				}
				else
				{
					if (_data[node]._right == _NO_ELEMENT)
					{
						if (_count == _capacity)
						{
							_capacity <<= 1;

							Array.Resize<Entry>(ref _data, _capacity);

							for (int i = _count; i < _capacity; ++i)
							{
								_data[i]._count = 1;
								_data[i]._height = 1;
								_data[i]._left = _NO_ELEMENT;
								_data[i]._parent = _NO_ELEMENT;
								_data[i]._right = i + 1;
							}
							_data[_capacity - 1]._right = _NO_ELEMENT;
							_free = _count;
						}

						int created = _free;
						_data[node]._right = created;

						_data[created].Key = key;
						_data[created].Value = value;
						_data[created]._parent = node;
						_data[created]._count = 1;

						_free = _data[created]._right;
						_data[created]._right = _data[created]._left = _NO_ELEMENT;

						return created;
					}
					else
					{
						node = _data[node]._right;
						continue;
					}
				}
			}
		}

		/// <summary>
		/// Removes item with specified key.
		/// </summary>
		/// <param name="key"> Search key of the element to remove. </param>
		/// <returns> Returns true if item with specified key was in the tree. </returns>
		public bool Remove(TKey key)
		{
			if (_root == _NO_ELEMENT)
				return false;

			if (_root != _NO_ELEMENT)
			{
				Int32 removedParent = _NO_ELEMENT;
				if (!Remove(_root, key, ref removedParent))
					return false;

				if (removedParent == _NO_ELEMENT)
					removedParent = _root;

				Int32 node = removedParent;
				while (node != _NO_ELEMENT)
				{
					_data[node]._count--;

					if (node == _root)
					{
						if (key.CompareTo(_data[node].Key) > 0)
							LeftFix(node, _NO_ELEMENT);
						else
							RightFix(node, _NO_ELEMENT);
					}
					else
						Fix(node);

					node = _data[node]._parent;
				}

			}

			if (_root != _NO_ELEMENT)
				_count = _data[_root]._count;
			else
				_count = 0;

			return true;
		}

		private bool Remove(Int32 node, TKey key, ref Int32 removedParent)
		{
			while (true)
			{
				Int32 compare = key.CompareTo(_data[node].Key);
				if (compare == 0)
				{
					Int32 parent = _data[node]._parent;

					if (_data[node]._left == _NO_ELEMENT)
					{
						if (parent == _NO_ELEMENT)
						{
							_root = _data[node]._right;
							_data[node]._parent = _NO_ELEMENT;

							if (_root != _NO_ELEMENT)
								_data[_root]._parent = _NO_ELEMENT;
						}
						else if (_data[parent]._left == node)
						{
							_data[parent]._left = _data[node]._right;

							if (_data[node]._right != _NO_ELEMENT)
								_data[_data[node]._right]._parent = parent;
						}
						else
						{
							_data[parent]._right = _data[node]._right;

							if (_data[node]._right != _NO_ELEMENT)
								_data[_data[node]._right]._parent = parent;
						}

						_data[node]._right = _free;

						int removed = _data[node]._parent;

						_data[node]._parent = _data[node]._left = _NO_ELEMENT;
						_free = node;

						removedParent = removed;
						return true;
					}
					else if (_data[node]._right == _NO_ELEMENT)
					{
						if (parent == _NO_ELEMENT)
						{
							_root = _data[node]._left;
							_data[node]._parent = _NO_ELEMENT;

							if (_root != _NO_ELEMENT)
								_data[_root]._parent = _NO_ELEMENT;
						}
						else if (_data[parent]._left == node)
						{
							_data[parent]._left = _data[node]._left;

							if (_data[node]._left != _NO_ELEMENT)
								_data[_data[node]._left]._parent = parent;
						}
						else
						{
							_data[parent]._right = _data[node]._left;

							if (_data[node]._left != _NO_ELEMENT)
								_data[_data[node]._left]._parent = parent;
						}

						_data[node]._right = _free;
						int removed = _data[node]._parent;
						_data[node]._parent = _data[node]._left = _NO_ELEMENT;
						_free = node;

						removedParent = removed;
						return true;
					}
					else
					{
						Int32 temp = _data[node]._left;
						while (_data[temp]._right != _NO_ELEMENT)
							temp = _data[temp]._right;

						_data[node].Value = _data[temp].Value;
						_data[node].Key = _data[temp].Key;

						_data[temp].Key = key;
						_data[temp].Value = default(TValue);

						//parent = node;
						node = _data[node]._left;
						continue;
					}
				}
				else if (compare < 0)
				{
					if (_data[node]._left != _NO_ELEMENT)
					{
						//parent = node;
						node = _data[node]._left;
						continue;
					}
				}
				else
				{
					if (_data[node]._right != _NO_ELEMENT)
					{
						//parent = node;
						node = _data[node]._right;
						continue;
					}
				}

				removedParent = _NO_ELEMENT;
				return false;
			}
		}

		/// <summary>
		/// Returns iterator which points to the first element in the tree.
		/// </summary>
		public AvlTreeIterator First
		{
			get
			{
				Int32 current = _root;
				if (current == _NO_ELEMENT)
				{
					return AvlTreeIterator.EmptyIterator;
				}

				while (_data[current]._left != _NO_ELEMENT)
				{
					current = _data[current]._left;
				}

				return new AvlTreeIterator(_data, current, 0);
			}
		}

		/// <summary>
		/// Returns iterator which points to the last element in the tree.
		/// </summary>
		public AvlTreeIterator Last
		{
			get
			{
				Int32 current = _root;
				if (current == _NO_ELEMENT)
				{
					return AvlTreeIterator.EmptyIterator;
				}

				while (_data[current]._right != _NO_ELEMENT)
				{
					current = _data[current]._right;
				}

				return new AvlTreeIterator(_data, current, _count - 1);
			}
		}

		/// <summary>
		/// Finds iterator at the specified position in the sorted order.
		/// </summary>
		/// <param name="position">Required position. </param>
		/// <returns>Returns iterator at the specified position in the sorted order.</returns>
		public AvlTreeIterator OrderStatisticIterator(int position)
		{
			int sorted = position;
			if (_root == _NO_ELEMENT || position >= _count)
				return AvlTreeIterator.EmptyIterator;

			Int32 node = _root;

			while (true)
			{
				if (_data[node]._left != _NO_ELEMENT)
				{
					if (_data[_data[node]._left]._count < position)
					{
						position -= _data[_data[node]._left]._count + 1;
						node = _data[node]._right;
					}
					else if (_data[_data[node]._left]._count > position)
					{
						node = _data[node]._left;
					}
					else
						return new AvlTreeIterator(_data, node, sorted);
				}
				else
				{
					if (position > 0)
					{
						position--;
						node = _data[node]._right;
					}
					else
						return new AvlTreeIterator(_data, node, sorted);
				}
			}
		}

		/// <summary>
		/// Finds iterator which is greater than specified quantile elements in the tree.
		/// </summary>
		/// <param name="quantile"> Part of elements to excel. </param>
		/// <returns> Returns iterator which is greater than specified quantile elements in the tree. </returns>
		public AvlTreeIterator QuantileIterator(double quantile)
		{
			if (!(quantile >= 0 && quantile <= 1)) // Double.NaN checking style
				throw new ArgumentOutOfRangeException(nameof(quantile), quantile, "The value must be in interval [0, 1].");
			int position = Math.Min((int)Math.Floor(quantile * (double)_count + 0.5), _count - 1);
			return OrderStatisticIterator(position);
		}

		/// <summary>
		/// Finds iterator which points to the greatest element less or equal than specified key.
		/// </summary>
		/// <param name="key"> Search key. </param>
		/// <returns>Returns iterator which points to the greatest element less or equal than specified key.</returns>
		public AvlTreeIterator LessOrEqualIterator(TKey key)
		{
			if (_root == _NO_ELEMENT)
				return AvlTreeIterator.EmptyIterator;

			int position = _data[_root]._count - 1;
			if (_data[_root]._right != _NO_ELEMENT)
				position -= _data[_data[_root]._right]._count;

			return FindLessOrEqual(_root, position, key);
		}

		/// <summary>
		/// Finds iterator which points to the smallest element greater or equal than specified key.
		/// </summary>
		/// <param name="key"> Search key. </param>
		/// <returns>Returns iterator which points to the smallest element greater or equal than specified key.</returns>
		public AvlTreeIterator GreaterOrEqualIterator(TKey key)
		{
			if (_root == _NO_ELEMENT)
				return AvlTreeIterator.EmptyIterator;

			int position = _data[_root]._count - 1;
			if (_data[_root]._right != _NO_ELEMENT)
				position -= _data[_data[_root]._right]._count;

			return FindGreaterOrEqual(_root, position, key);
		}

		private AvlTreeIterator FindGreaterOrEqual(Int32 node, Int32 position, TKey key)
		{
			while (true)
			{
				int compare = key.CompareTo(_data[node].Key);
				if (compare < 0)
				{
					if (_data[node]._left != _NO_ELEMENT)
					{
						position--;
						if (_data[_data[node]._left]._right != _NO_ELEMENT)
							position -= _data[_data[_data[node]._left]._right]._count;

						node = _data[node]._left;

						continue;
					}
					return new AvlTreeIterator(_data, node, position);
				}
				else if (compare > 0)
				{
					if (_data[node]._right != _NO_ELEMENT && _data[node]._right != _free)
					{
						position++;
						if (_data[_data[node]._right]._left != _NO_ELEMENT)
							position += _data[_data[_data[node]._right]._left]._count;

						node = _data[node]._right;

						continue;
					}
					AvlTreeIterator it = new AvlTreeIterator(_data, node, position);
					it.Next();

					return it;
				}
				else
					return new AvlTreeIterator(_data, node, position);
			}
		}

		private AvlTreeIterator FindLessOrEqual(Int32 node, Int32 position, TKey key)
		{
			while (true)
			{
				int compare = key.CompareTo(_data[node].Key);
				if (compare < 0)
				{
					if (_data[node]._left != _NO_ELEMENT)
					{
						position--;
						if (_data[_data[node]._left]._right != _NO_ELEMENT)
							position -= _data[_data[_data[node]._left]._right]._count;

						node = _data[node]._left;

						continue;
					}

					AvlTreeIterator it = new AvlTreeIterator(_data, node, position);
					it.Previous();

					return it;
				}
				else if (compare > 0)
				{
					if (_data[node]._right != _NO_ELEMENT && _data[node]._right != _free)
					{
						position++;
						if (_data[_data[node]._right]._left != _NO_ELEMENT)
							position += _data[_data[_data[node]._right]._left]._count;

						node = _data[node]._right;

						continue;
					}
					return new AvlTreeIterator(_data, node, position);
				}
				else
					return new AvlTreeIterator(_data, node, position);
			}
		}

		/// <summary>
		/// Finds iterator at the specified position in the sorted order.
		/// </summary>
		/// <param name="position">Required position. </param>
		/// <returns>Returns iterator at the specified position in the sorted order.</returns>
		public AvlTreeIterator this[int position]
		{
			get { return OrderStatisticIterator(position); }
		}

		/// <summary>
		/// Finds iterator which is greater than specified quantile elements in the tree.
		/// </summary>
		/// <param name="quantile"> Part of elements to excel. </param>
		/// <returns> Returns iterator which is greater than specified quantile elements in the tree. </returns>
		public AvlTreeIterator this[double quantile]
		{
			get { return QuantileIterator(quantile); }
		}

		#region Validation

		private Int32 Cnt(int node)
		{
			if (node == _NO_ELEMENT)
				return 0;

			return 1 + Cnt(_data[node]._left) + Cnt(_data[node]._right);
		}

		private bool Check()
		{
			if (_root == _NO_ELEMENT)
				return true;

			return Check(_root, _NO_ELEMENT);
		}

		private bool Check(Int32 node, Int32 parent)
		{
			int count = Cnt(node);
			if (_data[node]._count != count)
				return false;

			if (_data[node]._parent != parent)
			{
				return false;
			}

			if (_data[node]._right != _NO_ELEMENT)
			{
				if (_data[node].Key.CompareTo(_data[_data[node]._right].Key) > 0)
					return false;

				if (!Check(_data[node]._right, node))
					return false;
			}

			if (_data[node]._left != _NO_ELEMENT)
			{
				if (_data[node].Key.CompareTo(_data[_data[node]._left].Key) < 0)
					return false;

				if (!Check(_data[node]._left, node))
					return false;
			}

			return true;
		}

		#endregion

		#region Auxillary

		private void Fix(Int32 node)
		{
			if (_data[_data[node]._parent]._left == node)
			{
				LeftFix(node, _data[node]._parent);
			}
			else
			{
				RightFix(node, _data[node]._parent);
			}
		}

		private void LeftFix(Int32 NodeIndex, Int32 ParentIndex)
		{
			Entry node = _data[NodeIndex];

			Int32 leftHeight = node._left == _NO_ELEMENT ? 0 : _data[node._left]._height;
			Int32 rightHeight = node._right == _NO_ELEMENT ? 0 : _data[node._right]._height;

			if (leftHeight > rightHeight + 1)
			{
				Entry left = _data[node._left];

				leftHeight = left._left == _NO_ELEMENT ? 0 : _data[left._left]._height;
				rightHeight = left._right == _NO_ELEMENT ? 0 : _data[left._right]._height;

				if (rightHeight > leftHeight)
					RotateLeft(node._left, _data[node._left]._right, NodeIndex);

				RotateRight(_data[NodeIndex]._left, NodeIndex, ParentIndex);
			}

			node = _data[NodeIndex];

			leftHeight = node._left == _NO_ELEMENT ? 0 : _data[node._left]._height;
			rightHeight = node._right == _NO_ELEMENT ? 0 : _data[node._right]._height;

			_data[NodeIndex]._height = 1 + (leftHeight > rightHeight ? leftHeight : rightHeight);
			_data[NodeIndex]._count = 1 + (node._left == _NO_ELEMENT ? 0 : _data[node._left]._count) +
			 (node._right == _NO_ELEMENT ? 0 : _data[node._right]._count);
		}

		private void RightFix(Int32 NodeIndex, Int32 ParentIndex)
		{
			Entry node = _data[NodeIndex];

			Int32 leftHeight = node._left == _NO_ELEMENT ? 0 : _data[node._left]._height;
			Int32 rightHeight = node._right == _NO_ELEMENT ? 0 : _data[node._right]._height;
			if (leftHeight + 1 < rightHeight)
			{
				Entry right = _data[node._right];

				leftHeight = right._left == _NO_ELEMENT ? 0 : _data[right._left]._height;
				rightHeight = right._right == _NO_ELEMENT ? 0 : _data[right._right]._height;

				if (rightHeight < leftHeight)
					RotateRight(_data[_data[NodeIndex]._right]._left, _data[NodeIndex]._right, NodeIndex);

				RotateLeft(NodeIndex, _data[NodeIndex]._right, ParentIndex);
			}

			node = _data[NodeIndex];

			leftHeight = node._left == _NO_ELEMENT ? 0 : _data[node._left]._height;
			rightHeight = node._right == _NO_ELEMENT ? 0 : _data[node._right]._height;
			_data[NodeIndex]._height = 1 + (leftHeight > rightHeight ? leftHeight : rightHeight);

			_data[NodeIndex]._count = 1 + (node._left == _NO_ELEMENT ? 0 : _data[node._left]._count) +
			 (node._right == _NO_ELEMENT ? 0 : _data[node._right]._count);

		}

		private void RotateRight(Int32 LeftIndex, Int32 RightIndex, Int32 ParentIndex)
		{
			Int32 leftHeight;
			Int32 rightHeight;

			if (ParentIndex == _NO_ELEMENT)
			{
				_root = LeftIndex;
				_data[LeftIndex]._parent = _NO_ELEMENT;
			}
			else
			{
				if (_data[ParentIndex]._left == RightIndex)
					_data[ParentIndex]._left = LeftIndex;
				else
					_data[ParentIndex]._right = LeftIndex;

				_data[LeftIndex]._parent = ParentIndex;
			}

			_data[RightIndex]._left = _data[LeftIndex]._right;

			if (_data[LeftIndex]._right != _NO_ELEMENT)
				_data[_data[LeftIndex]._right]._parent = RightIndex;

			_data[LeftIndex]._right = RightIndex;
			_data[RightIndex]._parent = LeftIndex;

			Entry right = _data[RightIndex];

			leftHeight = right._left == _NO_ELEMENT ? 0 : _data[right._left]._height;
			rightHeight = right._right == _NO_ELEMENT ? 0 : _data[right._right]._height;

			_data[RightIndex]._height = 1 + Math.Max(leftHeight, rightHeight);
			_data[RightIndex]._count = 1 + (right._left == _NO_ELEMENT ? 0 : _data[right._left]._count) + (right._right == _NO_ELEMENT ? 0 : _data[right._right]._count);

			Entry left = _data[LeftIndex];

			leftHeight = left._left == _NO_ELEMENT ? 0 : _data[left._left]._height;
			rightHeight = _data[RightIndex]._height;
			_data[LeftIndex]._height = 1 + Math.Max(leftHeight, rightHeight);
			_data[LeftIndex]._count = 1 + (_data[LeftIndex]._left == _NO_ELEMENT ? 0 : _data[_data[LeftIndex]._left]._count) + _data[RightIndex]._count;
		}

		private void RotateLeft(Int32 LeftIndex, Int32 RightIndex, Int32 ParentIndex)
		{
			Int32 leftHeight;
			Int32 rightHeight;

			if (ParentIndex == _NO_ELEMENT)
			{
				_root = RightIndex;
				_data[RightIndex]._parent = _NO_ELEMENT;
			}
			else
			{

				if (_data[ParentIndex]._left == LeftIndex)
					_data[ParentIndex]._left = RightIndex;
				else
					_data[ParentIndex]._right = RightIndex;

				_data[RightIndex]._parent = ParentIndex;
			}

			_data[LeftIndex]._right = _data[RightIndex]._left;

			if (_data[RightIndex]._left != _NO_ELEMENT)
				_data[_data[RightIndex]._left]._parent = LeftIndex;

			_data[RightIndex]._left = LeftIndex;
			_data[LeftIndex]._parent = RightIndex;

			Entry left = _data[LeftIndex];

			leftHeight = left._left == _NO_ELEMENT ? 0 : _data[left._left]._height;
			rightHeight = left._right == _NO_ELEMENT ? 0 : _data[left._right]._height;
			_data[LeftIndex]._height = 1 + Math.Max(leftHeight, rightHeight);
			_data[LeftIndex]._count = 1 + (_data[LeftIndex]._left == _NO_ELEMENT ? 0 : _data[_data[LeftIndex]._left]._count) + (_data[LeftIndex]._right == _NO_ELEMENT ? 0 : _data[_data[LeftIndex]._right]._count);

			Entry right = _data[RightIndex];

			leftHeight = _data[LeftIndex]._height;
			rightHeight = right._right == _NO_ELEMENT ? 0 : _data[right._right]._height;
			_data[RightIndex]._height = 1 + Math.Max(leftHeight, rightHeight);
			_data[RightIndex]._count = 1 + _data[LeftIndex]._count + (right._right == _NO_ELEMENT ? 0 : _data[right._right]._count);

		}

		#endregion
	}
}