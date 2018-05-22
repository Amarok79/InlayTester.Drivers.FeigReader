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
using InlayTester.Shared;
using InlayTester.Shared.Transports;
using Nito.AsyncEx;


namespace InlayTester.Drivers.Feig
{
	internal sealed class DefaultFeigTransport :
		IFeigTransport
	{
		// data
		private readonly Object mSyncThis = new Object();
		private readonly AsyncAutoResetEvent mResultSignal = new AsyncAutoResetEvent(false);
		private readonly ITransport mTransport;
		private readonly Task[] mTaskArray = new Task[2];

		// state
		private Boolean mAcceptResponse;
		private BufferSpan mReceiveBuffer;
		private FeigParseResult mResult;


		public DefaultFeigTransport(String portName)
		{
			Verify.NotEmpty(portName, nameof(portName));

			// pre-allocate a receive buffer
			mReceiveBuffer = BufferSpan.From(new Byte[1024]);
			mReceiveBuffer = mReceiveBuffer.Clear();

			// set up serial transport
			var settings = new SerialTransportSettings {
				PortName = portName,
				Baud = 38400,
				DataBits = 8,
				Parity = Parity.Even,
				StopBits = StopBits.One,
				Handshake = Handshake.None,
			};

			mTransport = Transport.Create(settings);
			mTransport.Received += _HandleReceived;
		}


		public void Open()
		{
			mTransport.Open();
		}

		public void Close()
		{
			mTransport.Close();
		}

		public void Dispose()
		{
			mTransport.Dispose();
		}

		public async Task<FeigTransferResult> Transfer(
			FeigRequest request,
			FeigProtocol protocol,
			TimeSpan timeout,
			CancellationToken cancellationToken)
		{
			lock (mSyncThis)
			{
				// clear buffers
				mReceiveBuffer = mReceiveBuffer.Clear();

				// send request
				var requestData = request.ToBufferSpan(protocol);
				mTransport.Send(requestData);

				mAcceptResponse = true;
			}

			// wait for response or timeout
			var receiveTask = mResultSignal.WaitAsync(cancellationToken);
			var timeoutTask = Task.Delay(timeout);

			mTaskArray[0] = receiveTask;
			mTaskArray[1] = timeoutTask;

			await Task.WhenAny(mTaskArray)
				.ConfigureAwait(false);

			lock (mSyncThis)
			{
				mAcceptResponse = false;

				if (timeoutTask.IsCompleted)
					return FeigTransferResult.Timeout();
				if (receiveTask.IsCanceled)
					return FeigTransferResult.Canceled();

				if (mResult.Status == FeigParseStatus.ChecksumError)
					return FeigTransferResult.ChecksumError(mResult.Response);

				return FeigTransferResult.Success(mResult.Response);
			}
		}

		private void _HandleReceived(Object sender, TransportDataReceivedEventArgs e)
		{
			lock (mSyncThis)
			{
				if (!mAcceptResponse)
					return;     // ignore

				// append to receive buffer
				mReceiveBuffer = mReceiveBuffer.Append(e.Data);

				// parse response
				var result = FeigResponse.TryParse(mReceiveBuffer);

				if (result.Status == FeigParseStatus.MoreDataNeeded)
					return;     // wait for more data

				// set result
				mResult = result;
				mResultSignal.Set();
			}
		}
	}
}
