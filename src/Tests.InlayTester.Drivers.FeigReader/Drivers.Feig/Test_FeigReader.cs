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
using Common.Logging;
using Common.Logging.Simple;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig
{
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
				Check.That(reader)
					.IsInstanceOf<DefaultFeigReader>();

				var readerImpl = (DefaultFeigReader)reader;

				Check.That(readerImpl.Settings)
					.Not.IsSameReferenceAs(settings);
				Check.That(readerImpl.Logger)
					.IsInstanceOf<NoOpLogger>();
				Check.That(readerImpl.Transport)
					.IsInstanceOf<DefaultFeigTransport>();

				var transportImpl = (DefaultFeigTransport)readerImpl.Transport;

				Check.That(transportImpl.Settings)
					.Not.IsSameReferenceAs(settings.TransportSettings);
				Check.That(transportImpl.Logger)
					.IsInstanceOf<NoOpLogger>()
					.And
					.IsSameReferenceAs(readerImpl.Logger);
			}

			[Test]
			public void Exception_For_NullSettings()
			{
				Check.ThatCode(() => FeigReader.Create(null))
					.Throws<ArgumentNullException>();
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
				var logger = new ConsoleOutLogger("A", LogLevel.Info, false, false, false, "G");
				var reader = FeigReader.Create(settings, logger);

				// assert
				Check.That(reader)
					.IsInstanceOf<DefaultFeigReader>();

				var readerImpl = (DefaultFeigReader)reader;

				Check.That(readerImpl.Settings)
					.Not.IsSameReferenceAs(settings);
				Check.That(readerImpl.Logger)
					.IsInstanceOf<ConsoleOutLogger>();
				Check.That(readerImpl.Transport)
					.IsInstanceOf<DefaultFeigTransport>();

				var transportImpl = (DefaultFeigTransport)readerImpl.Transport;

				Check.That(transportImpl.Settings)
					.Not.IsSameReferenceAs(settings.TransportSettings);
				Check.That(transportImpl.Logger)
					.IsInstanceOf<ConsoleOutLogger>()
					.And
					.IsSameReferenceAs(readerImpl.Logger);
			}

			[Test]
			public void Exception_For_NullSettings()
			{
				Check.ThatCode(() => FeigReader.Create(null, new NoOpLogger()))
					.Throws<ArgumentNullException>();
			}

			[Test]
			public void Exception_For_NullLogger()
			{
				Check.ThatCode(() => FeigReader.Create(new FeigReaderSettings(), null))
					.Throws<ArgumentNullException>();
			}
		}
	}
}
