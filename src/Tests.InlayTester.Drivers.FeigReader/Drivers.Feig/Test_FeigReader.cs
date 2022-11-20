// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using InlayTester.Shared.Transports;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


public class Test_FeigReader
{
    [TestFixture]
    public class Create_Settings
    {
        [Test]
        public void Success()
        {
            // act
            var settings = new FeigReaderSettings();
            var reader = FeigReader.Create(settings);

            // assert
            Check.That(reader).IsInstanceOf<DefaultFeigReader>();

            var readerImpl = (DefaultFeigReader)reader;

            Check.That(readerImpl.Settings).Not.IsSameReferenceAs(settings);

            Check.That(readerImpl.Logger).IsEqualTo(NullLogger.Instance);

            Check.That(readerImpl.Transport).IsInstanceOf<DefaultFeigTransport>();

            var transportImpl = (DefaultFeigTransport)readerImpl.Transport;

            Check.That(transportImpl.Settings).Not.IsSameReferenceAs(settings.TransportSettings);

            Check.That(transportImpl.Logger).IsEqualTo(NullLogger.Instance).And.IsSameReferenceAs(readerImpl.Logger);
        }

        [Test]
        public void Exception_For_NullSettings()
        {
            Check.ThatCode(() => FeigReader.Create(null)).Throws<ArgumentNullException>();
        }
    }

    [TestFixture]
    public class Create_SettingsHooks
    {
        [Test]
        public void Success()
        {
            // act
            var hooks = new Mock<ITransportHooks>();
            var settings = new FeigReaderSettings();
            var reader = FeigReader.Create(settings, hooks.Object);

            // assert
            Check.That(reader).IsInstanceOf<DefaultFeigReader>();

            var readerImpl = (DefaultFeigReader)reader;

            Check.That(readerImpl.Settings).Not.IsSameReferenceAs(settings);

            Check.That(readerImpl.Logger).IsEqualTo(NullLogger.Instance);

            Check.That(readerImpl.Transport).IsInstanceOf<DefaultFeigTransport>();

            var transportImpl = (DefaultFeigTransport)readerImpl.Transport;

            Check.That(transportImpl.Settings).Not.IsSameReferenceAs(settings.TransportSettings);

            Check.That(transportImpl.Logger).IsEqualTo(NullLogger.Instance).And.IsSameReferenceAs(readerImpl.Logger);
        }

        [Test]
        public void Exception_For_NullSettings()
        {
            var hooks = new Mock<ITransportHooks>();

            Check.ThatCode(() => FeigReader.Create(null, hooks.Object)).Throws<ArgumentNullException>();
        }

        [Test]
        public void Exception_For_NullHooks()
        {
            var settings = new FeigReaderSettings();

            Check.ThatCode(() => FeigReader.Create(settings, (ITransportHooks)null)).Throws<ArgumentNullException>();
        }
    }

    [TestFixture]
    public class Create_SettingsLogger
    {
        [Test]
        public void Success()
        {
            // act
            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = FeigReader.Create(settings, logger);

            // assert
            Check.That(reader).IsInstanceOf<DefaultFeigReader>();

            var readerImpl = (DefaultFeigReader)reader;

            Check.That(readerImpl.Settings).Not.IsSameReferenceAs(settings);

            Check.That(readerImpl.Logger).IsSameReferenceAs(logger);

            Check.That(readerImpl.Transport).IsInstanceOf<DefaultFeigTransport>();

            var transportImpl = (DefaultFeigTransport)readerImpl.Transport;

            Check.That(transportImpl.Settings).Not.IsSameReferenceAs(settings.TransportSettings);

            Check.That(transportImpl.Logger).IsSameReferenceAs(logger).And.IsSameReferenceAs(readerImpl.Logger);
        }

        [Test]
        public void Exception_For_NullSettings()
        {
            Check.ThatCode(() => FeigReader.Create(null, NullLogger.Instance)).Throws<ArgumentNullException>();
        }

        [Test]
        public void Exception_For_NullLogger()
        {
            Check.ThatCode(() => FeigReader.Create(new FeigReaderSettings(), (ILogger)null))
               .Throws<ArgumentNullException>();
        }
    }

    [TestFixture]
    public class Create_SettingsLoggerHooks
    {
        [Test]
        public void Success()
        {
            // act
            var hooks = new Mock<ITransportHooks>();
            var settings = new FeigReaderSettings();

            var logger = LoggerFactory.Create(
                    builder => {
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddSimpleConsole();
                    }
                )
               .CreateLogger("Test");

            var reader = FeigReader.Create(settings, logger, hooks.Object);

            // assert
            Check.That(reader).IsInstanceOf<DefaultFeigReader>();

            var readerImpl = (DefaultFeigReader)reader;

            Check.That(readerImpl.Settings).Not.IsSameReferenceAs(settings);

            Check.That(readerImpl.Logger).IsSameReferenceAs(logger);

            Check.That(readerImpl.Transport).IsInstanceOf<DefaultFeigTransport>();

            var transportImpl = (DefaultFeigTransport)readerImpl.Transport;

            Check.That(transportImpl.Settings).Not.IsSameReferenceAs(settings.TransportSettings);

            Check.That(transportImpl.Logger).IsSameReferenceAs(logger).And.IsSameReferenceAs(readerImpl.Logger);
        }

        [Test]
        public void Exception_For_NullSettings()
        {
            Check.ThatCode(() => FeigReader.Create(null, NullLogger.Instance)).Throws<ArgumentNullException>();
        }

        [Test]
        public void Exception_For_NullLogger()
        {
            Check.ThatCode(() => FeigReader.Create(new FeigReaderSettings(), (ILogger)null))
               .Throws<ArgumentNullException>();
        }

        [Test]
        public void Exception_For_NullHooks()
        {
            Check.ThatCode(() => FeigReader.Create(new FeigReaderSettings(), NullLogger.Instance, null))
               .Throws<ArgumentNullException>();
        }
    }
}
