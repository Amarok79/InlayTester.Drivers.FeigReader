// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

using System;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This enumeration lists supported ISO Standard commands.
/// </summary>
public enum FeigISOStandardCommand : Byte
{
    /// <summary>
    ///     00h None
    /// </summary>
    None = 0x00,


    /// <summary>
    ///     01h Inventory
    /// </summary>
    Inventory = 0x01,

    /// <summary>
    ///     23h Read Multiple Blocks
    /// </summary>
    ReadMultipleBlocks = 0x23,

    /// <summary>
    ///     24h Write Multiple Blocks
    /// </summary>
    WriteMultipleBlocks = 0x24,

    /// <summary>
    ///     25h Select
    /// </summary>
    Select = 0x25,
}
