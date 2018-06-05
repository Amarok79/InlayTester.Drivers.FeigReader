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
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig
{
	[TestFixture]
	public class Test_FeigReaderType
	{
		[Test]
		public void TestNames()
		{
			Check.That(Enum.GetNames(typeof(FeigReaderType)))
				.IsOnlyMadeOf(
					nameof(FeigReaderType.Unknown),
					nameof(FeigReaderType.ISC_DAT),
					nameof(FeigReaderType.ISC_UMUX),
					nameof(FeigReaderType.ISC_GPC),
					nameof(FeigReaderType.RW40_30U),
					nameof(FeigReaderType.ISC_M01),
					nameof(FeigReaderType.ISC_M02),
					nameof(FeigReaderType.ISC_M02M8),
					nameof(FeigReaderType.ISC_LR100),
					nameof(FeigReaderType.ISC_LR200),
					nameof(FeigReaderType.ISC_LR2000),
					nameof(FeigReaderType.ISC_LR2500B),
					nameof(FeigReaderType.ISC_LR2500A),
					nameof(FeigReaderType.ISC_LR1002),
					nameof(FeigReaderType.ISC_MU02),
					nameof(FeigReaderType.ISC_MRU102),
					nameof(FeigReaderType.ISC_MRU200),
					nameof(FeigReaderType.ISC_MRU200U),
					nameof(FeigReaderType.ISC_PRH101),
					nameof(FeigReaderType.ISC_PRH101U),
					nameof(FeigReaderType.ISC_PRHD102),
					nameof(FeigReaderType.ISC_PRH102),
					nameof(FeigReaderType.ISC_PRH100U),
					nameof(FeigReaderType.ISC_PRH100),
					nameof(FeigReaderType.ISC_MR100U),
					nameof(FeigReaderType.ISC_MR100_PR100),
					nameof(FeigReaderType.ISC_MR200AE),
					nameof(FeigReaderType.ISC_MR101A),
					nameof(FeigReaderType.ISC_MR102),
					nameof(FeigReaderType.ISC_MR101U),
					nameof(FeigReaderType.CPR_M02),
					nameof(FeigReaderType.CPR_02),
					nameof(FeigReaderType.CPR40_30Ux),
					nameof(FeigReaderType.CPR40_0x_AxCx),
					nameof(FeigReaderType.CPR_M03),
					nameof(FeigReaderType.CPR_03),
					nameof(FeigReaderType.CPR30),
					nameof(FeigReaderType.CPR_52),
					nameof(FeigReaderType.CPR_04U),
					nameof(FeigReaderType.ISC_LRU1000),
					nameof(FeigReaderType.ISC_LRU2000),
					nameof(FeigReaderType.ISC_LRU3000),
					nameof(FeigReaderType.MAX50)
				);
		}

		[Test]
		public void TestValues()
		{
			Check.That((Byte)FeigReaderType.Unknown)
				.IsEqualTo(0);
			Check.That((Byte)FeigReaderType.CPR40_0x_AxCx)
				.IsEqualTo(83);
		}
	}
}
