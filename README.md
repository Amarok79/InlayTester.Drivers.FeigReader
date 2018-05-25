### Introduction

This library is available as NuGet package:
[Amarok.InlayTester.Drivers.FeigReader](https://www.nuget.org/packages/Amarok.InlayTester.Drivers.FeigReader/)

The library is compiled as *.NET Standard 2.0* library. Tests are performed with *.NET Framework 4.7.1* only. Currently, I have no plans to support other runtimes like .NET Core or older .NET Framework versions.

For development, you need *Visual Studio 2017* (v15.7 or later). For running the tests you need to install [com0com](https://sourceforge.net/projects/com0com/) and set up a serial port pair with names "COMA" and "COMB". This virtual serial port pair is used throughout unit tests.

### Documentation

#### IFeigReader

To communicate with a Feig RFID reader/module connected via RS232, you first have to instantiate a **IFeigReader**. This is done via factory method **FeigReader.Create(..)**, which requires an instance of **FeigReaderSettings**.

    var settings = new FeigReaderSettings {
        TransportSettings = new SerialTransportSettings {
                PortName = "COM4",
                Baud = 38400,
                DataBits = 8,
                Parity = Parity.Even,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
        },
        Address = 255,
        Protocol = FeigProtocol.Advanced,
        Timeout = TimeSpan.FromMilliseconds(500),
    };
    
    using (IFeigReader reader = FeigReader.Create(settings))
    {
        // ...
    }
These settings are used to provide configuration about serial communication (**SerialTransportSettings**) to the reader, but also to provide Feig-specific global settings like **Address**, **Protocol** and **Timeout**.

So far, we only configured and created an instance of our reader in code. The specified serial port hasn't been opened yet. To open communication you have to call **Open()** on the reader. If needed, you can **Close()** and re-open the communication transport multiple times on the same **IFeigReader** instance.

    using (IFeigReader reader = FeigReader.Create(settings))
    {
        reader.Open();      // now serial port is open
        reader.Close();     // serial port is closed again
        reader.Open();      // it's possible to open/close it multiple times
    }

Just, don't forget to dispose the **IFeigReader** at the end.










