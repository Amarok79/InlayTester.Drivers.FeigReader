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
	public class Test_FeigTransponderType
	{
		[Test]
		public void TestNames()
		{
			Check.That(Enum.GetNames(typeof(FeigTransponderType)))
				.IsOnlyMadeOf(
					nameof(FeigTransponderType.ICode1),
					nameof(FeigTransponderType.ICodeEPC),
					nameof(FeigTransponderType.ISO14443A),
					nameof(FeigTransponderType.ISO14443B),
					nameof(FeigTransponderType.ISO15693),
					nameof(FeigTransponderType.ISO18000_3M3),
					nameof(FeigTransponderType.Jewel),
					nameof(FeigTransponderType.None),
					nameof(FeigTransponderType.SR176),
					nameof(FeigTransponderType.SRIx)
				);
		}

		[Test]
		public void TestValues()
		{
			Check.That((Int32)FeigTransponderType.None)
				.IsEqualTo(0x0000);
			Check.That((Int32)FeigTransponderType.ICode1)
				.IsEqualTo(0x0001);
			Check.That((Int32)FeigTransponderType.ISO15693)
				.IsEqualTo(0x0008);
			Check.That((Int32)FeigTransponderType.ISO14443A)
				.IsEqualTo(0x0010);
			Check.That((Int32)FeigTransponderType.ISO14443B)
				.IsEqualTo(0x0020);
			Check.That((Int32)FeigTransponderType.ICodeEPC)
				.IsEqualTo(0x0040);
			Check.That((Int32)FeigTransponderType.Jewel)
				.IsEqualTo(0x0100);
			Check.That((Int32)FeigTransponderType.ISO18000_3M3)
				.IsEqualTo(0x0200);
			Check.That((Int32)FeigTransponderType.SR176)
				.IsEqualTo(0x0400);
			Check.That((Int32)FeigTransponderType.SRIx)
				.IsEqualTo(0x0800);
		}
	}
}
