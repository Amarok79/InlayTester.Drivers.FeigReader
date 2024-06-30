// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using Amarok.Shared;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Drivers.Feig;


[TestFixture]
public class Test_FeigTransponder
{
    [Test]
    public void Default()
    {
        var transponder = new FeigTransponder();

        Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.Unknown);

        Check.That(transponder.Identifier.IsEmpty).IsTrue();

        Check.That(transponder.ToString()).IsEqualTo("Type: Unknown, ID: <empty>");
    }

    [Test]
    public void Construction()
    {
        var transponder = new FeigTransponder {
            TransponderType = FeigTransponderType.ISO14443A,
            Identifier      = BufferSpan.From(0x11, 0x22, 0x33),
        };

        Check.That(transponder.TransponderType).IsEqualTo(FeigTransponderType.ISO14443A);

        Check.That(transponder.Identifier.ToArray()).ContainsExactly(0x11, 0x22, 0x33);

        Check.That(transponder.ToString()).IsEqualTo("Type: ISO14443A, ID: 11-22-33");
    }

    [Test]
    public void ToString_MultipleItems()
    {
        var transponders = new[] {
            new FeigTransponder {
                TransponderType = FeigTransponderType.ISO14443A,
                Identifier      = BufferSpan.From(0x11, 0x22, 0x33),
            },
            new FeigTransponder {
                TransponderType = FeigTransponderType.Jewel,
                Identifier      = BufferSpan.From(0x44, 0x55),
            },
        };

        Check.That(FeigTransponder.ToString(transponders))
            .IsEqualTo("{ Type: ISO14443A, ID: 11-22-33 }, { Type: Jewel, ID: 44-55 }");
    }

    [Test]
    public void ToString_SingleItems()
    {
        var transponders = new[] {
            new FeigTransponder {
                TransponderType = FeigTransponderType.ISO14443A,
                Identifier      = BufferSpan.From(0x11, 0x22, 0x33),
            },
        };

        Check.That(FeigTransponder.ToString(transponders)).IsEqualTo("{ Type: ISO14443A, ID: 11-22-33 }");
    }

    [Test]
    public void ToString_NoItems()
    {
        var transponders = Array.Empty<FeigTransponder>();

        Check.That(FeigTransponder.ToString(transponders)).IsEqualTo(String.Empty);
    }

    [Test]
    public void ToString_Null()
    {
        Check.That(FeigTransponder.ToString(null)).IsEqualTo(String.Empty);
    }
}
