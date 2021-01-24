/* MIT License
 * 
 * Copyright (c) 2020, Olaf Kober
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
    public class Test_FeigTransferStatus
    {
        [Test]
        public void TestNames()
        {
            Check.That(Enum.GetNames(typeof(FeigTransferStatus)))
               .IsOnlyMadeOf(
                    nameof(FeigTransferStatus.Success),
                    nameof(FeigTransferStatus.Canceled),
                    nameof(FeigTransferStatus.Timeout),
                    nameof(FeigTransferStatus.CommunicationError),
                    nameof(FeigTransferStatus.UnexpectedResponse)
                );
        }

        [Test]
        public void TestValues()
        {
            Check.That((Int32) FeigTransferStatus.Success).IsEqualTo(0);
            Check.That((Int32) FeigTransferStatus.Canceled).IsEqualTo(1);
            Check.That((Int32) FeigTransferStatus.Timeout).IsEqualTo(-1);
            Check.That((Int32) FeigTransferStatus.CommunicationError).IsEqualTo(-2);
            Check.That((Int32) FeigTransferStatus.UnexpectedResponse).IsEqualTo(-3);
        }
    }
}
