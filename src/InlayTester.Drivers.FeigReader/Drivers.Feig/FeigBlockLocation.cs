// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

using System;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This enumeration lists possible block locations.
/// </summary>
public enum FeigBlockLocation : Byte
{
    /// <summary>
    ///     The configuration block is located in the RFID reader/module's RAM.
    /// </summary>
    RAM = 0x00,

    /// <summary>
    ///     The configuration block is located in the RFID reader/module's EEPROM.
    /// </summary>
    EEPROM = 0x80,
}
