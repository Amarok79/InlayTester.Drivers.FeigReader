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
    public class Test_FeigSoftwareInfo
    {
        [Test]
        public void Construction_Defaults()
        {
            // act
            var info = new FeigSoftwareInfo();

            // assert
            Check.That(info.FirmwareVersion).IsEqualTo(new Version(0, 0, 0));
            Check.That(info.HardwareType).IsEqualTo(0x00);
            Check.That(info.ReaderType).IsEqualTo(FeigReaderType.Unknown);
            Check.That(info.SupportedTransponders).IsEqualTo(0x0000);

            Check.That(info.ToString())
                 .IsEqualTo(
                      "FirmwareVersion: 0.0.0, HardwareType: 0x00, ReaderType: Unknown, SupportedTransponders: 0x0000"
                  );
        }

        [Test]
        public void Construction_Copy()
        {
            // act
            var copy = new FeigSoftwareInfo {
                FirmwareVersion       = new Version(3, 4, 0),
                HardwareType          = 0x34,
                ReaderType            = FeigReaderType.CPR40,
                SupportedTransponders = 0x1234,
            };

            var info = new FeigSoftwareInfo(copy);

            // assert
            Check.That(info.FirmwareVersion).IsEqualTo(new Version(3, 4, 0));
            Check.That(info.HardwareType).IsEqualTo(0x34);
            Check.That(info.ReaderType).IsEqualTo(FeigReaderType.CPR40);
            Check.That(info.SupportedTransponders).IsEqualTo(0x1234);

            Check.That(info.ToString())
                 .IsEqualTo(
                      "FirmwareVersion: 3.4.0, HardwareType: 0x34, ReaderType: CPR40, SupportedTransponders: 0x1234"
                  );
        }
    }
}
