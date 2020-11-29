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
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig
{
    public class Test_FeigParseResult
    {
        [TestFixture]
        public class Success
        {
            [Test]
            public void Success_With_Response()
            {
                // act
                var response = new FeigResponse();
                var result   = FeigParseResult.Success(response);

                // assert
                Check.That(result.Status).IsEqualTo(FeigParseStatus.Success);
                Check.That(result.Response).IsSameReferenceAs(response);

                Check.That(result.ToString())
                     .IsEqualTo("Status: Success, Response: { Address: 0, Command: None, Status: OK, Data: <empty> }");
            }

            [Test]
            public void Exception_With_NullResponse()
            {
                Check.ThatCode(() => FeigParseResult.Success(null)).Throws<ArgumentNullException>();
            }
        }

        [TestFixture]
        public class MoreDataNeeded
        {
            [Test]
            public void Success()
            {
                // act
                var result = FeigParseResult.MoreDataNeeded();

                // assert
                Check.That(result.Status).IsEqualTo(FeigParseStatus.MoreDataNeeded);
                Check.That(result.Response).IsNull();

                Check.That(result.ToString()).IsEqualTo("Status: MoreDataNeeded, Response: { <null> }");
            }
        }

        [TestFixture]
        public class ChecksumError
        {
            [Test]
            public void Success()
            {
                // act
                var response = new FeigResponse();
                var result   = FeigParseResult.ChecksumError(response);

                // assert
                Check.That(result.Status).IsEqualTo(FeigParseStatus.ChecksumError);
                Check.That(result.Response).IsSameReferenceAs(response);

                Check.That(result.ToString())
                     .IsEqualTo(
                          "Status: ChecksumError, Response: { Address: 0, Command: None, Status: OK, Data: <empty> }"
                      );
            }
        }

        [TestFixture]
        public class FrameError
        {
            [Test]
            public void Success()
            {
                // act
                var result = FeigParseResult.FrameError();

                // assert
                Check.That(result.Status).IsEqualTo(FeigParseStatus.FrameError);
                Check.That(result.Response).IsNull();

                Check.That(result.ToString()).IsEqualTo("Status: FrameError, Response: { <null> }");
            }
        }
    }
}
