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


namespace InlayTester.Drivers.Feig
{
	/// <summary>
	/// This enumeration lists supported commands.
	/// </summary>
	public enum FeigCommand : Byte
	{
		/// <summary>
		/// 00h None
		/// </summary>
		None = 0x00,

		/// <summary>
		/// 52h Baud Rate Detection
		/// </summary>
		BaudRateDetection = 0x52,

		/// <summary>
		/// 55h Start Flash Loader
		/// </summary>
		StartFlashLoader = 0x55,

		/// <summary>
		/// 63h CPU Reset
		/// </summary>
		CPUReset = 0x63,

		/// <summary>
		/// 65h Get Software Version
		/// </summary>
		GetSoftwareVersion = 0x65,

		/// <summary>
		/// 66h Get Reader Info
		/// </summary>
		GetReaderInfo = 0x66,

		/// <summary>
		/// 69h RF Reset
		/// </summary>
		RFReset = 0x69,

		/// <summary>
		/// 6Ah RF Output On/Off
		/// </summary>
		RFOutputOnOff = 0x6A,

		/// <summary>
		/// 72h Set Output
		/// </summary>
		SetOutput = 0x72,

		/// <summary>
		/// A0h Reader Login
		/// </summary>
		ReaderLogin = 0xA0,

		/// <summary>
		/// 80h Read Configuration
		/// </summary>
		ReadConfiguration = 0x80,

		/// <summary>
		/// 81h Write Configuration
		/// </summary>
		WriteConfiguration = 0x81,

		/// <summary>
		/// 82h Save Configuration
		/// </summary>
		SaveConfiguration = 0x82,

		/// <summary>
		/// 83h Set Default Configuration
		/// </summary>
		SetDefaultConfiguration = 0x83,

		/// <summary>
		/// A2h Write Mifare Reader Keys
		/// </summary>
		WriteMifareReaderKeys = 0xA2,

		/// <summary>
		/// B0h ISO Standard Host Command
		/// </summary>
		ISOStandardHostCommand = 0xB0,

		/// <summary>
		/// B2h ISO14443 Special Host Command
		/// </summary>
		ISO14443SpecialHostCommand = 0xB2,

		/// <summary>
		/// BDh ISO14443A Transparent Command
		/// </summary>
		ISO14443ATransparentCommand = 0xBD,

		/// <summary>
		/// BEh ISO14443B Transparent Command
		/// </summary>
		ISO14443BTransparentCommand = 0xBE,

		/// <summary>
		/// BCh Command Queue
		/// </summary>
		CommandQueue = 0xBC,
	}
}
