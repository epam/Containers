/*
  Copyright 2021 EPAM Systems, Inc

  See the NOTICE file distributed with this work for additional information
  regarding copyright ownership. Licensed under the Apache License,
  Version 2.0 (the "License"); you may not use this file except in compliance
  with the License.  You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
  WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the
  License for the specific language governing permissions and limitations under
  the License.
 */
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Public interface for ReadOnly pair
	/// </summary>
	/// <typeparam name="T1">Type of first element</typeparam>
	/// <typeparam name="T2">Type of second element.</typeparam>
	public interface IPairReadOnly<T1, T2>
	{
		/// <summary>
		/// First element of the pair.
		/// </summary>
		T1 First { get;  }
		/// <summary>
		/// Second element of the pair.
		/// </summary>
		T2 Second { get;  }
	}

	public class Pair<T1, T2> : IPairReadOnly<T1, T2>
	{
		/// <summary>
		/// Create instance of pair with defaults first and second.
		/// </summary>
		public Pair()
		{
			First = default(T1);
			Second = default(T2);
		}
		/// <summary>
		/// Create new instance of pair
		/// </summary>
		/// <param name="first">first element of pair</param>
		/// <param name="second">second element of pair</param>
		public Pair(T1 first, T2 second) {
			this.First = first;
			this.Second = second;
		}

		/// <summary>
		/// First element of the pair.
		/// </summary>
		public T1 First { get; set; }
		/// <summary>
		/// Second element of the pair.
		/// </summary>
		public T2 Second { get; set; }

		public override int GetHashCode()
		{
			int hashCode = 0;
			if (First != null) hashCode += First.GetHashCode();
			hashCode *= 31;
			if (Second != null) hashCode += Second.GetHashCode();
			return hashCode;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is IPairReadOnly<T1, T2>))
			{
				return false;
			}
			IPairReadOnly<T1, T2> pairReadOnly = (IPairReadOnly<T1, T2>)obj;
			return Object.Equals(First, pairReadOnly.First) && Object.Equals(Second, pairReadOnly.Second);
		}

		public override string ToString()
		{
			MutableString mutableString = new MutableString();
			mutableString.Append("First: ");
			if (First != null)
			{
				mutableString.Append(First.ToString()).Append("; ");
			} else
			{
				mutableString.Append("null; ");
			}

			mutableString.Append("Second: ");

			if (Second != null)
			{
				mutableString.Append(Second.ToString());
			}
			else
			{
				mutableString.Append("null");
			}
			return mutableString.ToString();
		}
	}

	/// <summary>
	/// Public struct for pair.
	/// </summary>
	/// <typeparam name="T1">Type of first pair.</typeparam>
	/// <typeparam name="T2">Type of second pair.</typeparam>
	public struct ValuePair<T1, T2> : IPairReadOnly<T1, T2>
    {

		/// <summary>
		/// Create new instance of pair
		/// </summary>
		/// <param name="first">first element of pair</param>
		/// <param name="second">second element of pair</param>
		public ValuePair(T1 first, T2 second)
		{
			this.First = first;
			this.Second = second;
		}
		/// <summary>
		/// First element of the pair.
		/// </summary>
		public T1 First { get; set; }
		/// <summary>
		/// Second element of the pair.
		/// </summary>
		public T2 Second { get; set; }

		public override int GetHashCode()
		{
			int hashCode = 0;
			if (First != null) hashCode += First.GetHashCode();
			hashCode *= 31;
			if (Second != null) hashCode += Second.GetHashCode();
			return hashCode;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is IPairReadOnly<T1, T2>))
			{
				return false;
			}
			IPairReadOnly<T1, T2> pairReadOnly = (IPairReadOnly<T1, T2>)obj;
			return Object.Equals(First, pairReadOnly.First) && Object.Equals(Second, pairReadOnly.Second);
		}

		public override string ToString()
		{
			MutableString mutableString = new MutableString();
			mutableString.Append("First: ");
			if (First != null)
			{
				mutableString.Append(First.ToString()).Append("; ");
			}
			else
			{
				mutableString.Append("null; ");
			}

			mutableString.Append("Second: ");

			if (Second != null)
			{
				mutableString.Append(Second.ToString());
			}
			else
			{
				mutableString.Append("null");
			}
			return mutableString.ToString();
		}

	}
}