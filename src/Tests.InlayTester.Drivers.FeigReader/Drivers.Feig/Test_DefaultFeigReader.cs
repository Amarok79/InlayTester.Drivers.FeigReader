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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging.Simple;
using InlayTester.Shared;
using InlayTester.Shared.Transports;
using Moq;
using NCrunch.Framework;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig
{
	public class Test_DefaultFeigReader
	{
		[TestFixture]
		public class Open
		{
			[Test, ExclusivelyUses("COMA")]
			public void Success_For_FirstTime()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					Check.ThatCode(() => reader.Open())
						.DoesNotThrow();
				}
			}

			[Test, ExclusivelyUses("COMA")]
			public void Exception_When_AlreadyDisposed()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					reader.Dispose();

					Check.ThatCode(() => reader.Open())
						.Throws<ObjectDisposedException>();
				}
			}

			[Test, ExclusivelyUses("COMA")]
			public void Exception_When_AlreadyOpen()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					Check.ThatCode(() => reader.Open())
						.DoesNotThrow();

					Check.ThatCode(() => reader.Open())
						.Throws<InvalidOperationException>();
				}
			}

			[Test]
			public void Exception_For_InvalidPortName()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "InvalidPortName" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					Check.ThatCode(() => reader.Open())
						.Throws<IOException>();
				}
			}
		}

		[TestFixture]
		public class Close
		{
			[Test, ExclusivelyUses("COMA")]
			public void Success_When_NotOpen()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					Check.ThatCode(() => reader.Close())
						.DoesNotThrow();
				}
			}

			[Test, ExclusivelyUses("COMA")]
			public void Success_When_Open()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					reader.Open();

					Check.ThatCode(() => reader.Close())
						.DoesNotThrow();
				}
			}

			[Test, ExclusivelyUses("COMA")]
			public void Exception_When_AlreadyDisposed()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					reader.Dispose();

					Check.ThatCode(() => reader.Close())
						.Throws<ObjectDisposedException>();
				}
			}
		}

		[TestFixture]
		public class Dispose
		{
			[Test, ExclusivelyUses("COMA")]
			public void Success_When_NotOpen()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					Check.ThatCode(() => reader.Dispose())
						.DoesNotThrow();
				}
			}

			[Test, ExclusivelyUses("COMA")]
			public void Success_When_Open()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					reader.Open();

					Check.ThatCode(() => reader.Dispose())
						.DoesNotThrow();
				}
			}

			[Test, ExclusivelyUses("COMA")]
			public void Success_When_AlreadyDisposed()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					reader.Dispose();

					Check.ThatCode(() => reader.Dispose())
						.DoesNotThrow();
				}
			}
		}

		[TestFixture]
		public class Transfer_Request
		{
			[Test, ExclusivelyUses("COMA")]
			public void Exception_When_AlreadyDisposed()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					reader.Dispose();

					var request = new FeigRequest { Command = FeigCommand.GetSoftwareVersion };

					Check.ThatAsyncCode(async () => await reader.Transfer(request, FeigProtocol.Advanced))
						.Throws<ObjectDisposedException>();
				}
			}

			[Test]
			public async Task Success_AllSpecified()
			{
				// arrange
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
					Address = 123,
					Protocol = FeigProtocol.Standard,
					Timeout = TimeSpan.FromMilliseconds(275)
				};

				var request = new FeigRequest {
					Command = FeigCommand.GetSoftwareVersion,
					Address = 236,
				};

				var response = new FeigResponse();

				var timeout = TimeSpan.FromMilliseconds(1000);
				var cts = new CancellationTokenSource();

				var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

				transport.Setup(x => x.Transfer(request, FeigProtocol.Advanced, timeout, cts.Token))
					.Returns(Task.FromResult(FeigTransferResult.Success(response)));

				var reader = new DefaultFeigReader(settings, transport.Object, new NoOpLogger());

				// act
				await reader.Transfer(request, FeigProtocol.Advanced, timeout, cts.Token);

				// assert
				transport.Verify(x => x.Transfer(request, FeigProtocol.Advanced, timeout, cts.Token), Times.Once);
			}

			[Test]
			public async Task Success_MinimumSpecified()
			{
				// arrange
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
					Address = 123,
					Protocol = FeigProtocol.Standard,
					Timeout = TimeSpan.FromMilliseconds(275)
				};

				var request = new FeigRequest {
					Command = FeigCommand.GetSoftwareVersion,
					Address = 236,
				};

				var response = new FeigResponse();

				var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

				transport.Setup(x => x.Transfer(request, FeigProtocol.Standard, TimeSpan.FromMilliseconds(275), default))
					.Returns(Task.FromResult(FeigTransferResult.Success(response)));

				var reader = new DefaultFeigReader(settings, transport.Object, new NoOpLogger());

				// act
				await reader.Transfer(request);

				// assert
				transport.Verify(x => x.Transfer(request, FeigProtocol.Standard, TimeSpan.FromMilliseconds(275), default), Times.Once);
			}
		}

		[TestFixture]
		public class Transfer_Command
		{
			[Test, ExclusivelyUses("COMA")]
			public void Exception_When_AlreadyDisposed()
			{
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
				};

				using (var reader = FeigReader.Create(settings))
				{
					reader.Dispose();

					Check.ThatAsyncCode(async () => await reader.Transfer(FeigCommand.BaudRateDetection))
						.Throws<ObjectDisposedException>();
				}
			}

			[Test]
			public async Task Success_AllSpecified()
			{
				// arrange
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
					Address = 123,
					Protocol = FeigProtocol.Standard,
					Timeout = TimeSpan.FromMilliseconds(275)
				};

				FeigRequest request = null;
				TimeSpan timeout = TimeSpan.Zero;

				var response = new FeigResponse();

				var cts = new CancellationTokenSource();

				var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

				transport.Setup(x => x.Transfer(It.IsAny<FeigRequest>(), FeigProtocol.Standard, It.IsAny<TimeSpan>(), cts.Token))
					.Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>((r, p, t, c) => { request = r; timeout = t; })
					.Returns(Task.FromResult(FeigTransferResult.Success(response)));

				var reader = new DefaultFeigReader(settings, transport.Object, new NoOpLogger());

				// act
				await reader.Transfer(
					FeigCommand.BaudRateDetection,
					BufferSpan.From(0x11, 0x22),
					TimeSpan.FromMilliseconds(500),
					cts.Token);

				// assert
				Check.That(request)
					.IsNotNull();
				Check.That(request.Address)
					.IsEqualTo(123);
				Check.That(request.Command)
					.IsEqualTo(FeigCommand.BaudRateDetection);
				Check.That(request.Data.ToArray())
					.ContainsExactly(0x11, 0x22);
				Check.That(timeout)
					.IsEqualTo(TimeSpan.FromMilliseconds(500));
			}

			[Test]
			public async Task Success_MinimumSpecified()
			{
				// arrange
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
					Address = 123,
					Protocol = FeigProtocol.Standard,
					Timeout = TimeSpan.FromMilliseconds(275)
				};

				FeigRequest request = null;
				TimeSpan timeout = TimeSpan.Zero;

				var response = new FeigResponse();
				var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

				transport.Setup(x => x.Transfer(It.IsAny<FeigRequest>(), FeigProtocol.Standard, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
					.Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>((r, p, t, c) => { request = r; timeout = t; })
					.Returns(Task.FromResult(FeigTransferResult.Success(response)));

				var reader = new DefaultFeigReader(settings, transport.Object, new NoOpLogger());

				// act
				await reader.Transfer(FeigCommand.BaudRateDetection);

				// assert
				Check.That(request)
					.IsNotNull();
				Check.That(request.Address)
					.IsEqualTo(123);
				Check.That(request.Command)
					.IsEqualTo(FeigCommand.BaudRateDetection);
				Check.That(request.Data.IsEmpty)
					.IsTrue();
				Check.That(timeout)
					.IsEqualTo(TimeSpan.FromMilliseconds(275));
			}
		}

		[TestFixture]
		public class TestCommunication
		{
			[Test]
			public async Task True_For_ResponseOK()
			{
				// arrange
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
					Address = 123,
					Protocol = FeigProtocol.Standard,
					Timeout = TimeSpan.FromMilliseconds(275)
				};

				FeigRequest request = null;
				TimeSpan timeout = TimeSpan.Zero;
				CancellationToken cancellationToken = default;

				var response = new FeigResponse { Status = FeigStatus.OK };

				var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

				transport.Setup(x => x.Transfer(It.IsAny<FeigRequest>(), FeigProtocol.Standard, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
					.Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>((r, p, t, c) => { request = r; timeout = t; cancellationToken = c; })
					.Returns(Task.FromResult(FeigTransferResult.Success(response)));

				var reader = new DefaultFeigReader(settings, transport.Object, new NoOpLogger());

				// act
				var result = await reader.TestCommunication();

				// assert
				Check.That(result)
					.IsTrue();

				Check.That(request)
					.IsNotNull();
				Check.That(request.Address)
					.IsEqualTo(123);
				Check.That(request.Command)
					.IsEqualTo(FeigCommand.BaudRateDetection);
				Check.That(request.Data.ToArray())
					.ContainsExactly(0x00);

				Check.That(timeout)
					.IsEqualTo(TimeSpan.FromMilliseconds(275));
				Check.That(cancellationToken)
					.IsEqualTo(default);
			}

			[Test]
			public async Task False_For_ResponseTimeout()
			{
				// arrange
				var settings = new FeigReaderSettings {
					TransportSettings = new SerialTransportSettings { PortName = "COMA" },
					Address = 123,
					Protocol = FeigProtocol.Standard,
					Timeout = TimeSpan.FromMilliseconds(275)
				};

				FeigRequest request = null;
				TimeSpan timeout = TimeSpan.Zero;
				CancellationToken cancellationToken = default;

				var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

				transport.Setup(x => x.Transfer(It.IsAny<FeigRequest>(), FeigProtocol.Standard, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
					.Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>((r, p, t, c) => { request = r; timeout = t; cancellationToken = c; })
					.Returns(Task.FromResult(FeigTransferResult.Timeout()));

				var reader = new DefaultFeigReader(settings, transport.Object, new NoOpLogger());

				// act
				var result = await reader.TestCommunication();

				// assert
				Check.That(result)
					.IsFalse();

				Check.That(request)
					.IsNotNull();
				Check.That(request.Address)
					.IsEqualTo(123);
				Check.That(request.Command)
					.IsEqualTo(FeigCommand.BaudRateDetection);
				Check.That(request.Data.ToArray())
					.ContainsExactly(0x00);

				Check.That(timeout)
					.IsEqualTo(TimeSpan.FromMilliseconds(275));
				Check.That(cancellationToken)
					.IsEqualTo(default);
			}
		}
	}
}
