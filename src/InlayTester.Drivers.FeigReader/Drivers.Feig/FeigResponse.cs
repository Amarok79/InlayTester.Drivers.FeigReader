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
using System.Text;
using InlayTester.Shared;


namespace InlayTester.Drivers.Feig
{
	/// <summary>
	/// This type represents a single response.
	/// </summary>
	public sealed class FeigResponse
	{
		/// <summary>
		/// The length of the response frame.
		/// </summary>
		public Int32 FrameLength { get; set; }

		/// <summary>
		/// The address of the device.
		/// </summary>
		public Byte Address { get; set; }

		/// <summary>
		/// The command the reader performed.
		/// </summary>
		public FeigCommand Command { get; set; }

		/// <summary>
		/// The status of the command.
		/// </summary>
		public FeigStatus Status { get; set; }

		/// <summary>
		/// The response data for the command.
		/// </summary>
		public BufferSpan Data { get; set; }

		/// <summary>
		/// The checksum of the response frame.
		/// </summary>
		public Int32 Crc { get; set; }


		/// <summary>
		/// Returns a string that represents the current instance.
		/// </summary>
		public override String ToString()
		{
			StringBuilder sb = null;

			try
			{
				sb = StringBuilderPool.Alloc();

				sb.Append("Address: ");
				sb.Append(this.Address);
				sb.Append(", Command: ");
				sb.Append(this.Command);
				sb.Append(", Status: ");
				sb.Append(this.Status);
				sb.Append(", Data: ");
				sb.Append(this.Data);

				return sb.ToString();
			}
			finally
			{
				StringBuilderPool.Free(sb);
			}
		}


		/// <summary>
		/// Attempts to parse a response from the given data.
		/// </summary>
		public static FeigParseResult TryParse(in BufferSpan span, FeigProtocol protocol)
		{
			if (protocol == FeigProtocol.Advanced)
				return _TryParseAdvancedProtocolFrame(span);
			else
			if (protocol == FeigProtocol.Standard)
				return _TryParseStandardProtocolFrame(span);
			else
				throw ExceptionFactory.NotSupportedException("Protocol {0} not supported!", protocol);
		}

		private static FeigParseResult _TryParseAdvancedProtocolFrame(in BufferSpan span)
		{
			if (span.Count < 8)
				return FeigParseResult.MoreDataNeeded();

			if (span[0] != 0x02)
				return FeigParseResult.FrameError();

			var lenHigh = span[1];
			var lenLow = span[2];
			var frameLength = (lenHigh << 8) | lenLow;

			if (span.Count < frameLength)
				return FeigParseResult.MoreDataNeeded();

			var address = span[3];
			var command = span[4];
			var status = span[5];

			var crcLow = span[frameLength - 2];
			var crcHigh = span[frameLength - 1];
			var crc = (crcHigh << 8) | crcLow;

			var calcCrc = FeigChecksum.Calculate(BufferSpan.From(span.Buffer, span.Offset, frameLength - 2));

			var data = span.Slice(6, frameLength - 8);

			var response = new FeigResponse() {
				FrameLength = frameLength,
				Address = address,
				Command = (FeigCommand)command,
				Status = (FeigStatus)status,
				Data = data,
				Crc = calcCrc
			};

			if (crc != calcCrc)
				return FeigParseResult.ChecksumError(response);

			return FeigParseResult.Success(response);
		}

		private static FeigParseResult _TryParseStandardProtocolFrame(in BufferSpan span)
		{
			if (span.Count < 6)
				return FeigParseResult.MoreDataNeeded();

			var frameLength = span[0];

			if (frameLength < 6)
				return FeigParseResult.FrameError();

			if (span.Count < frameLength)
				return FeigParseResult.MoreDataNeeded();

			var address = span[1];
			var command = span[2];
			var status = span[3];

			var crcLow = span[frameLength - 2];
			var crcHigh = span[frameLength - 1];
			var crc = (crcHigh << 8) | crcLow;

			var calcCrc = FeigChecksum.Calculate(BufferSpan.From(span.Buffer, span.Offset, frameLength - 2));

			var data = span.Slice(4, frameLength - 6);

			var response = new FeigResponse() {
				FrameLength = frameLength,
				Address = address,
				Command = (FeigCommand)command,
				Status = (FeigStatus)status,
				Data = data,
				Crc = calcCrc
			};

			if (crc != calcCrc)
				return FeigParseResult.ChecksumError(response);

			return FeigParseResult.Success(response);
		}
	}
}
