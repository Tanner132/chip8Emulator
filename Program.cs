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
            CPU cpu = new CPU();

            using (BinaryReader reader = new BinaryReader(new FileStream("IBM Logo.ch8", FileMode.Open)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length) //looping through file until position is the end
                {
                    var opcode = (ushort)((reader.ReadByte() << 8) | reader.ReadByte()); // Setting our opcode variable ensuring it is formatted in big endian
                    try
                    {
                        cpu.ExecuteOpCode(opcode);
                    } 
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            Console.ReadKey();
        }

        public class CPU
        {
            public byte[] RAM = new byte[4096]; //Chip 8 uses 4kb's of ram
            public byte[] Register = new byte[16];
            public ushort I = 0;
            public Stack<ushort> Stack = new Stack<ushort>();
            public byte DelayTimer;
            public byte SoundTimer;
            public byte keyboard;

            public byte[] Display = new byte[64 * 32];

            public void ExecuteOpCode(ushort opcode)
            {
                ushort nibble = (ushort)(opcode & 0xf000); //grabbing the first 4 bytes of the opcode. 

                switch (nibble)
                {
                    case 0x0000:
                        if(opcode == 0x00e0)
                        {
                            //If opcode is equal to 00 e0 the we clear display
                            for (int i = 0; i < Display.Length; i++) Display[i] = 0;
                        } else if(opcode == 0x00ee)
                        {
                            //Opcode for returning a subroutine
                            I = Stack.Pop();
                        }
                        else
                        {
                            throw new Exception($"Unsupported Opcode {opcode.ToString("X4")}");
                        }
                        break;
                    case 0x1000:
                        I = (ushort)(opcode & 0x0fff);
                        break;
                    case 0x2000:
                        Stack.Push(I);
                        I = (ushort)(opcode & 0x0fff);
                        break;
                    case 0x3000:
                        if (Register[(opcode & 0x0f00 >> 8)] == (opcode & 0x0ff)) I += 2;
                        break;
                    case 0x4000:
                        if (Register[(opcode & 0x0f00 >> 8)] != (opcode & 0x0ff)) I += 2;
                        break;
                    case 0x5000:
                        if (Register[(opcode & 0x0f00 >> 8)] == (Register[opcode & 0x00f0 >> 4])) I += 2;
                        break;
                    case 0x6000:
                        Register[(opcode & 0x0f00 >> 8)] = (byte)(opcode & 0x00ff);
                        break;
                    case 0x7000:
                        Register[(opcode & 0x0f00 >> 8)] += (byte)(opcode & 0x00ff);
                        break;
                    default:
                        throw new Exception($"Unsupported Opcode {opcode.ToString("X4")}");
                }
            }
        }
    }
}



