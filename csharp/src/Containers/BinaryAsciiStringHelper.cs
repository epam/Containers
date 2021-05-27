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
    internal class BinaryAsciiStringHelper
    {
		private static readonly char[] HexDigitsUpper = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
		
		internal static short[] DigitPairs = new short[256];

		static BinaryAsciiStringHelper()
		{
			for (int i = 0; i < 16; ++i)
			{
				for (int j = 0; j < 16; ++j)
				{
					DigitPairs[i * 16 + j] = (short)((((((int)HexDigitsUpper[j] << 8) | (int)HexDigitsUpper[i]))) & (0xffff));
				}
			}
		}

	}
}