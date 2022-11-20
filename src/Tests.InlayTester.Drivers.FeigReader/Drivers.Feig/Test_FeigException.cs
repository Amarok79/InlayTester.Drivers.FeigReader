// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


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
        Check.That(exception.Message).Contains("InlayTester.Drivers.Feig.FeigException");

        Check.That(exception.InnerException).IsNull();

        Check.That(exception.Request).IsSameReferenceAs(request);

        Check.That(exception.Response).IsSameReferenceAs(response);
    }

    [Test]
    public void Succeed_With_DefaultConstructor()
    {
        // act
        var exception = new FeigException();

        // assert
        Check.That(exception.Message).Contains("InlayTester.Drivers.Feig.FeigException");

        Check.That(exception.InnerException).IsNull();

        Check.That(exception.Request).IsNull();

        Check.That(exception.Response).IsNull();
    }

    [Test]
    public void Succeed_With_Message()
    {
        // act
        var exception = new FeigException("MSG");

        // assert
        Check.That(exception.Message).IsEqualTo("MSG");

        Check.That(exception.InnerException).IsNull();

        Check.That(exception.Request).IsNull();

        Check.That(exception.Response).IsNull();
    }

    [Test]
    public void Succeed_With_MessageRequestResponse()
    {
        // act
        var request = new FeigRequest();
        var response = new FeigResponse();
        var exception = new FeigException("MSG", request, response);

        // assert
        Check.That(exception.Message).IsEqualTo("MSG");

        Check.That(exception.InnerException).IsNull();

        Check.That(exception.Request).IsSameReferenceAs(request);

        Check.That(exception.Response).IsSameReferenceAs(response);
    }

    [Test]
    public void Succeed_With_MessageInnerException()
    {
        // act
        var innerException = new ApplicationException();
        var exception = new FeigException("MSG", innerException);

        // assert
        Check.That(exception.Message).IsEqualTo("MSG");

        Check.That(exception.InnerException).IsSameReferenceAs(innerException);

        Check.That(exception.Request).IsNull();

        Check.That(exception.Response).IsNull();
    }
}
