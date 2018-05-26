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
	/// This type represents a single request.
	/// </summary>
	public sealed class FeigRequest
	{
		/// <summary>
		/// The address of the device. The reader can be addressed via address 255 at any time. 
		/// Defaults to 255.
		/// </summary>
		public Byte Address { get; set; } = 0xFF;

		/// <summary>
		/// The command the reader should perform.
		/// Defaults to <see cref="FeigCommand.None"/>.
		/// </summary>
		public FeigCommand Command { get; set; } = FeigCommand.None;

		/// <summary>
		/// The request data for the command.
		/// Defaults to empty data.
		/// </summary>
		public BufferSpan Data { get; set; } = BufferSpan.Empty;


		/// <summary>
		/// Returns a string that represents the current instance.
		/// </summary>
		public override String ToString()
		{
			return $"Address: {Address}, Command: {Command}, Data: {Data}";
		}


		/// <summary>
		/// Formats the request into its byte representation.
		/// </summary>
		public BufferSpan ToBufferSpan(FeigProtocol protocol = FeigProtocol.Standard)
		{
			if (protocol == FeigProtocol.Standard)
				return _ToStandardProtocolFrame();
			else if (protocol == FeigProtocol.Advanced)
				return _ToAdvancedProtocolFrame();
			else
				throw ExceptionFactory.NotSupportedException("Protocol {0} not supported!", protocol);
		}


		private BufferSpan _ToStandardProtocolFrame()
		{
			Int32 frameLength = 5 + this.Data.Count;
			Byte[] frame = new Byte[frameLength];

			frame[0] = (Byte)frameLength;
			frame[1] = (Byte)this.Address;
			frame[2] = (Byte)this.Command;

			System.Buffer.BlockCopy(this.Data.Buffer, this.Data.Offset, frame, 3, this.Data.Count);

			Int32 crc = FeigChecksum.Calculate(BufferSpan.From(frame, 0, frameLength - 2));

			frame[frameLength - 2] = (Byte)(crc & 0xff);
			frame[frameLength - 1] = (Byte)(crc >> 8);

			return BufferSpan.From(frame, frameLength);
		}

		private BufferSpan _ToAdvancedProtocolFrame()
		{
			Int32 frameLength = 7 + this.Data.Count;
			Byte[] frame = new Byte[frameLength];

			frame[0] = 0x02;
			frame[1] = (Byte)(frameLength >> 8);
			frame[2] = (Byte)(frameLength & 0xff);
			frame[3] = (Byte)this.Address;
			frame[4] = (Byte)this.Command;

			System.Buffer.BlockCopy(this.Data.Buffer, this.Data.Offset, frame, 5, this.Data.Count);

			Int32 crc = FeigChecksum.Calculate(BufferSpan.From(frame, 0, frameLength - 2));

			frame[frameLength - 2] = (Byte)(crc & 0xff);
			frame[frameLength - 1] = (Byte)(crc >> 8);

			return BufferSpan.From(frame, frameLength);
		}
	}
}
