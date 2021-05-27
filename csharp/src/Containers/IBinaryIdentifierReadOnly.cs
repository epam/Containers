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
	/// Char encoding.
	/// </summary>
	public enum CharEncoding
	{
		/// <summary>
		/// ASCII
		/// </summary>
		ASCII,
		/// <summary>
		/// UTF8
		/// </summary>
		Utf8,
		/// <summary>
		/// UTF16
		/// </summary>
		Utf16,
		/// <summary>
		/// HEX
		/// </summary>
		Hex
	}

	/// <summary>
	/// Interface for BinaryIdentifierReadOnly.
	/// </summary>
	public interface IBinaryIdentifierReadOnly : IBinaryArrayReadOnly
	{
		/// <summary>
		/// Convert BinaryArray to long
		/// </summary>
		/// <returns></returns>
		long ToLong();
		/// <summary>
		/// Convert BinaryArray with offset to long.
		/// </summary>
		/// <param name="offset">Offset in BinaryArray.</param>
		/// <returns></returns>
		long ToLong(int offset);

		/// <summary>
		/// Convert BinaryArray to MutableString with Encoding.
		/// </summary>
		/// <param name="charset">CharEncoding.</param>
		/// <param name="str">Destination MutableString.</param>
		void GetChars(CharEncoding charset, MutableString str);
		/// <summary>
		/// Convert BinaryArray to MutableString with Encoding.
		/// </summary>
		/// <param name="charset">CharEncoding.</param>
		/// <param name="str">Destination MutableString.</param>
		/// <param name="offset">Offset in BinaryArray.</param>
		void GetChars(CharEncoding charset, MutableString str, int offset);

		/// <summary>
		/// Convert BinaryArray to String with Encoding.
		/// </summary>
		/// <param name="charset">CharEncoding.</param>
		/// <param name="offset">Offset in BinaryArray.</param>
		/// <returns>String representation of BinaryArray.</returns>
		String ToString(CharEncoding charset, int offset);
		/// <summary>
		/// Convert BinaryArray to String with Encoding. 
		/// </summary>
		/// <param name="charset">CharEncoding.</param>
		/// <returns>String representation of BinaryArray.</returns>
		String ToString(CharEncoding charset);

		/*TODO: Strictly recommended to override*/
		/// <summary>
		/// Return true if binary array equals another.
		/// </summary>
		/// <param name="another">Another binary array.</param>
		/// <returns>True if binary array equals another.</returns>
		bool Equals(IBinaryIdentifierReadOnly another);
		/// <summary>
		/// Return hash code of binary array.
		/// </summary>
		/// <returns>Hash code of binary array.</returns>
		int HashCode();
		/// <summary>
		/// Return string representation of binary array.
		/// </summary>
		/// <returns>String representation of binary array.</returns>
		String ToString();
		/// <summary>
		/// Return clone of BinaryArray.
		/// </summary>
		/// <returns>Clone of BinaryArray.</returns>
		new IBinaryIdentifierReadOnly Clone();
	}
}