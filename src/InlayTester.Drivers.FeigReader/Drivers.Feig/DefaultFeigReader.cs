/* MIT License
 * 
 * Copyright (c) 2020, Olaf Kober
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
using Amarok.Contracts;
using Amarok.Shared;
using Microsoft.Extensions.Logging;


namespace InlayTester.Drivers.Feig
{
    internal sealed class DefaultFeigReader : IFeigReader
    {
        // data
        private readonly FeigReaderSettings mSettings;
        private readonly IFeigTransport mTransport;
        private readonly ILogger mLogger;
        private readonly Byte[] mRequestBuffer = new Byte[256];

        // state
        private Int32 mTransferNo;


        #region ++ Public Interface ++

        public FeigReaderSettings Settings => mSettings;

        public IFeigTransport Transport => mTransport;

        public ILogger Logger => mLogger;


        /// <summary>
        ///     Initializes a new instance.
        /// </summary>
        public DefaultFeigReader(FeigReaderSettings settings, IFeigTransport transport, ILogger logger)
        {
            mSettings  = settings;
            mTransport = transport;
            mLogger    = logger;
        }

        #endregion

        #region ++ IFeigReader Interface (Open, Close, Dispose) ++

        /// <summary>
        ///     Opens the transport (serial connection) to the Feig RFID reader. The transport can be opened
        ///     and closed multiple times.
        /// </summary>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has already been opened before.
        /// </exception>
        /// <exception cref="IOException">
        ///     The transport settings seem to be invalid.
        /// </exception>
        public void Open()
        {
            mTransport.Open();
        }

        /// <summary>
        ///     Closes the transport (serial connection) to the Feig RFID reader. The transport can be opened
        ///     and closed multiple times.
        /// </summary>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
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

        #endregion

        #region ++ IFeigReader Interface (Transfer) ++

        /// <summary>
        ///     Performs a transfer operation by sending a request to the reader/module and then waits for a
        ///     corresponding response from the reader/module or for timeout, whatever comes first. This method
        ///     doesn't throw exceptions for timeout or failed transfer operations. Instead, a result object
        ///     providing detailed information about the transfer operation is returned.
        /// </summary>
        /// 
        /// <param name="request">
        ///     The request to send to the reader.
        /// </param>
        /// <param name="protocol">
        ///     (Optional) The protocol to use in communication with the reader. If not specified, the global
        ///     setting is used.
        /// </param>
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <returns>
        ///     An object describing the outcome of the transfer operation.
        /// </returns>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        public Task<FeigTransferResult> Transfer(
            FeigRequest request,
            FeigProtocol? protocol = null,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            mTransferNo++;

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
                mLogger.LogInformation("[{0}]  TRANSFER  #{1}", mSettings.TransportSettings.PortName, mTransferNo);

            #endregion

            return mTransport.Transfer(
                request,
                protocol ?? mSettings.Protocol,
                timeout ?? mSettings.Timeout,
                cancellationToken
            );
        }

        /// <summary>
        ///     Performs a transfer operation by sending a request to the reader/module and then waits for a
        ///     corresponding response from the reader/module or for timeout, whatever comes first. This method
        ///     doesn't throw exceptions for timeout or failed transfer operations. Instead, a result object
        ///     providing detailed information about the transfer operation is returned.
        /// </summary>
        /// 
        /// <param name="command">
        ///     The command to execute with this transfer operation.
        /// </param>
        /// <param name="requestData">
        ///     (Optional) The data associated with the command that should be sent to the reader.
        /// </param>
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <returns>
        ///     An object describing the outcome of the transfer operation.
        /// </returns>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        public Task<FeigTransferResult> Transfer(
            FeigCommand command,
            in BufferSpan requestData = default,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            var request = new FeigRequest {
                Address = mSettings.Address,
                Command = command,
                Data    = requestData,
            };

            return Transfer(request, mSettings.Protocol, timeout, cancellationToken);
        }

        #endregion

        #region ++ IFeigReader Interface (Execute) ++

        /// <summary>
        ///     Executes the supplied command by sending a request to the reader/module and then waits for a
        ///     corresponding response from the reader/module or for timeout, whatever comes first. This
        ///     methods throws appropriate exceptions for timeout, cancellation or failed operations.
        /// </summary>
        /// 
        /// <param name="request">
        ///     The request to send to the reader.
        /// </param>
        /// <param name="protocol">
        ///     (Optional) The protocol to use in communication with the reader. If not specified, the global
        ///     setting is used.
        /// </param>
        /// <param name="timeout">
        ///     (Optional) The timeout for this operation. If not specified, the global timeout is used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the operation.
        /// </param>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public async Task<FeigResponse> Execute(
            FeigRequest request,
            FeigProtocol? protocol = null,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            var result = await Transfer(request, protocol, timeout, cancellationToken).ConfigureAwait(false);

            if (result.Status == FeigTransferStatus.Success)
            {
                if (result!.Response!.Status == FeigStatus.OK ||
                    result!.Response!.Status == FeigStatus.NoTransponder ||
                    result!.Response!.Status == FeigStatus.MoreData)
                    return result.Response; // success

                throw new FeigException(
                    $"The operation '{result.Request}' failed because the reader returned error code " +
                    $"'{result.Response.Status}'. Received response: '{result.Response}'.",
                    result.Request,
                    result.Response
                );
            }

            if (result.Status == FeigTransferStatus.Timeout)
            {
                var resolvedTimeout = timeout ?? mSettings.Timeout;

                throw new TimeoutException(
                    $"The operation '{request}' timed out after {resolvedTimeout.TotalMilliseconds} ms."
                );
            }

            if (result.Status == FeigTransferStatus.Canceled)
                throw new OperationCanceledException($"The operation '{request}' has been canceled.");

            if (result.Status == FeigTransferStatus.CommunicationError)
            {
                throw new FeigException(
                    $"The operation '{result.Request}' failed because of a communication error.",
                    result.Request,
                    null
                );
            }

            if (result.Status == FeigTransferStatus.UnexpectedResponse)
            {
                throw new FeigException(
                    $"The operation '{result.Request}' failed because an unexpected response " +
                    $"'{result.Response}' has been received.",
                    result.Request,
                    result.Response
                );
            }

            throw new NotSupportedException($"Unexpected FeigTransferStatus '{result.Status}'!");
        }

        /// <summary>
        ///     Executes the supplied command by sending a request to the reader/module and then waits for a
        ///     corresponding response from the reader/module or for timeout, whatever comes first. This
        ///     methods throws appropriate exceptions for timeout, cancellation or failed operations.
        /// </summary>
        /// 
        /// <param name="command">
        ///     The command to execute with this transfer operation.
        /// </param>
        /// <param name="requestData">
        ///     (Optional) The data associated with the command that should be sent to the reader.
        /// </param>
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public Task<FeigResponse> Execute(
            FeigCommand command,
            in BufferSpan requestData = default,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            var request = new FeigRequest {
                Address = mSettings.Address,
                Command = command,
                Data    = requestData,
            };

            return Execute(request, mSettings.Protocol, timeout, cancellationToken);
        }

        #endregion

        #region ++ IFeigReader Interface (Common Commands) ++

        /// <summary>
        ///     Tests whether communication to RFID reader is working. This method sends a 'Baud Rate
        ///     Detection' command request to the reader to determine whether communication is working. This
        ///     method doesn't throw exceptions for communication errors.
        /// </summary>
        /// 
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <returns>
        ///     True, if the communication test succeeded; otherwise False. In case of cancellation, False is
        ///     returned.
        /// </returns>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        public async Task<Boolean> TestCommunication(
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
                mLogger.LogInformation("[{0}]  TestCommunication()", mSettings.TransportSettings.PortName);

            #endregion

            mRequestBuffer[0] = 0x00;
            var data = BufferSpan.From(mRequestBuffer, 0, 1);

            var result = await Transfer(FeigCommand.BaudRateDetection, data, timeout, cancellationToken)
               .ConfigureAwait(false);

            var flag = result.Status == FeigTransferStatus.Success;

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  TestCommunication()  =>  {1}",
                    mSettings.TransportSettings.PortName,
                    flag
                );
            }

            #endregion

            return flag;
        }

        /// <summary>
        ///     Resets the CPU on the reader. The RF-field will be switched off during a CPU reset.
        /// </summary>
        /// 
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public async Task ResetCPU(TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
                mLogger.LogInformation("[{0}]  ResetCPU()", mSettings.TransportSettings.PortName);

            #endregion

            var response = await Execute(FeigCommand.CPUReset, BufferSpan.Empty, timeout, cancellationToken)
               .ConfigureAwait(false);

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  ResetCPU()  =>  {1}",
                    mSettings.TransportSettings.PortName,
                    response.Status
                );
            }

            #endregion
        }

        /// <summary>
        ///     The RF-field of the Reader antenna is switched off for approx. 6 ms. Thus, all transponders
        ///     which are within the antenna field of the reader will be reset to their base setting. After a
        ///     RF Reset a transponder which is located within the field has to be re-selected.
        /// </summary>
        /// 
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public async Task ResetRF(TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
                mLogger.LogInformation("[{0}]  ResetRF()", mSettings.TransportSettings.PortName);

            #endregion

            var response = await Execute(FeigCommand.RFReset, BufferSpan.Empty, timeout, cancellationToken)
               .ConfigureAwait(false);

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  ResetRF()  =>  {1}",
                    mSettings.TransportSettings.PortName,
                    response.Status
                );
            }

            #endregion
        }

        /// <summary>
        ///     Switches the RF-field of the Reader antenna on or off.
        /// </summary>
        /// 
        /// <param name="flag">
        ///     A value indicating which RF field should switched on or off. Specify 0x00 to switch off all
        ///     antennas and 0x01 to switch on the first antenna. Look up more information in the reader's
        ///     manual.
        /// </param>
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public async Task SwitchRF(Byte flag, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
                mLogger.LogInformation("[{0}]  SwitchRF(0x{1})", mSettings.TransportSettings.PortName, flag.ToHex());

            #endregion

            mRequestBuffer[0] = flag;
            var data = BufferSpan.From(mRequestBuffer, 1);

            var response = await Execute(FeigCommand.RFOutputOnOff, data, timeout, cancellationToken)
               .ConfigureAwait(false);

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  SwitchRF()  =>  {1}",
                    mSettings.TransportSettings.PortName,
                    response.Status
                );
            }

            #endregion
        }

        /// <summary>
        ///     Gets information about the reader/module's software.
        /// </summary>
        /// 
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <returns>
        ///     An object containing the parsed response data.
        /// </returns>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public async Task<FeigSoftwareInfo> GetSoftwareInfo(
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
                mLogger.LogInformation("[{0}]  GetSoftwareInfo()", mSettings.TransportSettings.PortName);

            #endregion

            var response = await Execute(FeigCommand.GetSoftwareVersion, BufferSpan.Empty, timeout, cancellationToken)
               .ConfigureAwait(false);

            var info = new FeigSoftwareInfo {
                FirmwareVersion       = new Version(response.Data[0], response.Data[1], response.Data[2]),
                HardwareType          = response.Data[3],
                ReaderType            = (FeigReaderType) response.Data[4],
                SupportedTransponders = ( response.Data[5] << 8 ) | response.Data[6],
            };

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  GetSoftwareInfo()  =>  {1}; {{ {2} }}",
                    mSettings.TransportSettings.PortName,
                    response.Status,
                    info
                );
            }

            #endregion

            return info;
        }

        /// <summary>
        ///     Reads a configuration block (14 bytes) from the reader's RAM or EEPROM.
        /// </summary>
        /// 
        /// <param name="block">
        ///     The configuration block to read.
        /// </param>
        /// <param name="location">
        ///     The location of the block to read from, either EEPROM or RAM.
        /// </param>
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <returns>
        ///     The configuration data.
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The block number must be between 0 and 63.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public async Task<BufferSpan> ReadConfiguration(
            Int32 block,
            FeigBlockLocation location,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            Verify.IsInRange(block, 0, 63, nameof(block));

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  ReadConfiguration(Block: {1}, Location: {2})",
                    mSettings.TransportSettings.PortName,
                    block,
                    location
                );
            }

            #endregion

            var addr = (Byte) 0x00;
            addr |= (Byte) location;
            addr |= (Byte) ( block & 0x3F );

            mRequestBuffer[0] = addr;
            var data = BufferSpan.From(mRequestBuffer, 0, 1);

            var response = await Execute(FeigCommand.ReadConfiguration, data, timeout, cancellationToken)
               .ConfigureAwait(false);

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  ReadConfiguration()  =>  {1}; {2}",
                    mSettings.TransportSettings.PortName,
                    response.Status,
                    response.Data
                );
            }

            #endregion

            return response.Data;
        }

        /// <summary>
        ///     Writes a configuration block (14 bytes) to the reader's RAM or EEPROM.
        /// </summary>
        /// 
        /// <param name="block">
        ///     The configuration block to write.
        /// </param>
        /// <param name="location">
        ///     The location of the block to write to, either EEPROM or RAM.
        /// </param>
        /// <param name="data">
        ///     The data of the configuration block; must be exactly 14 bytes.
        /// </param>
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The block number must be between 0 and 63.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Exactly 14 bytes must be specified as configuration data.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public Task WriteConfiguration(
            Int32 block,
            FeigBlockLocation location,
            BufferSpan data,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            Verify.IsInRange(block, 0, 63, nameof(block));

            if (data.Count != 14)
                throw new ArgumentException("Exactly 14 bytes must be specified as configuration data.", nameof(data));

            return writeConfiguration();


            async Task writeConfiguration()
            {
                #region (logging)

                if (mLogger.IsEnabled(LogLevel.Information))
                {
                    mLogger.LogInformation(
                        "[{0}]  WriteConfiguration(Block: {1}, Location: {2}, Data: {3})",
                        mSettings.TransportSettings.PortName,
                        block,
                        location,
                        data
                    );
                }

                #endregion

                var addr = (Byte) 0x00;
                addr |= (Byte) location;
                addr |= (Byte) ( block & 0x3F );

                mRequestBuffer[0] = addr;

                Buffer.BlockCopy(
                    data.Buffer,
                    data.Offset,
                    mRequestBuffer,
                    1,
                    data.Count
                );

                var cfgdata = BufferSpan.From(mRequestBuffer, 0, 1 + data.Count);

                var response = await Execute(FeigCommand.WriteConfiguration, cfgdata, timeout, cancellationToken)
                   .ConfigureAwait(false);

                #region (logging)

                if (mLogger.IsEnabled(LogLevel.Information))
                {
                    mLogger.LogInformation(
                        "[{0}]  WriteConfiguration()  =>  {1}",
                        mSettings.TransportSettings.PortName,
                        response.Status
                    );
                }

                #endregion
            }
        }

        /// <summary>
        ///     Saves all configuration blocks currently in the reader's RAM to EEPROM.
        /// </summary>
        /// 
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public async Task SaveConfigurations(TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
                mLogger.LogInformation("[{0}]  SaveConfigurations()", mSettings.TransportSettings.PortName);

            #endregion

            mRequestBuffer[0] = 0x40;

            var data = BufferSpan.From(mRequestBuffer, 0, 1);

            var response = await Execute(FeigCommand.SaveConfiguration, data, timeout, cancellationToken)
               .ConfigureAwait(false);

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  SaveConfigurations()  =>  {1}",
                    mSettings.TransportSettings.PortName,
                    response.Status
                );
            }

            #endregion
        }

        /// <summary>
        ///     Saves the specified configuration block currently in the reader's RAM to EEPROM.
        /// </summary>
        /// 
        /// <param name="block">
        ///     The configuration block to save.
        /// </param>
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The block number must be between 0 and 63.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public async Task SaveConfiguration(
            Int32 block,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            Verify.IsInRange(block, 0, 63, nameof(block));

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  SaveConfiguration(Block: {1})",
                    mSettings.TransportSettings.PortName,
                    block
                );
            }

            #endregion

            mRequestBuffer[0] = (Byte) ( block & 0x3F );

            var data = BufferSpan.From(mRequestBuffer, 0, 1);

            var response = await Execute(FeigCommand.SaveConfiguration, data, timeout, cancellationToken)
               .ConfigureAwait(false);

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  SaveConfiguration()  =>  {1}",
                    mSettings.TransportSettings.PortName,
                    response.Status
                );
            }

            #endregion
        }

        /// <summary>
        ///     Resets all configuration blocks to their defaults.
        /// </summary>
        /// 
        /// <param name="location">
        ///     The location of the block to reset, either EEPROM or RAM.
        /// </param>
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public async Task ResetConfigurations(
            FeigBlockLocation location,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  ResetConfigurations(Location: {1})",
                    mSettings.TransportSettings.PortName,
                    location
                );
            }

            #endregion

            var addr = (Byte) 0x00;
            addr |= (Byte) location;
            addr |= 0x40;

            mRequestBuffer[0] = addr;

            var data = BufferSpan.From(mRequestBuffer, 0, 1);

            var response = await Execute(FeigCommand.SetDefaultConfiguration, data, timeout, cancellationToken)
               .ConfigureAwait(false);

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  ResetConfigurations()  =>  {1}",
                    mSettings.TransportSettings.PortName,
                    response.Status
                );
            }

            #endregion
        }

        /// <summary>
        ///     Resets the specified configuration block to its defaults.
        /// </summary>
        /// 
        /// <param name="block">
        ///     The configuration block to reset.
        /// </param>
        /// <param name="location">
        ///     The location of the block to reset, either EEPROM or RAM.
        /// </param>
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The block number must be between 0 and 63.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public async Task ResetConfiguration(
            Int32 block,
            FeigBlockLocation location,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            Verify.IsInRange(block, 0, 63, nameof(block));

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  ResetConfiguration(Block: {1}, Location: {2})",
                    mSettings.TransportSettings.PortName,
                    block,
                    location
                );
            }

            #endregion

            var addr = (Byte) 0x00;
            addr |= (Byte) location;
            addr |= (Byte) ( block & 0x3F );

            mRequestBuffer[0] = addr;

            var data = BufferSpan.From(mRequestBuffer, 0, 1);

            var response = await Execute(FeigCommand.SetDefaultConfiguration, data, timeout, cancellationToken)
               .ConfigureAwait(false);

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  ResetConfiguration()  =>  {1}",
                    mSettings.TransportSettings.PortName,
                    response.Status
                );
            }

            #endregion
        }

        /// <summary>
        ///     Reads the identifier data of all transponders inside the antenna field.
        /// </summary>
        /// 
        /// <param name="timeout">
        ///     (Optional) The timeout for this transfer operation. If not specified, the global timeout is
        ///     used.
        /// </param>
        /// <param name="cancellationToken">
        ///     (Optional) A cancellation token that can be used to cancel the transfer operation.
        /// </param>
        /// 
        /// <returns>
        ///     A tuple containing the decoded transponders and the received response.
        /// </returns>
        /// 
        /// <exception cref="ObjectDisposedException">
        ///     A method or property was called on an already disposed object.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The transport has not been opened yet.
        /// </exception>
        /// <exception cref="TimeoutException">
        ///     The operation '(request)' timed out after (timeout) ms.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     The operation '(request)' has been canceled.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because of a communication error. Received corrupted
        ///     '(response)'.
        /// </exception>
        /// <exception cref="FeigException">
        ///     The operation '(request)' failed because the reader returned error code '(error)'. Received
        ///     '(response)'.
        /// </exception>
        public async Task<(FeigTransponder[] Transponders, FeigResponse Response)> Inventory(
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
                mLogger.LogInformation("[{0}]  Inventory()", mSettings.TransportSettings.PortName);

            #endregion

            mRequestBuffer[0] = (Byte) FeigISOStandardCommand.Inventory;
            mRequestBuffer[1] = 0x00; // new inventory

            var data = BufferSpan.From(mRequestBuffer, 0, 2);

            var response = await Execute(FeigCommand.ISOStandardHostCommand, data, timeout, cancellationToken)
               .ConfigureAwait(false);

            var rspDat = response.Data;

            var result = response.Status == FeigStatus.NoTransponder
                ? Array.Empty<FeigTransponder>()
                : Inventory_Parse(ref rspDat);

            #region (logging)

            if (mLogger.IsEnabled(LogLevel.Information))
            {
                mLogger.LogInformation(
                    "[{0}]  Inventory()  =>  {1}; {2}",
                    mSettings.TransportSettings.PortName,
                    response.Status,
                    FeigTransponder.ToString(result)
                );
            }

            #endregion

            return ( result, response );
        }

        internal static FeigTransponder[] Inventory_Parse(ref BufferSpan data)
        {
            var count        = data[0];
            var transponders = new FeigTransponder[count];

            data = data.Discard(1);

            for (var i = 0; i < count; i++)
                transponders[i] = _Inventory_ParseSingle(ref data);

            return transponders;
        }

        private static FeigTransponder _Inventory_ParseSingle(ref BufferSpan data)
        {
            var transponderType = (FeigTransponderType) data[0];
            data = data.Discard(1);

            return transponderType switch {
                FeigTransponderType.ISO14443A       => Inventory_Parse_ISO14443A(ref data),
                FeigTransponderType.ISO14443B       => Inventory_Parse_ISO14443B(ref data),
                FeigTransponderType.Innovatron      => Inventory_Parse_Innovatron(ref data),
                FeigTransponderType.Jewel           => Inventory_Parse_Jewel(ref data),
                FeigTransponderType.SR176           => Inventory_Parse_SR176(ref data),
                FeigTransponderType.SRIxx           => Inventory_Parse_SRIxx(ref data),
                FeigTransponderType.FeliCa          => Inventory_Parse_FeliCa(ref data),
                FeigTransponderType.ISO15693        => Inventory_Parse_ISO15693(ref data),
                FeigTransponderType.ISO18000_3M3    => Inventory_Parse_ISO18000_3M3(ref data),
                FeigTransponderType.EPC_Class1_Gen2 => Inventory_Parse_EPC_Class1_Gen2(ref data),
                FeigTransponderType.ICode1          => Inventory_Parse_ICode1(ref data),
                FeigTransponderType.ICodeEPC        => Inventory_Parse_ICodeEPC(ref data),
                FeigTransponderType.ICodeUID        => Inventory_Parse_ICodeUID(ref data),
                _ => throw new NotSupportedException(
                    $"Decoding response for transponder type {transponderType} is not supported!"
                ),
            };
        }

        internal static FeigTransponder Inventory_Parse_ISO14443A(ref BufferSpan data)
        {
            var info       = data[0];
            var length     = ( info & 0x04 ) != 0 ? 10 : 7;
            var identifier = data.Slice(2, length);

            data = data.Discard(2 + length);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.ISO14443A,
                Identifier      = identifier,
            };
        }

        internal static FeigTransponder Inventory_Parse_ISO14443B(ref BufferSpan data)
        {
            var identifier = data.Slice(5, 4).Clone();
            Array.Reverse(identifier.Buffer, identifier.Offset, identifier.Count);

            data = data.Discard(9);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.ISO14443B,
                Identifier      = identifier,
            };
        }

        internal static FeigTransponder Inventory_Parse_Innovatron(ref BufferSpan data)
        {
            var identifier = data.Slice(0, 8).Clone();
            var length     = data[10];

            data = data.Discard(11 + length);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.Innovatron,
                Identifier      = identifier,
            };
        }

        internal static FeigTransponder Inventory_Parse_Jewel(ref BufferSpan data)
        {
            var identifier = data.Slice(4, 4).Clone();
            Array.Reverse(identifier.Buffer, identifier.Offset, identifier.Count);

            data = data.Discard(8);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.Jewel,
                Identifier      = identifier,
            };
        }

        internal static FeigTransponder Inventory_Parse_SR176(ref BufferSpan data)
        {
            var identifier = data.Slice(1, 8).Clone();
            Array.Reverse(identifier.Buffer, identifier.Offset, identifier.Count);

            data = data.Discard(9);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.SR176,
                Identifier      = identifier,
            };
        }

        internal static FeigTransponder Inventory_Parse_SRIxx(ref BufferSpan data)
        {
            var identifier = data.Slice(1, 8).Clone();
            Array.Reverse(identifier.Buffer, identifier.Offset, identifier.Count);

            data = data.Discard(9);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.SRIxx,
                Identifier      = identifier,
            };
        }

        internal static FeigTransponder Inventory_Parse_FeliCa(ref BufferSpan data)
        {
            // TODO
            return null;
        }

        internal static FeigTransponder Inventory_Parse_ISO15693(ref BufferSpan data)
        {
            var identifier = data.Slice(1, 8);

            data = data.Discard(9);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.ISO15693,
                Identifier      = identifier,
            };
        }

        internal static FeigTransponder Inventory_Parse_ISO18000_3M3(ref BufferSpan data)
        {
            var length     = data[1];
            var identifier = data.Slice(2, length);

            data = data.Discard(2 + length);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.ISO18000_3M3,
                Identifier      = identifier,
            };
        }

        internal static FeigTransponder Inventory_Parse_EPC_Class1_Gen2(ref BufferSpan data)
        {
            var length     = data[1];
            var identifier = data.Slice(2, length);

            data = data.Discard(2 + length);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.EPC_Class1_Gen2,
                Identifier      = identifier,
            };
        }

        internal static FeigTransponder Inventory_Parse_ICode1(ref BufferSpan data)
        {
            var identifier = data.Slice(1, 8);

            data = data.Discard(9);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.ICode1,
                Identifier      = identifier,
            };
        }

        internal static FeigTransponder Inventory_Parse_ICodeEPC(ref BufferSpan data)
        {
            var identifier = data.Slice(0, 8);

            data = data.Discard(8);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.ICodeEPC,
                Identifier      = identifier,
            };
        }

        internal static FeigTransponder Inventory_Parse_ICodeUID(ref BufferSpan data)
        {
            var identifier = data.Slice(14, 5);

            data = data.Discard(19);

            return new FeigTransponder {
                TransponderType = FeigTransponderType.ICodeUID,
                Identifier      = identifier,
            };
        }

        #endregion
    }
}
