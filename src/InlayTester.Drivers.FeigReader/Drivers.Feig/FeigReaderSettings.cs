// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.Text;
using Amarok.Contracts;
using Amarok.Shared;
using InlayTester.Shared.Transports;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This type contains settings for the Feig RFID reader.
/// </summary>
public sealed class FeigReaderSettings
{
    /// <summary>
    ///     The transport settings for serial communication. Defaults to COM1,38400,8,e,n,None.
    /// </summary>
    public SerialTransportSettings TransportSettings { get; set; }

    /// <summary>
    ///     The address of the RFID reader. Defaults to 255.
    /// </summary>
    public Byte Address { get; set; }

    /// <summary>
    ///     The default timeout for communication. Defaults to 1000 ms.
    /// </summary>
    public TimeSpan Timeout { get; set; }

    /// <summary>
    ///     The default protocol for communication. Defaults to <see cref="FeigProtocol.Advanced"/>.
    /// </summary>
    public FeigProtocol Protocol { get; set; }


    /// <summary>
    ///     Initializes a new instance.
    /// </summary>
    public FeigReaderSettings()
    {
        TransportSettings = new SerialTransportSettings {
            PortName  = "COM1",
            Baud      = 38400,
            DataBits  = 8,
            Parity    = Parity.Even,
            StopBits  = StopBits.One,
            Handshake = Handshake.None,
        };

        Address  = 0xFF;
        Timeout  = TimeSpan.FromMilliseconds(1000);
        Protocol = FeigProtocol.Advanced;
    }

    /// <summary>
    ///     Initializes a new instance.
    /// </summary>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public FeigReaderSettings(FeigReaderSettings settings)
    {
        Verify.NotNull(settings, nameof(settings));

        TransportSettings = new SerialTransportSettings(settings.TransportSettings);
        Address           = settings.Address;
        Timeout           = settings.Timeout;
        Protocol          = settings.Protocol;
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

            sb.Append("Transport: '");
            sb.Append(TransportSettings);
            sb.Append("', Address: ");
            sb.Append(Address);
            sb.Append(", Timeout: ");
            sb.Append(Timeout.TotalMilliseconds);
            sb.Append(" ms, Protocol: ");
            sb.Append(Protocol);

            return sb.ToString();
        }
        finally
        {
            StringBuilderPool.Free(sb);
        }
    }
}
