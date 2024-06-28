// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

// ReSharper disable InconsistentNaming

using System;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This enumeration lists supported commands.
/// </summary>
public enum FeigCommand : Byte
{
    /// <summary>
    ///     00h None
    /// </summary>
    None = 0x00,


    /// <summary>
    ///     52h Baud Rate Detection
    /// </summary>
    BaudRateDetection = 0x52,

    /// <summary>
    ///     55h Start Flash Loader
    /// </summary>
    StartFlashLoader = 0x55,

    /// <summary>
    ///     63h CPU Reset
    /// </summary>
    CPUReset = 0x63,

    /// <summary>
    ///     65h Get Software Version
    /// </summary>
    GetSoftwareVersion = 0x65,

    /// <summary>
    ///     66h Get Reader Info
    /// </summary>
    GetReaderInfo = 0x66,

    /// <summary>
    ///     69h RF Reset
    /// </summary>
    RFReset = 0x69,

    /// <summary>
    ///     6Ah RF Output On/Off
    /// </summary>
    RFOutputOnOff = 0x6A,

    /// <summary>
    ///     72h Set Output
    /// </summary>
    SetOutput = 0x72,

    /// <summary>
    ///     A0h Reader Login
    /// </summary>
    ReaderLogin = 0xA0,

    /// <summary>
    ///     80h Read Configuration
    /// </summary>
    ReadConfiguration = 0x80,

    /// <summary>
    ///     81h Write Configuration
    /// </summary>
    WriteConfiguration = 0x81,

    /// <summary>
    ///     82h Save Configuration
    /// </summary>
    SaveConfiguration = 0x82,

    /// <summary>
    ///     83h Set Default Configuration
    /// </summary>
    SetDefaultConfiguration = 0x83,

    /// <summary>
    ///     A2h Write Mifare Reader Keys
    /// </summary>
    WriteMifareReaderKeys = 0xA2,

    /// <summary>
    ///     B0h ISO Standard Host Command
    /// </summary>
    ISOStandardHostCommand = 0xB0,

    /// <summary>
    ///     B2h ISO14443 Special Host Command
    /// </summary>
    ISO14443SpecialHostCommand = 0xB2,

    /// <summary>
    ///     BDh ISO14443A Transparent Command
    /// </summary>
    ISO14443ATransparentCommand = 0xBD,

    /// <summary>
    ///     BEh ISO14443B Transparent Command
    /// </summary>
    ISO14443BTransparentCommand = 0xBE,

    /// <summary>
    ///     BCh Command Queue
    /// </summary>
    CommandQueue = 0xBC,
}
