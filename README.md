## Introduction

This library is available as NuGet package:
[Amarok.InlayTester.Drivers.FeigReader](https://www.nuget.org/packages/Amarok.InlayTester.Drivers.FeigReader/) (*available soon*)

The library is compiled as *.NET Standard 2.0* library. Tests are performed with *.NET Framework 4.7.1* only. Currently, I have no plans to support other runtimes like .NET Core or older .NET Framework versions.

For development, you need *Visual Studio 2017* (v15.7 or later). For running the tests you need to install [com0com](https://sourceforge.net/projects/com0com/) and set up a serial port pair with names "COMA" and "COMB". This virtual serial port pair is used throughout unit tests.

## Documentation

### Create, Open, Close

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

So far, we only configured and created an instance of our reader in code. The specified serial port hasn't been opened yet. To open communication you have to call **Open()** on the reader. If necessary, you can **Close()** and re-open the communication transport multiple times on the same **IFeigReader** instance.

    using (IFeigReader reader = FeigReader.Create(settings))
    {
        reader.Open();      // now serial port is open
        reader.Close();     // serial port is closed again
        reader.Open();      // it's possible to open/close it multiple times
    }

Just, don't forget to dispose the **IFeigReader** at the end.


### Transfer

Now, to communicate with the reader/module, we perform a transfer operation, which sends a request to the reader and then waits for a response.

    var request = new FeigRequest {
        Address = 0xFF,
        Command = FeigCommand.CPUReset,
        Data = BufferSpan.Empty,
    };

    FeigTransferResult result = await reader.Transfer(request)
        .ConfigureAwait(false);

Here we first instantiate a **FeigRequest** and then call the async **Transfer(..)**. The method overload uses *protocol* and *timeout* from global settings supplied earlier during reader construction. To override global settings for this single transfer operation only, you can provide them as additional arguments. You can even provide a cancellation token to cancel the transfer operation at any time.

    FeigTransferResult result = await reader.Transfer(
            request, FeigProtocol.Standard, TimeSpan.FromMilliseconds(1000), cancellationToken)
        .ConfigureAwait(false);

Now, **Transfer(..)** never throws exceptions for timeout, cancellation or errors. Instead, it returns a specific result object that contains detailed information about the transfer operation. You can use it to determine whether the transfer operation succeeded as shown in the following code snippet.

    if (result.Status == FeigTransferStatus.Canceled)
    {
        // canceled; someone signals the cancellation token
    }
    else
    if (result.Status == FeigTransferStatus.Timeout)
    {
        // timeout, no response received
    }
    else
    if (result.Status == FeigTransferStatus.ChecksumError)
    {
        // corrupted response received; communication error
    }
    else
    if (result.Status == FeigTransferStatus.Success)
    {
        if (result.Response.Status != FeigStatus.OK)
        {
            // received response, but reader returned error
        }
        else
        {
            // received response; interpret data
            Console.WriteLine(result.Response.Data);
        }
    }

Error handling seems a bit tedious. But, you won't need to do it yourself. **Transfer(..)** represents a low-level method that gives you full control over transfer operations. In general, you will use other high-level methods better suited for most cases. It's just that you know you can do it that way.


### Execute

**Execute(..)** is one of those higher-level methods that does this result-code checking for you and that throws appropriate exceptions in certain error cases. On success, the method returns the received response.

    var request = new FeigRequest {
        Address = 0xFF,
        Command = FeigCommand.CPUReset,
        Data = BufferSpan.Empty,
    };

    var response = await reader.Execute(request)
        .ConfigureAwait(false);
        
    var data = response.Data;

In case of timeout, a **TimeoutException** is thrown. In case of cancellation an **OperationCanceledException**. Otherwise, if a communication error ocurred or the reader/module returned an error code, an exception of type **FeigException** is thrown. In all other cases, the received **FeigResponse** is returned.

Regarding the previous code snippet. There exists another overload that allows you to write this in a much shorter way.

    var response = await reader.Execute(FeigCommand.CPUReset)
        .ConfigureAwait(false);
        
This overload constructs the necessary **FeigRequest** internally.


### Common Commands

Most Feig RFID reader/modules support a set of common commands. These commands are already implemented and exposed as methods on the **IFeigReader** interface. For example, instead of sending a request with **FeigCommand.GetSoftwareVersion** and manually parse the returning bytes, invoke **GetSoftwareVersion(..)** instead.

Following commands are supported out-of-the-box:

| Feig Command              | Method on IFeigReader |
| ---                       | ---
| 0x52  Baud Rate Detection | TestCommunication()   |
| 0x63  CPU Reset           | ResetCPU()            |


*work in progress*

