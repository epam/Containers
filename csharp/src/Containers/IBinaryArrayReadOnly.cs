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
using System.Linq;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Interface for BinaryArrayReadOnly
	/// </summary>
	public interface IBinaryArrayReadOnly
	{
		/// <summary>
		/// Convert BinaryArray to byte array.
		/// </summary>
		/// <param name="buffer">Byte Array.</param>
		void GetBytes(byte[] buffer);
		/// <summary>
		/// Convert BinaryArray to byte array with offset.
		/// </summary>
		/// <param name="buffer">Destination byte array.</param>
		/// <param name="srcOffset">BinaryArray offset.</param>
		/// <param name="size">Count of bytes to convert.</param>
		void GetBytes(byte[] buffer, int srcOffset, int size);
		/// <summary>
		/// Convert BinaryArray to byte array with offset.
		/// </summary>
		/// <param name="buffer">Destination byte array.</param>
		/// <param name="srcOffset">Source offset.</param>
		/// <param name="dstOffset">Destination offset.</param>
		/// <param name="size">Count of bytes to convert.</param>
		void GetBytes(byte[] buffer, int srcOffset, int dstOffset, int size);

		/// <summary>
		/// Return clone of BinaryArray.
		/// </summary>
		/// <returns>Clone of BinaryArray.</returns>
		IBinaryArrayReadOnly Clone();
		
		/// <summary>
		/// Return capacity of BinaryArray.
		/// </summary>
		int Capacity { get;}
		/// <summary>
		/// Return number of bytes in BinaryArray.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Gets or sets the <see cref="Byte"/> at the specified index.
		/// </summary>
		/// <value>
		/// The <see cref="Byte"/>.
		/// </value>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		/// <exception cref="System.IndexOutOfRangeException">
		/// </exception>
		/// <exception cref="IndexOutOfRangeException"></exception>
		Byte this[Int32 index]
		{
			get;
		}

	}
}