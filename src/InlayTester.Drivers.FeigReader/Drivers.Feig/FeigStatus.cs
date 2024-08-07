﻿// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

// ReSharper disable InconsistentNaming

using System;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This enumeration lists possible response statuses.
/// </summary>
public enum FeigStatus : Byte
{
    /// <summary>
    ///     00h OK / Success
    /// </summary>
    OK = 0x00,

    /// <summary>
    ///     01h No Transponder (Success)
    /// </summary>
    NoTransponder = 0x01,

    /// <summary>
    ///     02h Data False
    /// </summary>
    DataFalse = 0x02,

    /// <summary>
    ///     03h Write Error
    /// </summary>
    WriteError = 0x03,

    /// <summary>
    ///     04h Address Error
    /// </summary>
    AddressError = 0x04,

    /// <summary>
    ///     05h Wrong Transponder Type
    /// </summary>
    WrongTransponderType = 0x05,

    /// <summary>
    ///     08h Authentication Error
    /// </summary>
    AuthenticationError = 0x08,

    /// <summary>
    ///     0Bh Collision Error
    /// </summary>
    CollisionError = 0x0B,

    /// <summary>
    ///     0Eh General Error
    /// </summary>
    GeneralError = 0x0E,

    /// <summary>
    ///     10h EEPROM Failure
    /// </summary>
    EEPROMFailure = 0x10,

    /// <summary>
    ///     11h Parameter Range Error
    /// </summary>
    ParameterRangeError = 0x11,

    /// <summary>
    ///     13h Login Request
    /// </summary>
    LoginRequest = 0x13,

    /// <summary>
    ///     14h Login Error
    /// </summary>
    LoginError = 0x14,

    /// <summary>
    ///     15h Read Protect
    /// </summary>
    ReadProtect = 0x15,

    /// <summary>
    ///     16h Write Protect
    /// </summary>
    WriteProtect = 0x16,

    /// <summary>
    ///     17h Firmware Activation Required
    /// </summary>
    FirmwareActivationRequired = 0x17,

    /// <summary>
    ///     31h No SAM Detected
    /// </summary>
    NoSAMDetected = 0x31,

    /// <summary>
    ///     32h Requested SAM Is Not Activated
    /// </summary>
    RequestedSAMIsNotActivated = 0x32,

    /// <summary>
    ///     33h Requested SAM Is Already Activated
    /// </summary>
    RequestedSAMIsAlreadyActivated = 0x33,

    /// <summary>
    ///     34h Requested Protocol Not Supported By SAM
    /// </summary>
    RequestedProtocolNotSupportedBySAM = 0x34,

    /// <summary>
    ///     35h SAM Communication Error
    /// </summary>
    SAMCommunicationError = 0x35,

    /// <summary>
    ///     36h SAM Timeout
    /// </summary>
    SAMTimeout = 0x36,

    /// <summary>
    ///     37h SAM Unsupported Baudrate
    /// </summary>
    SAMUnsupportedBaudrate = 0x37,

    /// <summary>
    ///     80h Unknown Command
    /// </summary>
    UnknownCommand = 0x80,

    /// <summary>
    ///     81h Length Error
    /// </summary>
    LengthError = 0x81,

    /// <summary>
    ///     82h Command Not Available
    /// </summary>
    CommandNotAvailable = 0x82,

    /// <summary>
    ///     83h RF Communication Error
    /// </summary>
    RFCommunicationError = 0x83,

    /// <summary>
    ///     84h RF Warning
    /// </summary>
    RFWarning = 0x84,

    /// <summary>
    ///     85h EPC Error
    /// </summary>
    EPCError = 0x85,

    /// <summary>
    ///     93h Data Buffer Overflow
    /// </summary>
    DataBufferOverflow = 0x93,

    /// <summary>
    ///     94h More Data
    /// </summary>
    MoreData = 0x94,

    /// <summary>
    ///     95h ISO 15693 Error
    /// </summary>
    ISO15693Error = 0x95,

    /// <summary>
    ///     96h ISO 14443 Error
    /// </summary>
    ISO14443Error = 0x96,

    /// <summary>
    ///     97h Crypto Processing Error
    /// </summary>
    CryptoProcessingError = 0x97,

    /// <summary>
    ///     F1h Hardware Warning
    /// </summary>
    HardwareWarning = 0xF1,
}
