using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip8
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream("IBM Logo.ch8", FileMode.Open)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var opcode = (ushort)((reader.ReadByte() << 8) | reader.ReadByte());
                    Console.WriteLine($"{opcode.ToString("X4")}");
                }
            }
            Console.ReadKey();
        }
    }
}



