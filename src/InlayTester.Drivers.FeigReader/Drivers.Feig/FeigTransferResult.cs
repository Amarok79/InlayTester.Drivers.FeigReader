// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.Text;
using Amarok.Contracts;
using Amarok.Shared;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This type represents the result of a transfer operation.
/// </summary>
public readonly struct FeigTransferResult
{
    /// <summary>
    ///     Gets the status of the transfer operation.
    /// </summary>
    public FeigTransferStatus Status { get; }

    /// <summary>
    ///     Gets the request of this transfer operation.
    /// </summary>
    public FeigRequest Request { get; }

    /// <summary>
    ///     Gets the parsed response, if available, otherwise null.
    /// </summary>
    public FeigResponse? Response { get; }


    private FeigTransferResult(FeigTransferStatus status, FeigRequest request, FeigResponse? response)
    {
        Status = status;
        Request = request;
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
            sb.Append(", Request: { ");
            sb.Append(Request);
            sb.Append(" }, Response: { ");
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
    ///     Returns a result indicating a successfully transfer operation.
    /// </summary>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static FeigTransferResult Success(FeigRequest request, FeigResponse response)
    {
        Verify.NotNull(request, nameof(request));
        Verify.NotNull(response, nameof(response));

        return new FeigTransferResult(FeigTransferStatus.Success, request, response);
    }

    /// <summary>
    ///     Returns a result indicating that the transfer operation has been canceled.
    /// </summary>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static FeigTransferResult Canceled(FeigRequest request)
    {
        Verify.NotNull(request, nameof(request));

        return new FeigTransferResult(FeigTransferStatus.Canceled, request, null);
    }

    /// <summary>
    ///     Returns a result indicating that the transfer operation has timed out.
    /// </summary>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static FeigTransferResult Timeout(FeigRequest request)
    {
        Verify.NotNull(request, nameof(request));

        return new FeigTransferResult(FeigTransferStatus.Timeout, request, null);
    }

    /// <summary>
    ///     Returns a result indicating that the transfer operation failed due to a communication error.
    /// </summary>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static FeigTransferResult CommunicationError(FeigRequest request)
    {
        Verify.NotNull(request, nameof(request));

        return new FeigTransferResult(FeigTransferStatus.CommunicationError, request, null);
    }

    /// <summary>
    ///     Returns a result indicating that the transfer operation failed because an unexpected response
    ///     has been received.
    /// </summary>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static FeigTransferResult UnexpectedResponse(FeigRequest request, FeigResponse response)
    {
        Verify.NotNull(request, nameof(request));
        Verify.NotNull(response, nameof(response));

        return new FeigTransferResult(FeigTransferStatus.UnexpectedResponse, request, response);
    }
}
