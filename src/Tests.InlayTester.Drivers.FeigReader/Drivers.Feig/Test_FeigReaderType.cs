﻿// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_FeigReaderType
{
    [Test]
    public void TestNames()
    {
        Check.That(Enum.GetNames(typeof(FeigReaderType)))
            .IsOnlyMadeOf(
                nameof(FeigReaderType.CPR30),
                nameof(FeigReaderType.CPR40),
                nameof(FeigReaderType.CPR40U),
                nameof(FeigReaderType.CPR44),
                nameof(FeigReaderType.CPR46),
                nameof(FeigReaderType.CPR47),
                nameof(FeigReaderType.CPR50),
                nameof(FeigReaderType.CPR52),
                nameof(FeigReaderType.CPR74),
                nameof(FeigReaderType.CPR_02),
                nameof(FeigReaderType.CPR_04U),
                nameof(FeigReaderType.CPR_M02),
                nameof(FeigReaderType.ISC_LR100),
                nameof(FeigReaderType.ISC_LR1002),
                nameof(FeigReaderType.ISC_LR200),
                nameof(FeigReaderType.ISC_LR2000),
                nameof(FeigReaderType.ISC_LR2500A),
                nameof(FeigReaderType.ISC_LR2500B),
                nameof(FeigReaderType.ISC_LRU1000),
                nameof(FeigReaderType.ISC_LRU1002),
                nameof(FeigReaderType.ISC_LRU2000),
                nameof(FeigReaderType.ISC_LRU3000),
                nameof(FeigReaderType.ISC_M01),
                nameof(FeigReaderType.ISC_M02),
                nameof(FeigReaderType.ISC_M02_M8),
                nameof(FeigReaderType.ISC_MR100),
                nameof(FeigReaderType.ISC_MR100U),
                nameof(FeigReaderType.ISC_MR101),
                nameof(FeigReaderType.ISC_MR101U),
                nameof(FeigReaderType.ISC_MR102),
                nameof(FeigReaderType.ISC_MR200),
                nameof(FeigReaderType.ISC_MRU102),
                nameof(FeigReaderType.ISC_MRU200),
                nameof(FeigReaderType.ISC_MRU200U),
                nameof(FeigReaderType.ISC_MU02),
                nameof(FeigReaderType.ISC_PRH100),
                nameof(FeigReaderType.ISC_PRH100U),
                nameof(FeigReaderType.ISC_PRH101),
                nameof(FeigReaderType.ISC_PRH101U),
                nameof(FeigReaderType.ISC_PRH102),
                nameof(FeigReaderType.ISC_PRH200),
                nameof(FeigReaderType.ISC_PRHD102),
                nameof(FeigReaderType.MAX50),
                nameof(FeigReaderType.MAXU1002),
                nameof(FeigReaderType.Unknown)
            );
    }

    [Test]
    public void TestValues()
    {
        Check.That((Byte)FeigReaderType.Unknown).IsEqualTo(0);

        Check.That((Byte)FeigReaderType.CPR40).IsEqualTo(83);

        Check.That((Byte)FeigReaderType.CPR74).IsEqualTo(114);
    }
}
