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
using InlayTester.Shared.Transports;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig
{
	[TestFixture]
	public class Test_FeigReaderSettings
	{
		[Test]
		public void Construction_Defaults()
		{
			var settings = new FeigReaderSettings();

			Check.That(settings.TransportSettings.PortName)
				.IsEqualTo("COM1");
			Check.That(settings.TransportSettings.Baud)
				.IsEqualTo(38400);
			Check.That(settings.TransportSettings.DataBits)
				.IsEqualTo(8);
			Check.That(settings.TransportSettings.Parity)
				.IsEqualTo(Parity.Even);
			Check.That(settings.TransportSettings.StopBits)
				.IsEqualTo(StopBits.One);
			Check.That(settings.TransportSettings.Handshake)
				.IsEqualTo(Handshake.None);
			Check.That(settings.Address)
				.IsEqualTo(255);
			Check.That(settings.Timeout)
				.IsEqualTo(TimeSpan.FromMilliseconds(1000));
			Check.That(settings.Protocol)
				.IsEqualTo(FeigProtocol.Advanced);

			Check.That(settings.ToString())
				.IsEqualTo("Transport: 'COM1,38400,8,Even,One,None', Address: 255, Timeout: 1000 ms, Protocol: Advanced");
		}

		[Test]
		public void Construction_Copy()
		{
			var copy = new FeigReaderSettings {
				TransportSettings = new SerialTransportSettings {
					PortName = "COM3",
					Baud = 19200,
					DataBits = 7,
					Parity = Parity.Mark,
					StopBits = StopBits.Two,
					Handshake = Handshake.RequestToSendXOnXOff,
				},
				Address = 123,
				Timeout = TimeSpan.FromMilliseconds(500),
				Protocol = FeigProtocol.Standard
			};

			var settings = new FeigReaderSettings(copy);

			Check.That(settings.TransportSettings.PortName)
				.IsEqualTo("COM3");
			Check.That(settings.TransportSettings.Baud)
				.IsEqualTo(19200);
			Check.That(settings.TransportSettings.DataBits)
				.IsEqualTo(7);
			Check.That(settings.TransportSettings.Parity)
				.IsEqualTo(Parity.Mark);
			Check.That(settings.TransportSettings.StopBits)
				.IsEqualTo(StopBits.Two);
			Check.That(settings.TransportSettings.Handshake)
				.IsEqualTo(Handshake.RequestToSendXOnXOff);
			Check.That(settings.Address)
				.IsEqualTo(123);
			Check.That(settings.Timeout)
				.IsEqualTo(TimeSpan.FromMilliseconds(500));
			Check.That(settings.Protocol)
				.IsEqualTo(FeigProtocol.Standard);

			Check.That(settings.ToString())
				.IsEqualTo("Transport: 'COM3,19200,7,Mark,Two,RequestToSendXOnXOff', Address: 123, Timeout: 500 ms, Protocol: Standard");
		}
	}
}
