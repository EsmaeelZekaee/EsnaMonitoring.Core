using System;

namespace ModbusUtility
{

    internal class Modbus
    {

        public Modbus(CTxRx Tx)
        {
            TxRx = Tx;
        }


        public Result ReadFlags(byte unitId, byte function, ushort address, ushort quantity, bool[] Bools, int offset)
        {
            ushort num = 0;
            ushort num2 = 0;
            if (function < 1 || function > 127)
            {
                return Result.FUNCTION;
            }
            if (quantity < 1 || quantity > 2000)
            {
                return Result.QUANTITY;
            }
            if (quantity + offset > Bools.GetLength(0))
            {
                return Result.QUANTITY;
            }
            byte[] array = new byte[8];
            byte[] array2 = new byte[261];
            array[0] = unitId;
            array[1] = function;
            array[2] = ByteAccess.HiByte(address);
            array[3] = ByteAccess.LoByte(address);
            array[4] = ByteAccess.HiByte(quantity);
            array[5] = ByteAccess.LoByte(quantity);
            int num3 = (quantity + 7) / 8 + 3;
            Result result = TxRx.TxRx(array, 6, array2, num3);
            if (result == Result.SUCCESS)
            {
                if (array[0] != array2[0] || array[1] != array2[1])
                {
                    result = Result.RESPONSE;
                }
                else if (num3 - 3 != array2[2])
                {
                    result = Result.BYTECOUNT;
                }
                else
                {
                    int num4 = array2[3];
                    for (int i = 0; i < quantity; i++)
                    {
                        Bools[i + offset] = ((num4 & 1) == 1);
                        num4 >>= 1;
                        if ((num += 1) == 8)
                        {
                            num2 += 1;
                            num = 0;
                            num4 = array2[3 + num2];
                        }
                    }
                }
            }
            return result;
        }


        public Result DetectDevice(byte unitId, out string name)
        {
            name = string.Empty;
            byte function = 66;
            ushort address = 1;
            ushort quantity = 40;
            byte[] array = new byte[8];
            byte[] array2 = new byte[261];
            array[0] = unitId;
            array[1] = (byte)(function & 255);
            array[2] = ByteAccess.HiByte(address);
            array[3] = ByteAccess.LoByte(address);
            array[4] = ByteAccess.HiByte(quantity);
            array[5] = ByteAccess.LoByte(quantity);
            int responseLength = 3 + quantity;
            Result result = TxRx.TxRx(array, 6, array2, responseLength);
            if (result == Result.SUCCESS)
            {
                if (array[0] != array2[0] || array[1] != array2[1])
                {
                    result = Result.RESPONSE;
                }
                else if (quantity != array2[2])
                {
                    result = Result.BYTECOUNT;
                }
                else
                {
                    for (int i = 0; i < quantity && array2[i + 3] > 0; i++)
                    {
                        name += (char)array2[i + 3];

                    }
                }
            }
            return result;
        }


        public Result ReadRegisters(byte unitId, ushort function, ushort address, ushort quantity, short[] registers, int offset)
        {
            if (function < 1 || function > 127)
            {
                return Result.FUNCTION;
            }
            if (quantity < 1 || quantity > 125)
            {
                return Result.QUANTITY;
            }
            if (quantity + offset > registers.GetLength(0))
            {
                return Result.QUANTITY;
            }
            byte[] array = new byte[8];
            byte[] array2 = new byte[261];
            array[0] = unitId;
            array[1] = (byte)(function & 255);
            array[2] = ByteAccess.HiByte(address);
            array[3] = ByteAccess.LoByte(address);
            array[4] = ByteAccess.HiByte(quantity);
            array[5] = ByteAccess.LoByte(quantity);
            int responseLength = 3 + quantity * 2;
            Result result = TxRx.TxRx(array, 6, array2, responseLength);
            if (result == Result.SUCCESS)
            {
                if (array[0] != array2[0] || array[1] != array2[1])
                {
                    result = Result.RESPONSE;
                }
                else if (quantity * 2 != array2[2])
                {
                    result = Result.BYTECOUNT;
                }
                else
                {
                    for (int i = 0; i < quantity; i++)
                    {
                        registers[i + offset] = (short)(array2[2 * i + 4] & byte.MaxValue | (array2[2 * i + 3] & byte.MaxValue) << 8);
                    }
                }
            }
            return result;
        }




        public Result WriteSingleCoil(byte unitId, ushort address, bool coil)
        {
            byte[] array = new byte[8];
            byte[] array2 = new byte[8];
            array[0] = unitId;
            array[1] = 5;
            array[2] = ByteAccess.HiByte(address);
            array[3] = ByteAccess.LoByte(address);
            array[4] = (byte)(coil ? byte.MaxValue : 0);
            array[5] = 0;
            Result result = TxRx.TxRx(array, 6, array2, 6);
            if (result == Result.SUCCESS && array[0] != 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (array[i] != array2[i])
                    {
                        result = Result.RESPONSE;
                    }
                }
            }
            return result;
        }


        public Result WriteSingleRegister(byte unitId, ushort address, short register)
        {
            byte[] array = new byte[8];
            byte[] array2 = new byte[8];
            array[0] = unitId;
            array[1] = 6;
            array[2] = ByteAccess.HiByte(address);
            array[3] = ByteAccess.LoByte(address);
            array[4] = ByteAccess.HiByte((ushort)register);
            array[5] = ByteAccess.LoByte((ushort)register);
            Result result = TxRx.TxRx(array, 6, array2, 6);
            if (result == Result.SUCCESS && array[0] != 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (array[i] != array2[i])
                    {
                        result = Result.RESPONSE;
                    }
                }
            }
            return result;
        }


        public Result WriteFlags(byte unitId, byte function, ushort address, ushort quantity, bool[] Bools, int offset)
        {
            if (function < 1 || function > 127)
            {
                return Result.FUNCTION;
            }
            if (quantity < 1 || quantity > 1968)
            {
                return Result.QUANTITY;
            }
            if (quantity + offset > Bools.GetLength(0))
            {
                return Result.QUANTITY;
            }
            byte[] array = new byte[265];
            byte[] array2 = new byte[8];
            ushort num = 0;
            byte b = 7;
            byte b2 = 0;
            array[0] = unitId;
            array[1] = function;
            array[2] = ByteAccess.HiByte(address);
            array[3] = ByteAccess.LoByte(address);
            array[4] = ByteAccess.HiByte(quantity);
            array[5] = ByteAccess.LoByte(quantity);
            array[6] = (byte)((quantity + 7) / 8);
            for (int i = 0; i < quantity; i++)
            {
                if (Bools[i + offset])
                {
                    b2 |= (byte)(1 << num);
                }
                num += 1;
                if (num == 8)
                {
                    num = 0;
                    array[b] = b2;
                    b += 1;
                    b2 = 0;
                }
            }
            array[b] = b2;
            Result result = TxRx.TxRx(array, array[6] + 7, array2, 6);
            if (result == Result.SUCCESS && array[0] != 0)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (array[j] != array2[j])
                    {
                        result = Result.RESPONSE;
                    }
                }
            }
            return result;
        }


        public Result WriteRegisters(byte unitId, byte function, ushort address, ushort quantity, short[] registers, int offset)
        {
            if (function < 1 || function > 127)
            {
                return Result.FUNCTION;
            }
            if (quantity < 1 || quantity > 123)
            {
                return Result.QUANTITY;
            }
            if (quantity + offset > registers.GetLength(0))
            {
                return Result.QUANTITY;
            }
            byte[] array = new byte[265];
            byte[] array2 = new byte[8];
            array[0] = unitId;
            array[1] = function;
            array[2] = ByteAccess.HiByte(address);
            array[3] = ByteAccess.LoByte(address);
            array[4] = ByteAccess.HiByte(quantity);
            array[5] = ByteAccess.LoByte(quantity);
            array[6] = (byte)(quantity * 2);
            for (int i = 0; i < quantity; i++)
            {
                array[7 + i * 2] = ByteAccess.HiByte((ushort)registers[i + offset]);
                array[7 + i * 2 + 1] = ByteAccess.LoByte((ushort)registers[i + offset]);
            }
            Result result = TxRx.TxRx(array, array[6] + 7, array2, 6);
            if (result == Result.SUCCESS && array[0] != 0)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (array[j] != array2[j])
                    {
                        result = Result.RESPONSE;
                    }
                }
            }
            return result;
        }


        public Result MaskWriteRegister(byte unitId, ushort address, ushort ANDMask, ushort ORMask)
        {
            byte[] array = new byte[10];
            byte[] array2 = new byte[10];
            array[0] = unitId;
            array[1] = 22;
            array[2] = ByteAccess.HiByte(address);
            array[3] = ByteAccess.LoByte(address);
            array[4] = ByteAccess.HiByte(ANDMask);
            array[5] = ByteAccess.LoByte(ANDMask);
            array[6] = ByteAccess.HiByte(ORMask);
            array[7] = ByteAccess.LoByte(ORMask);
            Result result = TxRx.TxRx(array, 8, array2, 8);
            if (result == Result.SUCCESS && array[0] != 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (array[i] != array2[i])
                    {
                        result = Result.RESPONSE;
                    }
                }
            }
            return result;
        }


        public Result ReadWriteMultipleRegisters(byte unitId, ushort readAddress, ushort readSize, short[] readRegisters, ushort writeAddress, ushort writeSize, short[] writeRegisters)
        {
            if (readSize < 1 || readSize > 123)
            {
                return Result.QUANTITY;
            }
            if (writeSize < 1 || writeSize > 121)
            {
                return Result.QUANTITY;
            }
            if (readSize > readRegisters.GetLength(0))
            {
                return Result.QUANTITY;
            }
            if (writeSize > writeRegisters.GetLength(0))
            {
                return Result.QUANTITY;
            }
            byte[] array = new byte[269];
            byte[] array2 = new byte[261];
            array[0] = unitId;
            array[1] = 23;
            array[2] = ByteAccess.HiByte(readAddress);
            array[3] = ByteAccess.LoByte(readAddress);
            array[4] = ByteAccess.HiByte(readSize);
            array[5] = ByteAccess.LoByte(readSize);
            array[6] = ByteAccess.HiByte(writeAddress);
            array[7] = ByteAccess.LoByte(writeAddress);
            array[8] = ByteAccess.HiByte(writeSize);
            array[9] = ByteAccess.LoByte(writeSize);
            array[10] = (byte)(writeSize * 2);
            int responseLength = 3 + readSize * 2;
            for (int i = 0; i < writeSize; i++)
            {
                array[11 + i * 2] = ByteAccess.HiByte((ushort)writeRegisters[i]);
                array[11 + i * 2 + 1] = ByteAccess.LoByte((ushort)writeRegisters[i]);
            }
            Result result = TxRx.TxRx(array, array[10] + 11, array2, responseLength);
            if (result == Result.SUCCESS)
            {
                if (array[0] != array2[0] || array[1] != array2[1])
                {
                    result = Result.RESPONSE;
                }
                else
                {
                    for (int j = 0; j < readSize; j++)
                    {
                        readRegisters[j] = (short)ByteAccess.MakeWord(array2[2 * j + 4], array2[2 * j + 3]);
                    }
                }
            }
            return result;
        }


        public Result ReportSlaveID(byte unitId, out byte byteCount, byte[] deviceSpecific)
        {
            byte[] array = new byte[4];
            byte[] array2 = new byte[255];
            array[0] = unitId;
            array[1] = 17;
            byteCount = 0;
            Result result = TxRx.TxRx(array, 2, array2, int.MaxValue);
            if (result == Result.SUCCESS)
            {
                if (array[0] != array2[0])
                {
                    result = Result.RESPONSE;
                }
                else
                {
                    byteCount = array2[2];
                    int num = Math.Min(array2[2], deviceSpecific.GetLength(0));
                    for (int i = 0; i < num; i++)
                    {
                        deviceSpecific[i] = array2[i + 3];
                    }
                }
            }
            return result;
        }


        private readonly CTxRx TxRx;
    }
}
