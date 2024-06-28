// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

// ReSharper disable InconsistentNaming

using System;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This enumeration lists well-known Feig reader types.
/// </summary>
public enum FeigReaderType : Byte
{
    /// <summary>
    ///     0  Unknown
    /// </summary>
    Unknown = 0,


    /// <summary>
    ///     30  ISC.M01
    /// </summary>
    ISC_M01 = 30,

    /// <summary>
    ///     31  ISC.M02
    /// </summary>
    ISC_M02 = 31,

    /// <summary>
    ///     33  ISC.M02_M8
    /// </summary>
    ISC_M02_M8 = 33,

    /// <summary>
    ///     40  ISC.LR100
    /// </summary>
    ISC_LR100 = 40,

    /// <summary>
    ///     41  ISC.LR200
    /// </summary>
    ISC_LR200 = 41,

    /// <summary>
    ///     42  ISC.LR2000
    /// </summary>
    ISC_LR2000 = 42,

    /// <summary>
    ///     43  ISC.LR2500-B
    /// </summary>
    ISC_LR2500B = 43,

    /// <summary>
    ///     44  ISC.LR2500-A
    /// </summary>
    ISC_LR2500A = 44,

    /// <summary>
    ///     45  ISC.LR1002
    /// </summary>
    ISC_LR1002 = 45,

    /// <summary>
    ///     50  ISC.MU02
    /// </summary>
    ISC_MU02 = 50,

    /// <summary>
    ///     54  ISC.MRU102
    /// </summary>
    ISC_MRU102 = 54,

    /// <summary>
    ///     55  ISC.MRU200
    /// </summary>
    ISC_MRU200 = 55,

    /// <summary>
    ///     56  ISC.MRU200-U
    /// </summary>
    ISC_MRU200U = 56,

    /// <summary>
    ///     60  ISC.PRH101
    /// </summary>
    ISC_PRH101 = 60,

    /// <summary>
    ///     61  ISC.PRH101-U
    /// </summary>
    ISC_PRH101U = 61,

    /// <summary>
    ///     62  ISC.PRHD102
    /// </summary>
    ISC_PRHD102 = 62,

    /// <summary>
    ///     63  ISC.PRH102
    /// </summary>
    ISC_PRH102 = 63,

    /// <summary>
    ///     64  ISC.PRH200
    /// </summary>
    ISC_PRH200 = 64,

    /// <summary>
    ///     71  ISC.PRH100–U
    /// </summary>
    ISC_PRH100U = 71,

    /// <summary>
    ///     72  ISC.PRH100
    /// </summary>
    ISC_PRH100 = 72,

    /// <summary>
    ///     73  ISC.MR100–U
    /// </summary>
    ISC_MR100U = 73,

    /// <summary>
    ///     74  ISC.MR100
    /// </summary>
    ISC_MR100 = 74,

    /// <summary>
    ///     75  ISC.MR200
    /// </summary>
    ISC_MR200 = 75,

    /// <summary>
    ///     76  ISC.MR101
    /// </summary>
    ISC_MR101 = 76,

    /// <summary>
    ///     77  ISC.MR102
    /// </summary>
    ISC_MR102 = 77,

    /// <summary>
    ///     78  ISC.MR101-U
    /// </summary>
    ISC_MR101U = 78,

    /// <summary>
    ///     80  CPR.M02
    /// </summary>
    CPR_M02 = 80,

    /// <summary>
    ///     81  CPR.02
    /// </summary>
    CPR_02 = 81,

    /// <summary>
    ///     82  CPR40.xx-U
    /// </summary>
    CPR40U = 82,

    /// <summary>
    ///     83  CPR40
    /// </summary>
    CPR40 = 83,

    /// <summary>
    ///     84  CPR50
    /// </summary>
    CPR50 = 84,

    /// <summary>
    ///     85  CPR44
    /// </summary>
    CPR44 = 85,

    /// <summary>
    ///     86  CPR30
    /// </summary>
    CPR30 = 86,

    /// <summary>
    ///     87  CPR52
    /// </summary>
    CPR52 = 87,

    /// <summary>
    ///     88  CPR.04-U
    /// </summary>
    CPR_04U = 88,

    /// <summary>
    ///     89  CPR46
    /// </summary>
    CPR46 = 89,

    /// <summary>
    ///     91  ISC.LRU1002
    /// </summary>
    ISC_LRU1002 = 91,

    /// <summary>
    ///     92  ISC.LRU1000
    /// </summary>
    ISC_LRU1000 = 92,

    /// <summary>
    ///     93  ISC.LRU2000
    /// </summary>
    ISC_LRU2000 = 93,

    /// <summary>
    ///     94  ISC.LRU3000
    /// </summary>
    ISC_LRU3000 = 94,

    /// <summary>
    ///     100  MAX50
    /// </summary>
    MAX50 = 100,

    /// <summary>
    ///     101  MAXU1002
    /// </summary>
    MAXU1002 = 101,

    /// <summary>
    ///     111  CPR47
    /// </summary>
    CPR47 = 111,

    /// <summary>
    ///     114  CPR74
    /// </summary>
    CPR74 = 114,
}
