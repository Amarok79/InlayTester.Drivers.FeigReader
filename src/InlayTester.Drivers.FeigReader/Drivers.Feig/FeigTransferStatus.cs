// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

namespace InlayTester.Drivers.Feig;


/// <summary>
///     This enumeration lists possible transfer statuses.
/// </summary>
public enum FeigTransferStatus
{
    /// <summary>
    ///     The transfer completed successful. A response has been received.
    /// </summary>
    Success = 0,

    /// <summary>
    ///     The transfer has been canceled. No response has been received.
    /// </summary>
    Canceled = 1,

    /// <summary>
    ///     The transfer has timed out. No response has been received.
    /// </summary>
    Timeout = -1,

    /// <summary>
    ///     The transfer failed, because the received response cannot be decoded.
    /// </summary>
    CommunicationError = -2,

    /// <summary>
    ///     The transfer failed, because an unexpected response has been received.
    /// </summary>
    UnexpectedResponse = -3,
}
