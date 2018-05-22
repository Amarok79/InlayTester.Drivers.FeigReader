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
using InlayTester.Shared;


namespace InlayTester.Drivers.Feig
{
	/// <summary>
	/// This interface represents a driver for a Feig RFID reader.
	/// </summary>
	public interface IFeigReader :
		IDisposable
	{
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
		void Open();

		/// <summary>
		/// Closes the transport (serial connection) to the Feig RFID reader.
		/// 
		/// The transport can be opened and closed multiple times.
		/// </summary>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		void Close();


		/// <summary>
		/// Performs a transfer operation by sending a request and waiting for the response or timeout.
		/// 
		/// This method takes all settings, i.e. the reader address and timeout, from the supplied parameters. 
		/// Settings supplied during the reader's construction are not respected.
		/// </summary>
		/// 
		/// <param name="request">
		/// The request to send to the reader.</param>
		/// <param name="protocol">
		/// The protocol to use in communication with the reader.</param>
		/// <param name="timeout">
		/// The timeout for the transfer operation.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		Task<FeigTransferResult> Transfer(
			FeigRequest request,
			FeigProtocol protocol,
			TimeSpan timeout,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Performs a transfer operation by sending a request and waiting for the response or timeout.
		/// 
		/// This method takes some settings, i.e. the reader address, protocol and timeout, from the settings 
		/// supplied during the reader's construction.
		/// </summary>
		/// 
		/// <param name="command">
		/// The command to execute with this transfer operation.</param>
		/// <param name="requestData">
		/// The data associated with the command that should be sent to the reader.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		Task<FeigTransferResult> Transfer(
			FeigCommand command,
			BufferSpan requestData = default,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Performs a transfer operation by sending a request and waiting for the response or timeout.
		/// 
		/// This method takes some settings, i.e. the reader address and protocol, from the settings 
		/// supplied during the reader's construction.
		/// </summary>
		/// 
		/// <param name="command">
		/// The command to execute with this transfer operation.</param>
		/// <param name="timeout">
		/// The timeout for the transfer operation.</param>
		/// <param name="requestData">
		/// The data associated with the command that should be sent to the reader.</param>
		/// <param name="cancellationToken">
		/// A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		Task<FeigTransferResult> Transfer(
			FeigCommand command,
			TimeSpan timeout,
			BufferSpan requestData = default,
			CancellationToken cancellationToken = default);
	}
}
