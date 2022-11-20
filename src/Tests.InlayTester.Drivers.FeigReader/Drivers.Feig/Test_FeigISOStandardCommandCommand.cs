// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_FeigISOStandardCommandCommand
{
    [Test]
    public void TestNames()
    {
        Check.That(Enum.GetNames(typeof(FeigISOStandardCommand)))
           .IsOnlyMadeOf(
                nameof(FeigISOStandardCommand.None),
                nameof(FeigISOStandardCommand.Inventory),
                nameof(FeigISOStandardCommand.ReadMultipleBlocks),
                nameof(FeigISOStandardCommand.WriteMultipleBlocks),
                nameof(FeigISOStandardCommand.Select)
            );
    }

    [Test]
    public void TestValues()
    {
        Check.That((Byte)FeigISOStandardCommand.None).IsEqualTo(0x00);

        Check.That((Byte)FeigISOStandardCommand.Inventory).IsEqualTo(0x01);

        Check.That((Byte)FeigISOStandardCommand.ReadMultipleBlocks).IsEqualTo(0x23);

        Check.That((Byte)FeigISOStandardCommand.WriteMultipleBlocks).IsEqualTo(0x24);

        Check.That((Byte)FeigISOStandardCommand.Select).IsEqualTo(0x25);
    }
}
