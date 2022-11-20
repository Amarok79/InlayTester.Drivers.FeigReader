// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.Globalization;
using System.Text;
using Amarok.Contracts;
using Amarok.Shared;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This type contains information about a Feig reader/module software.
/// </summary>
public sealed class FeigSoftwareInfo
{
    /// <summary>
    ///     The firmware version.
    /// </summary>
    public Version FirmwareVersion { get; set; }

    /// <summary>
    ///     The hardware type.
    /// </summary>
    public Byte HardwareType { get; set; }

    /// <summary>
    ///     The reader type.
    /// </summary>
    public FeigReaderType ReaderType { get; set; }

    /// <summary>
    ///     The transponders supported by the firmware.
    /// </summary>
    public Int32 SupportedTransponders { get; set; }


    /// <summary>
    ///     Initializes a new instance.
    /// </summary>
    public FeigSoftwareInfo()
    {
        FirmwareVersion = new Version(0, 0, 0);
        ReaderType = FeigReaderType.Unknown;
    }

    /// <summary>
    ///     Initializes a new instance.
    /// </summary>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public FeigSoftwareInfo(FeigSoftwareInfo info)
    {
        Verify.NotNull(info, nameof(info));

        FirmwareVersion = info.FirmwareVersion;
        HardwareType = info.HardwareType;
        ReaderType = info.ReaderType;
        SupportedTransponders = info.SupportedTransponders;
    }


    /// <summary>
    ///     Returns a string that represents the current instance.
    /// </summary>
    public override String ToString()
    {
        StringBuilder? sb = null;

        try
        {
            sb = StringBuilderPool.Rent();

            sb.Append("FirmwareVersion: ");
            sb.Append(FirmwareVersion);
            sb.Append(", HardwareType: 0x");
            sb.Append(HardwareType.ToString("X2", CultureInfo.InvariantCulture));
            sb.Append(", ReaderType: ");
            sb.Append(ReaderType);
            sb.Append(", SupportedTransponders: 0x");
            sb.Append(SupportedTransponders.ToString("X4", CultureInfo.InvariantCulture));

            return sb.ToString();
        }
        finally
        {
            StringBuilderPool.Free(sb);
        }
    }
}
