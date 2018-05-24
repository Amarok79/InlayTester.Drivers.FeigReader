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
				var request = new FeigRequest();
				var response = new FeigResponse();
				var result = FeigTransferResult.Success(request, response);

				Check.That(result.Status)
					.IsEqualTo(FeigTransferStatus.Success);
				Check.That(result.Request)
					.IsSameReferenceAs(request);
				Check.That(result.Response)
					.IsSameReferenceAs(response);

				Check.That(result.ToString())
					.IsEqualTo("Status: Success, Request: { }, Response: { Address: 0, Command: None, Status: OK, Data: <empty> }");
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
				var result = FeigTransferResult.Canceled();

				Check.That(result.Status)
					.IsEqualTo(FeigTransferStatus.Canceled);
				Check.That(result.Response)
					.IsNull();

				Check.That(result.ToString())
					.IsEqualTo("Status: Canceled, Response: { <null> }");
			}
		}

		[TestFixture]
		public class Timeout
		{
			[Test]
			public void Success()
			{
				var result = FeigTransferResult.Timeout();

				Check.That(result.Status)
					.IsEqualTo(FeigTransferStatus.Timeout);
				Check.That(result.Response)
					.IsNull();

				Check.That(result.ToString())
					.IsEqualTo("Status: Timeout, Response: { <null> }");
			}
		}

		[TestFixture]
		public class ChecksumError
		{
			[Test]
			public void Success()
			{
				var response = new FeigResponse();
				var result = FeigTransferResult.ChecksumError(response);

				Check.That(result.Status)
					.IsEqualTo(FeigTransferStatus.ChecksumError);
				Check.That(result.Response)
					.IsSameReferenceAs(response);
				Check.That(result.ToString())
					.IsEqualTo("Status: ChecksumError, Response: { Address: 0, Command: None, Status: OK, Data: <empty> }");
			}
		}
	}
}
