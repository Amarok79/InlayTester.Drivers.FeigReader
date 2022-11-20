// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using Amarok.Shared;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_FeigRequest
{
    [Test]
    public void Defaults()
    {
        // act
        var req = new FeigRequest();

        // assert
        Check.That(req.Address).IsEqualTo(0xff);

        Check.That(req.Command).IsEqualTo(FeigCommand.None);

        Check.That(req.Data.IsEmpty).IsTrue();

        Check.That(req.ToString()).IsEqualTo("Address: 255, Command: None, Data: <empty>");
    }

    [Test]
    public void StandardFrame()
    {
        // act
        var req = new FeigRequest {
            Address = 0x12,
            Command = FeigCommand.ReadConfiguration,
            Data = BufferSpan.From(0x07),
        };

        // assert
        Check.That(req.Address).IsEqualTo(0x12);

        Check.That(req.Command).IsEqualTo(FeigCommand.ReadConfiguration);

        Check.That(req.Data.Buffer).ContainsExactly(0x07);

        // act
        var frame = req.ToBufferSpan();

        // assert
        Check.That(frame.ToArray()).ContainsExactly(0x06, 0x12, 0x80, 0x07, 0xe5, 0x80);

        Check.That(req.ToString()).IsEqualTo("Address: 18, Command: ReadConfiguration, Data: 07");
    }

    [Test]
    public void StandardFrame_GetSoftwareVersion()
    {
        // act
        var req = new FeigRequest { Command = FeigCommand.GetSoftwareVersion };

        // assert
        Check.That(req.Address).IsEqualTo(0xFF);

        Check.That(req.Command).IsEqualTo(FeigCommand.GetSoftwareVersion);

        Check.That(req.Data.IsEmpty).IsTrue();

        // act
        var frame = req.ToBufferSpan();

        // assert
        Check.That(frame.ToArray()).ContainsExactly(0x05, 0xFF, 0x65, 0xE5, 0xCB);

        Check.That(req.ToString()).IsEqualTo("Address: 255, Command: GetSoftwareVersion, Data: <empty>");
    }

    [Test]
    public void AdvancedFrame()
    {
        // act
        var req = new FeigRequest {
            Address = 0x12,
            Command = FeigCommand.ReadConfiguration,
            Data = BufferSpan.From(0x07),
        };

        // assert
        Check.That(req.Address).IsEqualTo(0x12);

        Check.That(req.Command).IsEqualTo(FeigCommand.ReadConfiguration);

        Check.That(req.Data.Buffer).ContainsExactly(0x07);

        // act
        var frame = req.ToBufferSpan(FeigProtocol.Advanced);

        // assert
        Check.That(frame.ToArray()).ContainsExactly(0x02, 0x00, 0x08, 0x12, 0x80, 0x07, 0xA0, 0x2D);

        Check.That(req.ToString()).IsEqualTo("Address: 18, Command: ReadConfiguration, Data: 07");
    }

    [Test]
    public void AdvancedFrame_GetSoftwareVersion()
    {
        // act
        var req = new FeigRequest { Command = FeigCommand.GetSoftwareVersion };

        // assert
        Check.That(req.Address).IsEqualTo(0xFF);

        Check.That(req.Command).IsEqualTo(FeigCommand.GetSoftwareVersion);

        Check.That(req.Data.IsEmpty).IsTrue();

        // act
        var frame = req.ToBufferSpan(FeigProtocol.Advanced);

        // assert
        Check.That(frame.ToArray()).ContainsExactly(0x02, 0x00, 0x07, 0xFF, 0x65, 0x6E, 0x61);

        Check.That(req.ToString()).IsEqualTo("Address: 255, Command: GetSoftwareVersion, Data: <empty>");
    }
}
