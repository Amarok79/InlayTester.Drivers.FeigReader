// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.Threading;
using System.Threading.Tasks;


namespace InlayTester.Drivers.Feig;


internal interface IFeigTransport : IDisposable
{
    /// <summary>
    ///     Opens the transport.
    /// </summary>
    void Open();

    /// <summary>
    ///     Closes the transport.
    /// </summary>
    void Close();

    /// <summary>
    ///     Performs a transfer by sending a request and waiting for a response or timeout.
    /// </summary>
    Task<FeigTransferResult> Transfer(
        FeigRequest request,
        FeigProtocol protocol,
        TimeSpan timeout,
        CancellationToken cancellationToken
    );
}
