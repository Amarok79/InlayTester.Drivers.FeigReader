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


namespace InlayTester.Drivers.Feig
{
	/// <summary>
	/// This enumeration lists well-known transponder types.
	/// </summary>
	public enum FeigTransponderType : Byte
	{
		/// <summary>
		/// 00h I-Code1
		/// </summary>
		ICode1 = 0x00,

		/// <summary>
		/// </summary>
		/// 03h ISO15693
		ISO15693 = 0x03,

		/// <summary>
		/// 04h ISO14443A
		/// </summary>
		ISO14443A = 0x04,

		/// <summary>
		/// </summary>
		/// 05h ISO14443B
		ISO14443B = 0x05,

		/// <summary>
		/// 06h I-Code EPC
		/// </summary>
		ICodeEPC = 0x06,

		/// <summary>
		/// 08h Jewel
		/// </summary>
		Jewel = 0x08,

		/// <summary>
		/// 09h ISO18000-3M3
		/// </summary>
		ISO18000_3M3 = 0x09,

		/// <summary>
		/// 0Ah SR176
		/// </summary>
		SR176 = 0x0A,

		/// <summary>
		/// 0Bh SRIxx (SRI512, SRIX512, SRI4K, SRIX4K)
		/// </summary>
		SRIxx = 0x0B,

		/// <summary>
		/// 10h Innovatron (14443-B’)
		/// </summary>
		Innovatron = 0x10,

		/// <summary>
		/// 11h CTx
		/// </summary>
		CTx = 0x11,

		/// <summary>
		/// 84h EPC class 1 Gen 2
		/// </summary>
		EPC_Class1_Gen2 = 0x084,


		/// <summary>
		/// FFh Unknown Transponder Type
		/// </summary>
		Unknown = 0xFF,
	}
}
