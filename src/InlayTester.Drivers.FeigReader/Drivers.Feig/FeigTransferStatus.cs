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

namespace InlayTester.Drivers.Feig
{
    /// <summary>
    /// This enumeration lists possible transfer statuses.
    /// </summary>
    public enum FeigTransferStatus
    {
        /// <summary>
        /// The transfer completed successful. A response has been received.
        /// </summary>
        Success = 0,

        /// <summary>
        /// The transfer has been canceled. No response has been received.
        /// </summary>
        Canceled = 1,

        /// <summary>
        /// The transfer has timed out. No response has been received.
        /// </summary>
        Timeout = -1,

        /// <summary>
        /// The transfer failed, because the received response cannot be decoded.
        /// </summary>
        CommunicationError = -2,

        /// <summary>
        /// The transfer failed, because an unexpected response has been received.
        /// </summary>
        UnexpectedResponse = -3,
    }
}
