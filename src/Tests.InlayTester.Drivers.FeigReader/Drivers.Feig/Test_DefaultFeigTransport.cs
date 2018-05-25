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
using System.Threading;
using System.Threading.Tasks;
using Common.Logging.Simple;
using InlayTester.Shared;
using InlayTester.Shared.Transports;
using NCrunch.Framework;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig
{
	[TestFixture]
	public class Test_DefaultFeigTransport
	{
		[Test, ExclusivelyUses("COMA")]
		public void Open_Close_Dispose()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA"
			};

			using (var transportA = new DefaultFeigTransport(settingsA, new NoOpLogger()))
			{
				transportA.Open();
				transportA.Close();
			}
		}

		[Test, ExclusivelyUses("COMA", "COMB")]
		public void ReceivedDataIgnored()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA"
			};

			using (var transportA = new DefaultFeigTransport(settingsA, new NoOpLogger()))
			{
				var settings = new SerialTransportSettings {
					PortName = "COMB",
					Baud = 38400,
					DataBits = 8,
					Parity = Parity.Even,
					StopBits = StopBits.One,
					Handshake = Handshake.None,
				};

				using (var transportB = Transport.Create(settings))
				{
					transportA.Open();
					transportB.Open();

					transportB.Send(BufferSpan.From(
						0x02, 0x00, 0x0f, 0x00, 0x65, 0x00, 0x03, 0x03,
						0x00, 0x44, 0x53, 0x0d, 0x30, 0x74, 0x69
					));

					Thread.Sleep(2000);
				}
			}
		}

		[Test, ExclusivelyUses("COMA", "COMB")]
		public async Task Success_ReceivedResponse()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA"
			};

			using (var transportA = new DefaultFeigTransport(settingsA, new NoOpLogger()))
			{
				var settings = new SerialTransportSettings {
					PortName = "COMB",
					Baud = 38400,
					DataBits = 8,
					Parity = Parity.Even,
					StopBits = StopBits.One,
					Handshake = Handshake.None,
				};

				using (var transportB = Transport.Create(settings))
				{
					transportA.Open();
					transportB.Open();

					transportB.Received += (sender, e) =>
					{
						if (e.Data[0] == 0x02 && e.Data[1] == 0x00 && e.Data[2] == 0x07 && e.Data[3] == 0xff
						 && e.Data[4] == 0x65 && e.Data[5] == 0x6e && e.Data[6] == 0x61)
						{
							transportB.Send(BufferSpan.From(
								0x02, 0x00, 0x0f, 0x00, 0x65, 0x00, 0x03, 0x03,
								0x00, 0x44, 0x53, 0x0d, 0x30, 0x74, 0x69));
						}
						else
						{
							Assert.Fail("Received unknown data");
						}
					};

					var result = await transportA.Transfer(
						new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
						FeigProtocol.Advanced,
						TimeSpan.FromMilliseconds(5000),
						default);

					Check.That(result.Status)
						.IsEqualTo(FeigTransferStatus.Success);
					Check.That(result.Response.Address)
						.IsEqualTo(0x00);
					Check.That(result.Response.Command)
						.IsEqualTo(FeigCommand.GetSoftwareVersion);
					Check.That(result.Response.Status)
						.IsEqualTo(FeigStatus.OK);
					Check.That(result.Response.Data.ToArray())
						.ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0d, 0x30);
				}
			}
		}

		[Test, ExclusivelyUses("COMA", "COMB")]
		public async Task Success_ReceivedResponse_MultiplePackets()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA"
			};

			using (var transportA = new DefaultFeigTransport(settingsA, new NoOpLogger()))
			{
				var settings = new SerialTransportSettings {
					PortName = "COMB",
					Baud = 38400,
					DataBits = 8,
					Parity = Parity.Even,
					StopBits = StopBits.One,
					Handshake = Handshake.None,
				};

				using (var transportB = Transport.Create(settings))
				{
					transportA.Open();
					transportB.Open();

					transportB.Received += (sender, e) =>
					{
						if (e.Data[0] == 0x02 && e.Data[1] == 0x00 && e.Data[2] == 0x07 && e.Data[3] == 0xff
						 && e.Data[4] == 0x65 && e.Data[5] == 0x6e && e.Data[6] == 0x61)
						{
							transportB.Send(BufferSpan.From(
								0x02, 0x00, 0x0f, 0x00, 0x65, 0x00, 0x03, 0x03));

							Thread.Sleep(100);

							transportB.Send(BufferSpan.From(
								0x00, 0x44, 0x53, 0x0d, 0x30, 0x74, 0x69));
						}
						else
						{
							Assert.Fail("Received unknown data");
						}
					};

					var result = await transportA.Transfer(
						new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
						FeigProtocol.Advanced,
						TimeSpan.FromMilliseconds(5000),
						default);

					Check.That(result.Status)
						.IsEqualTo(FeigTransferStatus.Success);
					Check.That(result.Response.Address)
						.IsEqualTo(0x00);
					Check.That(result.Response.Command)
						.IsEqualTo(FeigCommand.GetSoftwareVersion);
					Check.That(result.Response.Status)
						.IsEqualTo(FeigStatus.OK);
					Check.That(result.Response.Data.ToArray())
						.ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0d, 0x30);
				}
			}
		}

		[Test, ExclusivelyUses("COMA", "COMB")]
		public async Task Timeout()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA"
			};

			using (var transportA = new DefaultFeigTransport(settingsA, new NoOpLogger()))
			{
				var settings = new SerialTransportSettings {
					PortName = "COMB",
					Baud = 38400,
					DataBits = 8,
					Parity = Parity.Even,
					StopBits = StopBits.One,
					Handshake = Handshake.None,
				};

				using (var transportB = Transport.Create(settings))
				{
					transportA.Open();
					transportB.Open();

					transportB.Received += (sender, e) =>
					{
						if (e.Data[0] == 0x02 && e.Data[1] == 0x00 && e.Data[2] == 0x07 && e.Data[3] == 0xff
						 && e.Data[4] == 0x65 && e.Data[5] == 0x6e && e.Data[6] == 0x61)
						{
							Thread.Sleep(500);

							transportB.Send(BufferSpan.From(
								0x02, 0x00, 0x0f, 0x00, 0x65, 0x00, 0x03, 0x03,
								0x00, 0x44, 0x53, 0x0d, 0x30, 0x74, 0x69));
						}
						else
						{
							Assert.Fail("Received unknown data");
						}
					};

					var result = await transportA.Transfer(
						new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
						FeigProtocol.Advanced,
						TimeSpan.FromMilliseconds(100),
						default);

					Check.That(result.Status)
						.IsEqualTo(FeigTransferStatus.Timeout);
					Check.That(result.Response)
						.IsNull();

					Thread.Sleep(1000);
				}
			}
		}

		[Test, ExclusivelyUses("COMA", "COMB")]
		public async Task Canceled()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA"
			};

			using (var transportA = new DefaultFeigTransport(settingsA, new NoOpLogger()))
			{
				var settings = new SerialTransportSettings {
					PortName = "COMB",
					Baud = 38400,
					DataBits = 8,
					Parity = Parity.Even,
					StopBits = StopBits.One,
					Handshake = Handshake.None,
				};

				using (var transportB = Transport.Create(settings))
				{
					transportA.Open();
					transportB.Open();

					transportB.Received += (sender, e) =>
					{
						if (e.Data[0] == 0x02 && e.Data[1] == 0x00 && e.Data[2] == 0x07 && e.Data[3] == 0xff
						 && e.Data[4] == 0x65 && e.Data[5] == 0x6e && e.Data[6] == 0x61)
						{
							Thread.Sleep(500);

							transportB.Send(BufferSpan.From(
								0x02, 0x00, 0x0f, 0x00, 0x65, 0x00, 0x03, 0x03,
								0x00, 0x44, 0x53, 0x0d, 0x30, 0x74, 0x69));
						}
						else
						{
							Assert.Fail("Received unknown data");
						}
					};

					var cts = new CancellationTokenSource();

					var task = transportA.Transfer(
						new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
						FeigProtocol.Advanced,
						TimeSpan.FromMilliseconds(5000),
						cts.Token);

					cts.Cancel();

					var result = await task;

					Check.That(result.Status)
						.IsEqualTo(FeigTransferStatus.Canceled);
					Check.That(result.Response)
						.IsNull();

					Thread.Sleep(1000);
				}
			}
		}

		[Test, ExclusivelyUses("COMA", "COMB")]
		public async Task ChecksumError()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};

			using (var transportA = new DefaultFeigTransport(settingsA, new NoOpLogger()))
			{
				var settings = new SerialTransportSettings {
					PortName = "COMB",
					Baud = 38400,
					DataBits = 8,
					Parity = Parity.Even,
					StopBits = StopBits.One,
					Handshake = Handshake.None,
				};

				using (var transportB = Transport.Create(settings))
				{
					transportA.Open();
					transportB.Open();

					transportB.Received += (sender, e) =>
					{
						if (e.Data[0] == 0x02 && e.Data[1] == 0x00 && e.Data[2] == 0x07 && e.Data[3] == 0xff
						 && e.Data[4] == 0x65 && e.Data[5] == 0x6e && e.Data[6] == 0x61)
						{
							transportB.Send(BufferSpan.From(
								0x02, 0x00, 0x0f, 0x00, 0x65, 0x00, 0x03, 0x03,
								0xFF, 0x44, 0x53, 0x0d, 0x30, 0x74, 0x69));
						}
						else
						{
							Assert.Fail("Received unknown data");
						}
					};

					var result = await transportA.Transfer(
						new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
						FeigProtocol.Advanced,
						TimeSpan.FromMilliseconds(5000),
						default);

					Check.That(result.Status)
						.IsEqualTo(FeigTransferStatus.ChecksumError);
					Check.That(result.Response.Address)
						.IsEqualTo(0x00);
					Check.That(result.Response.Command)
						.IsEqualTo(FeigCommand.GetSoftwareVersion);
					Check.That(result.Response.Status)
						.IsEqualTo(FeigStatus.OK);
					Check.That(result.Response.Data.ToArray())
						.ContainsExactly(0x03, 0x03, 0xFF, 0x44, 0x53, 0x0d, 0x30);
				}
			}
		}

		[Test, ExclusivelyUses("COMA", "COMB")]
		public async Task UnexpectedResponse()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};

			using (var transportA = new DefaultFeigTransport(settingsA, new NoOpLogger()))
			{
				var settings = new SerialTransportSettings {
					PortName = "COMB",
					Baud = 38400,
					DataBits = 8,
					Parity = Parity.Even,
					StopBits = StopBits.One,
					Handshake = Handshake.None,
				};

				using (var transportB = Transport.Create(settings))
				{
					transportA.Open();
					transportB.Open();

					transportB.Received += (sender, e) =>
					{
						if (e.Data[0] == 0x02 && e.Data[1] == 0x00 && e.Data[2] == 0x07 && e.Data[3] == 0xff
						 && e.Data[4] == 0x65 && e.Data[5] == 0x6e && e.Data[6] == 0x61)
						{
							transportB.Send(BufferSpan.From(
								0x02, 0x00, 0x08, 0xff, 0x80, 0x81, 0x40, 0x3a,
								0x02, 0x00, 0x08, 0xff, 0x80, 0x82, 0xdb, 0x08,
								0x02, 0x00, 0x08, 0xff, 0x80, 0x83, 0x52
							));
						}
						else
						{
							Assert.Fail("Received unknown data");
						}
					};

					var result = await transportA.Transfer(
						new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
						FeigProtocol.Advanced,
						TimeSpan.FromMilliseconds(5000),
						default);

					Check.That(result.Status)
						.IsEqualTo(FeigTransferStatus.UnexpectedResponse);
					Check.That(result.Response.Address)
						.IsEqualTo(0xff);
					Check.That(result.Response.Command)
						.IsEqualTo(FeigCommand.ReadConfiguration);
					Check.That(result.Response.Status)
						.IsEqualTo(FeigStatus.LengthError);
				}
			}
		}

		[Test, ExclusivelyUses("COMA", "COMB")]
		public async Task Success_ReceivedResponse_MultipleTimes()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA"
			};

			using (var transportA = new DefaultFeigTransport(settingsA, new NoOpLogger()))
			{
				var settings = new SerialTransportSettings {
					PortName = "COMB",
					Baud = 38400,
					DataBits = 8,
					Parity = Parity.Even,
					StopBits = StopBits.One,
					Handshake = Handshake.None,
				};

				using (var transportB = Transport.Create(settings))
				{
					transportA.Open();
					transportB.Open();

					transportB.Received += (sender, e) =>
					{
						if (e.Data[0] == 0x02 && e.Data[1] == 0x00 && e.Data[2] == 0x07 && e.Data[3] == 0xff
						 && e.Data[4] == 0x65 && e.Data[5] == 0x6e && e.Data[6] == 0x61)
						{
							transportB.Send(BufferSpan.From(
								0x02, 0x00, 0x0f, 0x00, 0x65, 0x00, 0x03, 0x03,
								0x00, 0x44, 0x53, 0x0d, 0x30, 0x74, 0x69));
						}
						else
						{
							Assert.Fail("Received unknown data");
						}
					};

					for (Int32 i = 0; i < 100; i++)
					{
						var result = await transportA.Transfer(
							new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
							FeigProtocol.Advanced,
							TimeSpan.FromMilliseconds(5000),
							default);

						Check.That(result.Status)
							.IsEqualTo(FeigTransferStatus.Success);
						Check.That(result.Response.Address)
							.IsEqualTo(0x00);
						Check.That(result.Response.Command)
							.IsEqualTo(FeigCommand.GetSoftwareVersion);
						Check.That(result.Response.Status)
							.IsEqualTo(FeigStatus.OK);
						Check.That(result.Response.Data.ToArray())
							.ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0d, 0x30);
					}
				}
			}
		}
	}
}
