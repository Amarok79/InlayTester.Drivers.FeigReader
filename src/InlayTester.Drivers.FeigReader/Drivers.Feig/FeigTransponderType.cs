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


namespace InlayTester.Drivers.Feig
{
    /// <summary>
    /// This enumeration lists well-known transponder types.
    /// </summary>
    public enum FeigTransponderType : Byte
    {
        /// <summary>
        /// 00h  I-Code1
        /// </summary>
        ICode1 = 0x00,

        /// <summary>
        /// 01h  TagIt
        /// </summary>
        TagIt = 0x01,

        /// <summary>
        /// 03h  ISO15693
        /// </summary>
        ISO15693 = 0x03,

        /// <summary>
        /// 04h  ISO14443A
        /// </summary>
        ISO14443A = 0x04,

        /// <summary>
        /// 05h  ISO14443B
        /// </summary>
        ISO14443B = 0x05,

        /// <summary>
        /// 06h  I-CodeEPC
        /// </summary>
        ICodeEPC = 0x06,

        /// <summary>
        /// 07h  I-CodeUID
        /// </summary>
        ICodeUID = 0x07,

        /// <summary>
        /// 08h  Jewel
        /// </summary>
        Jewel = 0x08,

        /// <summary>
        /// 09h  ISO18000-3M3
        /// </summary>
        ISO18000_3M3 = 0x09,

        /// <summary>
        /// 0Ah  SR176
        /// </summary>
        SR176 = 0x0A,

        /// <summary>
        /// 0Bh  SRIxx (SRI512, SRIX512, SRI4K, SRIX4K)
        /// </summary>
        SRIxx = 0x0B,

        /// <summary>
        /// 0Ch  MCRFxxx
        /// </summary>
        MCRFxxx = 0xC,

        /// <summary>
        /// 0Dh  FeliCa
        /// </summary>
        FeliCa = 0x0D,

        /// <summary>
        /// 10h  Innovatron (14443-B’)
        /// </summary>
        Innovatron = 0x10,

        /// <summary>
        /// 11h  ASK CTx
        /// </summary>
        ASK_CTx = 0x11,

        /// <summary>
        /// 80h  ISO18000-6-A
        /// </summary>
        ISO18000_6A = 0x80,

        /// <summary>
        /// 81h  ISO18000-6-B
        /// </summary>
        ISO18000_6B = 0x81,

        /// <summary>
        /// 83h  EM4222
        /// </summary>
        EM4222 = 0x83,

        /// <summary>
        /// 84h  EPC Class1 Gen2
        /// </summary>
        EPC_Class1_Gen2 = 0x84,

        /// <summary>
        /// 88h  EPC Class0
        /// </summary>
        EPC_Class0 = 0x88,

        /// <summary>
        /// 89h  EPC Class1 Gen1
        /// </summary>
        EPC_Class1_Gen1 = 0x89,


        /// <summary>
        /// FFh  Unknown Transponder Type
        /// </summary>
        Unknown = 0xFF,
    }
}
