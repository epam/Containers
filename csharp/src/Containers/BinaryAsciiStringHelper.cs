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
