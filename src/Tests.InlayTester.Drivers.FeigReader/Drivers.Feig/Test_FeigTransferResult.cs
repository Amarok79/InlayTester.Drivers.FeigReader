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
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig
{
	public class Test_FeigTransferResult
	{
		[TestFixture]
		public class Success
		{
			[Test]
			public void Success_With_Response()
			{
				// act
				var request = new FeigRequest();
				var response = new FeigResponse();
				var result = FeigTransferResult.Success(request, response);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigTransferStatus.Success);
				Check.That(result.Request)
					.IsSameReferenceAs(request);
				Check.That(result.Response)
					.IsSameReferenceAs(response);

				Check.That(result.ToString())
					.IsEqualTo("Status: Success, Request: { Address: 255, Command: None, Data: <empty> }, Response: { Address: 0, Command: None, Status: OK, Data: <empty> }");
			}

			[Test]
			public void Exception_With_NullRequest()
			{
				Check.ThatCode(() => FeigTransferResult.Success(null, new FeigResponse()))
					.Throws<ArgumentNullException>();
			}

			[Test]
			public void Exception_With_NullResponse()
			{
				Check.ThatCode(() => FeigTransferResult.Success(new FeigRequest(), null))
					.Throws<ArgumentNullException>();
			}
		}

		[TestFixture]
		public class Canceled
		{
			[Test]
			public void Success()
			{
				// act
				var request = new FeigRequest();
				var result = FeigTransferResult.Canceled(request);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigTransferStatus.Canceled);
				Check.That(result.Request)
					.IsSameReferenceAs(request);
				Check.That(result.Response)
					.IsNull();

				Check.That(result.ToString())
					.IsEqualTo("Status: Canceled, Request: { Address: 255, Command: None, Data: <empty> }, Response: { <null> }");
			}

			[Test]
			public void Exception_With_NullRequest()
			{
				Check.ThatCode(() => FeigTransferResult.Canceled(null))
					.Throws<ArgumentNullException>();
			}
		}

		[TestFixture]
		public class Timeout
		{
			[Test]
			public void Success()
			{
				// act
				var request = new FeigRequest();
				var result = FeigTransferResult.Timeout(request);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigTransferStatus.Timeout);
				Check.That(result.Request)
					.IsSameReferenceAs(request);
				Check.That(result.Response)
					.IsNull();

				Check.That(result.ToString())
					.IsEqualTo("Status: Timeout, Request: { Address: 255, Command: None, Data: <empty> }, Response: { <null> }");
			}

			[Test]
			public void Exception_With_NullRequest()
			{
				Check.ThatCode(() => FeigTransferResult.Timeout(null))
					.Throws<ArgumentNullException>();
			}
		}

		[TestFixture]
		public class ChecksumError
		{
			[Test]
			public void Success()
			{
				// act
				var request = new FeigRequest();
				var response = new FeigResponse();
				var result = FeigTransferResult.ChecksumError(request, response);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigTransferStatus.ChecksumError);
				Check.That(result.Request)
					.IsSameReferenceAs(request);
				Check.That(result.Response)
					.IsSameReferenceAs(response);
				Check.That(result.ToString())
					.IsEqualTo("Status: ChecksumError, Request: { Address: 255, Command: None, Data: <empty> }, Response: { Address: 0, Command: None, Status: OK, Data: <empty> }");
			}

			[Test]
			public void Exception_With_NullRequest()
			{
				Check.ThatCode(() => FeigTransferResult.ChecksumError(null, new FeigResponse()))
					.Throws<ArgumentNullException>();
			}

			[Test]
			public void Exception_With_NullResponse()
			{
				Check.ThatCode(() => FeigTransferResult.ChecksumError(new FeigRequest(), null))
					.Throws<ArgumentNullException>();
			}
		}

		[TestFixture]
		public class UnexpectedResponse
		{
			[Test]
			public void Success()
			{
				// act
				var request = new FeigRequest();
				var response = new FeigResponse();
				var result = FeigTransferResult.UnexpectedResponse(request, response);

				// assert
				Check.That(result.Status)
					.IsEqualTo(FeigTransferStatus.UnexpectedResponse);
				Check.That(result.Request)
					.IsSameReferenceAs(request);
				Check.That(result.Response)
					.IsSameReferenceAs(response);
				Check.That(result.ToString())
					.IsEqualTo("Status: UnexpectedResponse, Request: { Address: 255, Command: None, Data: <empty> }, Response: { Address: 0, Command: None, Status: OK, Data: <empty> }");
			}

			[Test]
			public void Exception_With_NullRequest()
			{
				Check.ThatCode(() => FeigTransferResult.UnexpectedResponse(null, new FeigResponse()))
					.Throws<ArgumentNullException>();
			}

			[Test]
			public void Exception_With_NullResponse()
			{
				Check.ThatCode(() => FeigTransferResult.UnexpectedResponse(new FeigRequest(), null))
					.Throws<ArgumentNullException>();
			}
		}
	}
}
