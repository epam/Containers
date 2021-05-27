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
	/// Interface for IBinaryArrayReadWrite
	/// </summary>
	public interface IBinaryArrayReadWrite : IBinaryArrayReadOnly
	{
		/// <summary>
		/// Assign BinaryArray from source byte array.
		/// </summary>
		/// <param name="bytes">Source byte array.</param>
		/// <returns>BinaryArray after assign.</returns>
		IBinaryArrayReadWrite Assign(byte[] bytes);
		/// <summary>
		/// Append BinaryArray from source byte array.
		/// </summary>
		/// <param name="bytes">Source byte array.</param>
		/// <returns>BinaryArray after append.</returns>
		IBinaryArrayReadWrite Append(byte[] bytes);

		/// <summary>
		/// Assign BinaryArray from source byte array.
		/// </summary>
		/// <param name="bytes">Source byte array.</param>
		/// <param name="offset">Offset of source byte array. </param>
		/// <param name="count">Count of bytes to assign.</param>
		/// <returns>BinaryArray after assign</returns>
		IBinaryArrayReadWrite Assign(byte[] bytes, int offset, int count);
		/// <summary>
		/// Append BinaryArray from source byte array.
		/// </summary>
		/// <param name="bytes">Source byte array.</param>
		/// <param name="offset">Offset of source byte array. </param>
		/// <param name="count">Count of bytes to assign.</param>
		/// <returns>BinaryArray after append</returns>
		IBinaryArrayReadWrite Append(byte[] bytes, int offset, int count);

		/// <summary>
		/// Assign BinaryArray from source another BinaryArray.
		/// </summary>
		/// <param name="str">Source BinaryArray</param>
		/// <returns>BinaryArray after assign</returns>
		IBinaryArrayReadWrite Assign(IBinaryArrayReadOnly str);
		/// <summary>
		/// Append BinaryArray from source another BinaryArray.
		/// </summary>
		/// <param name="str">Source BinaryArray</param>
		/// <returns>BinaryArray after append</returns>
		IBinaryArrayReadWrite Append(IBinaryArrayReadOnly str);

		/// <summary>
		/// Return clone of BinaryArray.
		/// </summary>
		/// <returns>Clone of BinaryArray.</returns>
		new IBinaryArrayReadWrite Clone();

		/*TODO: implement only these methods*/
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
		new Byte this[Int32 index]
		{
			get;
			set;
		}

		/// <summary>
		/// Append byte to BinaryArray.
		/// </summary>
		/// <param name="b">Byte to append.</param>
		/// <returns>BinaryArray after changing.</returns>
		IBinaryArrayReadWrite Append(byte b);
		/// <summary>
		/// Clear BinaryArray.
		/// </summary>
		/// <returns>BinaryArray after clearing.</returns>
		IBinaryArrayReadWrite Clear();

	}
}