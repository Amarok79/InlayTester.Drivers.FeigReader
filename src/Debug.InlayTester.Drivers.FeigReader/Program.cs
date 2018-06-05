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
using System.Diagnostics;
using System.Threading.Tasks;
using InlayTester.Drivers.Feig;
using InlayTester.Shared;
using InlayTester.Shared.Transports;


namespace InlayTester
{
	public static class Program
	{
		public static async Task Main()
		{
			var config = new Common.Logging.Configuration.LogConfiguration {
				FactoryAdapter = new Common.Logging.Configuration.FactoryAdapterConfiguration {
					Type = typeof(Common.Logging.NLog.NLogLoggerFactoryAdapter).AssemblyQualifiedName,
					Arguments = new Common.Logging.Configuration.NameValueCollection {
						{ "configType", "FILE" },
						{ "configFile", "./nlog.config" }
					}
				}
			};

			Common.Logging.LogManager.Configure(config);

			var log = Common.Logging.LogManager.GetLogger("Feig");
			//var log = new NoOpLogger();

			var settings = new FeigReaderSettings {
				TransportSettings = new SerialTransportSettings {
					PortName = "COM3",
					Baud = 38400,
					DataBits = 8,
					Parity = Parity.Even,
					StopBits = StopBits.One,
					Handshake = Handshake.None,
				},
				Address = 255,
				Protocol = FeigProtocol.Advanced,
				Timeout = TimeSpan.FromMilliseconds(1500),
			};

			using (IFeigReader reader = FeigReader.Create(settings, log))
			{
				reader.Open();

				var sw = new Stopwatch();

				//await reader.ResetConfigurations(FeigBlockLocation.EEPROM)
				//	.ConfigureAwait(false);

				//await reader.ResetCPU()
				//	.ConfigureAwait(false);

				//var cfg = await reader.ReadConfiguration(1, FeigBlockLocation.RAM)
				//	.ConfigureAwait(false);
				//cfg.Buffer[cfg.Offset + 7] = 10;    // TR-RESPONSE-TIME: 1 sec
				//await reader.WriteConfiguration(1, FeigBlockLocation.RAM, cfg)
				//	.ConfigureAwait(false);

				//var cfg = await reader.ReadConfiguration(5, FeigBlockLocation.RAM)
				//	.ConfigureAwait(false);
				//cfg.Buffer[cfg.Offset + 7] = 10;    // TR-RESPONSE-TIME: 1 sec
				//await reader.WriteConfiguration(1, FeigBlockLocation.RAM, cfg)
				//	.ConfigureAwait(false);

				var data = await reader.Execute(FeigCommand.GetReaderInfo, BufferSpan.From(0x08))
					.ConfigureAwait(false);


				for (Int32 i = 0; i < 1000000; i++)
				{
					if (i % 1000 == 0)
						Console.WriteLine(i);


					try
					{
						sw.Restart();

						var transponders = await reader.Inventory()
							.ConfigureAwait(false);

						Console.WriteLine(FeigTransponder.ToString(transponders));

						//	var info = await reader.GetSoftwareInfo()
						//		.ConfigureAwait(false);

						//	var result = await reader.TestCommunication()
						//		.ConfigureAwait(false);

						//	if (!result)
						//		Debugger.Launch();

						//	await reader.ResetRF()
						//		.ConfigureAwait(false);

						//	BufferSpan cfg;

						//	cfg = await reader.ReadConfiguration(1, FeigBlockLocation.RAM)
						//		.ConfigureAwait(false);
						//	cfg = await reader.ReadConfiguration(1, FeigBlockLocation.EEPROM)
						//		.ConfigureAwait(false);

						//	cfg.Buffer[cfg.Offset + 0] = 0xCD;

						//	await reader.WriteConfiguration(1, FeigBlockLocation.RAM, cfg)
						//		.ConfigureAwait(false);

						//	await reader.ResetConfigurations(FeigBlockLocation.RAM)
						//		.ConfigureAwait(false);

						sw.Stop();

						//	File.AppendAllText("d:\\test.txt", sw.ElapsedMilliseconds + "\r\n");
					}
					catch (Exception ex)
					{
						Debugger.Launch();
					}
				}
			}
		}
	}
}
