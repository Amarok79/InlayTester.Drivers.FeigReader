// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_FeigParseStatus
{
    [Test]
    public void TestNames()
    {
        Check.That(Enum.GetNames(typeof(FeigParseStatus)))
           .IsOnlyMadeOf(
                nameof(FeigParseStatus.Success),
                nameof(FeigParseStatus.MoreDataNeeded),
                nameof(FeigParseStatus.ChecksumError),
                nameof(FeigParseStatus.FrameError)
            );
    }

    [Test]
    public void TestValues()
    {
        Check.That((Int32)FeigParseStatus.Success).IsEqualTo(0);

        Check.That((Int32)FeigParseStatus.MoreDataNeeded).IsEqualTo(1);

        Check.That((Int32)FeigParseStatus.ChecksumError).IsEqualTo(-1);

        Check.That((Int32)FeigParseStatus.FrameError).IsEqualTo(-2);
    }
}
