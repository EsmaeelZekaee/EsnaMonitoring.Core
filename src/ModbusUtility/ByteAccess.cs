namespace ModbusUtility
{

    internal class ByteAccess
	{
		
		public static byte HI4BITS(byte n)
		{
			return (byte)(n >> 4 & 15);
		}

		
		public static byte LO4BITS(byte n)
		{
			return (byte)(n & 15);
		}

		
		public static uint MakeLong(ushort high, ushort low)
		{
			return (uint)(low & ushort.MaxValue | (high & ushort.MaxValue) << 16);
		}

		
		public static ushort MakeWord(byte high, byte low)
		{
			return (ushort)(low & byte.MaxValue | (high & byte.MaxValue) << 8);
		}

		
		public static ushort LoWord(uint nValue)
		{
			return (ushort)(nValue & 65535u);
		}

		
		public static ushort HiWord(uint nValue)
		{
			return (ushort)(nValue >> 16);
		}

		
		public static byte LoByte(ushort nValue)
		{
			return (byte)(nValue & 255);
		}

		
		public static byte HiByte(ushort nValue)
		{
			return (byte)(nValue >> 8);
		}
	}
}
