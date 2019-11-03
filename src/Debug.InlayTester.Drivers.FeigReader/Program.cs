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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.NLog;
using InlayTester.Drivers.Feig;
using InlayTester.Shared.Transports;


namespace InlayTester
{
	public static class Program
	{
		public static async Task Main()
		{
			var config = new LogConfiguration() {
				FactoryAdapter = new FactoryAdapterConfiguration() {
					Type = typeof(NLogLoggerFactoryAdapter).AssemblyQualifiedName,
					Arguments = new NameValueCollection() {
						{ "configType", "FILE" },
						{ "configFile", "./nlog.config" }
					}
				}
			};

			LogManager.Configure(config);

			var log = LogManager.GetLogger("Feig");
			//var log = new NoOpLogger();

			var settings = new FeigReaderSettings() {
				TransportSettings = new SerialTransportSettings() {
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

				var cfg1 = await reader.ReadConfiguration(1, FeigBlockLocation.RAM)
					.ConfigureAwait(false);
				cfg1.Buffer[cfg1.Offset + 7] = 10;    // TR-RESPONSE-TIME: 1 sec
				await reader.WriteConfiguration(1, FeigBlockLocation.RAM, cfg1)
					.ConfigureAwait(false);

				var cfg3 = await reader.ReadConfiguration(3, FeigBlockLocation.RAM)
					.ConfigureAwait(false);
				cfg3.Buffer[cfg3.Offset + 0] = 0x00;
				cfg3.Buffer[cfg3.Offset + 1] = 0x10;
				await reader.WriteConfiguration(3, FeigBlockLocation.RAM, cfg3)
					.ConfigureAwait(false);


				for (Int32 i = 0; i < 1000000; i++)
				{
					if (i % 1000 == 0)
						Console.WriteLine(i);

					try
					{
						sw.Restart();

						try
						{
							await reader.SwitchRF(0x00)
								.ConfigureAwait(false);
							await reader.SwitchRF(0x01)
								.ConfigureAwait(false);

							sw.Stop();

							//if (result.Response.Status != FeigStatus.OK)
							//	Debugger.Launch();

							//Console.WriteLine(result.Response.Status + " " + FeigTransponder.ToString(result.Transponders));
						}
						catch (FeigException exception)
						{
							Console.WriteLine(exception.Response.Status);
							Debugger.Launch();
						}
						catch (TimeoutException)
						{
							Console.WriteLine("TIMEOUT");
							Debugger.Launch();
						}


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

						File.AppendAllText("d:\\test.txt", sw.ElapsedMilliseconds + "\r\n");
					}
					catch (Exception)
					{
						Debugger.Launch();
					}
				}
			}
		}
	}
}
