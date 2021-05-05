using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.Deltix.Containers{


	/// <summary>
	/// Sorted collection. Insert/Delete works with complexity O(sqrt(N)). Iteration to next element works with complexity O(1).
	/// Iterators can become incorrect after any modify operation.
	/// </summary>
	/// <typeparam name="Type">Type of elements</typeparam>
	public class SortedSqrtDecomposition<Type>
	{
		private Type[] data;
		private Type[] dataInBegin;
		private Type[] tempArray;
		private int numberOfBlocks;
		private int[] endOfBlock;
		private int size, len;
		IComparer<Type> comparator;
		bool needRebuild = false;

		/// <summary>
		/// No element pointer
		/// </summary>
		public static int NoElement = -1;

		/// <summary>
		/// Create instance of sorted sqrt decomposition.
		/// </summary>
		/// <param name="capacity">Capacity of decomposition.</param>
		/// <param name="comparator">Comparator.</param>
		public SortedSqrtDecomposition(int capacity, IComparer<Type> comparator)
		{
			size = 0;
			numberOfBlocks = 1;
			data = new Type[10];
			dataInBegin = new Type[10];
			tempArray = new Type[10];
			len = 5;
			endOfBlock = new int[10];
			this.comparator = comparator;
		}

		/// <summary>
		/// Create instance of sorted sqrt decomposition with default capacity.
		/// </summary>
		/// <param name="comparator">Comparator.</param>
		public SortedSqrtDecomposition(IComparer<Type> comparator) : this(10, comparator)
		{

		}

		private void ResizeBlocks(int size)
		{
			Array.Resize(ref endOfBlock, size);
			Array.Resize(ref dataInBegin, size);
		}


		private void ResizeData(int size)
		{
			Array.Resize(ref data, size);
			Array.Resize(ref tempArray, size);
		}

		private void RebuildSqrt()
		{
			int index = 0;
			needRebuild = false;
			for (int i = 0; i < numberOfBlocks; ++i)
			{
				for (int j = (len * i << 1); j < endOfBlock[i]; ++j)
				{
					tempArray[index] = data[j];
					index++;
				}
			}
			len = (int)Math.Ceiling(Math.Sqrt(index));
			if (len < 5)
			{
				len = 5;
			}
			numberOfBlocks = (int)Math.Ceiling(1.0 * index / len);
			if (numberOfBlocks == 0)
			{
				numberOfBlocks = 1;
				return;
			}
			if (endOfBlock.Length <= numberOfBlocks)
			{
				ResizeBlocks(numberOfBlocks << 1);
			}
			if (data.Length <= (len * (numberOfBlocks + 3) << 1))
			{
				ResizeData((len * (2 + numberOfBlocks)) << 2);
			}

			for (int i = 0; i < index; ++i)
			{
				int tempIndex = i / len;
				data[(len * tempIndex << 1) + i % len] = tempArray[i];
				endOfBlock[tempIndex] = (len * tempIndex << 1) + i % len + 1;
			}
			for (int i = 0; i < numberOfBlocks; ++i) dataInBegin[i] = data[len * i << 1];
		}

		/// <summary>
		/// Return size of sqrt decomposition.
		/// </summary>
		public int Count
		{
			get
			{
				return size;
			}
		}


		/// <summary>
		/// Add element if there is no such element in decomposition. Return iterator to added element of iterator to existing element.
		/// </summary>
		/// <param name="x">Element to add.</param>
		/// <returns>Iterator to given element.</returns>
		public int AddIfNotExist(Type x)
		{
			bool insert = false;
			if (needRebuild)
			{
				RebuildSqrt();
			}
			for (int j = 0; j < numberOfBlocks; ++j)
			{
				if (size != 0 && dataInBegin[j] != null && comparator.Compare(dataInBegin[j], x) > 0)
				{
					if (j == 0)
					{
						for (int k = endOfBlock[0]; k > 0; k--)
						{
							data[k] = data[k - 1];
						}
						data[0] = x;
						dataInBegin[0] = x;
						endOfBlock[0]++;
						insert = true;
						size++;
						if (endOfBlock[0] == (len << 1))
						{
							needRebuild = true;
						}
						return 0;
					}
					for (int k = (len * (j - 1) << 1); k < endOfBlock[j - 1]; k++)
						if (comparator.Compare(data[k], x) > 0)
						{
							for (int l = endOfBlock[j - 1]; l > k; l--)
							{
								data[l] = data[l - 1];
							}
							data[k] = x;
							endOfBlock[j - 1]++;
							insert = true;
							size++;
							if (endOfBlock[j - 1] == (len * j << 1)) needRebuild = true;
							return k;
						}
						else if (comparator.Compare(data[k], x) == 0)
						{
							return -(k + 1);
						}
					if (!insert)
					{
						data[endOfBlock[j - 1]] = x;
						insert = true;
						endOfBlock[j - 1]++;
						size++;
						if (endOfBlock[j - 1] == (len * j << 1))
						{
							needRebuild = true;
						}
						return endOfBlock[j - 1] - 1;
					}
				}
			}
			if (!insert)
			{
				for (int j = (len * (numberOfBlocks - 1) << 1); j < endOfBlock[numberOfBlocks - 1]; ++j)
					if (data[j] != null && comparator.Compare(data[j], x) > 0)
					{
						for (int l = endOfBlock[numberOfBlocks - 1]; l > j; l--)
						{
							data[l] = data[l - 1];
						}
						data[j] = x;
						if (j == ((len * (numberOfBlocks - 1)) << 1)) dataInBegin[numberOfBlocks - 1] = x;
						insert = true;
						endOfBlock[numberOfBlocks - 1]++;
						size++;
						if (endOfBlock[numberOfBlocks - 1] == (len * numberOfBlocks << 1)) needRebuild = true;
						return j;
					}
					else if (comparator.Compare(data[j], x) == 0)
					{
						return -(j + 1);
					}
				if (!insert)
				{
					size++;
					if (endOfBlock[numberOfBlocks - 1] == (len * (numberOfBlocks - 1) << 1))
						dataInBegin[numberOfBlocks - 1] = x;
					data[endOfBlock[numberOfBlocks - 1]] = x;
					endOfBlock[numberOfBlocks - 1]++;
					if (endOfBlock[numberOfBlocks - 1] == (len * numberOfBlocks << 1)) needRebuild = true;
					return endOfBlock[numberOfBlocks - 1] - 1;
				}
			}
			return NoElement;
		}

		/// <summary>
		/// Remove element from sorted sqrt decomposition.
		/// </summary>
		/// <param name="x">Element to remove.</param>
		/// <returns>True if this element was in sqrt decomposition. False otherwise.</returns>
		public bool Remove(Type x)
		{
			bool delete = false;
			if (needRebuild)
			{
				RebuildSqrt();
			}
			for (int j = 1; j < numberOfBlocks; ++j)
			{
				if (comparator.Compare(dataInBegin[j], x) > 0)
				{
					for (int k = (len * (j - 1) << 1); k < endOfBlock[j - 1]; ++k)
					{
						if (comparator.Compare(data[k], x) == 0)
						{
							if (k == (len * (j - 1) << 1)) dataInBegin[j - 1] = data[k + 1];
							for (int l = k; l < endOfBlock[j - 1] - 1; l++)
							{
								data[l] = data[l + 1];
							}
							endOfBlock[j - 1]--;
							size--;
							if (endOfBlock[j - 1] <= (len * (j - 1) << 1))
							{
								RebuildSqrt();
							};
							return true;
						}
					}
					return false;
				}
			}
			if (!delete)
			{
				for (int k = (len * (numberOfBlocks - 1) << 1); k < endOfBlock[numberOfBlocks - 1]; k++)
				{
					if (comparator.Compare(data[k], x) == 0)
					{
						if (k == (len * (numberOfBlocks - 1)) << 1) dataInBegin[numberOfBlocks - 1] = data[k + 1];
						for (int l = k; l < endOfBlock[numberOfBlocks - 1] - 1; l++)
						{
							data[l] = data[l + 1];
						}
						endOfBlock[numberOfBlocks - 1]--;
						size--;
						if (endOfBlock[numberOfBlocks - 1] <= (len * (numberOfBlocks - 1) << 1))
						{
							RebuildSqrt();
						}
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Get iterator to first element of decomposition.
		/// </summary>
		public int First
		{
			get
			{
				if (size > 0)
					return 0;
				else
					return NoElement;
			}
		}

		/// <summary>
		/// Get iterator to element.
		/// </summary>
		/// <param name="element">Element to find.</param>
		/// <returns>Iterator to element.</returns>
		public int GetIterator(Type element)
		{
			for (int j = 1; j < numberOfBlocks; ++j)
			{
				if (dataInBegin[j] != null && comparator.Compare(dataInBegin[j], element) > 0)
				{
					for (int k = (len * (j - 1) << 1); k < endOfBlock[j - 1]; ++k)
					{
						if (comparator.Compare(data[k], element) == 0)
						{
							return k;
						}
					}
				}
			}

			for (int k = (len * (numberOfBlocks - 1) << 1); k < endOfBlock[numberOfBlocks - 1]; k++)
				if (comparator.Compare(data[k], element) == 0)
				{
					return k;
				}
			return NoElement;
		}

		/// <summary>
		/// Get iterator to element at index.
		/// </summary>
		/// <param name="index">Iterator to element at index.</param>
		/// <returns>Iterator to element at index</returns>
		public int GetIteratorByIndex(int index)
		{
			int current = 0;
			for (int j = 0; j < numberOfBlocks; ++j)
			{
				if (current + endOfBlock[j] - (len * j << 1) > index)
				{
					int indexInData = (len * j << 1) + (index - current);
					return indexInData;
				}
				else current += endOfBlock[j] - (len * j << 1);
			}
			return NoElement;
		}

		/// <summary>
		/// Get element by iterator.
		/// </summary>
		/// <param name="iterator">iterator</param>
		/// <returns>Element by iterator.</returns>
		public Type GetAt(int iterator)
		{
			return data[iterator];
		}


		/// <summary>
		/// Set element by iterator to new value (sqrt decomposition should stay sorted).
		/// </summary>
		/// <param name="iterator">Iterator to element.</param>
		/// <param name="element">New value of element.</param>
		public void SetAt(int iterator, Type element)
		{
			data[iterator] = element;
		}


		/// <summary>
		/// Get iterator to element followed current.
		/// </summary>
		/// <param name="iterator">Iterator to current element.</param>
		/// <returns>Iterator to element followed current.</returns>
		public int GetNext(int iterator)
		{
			int block = iterator / (len << 1);
			iterator++;
			if (iterator >= endOfBlock[block])
			{
				block++;
				if (block >= numberOfBlocks) return NoElement;
				iterator = 2 * len * block;
			}
			return iterator;
		}

		/// <summary>
		/// Remove element by iterator.
		/// </summary>
		/// <param name="iterator">Iterator to element.</param>
		public void RemoveAt(int iterator)
		{
			int block = iterator / (len << 1);
			if (block >= numberOfBlocks || iterator >= endOfBlock[block]) throw new IndexOutOfRangeException("Try to remove by incorrect iterator");
			if (iterator == (len * (block) << 1)) dataInBegin[block] = data[iterator + 1];
			for (int l = iterator; l < endOfBlock[block] - 1; l++)
			{
				data[l] = data[l + 1];
			}
			endOfBlock[block]--;
			size--;
			if (endOfBlock[block] <= (len * block << 1))
			{
				RebuildSqrt();
			}
		}

		/// <summary>
		/// Get iterator to element before current.
		/// </summary>
		/// <param name="iterator">Iterator to current element.</param>
		/// <returns>Iterator to previous element.</returns>
		public int GetPrev(int iterator)
		{
			int block = iterator / (len << 1);
			iterator--;
			if (iterator < 2 * len * block)
			{
				block--;
				if (block < 0) return NoElement;
				iterator = endOfBlock[block] - 1;
			}
			return iterator;
		}

		/// <summary>
		/// Get iterator to last element.
		/// </summary>
		public int Last
		{
			get
			{
				if (size == 0)
					return NoElement;
				else
					return endOfBlock[numberOfBlocks - 1] - 1;
			}
		}
	}
}