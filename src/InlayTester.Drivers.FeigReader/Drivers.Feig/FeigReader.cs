// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

using System;
using Amarok.Contracts;
using InlayTester.Shared.Transports;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This class provides methods for creating <see cref="IFeigReader"/>.
/// </summary>
public static class FeigReader
{
    /// <summary>
    ///     Creates a <see cref="IFeigReader"/> based on the supplied settings.
    /// </summary>
    /// 
    /// <param name="settings">
    ///     The settings used to create the reader.
    /// </param>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static IFeigReader Create(FeigReaderSettings settings)
    {
        Verify.NotNull(settings, nameof(settings));

        var copy      = new FeigReaderSettings(settings);
        var logger    = NullLogger.Instance;
        var transport = new DefaultFeigTransport(copy.TransportSettings, logger);

        return new DefaultFeigReader(copy, transport, logger);
    }

    /// <summary>
    ///     Creates a <see cref="IFeigReader"/> based on the supplied settings.
    /// </summary>
    /// 
    /// <param name="settings">
    ///     The settings used to create the reader.
    /// </param>
    /// <param name="hooks">
    ///     A hooks implementation being called for sent or received data.
    /// </param>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static IFeigReader Create(FeigReaderSettings settings, ITransportHooks hooks)
    {
        Verify.NotNull(settings, nameof(settings));
        Verify.NotNull(hooks, nameof(hooks));

        var copy      = new FeigReaderSettings(settings);
        var logger    = NullLogger.Instance;
        var transport = new DefaultFeigTransport(copy.TransportSettings, logger, hooks);

        return new DefaultFeigReader(copy, transport, logger);
    }

    /// <summary>
    ///     Creates a <see cref="IFeigReader"/> based on the supplied settings.
    /// </summary>
    /// 
    /// <param name="settings">
    ///     The settings used to create the reader.
    /// </param>
    /// <param name="logger">
    ///     The logger used to log operations of transport and reader.
    /// </param>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static IFeigReader Create(FeigReaderSettings settings, ILogger logger)
    {
        Verify.NotNull(settings, nameof(settings));
        Verify.NotNull(logger, nameof(logger));

        var copy      = new FeigReaderSettings(settings);
        var transport = new DefaultFeigTransport(copy.TransportSettings, logger);

        return new DefaultFeigReader(copy, transport, logger);
    }

    /// <summary>
    ///     Creates a <see cref="IFeigReader"/> based on the supplied settings.
    /// </summary>
    /// 
    /// <param name="settings">
    ///     The settings used to create the reader.
    /// </param>
    /// <param name="logger">
    ///     The logger used to log operations of transport and reader.
    /// </param>
    /// <param name="hooks">
    ///     A hooks implementation being called for sent or received data.
    /// </param>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public static IFeigReader Create(
        FeigReaderSettings settings,
        ILogger logger,
        ITransportHooks hooks
    )
    {
        Verify.NotNull(settings, nameof(settings));
        Verify.NotNull(logger, nameof(logger));
        Verify.NotNull(hooks, nameof(hooks));

        var copy      = new FeigReaderSettings(settings);
        var transport = new DefaultFeigTransport(copy.TransportSettings, logger, hooks);

        return new DefaultFeigReader(copy, transport, logger);
    }
}
