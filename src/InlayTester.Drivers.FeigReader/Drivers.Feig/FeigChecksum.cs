// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using Amarok.Shared;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This type provides methods for calculating a CRC as used by Feig RFID readers.
/// </summary>
public static class FeigChecksum
{
    // From official Feig reader documentation:
    //
    // x16 + x12 + x5 + 1 = 0x8408
    // start value: 0xFFFF
    //
    // After long search, identified as "CRC-16/MCRF4XX"
    // http://reveng.sourceforge.net/crc-catalogue/all.htm

    // static data
    private static readonly CrcCalculator sCalculator = new(16, 0x1021, true, 0xffff, 0x0000, true, true);


    /// <summary>
    ///     Calculates the CRC for the given data.
    /// </summary>
    public static Int32 Calculate(in BufferSpan data)
    {
        return sCalculator.Calculate(data);
    }
}
