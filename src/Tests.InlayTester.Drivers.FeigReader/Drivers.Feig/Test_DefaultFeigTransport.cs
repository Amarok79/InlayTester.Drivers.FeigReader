// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.Threading;
using System.Threading.Tasks;
using Amarok.Shared;
using InlayTester.Shared.Transports;
using Microsoft.Extensions.Logging;
using NCrunch.Framework;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_DefaultFeigTransport
{
    [Test, NUnit.Framework.Category("com0com"), Serial]
    public void Open_Close_Dispose()
    {
        var settingsA = new SerialTransportSettings { PortName = "COMA" };

        var logger = LoggerFactory.Create(
                builder => {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddSimpleConsole();
                }
            )
           .CreateLogger("Test");

        using (var transportA = new DefaultFeigTransport(settingsA, logger))
        {
            transportA.Open();
            transportA.Close();
        }
    }

    [Test, NUnit.Framework.Category("com0com"), Serial]
    public void ReceivedDataIgnored()
    {
        var settingsA = new SerialTransportSettings { PortName = "COMA" };

        var logger = LoggerFactory.Create(
                builder => {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddSimpleConsole();
                }
            )
           .CreateLogger("Test");

        using (var transportA = new DefaultFeigTransport(settingsA, logger))
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

                transportB.Send(
                    BufferSpan.From(
                        0x02,
                        0x00,
                        0x0f,
                        0x00,
                        0x65,
                        0x00,
                        0x03,
                        0x03,
                        0x00,
                        0x44,
                        0x53,
                        0x0d,
                        0x30,
                        0x74,
                        0x69
                    )
                );

                Thread.Sleep(2000);
            }
        }
    }

    [Test, NUnit.Framework.Category("com0com"), Serial]
    public async Task Success_ReceivedResponse()
    {
        var settingsA = new SerialTransportSettings { PortName = "COMA" };

        var logger = LoggerFactory.Create(
                builder => {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddSimpleConsole();
                }
            )
           .CreateLogger("Test");

        using (var transportA = new DefaultFeigTransport(settingsA, logger))
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

                transportB.Received.Subscribe(
                    data => {
                        if (data[0] == 0x02 &&
                            data[1] == 0x00 &&
                            data[2] == 0x07 &&
                            data[3] == 0xff &&
                            data[4] == 0x65 &&
                            data[5] == 0x6e &&
                            data[6] == 0x61)
                        {
                            transportB.Send(
                                BufferSpan.From(
                                    0x02,
                                    0x00,
                                    0x0f,
                                    0x00,
                                    0x65,
                                    0x00,
                                    0x03,
                                    0x03,
                                    0x00,
                                    0x44,
                                    0x53,
                                    0x0d,
                                    0x30,
                                    0x74,
                                    0x69
                                )
                            );
                        }
                        else
                        {
                            Assert.Fail("Received unknown data");
                        }
                    }
                );

                var result = await transportA.Transfer(
                    new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
                    FeigProtocol.Advanced,
                    TimeSpan.FromMilliseconds(5000),
                    default
                );

                Check.That(result.Status).IsEqualTo(FeigTransferStatus.Success);

                Check.That(result.Response.Address).IsEqualTo(0x00);

                Check.That(result.Response.Command).IsEqualTo(FeigCommand.GetSoftwareVersion);

                Check.That(result.Response.Status).IsEqualTo(FeigStatus.OK);

                Check.That(result.Response.Data.ToArray()).ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0d, 0x30);
            }
        }
    }

    [Test, NUnit.Framework.Category("com0com"), Serial]
    public async Task Success_ReceivedResponse_MultiplePackets()
    {
        var settingsA = new SerialTransportSettings { PortName = "COMA" };

        var logger = LoggerFactory.Create(
                builder => {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddSimpleConsole();
                }
            )
           .CreateLogger("Test");

        using (var transportA = new DefaultFeigTransport(settingsA, logger))
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

                transportB.Received.Subscribe(
                    data => {
                        if (data[0] == 0x02 &&
                            data[1] == 0x00 &&
                            data[2] == 0x07 &&
                            data[3] == 0xff &&
                            data[4] == 0x65 &&
                            data[5] == 0x6e &&
                            data[6] == 0x61)
                        {
                            transportB.Send(BufferSpan.From(0x02, 0x00, 0x0f, 0x00, 0x65, 0x00, 0x03, 0x03));

                            Thread.Sleep(100);

                            transportB.Send(BufferSpan.From(0x00, 0x44, 0x53, 0x0d, 0x30, 0x74, 0x69));
                        }
                        else
                        {
                            Assert.Fail("Received unknown data");
                        }
                    }
                );

                var result = await transportA.Transfer(
                    new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
                    FeigProtocol.Advanced,
                    TimeSpan.FromMilliseconds(5000),
                    default
                );

                Check.That(result.Status).IsEqualTo(FeigTransferStatus.Success);

                Check.That(result.Response.Address).IsEqualTo(0x00);

                Check.That(result.Response.Command).IsEqualTo(FeigCommand.GetSoftwareVersion);

                Check.That(result.Response.Status).IsEqualTo(FeigStatus.OK);

                Check.That(result.Response.Data.ToArray()).ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0d, 0x30);
            }
        }
    }

    [Test, NUnit.Framework.Category("com0com"), Serial]
    public async Task Timeout()
    {
        var settingsA = new SerialTransportSettings { PortName = "COMA" };

        var logger = LoggerFactory.Create(
                builder => {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddSimpleConsole();
                }
            )
           .CreateLogger("Test");

        using (var transportA = new DefaultFeigTransport(settingsA, logger))
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

                transportB.Received.Subscribe(
                    data => {
                        if (data[0] == 0x02 &&
                            data[1] == 0x00 &&
                            data[2] == 0x07 &&
                            data[3] == 0xff &&
                            data[4] == 0x65 &&
                            data[5] == 0x6e &&
                            data[6] == 0x61)
                        {
                            Thread.Sleep(500);

                            transportB.Send(
                                BufferSpan.From(
                                    0x02,
                                    0x00,
                                    0x0f,
                                    0x00,
                                    0x65,
                                    0x00,
                                    0x03,
                                    0x03,
                                    0x00,
                                    0x44,
                                    0x53,
                                    0x0d,
                                    0x30,
                                    0x74,
                                    0x69
                                )
                            );
                        }
                        else
                        {
                            Assert.Fail("Received unknown data");
                        }
                    }
                );

                var result = await transportA.Transfer(
                    new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
                    FeigProtocol.Advanced,
                    TimeSpan.FromMilliseconds(100),
                    default
                );

                Check.That(result.Status).IsEqualTo(FeigTransferStatus.Timeout);

                Check.That(result.Response).IsNull();

                Thread.Sleep(1000);
            }
        }
    }

    [Test, NUnit.Framework.Category("com0com"), Serial]
    public async Task Canceled()
    {
        var settingsA = new SerialTransportSettings { PortName = "COMA" };

        var logger = LoggerFactory.Create(
                builder => {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddSimpleConsole();
                }
            )
           .CreateLogger("Test");

        using (var transportA = new DefaultFeigTransport(settingsA, logger))
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

                transportB.Received.Subscribe(
                    data => {
                        if (data[0] == 0x02 &&
                            data[1] == 0x00 &&
                            data[2] == 0x07 &&
                            data[3] == 0xff &&
                            data[4] == 0x65 &&
                            data[5] == 0x6e &&
                            data[6] == 0x61)
                        {
                            Thread.Sleep(500);

                            transportB.Send(
                                BufferSpan.From(
                                    0x02,
                                    0x00,
                                    0x0f,
                                    0x00,
                                    0x65,
                                    0x00,
                                    0x03,
                                    0x03,
                                    0x00,
                                    0x44,
                                    0x53,
                                    0x0d,
                                    0x30,
                                    0x74,
                                    0x69
                                )
                            );
                        }
                        else
                        {
                            Assert.Fail("Received unknown data");
                        }
                    }
                );

                var cts = new CancellationTokenSource();

                var task = transportA.Transfer(
                    new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
                    FeigProtocol.Advanced,
                    TimeSpan.FromMilliseconds(5000),
                    cts.Token
                );

                cts.Cancel();

                var result = await task;

                Check.That(result.Status).IsEqualTo(FeigTransferStatus.Canceled);

                Check.That(result.Response).IsNull();

                Thread.Sleep(1000);
            }
        }
    }

    [Test, NUnit.Framework.Category("com0com"), Serial]
    public async Task CommunicationError_ChecksumError()
    {
        var settingsA = new SerialTransportSettings { PortName = "COMA" };

        var logger = LoggerFactory.Create(
                builder => {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddSimpleConsole();
                }
            )
           .CreateLogger("Test");

        using (var transportA = new DefaultFeigTransport(settingsA, logger))
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

                transportB.Received.Subscribe(
                    data => {
                        if (data[0] == 0x02 &&
                            data[1] == 0x00 &&
                            data[2] == 0x07 &&
                            data[3] == 0xff &&
                            data[4] == 0x65 &&
                            data[5] == 0x6e &&
                            data[6] == 0x61)
                        {
                            transportB.Send(
                                BufferSpan.From(
                                    0x02,
                                    0x00,
                                    0x0f,
                                    0x00,
                                    0x65,
                                    0x00,
                                    0x03,
                                    0x03,
                                    0xFF,
                                    0x44,
                                    0x53,
                                    0x0d,
                                    0x30,
                                    0x74,
                                    0x69
                                )
                            );
                        }
                        else
                        {
                            Assert.Fail("Received unknown data");
                        }
                    }
                );

                var result = await transportA.Transfer(
                    new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
                    FeigProtocol.Advanced,
                    TimeSpan.FromMilliseconds(5000),
                    default
                );

                Check.That(result.Status).IsEqualTo(FeigTransferStatus.CommunicationError);

                Check.That(result.Response).IsNull();
            }
        }
    }

    [Test, NUnit.Framework.Category("com0com"), Serial]
    public async Task CommunicationError_FrameError()
    {
        var settingsA = new SerialTransportSettings { PortName = "COMA" };

        var logger = LoggerFactory.Create(
                builder => {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddSimpleConsole();
                }
            )
           .CreateLogger("Test");

        using (var transportA = new DefaultFeigTransport(settingsA, logger))
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

                transportB.Received.Subscribe(
                    data => {
                        if (data[0] == 0x02 &&
                            data[1] == 0x00 &&
                            data[2] == 0x07 &&
                            data[3] == 0xff &&
                            data[4] == 0x65 &&
                            data[5] == 0x6e &&
                            data[6] == 0x61)
                        {
                            transportB.Send(
                                BufferSpan.From(
                                    0xFF,
                                    0x00,
                                    0x0f,
                                    0x00,
                                    0x65,
                                    0x00,
                                    0x03,
                                    0x03,
                                    0x00,
                                    0x44,
                                    0x53,
                                    0x0d,
                                    0x30,
                                    0x74,
                                    0x69
                                )
                            );
                        }
                        else
                        {
                            Assert.Fail("Received unknown data");
                        }
                    }
                );

                var result = await transportA.Transfer(
                    new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
                    FeigProtocol.Advanced,
                    TimeSpan.FromMilliseconds(5000),
                    default
                );

                Check.That(result.Status).IsEqualTo(FeigTransferStatus.CommunicationError);

                Check.That(result.Response).IsNull();
            }
        }
    }

    [Test, NUnit.Framework.Category("com0com"), Serial]
    public async Task UnexpectedResponse()
    {
        var settingsA = new SerialTransportSettings { PortName = "COMA" };

        var logger = LoggerFactory.Create(
                builder => {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddSimpleConsole();
                }
            )
           .CreateLogger("Test");

        using (var transportA = new DefaultFeigTransport(settingsA, logger))
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

                transportB.Received.Subscribe(
                    data => {
                        if (data[0] == 0x02 &&
                            data[1] == 0x00 &&
                            data[2] == 0x07 &&
                            data[3] == 0xff &&
                            data[4] == 0x65 &&
                            data[5] == 0x6e &&
                            data[6] == 0x61)
                        {
                            transportB.Send(
                                BufferSpan.From(
                                    0x02,
                                    0x00,
                                    0x08,
                                    0xff,
                                    0x80,
                                    0x81,
                                    0x40,
                                    0x3a,
                                    0x02,
                                    0x00,
                                    0x08,
                                    0xff,
                                    0x80,
                                    0x82,
                                    0xdb,
                                    0x08,
                                    0x02,
                                    0x00,
                                    0x08,
                                    0xff,
                                    0x80,
                                    0x83,
                                    0x52
                                )
                            );
                        }
                        else
                        {
                            Assert.Fail("Received unknown data");
                        }
                    }
                );

                var result = await transportA.Transfer(
                    new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
                    FeigProtocol.Advanced,
                    TimeSpan.FromMilliseconds(5000),
                    default
                );

                Check.That(result.Status).IsEqualTo(FeigTransferStatus.UnexpectedResponse);

                Check.That(result.Response.Address).IsEqualTo(0xff);

                Check.That(result.Response.Command).IsEqualTo(FeigCommand.ReadConfiguration);

                Check.That(result.Response.Status).IsEqualTo(FeigStatus.LengthError);
            }
        }
    }

    [Test, NUnit.Framework.Category("com0com"), Serial]
    public async Task Success_ReceivedResponse_MultipleTimes()
    {
        var settingsA = new SerialTransportSettings { PortName = "COMA" };

        var logger = LoggerFactory.Create(
                builder => {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddSimpleConsole();
                }
            )
           .CreateLogger("Test");

        using (var transportA = new DefaultFeigTransport(settingsA, logger))
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

                transportB.Received.Subscribe(
                    data => {
                        if (data[0] == 0x02 &&
                            data[1] == 0x00 &&
                            data[2] == 0x07 &&
                            data[3] == 0xff &&
                            data[4] == 0x65 &&
                            data[5] == 0x6e &&
                            data[6] == 0x61)
                        {
                            transportB.Send(
                                BufferSpan.From(
                                    0x02,
                                    0x00,
                                    0x0f,
                                    0x00,
                                    0x65,
                                    0x00,
                                    0x03,
                                    0x03,
                                    0x00,
                                    0x44,
                                    0x53,
                                    0x0d,
                                    0x30,
                                    0x74,
                                    0x69
                                )
                            );
                        }
                        else
                        {
                            Assert.Fail("Received unknown data");
                        }
                    }
                );

                for (var i = 0; i < 100; i++)
                {
                    var result = await transportA.Transfer(
                        new FeigRequest { Command = FeigCommand.GetSoftwareVersion },
                        FeigProtocol.Advanced,
                        TimeSpan.FromMilliseconds(5000),
                        default
                    );

                    Check.That(result.Status).IsEqualTo(FeigTransferStatus.Success);

                    Check.That(result.Response.Address).IsEqualTo(0x00);

                    Check.That(result.Response.Command).IsEqualTo(FeigCommand.GetSoftwareVersion);

                    Check.That(result.Response.Status).IsEqualTo(FeigStatus.OK);

                    Check.That(result.Response.Data.ToArray())
                       .ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0d, 0x30);
                }
            }
        }
    }
}
