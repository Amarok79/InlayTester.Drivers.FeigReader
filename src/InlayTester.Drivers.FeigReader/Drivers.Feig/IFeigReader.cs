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
		Task<FeigTransferResult> Transfer(
			FeigRequest request,
			FeigProtocol? protocol = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

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
		Task<FeigTransferResult> Transfer(
			FeigCommand command,
			in BufferSpan requestData = default,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);


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
		Task<FeigResponse> Execute(
			FeigRequest request,
			FeigProtocol? protocol = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

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
		Task<FeigResponse> Execute(
			FeigCommand command,
			in BufferSpan requestData = default,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);


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
		Task<Boolean> TestCommunication(
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

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
		Task ResetCPU(
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

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
		Task ResetRF(
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Switches the RF-field of the Reader antenna on or off.
		/// </summary>
		/// 
		/// <param name="flag">
		/// A value indicating which RF field should switched on or off. Specify 0x00 to switch off all 
		/// antennas and 0x01 to switch on the first antenna. Look up more information in the reader's manual.</param>
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
		Task SwitchRF(
			Byte flag,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets information about the reader/module's software.
		/// </summary>
		/// 
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <returns>
		/// An object containing the parsed response data.
		/// </returns>
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
		Task<FeigSoftwareInfo> GetSoftwareInfo(
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);


		/// <summary>
		/// Reads a configuration block (14 bytes) from the reader's RAM or EEPROM.
		/// </summary>
		/// 
		/// <param name="block">
		/// The configuration block to read.</param>
		/// <param name="location">
		/// The location of the block to read from, either EEPROM or RAM.</param>
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <returns>
		/// The configuration data.
		/// </returns>
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
		Task<BufferSpan> ReadConfiguration(
			Int32 block,
			FeigBlockLocation location,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Writes a configuration block (14 bytes) to the reader's RAM or EEPROM.
		/// </summary>
		/// 
		/// <param name="block">
		/// The configuration block to write.</param>
		/// <param name="location">
		/// The location of the block to write to, either EEPROM or RAM.</param>
		/// <param name="data">
		/// The data of the configuration block; must be exactly 14 bytes.</param>
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <exception cref="ArgumentOutOfRangeException">
		/// The block number must be between 0 and 63.</exception>
		/// <exception cref="ArgumentException">
		/// Exactly 14 bytes must be specified as configuration data.</exception>
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
		Task WriteConfiguration(
			Int32 block,
			FeigBlockLocation location,
			BufferSpan data,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Saves all configuration blocks currently in the reader's RAM to EEPROM.
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
		Task SaveConfigurations(
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Saves the specified configuration block currently in the reader's RAM to EEPROM.
		/// </summary>
		/// 
		/// <param name="block">
		/// The configuration block to save.</param>
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
		Task SaveConfiguration(
			Int32 block,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Resets all configuration blocks to their defaults.
		/// </summary>
		/// 
		/// <param name="location">
		/// The location of the block to reset, either EEPROM or RAM.</param>
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
		Task ResetConfigurations(
			FeigBlockLocation location,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Resets the specified configuration block to its defaults.
		/// </summary>
		/// 
		/// <param name="block">
		/// The configuration block to reset.</param>
		/// <param name="location">
		/// The location of the block to reset, either EEPROM or RAM.</param>
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
		Task ResetConfiguration(
			Int32 block,
			FeigBlockLocation location,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Reads the identifier data of all transponders inside the antenna field.
		/// </summary>
		/// 
		/// <param name="timeout">
		/// (Optional) The timeout for this transfer operation. If not specified, the global timeout is used.</param>
		/// <param name="cancellationToken">
		/// (Optional) A cancellation token that can be used to cancel the transfer operation.</param>
		/// 
		/// <returns>
		/// A tuple containing the decoded transponders and the received response.
		/// </returns>
		/// 
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
		Task<(FeigTransponder[] Transponders, FeigResponse Response)> Inventory(
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default);
	}
}
