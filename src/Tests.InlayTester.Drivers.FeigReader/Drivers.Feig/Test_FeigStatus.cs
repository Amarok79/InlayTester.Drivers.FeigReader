﻿// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_FeigStatus
{
    [Test]
    public void TestNames()
    {
        Check.That(Enum.GetNames(typeof(FeigStatus)))
            .IsOnlyMadeOf(
                nameof(FeigStatus.OK),
                nameof(FeigStatus.NoTransponder),
                nameof(FeigStatus.DataFalse),
                nameof(FeigStatus.WriteError),
                nameof(FeigStatus.AddressError),
                nameof(FeigStatus.WrongTransponderType),
                nameof(FeigStatus.AuthenticationError),
                nameof(FeigStatus.CollisionError),
                nameof(FeigStatus.GeneralError),
                nameof(FeigStatus.EEPROMFailure),
                nameof(FeigStatus.ParameterRangeError),
                nameof(FeigStatus.LoginRequest),
                nameof(FeigStatus.LoginError),
                nameof(FeigStatus.ReadProtect),
                nameof(FeigStatus.WriteProtect),
                nameof(FeigStatus.FirmwareActivationRequired),
                nameof(FeigStatus.NoSAMDetected),
                nameof(FeigStatus.RequestedSAMIsNotActivated),
                nameof(FeigStatus.RequestedSAMIsAlreadyActivated),
                nameof(FeigStatus.RequestedProtocolNotSupportedBySAM),
                nameof(FeigStatus.SAMCommunicationError),
                nameof(FeigStatus.SAMTimeout),
                nameof(FeigStatus.SAMUnsupportedBaudrate),
                nameof(FeigStatus.UnknownCommand),
                nameof(FeigStatus.LengthError),
                nameof(FeigStatus.CommandNotAvailable),
                nameof(FeigStatus.RFCommunicationError),
                nameof(FeigStatus.RFWarning),
                nameof(FeigStatus.EPCError),
                nameof(FeigStatus.DataBufferOverflow),
                nameof(FeigStatus.MoreData),
                nameof(FeigStatus.ISO15693Error),
                nameof(FeigStatus.ISO14443Error),
                nameof(FeigStatus.CryptoProcessingError),
                nameof(FeigStatus.HardwareWarning)
            );
    }

    [Test]
    public void TestValues()
    {
        Check.That((Byte)FeigStatus.OK).IsEqualTo(0x00);

        Check.That((Byte)FeigStatus.NoTransponder).IsEqualTo(0x01);

        Check.That((Byte)FeigStatus.DataFalse).IsEqualTo(0x02);

        Check.That((Byte)FeigStatus.WriteError).IsEqualTo(0x03);

        Check.That((Byte)FeigStatus.AddressError).IsEqualTo(0x04);

        Check.That((Byte)FeigStatus.WrongTransponderType).IsEqualTo(0x05);

        Check.That((Byte)FeigStatus.AuthenticationError).IsEqualTo(0x08);

        Check.That((Byte)FeigStatus.CollisionError).IsEqualTo(0x0B);

        Check.That((Byte)FeigStatus.GeneralError).IsEqualTo(0x0E);

        Check.That((Byte)FeigStatus.EEPROMFailure).IsEqualTo(0x10);

        Check.That((Byte)FeigStatus.ParameterRangeError).IsEqualTo(0x11);

        Check.That((Byte)FeigStatus.LoginRequest).IsEqualTo(0x13);

        Check.That((Byte)FeigStatus.LoginError).IsEqualTo(0x14);

        Check.That((Byte)FeigStatus.ReadProtect).IsEqualTo(0x15);

        Check.That((Byte)FeigStatus.WriteProtect).IsEqualTo(0x16);

        Check.That((Byte)FeigStatus.FirmwareActivationRequired).IsEqualTo(0x17);

        Check.That((Byte)FeigStatus.NoSAMDetected).IsEqualTo(0x31);

        Check.That((Byte)FeigStatus.RequestedSAMIsNotActivated).IsEqualTo(0x32);

        Check.That((Byte)FeigStatus.RequestedSAMIsAlreadyActivated).IsEqualTo(0x33);

        Check.That((Byte)FeigStatus.RequestedProtocolNotSupportedBySAM).IsEqualTo(0x34);

        Check.That((Byte)FeigStatus.SAMCommunicationError).IsEqualTo(0x35);

        Check.That((Byte)FeigStatus.SAMTimeout).IsEqualTo(0x36);

        Check.That((Byte)FeigStatus.SAMUnsupportedBaudrate).IsEqualTo(0x37);

        Check.That((Byte)FeigStatus.UnknownCommand).IsEqualTo(0x80);

        Check.That((Byte)FeigStatus.LengthError).IsEqualTo(0x81);

        Check.That((Byte)FeigStatus.CommandNotAvailable).IsEqualTo(0x82);

        Check.That((Byte)FeigStatus.RFCommunicationError).IsEqualTo(0x83);

        Check.That((Byte)FeigStatus.RFWarning).IsEqualTo(0x84);

        Check.That((Byte)FeigStatus.EPCError).IsEqualTo(0x85);

        Check.That((Byte)FeigStatus.DataBufferOverflow).IsEqualTo(0x93);

        Check.That((Byte)FeigStatus.MoreData).IsEqualTo(0x94);

        Check.That((Byte)FeigStatus.ISO15693Error).IsEqualTo(0x95);

        Check.That((Byte)FeigStatus.ISO14443Error).IsEqualTo(0x96);

        Check.That((Byte)FeigStatus.CryptoProcessingError).IsEqualTo(0x97);

        Check.That((Byte)FeigStatus.HardwareWarning).IsEqualTo(0xF1);
    }
}
