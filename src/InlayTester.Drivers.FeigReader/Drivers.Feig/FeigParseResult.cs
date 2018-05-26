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
using InlayTester.Shared;


namespace InlayTester.Drivers.Feig
{
	/// <summary>
	/// This type represents the result of a <see cref="FeigResponse.TryParse"/> invocation.
	/// </summary>
	public readonly struct FeigParseResult
	{
		/// <summary>
		/// Gets the status of parse operation.
		/// </summary>
		public FeigParseStatus Status { get; }

		/// <summary>
		/// Gets the parsed response, if available, otherwise null.
		/// </summary>
		public FeigResponse Response { get; }


		private FeigParseResult(FeigParseStatus status, FeigResponse response)
		{
			this.Status = status;
			this.Response = response;
		}


		/// <summary>
		/// Returns a string that represents the current instance.
		/// </summary>
		public override String ToString()
		{
			return $"Status: {Status}, Response: {{ {Response?.ToString() ?? "<null>"} }}";
		}


		/// <summary>
		/// Returns a result indicating a successfully parsed response.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		/// A null reference was passed to a method that did not accept it as a valid argument.</exception>
		public static FeigParseResult Success(FeigResponse response)
		{
			Verify.NotNull(response, nameof(response));

			return new FeigParseResult(FeigParseStatus.Success, response);
		}

		/// <summary>
		/// Returns a result indicating that more data is needed for an entire response frame.
		/// </summary>
		public static FeigParseResult MoreDataNeeded()
		{
			return new FeigParseResult(FeigParseStatus.MoreDataNeeded, null);
		}

		/// <summary>
		/// Returns a result indicating that a checksum error has been detected.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		/// A null reference was passed to a method that did not accept it as a valid argument.</exception>
		public static FeigParseResult ChecksumError(FeigResponse response)
		{
			Verify.NotNull(response, nameof(response));

			return new FeigParseResult(FeigParseStatus.ChecksumError, response);
		}

		/// <summary>
		/// Returns a result indicating that an error in the protocol frame has been detected.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		/// A null reference was passed to a method that did not accept it as a valid argument.</exception>
		public static FeigParseResult FrameError()
		{
			return new FeigParseResult(FeigParseStatus.FrameError, null);
		}
	}
}
