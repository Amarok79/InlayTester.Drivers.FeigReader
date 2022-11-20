// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_FeigTransferStatus
{
    [Test]
    public void TestNames()
    {
        Check.That(Enum.GetNames(typeof(FeigTransferStatus)))
           .IsOnlyMadeOf(
                nameof(FeigTransferStatus.Success),
                nameof(FeigTransferStatus.Canceled),
                nameof(FeigTransferStatus.Timeout),
                nameof(FeigTransferStatus.CommunicationError),
                nameof(FeigTransferStatus.UnexpectedResponse)
            );
    }

    [Test]
    public void TestValues()
    {
        Check.That((Int32)FeigTransferStatus.Success).IsEqualTo(0);

        Check.That((Int32)FeigTransferStatus.Canceled).IsEqualTo(1);

        Check.That((Int32)FeigTransferStatus.Timeout).IsEqualTo(-1);

        Check.That((Int32)FeigTransferStatus.CommunicationError).IsEqualTo(-2);

        Check.That((Int32)FeigTransferStatus.UnexpectedResponse).IsEqualTo(-3);
    }
}
