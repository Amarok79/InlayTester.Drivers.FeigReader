// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


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
            Check.That(result.Status).IsEqualTo(FeigTransferStatus.Success);

            Check.That(result.Request).IsSameReferenceAs(request);

            Check.That(result.Response).IsSameReferenceAs(response);

            Check.That(result.ToString())
               .IsEqualTo(
                    "Status: Success, Request: { Address: 255, Command: None, Data: <empty> }, Response: { Address: 0, Command: None, Status: OK, Data: <empty> }"
                );
        }

        [Test]
        public void Exception_With_NullRequest()
        {
            Check.ThatCode(() => FeigTransferResult.Success(null, new FeigResponse())).Throws<ArgumentNullException>();
        }

        [Test]
        public void Exception_With_NullResponse()
        {
            Check.ThatCode(() => FeigTransferResult.Success(new FeigRequest(), null)).Throws<ArgumentNullException>();
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
            Check.That(result.Status).IsEqualTo(FeigTransferStatus.Canceled);

            Check.That(result.Request).IsSameReferenceAs(request);

            Check.That(result.Response).IsNull();

            Check.That(result.ToString())
               .IsEqualTo(
                    "Status: Canceled, Request: { Address: 255, Command: None, Data: <empty> }, Response: { <null> }"
                );
        }

        [Test]
        public void Exception_With_NullRequest()
        {
            Check.ThatCode(() => FeigTransferResult.Canceled(null)).Throws<ArgumentNullException>();
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
            Check.That(result.Status).IsEqualTo(FeigTransferStatus.Timeout);

            Check.That(result.Request).IsSameReferenceAs(request);

            Check.That(result.Response).IsNull();

            Check.That(result.ToString())
               .IsEqualTo(
                    "Status: Timeout, Request: { Address: 255, Command: None, Data: <empty> }, Response: { <null> }"
                );
        }

        [Test]
        public void Exception_With_NullRequest()
        {
            Check.ThatCode(() => FeigTransferResult.Timeout(null)).Throws<ArgumentNullException>();
        }
    }

    [TestFixture]
    public class CommunicationError
    {
        [Test]
        public void Success()
        {
            // act
            var request = new FeigRequest();
            var result = FeigTransferResult.CommunicationError(request);

            // assert
            Check.That(result.Status).IsEqualTo(FeigTransferStatus.CommunicationError);

            Check.That(result.Request).IsSameReferenceAs(request);

            Check.That(result.Response).IsNull();

            Check.That(result.ToString())
               .IsEqualTo(
                    "Status: CommunicationError, Request: { Address: 255, Command: None, Data: <empty> }, Response: { <null> }"
                );
        }

        [Test]
        public void Exception_With_NullRequest()
        {
            Check.ThatCode(() => FeigTransferResult.CommunicationError(null)).Throws<ArgumentNullException>();
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
            Check.That(result.Status).IsEqualTo(FeigTransferStatus.UnexpectedResponse);

            Check.That(result.Request).IsSameReferenceAs(request);

            Check.That(result.Response).IsSameReferenceAs(response);

            Check.That(result.ToString())
               .IsEqualTo(
                    "Status: UnexpectedResponse, Request: { Address: 255, Command: None, Data: <empty> }, Response: { Address: 0, Command: None, Status: OK, Data: <empty> }"
                );
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
