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

using InlayTester.Shared;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig
{
	[TestFixture]
	public class Test_FeigTransponder
	{
		[Test]
		public void Default()
		{
			var transponder = new FeigTransponder();

			Check.That(transponder.TransponderType)
				.IsEqualTo(FeigTransponderType.Unknown);
			Check.That(transponder.Identifier.IsEmpty)
				.IsTrue();

			Check.That(transponder.ToString())
				.IsEqualTo("Type: Unknown, ID: <empty>");
		}

		[Test]
		public void Construction()
		{
			var transponder = new FeigTransponder {
				TransponderType = FeigTransponderType.ISO14443A,
				Identifier = BufferSpan.From(0x11, 0x22, 0x33)
			};

			Check.That(transponder.TransponderType)
				.IsEqualTo(FeigTransponderType.ISO14443A);
			Check.That(transponder.Identifier.ToArray())
				.ContainsExactly(0x11, 0x22, 0x33);

			Check.That(transponder.ToString())
				.IsEqualTo("Type: ISO14443A, ID: 11-22-33");
		}

		[Test]
		public void ToString_MultipleItems()
		{
			var transponders = new[] {
				new FeigTransponder {
					TransponderType = FeigTransponderType.ISO14443A,
					Identifier = BufferSpan.From(0x11, 0x22, 0x33)
				},
				new FeigTransponder {
					TransponderType = FeigTransponderType.Jewel,
					Identifier = BufferSpan.From(0x44, 0x55)
				},
			};

			Check.That(FeigTransponder.ToString(transponders))
				.IsEqualTo("{ Type: ISO14443A, ID: 11-22-33 }, { Type: Jewel, ID: 44-55 }");
		}

		[Test]
		public void ToString_SingleItems()
		{
			var transponders = new[] {
				new FeigTransponder {
					TransponderType = FeigTransponderType.ISO14443A,
					Identifier = BufferSpan.From(0x11, 0x22, 0x33)
				},
			};

			Check.That(FeigTransponder.ToString(transponders))
				.IsEqualTo("{ Type: ISO14443A, ID: 11-22-33 }");
		}

		[Test]
		public void ToString_NoItems()
		{
			var transponders = new FeigTransponder[] {
			};

			Check.That(FeigTransponder.ToString(transponders))
				.IsEqualTo("");
		}

		[Test]
		public void ToString_Null()
		{
			Check.That(FeigTransponder.ToString(null))
				.IsEqualTo("");
		}
	}
}
