// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


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
            Check.That(result.Status)
               .IsEqualTo(FeigParseStatus.Success);

            Check.That(result.Response)
               .IsSameReferenceAs(response);

            Check.That(result.ToString())
               .IsEqualTo(
                    "Status: Success, Response: { Address: 0, Command: None, Status: OK, Data: <empty> }"
                );
        }

        [Test]
        public void Exception_With_NullResponse()
        {
            Check.ThatCode(() => FeigParseResult.Success(null))
               .Throws<ArgumentNullException>();
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
            Check.That(result.Status)
               .IsEqualTo(FeigParseStatus.MoreDataNeeded);

            Check.That(result.Response)
               .IsNull();

            Check.That(result.ToString())
               .IsEqualTo("Status: MoreDataNeeded, Response: { <null> }");
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
            Check.That(result.Status)
               .IsEqualTo(FeigParseStatus.ChecksumError);

            Check.That(result.Response)
               .IsSameReferenceAs(response);

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
            Check.That(result.Status)
               .IsEqualTo(FeigParseStatus.FrameError);

            Check.That(result.Response)
               .IsNull();

            Check.That(result.ToString())
               .IsEqualTo("Status: FrameError, Response: { <null> }");
        }
    }
}
