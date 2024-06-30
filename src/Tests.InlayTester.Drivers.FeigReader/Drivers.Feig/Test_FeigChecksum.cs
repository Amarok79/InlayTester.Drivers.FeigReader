// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.Text;
using Amarok.Shared;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_FeigChecksum
{
    [Test]
    public void UseCase()
    {
        var buffer = new Byte[] {
            0x0D, 0x00, 0x65, 0x00, 0x03, 0x03, 0x00, 0x44,
            0x53, 0x0D, 0x30,
        };

        var crc = FeigChecksum.Calculate(BufferSpan.From(buffer));

        Check.That(crc).IsEqualTo(0x0933);
    }

    [Test]
    public void UseCase_123456789()
    {
        var buffer = Encoding.UTF8.GetBytes("123456789");

        var crc = FeigChecksum.Calculate(BufferSpan.From(buffer));

        Check.That(crc).IsEqualTo(0x6F91);
    }
}
