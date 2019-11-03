/* MIT License
 * 
 * Copyright (c) 2019, Olaf Kober
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
using System.Collections.Generic;
using System.Text;
using Amarok.Shared;


namespace InlayTester.Drivers.Feig
{
	/// <summary>
	/// This type represents a RFID transponder consisting of type and identifier.
	/// </summary>
	public sealed class FeigTransponder
	{
		/// <summary>
		/// The type of the transponder.
		/// </summary>
		public FeigTransponderType TransponderType { get; set; } = FeigTransponderType.Unknown;

		/// <summary>
		/// The identifier of the transponder.
		/// </summary>
		public BufferSpan Identifier { get; set; }


		/// <summary>
		/// Returns a string that represents the current instance.
		/// </summary>
		public override String ToString()
		{
			StringBuilder? sb = null;

			try
			{
				sb = StringBuilderPool.Rent();

				sb.Append("Type: ");
				sb.Append(this.TransponderType);
				sb.Append(", ID: ");
				sb.Append(this.Identifier);

				return sb.ToString();
			}
			finally
			{
				StringBuilderPool.Free(sb);
			}
		}


		/// <summary>
		/// Returns a string representation for the given transponders.
		/// </summary>
		public static String ToString(IEnumerable<FeigTransponder> transponders)
		{
			StringBuilder? sb = null;

			try
			{
				sb = StringBuilderPool.Rent();

				if (transponders != null)
				{
					var i = 0;
					foreach (var item in transponders)
					{
						if (i++ > 0)
							sb.Append(", ");

						sb.Append("{ ");
						sb.Append(item);
						sb.Append(" }");
					}
				}

				return sb.ToString();
			}
			finally
			{
				StringBuilderPool.Free(sb);
			}
		}
	}
}
