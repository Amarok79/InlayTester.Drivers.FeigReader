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
	/// This enumeration lists well-known Feig reader types.
	/// </summary>
	public enum FeigReaderType : Byte
	{
		/// <summary>
		/// 0 Unknown
		/// </summary>
		Unknown = 0,


		/// <summary>
		/// 11 ISC.DAT 
		/// </summary>
		ISC_DAT = 11,

		/// <summary>
		/// 12 ISC.UMUX 
		/// </summary>
		ISC_UMUX = 12,

		/// <summary>
		/// 13 ISC.GPC 
		/// </summary>
		ISC_GPC = 13,

		/// <summary>
		/// 20 RW40.30-U 
		/// </summary>
		RW40_30U = 20,

		/// <summary>
		/// 30 ISC.M01 
		/// </summary>
		ISC_M01 = 30,

		/// <summary>
		/// 31 ISC.M02 
		/// </summary>
		ISC_M02 = 31,

		/// <summary>
		/// 33 ISC.M02M8 
		/// </summary>
		ISC_M02M8 = 33,

		/// <summary>
		/// 40 ISC.LR100 
		/// </summary>
		ISC_LR100 = 40,

		/// <summary>
		/// 41 ISC.LR200 
		/// </summary>
		ISC_LR200 = 41,

		/// <summary>
		/// 42 ISC.LR2000 
		/// </summary>
		ISC_LR2000 = 42,

		/// <summary>
		/// 43 ISC.LR2500-B 
		/// </summary>
		ISC_LR2500B = 43,

		/// <summary>
		/// 44 ISC.LR2500-A 
		/// </summary>
		ISC_LR2500A = 44,

		/// <summary>
		/// 45 ISC.LR1002 
		/// </summary>
		ISC_LR1002 = 45,

		/// <summary>
		/// 50 ISC.MU02 
		/// </summary>
		ISC_MU02 = 50,

		/// <summary>
		/// 54 ISC.MRU102 
		/// </summary>
		ISC_MRU102 = 54,

		/// <summary>
		/// 55 ISC.MRU200 
		/// </summary>
		ISC_MRU200 = 55,

		/// <summary>
		/// 56 ISC.MRU200-U 
		/// </summary>
		ISC_MRU200U = 56,

		/// <summary>
		/// 60 ISC.PRH101 
		/// </summary>
		ISC_PRH101 = 60,

		/// <summary>
		/// 61 ISC.PRH101-U (USB-Version) 
		/// </summary>
		ISC_PRH101U = 61,

		/// <summary>
		/// 62 ISC.PRHD102 
		/// </summary>
		ISC_PRHD102 = 62,

		/// <summary>
		/// 63 ISC.PRH102 
		/// </summary>
		ISC_PRH102 = 63,

		/// <summary>
		/// 71 ISC.PRH100–U (USB-Version) 
		/// </summary>
		ISC_PRH100U = 71,

		/// <summary>
		/// 72 ISC.PRH100 
		/// </summary>
		ISC_PRH100 = 72,

		/// <summary>
		/// 73 ISC.MR100–U (USB-Version) 
		/// </summary>
		ISC_MR100U = 73,

		/// <summary>
		/// 74 ISC.MR100 / .PR100 
		/// </summary>
		ISC_MR100_PR100 = 74,

		/// <summary>
		/// 75 ISC.MR200-A / -E 
		/// </summary>
		ISC_MR200AE = 75,

		/// <summary>
		/// 76 ISC.MR101-A 
		/// </summary>
		ISC_MR101A = 76,

		/// <summary>
		/// 77 ISC.MR102 
		/// </summary>
		ISC_MR102 = 77,

		/// <summary>
		/// 78 ISC.MR101-U 
		/// </summary>
		ISC_MR101U = 78,

		/// <summary>
		/// 80 CPR.M02 
		/// </summary>
		CPR_M02 = 80,

		/// <summary>
		/// 81 CPR.02 
		/// </summary>
		CPR_02 = 81,

		/// <summary>
		/// 82 CPR40.30-Ux 
		/// </summary>
		CPR40_30Ux = 82,

		/// <summary>
		/// 83 CPR40.0x-Ax / -Cx 
		/// </summary>
		CPR40_0x_AxCx = 83,

		/// <summary>
		/// 84 CPR.M03 (586/#) 
		/// </summary>
		CPR_M03 = 84,

		/// <summary>
		/// 85 CPR.03 (584/#) 
		/// </summary>
		CPR_03 = 85,

		/// <summary>
		/// 86 CPR30 
		/// </summary>
		CPR30 = 86,

		/// <summary>
		/// 87 CPR.52 
		/// </summary>
		CPR_52 = 87,

		/// <summary>
		/// 88 CPR.04-U 
		/// </summary>
		CPR_04U = 88,

		/// <summary>
		/// 92 ISC.LRU1000 
		/// </summary>
		ISC_LRU1000 = 92,

		/// <summary>
		/// 93 ISC.LRU2000 
		/// </summary>
		ISC_LRU2000 = 93,

		/// <summary>
		/// 94 ISC.LRU3000 
		/// </summary>
		ISC_LRU3000 = 94,

		/// <summary>
		/// 100 MAX50
		/// </summary>
		MAX50 = 100,
	}
}
