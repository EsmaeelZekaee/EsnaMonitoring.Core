using System;

namespace ModbusUtility
{
	/// <summary>
	/// Specifies the modbus mode for a ModbusMasterLib object.
	/// </summary>
	
	public enum Mode
	{
		/// <summary>
		/// Modbus RTU Mode.
		/// </summary>
		
		RTU,
		/// <summary>
		/// Modbus ASCII Mode.
		/// </summary>
		
		ASCII
	}
}
