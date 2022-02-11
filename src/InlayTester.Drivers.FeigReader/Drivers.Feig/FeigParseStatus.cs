// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

namespace InlayTester.Drivers.Feig;


/// <summary>
///     This enumeration lists possible parse statuses.
/// </summary>
public enum FeigParseStatus
{
    /// <summary>
    ///     A response has been successfully parsed.
    /// </summary>
    Success = 0,

    /// <summary>
    ///     More data is needed for an entire frame.
    /// </summary>
    MoreDataNeeded = 1,

    /// <summary>
    ///     A checksum error has been detected.
    /// </summary>
    ChecksumError = -1,

    /// <summary>
    ///     An error in the protocol frame has been detected.
    /// </summary>
    FrameError = -2,
}
