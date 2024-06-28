// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

// ReSharper disable InconsistentNaming

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
