using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Server.TotpLib.Helper
{
    public static class Base32
    {
        private static readonly string allowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        internal static string Encode(string accountSecretKey)
        {
            var data = Encoding.UTF8.GetBytes(accountSecretKey);
            var output = "";
            for (var bitIndex = 0; bitIndex < data.Length * 8; bitIndex += 5)
            {
                var dualbyte = data[bitIndex / 8] << 8;
                if (bitIndex / 8 + 1 < data.Length)
                    dualbyte |= data[bitIndex / 8 + 1];
                dualbyte = 0x1f & dualbyte >> 16 - bitIndex % 8 - 5;
                output += allowedCharacters[dualbyte];
            }

            return output;
        }

        internal static string Decode(string base32)
        {
            var output = new List<byte>();
            var bytes = base32.ToCharArray();
            for (var bitIndex = 0; bitIndex < base32.Length * 5; bitIndex += 8)
            {
                var dualbyte = allowedCharacters.IndexOf(bytes[bitIndex / 5]) << 10;
                if (bitIndex / 5 + 1 < bytes.Length)
                    dualbyte |= allowedCharacters.IndexOf(bytes[bitIndex / 5 + 1]) << 5;
                if (bitIndex / 5 + 2 < bytes.Length)
                    dualbyte |= allowedCharacters.IndexOf(bytes[bitIndex / 5 + 2]);

                dualbyte = 0xff & dualbyte >> 15 - bitIndex % 5 - 8;
                output.Add((byte)dualbyte);
            }

            var key = Encoding.UTF8.GetString(output.ToArray());
            if (key.EndsWith("\0"))
            {
                var index = key.IndexOf("\0", StringComparison.Ordinal);
                //key = key.Replace("\0", "");
                key = key.Remove(index, 1);
            }

            return key;
        }

        public static string ToBase32(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            StringBuilder sb = new StringBuilder();
            for (int offset = 0; offset < input.Length;)
            {
                byte a, b, c, d, e, f, g, h;
                int numCharsToOutput = GetNextGroup(input, ref offset, out a, out b, out c, out d, out e, out f, out g, out h);

                sb.Append(numCharsToOutput >= 1 ? allowedCharacters[a] : '=');
                sb.Append(numCharsToOutput >= 2 ? allowedCharacters[b] : '=');
                sb.Append(numCharsToOutput >= 3 ? allowedCharacters[c] : '=');
                sb.Append(numCharsToOutput >= 4 ? allowedCharacters[d] : '=');
                sb.Append(numCharsToOutput >= 5 ? allowedCharacters[e] : '=');
                sb.Append(numCharsToOutput >= 6 ? allowedCharacters[f] : '=');
                sb.Append(numCharsToOutput >= 7 ? allowedCharacters[g] : '=');
                sb.Append(numCharsToOutput >= 8 ? allowedCharacters[h] : '=');
            }

            return sb.ToString();
        }

        // returns the number of bytes that were output
        private static int GetNextGroup(byte[] input, ref int offset, out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h)
        {
            uint b1, b2, b3, b4, b5;

            int retVal;
            switch (input.Length - offset)
            {
                case 1: retVal = 2; break;
                case 2: retVal = 4; break;
                case 3: retVal = 5; break;
                case 4: retVal = 7; break;
                default: retVal = 8; break;
            }

            b1 = offset < input.Length ? input[offset++] : 0U;
            b2 = offset < input.Length ? input[offset++] : 0U;
            b3 = offset < input.Length ? input[offset++] : 0U;
            b4 = offset < input.Length ? input[offset++] : 0U;
            b5 = offset < input.Length ? input[offset++] : 0U;

            a = (byte)(b1 >> 3);
            b = (byte)((b1 & 0x07) << 2 | b2 >> 6);
            c = (byte)(b2 >> 1 & 0x1f);
            d = (byte)((b2 & 0x01) << 4 | b3 >> 4);
            e = (byte)((b3 & 0x0f) << 1 | b4 >> 7);
            f = (byte)(b4 >> 2 & 0x1f);
            g = (byte)((b4 & 0x3) << 3 | b5 >> 5);
            h = (byte)(b5 & 0x1f);

            return retVal;
        }

        //public static string Encode(string accountSecretKey)
        //{
        //    var data = Encoding.UTF8.GetBytes(accountSecretKey);
        //    const int inByteSize = 8;
        //    const int outByteSize = 5;
        //    var alphabet = allowedCharacters.ToCharArray();

        //    int i = 0, index = 0;
        //    var result = new StringBuilder((data.Length + 7) * inByteSize / outByteSize);

        //    while (i < data.Length)
        //    {
        //        var currentByte = (data[i] >= 0) ? data[i] : (data[i] + 256);

        //        /* Is the current digit going to span a byte boundary? */
        //        var digit = 0;
        //        if (index > (inByteSize - outByteSize))
        //        {
        //            int nextByte;
        //            if ((i + 1) < data.Length)
        //                nextByte = (data[i + 1] >= 0) ? data[i + 1] : (data[i + 1] + 256);
        //            else
        //                nextByte = 0;

        //            digit = currentByte & (0xFF >> index);
        //            index = (index + outByteSize) % inByteSize;
        //            digit <<= index;
        //            digit |= nextByte >> (inByteSize - index);
        //            i++;
        //        }
        //        else
        //        {
        //            digit = (currentByte >> (inByteSize - (index + outByteSize))) & 0x1F;
        //            index = (index + outByteSize) % inByteSize;
        //            if (index == 0)
        //                i++;
        //        }
        //        result.Append(alphabet[digit]);
        //    }

        //    return result.ToString();
        //}
    }
}