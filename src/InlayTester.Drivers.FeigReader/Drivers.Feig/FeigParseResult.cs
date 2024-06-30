// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.Text;
using Amarok.Contracts;
using Amarok.Shared;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This type represents the result of a <see cref="FeigResponse.TryParse"/> invocation.
/// </summary>
public readonly struct FeigParseResult
{
    /// <summary>
    ///     Gets the status of parse operation.
    /// </summary>
    public FeigParseStatus Status { get; }

    /// <summary>
    ///     Gets the parsed response, if available, otherwise null.
    /// </summary>
    public FeigResponse? Response { get; }


    private FeigParseResult(FeigParseStatus status, FeigResponse? response)
    {
        Status   = status;
        Response = response;
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

            sb.Append("Status: ");
            sb.Append(Status);
            sb.Append(", Response: { ");
            sb.Append(Response?.ToString() ?? "<null>");
            sb.Append(" }");

            return sb.ToString();
        }
        finally
        {
            StringBuilderPool.Free(sb);
        }
    }


    /// <summary>
    ///     Returns a result indicating a successfully parsed response.
    /// </summary>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static FeigParseResult Success(FeigResponse response)
    {
        Verify.NotNull(response, nameof(response));

        return new FeigParseResult(FeigParseStatus.Success, response);
    }

    /// <summary>
    ///     Returns a result indicating that more data is needed for an entire response frame.
    /// </summary>
    public static FeigParseResult MoreDataNeeded()
    {
        return new FeigParseResult(FeigParseStatus.MoreDataNeeded, null);
    }

    /// <summary>
    ///     Returns a result indicating that a checksum error has been detected.
    /// </summary>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static FeigParseResult ChecksumError(FeigResponse response)
    {
        Verify.NotNull(response, nameof(response));

        return new FeigParseResult(FeigParseStatus.ChecksumError, response);
    }

    /// <summary>
    ///     Returns a result indicating that an error in the protocol frame has been detected.
    /// </summary>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static FeigParseResult FrameError()
    {
        return new FeigParseResult(FeigParseStatus.FrameError, null);
    }
}
