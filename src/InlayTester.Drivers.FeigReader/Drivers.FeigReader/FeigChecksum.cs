/* MIT License
 * 
 * Copyright (c) 2018, Olaf Kober
 * https://github.com/Amarok79/InlayTester.Drivers.FeigReader
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
*/

using System;
using InlayTester.Shared;


namespace InlayTester.Drivers.Feig
{
	/// <summary>
	/// This type provides methods for calculating a CRC as used by Feig RFID readers.
	/// </summary>
	public static class FeigChecksum
	{
		// From official Feig reader documentation:
		//
		// x16 + x12 + x5 + 1 = 0x8408
		// start value: 0xFFFF
		//
		// After long search, identified as "CRC-16/MCRF4XX"
		// http://reveng.sourceforge.net/crc-catalogue/all.htm

		// static data
		private static readonly CrcCalculator sCalculator = new CrcCalculator(
			16, 0x1021, true, 0xffff, 0x0000, true, true
		);


		/// <summary>
		/// Calculates the CRC for the given data.
		/// </summary>
		public static Int32 Calculate(in BufferSpan data)
		{
			return sCalculator.Calculate(data);
		}
	}
}
