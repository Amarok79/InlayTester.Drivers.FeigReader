/* MIT License
 * 
 * Copyright (c) 2019, Olaf Kober
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
using Amarok.Shared;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig
{
	public class Test_FeigResponse
	{
		[TestFixture]
		public class TryParse
		{
			[Test]
			public void StandardResponse_GetSoftwareVersion()
			{
				// act
				var data = new Byte[] {
					0x0D, 0x00, 0x65, 0x00, 0x03, 0x03, 0x00, 0x44,
					0x53, 0x0D, 0x30, 0x33, 0x09
				};

				var span = BufferSpan.From(data, 0, data.Length);

				var result = FeigResponse.TryParse(span, FeigProtocol.Standard);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.Success);
				Check.That(result.Response)
					.IsNotNull();

				Check.That(result.Response.FrameLength)
					.IsEqualTo(13);
				Check.That(result.Response.Address)
					.IsEqualTo(0x00);
				Check.That((FeigCommand)result.Response.Command)
					.IsEqualTo(FeigCommand.GetSoftwareVersion);
				Check.That((FeigStatus)result.Response.Status)
					.IsEqualTo(FeigStatus.OK);
				Check.That(result.Response.Data.ToArray())
					.ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0D, 0x30);
				Check.That(result.Response.Crc)
					.IsEqualTo(0x0933);

				Check.That(result.Response.ToString())
					.IsEqualTo("Address: 0, Command: GetSoftwareVersion, Status: OK, Data: 03-03-00-44-53-0D-30");
			}

			[Test]
			public void StandardResponse_GetSoftwareVersion_ByteByByte()
			{
				var data = new Byte[] {
					0xFF, 0xFF,
					0x0D, 0x00, 0x65, 0x00, 0x03, 0x03, 0x00, 0x44,
					0x53, 0x0D, 0x30, 0x33, 0x09,
					0xFF, 0xFF
				};

				// act
				var result = FeigResponse.TryParse(BufferSpan.From(data, 2, 0), FeigProtocol.Standard);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.MoreDataNeeded);
				Check.That(result.Response)
					.IsNull();

				// act
				result = FeigResponse.TryParse(BufferSpan.From(data, 2, 1), FeigProtocol.Standard);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.MoreDataNeeded);
				Check.That(result.Response)
					.IsNull();

				// act
				result = FeigResponse.TryParse(BufferSpan.From(data, 2, 2), FeigProtocol.Standard);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.MoreDataNeeded);
				Check.That(result.Response)
					.IsNull();

				// act
				result = FeigResponse.TryParse(BufferSpan.From(data, 2, 12), FeigProtocol.Standard);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.MoreDataNeeded);
				Check.That(result.Response)
					.IsNull();

				// act
				result = FeigResponse.TryParse(BufferSpan.From(data, 2, 13), FeigProtocol.Standard);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.Success);
				Check.That(result.Response)
					.IsNotNull();

				Check.That(result.Response.FrameLength)
					.IsEqualTo(13);
				Check.That(result.Response.Address)
					.IsEqualTo(0x00);
				Check.That((FeigCommand)result.Response.Command)
					.IsEqualTo(FeigCommand.GetSoftwareVersion);
				Check.That((FeigStatus)result.Response.Status)
					.IsEqualTo(FeigStatus.OK);
				Check.That(result.Response.Data.ToArray())
					.ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0D, 0x30);
				Check.That(result.Response.Crc)
					.IsEqualTo(0x0933);

				Check.That(result.Response.ToString())
					.IsEqualTo("Address: 0, Command: GetSoftwareVersion, Status: OK, Data: 03-03-00-44-53-0D-30");
			}

			[Test]
			public void StandardResponse_GetSoftwareVersion_ChecksumError()
			{
				// act
				var data = new Byte[] {
					0x0D, 0x00, 0x65, 0x00, 0x03, 0x03, 0x00, 0x44,
					0x53, 0x0D, 0x30, 0x33, /*0x09*/ 0x08
				};

				var span = BufferSpan.From(data, 0, data.Length);

				var result = FeigResponse.TryParse(span, FeigProtocol.Standard);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.ChecksumError);
				Check.That(result.Response)
					.IsNotNull();

				Check.That(result.Response.FrameLength)
					.IsEqualTo(13);
				Check.That(result.Response.Address)
					.IsEqualTo(0x00);
				Check.That((FeigCommand)result.Response.Command)
					.IsEqualTo(FeigCommand.GetSoftwareVersion);
				Check.That((FeigStatus)result.Response.Status)
					.IsEqualTo(FeigStatus.OK);
				Check.That(result.Response.Data.ToArray())
					.ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0D, 0x30);
				Check.That(result.Response.Crc)
					.IsEqualTo(0x0933);

				Check.That(result.Response.ToString())
					.IsEqualTo("Address: 0, Command: GetSoftwareVersion, Status: OK, Data: 03-03-00-44-53-0D-30");
			}

			[Test]
			public void StandardResponse_GetSoftwareVersion_FrameError()
			{
				// act
				var data = new Byte[] {
					/*0x0D,*/ 0x03, 0x00, 0x65, 0x00, 0x03, 0x03, 0x00, 0x44,
					0x53, 0x0D, 0x30, 0x33, 0x08
				};

				var span = BufferSpan.From(data, 0, data.Length);

				var result = FeigResponse.TryParse(span, FeigProtocol.Standard);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.FrameError);
				Check.That(result.Response)
					.IsNull();
			}


			[Test]
			public void AdvancedResponse_GetSoftwareVersion()
			{
				// act
				var data = new Byte[] {
					0x02, 0x00, 0x0F, 0x00, 0x65, 0x00, 0x03, 0x03,
					0x00, 0x44, 0x53, 0x0D, 0x30, 0x74, 0x69
				};

				var span = BufferSpan.From(data, 0, data.Length);

				var result = FeigResponse.TryParse(span, FeigProtocol.Advanced);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.Success);
				Check.That(result.Response)
					.IsNotNull();

				Check.That(result.Response.FrameLength)
					.IsEqualTo(15);
				Check.That(result.Response.Address)
					.IsEqualTo(0x00);
				Check.That((FeigCommand)result.Response.Command)
					.IsEqualTo(FeigCommand.GetSoftwareVersion);
				Check.That((FeigStatus)result.Response.Status)
					.IsEqualTo(FeigStatus.OK);
				Check.That(result.Response.Data.ToArray())
					.ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0D, 0x30);
				Check.That(result.Response.Crc)
					.IsEqualTo(0x6974);

				Check.That(result.Response.ToString())
					.IsEqualTo("Address: 0, Command: GetSoftwareVersion, Status: OK, Data: 03-03-00-44-53-0D-30");
			}

			[Test]
			public void AdvancedResponse_GetSoftwareVersion_ByteByByte()
			{
				var data = new Byte[] {
					0xFF, 0xFF,
					0x02, 0x00, 0x0F, 0x00, 0x65, 0x00, 0x03, 0x03,
					0x00, 0x44, 0x53, 0x0D, 0x30, 0x74, 0x69,
					0xFF, 0xFF
				};

				// act
				var result = FeigResponse.TryParse(BufferSpan.From(data, 2, 0), FeigProtocol.Advanced);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.MoreDataNeeded);
				Check.That(result.Response)
					.IsNull();

				// act
				result = FeigResponse.TryParse(BufferSpan.From(data, 2, 1), FeigProtocol.Advanced);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.MoreDataNeeded);
				Check.That(result.Response)
					.IsNull();

				// act
				result = FeigResponse.TryParse(BufferSpan.From(data, 2, 2), FeigProtocol.Advanced);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.MoreDataNeeded);
				Check.That(result.Response)
					.IsNull();

				// act
				result = FeigResponse.TryParse(BufferSpan.From(data, 2, 14), FeigProtocol.Advanced);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.MoreDataNeeded);
				Check.That(result.Response)
					.IsNull();

				// act
				result = FeigResponse.TryParse(BufferSpan.From(data, 2, 15), FeigProtocol.Advanced);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.Success);
				Check.That(result.Response)
					.IsNotNull();

				Check.That(result.Response.FrameLength)
					.IsEqualTo(15);
				Check.That(result.Response.Address)
					.IsEqualTo(0x00);
				Check.That((FeigCommand)result.Response.Command)
					.IsEqualTo(FeigCommand.GetSoftwareVersion);
				Check.That((FeigStatus)result.Response.Status)
					.IsEqualTo(FeigStatus.OK);
				Check.That(result.Response.Data.ToArray())
					.ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0D, 0x30);
				Check.That(result.Response.Crc)
					.IsEqualTo(0x6974);

				Check.That(result.Response.ToString())
					.IsEqualTo("Address: 0, Command: GetSoftwareVersion, Status: OK, Data: 03-03-00-44-53-0D-30");
			}

			[Test]
			public void AdvancedResponse_GetSoftwareVersion_ChecksumError()
			{
				// act
				var data = new Byte[] {
					0x02, 0x00, 0x0F, 0x00, 0x65, 0x00, 0x03, 0x03,
					0x00, 0x44, 0x53, 0x0D, 0x30, 0x74, /*0x69*/ 0x68
				};

				var span = BufferSpan.From(data, 0, data.Length);

				var result = FeigResponse.TryParse(span, FeigProtocol.Advanced);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.ChecksumError);
				Check.That(result.Response)
					.IsNotNull();

				Check.That(result.Response.FrameLength)
					.IsEqualTo(15);
				Check.That(result.Response.Address)
					.IsEqualTo(0x00);
				Check.That((FeigCommand)result.Response.Command)
					.IsEqualTo(FeigCommand.GetSoftwareVersion);
				Check.That((FeigStatus)result.Response.Status)
					.IsEqualTo(FeigStatus.OK);
				Check.That(result.Response.Data.ToArray())
					.ContainsExactly(0x03, 0x03, 0x00, 0x44, 0x53, 0x0D, 0x30);
				Check.That(result.Response.Crc)
					.IsEqualTo(0x6974);

				Check.That(result.Response.ToString())
					.IsEqualTo("Address: 0, Command: GetSoftwareVersion, Status: OK, Data: 03-03-00-44-53-0D-30");
			}

			[Test]
			public void AdvancedResponse_GetSoftwareVersion_FrameError()
			{
				// act
				var data = new Byte[] {
					/*0x02*/ 0xFF, 0x00, 0x0F, 0x00, 0x65, 0x00, 0x03, 0x03,
					0x00, 0x44, 0x53, 0x0D, 0x30, 0x74, 0x68
				};

				var span = BufferSpan.From(data, 0, data.Length);

				var result = FeigResponse.TryParse(span, FeigProtocol.Advanced);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigParseStatus.FrameError);
				Check.That(result.Response)
					.IsNull();
			}
		}
	}
}
