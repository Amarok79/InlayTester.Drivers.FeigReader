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
					nameof(FeigTransponderType.TagIt),
					nameof(FeigTransponderType.ISO15693),
					nameof(FeigTransponderType.ISO14443A),
					nameof(FeigTransponderType.ISO14443B),
					nameof(FeigTransponderType.ICodeEPC),
					nameof(FeigTransponderType.ICodeUID),
					nameof(FeigTransponderType.Jewel),
					nameof(FeigTransponderType.ISO18000_3M3),
					nameof(FeigTransponderType.SR176),
					nameof(FeigTransponderType.SRIxx),
					nameof(FeigTransponderType.MCRFxxx),
					nameof(FeigTransponderType.FeliCa),
					nameof(FeigTransponderType.Innovatron),
					nameof(FeigTransponderType.ASK_CTx),
					nameof(FeigTransponderType.ISO18000_6A),
					nameof(FeigTransponderType.ISO18000_6B),
					nameof(FeigTransponderType.EM4222),
					nameof(FeigTransponderType.EPC_Class1_Gen2),
					nameof(FeigTransponderType.EPC_Class0),
					nameof(FeigTransponderType.EPC_Class1_Gen1),
					nameof(FeigTransponderType.Unknown)
				);
		}

		[Test]
		public void TestValues()
		{
			Check.That((Byte)FeigTransponderType.ICode1)
				.IsEqualTo(0x00);
			Check.That((Byte)FeigTransponderType.TagIt)
				.IsEqualTo(0x01);
			Check.That((Byte)FeigTransponderType.ISO15693)
				.IsEqualTo(0x03);
			Check.That((Byte)FeigTransponderType.ISO14443A)
				.IsEqualTo(0x04);
			Check.That((Byte)FeigTransponderType.ISO14443B)
				.IsEqualTo(0x05);
			Check.That((Byte)FeigTransponderType.ICodeEPC)
				.IsEqualTo(0x06);
			Check.That((Byte)FeigTransponderType.ICodeUID)
				.IsEqualTo(0x07);
			Check.That((Byte)FeigTransponderType.Jewel)
				.IsEqualTo(0x08);
			Check.That((Byte)FeigTransponderType.ISO18000_3M3)
				.IsEqualTo(0x09);
			Check.That((Byte)FeigTransponderType.SR176)
				.IsEqualTo(0x0A);
			Check.That((Byte)FeigTransponderType.SRIxx)
				.IsEqualTo(0x0B);
			Check.That((Byte)FeigTransponderType.MCRFxxx)
				.IsEqualTo(0x0C);
			Check.That((Byte)FeigTransponderType.FeliCa)
				.IsEqualTo(0x0D);
			Check.That((Byte)FeigTransponderType.Innovatron)
				.IsEqualTo(0x10);
			Check.That((Byte)FeigTransponderType.ASK_CTx)
				.IsEqualTo(0x11);
			Check.That((Byte)FeigTransponderType.ISO18000_6A)
				.IsEqualTo(0x80);
			Check.That((Byte)FeigTransponderType.ISO18000_6B)
				.IsEqualTo(0x81);
			Check.That((Byte)FeigTransponderType.EM4222)
				.IsEqualTo(0x83);
			Check.That((Byte)FeigTransponderType.EPC_Class1_Gen2)
				.IsEqualTo(0x84);
			Check.That((Byte)FeigTransponderType.EPC_Class0)
				.IsEqualTo(0x88);
			Check.That((Byte)FeigTransponderType.EPC_Class1_Gen1)
				.IsEqualTo(0x89);
			Check.That((Byte)FeigTransponderType.Unknown)
				.IsEqualTo(0xFF);
		}
	}
}
