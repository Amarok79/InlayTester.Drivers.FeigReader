// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_FeigProtocol
{
    [Test]
    public void TestNames()
    {
        Check.That(Enum.GetNames(typeof(FeigProtocol)))
            .IsOnlyMadeOf(nameof(FeigProtocol.Standard), nameof(FeigProtocol.Advanced));
    }

    [Test]
    public void TestValues()
    {
        Check.That((Int32)FeigProtocol.Standard).IsEqualTo(0);

        Check.That((Int32)FeigProtocol.Advanced).IsEqualTo(1);
    }
}
