using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ByteConvert
{
    class Program
    {
        static void Main(string[] args)
        {
            UInt32 a = 1879048193;
            byte[] bytes = BitConverter.GetBytes(a);
            Console.WriteLine(BitConverter.ToString(bytes));
            Array.Reverse(bytes);
            string result2 = BitConverter.ToString(bytes).Replace("-","");
            Console.WriteLine(result2);
            BitArray ba = ConvertHexToBitArray(result2);
            foreach (var cccd in ba)
            {
                Console.Write(cccd);
            }
            Console.WriteLine();
            UInt32 result = BitConverter.ToUInt32(bytes,0);
            Console.WriteLine(result);
            Console.WriteLine(BitConverter.ToString(BitConverter.GetBytes(result)));
            Console.WriteLine(result.ToString("X8"));
            string b = Convert.ToString(result, 2).PadLeft(32, '0');
            Console.WriteLine(b);
            var c = b.Select(s => s.Equals('1')).ToArray(); ;
            for (int i = 0; i < c.Length; i++)
            {
                Console.Write(c[i]+" ");
            }
            Console.WriteLine("====");
            Console.ReadLine();
            byte cc = 78;
            Console.WriteLine(cc.ToString("X"));
            string bb = Convert.ToString(cc, 2).PadLeft(8, '0');
            Console.WriteLine(bb);
            bool[] ccc = bb.Select(s => s.Equals('1')).ToArray(); ;
            for (int i = 0; i < ccc.Length; i++)
            {
                Console.Write(ccc[i] + " ");
            }
            Console.ReadLine();
            //take address:0x58B array[3-6]->bit27-bit30

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Console.WriteLine("1");
                    Thread.Sleep(1);
                }
            });
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Console.WriteLine("2");
                    Thread.Sleep(1);
                }
            });
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Console.WriteLine("3");
                    Thread.Sleep(1);
                }
            });
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Console.WriteLine("4");
                    Thread.Sleep(1);
                }
            });
            Console.ReadLine();
        }
        public static BitArray ConvertHexToBitArray(string hexData)
        {
            if (hexData == null)
                return null; // or do something else, throw, ...

            BitArray ba = new BitArray(4 * hexData.Length);
            for (int i = 0; i < hexData.Length; i++)
            {
                byte b = byte.Parse(hexData[i].ToString(), NumberStyles.HexNumber);
                for (int j = 0; j < 4; j++)
                {
                    ba.Set(i * 4 + j, (b & (1 << (3 - j))) != 0);
                }
            }
            return ba;
        }
        
    }
}
