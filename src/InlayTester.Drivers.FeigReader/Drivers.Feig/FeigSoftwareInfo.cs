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
using System.Globalization;
using System.Text;
using Amarok.Contracts;
using Amarok.Shared;


namespace InlayTester.Drivers.Feig
{
	/// <summary>
	/// This type contains information about a Feig reader/module software.
	/// </summary>
	public sealed class FeigSoftwareInfo
	{
		/// <summary>
		/// The firmware version.
		/// </summary>
		public Version FirmwareVersion { get; set; }

		/// <summary>
		/// The hardware type.
		/// </summary>
		public Byte HardwareType { get; set; }

		/// <summary>
		/// The reader type.
		/// </summary>
		public FeigReaderType ReaderType { get; set; }

		/// <summary>
		/// The transponders supported by the firmware.
		/// </summary>
		public Int32 SupportedTransponders { get; set; }


		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public FeigSoftwareInfo()
		{
			this.FirmwareVersion = new Version(0, 0, 0);
			this.ReaderType = FeigReaderType.Unknown;
		}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		/// A null reference was passed to a method that did not accept it as a valid argument.</exception>
		public FeigSoftwareInfo(FeigSoftwareInfo info)
		{
			Verify.NotNull(info, nameof(info));

			this.FirmwareVersion = info.FirmwareVersion;
			this.HardwareType = info.HardwareType;
			this.ReaderType = info.ReaderType;
			this.SupportedTransponders = info.SupportedTransponders;
		}


		/// <summary>
		/// Returns a string that represents the current instance.
		/// </summary>
		public override String ToString()
		{
			StringBuilder sb = null;

			try
			{
				sb = StringBuilderPool.Rent();

				sb.Append("FirmwareVersion: ");
				sb.Append(this.FirmwareVersion);
				sb.Append(", HardwareType: 0x");
				sb.Append(this.HardwareType.ToString("X2", CultureInfo.InvariantCulture));
				sb.Append(", ReaderType: ");
				sb.Append(this.ReaderType);
				sb.Append(", SupportedTransponders: 0x");
				sb.Append(this.SupportedTransponders.ToString("X4", CultureInfo.InvariantCulture));

				return sb.ToString();
			}
			finally
			{
				StringBuilderPool.Free(sb);
			}
		}
	}
}
