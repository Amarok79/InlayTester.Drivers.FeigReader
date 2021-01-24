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
    public class Test_FeigCommand
    {
        [Test]
        public void TestNames()
        {
            Check.That(Enum.GetNames(typeof(FeigCommand)))
               .IsOnlyMadeOf(
                    nameof(FeigCommand.None),
                    nameof(FeigCommand.BaudRateDetection),
                    nameof(FeigCommand.StartFlashLoader),
                    nameof(FeigCommand.CPUReset),
                    nameof(FeigCommand.GetSoftwareVersion),
                    nameof(FeigCommand.GetReaderInfo),
                    nameof(FeigCommand.RFReset),
                    nameof(FeigCommand.RFOutputOnOff),
                    nameof(FeigCommand.SetOutput),
                    nameof(FeigCommand.ReaderLogin),
                    nameof(FeigCommand.ReadConfiguration),
                    nameof(FeigCommand.WriteConfiguration),
                    nameof(FeigCommand.SaveConfiguration),
                    nameof(FeigCommand.SetDefaultConfiguration),
                    nameof(FeigCommand.WriteMifareReaderKeys),
                    nameof(FeigCommand.ISOStandardHostCommand),
                    nameof(FeigCommand.ISO14443SpecialHostCommand),
                    nameof(FeigCommand.ISO14443ATransparentCommand),
                    nameof(FeigCommand.ISO14443BTransparentCommand),
                    nameof(FeigCommand.CommandQueue)
                );
        }

        [Test]
        public void TestValues()
        {
            Check.That((Byte) FeigCommand.None).IsEqualTo(0x00);
            Check.That((Byte) FeigCommand.BaudRateDetection).IsEqualTo(0x52);
            Check.That((Byte) FeigCommand.StartFlashLoader).IsEqualTo(0x55);
            Check.That((Byte) FeigCommand.CPUReset).IsEqualTo(0x63);
            Check.That((Byte) FeigCommand.GetSoftwareVersion).IsEqualTo(0x65);
            Check.That((Byte) FeigCommand.GetReaderInfo).IsEqualTo(0x66);
            Check.That((Byte) FeigCommand.RFReset).IsEqualTo(0x69);
            Check.That((Byte) FeigCommand.RFOutputOnOff).IsEqualTo(0x6A);
            Check.That((Byte) FeigCommand.SetOutput).IsEqualTo(0x72);
            Check.That((Byte) FeigCommand.ReaderLogin).IsEqualTo(0xA0);
            Check.That((Byte) FeigCommand.ReadConfiguration).IsEqualTo(0x80);
            Check.That((Byte) FeigCommand.WriteConfiguration).IsEqualTo(0x81);
            Check.That((Byte) FeigCommand.SaveConfiguration).IsEqualTo(0x82);
            Check.That((Byte) FeigCommand.SetDefaultConfiguration).IsEqualTo(0x83);
            Check.That((Byte) FeigCommand.WriteMifareReaderKeys).IsEqualTo(0xA2);
            Check.That((Byte) FeigCommand.ISOStandardHostCommand).IsEqualTo(0xB0);
            Check.That((Byte) FeigCommand.ISO14443SpecialHostCommand).IsEqualTo(0xB2);
            Check.That((Byte) FeigCommand.ISO14443ATransparentCommand).IsEqualTo(0xBD);
            Check.That((Byte) FeigCommand.ISO14443BTransparentCommand).IsEqualTo(0xBE);
            Check.That((Byte) FeigCommand.CommandQueue).IsEqualTo(0xBC);
        }
    }
}
