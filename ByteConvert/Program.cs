﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteConvert
{
    class Program
    {
        static void Main(string[] args)
        {
            UInt32 a = 12345678;
            byte[] bytes = BitConverter.GetBytes(a);
            Console.WriteLine(BitConverter.ToString(bytes));
            Array.Reverse(bytes);
            Console.WriteLine(BitConverter.ToString(bytes));
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
            
            Console.ReadLine();
        }
        
    }
}