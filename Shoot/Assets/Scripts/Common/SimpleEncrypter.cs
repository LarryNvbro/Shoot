using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SimpleEncrypter
{
    public static void Swap(ref byte lhs, ref byte rhs)
    {
        byte temp = lhs;
        lhs = rhs;
        rhs = temp;
    }

    public static void Encryption(ref byte[] buf)
    {
        Random rand = new Random();
        byte key = (byte)rand.Next(255);
        int cnt = (buf.Length - 2) / 2;
        for (int i = 0; i < cnt; ++i)
        {
            buf[i] ^= key;
            buf[cnt + i] ^= key;
            Swap(ref buf[i], ref buf[cnt + i]);
        }

        buf[buf.Length - 1] = key;
        buf[buf.Length - 1] ^= Convert.ToByte(buf.Length % 255);
    }

    public static void Descryption(ref byte[] buf)
    {
        byte key = buf[buf.Length - 1];
        key ^= Convert.ToByte(buf.Length % 255);
        int cnt = (buf.Length - 2) / 2;
        for (int i = 0; i < cnt; ++i)
        {
            buf[i] ^= key;
            buf[cnt + i] ^= key;
            Swap(ref buf[i], ref buf[cnt + i]);
        }
    }
}
