/* MIT License
 * 
 * Copyright (c) 2019, Olaf Kober
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
	public class Test_FeigISOStandardCommandCommand
	{
		[Test]
		public void TestNames()
		{
			Check.That(Enum.GetNames(typeof(FeigISOStandardCommand)))
				.IsOnlyMadeOf(
					nameof(FeigISOStandardCommand.None),
					nameof(FeigISOStandardCommand.Inventory),
					nameof(FeigISOStandardCommand.ReadMultipleBlocks),
					nameof(FeigISOStandardCommand.WriteMultipleBlocks),
					nameof(FeigISOStandardCommand.Select)
				);
		}

		[Test]
		public void TestValues()
		{
			Check.That((Byte)FeigISOStandardCommand.None)
				.IsEqualTo(0x00);
			Check.That((Byte)FeigISOStandardCommand.Inventory)
				.IsEqualTo(0x01);
			Check.That((Byte)FeigISOStandardCommand.ReadMultipleBlocks)
				.IsEqualTo(0x23);
			Check.That((Byte)FeigISOStandardCommand.WriteMultipleBlocks)
				.IsEqualTo(0x24);
			Check.That((Byte)FeigISOStandardCommand.Select)
				.IsEqualTo(0x25);
		}
	}
}
