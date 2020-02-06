namespace ModbusUtility
{

    internal class Ascii
	{
		
		public static byte Num2Ascii(byte nNum)
		{
			if (nNum <= 9)
			{
				return (byte)(nNum + 48);
			}
			if (nNum >= 10 && nNum <= 15)
			{
				return (byte)(nNum - 10 + 65);
			}
			return 48;
		}

		
		public static byte Ascii2Num(byte nChar)
		{
			if (nChar >= 48 && nChar <= 57)
			{
				return (byte)(nChar - 48);
			}
			if (nChar >= 65 && nChar <= 70)
			{
				return (byte)(nChar - 65 + 10);
			}
			return 0;
		}

		
		public static byte HiLo4BitsToByte(byte nHi, byte nLo)
		{
			return (byte)((15 & nHi) << 4 | 15 & nLo);
		}

		
		public static void RTU2ASCII(byte[] nRtu, int Size, byte[] nAscii)
		{
			for (int i = 0; i < Size; i++)
			{
				nAscii[1 + i * 2] = Ascii.Num2Ascii(ByteAccess.HI4BITS(nRtu[i]));
				nAscii[1 + i * 2 + 1] = Ascii.Num2Ascii(ByteAccess.LO4BITS(nRtu[i]));
			}
		}

		
		public static byte LRC(byte[] nMsg, int DataLen)
		{
			byte b = 0;
			for (int i = 0; i < DataLen; i++)
			{
				b += nMsg[i];
			}
			return (byte)-b;
		}

		
		public static byte LRCASCII(byte[] MsgASCII, int DataLen)
		{
			byte b = 0;
			int num = (DataLen - 5) / 2;
			for (int i = 0; i < num; i++)
			{
				byte b2 = Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(MsgASCII[1 + i * 2]), Ascii.Ascii2Num(MsgASCII[1 + i * 2 + 1]));
				b += b2;
			}
			return (byte)-b;
		}

		
		public static bool VerifyRespLRC(byte[] Resp, int Length)
		{
			if (Length < 5)
			{
				return false;
			}
			byte b = Ascii.LRCASCII(Resp, Length);
			byte b2 = Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(Resp[Length - 4]), Ascii.Ascii2Num(Resp[Length - 3]));
			return b == b2;
		}
	}
}
