// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_FeigSoftwareInfo
{
    [Test]
    public void Construction_Defaults()
    {
        // act
        var info = new FeigSoftwareInfo();

        // assert
        Check.That(info.FirmwareVersion).IsEqualTo(new Version(0, 0, 0));

        Check.That(info.HardwareType).IsEqualTo(0x00);

        Check.That(info.ReaderType).IsEqualTo(FeigReaderType.Unknown);

        Check.That(info.SupportedTransponders).IsEqualTo(0x0000);

        Check.That(info.ToString())
            .IsEqualTo(
                "FirmwareVersion: 0.0.0, HardwareType: 0x00, ReaderType: Unknown, SupportedTransponders: 0x0000"
            );
    }

    [Test]
    public void Construction_Copy()
    {
        // act
        var copy = new FeigSoftwareInfo {
            FirmwareVersion       = new Version(3, 4, 0),
            HardwareType          = 0x34,
            ReaderType            = FeigReaderType.CPR40,
            SupportedTransponders = 0x1234,
        };

        var info = new FeigSoftwareInfo(copy);

        // assert
        Check.That(info.FirmwareVersion).IsEqualTo(new Version(3, 4, 0));

        Check.That(info.HardwareType).IsEqualTo(0x34);

        Check.That(info.ReaderType).IsEqualTo(FeigReaderType.CPR40);

        Check.That(info.SupportedTransponders).IsEqualTo(0x1234);

        Check.That(info.ToString())
            .IsEqualTo("FirmwareVersion: 3.4.0, HardwareType: 0x34, ReaderType: CPR40, SupportedTransponders: 0x1234");
    }
}
