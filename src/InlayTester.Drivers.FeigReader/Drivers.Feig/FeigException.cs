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

#pragma warning disable S3925 // "ISerializable" should be implemented correctly

using System;


namespace InlayTester.Drivers.Feig
{
    /// <summary>
    ///     This exception indicates that an error occurred while interacting with the Feig RFID reader.
    /// </summary>
    public class FeigException : Exception
    {
        /// <summary>
        ///     Gets the request sent to the reader, if available, otherwise null.
        /// </summary>
        public FeigRequest? Request { get; }

        /// <summary>
        ///     Gets the response received from the reader, if available, otherwise null.
        /// </summary>
        public FeigResponse? Response { get; }


        /// <summary>
        ///     Initializes a new instance.
        /// </summary>
        /// 
        /// <param name="request">
        ///     The request sent to the reader, if available, otherwise null.
        /// </param>
        /// <param name="response">
        ///     The response received from the reader, if available, otherwise null.
        /// </param>
        public FeigException(FeigRequest? request, FeigResponse? response)
        {
            Request  = request;
            Response = response;
        }

        /// <summary>
        ///     Initializes a new instance.
        /// </summary>
        public FeigException()
        {
        }

        /// <summary>
        ///     Initializes a new instance.
        /// </summary>
        /// 
        /// <param name="message">
        ///     The error message that explains the reason for the exception.
        /// </param>
        /// <param name="request">
        ///     The request sent to the reader, if available, otherwise null.
        /// </param>
        /// <param name="response">
        ///     The response received from the reader, if available, otherwise null.
        /// </param>
        public FeigException(String message, FeigRequest? request, FeigResponse? response)
            : base(message)
        {
            Request  = request;
            Response = response;
        }

        /// <summary>
        ///     Initializes a new instance.
        /// </summary>
        /// 
        /// <param name="message">
        ///     The error message that explains the reason for the exception.
        /// </param>
        public FeigException(String message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance.
        /// </summary>
        /// 
        /// <param name="message">
        ///     The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception.
        /// </param>
        public FeigException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
