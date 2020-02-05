#nullable enable
using System;
namespace ModbusUtility
{

    public class DataResult<T>
        where T : class
    {
        public DataResult(Result result, T? data)
        {
            Result = result;
            Data = data;
        }
        public DataResult()
        {
        }
        public Result Result { get; set; }
        public T? Data { get; set; }
    }
    /// <summary>This table lists all enumerations defined in Result.</summary>
    ///  <seealso cref="M:ModbusMasterLib.WSMBSControl.GetLastErrorString">GetLastErrorString</seealso>

    public enum Result
    {
        /// <summary>0 - No error.</summary>
        SUCCESS,
        /// <summary>1 - Illegal function.</summary>
        ILLEGAL_FUNCTION,
        /// <summary>2 - Illegal data address.</summary>
        ILLEGAL_DATA_ADDRESS,
        /// <summary>3 - Illegal data value.</summary>
        ILLEGAL_DATA_VALUE,
        /// <summary>4 - Slave device failure.</summary>
        SLAVE_DEVICE_FAILURE,
        /// <summary>5 - Acknowledge.</summary>
        ACKNOWLEDGE,
        /// <summary>6 - Slave device busy.</summary>
        SLAVE_DEVICE_BUSY,
        /// <summary>7 - Negative acknowledge.</summary>
        NEGATIVE_ACKNOWLEDGE,
        /// <summary>8 - Memory parity error.</summary>
        MEMORY_PARITY_ERROR,
        /// <summary>300 - Response timeout.</summary>
        RESPONSE_TIMEOUT = 300,
        /// <summary>301 - Port not open.</summary>
        ISCLOSED,
        /// <summary>302 - CRC Error.</summary>
        CRC,
        /// <summary>303 - Not the expected response received.</summary>
        RESPONSE,
        /// <summary>304 - Byte count error.</summary>
        BYTECOUNT,
        /// <summary>305 - Quantity is out of range.</summary>
        QUANTITY,
        /// <summary>306 - Function out of range. 1 - 127.</summary>
        FUNCTION,
        /// <summary>400 - Comm port not available or in use by other program.</summary>
        NOT_AVAILABLE = 400,
        /// <summary>401 - Write error.</summary>
        WRITE,
        /// <summary>402 - Read error.</summary>
        READ
    }
}
