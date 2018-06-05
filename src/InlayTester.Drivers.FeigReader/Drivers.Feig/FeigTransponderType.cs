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
	[Flags]
	public enum FeigTransponderType
	{
		/// <summary>
		/// 0000h None
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// 0001h I-Code1
		/// </summary>
		ICode1 = 0x0001,

		/// <summary>
		/// 0008h ISO15693
		/// </summary>
		ISO15693 = 0x0008,

		/// <summary>
		/// 0010h ISO14443A
		/// </summary>
		ISO14443A = 0x0010,

		/// <summary>
		/// 0020h ISO14443B
		/// </summary>
		ISO14443B = 0x0020,

		/// <summary>
		/// 0040h I-Code EPC
		/// </summary>
		ICodeEPC = 0x0040,

		/// <summary>
		/// 0100h Jewel
		/// </summary>
		Jewel = 0x0100,

		/// <summary>
		/// 0200h ISO18000-3M3
		/// </summary>
		ISO18000_3M3 = 0x0200,

		/// <summary>
		/// 0400h SR176
		/// </summary>
		SR176 = 0x0400,

		/// <summary>
		/// 0800h SRIx
		/// </summary>
		SRIx = 0x0800,
	}
}
