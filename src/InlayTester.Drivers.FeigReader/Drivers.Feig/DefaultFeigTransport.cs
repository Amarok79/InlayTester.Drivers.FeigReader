/* MIT License
 * 
 * Copyright (c) 2018, Olaf Kober
 * https://github.com/Amarok79/InlayTester.Drivers.FeigReader
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
*/

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using InlayTester.Shared;
using InlayTester.Shared.Transports;


namespace InlayTester.Drivers.Feig
{
	internal sealed class DefaultFeigTransport :
		IFeigTransport
	{
		// data
		private readonly Object mSyncThis = new Object();
		private readonly SerialTransportSettings mSettings;
		private readonly ILog mLog;
		private readonly ITransport mTransport;

		// state
		private FeigRequest mRequest;
		private FeigProtocol mProtocol;
		private BufferSpan mReceiveBuffer;
		private TaskCompletionSource<FeigTransferResult> mCompletionSource;


		internal SerialTransportSettings Settings => mSettings;

		internal ILog Logger => mLog;


		public DefaultFeigTransport(SerialTransportSettings settings, ILog logger)
		{
			mSettings = settings;
			mLog = logger;

			mCompletionSource = new TaskCompletionSource<FeigTransferResult>();
			mCompletionSource.SetCanceled();

			mReceiveBuffer = BufferSpan.From(new Byte[1024]);
			mReceiveBuffer = mReceiveBuffer.Clear();

			mTransport = Transport.Create(settings, logger);
			mTransport.Received += _HandleReceived;
		}


		/// <summary>
		/// Opens the transport.
		/// </summary>
		public void Open()
		{
			mTransport.Open();
		}

		/// <summary>
		/// Closes the transport.
		/// </summary>
		public void Close()
		{
			mTransport.Close();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			mTransport.Dispose();
		}

		/// <summary>
		/// Performs a transfer by sending a request and waiting for a response or timeout.
		/// </summary>
		public Task<FeigTransferResult> Transfer(
			FeigRequest request,
			FeigProtocol protocol,
			TimeSpan timeout,
			CancellationToken cancellationToken)
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
					() => mCompletionSource.TrySetResult(FeigTransferResult.Canceled(mRequest)),
					false
				);

				// handle timeout
				var cts = new CancellationTokenSource(timeout);
				var timeoutRegistration = cts.Token.Register(
					() => mCompletionSource.TrySetResult(FeigTransferResult.Timeout(mRequest)),
					false
				);

				// cleanup after completion
				mCompletionSource.Task.ContinueWith(_ =>
				{
					cancellationRegistration.Dispose();
					timeoutRegistration.Dispose();
				},
				TaskContinuationOptions.ExecuteSynchronously);

				// send request
				var requestData = request.ToBufferSpan(protocol);
				mTransport.Send(requestData);
			}

			return mCompletionSource.Task;
		}

		private void _HandleReceived(Object sender, TransportDataReceivedEventArgs e)
		{
			lock (mSyncThis)
			{
				// ignore received data
				if (mCompletionSource.Task.IsCompleted)
					return;

				// append to receive buffer
				mReceiveBuffer = mReceiveBuffer.Append(e.Data);

				// parse response
				var result = FeigResponse.TryParse(mReceiveBuffer, mProtocol);

				if (result.Status == FeigParseStatus.MoreDataNeeded)
					return;     // wait for more data

				// complete transfer
				if (result.Status == FeigParseStatus.FrameError ||
					result.Status == FeigParseStatus.ChecksumError)
					mCompletionSource.TrySetResult(FeigTransferResult.CommunicationError(mRequest, result.Response));

				if (result.Response.Command != mRequest.Command)
					mCompletionSource.TrySetResult(FeigTransferResult.UnexpectedResponse(mRequest, result.Response));

				mCompletionSource.TrySetResult(FeigTransferResult.Success(mRequest, result.Response));
			}
		}
	}
}
