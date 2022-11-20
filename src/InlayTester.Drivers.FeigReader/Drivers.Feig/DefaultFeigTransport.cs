// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.Threading;
using System.Threading.Tasks;
using Amarok.Contracts;
using Amarok.Shared;
using InlayTester.Shared.Transports;
using Microsoft.Extensions.Logging;


namespace InlayTester.Drivers.Feig;


internal sealed class DefaultFeigTransport : IFeigTransport
{
    // data
    private readonly Object mSyncThis = new();
    private readonly SerialTransportSettings mSettings;
    private readonly ILogger mLogger;
    private readonly ITransport mTransport;

    // state
    private FeigRequest? mRequest;
    private FeigProtocol mProtocol;
    private BufferSpan mReceiveBuffer;
    private TaskCompletionSource<FeigTransferResult> mCompletionSource;


    internal SerialTransportSettings Settings => mSettings;

    internal ILogger Logger => mLogger;


    public DefaultFeigTransport(SerialTransportSettings settings, ILogger logger, ITransportHooks? hooks = null)
    {
        Verify.NotNull(settings, nameof(settings));
        Verify.NotNull(logger, nameof(logger));

        mSettings = settings;
        mLogger = logger;

        mCompletionSource = new TaskCompletionSource<FeigTransferResult>();
        mCompletionSource.SetCanceled();

        mReceiveBuffer = BufferSpan.From(new Byte[1024]);
        mReceiveBuffer = mReceiveBuffer.Clear();

        mTransport = hooks != null ? Transport.Create(settings, logger, hooks) : Transport.Create(settings, logger);

        mTransport.Received.Subscribe(x => _HandleReceived(x));
    }


    /// <summary>
    ///     Opens the transport.
    /// </summary>
    public void Open()
    {
        mTransport.Open();
    }

    /// <summary>
    ///     Closes the transport.
    /// </summary>
    public void Close()
    {
        mTransport.Close();
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
    ///     resources.
    /// </summary>
    public void Dispose()
    {
        mTransport.Dispose();
    }

    /// <summary>
    ///     Performs a transfer by sending a request and waiting for a response or timeout.
    /// </summary>
    public Task<FeigTransferResult> Transfer(
        FeigRequest request,
        FeigProtocol protocol,
        TimeSpan timeout,
        CancellationToken cancellationToken
    )
    {
        lock (mSyncThis)
        {
            // store for later use
            mRequest = request;
            mProtocol = protocol;

            // clear buffers
            mReceiveBuffer = mReceiveBuffer.Clear();

            // create new completion source for this transfer operation
            mCompletionSource = new TaskCompletionSource<FeigTransferResult>();

            // handle cancellation
            var cancellationRegistration = cancellationToken.Register(
                completionSource => {
                    #region (logging)

                    if (mLogger.IsEnabled(LogLevel.Information))
                    {
                        mLogger.LogInformation("[{0}]  CANCELED", mSettings.PortName);
                    }

                    #endregion

                    ( (TaskCompletionSource<FeigTransferResult>)completionSource! ).TrySetResult(
                        FeigTransferResult.Canceled(mRequest)
                    );
                },
                mCompletionSource,
                false
            );

            // handle timeout
            var cts = new CancellationTokenSource(timeout);

            var timeoutRegistration = cts.Token.Register(
                completionSource => {
                    #region (logging)

                    if (mLogger.IsEnabled(LogLevel.Information))
                    {
                        mLogger.LogInformation(
                            "[{0}]  TIMEOUT   {1} ms",
                            mSettings.PortName,
                            timeout.TotalMilliseconds
                        );
                    }

                    #endregion

                    ( (TaskCompletionSource<FeigTransferResult>)completionSource! ).TrySetResult(
                        FeigTransferResult.Timeout(mRequest)
                    );
                },
                mCompletionSource,
                false
            );

            // cleanup after completion
            mCompletionSource.Task.ContinueWith(
                _ => {
                    cancellationRegistration.Dispose();
                    timeoutRegistration.Dispose();
                },
                TaskContinuationOptions.ExecuteSynchronously
            );

            // send request

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation("[{0}]  SENT      {1}", mSettings.PortName, mRequest);
            }

            #endregion

            var requestData = request.ToBufferSpan(protocol);
            mTransport.Send(requestData);
        }

        return mCompletionSource.Task;
    }

    private void _HandleReceived(BufferSpan data)
    {
        lock (mSyncThis)
        {
            if (mCompletionSource.Task.IsCompleted)
            {
                _IgnoreReceivedData(data);
            }
            else
            {
                mReceiveBuffer = mReceiveBuffer.Append(data);
                var result = FeigResponse.TryParse(mReceiveBuffer, mProtocol);

                if (result.Status == FeigParseStatus.MoreDataNeeded)
                {
                    _WaitForMoreData();
                }
                else if (result.Status == FeigParseStatus.FrameError || result.Status == FeigParseStatus.ChecksumError)
                {
                    _CompleteWithError(result);
                }
                else if (result!.Response!.Command != mRequest!.Command)
                {
                    _CompleteWithUnexpectedResponse(result);
                }
                else
                {
                    _CompleteWithSuccess(result);
                }
            }
        }
    }

    private void _IgnoreReceivedData(BufferSpan data)
    {
        // ignore received data

        #region (logging)

        mLogger.LogTrace("[{0}]  IGNORED   {1}", mSettings.PortName, data);

        #endregion
    }

    private void _WaitForMoreData()
    {
        // wait for more data

        #region (logging)

        if (mLogger.IsEnabled(LogLevel.Trace))
        {
            mLogger.LogTrace(
                "[{0}]  PARSED    MoreDataNeeded;  ReceiveBuffer: {1}",
                mSettings.PortName,
                mReceiveBuffer
            );
        }

        #endregion
    }

    private void _CompleteWithError(FeigParseResult result)
    {
        // complete with error

        #region (logging)

        if (mLogger.IsEnabled(LogLevel.Trace))
        {
            mLogger.LogTrace(
                "[{0}]  PARSED    {1};  ReceiveBuffer: {2}",
                mSettings.PortName,
                result.Status,
                mReceiveBuffer
            );
        }

        if (mLogger.IsEnabled(LogLevel.Trace))
        {
            mLogger.LogTrace("[{0}]  COMMERR", mSettings.PortName);
        }

        #endregion

        mCompletionSource.TrySetResult(FeigTransferResult.CommunicationError(mRequest!));
    }

    private void _CompleteWithUnexpectedResponse(FeigParseResult result)
    {
        // complete with error

        #region (logging)

        if (mLogger.IsEnabled(LogLevel.Information))
        {
            mLogger.LogInformation("[{0}]  UNEXPECT  {1}", mSettings.PortName, result.Response);
        }

        #endregion

        mCompletionSource.TrySetResult(FeigTransferResult.UnexpectedResponse(mRequest!, result.Response!));
    }

    private void _CompleteWithSuccess(FeigParseResult result)
    {
        // complete with success

        #region (logging)

        if (mLogger.IsEnabled(LogLevel.Information))
        {
            mLogger.LogInformation("[{0}]  RECEIVED  {1}", mSettings.PortName, result.Response);
        }

        #endregion

        mCompletionSource.TrySetResult(FeigTransferResult.Success(mRequest!, result.Response!));
    }
}
