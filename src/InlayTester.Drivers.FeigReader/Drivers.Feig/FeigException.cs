// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This exception indicates that an error occurred while interacting with the Feig RFID reader.
/// </summary>
public class FeigException : Exception
{
    /// <summary>
    ///     Gets the request sent to the reader, if available, otherwise null.
    /// </summary>
    public FeigRequest? Request { get; }

    /// <summary>
    ///     Gets the response received from the reader, if available, otherwise null.
    /// </summary>
    public FeigResponse? Response { get; }


    /// <summary>
    ///     Initializes a new instance.
    /// </summary>
    /// 
    /// <param name="request">
    ///     The request sent to the reader, if available, otherwise null.
    /// </param>
    /// <param name="response">
    ///     The response received from the reader, if available, otherwise null.
    /// </param>
    public FeigException(FeigRequest? request, FeigResponse? response)
    {
        Request = request;
        Response = response;
    }

    /// <summary>
    ///     Initializes a new instance.
    /// </summary>
    public FeigException()
    {
    }

    /// <summary>
    ///     Initializes a new instance.
    /// </summary>
    /// 
    /// <param name="message">
    ///     The error message that explains the reason for the exception.
    /// </param>
    /// <param name="request">
    ///     The request sent to the reader, if available, otherwise null.
    /// </param>
    /// <param name="response">
    ///     The response received from the reader, if available, otherwise null.
    /// </param>
    public FeigException(String message, FeigRequest? request, FeigResponse? response)
        : base(message)
    {
        Request = request;
        Response = response;
    }

    /// <summary>
    ///     Initializes a new instance.
    /// </summary>
    /// 
    /// <param name="message">
    ///     The error message that explains the reason for the exception.
    /// </param>
    public FeigException(String message)
        : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance.
    /// </summary>
    /// 
    /// <param name="message">
    ///     The error message that explains the reason for the exception.
    /// </param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception.
    /// </param>
    public FeigException(String message, Exception innerException)
        : base(message, innerException)
    {
    }
}
