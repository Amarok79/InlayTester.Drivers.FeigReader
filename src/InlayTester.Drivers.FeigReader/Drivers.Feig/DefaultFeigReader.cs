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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using InlayTester.Shared;


namespace InlayTester.Drivers.Feig
{
	internal sealed class DefaultFeigReader :
		IFeigReader
	{
		// data
		private readonly FeigReaderSettings mSettings;
		private readonly IFeigTransport mTransport;
		private readonly ILog mLogger;


		public FeigReaderSettings Settings => mSettings;

		public IFeigTransport Transport => mTransport;

		public ILog Logger => mLogger;


		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public DefaultFeigReader(FeigReaderSettings settings, IFeigTransport transport, ILog logger)
		{
			mSettings = settings;
			mTransport = transport;
			mLogger = logger;
		}


		#region ++ IFeigReader Interface (Open, Close, Dispose) ++

		/// <summary>
		/// Opens the transport (serial connection) to the Feig RFID reader.
		/// 
		/// The transport can be opened and closed multiple times.
		/// </summary>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has already been opened before.</exception>
		/// <exception cref="IOException">
		/// The transport settings seem to be invalid.</exception>
		public void Open()
		{
			mTransport.Open();
		}

		/// <summary>
		/// Closes the transport (serial connection) to the Feig RFID reader.
		/// 
		/// The transport can be opened and closed multiple times.
		/// </summary>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
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

		#endregion

		#region ++ IFeigReader Interface (Transfer) ++

		/// <summary>
		/// Performs a transfer operation by sending a request to the reader/module and then waits for 
		/// a corresponding response from the reader/module or for timeout, whatever comes first.
		/// 
		/// This method doesn't throw exceptions for timeout or failed transfer operations. Instead, a 
		/// result object providing detailed information about the transfer operation is returned.
		/// </summary>
		/// 
		/// <param name="request">
		/// The request to send to the reader.</param>
		/// <param name="protocol">
		/// (Optional) The protocol to use in communication with the reader. If not specified, the global setting is used.</param>
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <returns>
		/// An object describing the outcome of the transfer operation.
		/// </returns>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		public Task<FeigTransferResult> Transfer(
			FeigRequest request,
			FeigProtocol? protocol = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default)
		{
			return mTransport.Transfer(
				request,
				protocol ?? mSettings.Protocol,
				timeout ?? mSettings.Timeout,
				cancellationToken
			);
		}

		/// <summary>
		/// Performs a transfer operation by sending a request to the reader/module and then waits for 
		/// a corresponding response from the reader/module or for timeout, whatever comes first.
		/// 
		/// This method doesn't throw exceptions for timeout or failed transfer operations. Instead, a 
		/// result object providing detailed information about the transfer operation is returned.
		/// </summary>
		/// 
		/// <param name="command">
		/// The command to execute with this transfer operation.</param>
		/// <param name="requestData">
		/// (Optional) The data associated with the command that should be sent to the reader.</param>
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <returns>
		/// An object describing the outcome of the transfer operation.
		/// </returns>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		public Task<FeigTransferResult> Transfer(
			FeigCommand command,
			BufferSpan requestData = default,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default)
		{
			var request = new FeigRequest {
				Address = mSettings.Address,
				Command = command,
				Data = requestData,
			};

			return this.Transfer(
				request,
				mSettings.Protocol,
				timeout,
				cancellationToken
			);
		}

		#endregion

		#region ++ IFeigReader Interface (Execute) ++

		/// <summary>
		/// Executes the supplied command by sending a request to the reader/module and then waits for 
		/// a corresponding response from the reader/module or for timeout, whatever comes first.
		/// 
		/// This methods throws appropriate exceptions for timeout, cancellation or failed operations.
		/// </summary>
		/// 
		/// <param name="request">
		/// The request to send to the reader.</param>
		/// <param name="protocol">
		/// (Optional) The protocol to use in communication with the reader. If not specified, the global setting is used.</param>
		/// <param name="timeout">
		/// (Optional) The timeout for this operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the operation.</param>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		/// <exception cref="TimeoutException">
		/// The operation '(request)' timed out after (timeout) ms.</exception>
		/// <exception cref="OperationCanceledException">
		/// The operation '(request)' has been canceled.</exception>
		/// <exception cref="FeigException">
		/// The operation '(request)' failed because of a communication error. Received corrupted '(response)'.</exception>
		/// <exception cref="FeigException">
		/// The operation '(request)' failed because the reader returned error code '(error)'. Received '(response)'.</exception>
		public async Task<FeigResponse> Execute(
			FeigRequest request,
			FeigProtocol? protocol = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default)
		{
			var result = await this.Transfer(request, protocol, timeout, cancellationToken)
				.ConfigureAwait(false);

			if (result.Status == FeigTransferStatus.Timeout)
			{
				var resolvedTimeout = timeout ?? mSettings.Timeout;

				throw new TimeoutException(
					$"The operation '{request}' timed out after {resolvedTimeout.TotalMilliseconds} ms."
				);
			}
			else
			if (result.Status == FeigTransferStatus.Canceled)
			{
				throw new OperationCanceledException(
					$"The operation '{request}' has been canceled."
				);
			}
			else
			if (result.Status == FeigTransferStatus.ChecksumError)
			{
				throw new FeigException(
					$"The operation '{result.Request}' failed because of a communication error. " +
					$"Received corrupted response: '{result.Request}'.",
					result.Request,
					result.Response
				);
			}
			else
			if (result.Status == FeigTransferStatus.Success)
			{
				if (result.Response.Status != FeigStatus.OK)
				{
					throw new FeigException(
						$"The operation '{result.Request}' failed because the reader returned error code " +
						$"'{result.Response.Status}'. Received response: '{result.Response}'.",
						result.Request,
						result.Response
					);
				}
				else
				{
					return result.Response;     // success
				}
			}
			else
			{
				throw new InvalidOperationException("Encountered unknown FeigTransferStatus!");
			}
		}

		/// <summary>
		/// Executes the supplied command by sending a request to the reader/module and then waits for 
		/// a corresponding response from the reader/module or for timeout, whatever comes first.
		/// 
		/// This methods throws appropriate exceptions for timeout, cancellation or failed operations.
		/// </summary>
		/// 
		/// <param name="command">
		/// The command to execute with this transfer operation.</param>
		/// <param name="requestData">
		/// (Optional) The data associated with the command that should be sent to the reader.</param>
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		/// <exception cref="TimeoutException">
		/// The operation '(request)' timed out after (timeout) ms.</exception>
		/// <exception cref="OperationCanceledException">
		/// The operation '(request)' has been canceled.</exception>
		/// <exception cref="FeigException">
		/// The operation '(request)' failed because of a communication error. Received corrupted '(response)'.</exception>
		/// <exception cref="FeigException">
		/// The operation '(request)' failed because the reader returned error code '(error)'. Received '(response)'.</exception>
		public Task<FeigResponse> Execute(
			FeigCommand command,
			BufferSpan requestData = default,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default)
		{
			var request = new FeigRequest {
				Address = mSettings.Address,
				Command = command,
				Data = requestData,
			};

			return this.Execute(
				request,
				mSettings.Protocol,
				timeout,
				cancellationToken
			);
		}

		#endregion

		#region ++ IFeigReader Interface (Common Commands) ++

		/// <summary>
		/// Tests whether communication to RFID reader is working.
		/// 
		/// This method sends a 'Baud Rate Detection' command request to the reader to determine whether 
		/// communication is working.
		/// 
		/// This method doesn't throw exceptions for communication errors.
		/// </summary>
		/// 
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <returns>
		/// True, if the communication test succeeded; otherwise False. In case of cancellation, False is returned.
		/// </returns>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		public async Task<Boolean> TestCommunication(
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default)
		{
			var result = await this.Transfer(
				FeigCommand.BaudRateDetection, BufferSpan.From(0x00), timeout, cancellationToken)
				.ConfigureAwait(false);

			return result.Status == FeigTransferStatus.Success;
		}

		/// <summary>
		/// Resets the CPU on the reader.
		/// 
		/// The RF-field will be switched off during a CPU reset.
		/// </summary>
		/// 
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		/// <exception cref="TimeoutException">
		/// The operation '(request)' timed out after (timeout) ms.</exception>
		/// <exception cref="OperationCanceledException">
		/// The operation '(request)' has been canceled.</exception>
		/// <exception cref="FeigException">
		/// The operation '(request)' failed because of a communication error. Received corrupted '(response)'.</exception>
		/// <exception cref="FeigException">
		/// The operation '(request)' failed because the reader returned error code '(error)'. Received '(response)'.</exception>
		public Task ResetCPU(
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default)
		{
			return this.Execute(
				FeigCommand.CPUReset,
				BufferSpan.Empty,
				timeout,
				cancellationToken
			);
		}

		/// <summary>
		/// The RF-field of the Reader antenna is switched off for approx. 6 ms. Thus, all transponders which 
		/// are within the antenna field of the reader will be reset to their base setting.
		/// 
		/// After a RF Reset a transponder which is located within the field has to be re-selected.
		/// </summary>
		/// 
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		/// <exception cref="TimeoutException">
		/// The operation '(request)' timed out after (timeout) ms.</exception>
		/// <exception cref="OperationCanceledException">
		/// The operation '(request)' has been canceled.</exception>
		/// <exception cref="FeigException">
		/// The operation '(request)' failed because of a communication error. Received corrupted '(response)'.</exception>
		/// <exception cref="FeigException">
		/// The operation '(request)' failed because the reader returned error code '(error)'. Received '(response)'.</exception>
		public Task ResetRF(
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default)
		{
			return this.Execute(
				FeigCommand.RFReset,
				BufferSpan.Empty,
				timeout,
				cancellationToken
			);
		}

		/// <summary>
		/// Reads a configuration block (14 bytes) from the reader's RAM or EEPROM.
		/// </summary>
		/// 
		/// <param name="block">
		/// The configuration block to read.</param>
		/// <param name="eeprom">
		/// True, to read from EEPROM, otherwise from RAM.</param>
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		/// The block number must be between 0 and 63.</exception>
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		/// <exception cref="TimeoutException">
		/// The operation '(request)' timed out after (timeout) ms.</exception>
		/// <exception cref="OperationCanceledException">
		/// The operation '(request)' has been canceled.</exception>
		/// <exception cref="FeigException">
		/// The operation '(request)' failed because of a communication error. Received corrupted '(response)'.</exception>
		/// <exception cref="FeigException">
		/// The operation '(request)' failed because the reader returned error code '(error)'. Received '(response)'.</exception>
		public async Task<BufferSpan> ReadConfiguration(
			Int32 block,
			Boolean eeprom,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default)
		{
			Verify.InRange(block, 0, 63, nameof(block));

			Byte addr = 0x00;
			addr |= (Byte)(eeprom ? 0x80 : 0x00);
			addr |= (Byte)(block & 0x3F);

			var response = await this.Execute(
				FeigCommand.ReadConfiguration, BufferSpan.From(addr), timeout, cancellationToken)
				.ConfigureAwait(false);

			return response.Data;
		}

		#endregion
	}
}
