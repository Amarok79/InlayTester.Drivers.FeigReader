﻿/* MIT License
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


		/// <summary>
		/// Performs a transfer operation by sending a request and waiting for the response or timeout.
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
		/// Performs a transfer operation by sending a request and waiting for the response or timeout.
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


		public async Task<FeigResponse> Execute(
			FeigRequest request,
			FeigProtocol? protocol = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default)
		{
			var result = await this.Transfer(request, protocol, timeout, cancellationToken)
				.ConfigureAwait(false);

			_ThrowIfNotSuccessful(result);

			return result.Response;
		}

		public async Task<FeigResponse> Execute(
			FeigCommand command,
			BufferSpan requestData = default,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default)
		{
			var result = await this.Transfer(command, requestData, timeout, cancellationToken)
				.ConfigureAwait(false);

			_ThrowIfNotSuccessful(result);

			return result.Response;
		}

		private void _ThrowIfNotSuccessful(FeigTransferResult result)
		{
			if (result.Status == FeigTransferStatus.Timeout)
				throw new TimeoutException();
			else
			if (result.Status == FeigTransferStatus.Canceled)
				throw new OperationCanceledException();
			else
			if (result.Status == FeigTransferStatus.ChecksumError)
				throw new FeigException(result.Request, result.Response);
			else
			{
				if (result.Response.Status != FeigStatus.OK)
					throw new FeigException(result.Request, result.Response);
			}
		}





		/// <summary>
		/// Tests whether communication to RFID reader is working.
		/// 
		/// This method sends a 'Baud Rate Detection' command request to the reader to determine whether 
		/// communication is working.
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
		/// </summary>
		/// 
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <exception cref="FeigException">
		/// An error occurred while performing the transfer operation.</exception>
		public Task ResetCPU(
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default)
		{
			return this.Execute(FeigCommand.CPUReset, BufferSpan.Empty, timeout, cancellationToken);
		}





		//public async Task<Byte[]> ReadConfiguration(Int32 configurationBlock, Boolean readFromEEPROM)
		//{
		//	Verify.InRange(configurationBlock, 0, 8, nameof(configurationBlock));

		//	Byte configAddress = 0x00;
		//	configAddress |= (Byte)(readFromEEPROM ? 0x80 : 0x00);
		//	configAddress |= (Byte)(configurationBlock & 0x3F);

		//	var response = await _Transfer(FeigCommand.ReadConfiguration, configAddress)
		//		.ConfigureAwait(false);

		//	return response.Data.ToArray();
		//}

		//private async Task<FeigResponse> _Transfer(FeigCommand command, TimeSpan timeout, params Byte[] requestData)
		//{
		//	var request = new FeigRequest {
		//		Address = mSettings.Address,
		//		Command = command,
		//		Data = BufferSpan.From(requestData, requestData.Length)
		//	};

		//	var result = await mTransport.Transfer(request, FeigProtocol.Advanced, timeout, default)
		//		.ConfigureAwait(false);

		//	if (result.Status == FeigTransferStatus.Timeout)
		//		throw new TimeoutException();
		//	if (result.Status == FeigTransferStatus.ChecksumError)
		//		throw new IOException();

		//	var response = result.Response;

		//	if (response.Command != command)
		//		throw new IOException();
		//	if (response.Status != FeigStatus.OK)
		//		throw new FeigException(request, response);

		//	return response;
		//}

		//private Task<FeigResponse> _Transfer(FeigCommand command, params Byte[] requestData)
		//{
		//	return _Transfer(command, mSettings.Timeout, requestData);
		//}
	}
}
