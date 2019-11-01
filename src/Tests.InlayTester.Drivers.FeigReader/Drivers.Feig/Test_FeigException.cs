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
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig
{
	[TestFixture]
	public class Test_FeigException
	{
		[Test]
		public void Succeed_With_RequestResponse()
		{
			// act
			var request = new FeigRequest();
			var response = new FeigResponse();
			var exception = new FeigException(request, response);

			// assert
			Check.That(exception.Message)
				.Contains("InlayTester.Drivers.Feig.FeigException");
			Check.That(exception.InnerException)
				.IsNull();
			Check.That(exception.Request)
				.IsSameReferenceAs(request);
			Check.That(exception.Response)
				.IsSameReferenceAs(response);
		}

		[Test]
		public void Succeed_With_DefaultConstructor()
		{
			// act
			var exception = new FeigException();

			// assert
			Check.That(exception.Message)
				.Contains("InlayTester.Drivers.Feig.FeigException");
			Check.That(exception.InnerException)
				.IsNull();
			Check.That(exception.Request)
				.IsNull();
			Check.That(exception.Response)
				.IsNull();
		}

		[Test]
		public void Succeed_With_Message()
		{
			// act
			var exception = new FeigException("MSG");

			// assert
			Check.That(exception.Message)
				.IsEqualTo("MSG");
			Check.That(exception.InnerException)
				.IsNull();
			Check.That(exception.Request)
				.IsNull();
			Check.That(exception.Response)
				.IsNull();
		}

		[Test]
		public void Succeed_With_MessageRequestResponse()
		{
			// act
			var request = new FeigRequest();
			var response = new FeigResponse();
			var exception = new FeigException("MSG", request, response);

			// assert
			Check.That(exception.Message)
				.IsEqualTo("MSG");
			Check.That(exception.InnerException)
				.IsNull();
			Check.That(exception.Request)
				.IsSameReferenceAs(request);
			Check.That(exception.Response)
				.IsSameReferenceAs(response);
		}

		[Test]
		public void Succeed_With_MessageInnerException()
		{
			// act
			var innerException = new ApplicationException();
			var exception = new FeigException("MSG", innerException);

			// assert
			Check.That(exception.Message)
				.IsEqualTo("MSG");
			Check.That(exception.InnerException)
				.IsSameReferenceAs(innerException);
			Check.That(exception.Request)
				.IsNull();
			Check.That(exception.Response)
				.IsNull();
		}
	}
}
