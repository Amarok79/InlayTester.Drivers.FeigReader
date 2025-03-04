﻿// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using InlayTester.Drivers.Feig;
using InlayTester.Shared.Transports;
using Microsoft.Extensions.Logging.Abstractions;


namespace InlayTester;


public static class Program
{
    public static async Task Main()
    {
        //var config = new LogConfiguration {
        //    FactoryAdapter = new FactoryAdapterConfiguration {
        //        Type = typeof(NLogLoggerFactoryAdapter).AssemblyQualifiedName,
        //        Arguments = new NameValueCollection {
        //            { "configType", "FILE" }, { "configFile", "./nlog.config" },
        //        },
        //    },
        //};

        //LogManager.Configure(config);

        //var log = LogManager.GetLogger("Feig");

        var log = NullLogger.Instance;

        var settings = new FeigReaderSettings {
            TransportSettings = new SerialTransportSettings {
                PortName  = "COM3",
                Baud      = 38400,
                DataBits  = 8,
                Parity    = Parity.Even,
                StopBits  = StopBits.One,
                Handshake = Handshake.None,
            },
            Address  = 255,
            Protocol = FeigProtocol.Advanced,
            Timeout  = TimeSpan.FromMilliseconds(1500),
        };

        using (var reader = FeigReader.Create(settings, log))
        {
            reader.Open();

            var sw = new Stopwatch();


            //await reader.ResetConfigurations(FeigBlockLocation.EEPROM)
            //	.ConfigureAwait(false);

            //await reader.ResetCPU()
            //	.ConfigureAwait(false);

            var cfg1 = await reader.ReadConfiguration(1, FeigBlockLocation.RAM).ConfigureAwait(false);

            cfg1.Buffer[cfg1.Offset + 7] = 10; // TR-RESPONSE-TIME: 1 sec

            await reader.WriteConfiguration(1, FeigBlockLocation.RAM, cfg1).ConfigureAwait(false);

            var cfg3 = await reader.ReadConfiguration(3, FeigBlockLocation.RAM).ConfigureAwait(false);

            cfg3.Buffer[cfg3.Offset + 0] = 0x00;
            cfg3.Buffer[cfg3.Offset + 1] = 0x10;

            await reader.WriteConfiguration(3, FeigBlockLocation.RAM, cfg3).ConfigureAwait(false);


            for (var i = 0; i < 1000000; i++)
            {
                if (i % 1000 == 0)
                    Console.WriteLine(i);

                try
                {
                    sw.Restart();

                    try
                    {
                        await reader.SwitchRF(0x00).ConfigureAwait(false);

                        await reader.SwitchRF(0x01).ConfigureAwait(false);

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
