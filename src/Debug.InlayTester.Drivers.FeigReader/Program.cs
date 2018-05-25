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
using System.Reflection;
using System.Threading.Tasks;
using InlayTester.Drivers.Feig;
using InlayTester.Shared.Transports;


namespace InlayTester
{
	public static class Program
	{
		public static async Task Main()
		{
			var settings = new FeigReaderSettings {
				TransportSettings = new SerialTransportSettings {
					PortName = "COM4",
					Baud = 38400,
					DataBits = 8,
					Parity = Parity.Even,
					StopBits = StopBits.One,
					Handshake = Handshake.None,
				},
				Address = 255,
				Protocol = FeigProtocol.Advanced,
				Timeout = TimeSpan.FromMilliseconds(250)
			};

			using (var reader = FeigReader.Create(settings))
			{
				reader.Open();

				for (Int32 i = 0; i < 10000; i++)
				{
					if (i % 1000 == 0)
						Console.WriteLine(i);

					//var result = await reader.Transfer(FeigCommand.BaudRateDetection)
					//	.ConfigureAwait(false);

					//if (result.Status != FeigTransferStatus.Success)
					//	Console.WriteLine(result.Status);

					var result = await reader.TestCommunication()
						.ConfigureAwait(false);

					Console.WriteLine(result);
				}
			}
		}
	}
}
