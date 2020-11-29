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
using Amarok.Contracts;
using Common.Logging;
using Common.Logging.Simple;
using InlayTester.Shared.Transports;


namespace InlayTester.Drivers.Feig
{
    /// <summary>
    /// This class provides methods for creating <see cref="IFeigReader"/>.
    /// </summary>
    public static class FeigReader
    {
        /// <summary>
        /// Creates a <see cref="IFeigReader"/> based on the supplied settings.
        /// </summary>
        /// 
        /// <param name="settings">
        /// The settings used to create the reader.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// A null reference was passed to a method that did not accept it as a valid argument.</exception>
        public static IFeigReader Create(FeigReaderSettings settings)
        {
            Verify.NotNull(settings, nameof(settings));

            var copy      = new FeigReaderSettings(settings);
            var logger    = new NoOpLogger();
            var transport = new DefaultFeigTransport(copy.TransportSettings, logger);

            return new DefaultFeigReader(copy, transport, logger);
        }

        /// <summary>
        /// Creates a <see cref="IFeigReader"/> based on the supplied settings.
        /// </summary>
        /// 
        /// <param name="settings">
        /// The settings used to create the reader.</param>
        /// <param name="hooks">
        /// A hooks implementation being called for sent or received data.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// A null reference was passed to a method that did not accept it as a valid argument.</exception>
        public static IFeigReader Create(FeigReaderSettings settings, ITransportHooks hooks)
        {
            Verify.NotNull(settings, nameof(settings));
            Verify.NotNull(hooks, nameof(hooks));

            var copy      = new FeigReaderSettings(settings);
            var logger    = new NoOpLogger();
            var transport = new DefaultFeigTransport(copy.TransportSettings, logger, hooks);

            return new DefaultFeigReader(copy, transport, logger);
        }

        /// <summary>
        /// Creates a <see cref="IFeigReader"/> based on the supplied settings.
        /// </summary>
        /// 
        /// <param name="settings">
        /// The settings used to create the reader.</param>
        /// <param name="logger">
        /// The logger used to log operations of transport and reader.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// A null reference was passed to a method that did not accept it as a valid argument.</exception>
        public static IFeigReader Create(FeigReaderSettings settings, ILog logger)
        {
            Verify.NotNull(settings, nameof(settings));
            Verify.NotNull(logger, nameof(logger));

            var copy      = new FeigReaderSettings(settings);
            var transport = new DefaultFeigTransport(copy.TransportSettings, logger);

            return new DefaultFeigReader(copy, transport, logger);
        }

        /// <summary>
        /// Creates a <see cref="IFeigReader"/> based on the supplied settings.
        /// </summary>
        /// 
        /// <param name="settings">
        /// The settings used to create the reader.</param>
        /// <param name="logger">
        /// The logger used to log operations of transport and reader.</param>
        /// <param name="hooks">
        /// A hooks implementation being called for sent or received data.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// A null reference was passed to a method that did not accept it as a valid argument.</exception>
        public static IFeigReader Create(FeigReaderSettings settings, ILog logger, ITransportHooks hooks)
        {
            Verify.NotNull(settings, nameof(settings));
            Verify.NotNull(logger, nameof(logger));
            Verify.NotNull(hooks, nameof(hooks));

            var copy      = new FeigReaderSettings(settings);
            var transport = new DefaultFeigTransport(copy.TransportSettings, logger, hooks);

            return new DefaultFeigReader(copy, transport, logger);
        }
    }
}
