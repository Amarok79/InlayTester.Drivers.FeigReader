// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amarok.Shared;
using InlayTester.Shared.Transports;
using Microsoft.Extensions.Logging;
using Moq;
using NCrunch.Framework;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


public class Test_DefaultFeigReader
{
    [TestFixture]
    public class Open
    {
        [Test, NUnit.Framework.Category("com0com"), Serial]
        public void Success_For_FirstTime()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                Check.ThatCode(() => reader.Open()).DoesNotThrow();
            }
        }

        [Test]
        public void Exception_When_AlreadyDisposed()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                reader.Dispose();

                Check.ThatCode(() => reader.Open()).Throws<ObjectDisposedException>();
            }
        }

        [Test, NUnit.Framework.Category("com0com"), Serial]
        public void Exception_When_AlreadyOpen()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                Check.ThatCode(() => reader.Open()).DoesNotThrow();

                Check.ThatCode(() => reader.Open()).Throws<InvalidOperationException>();
            }
        }

        [Test]
        public void Exception_For_InvalidPortName()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "InvalidPortName" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                Check.ThatCode(() => reader.Open()).Throws<IOException>();
            }
        }
    }

    [TestFixture]
    public class Close
    {
        [Test]
        public void Success_When_NotOpen()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                Check.ThatCode(() => reader.Close()).DoesNotThrow();
            }
        }

        [Test, NUnit.Framework.Category("com0com"), Serial]
        public void Success_When_Open()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                reader.Open();

                Check.ThatCode(() => reader.Close()).DoesNotThrow();
            }
        }

        [Test]
        public void Exception_When_AlreadyDisposed()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                reader.Dispose();

                Check.ThatCode(() => reader.Close()).Throws<ObjectDisposedException>();
            }
        }
    }

    [TestFixture]
    public class Dispose
    {
        [Test]
        public void Success_When_NotOpen()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                Check.ThatCode(() => reader.Dispose()).DoesNotThrow();
            }
        }

        [Test, NUnit.Framework.Category("com0com"), Serial]
        public void Success_When_Open()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                reader.Open();

                Check.ThatCode(() => reader.Dispose()).DoesNotThrow();
            }
        }

        [Test]
        public void Success_When_AlreadyDisposed()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                reader.Dispose();

                Check.ThatCode(() => reader.Dispose()).DoesNotThrow();
            }
        }
    }

    [TestFixture]
    public class Transfer_Request
    {
        [Test]
        public void Exception_When_AlreadyDisposed()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                reader.Dispose();

                var request = new FeigRequest { Command = FeigCommand.GetSoftwareVersion };

                Check.ThatCode(async () => await reader.Transfer(request, FeigProtocol.Advanced))
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
                Timeout = TimeSpan.FromMilliseconds(275),
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
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

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
                Timeout = TimeSpan.FromMilliseconds(275),
            };

            var request = new FeigRequest {
                Command = FeigCommand.GetSoftwareVersion,
                Address = 236,
            };

            var response = new FeigResponse();

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(x => x.Transfer(request, FeigProtocol.Standard, TimeSpan.FromMilliseconds(275), default))
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            await reader.Transfer(request);

            // assert
            transport.Verify(
                x => x.Transfer(request, FeigProtocol.Standard, TimeSpan.FromMilliseconds(275), default),
                Times.Once
            );
        }
    }

    [TestFixture]
    public class Transfer_Command
    {
        [Test]
        public void Exception_When_AlreadyDisposed()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                reader.Dispose();

                Check.ThatCode(async () => await reader.Transfer(FeigCommand.BaudRateDetection))
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
                Timeout = TimeSpan.FromMilliseconds(275),
            };

            FeigRequest request = null;
            var timeout = TimeSpan.Zero;

            var response = new FeigResponse();

            var cts = new CancellationTokenSource();

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(It.IsAny<FeigRequest>(), FeigProtocol.Standard, It.IsAny<TimeSpan>(), cts.Token)
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            await reader.Transfer(
                FeigCommand.BaudRateDetection,
                BufferSpan.From(0x11, 0x22),
                TimeSpan.FromMilliseconds(500),
                cts.Token
            );

            // assert
            Check.That(request).IsNotNull();

            Check.That(request.Address).IsEqualTo(123);

            Check.That(request.Command).IsEqualTo(FeigCommand.BaudRateDetection);

            Check.That(request.Data.ToArray()).ContainsExactly(0x11, 0x22);

            Check.That(timeout).IsEqualTo(TimeSpan.FromMilliseconds(500));
        }

        [Test]
        public async Task Success_MinimumSpecified()
        {
            // arrange
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
                Address = 123,
                Protocol = FeigProtocol.Standard,
                Timeout = TimeSpan.FromMilliseconds(275),
            };

            FeigRequest request = null;
            var timeout = TimeSpan.Zero;

            var response = new FeigResponse();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Standard,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            await reader.Transfer(FeigCommand.BaudRateDetection);

            // assert
            Check.That(request).IsNotNull();

            Check.That(request.Address).IsEqualTo(123);

            Check.That(request.Command).IsEqualTo(FeigCommand.BaudRateDetection);

            Check.That(request.Data.IsEmpty).IsTrue();

            Check.That(timeout).IsEqualTo(TimeSpan.FromMilliseconds(275));
        }
    }

    [TestFixture]
    public class Execute_Request
    {
        [Test]
        public void Exception_When_AlreadyDisposed()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                reader.Dispose();

                var request = new FeigRequest { Command = FeigCommand.GetSoftwareVersion };

                Check.ThatCode(async () => await reader.Execute(request, FeigProtocol.Advanced))
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
                Timeout = TimeSpan.FromMilliseconds(275),
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
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var rsp = await reader.Execute(request, FeigProtocol.Advanced, timeout, cts.Token);

            // assert
            transport.Verify(x => x.Transfer(request, FeigProtocol.Advanced, timeout, cts.Token), Times.Once);

            Check.That(rsp).IsSameReferenceAs(response);
        }

        [Test]
        public async Task Success_MinimumSpecified()
        {
            // arrange
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
                Address = 123,
                Protocol = FeigProtocol.Standard,
                Timeout = TimeSpan.FromMilliseconds(275),
            };

            var request = new FeigRequest {
                Command = FeigCommand.GetSoftwareVersion,
                Address = 236,
            };

            var response = new FeigResponse();

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(x => x.Transfer(request, FeigProtocol.Standard, TimeSpan.FromMilliseconds(275), default))
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var rsp = await reader.Execute(request);

            // assert
            transport.Verify(
                x => x.Transfer(request, FeigProtocol.Standard, TimeSpan.FromMilliseconds(275), default),
                Times.Once
            );

            Check.That(rsp).IsSameReferenceAs(response);
        }

        [Test]
        public void Timeout()
        {
            var request = new FeigRequest();

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.Timeout(request)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            Check.ThatCode(async () => await reader.Execute(request)).Throws<TimeoutException>();
        }

        [Test]
        public void Canceled()
        {
            var request = new FeigRequest();

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.Canceled(request)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            Check.ThatCode(async () => await reader.Execute(request)).Throws<OperationCanceledException>();
        }

        [Test]
        public void CommunicationError()
        {
            var request = new FeigRequest();

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.CommunicationError(request)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            Check.ThatCode(async () => await reader.Execute(request))
               .Throws<FeigException>()
               .WithProperty(x => x.Request, request);
        }

        [Test]
        public void UnexpectedResponse()
        {
            var request = new FeigRequest();
            var response = new FeigResponse();

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.UnexpectedResponse(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            Check.ThatCode(async () => await reader.Execute(request))
               .Throws<FeigException>()
               .WithProperty(x => x.Request, request)
               .And.WithProperty(x => x.Response, response);
        }

        [Test]
        public void Success_NotOk()
        {
            var request = new FeigRequest();
            var response = new FeigResponse { Status = FeigStatus.GeneralError };

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            Check.ThatCode(async () => await reader.Execute(request))
               .Throws<FeigException>()
               .WithProperty(x => x.Request, request)
               .And.WithProperty(x => x.Response, response);
        }

        [Test]
        public async Task Success_OK()
        {
            var request = new FeigRequest();
            var response = new FeigResponse { Status = FeigStatus.OK };

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var rsp = await reader.Execute(request);

            Check.That(rsp).IsSameReferenceAs(response);
        }

        [Test]
        public async Task Success_NoTransponder()
        {
            var request = new FeigRequest();
            var response = new FeigResponse { Status = FeigStatus.NoTransponder };

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var rsp = await reader.Execute(request);

            Check.That(rsp).IsSameReferenceAs(response);
        }
    }

    [TestFixture]
    public class Execute_Command
    {
        [Test]
        public void Exception_When_AlreadyDisposed()
        {
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
            };

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            using (var reader = FeigReader.Create(settings, logger))
            {
                reader.Dispose();

                Check.ThatCode(async () => await reader.Execute(FeigCommand.GetSoftwareVersion))
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
                Timeout = TimeSpan.FromMilliseconds(275),
            };

            var request = new FeigRequest {
                Command = FeigCommand.GetSoftwareVersion,
                Address = 236,
            };

            var response = new FeigResponse();

            var timeout = TimeSpan.FromMilliseconds(1000);
            var cts = new CancellationTokenSource();

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(x => x.Transfer(It.IsAny<FeigRequest>(), FeigProtocol.Standard, timeout, cts.Token))
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var rsp = await reader.Execute(FeigCommand.GetSoftwareVersion, BufferSpan.Empty, timeout, cts.Token);

            // assert
            transport.Verify(
                x => x.Transfer(It.IsAny<FeigRequest>(), FeigProtocol.Standard, timeout, cts.Token),
                Times.Once
            );

            Check.That(rsp).IsSameReferenceAs(response);
        }

        [Test]
        public async Task Success_MinimumSpecified()
        {
            // arrange
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
                Address = 123,
                Protocol = FeigProtocol.Standard,
                Timeout = TimeSpan.FromMilliseconds(275),
            };

            var request = new FeigRequest {
                Command = FeigCommand.GetSoftwareVersion,
                Address = 236,
            };

            var response = new FeigResponse();

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Standard,
                        TimeSpan.FromMilliseconds(275),
                        default
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var rsp = await reader.Execute(FeigCommand.GetSoftwareVersion);

            // assert
            transport.Verify(
                x => x.Transfer(
                    It.IsAny<FeigRequest>(),
                    FeigProtocol.Standard,
                    TimeSpan.FromMilliseconds(275),
                    default
                ),
                Times.Once
            );

            Check.That(rsp).IsSameReferenceAs(response);
        }

        [Test]
        public void Timeout()
        {
            var request = new FeigRequest();

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.Timeout(request)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            Check.ThatCode(async () => await reader.Execute(FeigCommand.GetSoftwareVersion)).Throws<TimeoutException>();
        }

        [Test]
        public void Canceled()
        {
            var request = new FeigRequest();

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.Canceled(request)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            Check.ThatCode(async () => await reader.Execute(FeigCommand.GetSoftwareVersion))
               .Throws<OperationCanceledException>();
        }

        [Test]
        public void CommunicationError()
        {
            var request = new FeigRequest();

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.CommunicationError(request)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            Check.ThatCode(async () => await reader.Execute(FeigCommand.GetSoftwareVersion))
               .Throws<FeigException>()
               .WithProperty(x => x.Request, request);
        }

        [Test]
        public void UnexpectedResponse()
        {
            var request = new FeigRequest();
            var response = new FeigResponse();

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.UnexpectedResponse(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            Check.ThatCode(async () => await reader.Execute(FeigCommand.BaudRateDetection))
               .Throws<FeigException>()
               .WithProperty(x => x.Request, request)
               .And.WithProperty(x => x.Response, response);
        }

        [Test]
        public void Success_NotOk()
        {
            var request = new FeigRequest();
            var response = new FeigResponse { Status = FeigStatus.GeneralError };

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            Check.ThatCode(async () => await reader.Execute(FeigCommand.GetSoftwareVersion))
               .Throws<FeigException>()
               .WithProperty(x => x.Request, request)
               .And.WithProperty(x => x.Response, response);
        }

        [Test]
        public async Task Success_OK()
        {
            var request = new FeigRequest();
            var response = new FeigResponse { Status = FeigStatus.OK };

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var rsp = await reader.Execute(FeigCommand.CPUReset);

            Check.That(rsp).IsSameReferenceAs(response);
        }

        [Test]
        public async Task Success_NoTransponder()
        {
            var request = new FeigRequest();
            var response = new FeigResponse { Status = FeigStatus.NoTransponder };

            // arrange
            var settings = new FeigReaderSettings();
            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        It.IsAny<FeigProtocol>(),
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Returns(Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var rsp = await reader.Execute(FeigCommand.CPUReset);

            Check.That(rsp).IsSameReferenceAs(response);
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
                Timeout = TimeSpan.FromMilliseconds(275),
            };

            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var response = new FeigResponse { Status = FeigStatus.OK };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Standard,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var result = await reader.TestCommunication();

            // assert
            Check.That(result).IsTrue();

            Check.That(request).IsNotNull();

            Check.That(request.Address).IsEqualTo(123);

            Check.That(request.Command).IsEqualTo(FeigCommand.BaudRateDetection);

            Check.That(request.Data.ToArray()).ContainsExactly(0x00);

            Check.That(timeout).IsEqualTo(TimeSpan.FromMilliseconds(275));
        }

        [Test]
        public async Task False_For_ResponseTimeout()
        {
            // arrange
            var settings = new FeigReaderSettings {
                TransportSettings = new SerialTransportSettings { PortName = "COMA" },
                Address = 123,
                Protocol = FeigProtocol.Standard,
                Timeout = TimeSpan.FromMilliseconds(275),
            };

            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Standard,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Timeout(request)));

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var result = await reader.TestCommunication();

            // assert
            Check.That(result).IsFalse();

            Check.That(request).IsNotNull();

            Check.That(request.Address).IsEqualTo(123);

            Check.That(request.Command).IsEqualTo(FeigCommand.BaudRateDetection);

            Check.That(request.Data.ToArray()).ContainsExactly(0x00);

            Check.That(timeout).IsEqualTo(TimeSpan.FromMilliseconds(275));
        }
    }

    [TestFixture]
    public class ResetCPU
    {
        [Test]
        public async Task Success()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var response = new FeigResponse { Status = FeigStatus.OK };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            await reader.ResetCPU();

            // assert
            Check.That(request.Command).IsEqualTo(FeigCommand.CPUReset);

            Check.That(request.Data.ToArray()).IsEmpty();
        }
    }

    [TestFixture]
    public class ResetRF
    {
        [Test]
        public async Task Success()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var response = new FeigResponse { Status = FeigStatus.OK };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            await reader.ResetRF();

            // assert
            Check.That(request.Command).IsEqualTo(FeigCommand.RFReset);

            Check.That(request.Data.ToArray()).IsEmpty();
        }
    }

    [TestFixture]
    public class SwitchRF
    {
        [Test]
        public async Task Success()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var response = new FeigResponse { Status = FeigStatus.OK };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            await reader.SwitchRF(0x23);

            // assert
            Check.That(request.Command).IsEqualTo(FeigCommand.RFOutputOnOff);

            Check.That(request.Data.ToArray()).ContainsExactly(0x23);
        }
    }

    [TestFixture]
    public class GetSoftwareInfo
    {
        [Test]
        public async Task Success()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var data = new Byte[] { 0x03, 0x03, 0x00, 0x44, 0x53, 0x0D, 0x30 };

            var response = new FeigResponse {
                Status = FeigStatus.OK,
                Data = BufferSpan.From(data),
            };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var info = await reader.GetSoftwareInfo();

            // assert
            Check.That(request.Command).IsEqualTo(FeigCommand.GetSoftwareVersion);

            Check.That(request.Data.ToArray()).IsEmpty();

            Check.That(info.FirmwareVersion.Major).IsEqualTo(3);

            Check.That(info.FirmwareVersion.Minor).IsEqualTo(3);

            Check.That(info.FirmwareVersion.Build).IsEqualTo(0);

            Check.That(info.HardwareType).IsEqualTo(0x44);

            Check.That(info.ReaderType).IsEqualTo(FeigReaderType.CPR40);

            Check.That(info.SupportedTransponders).IsEqualTo(0x0D30);
        }
    }

    [TestFixture]
    public class ReadConfiguration
    {
        [Test]
        public async Task Success()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var data = new Byte[14];

            var response = new FeigResponse {
                Status = FeigStatus.OK,
                Data = BufferSpan.From(data),
            };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var result = await reader.ReadConfiguration(3, FeigBlockLocation.EEPROM);

            // assert
            Check.That(result.Count).IsEqualTo(14);

            Check.That(request.Command).IsEqualTo(FeigCommand.ReadConfiguration);

            Check.That(request.Data.ToArray()).ContainsExactly(0x83);
        }
    }

    [TestFixture]
    public class WriteConfiguration
    {
        [Test]
        public async Task Success()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var response = new FeigResponse {
                Status = FeigStatus.OK,
                Data = BufferSpan.Empty,
            };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            await reader.WriteConfiguration(
                3,
                FeigBlockLocation.EEPROM,
                BufferSpan.From(
                    0x11,
                    0x22,
                    0x33,
                    0x44,
                    0x55,
                    0x66,
                    0x77,
                    0x88,
                    0x99,
                    0xAA,
                    0xBB,
                    0xCC,
                    0xDD,
                    0xEE
                )
            );

            // assert
            Check.That(request.Command).IsEqualTo(FeigCommand.WriteConfiguration);

            Check.That(request.Data.ToArray())
           .ContainsExactly(
                0x83,
                0x11,
                0x22,
                0x33,
                0x44,
                0x55,
                0x66,
                0x77,
                0x88,
                0x99,
                0xAA,
                0xBB,
                0xCC,
                0xDD,
                0xEE
            );
        }
    }

    [TestFixture]
    public class SaveConfigurations
    {
        [Test]
        public async Task Success()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var response = new FeigResponse {
                Status = FeigStatus.OK,
                Data = BufferSpan.Empty,
            };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            await reader.SaveConfigurations();

            // assert
            Check.That(request.Command).IsEqualTo(FeigCommand.SaveConfiguration);

            Check.That(request.Data.ToArray()).ContainsExactly(0x40);
        }
    }

    [TestFixture]
    public class SaveConfiguration
    {
        [Test]
        public async Task Success()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var response = new FeigResponse {
                Status = FeigStatus.OK,
                Data = BufferSpan.Empty,
            };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            await reader.SaveConfiguration(13);

            // assert
            Check.That(request.Command).IsEqualTo(FeigCommand.SaveConfiguration);

            Check.That(request.Data.ToArray()).ContainsExactly(0x0D);
        }
    }

    [TestFixture]
    public class ResetConfigurations
    {
        [Test]
        public async Task Success()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var response = new FeigResponse {
                Status = FeigStatus.OK,
                Data = BufferSpan.Empty,
            };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            await reader.ResetConfigurations(FeigBlockLocation.EEPROM);

            // assert
            Check.That(request.Command).IsEqualTo(FeigCommand.SetDefaultConfiguration);

            Check.That(request.Data.ToArray()).ContainsExactly(0xC0);
        }
    }

    [TestFixture]
    public class ResetConfiguration
    {
        [Test]
        public async Task Success()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var response = new FeigResponse {
                Status = FeigStatus.OK,
                Data = BufferSpan.Empty,
            };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            await reader.ResetConfiguration(13, FeigBlockLocation.EEPROM);

            // assert
            Check.That(request.Command).IsEqualTo(FeigCommand.SetDefaultConfiguration);

            Check.That(request.Data.ToArray()).ContainsExactly(0x8D);
        }
    }


    [TestFixture]
    public class Inventory
    {
        [Test]
        public void Inventory_Parse_ISO14443A__1()
        {
            var data = BufferSpan.From(
                0x00,
                0xFF,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11,
                0xDD
            );

            var transponder = DefaultFeigReader.Inventory_Parse_ISO14443A(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.ISO14443A);

            Check.That(transponder.Identifier.ToArray()).ContainsExactly(0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_ISO14443A__2()
        {
            var data = BufferSpan.From(
                0x24,
                0xFF,
                0xAA,
                0x99,
                0x88,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11,
                0xDD
            );

            var transponder = DefaultFeigReader.Inventory_Parse_ISO14443A(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.ISO14443A);

            Check.That(transponder.Identifier.ToArray())
           .ContainsExactly(
                0xAA,
                0x99,
                0x88,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11
            );

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_ISO14443B()
        {
            var data = BufferSpan.From(
                0xFF,
                0xAA,
                0xBB,
                0xCC,
                0xDD,
                0x11,
                0x22,
                0x33,
                0x44,
                0xDD
            );

            var transponder = DefaultFeigReader.Inventory_Parse_ISO14443B(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.ISO14443B);

            Check.That(transponder.Identifier.ToArray()).ContainsExactly(0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_Innovatron()
        {
            var data = BufferSpan.From(
                0x11,
                0x22,
                0x33,
                0x44,
                0x55,
                0x66,
                0x77,
                0x88,
                0xAA, // VERLOG
                0xBB, // CONFIG
                0x04, // ATR-LEN
                0xCC,
                0xDD,
                0xEE,
                0xFF,
                0x13
            );

            var transponder = DefaultFeigReader.Inventory_Parse_Innovatron(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.Innovatron);

            Check.That(transponder.Identifier.ToArray())
               .ContainsExactly(0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88);

            Check.That(data.ToArray()).ContainsExactly(0x13);
        }

        [Test]
        public void Inventory_Parse_Jewel()
        {
            var data = BufferSpan.From(
                0x00,
                0x00,
                0x01,
                0x3C,
                0x11,
                0x22,
                0x33,
                0x44,
                0xDD
            );

            var transponder = DefaultFeigReader.Inventory_Parse_Jewel(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.Jewel);

            Check.That(transponder.Identifier.ToArray()).ContainsExactly(0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_SR176()
        {
            var data = BufferSpan.From(
                0xFF,
                0x11,
                0x22,
                0x33,
                0x44,
                0x55,
                0x66,
                0x77,
                0x88,
                0xDD
            );

            var transponder = DefaultFeigReader.Inventory_Parse_SR176(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.SR176);

            Check.That(transponder.Identifier.ToArray())
               .ContainsExactly(0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_SRIxx()
        {
            var data = BufferSpan.From(
                0xFF,
                0x11,
                0x22,
                0x33,
                0x44,
                0x55,
                0x66,
                0x77,
                0x88,
                0xDD
            );

            var transponder = DefaultFeigReader.Inventory_Parse_SRIxx(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.SRIxx);

            Check.That(transponder.Identifier.ToArray())
               .ContainsExactly(0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_FeliCa()
        {
            var data = BufferSpan.From(
                0xFF, // LENGTH
                0x11,
                0x22,
                0x33,
                0x44,
                0x55,
                0x66,
                0x77,
                0x88,
                0xAA,
                0xBB,
                0xCC,
                0xDD,
                0xEE,
                0xFF,
                0xAB,
                0xAC,
                0x13
            );

            var transponder = DefaultFeigReader.Inventory_Parse_FeliCa(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.FeliCa);

            Check.That(transponder.Identifier.ToArray())
               .ContainsExactly(0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88);

            Check.That(data.ToArray()).ContainsExactly(0x13);
        }

        [Test]
        public void Inventory_Parse_ISO15693()
        {
            var data = BufferSpan.From(
                0xFF,
                0x88,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11,
                0xDD
            );

            var transponder = DefaultFeigReader.Inventory_Parse_ISO15693(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.ISO15693);

            Check.That(transponder.Identifier.ToArray())
               .ContainsExactly(0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_ISO18000_3M3()
        {
            var data = BufferSpan.From(0xFF, 0x05, 0x55, 0x44, 0x33, 0x22, 0x11, 0xDD);

            var transponder = DefaultFeigReader.Inventory_Parse_ISO18000_3M3(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.ISO18000_3M3);

            Check.That(transponder.Identifier.ToArray()).ContainsExactly(0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_EPC_Class1_Gen2()
        {
            var data = BufferSpan.From(0xFF, 0x05, 0x55, 0x44, 0x33, 0x22, 0x11, 0xDD);

            var transponder = DefaultFeigReader.Inventory_Parse_EPC_Class1_Gen2(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.EPC_Class1_Gen2);

            Check.That(transponder.Identifier.ToArray()).ContainsExactly(0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_ICode1()
        {
            var data = BufferSpan.From(
                0xFF,
                0x88,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11,
                0xDD
            );

            var transponder = DefaultFeigReader.Inventory_Parse_ICode1(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.ICode1);

            Check.That(transponder.Identifier.ToArray())
               .ContainsExactly(0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_ICodeEPC()
        {
            var data = BufferSpan.From(
                0x88,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11,
                0xDD
            );

            var transponder = DefaultFeigReader.Inventory_Parse_ICodeEPC(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.ICodeEPC);

            Check.That(transponder.Identifier.ToArray())
               .ContainsExactly(0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_ICodeUID()
        {
            var data = BufferSpan.From(
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11,
                0xDD
            );

            var transponder = DefaultFeigReader.Inventory_Parse_ICodeUID(ref data);

            Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.ICodeUID);

            Check.That(transponder.Identifier.ToArray()).ContainsExactly(0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse()
        {
            var data = BufferSpan.From(
                12,
                0x04,
                0x24,
                0xFF,
                0xAA,
                0x99,
                0x88,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11, // ISO14443A
                0x04,
                0x00,
                0xFF,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11, // ISO14443A
                0x05,
                0xFF,
                0xAA,
                0xBB,
                0xCC,
                0xDD,
                0x11,
                0x22,
                0x33,
                0x44, // ISO14443B
                0x08,
                0x00,
                0x00,
                0x01,
                0x3C,
                0x11,
                0x22,
                0x33,
                0x44, // Jewel
                0x0A,
                0xFF,
                0x11,
                0x22,
                0x33,
                0x44,
                0x55,
                0x66,
                0x77,
                0x88, // SR176
                0x0B,
                0xFF,
                0x11,
                0x22,
                0x33,
                0x44,
                0x55,
                0x66,
                0x77,
                0x88, // SRIxx
                0x03,
                0xFF,
                0x88,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11, // ISO15693
                0x09,
                0xFF,
                0x05,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11, // ISO18000_3M3
                0x84,
                0xFF,
                0x05,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11, // EPC_Class1_Gen2
                0x06,
                0x88,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11, // I-CodeEPC
                0x00,
                0xFF,
                0x88,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11, // I-Code1
                0x07,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0xFF,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11, // I-CodeUID
                0xDD
            );

            var transponders = DefaultFeigReader.Inventory_Parse(ref data);

            Check.That(transponders[0].TransponderType).IsEqualTo(FeigTransponderType.ISO14443A);

            Check.That(transponders[0].Identifier.ToArray())
               .ContainsExactly(
                    0xAA,
                    0x99,
                    0x88,
                    0x77,
                    0x66,
                    0x55,
                    0x44,
                    0x33,
                    0x22,
                    0x11
                );

            Check.That(transponders[1].TransponderType).IsEqualTo(FeigTransponderType.ISO14443A);

            Check.That(transponders[1].Identifier.ToArray()).ContainsExactly(0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(transponders[2].TransponderType).IsEqualTo(FeigTransponderType.ISO14443B);

            Check.That(transponders[2].Identifier.ToArray()).ContainsExactly(0x44, 0x33, 0x22, 0x11);

            Check.That(transponders[3].TransponderType).IsEqualTo(FeigTransponderType.Jewel);

            Check.That(transponders[3].Identifier.ToArray()).ContainsExactly(0x44, 0x33, 0x22, 0x11);

            Check.That(transponders[4].TransponderType).IsEqualTo(FeigTransponderType.SR176);

            Check.That(transponders[4].Identifier.ToArray())
               .ContainsExactly(0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(transponders[5].TransponderType).IsEqualTo(FeigTransponderType.SRIxx);

            Check.That(transponders[5].Identifier.ToArray())
               .ContainsExactly(0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(transponders[6].TransponderType).IsEqualTo(FeigTransponderType.ISO15693);

            Check.That(transponders[6].Identifier.ToArray())
               .ContainsExactly(0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(transponders[7].TransponderType).IsEqualTo(FeigTransponderType.ISO18000_3M3);

            Check.That(transponders[7].Identifier.ToArray()).ContainsExactly(0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(transponders[8].TransponderType).IsEqualTo(FeigTransponderType.EPC_Class1_Gen2);

            Check.That(transponders[8].Identifier.ToArray()).ContainsExactly(0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(transponders[9].TransponderType).IsEqualTo(FeigTransponderType.ICodeEPC);

            Check.That(transponders[9].Identifier.ToArray())
               .ContainsExactly(0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(transponders[10].TransponderType).IsEqualTo(FeigTransponderType.ICode1);

            Check.That(transponders[10].Identifier.ToArray())
               .ContainsExactly(0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(transponders[11].TransponderType).IsEqualTo(FeigTransponderType.ICodeUID);

            Check.That(transponders[11].Identifier.ToArray()).ContainsExactly(0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(data.ToArray()).ContainsExactly(0xDD);
        }

        [Test]
        public void Inventory_Parse_UnsupportedTransponderType()
        {
            var data = BufferSpan.From(
                0x01,
                0xAD,
                0xAA,
                0x99,
                0x88,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11,
                0xDD
            );

            Check.ThatCode(() => DefaultFeigReader.Inventory_Parse(ref data)).Throws<NotSupportedException>();
        }

        [Test]
        public async Task Success()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var data = new Byte[] {
                0x01,
                0x04,
                0x00,
                0xFF,
                0x77,
                0x66,
                0x55,
                0x44,
                0x33,
                0x22,
                0x11,
            };

            var response = new FeigResponse {
                Status = FeigStatus.OK,
                Data = BufferSpan.From(data),
            };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var result = await reader.Inventory();

            // assert
            Check.That(result.Transponders.Length).IsEqualTo(1);

            Check.That(result.Transponders[0].TransponderType).IsEqualTo(FeigTransponderType.ISO14443A);

            Check.That(result.Transponders[0].Identifier.ToArray())
               .ContainsExactly(0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11);

            Check.That(request.Command).IsEqualTo(FeigCommand.ISOStandardHostCommand);

            Check.That(request.Data.ToArray()).ContainsExactly((Byte)FeigISOStandardCommand.Inventory, 0x00);
        }

        [Test]
        public async Task NoTransponder()
        {
            // arrange
            FeigRequest request = null;
            var timeout = TimeSpan.Zero;
            CancellationToken cancellationToken = default;

            var response = new FeigResponse {
                Status = FeigStatus.NoTransponder,
                Data = BufferSpan.Empty,
            };

            var transport = new Mock<IFeigTransport>(MockBehavior.Strict);

            transport.Setup(
                    x => x.Transfer(
                        It.IsAny<FeigRequest>(),
                        FeigProtocol.Advanced,
                        It.IsAny<TimeSpan>(),
                        It.IsAny<CancellationToken>()
                    )
                )
               .Callback<FeigRequest, FeigProtocol, TimeSpan, CancellationToken>(
                    (r, p, t, c) => {
                        request = r;
                        timeout = t;
                        cancellationToken = c;
                    }
                )
               .Returns(() => Task.FromResult(FeigTransferResult.Success(request, response)));

            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = new DefaultFeigReader(settings, transport.Object, logger);

            // act
            var result = await reader.Inventory();

            // assert
            Check.That(result.Transponders.Length).IsEqualTo(0);

            Check.That(request.Command).IsEqualTo(FeigCommand.ISOStandardHostCommand);

            Check.That(request.Data.ToArray()).ContainsExactly((Byte)FeigISOStandardCommand.Inventory, 0x00);
        }
    }
}
