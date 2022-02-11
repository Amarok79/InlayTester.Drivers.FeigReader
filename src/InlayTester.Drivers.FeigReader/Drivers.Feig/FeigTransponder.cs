// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.Collections.Generic;
using System.Text;
using Amarok.Shared;


namespace InlayTester.Drivers.Feig;


/// <summary>
///     This type represents a RFID transponder consisting of type and identifier.
/// </summary>
public sealed class FeigTransponder
{
    /// <summary>
    ///     The type of the transponder.
    /// </summary>
    public FeigTransponderType TransponderType { get; set; } = FeigTransponderType.Unknown;

    /// <summary>
    ///     The identifier of the transponder.
    /// </summary>
    public BufferSpan Identifier { get; set; }


    /// <summary>
    ///     Returns a string that represents the current instance.
    /// </summary>
    public override String ToString()
    {
        StringBuilder? sb = null;

        try
        {
            sb = StringBuilderPool.Rent();

            sb.Append("Type: ");
            sb.Append(TransponderType);
            sb.Append(", ID: ");
            sb.Append(Identifier);

            return sb.ToString();
        }
        finally
        {
            StringBuilderPool.Free(sb);
        }
    }


    /// <summary>
    ///     Returns a string representation for the given transponders.
    /// </summary>
    public static String ToString(IEnumerable<FeigTransponder> transponders)
    {
        if (transponders == null)
        {
            return String.Empty;
        }

        StringBuilder? sb = null;

        try
        {
            sb = StringBuilderPool.Rent();

            var i = 0;

            foreach (var item in transponders)
            {
                if (i++ > 0)
                {
                    sb.Append(", ");
                }

                sb.Append("{ ");
                sb.Append(item);
                sb.Append(" }");
            }

            return sb.ToString();
        }
        finally
        {
            StringBuilderPool.Free(sb);
        }
    }
}
