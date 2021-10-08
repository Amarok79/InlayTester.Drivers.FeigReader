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
using Amarok.Contracts;
using Amarok.Shared;


namespace InlayTester.Shared
{
    /// <summary>
    ///     This class implements a CRC calculator that is configurable.
    /// </summary>
    internal sealed class CrcCalculator
    {
        // settings
        private readonly Int32 mOrder;
        private readonly UInt64 mPolynom;
        private readonly Boolean mIsDirect;
        private readonly UInt64 mCrcInit;
        private readonly UInt64 mCrcXor;
        private readonly Boolean mReflectInput;
        private readonly Boolean mReflectOutput;

        // data
        private UInt64 mCrcMask;
        private UInt64 mCrcHighBit;
        private UInt64 mCrcInitDirect;
        private readonly UInt64[] mCrcTable = new UInt64[256];


        #region ++ Public Interface ++

        /// <summary>
        ///     Initializes a new instance.
        /// </summary>
        /// 
        /// <param name="order">
        ///     The CRC polynom order, counted without the leading '1' bit. A value between 1 and 32.
        /// </param>
        /// <param name="polynom">
        ///     Polynom without leading '1' bit.
        /// </param>
        /// <param name="isDirect">
        ///     true = direct, no augmented zero bits
        /// </param>
        /// <param name="crcInit">
        ///     The initial CRC value belonging to that algorithm.
        /// </param>
        /// <param name="crcXor">
        ///     The final XOR value.
        /// </param>
        /// <param name="reflectInput">
        ///     Specifies if a data byte is reflected before processing (UART) or not.
        /// </param>
        /// <param name="reflectOutput">
        ///     Specifies if the CRC will be reflected before XOR.
        /// </param>
        public CrcCalculator(
            Int32 order,
            Int32 polynom,
            Boolean isDirect,
            Int32 crcInit,
            Int32 crcXor,
            Boolean reflectInput,
            Boolean reflectOutput
        )
        {
            Verify.IsInRange(order, 1, 32, nameof(order));

            mOrder         = order;
            mPolynom       = (UInt64) polynom;
            mIsDirect      = isDirect;
            mCrcInit       = (UInt64) crcInit;
            mCrcXor        = (UInt64) crcXor;
            mReflectInput  = reflectInput;
            mReflectOutput = reflectOutput;

            _Setup();
        }

        /// <summary>
        ///     Calculates the CRC value for the given buffer.
        /// </summary>
        /// 
        /// <returns>
        ///     The calculated checksum for the supplied buffer fragment.
        /// </returns>
        public Int32 Calculate(in BufferSpan data)
        {
            return (Int32) _Calculate(data.Buffer, data.Offset, data.Count);
        }

        #endregion

        #region Implementation

        private void _Setup()
        {
            mCrcMask    = ( ( ( (UInt64) 1 << ( mOrder - 1 ) ) - 1 ) << 1 ) | 1;
            mCrcHighBit = (UInt64) 1 << ( mOrder - 1 );

            _BuildTable();
            _Prepare();
        }

        private void _Prepare()
        {
            UInt64 bit;
            UInt64 crc;
            Int32  i;

            if (!mIsDirect)
            {
                crc = mCrcInit;

                for (i = 0; i < mOrder; i++)
                {
                    bit =   crc & mCrcHighBit;
                    crc <<= 1;

                    if (bit != 0)
                        crc ^= mPolynom;
                }

                crc            &= mCrcMask;
                mCrcInitDirect =  crc;
            }
            else
            {
                mCrcInitDirect = mCrcInit;
                crc            = mCrcInit;

                for (i = 0; i < mOrder; i++)
                {
                    bit = crc & 1;

                    if (bit != 0)
                        crc ^= mPolynom;

                    crc >>= 1;

                    if (bit != 0)
                        crc |= mCrcHighBit;
                }
            }
        }

        private void _BuildTable()
        {
            for (var i = 0; i < 256; i++)
            {
                var crc = (UInt64) i;

                if (mReflectInput)
                    crc = _Reflect(crc, 8);

                crc <<= mOrder - 8;

                for (var j = 0; j < 8; j++)
                {
                    var bit = crc & mCrcHighBit;
                    crc <<= 1;

                    if (bit != 0)
                        crc ^= mPolynom;
                }

                if (mReflectInput)
                    crc = _Reflect(crc, mOrder);

                crc          &= mCrcMask;
                mCrcTable[i] =  crc;
            }
        }

        private static UInt64 _Reflect(UInt64 crc, Int32 bitnum)
        {
            // reflects the lower 'bitnum' bits of 'crc'
            UInt64 j      = 1;
            UInt64 crcout = 0;

            for (var i = (UInt64) 1 << ( bitnum - 1 ); i != 0; i >>= 1)
            {
                if (( crc & i ) != 0)
                    crcout |= j;

                j <<= 1;
            }

            return crcout;
        }

        private UInt64 _Calculate(Byte[] buffer, Int32 offset, Int32 count)
        {
            unchecked
            {
                // fast lookup table algorithm without augmented zero bytes, e.g. used in pkzip.
                // only usable with mPolynom mOrders of 8, 16, 24 or 32.

                var crc = mCrcInitDirect;

                if (!mReflectInput)
                {
                    for (var i = offset; i < offset + count; i++)
                        crc = ( crc << 8 ) ^ mCrcTable[( ( crc >> ( mOrder - 8 ) ) & 0xff ) ^ buffer[i]];
                }
                else
                {
                    crc = _Reflect(crc, mOrder);

                    for (var i = offset; i < offset + count; i++)
                        crc = ( crc >> 8 ) ^ mCrcTable[( crc & 0xff ) ^ buffer[i]];
                }

                if (mReflectOutput ^ mReflectInput)
                    crc = _Reflect(crc, mOrder);

                crc ^= mCrcXor;
                crc &= mCrcMask;

                return crc;
            }
        }

        #endregion
    }
}
