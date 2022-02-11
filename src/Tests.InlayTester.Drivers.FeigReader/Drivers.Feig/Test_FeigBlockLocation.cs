// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_FeigBlockLocation
{
    [Test]
    public void TestNames()
    {
        Check.That(Enum.GetNames(typeof(FeigBlockLocation)))
           .IsOnlyMadeOf(nameof(FeigBlockLocation.RAM), nameof(FeigBlockLocation.EEPROM));
    }

    [Test]
    public void TestValues()
    {
        Check.That((Byte) FeigBlockLocation.RAM)
           .IsEqualTo(0x00);

        Check.That((Byte) FeigBlockLocation.EEPROM)
           .IsEqualTo(0x80);
    }
}
