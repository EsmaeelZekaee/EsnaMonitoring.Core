using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;

namespace ModbusUtility
{
    /// <summary>
    /// ModbusMasterLib Modbus Master RTU/ASCII .NET control.
    /// </summary>
    ///
    /// WARNING: If you change the name of this class, you will need to change the
    ///          'Resource File Name' property for the managed resource compiler tool
    ///          associated with all .resx files this class depends on.  Otherwise,
    ///          the designers will not be able to interact properly with localized
    ///          resources associated with this form.

    public class ModbusControl : Component, IModbusControl
    {
        /// <summary>
        /// Initializes a new instance of the WSMBSControl class.
        /// </summary>

        public ModbusControl()
        {
            InitializeComponent();
            port.BaudRate = 9600;
            port.Parity = System.IO.Ports.Parity.None;
            port.StopBits = System.IO.Ports.StopBits.One;
            port.DataBits = 8;
            port.PortName = "COM1";
            port.ReadTimeout = 1000;
            port.WriteTimeout = 1000;
            port.DtrEnable = false;
            port.RtsEnable = false;
            Modbus = new Modbus(TxRx = new CTxRx(port));
            TxRx.Mode = Mode.RTU;
        }

        /// <summary>
        /// Initializes a new instance of the WSMBSControl class together with the specified container.
        /// </summary>
        /// <param name="container">An IContainer that represents the container for the WSMBSControl.</param>

        public ModbusControl(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            port.BaudRate = 9600;
            port.Parity = System.IO.Ports.Parity.None;
            port.StopBits = System.IO.Ports.StopBits.One;
            port.DataBits = 8;
            port.PortName = "COM1";
            port.ReadTimeout = 1000;
            port.WriteTimeout = 1000;
            port.DtrEnable = false;
            port.RtsEnable = false;
            Modbus = new Modbus(TxRx = new CTxRx(port));
            TxRx.Mode = Mode.RTU;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Request to Send (RTS) signal is enabled during serial communication.
        /// </summary>
        /// <example>
        /// This example shows how to set the RTSEnable to false.
        /// <code lang="c#">
        /// wsmbsControl1.RTSEnable = false;
        /// </code>
        /// <code lang="vbnet">
        /// WsmbsControl1.RTSEnable = false
        /// </code>
        /// <code lang="cpp">
        /// wsmbsControl1->RTSEnable = false;
        /// </code>
        /// </example>
        /// <remarks>Set the RTSEnable before Open(). The default value is false.</remarks>



        [Description("Indicating whether the Request to Send (RTS) signal is enabled during serial communication.")]
        [Category("Port settings")]
        public bool RTSEnable { get; set; }

        /// <summary>
        /// Gets or sets a value that enables the Data Terminal Ready (DTR) signal during serial communication.
        /// </summary>
        /// <example>
        /// This example shows how to set the DTREnable to false.
        /// <code lang="c#">
        /// wsmbsControl1.DTREnable = false;
        /// </code>
        /// <code lang="vbnet">
        /// WsmbsControl1.DTREnable = false
        /// </code>
        /// <code lang="cpp">
        /// wsmbsControl1->DTREnable = false;
        /// </code>
        /// </example>
        /// <remarks>Set the DTREnable before Open(). The default value is false.</remarks>



        [Category("Port settings")]
        [Description("Enables the Data Terminal Ready (DTR) signal during serial communication.")]
        public bool DTREnable { get; set; }

        /// <summary>
        /// Gets or sets the Modbus protocol mode.
        /// </summary>
        /// <example>
        /// This example shows how to set the Mode to Modbus RTU.
        /// <code lang="c#">
        /// wsmbsControl1.Mode = ModbusMasterLib.Mode.RTU;
        /// </code>
        /// <code lang="vbnet">
        /// WsmbsControl1.Mode = ModbusMasterLib.Mode.RTU
        /// </code>
        /// <code lang="cpp">
        /// wsmbsControl1->Mode = ModbusMasterLib::Mode::RTU;
        /// </code>
        /// </example>
        /// <remarks>Set the Mode before Open(). The default value is Modbus RTU.</remarks>



        [Category("Modbus")]
        [Description("Select which protocol mode to use.")]
        public Mode Mode { get; set; }

        /// <summary>
        /// If your slave device or RS232/RS485 converter echoes the request just sent.
        /// Setting RemoveEcho to true will remove the request echoed back from the response.
        /// </summary>
        /// <example>
        /// This example shows how to set RemoveEcho to true.
        /// <code lang="c#">
        /// wsmbsControl1.RemoveEcho = true;
        /// </code>
        /// <code lang="vbnet">
        /// WsmbsControl1.RemoveEcho = true
        /// </code>
        /// <code lang="cpp">
        /// wsmbsControl1->RemoveEcho = true;
        /// </code>
        /// </example>
        /// <remarks>Set the RemoveEcho before Open(). The default value is false.</remarks>



        [Description("True if request is echoed back")]
        [Category("Port settings")]
        public bool RemoveEcho { get; set; }

        /// <summary>
        /// Gets or sets the baud rate at which the communications device operates.
        /// </summary>
        /// <example>
        /// This example shows how to set the baud rate to 9600.
        /// <code lang="c#">
        /// wsmbsControl1.BaudRate = 9600;
        /// </code>
        /// <code lang="vbnet">
        /// WsmbsControl1.BaudRate = 9600
        /// </code>
        /// <code lang="cpp">
        /// wsmbsControl1->BaudRate = 9600;
        /// </code>
        /// </example>
        /// <remarks>Set the BaudRate before Open(). The default value is 9600.</remarks>



        [Category("Port settings")]
        [Description("The baud rate at which the communications device operates.")]
        public int BaudRate { get; set; } = 9600;

        /// <summary>
        /// Gets or sets the standard length of data bits per byte.
        /// </summary>
        /// <example>
        /// This example shows how to set the DataBits to 8.
        /// <code lang="c#">
        /// wsmbsControl1.DataBits = 8;
        /// </code>
        /// <code lang="vbnet">
        /// WsmbsControl1.DataBits = 8
        /// </code>
        /// <code lang="cpp">
        /// wsmbsControl1->DataBits = 8;
        /// </code>
        /// </example>
        /// <remarks>Set the DataBits before Open(). Forced to 8 data bits in RTU mode. Default 7 for ASCII mode.</remarks>



        [Description("The number of stop bits to be used. 7 or 8")]
        [Category("Port settings")]
        public int DataBits
        {
            get
            {
                return _DataBits;
            }
            set
            {
                _DataBitsSet = true;
                _DataBits = value;
            }
        }

        /// <summary>
        /// Gets or sets the parity scheme to be used.
        /// </summary>
        /// <example>
        /// This example shows how to set the Parity to None.
        /// <code lang="c#">
        /// wsmbsControl1.Parity = ModbusMasterLib.Parity.None;
        /// </code>
        /// <code lang="vbnet">
        /// WsmbsControl1.Parity = ModbusMasterLib.Parity.None
        /// </code>
        /// <code lang="cpp">
        /// wsmbsControl1->Parity = ModbusMasterLib::Parity::None;
        /// </code>
        /// </example>
        /// <remarks>Set the Parity before Open(). The default value is None.</remarks>



        [Category("Port settings")]
        [Description("The parity scheme to be used.")]
        public Parity Parity { get; set; }

        /// <summary>
        /// Gets or sets the number of stop bits to be used.
        /// </summary>
        /// <example>
        /// This example shows how to set the stop bits to 1 stop bit.
        /// <code lang="c#">
        /// wsmbsControl1.StopBits = 1;
        /// </code>
        /// <code lang="vbnet">
        /// WsmbsControl1.StopBits = 1
        /// </code>
        /// <code lang="cpp">
        /// wsmbsControl1->StopBits = 1;
        /// </code>
        /// </example>
        /// <remarks>Set the number of stop bits before Open(). The default value is 1.</remarks>



        [Description("The number of stop bits to be used. 1 or 2")]
        [Category("Port settings")]
        public int StopBits
        {
            get
            {
                return _StopBits;
            }
            set
            {
                switch (value)
                {
                    case 1:
                        _StopBits = 1;
                        return;
                    case 2:
                        _StopBits = 2;
                        return;
                    default:
                        return;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Max time to wait for response.
        /// </summary>
        /// <example>
        /// This example shows how to set the response timeout to 1000ms.
        /// <code lang="c#">
        /// wsmbsControl1.ResponseTimeout = 1000;
        /// </code>
        /// <code lang="vbnet">
        /// WsmbsControl1.ResponseTimeout = 1000
        /// </code>
        /// <code lang="cpp">
        /// wsmbsControl1->ResponseTimeout = 1000;
        /// </code>
        /// </example>
        /// <remarks>Set the ResponseTimeout before Open(). The default value is 1000.</remarks>

        [Description("Max time to wait for response 100 - 30000ms.")]
        [Category("Port settings")]
        public int ResponseTimeout
        {
            get
            {
                return _ResponseTimeout;
            }
            set
            {
                if (value >= 100 && value <= 30000)
                {
                    _ResponseTimeout = value;
                }
            }
        }

        /// <summary>
        /// The communications port. The default is "COM1".
        /// </summary>
        /// <example>
        /// This example shows how to set the port to COM1.
        /// <code lang="c#">
        /// wsmbsControl1.PortName = "COM1";
        /// </code>
        /// <code lang="vbnet">
        /// WsmbsControl1.PortName = "COM1"
        /// </code>
        /// <code lang="cpp">
        /// wsmbsControl1->PortName = "COM1";
        /// </code>
        /// </example>
        /// <remarks>A list of valid port names can be obtained using the <see cref="M:ModbusMasterLib.WSMBSControl.GetPortNames">GetPortNames</see> method.</remarks>
        /// <seealso cref="M:ModbusMasterLib.WSMBSControl.GetPortNames">GetPortNames</seealso>



        [Description("The communications port. The default is \"COM1\"")]
        [Category("Port settings")]
        public string PortName
        {
            get
            {
                return _PortName;
            }
            set
            {
                _PortName = value;
            }
        }

        /// <summary>
        /// Gets an array of serial port names for the current computer.
        /// <para>Use the GetPortNames method to query the current computer for a list of valid serial port names. For example, you can use this method to determine whether COM1 and COM2 are valid serial ports for the current computer.</para>
        /// </summary>
        /// <returns>An array of serial port names for the current computer.</returns>

        public string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// Modbus function 01 (0x01). Read Coils. 0X references.
        /// <para>This function code is used to read from 1 to 2000 contiguous status of coils in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of coils to read 1-2000.</param>
        /// <param name="coils">Array to hold the values.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result.</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 coils.
        /// <code lang="c#">
        /// bool[] coils = new bool[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadCoils(1, 0, 10, coils);
        /// </code>
        /// <code lang="vbnet">
        /// Dim coils(10) As boolean
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadCoils(1, 0, 10, coils)
        /// </code>
        /// <code lang="cpp">
        /// array<bool>^ coils = gcnew array<bool>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadCoils(1, 0, 10, coils);
        /// </code>
        /// </example>

        public Result ReadCoils(byte unitId, ushort address, ushort quantity, bool[] coils)
        {
            return Res = Modbus.ReadFlags(unitId, 1, address, quantity, coils, 0);
        }

        /// <summary>
        /// Modbus function 01 (0x01). Read Coils. 0X references.
        /// <para>This function code is used to read from 1 to 2000 contiguous status of coils in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of coils to read 1-2000.</param>
        /// <param name="coils">Array to hold the values.</param>
        /// <param name="offset">The offset in the coils array to begin writing.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result.</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 coils.
        /// <code lang="c#">
        /// bool[] coils = new bool[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadCoils(1, 0, 10, coils, 0);
        /// </code>
        /// <code lang="vbnet">
        /// Dim coils(10) As boolean
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadCoils(1, 0, 10, coils, 0)
        /// </code>
        /// <code lang="cpp">
        /// array<bool>^ coils = gcnew array<bool>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadCoils(1, 0, 10, coils, 0);
        /// </code>
        /// </example>

        public Result ReadCoils(byte unitId, ushort address, ushort quantity, bool[] coils, int offset)
        {
            return Res = Modbus.ReadFlags(unitId, 1, address, quantity, coils, offset);
        }

        /// <summary>
        /// Modbus function 02 (0x02). Read Discrete Inputs. 1X references.
        /// <para>This function code is used to read from 1 to 2000 contiguous status of discrete inputs in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of inputs to read 1-2000.</param>
        /// <param name="discreteInputs">Array to hold the values.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 Discrete Inputs.
        /// <code lang="c#">
        /// bool[] discreteInputs = new bool[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadDiscreteInputs(1, 0, 10, discreteInputs);
        /// </code>
        /// <code lang="vbnet">
        /// Dim discreteInputs(10) As boolean
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadDiscreteInputs(1, 0, 10, discreteInputs)
        /// </code>
        /// <code lang="cpp">
        /// array<bool>^ discreteInputs = gcnew array<bool>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadDiscreteInputs(1, 0, 10, discreteInputs);
        /// </code>
        /// </example>

        public Result ReadDiscreteInputs(byte unitId, ushort address, ushort quantity, bool[] discreteInputs)
        {
            return Res = Modbus.ReadFlags(unitId, 2, address, quantity, discreteInputs, 0);
        }

        /// <summary>
        /// Modbus function 02 (0x02). Read Discrete Inputs. 1X references.
        /// <para>This function code is used to read from 1 to 2000 contiguous status of discrete inputs in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of inputs to read 1-2000.</param>
        /// <param name="discreteInputs">Array to hold the values.</param>
        /// <param name="offset">The offset in the discreteInputs array to begin writing.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 Discrete Inputs.
        /// <code lang="c#">
        /// bool[] discreteInputs = new bool[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadDiscreteInputs(1, 0, 10, discreteInputs, 0);
        /// </code>
        /// <code lang="vbnet">
        /// Dim discreteInputs(10) As boolean
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadDiscreteInputs(1, 0, 10, discreteInputs, 0)
        /// </code>
        /// <code lang="cpp">
        /// array<bool>^ discreteInputs = gcnew array<bool>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadDiscreteInputs(1, 0, 10, discreteInputs, 0);
        /// </code>
        /// </example>

        public Result ReadDiscreteInputs(byte unitId, ushort address, ushort quantity, bool[] discreteInputs, int offset)
        {
            return Res = Modbus.ReadFlags(unitId, 2, address, quantity, discreteInputs, offset);
        }

        /// <summary>
        /// Modbus function 03 (0x03). Read Holding Registers. 4X references.
        /// <para>This function code is used to read the contents of a contiguous block of holding registers in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of holding registers to read 1-125.</param>
        /// <param name="registers">Array to hold the values.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 holding registers.
        /// <code lang="c#">
        /// Int16[] registers = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadHoldingRegisters(1, 0, 10, registers);
        /// </code>
        /// <code lang="vbnet">
        /// Dim registers(10) As Short
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadHoldingRegisters(1, 0, 10, registers)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ registers = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadHoldingRegisters(1, 0, 10, registers);
        /// </code>
        /// </example>

        public Result ReadHoldingRegisters(byte unitId, ushort address, ushort quantity, short[] registers)
        {
            return Res = Modbus.ReadRegisters(unitId, 3, address, quantity, registers, 0);
        }

        /// <summary>
        /// Modbus function 03 (0x03). Read Holding Registers. 4X references.
        /// <para>This function code is used to read the contents of a contiguous block of holding registers in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of holding registers to read 1-125.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 holding registers.
        /// <code lang="c#">
        /// Int16[] registers = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadHoldingRegisters(1, 0, 10, registers);
        /// </code>
        /// <code lang="vbnet">
        /// Dim registers(10) As Short
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadHoldingRegisters(1, 0, 10, registers)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ registers = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadHoldingRegisters(1, 0, 10, registers);
        /// </code>
        /// </example>
        public async ValueTask<DataResult<short[]>> ReadHoldingRegistersAsync(byte unitId, ushort address, ushort quantity)
        {
            return await Task.Run(() =>
            {
                short[] registers = new short[quantity];
                var result = ReadHoldingRegisters(unitId, address, quantity, registers);
                return new DataResult<short[]>(result, registers);
            });
        }

        /// <summary>
        /// Modbus function 03 (0x03). Read Holding Registers. 4X references.
        /// <para>This function code is used to read the contents of a contiguous block of holding registers in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of holding registers to read 1-125.</param>
        /// <param name="registers">Array to hold the values.</param>
        /// <param name="offset">The offset in the registers array to begin writing.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 holding registers.
        /// <code lang="c#">
        /// Int16[] registers = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadHoldingRegisters(1, 0, 10, registers, 0);
        /// </code>
        /// <code lang="vbnet">
        /// Dim registers(10) As Short
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadHoldingRegisters(1, 0, 10, registers, 0)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ registers = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadHoldingRegisters(1, 0, 10, registers, 0);
        /// </code>
        /// </example>

        public Result ReadHoldingRegisters(byte unitId, ushort address, ushort quantity, short[] registers, int offset)
        {
            return Res = Modbus.ReadRegisters(unitId, 3, address, quantity, registers, offset);
        }

        /// <summary>
        /// Modbus function 04 (0x04). Read Input Registers. 3X references.
        /// <para>This function code is used to read from 1 to 125 contiguous input registers in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of inputs registers to read 1-125.</param>
        /// <param name="registers">Array to hold the values.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 input registers.
        /// <code lang="c#">
        /// Int16[] registers = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadInputRegisters(1, 0, 10, registers);
        /// </code>
        /// <code lang="vbnet">
        /// Dim registers(10) As Short
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadInputRegisters(1, 0, 10, registers)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ registers = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadInputRegisters(1, 0, 10, registers);
        /// </code>
        /// </example>

        public Result ReadInputRegisters(byte unitId, ushort address, ushort quantity, short[] registers)
        {
            return Res = Modbus.ReadRegisters(unitId, 4, address, quantity, registers, 0);
        }

        /// <summary>
        /// Modbus function 04 (0x04). Read Input Registers. 3X references.
        /// <para>This function code is used to read from 1 to 125 contiguous input registers in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of inputs registers to read 1-125.</param>
        /// <param name="registers">Array to hold the values.</param>
        /// <param name="offset">The offset in the registers array to begin writing.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 input registers.
        /// <code lang="c#">
        /// Int16[] registers = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadInputRegisters(1, 0, 10, registers, 0);
        /// </code>
        /// <code lang="vbnet">
        /// Dim registers(10) As Short
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadInputRegisters(1, 0, 10, registers, 0)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ registers = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadInputRegisters(1, 0, 10, registers, 0);
        /// </code>
        /// </example>

        public Result ReadInputRegisters(byte unitId, ushort address, ushort quantity, short[] registers, int offset)
        {
            return Res = Modbus.ReadRegisters(unitId, 4, address, quantity, registers, offset);
        }

        /// <summary>
        /// Modbus function 05 (0x05). Write Single Coil. 0X references.
        /// <para>This function code is used to write a single output to either ON or OFF in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 0-255. 0 means broadcast.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="coil">The value of the coil to write. 0 or 1.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to write a "1" to address 0.
        /// <code lang="c#">
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.WriteSingleCoil(1, 0, true);
        /// </code>
        /// <code lang="vbnet">
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.WriteSingleCoil(1, 0, 1)
        /// </code>
        /// <code lang="cpp">
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->WriteSingleCoil(1, 0, true);
        /// </code>
        /// </example>

        public Result WriteSingleCoil(byte unitId, ushort address, bool coil)
        {
            return Res = Modbus.WriteSingleCoil(unitId, address, coil);
        }

        public Result DetectDevice(byte unitId, out string name)
        {
            return Res = Modbus.DetectDevice(unitId, out name);
        }

        /// <summary>
        /// Modbus function 06 (0x06). Write Single Register. 3X references.
        /// <para>This function code is used to write a single holding register in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 0-255. 0 means broadcast.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="register">The value of the register to write.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// This example show how to write the value 100 to address 0.
        /// <example>
        /// <code lang="c#">
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.WriteSingleRegister(1, 0, 100);
        /// </code>
        /// <code lang="vbnet">
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.WriteSingleRegister(1, 0, 100)
        /// </code>
        /// <code lang="cpp">
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->WriteSingleRegister(1, 0, 100);
        /// </code>
        /// </example>

        public Result WriteSingleRegister(byte unitId, ushort address, short register)
        {
            return Res = Modbus.WriteSingleRegister(unitId, address, register);
        }

        /// <summary>
        /// Modbus function 15 (0x0F). Write Multiple Coils. 0X references.
        /// <para>This function code is used to force each coil in a sequence of coils to either ON or OFF in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 0-255. 0 means broadcast.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of coils to write 1-1968.</param>
        /// <param name="coils">Array to hold the coils to write.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to write 10 coils.
        /// <code lang="c#">
        /// bool[] coils = new bool[10];
        /// ModbusMasterLib.Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    coils[i] = true;
        /// Result = wsmbsControl1.WriteMultipleCoils(1, 0, 10, coils);
        /// </code>
        /// <code lang="vbnet">
        /// Dim coils(10) As boolean
        /// Dim i As Integer
        /// Dim Result As ModbusMasterLib.Result
        /// For i = 0 To 9
        ///    coils(i) = 1
        /// Next i
        /// Result = WsmbsControl1.WriteMultipleCoils(1, 0, 10, coils)
        /// </code>
        /// <code lang="cpp">
        ///  array<bool>^ coils = gcnew array<bool>(10);
        ///  ModbusMasterLib::Result Result;
        ///  for (int i = 0; i < 10; i++)
        ///     coils[i] = true;
        ///  Result = wsmbsControl1->WriteMultipleCoils(1, 0, 10, coils);
        /// </code>
        /// </example>

        public Result WriteMultipleCoils(byte unitId, ushort address, ushort quantity, bool[] coils)
        {
            return Res = Modbus.WriteFlags(unitId, 15, address, quantity, coils, 0);
        }

        /// <summary>
        /// Modbus function 15 (0x0F). Write Multiple Coils. 0X references.
        /// <para>This function code is used to force each coil in a sequence of coils to either ON or OFF in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 0-255. 0 means broadcast.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of coils to write 1-1968.</param>
        /// <param name="coils">Array to hold the coils to write.</param>
        /// <param name="offset">The zero-based offset in the coils parameter at which to begin copying coils to the port.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to write 10 coils.
        /// <code lang="c#">
        /// bool[] coils = new bool[10];
        /// ModbusMasterLib.Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    coils[i] = true;
        /// Result = wsmbsControl1.WriteMultipleCoils(1, 0, 10, coils, 0);
        /// </code>
        /// <code lang="vbnet">
        /// Dim coils(10) As boolean
        /// Dim i As Integer
        /// Dim Result As ModbusMasterLib.Result
        /// For i = 0 To 9
        ///    coils(i) = 1
        /// Next i
        /// Result = WsmbsControl1.WriteMultipleCoils(1, 0, 10, coils, 0)
        /// </code>
        /// <code lang="cpp">
        ///  array<bool>^ coils = gcnew array<bool>(10);
        ///  ModbusMasterLib::Result Result;
        ///  for (int i = 0; i < 10; i++)
        ///     coils[i] = true;
        ///  Result = wsmbsControl1->WriteMultipleCoils(1, 0, 10, coils, 0);
        /// </code>
        /// </example>

        public Result WriteMultipleCoils(byte unitId, ushort address, ushort quantity, bool[] coils, int offset)
        {
            return Res = Modbus.WriteFlags(unitId, 15, address, quantity, coils, offset);
        }

        /// <summary>
        /// Modbus function  16 (0x10). Write Multiple registers. 3X references.
        /// <para>This function code is used to write a block of contiguous registers (1 to 123 registers) in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 0-255. 0 means broadcast.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of registers to write 1-123.</param>
        /// <param name="registers">Array to hold the registers to write.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to write 10 registers.
        /// <code lang="c#">
        /// Int16[] registers = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    registers[i] = i;
        /// Result = wsmbsControl1.WriteMultipleRegisters(1, 0, 10, registers);
        /// </code>
        /// <code lang="vbnet">
        /// Dim registers(10) As Short
        /// Dim i As Integer
        /// Dim Result As ModbusMasterLib.Result
        /// For i = 0 To 9
        ///    registers(i) = i
        /// Next i
        /// Result = WsmbsControl1.WriteMultipleRegisters(1, 0, 10, registers)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ registers = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    registers[i] = i;
        /// Result = wsmbsControl1->WriteMultipleRegisters(1, 0, 10, registers);
        /// </code>
        /// </example>

        public Result WriteMultipleRegisters(byte unitId, ushort address, ushort quantity, short[] registers)
        {
            return Res = Modbus.WriteRegisters(unitId, 16, address, quantity, registers, 0);
        }

        /// <summary>
        /// Modbus function  16 (0x10). Write Multiple registers. 3X references.
        /// <para>This function code is used to write a block of contiguous registers (1 to 123 registers) in a remote device.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 0-255. 0 means broadcast.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of registers to write 1-123.</param>
        /// <param name="registers">Array to hold the registers to write.</param>
        /// <param name="offset">The zero-based offset in the registers parameter at which to begin copying registers to the port.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to write 10 registers.
        /// <code lang="c#">
        /// Int16[] registers = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    registers[i] = i;
        /// Result = wsmbsControl1.WriteMultipleRegisters(1, 0, 10, registers, 0);
        /// </code>
        /// <code lang="vbnet">
        /// Dim registers(10) As Short
        /// Dim i As Integer
        /// Dim Result As ModbusMasterLib.Result
        /// For i = 0 To 9
        ///    registers(i) = i
        /// Next i
        /// Result = WsmbsControl1.WriteMultipleRegisters(1, 0, 10, registers, 0)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ registers = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    registers[i] = i;
        /// Result = wsmbsControl1->WriteMultipleRegisters(1, 0, 10, registers, 0);
        /// </code>
        /// </example>

        public Result WriteMultipleRegisters(byte unitId, ushort address, ushort quantity, short[] registers, int offset)
        {
            return Res = Modbus.WriteRegisters(unitId, 16, address, quantity, registers, offset);
        }

        /// <summary>
        /// Modbus function 23 (0x17). Read/Write Multiple registers. 3X references.
        /// <para>This function code performs a combination of one read operation and one write operation in a
        /// single Modbus transaction. The write operation is performed before the read.</para>
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="readAddress">Modbus address 0-65535.</param>
        /// <param name="readQuantity">Number of registers to Read 1-125.</param>
        /// <param name="readRegisters">Array to hold the registers to read.</param>
        /// <param name="writeAddress">Modbus address 0-65535.</param>
        /// <param name="writeQuantity">Number of registers to write 1-121.</param>
        /// <param name="writeRegisters">Array to hold the registers to write.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to write 10 registers and read 10 registers.
        /// <code lang="c#">
        /// Int16[] readRegisters = new Int16[10];
        /// Int16[] writeRegisters = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    writeRegisters[i] = i;
        /// Result = wsmbsControl1.ReadWriteMultipleRegisters(1, 0, 10, readRegisters, 0, 10, writeRegisters);
        /// </code>
        /// <code lang="vbnet">
        /// Dim readRegisters(10) As Short
        /// Dim writeRegisters(10) As Short
        /// Dim i As Integer
        /// Dim Result As WSMBSResult
        /// For i = 0 To 9
        ///    writeRegisters(i) = i
        /// Next i
        /// Result = WsmbsControl1.ReadWriteMultipleRegisters(1, 0, 10, readRegisters, 0, 10, writeRegisters)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ readRegisters = gcnew array<Int16>(10);
        /// array<Int16>^ writeRegisters = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    writeRegisters[i] = i;
        /// Result = wsmbsControl1->ReadWriteMultipleRegisters(1, 0, 10, readRegisters, 0, 10, writeRegisters);
        /// </code>
        /// </example>

        public Result ReadWriteMultipleRegisters(byte unitId, ushort readAddress, ushort readQuantity, short[] readRegisters, ushort writeAddress, ushort writeQuantity, short[] writeRegisters)
        {
            return Res = Modbus.ReadWriteMultipleRegisters(unitId, readAddress, readQuantity, readRegisters, writeAddress, writeQuantity, writeRegisters);
        }

        /// <summary>
        /// Read coils using a user defined Modbus function code.
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="function">The user function 1-127.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of coils to read 1-2000.</param>
        /// <param name="coils">Array to hold the values.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 user defined coils. If function 01 is used then the function do the same as ReadCoils.
        /// <code lang="c#">
        /// bool[] coils = new bool[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadUserDefinedCoils(1, 1, 0, 10, coils);
        /// </code>
        /// <code lang="vbnet">
        /// Dim coils(10) As boolean
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadUserDefinedCoils(1, 1, 0, 10, coils)
        /// </code>
        /// <code lang="cpp">
        /// array<bool>^ coils = gcnew array<bool>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadUserDefinedCoils(1, 1, 0, 10, coils);
        /// </code>
        /// </example>

        public Result ReadUserDefinedCoils(byte unitId, byte function, ushort address, ushort quantity, bool[] coils)
        {
            return Res = Modbus.ReadFlags(unitId, function, address, quantity, coils, 0);
        }

        /// <summary>
        /// Read coils using a user defined Modbus function code.
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="function">The user function 1-127.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of coils to read 1-2000.</param>
        /// <param name="coils">Array to hold the values.</param>
        /// <param name="offset">The offset in the coils array to begin writing.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 user defined coils. If function 01 is used then the function do the same as ReadCoils.
        /// <code lang="c#">
        /// bool[] coils = new bool[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadUserDefinedCoils(1, 1, 0, 10, coils, 0);
        /// </code>
        /// <code lang="vbnet">
        /// Dim coils(10) As boolean
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadUserDefinedCoils(1, 1, 0, 10, coils, 0)
        /// </code>
        /// <code lang="cpp">
        /// array<bool>^ coils = gcnew array<bool>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadUserDefinedCoils(1, 1, 0, 10, coils, 0);
        /// </code>
        /// </example>

        public Result ReadUserDefinedCoils(byte unitId, byte function, ushort address, ushort quantity, bool[] coils, int offset)
        {
            return Res = Modbus.ReadFlags(unitId, function, address, quantity, coils, offset);
        }

        /// <summary>
        /// Read registers using a user defined Modbus function code.
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="function">The user function 1-127.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of registers to read 1-125.</param>
        /// <param name="registers">Array to hold the values.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 user defined registers. If function 03 is used then the function do the same as ReadHoldingRegisters.
        /// <code lang="c#">
        /// Int16[] registers = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadUserDefinedRegisters(1, 3, 0, 10, registers);
        /// </code>
        /// <code lang="vbnet">
        /// Dim registers(10) As Short
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadUserDefinedRegisters(1, 3, 0, 10, registers)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ registers = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadUserDefinedRegisters(1, 3, 0, 10, registers);
        /// </code>
        /// </example>

        public Result ReadUserDefinedRegisters(byte unitId, byte function, ushort address, ushort quantity, short[] registers)
        {
            return Res = Modbus.ReadRegisters(unitId, function, address, quantity, registers, 0);
        }

        /// <summary>
        /// Read registers using a user defined Modbus function code.
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="function">The user function 1-127.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of registers to read 1-125.</param>
        /// <param name="registers">Array to hold the values.</param>
        /// <param name="offset">The offset in the registers array to begin writing.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read 10 user defined registers. If function 03 is used then the function do the same as ReadHoldingRegisters.
        /// <code lang="c#">
        /// Int16[] registers = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// Result = wsmbsControl1.ReadUserDefinedRegisters(1, 3, 0, 10, registers, 0);
        /// </code>
        /// <code lang="vbnet">
        /// Dim registers(10) As Short
        /// Dim Result As ModbusMasterLib.Result
        /// Result = WsmbsControl1.ReadUserDefinedRegisters(1, 3, 0, 10, registers, 0)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ registers = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// Result = wsmbsControl1->ReadUserDefinedRegisters(1, 3, 0, 10, registers, 0);
        /// </code>
        /// </example>

        public Result ReadUserDefinedRegisters(byte unitId, byte function, ushort address, ushort quantity, short[] registers, int offset)
        {
            return Res = Modbus.ReadRegisters(unitId, function, address, quantity, registers, offset);
        }

        /// <summary>
        /// Write coils using a user defined Modbus function code.
        /// </summary>
        /// <param name="unitId">The Unit ID 0-255. 0 means broadcast.</param>
        /// <param name="function">The user function 1-127.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of coils to write 1-1968.</param>
        /// <param name="coils">Array to hold the coils to write.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// <code lang="c#">
        /// bool[] coils = new bool[10];
        /// ModbusMasterLib.Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    coils[i] = true;
        /// Result = wsmbsControl1.WriteUserDefinedCoils(1, 15, 0, 10, coils);
        /// </code>
        /// <code lang="vbnet">
        /// Dim coils(10) As Boolean
        /// Dim i As Integer
        /// Dim Result As ModbusMasterLib.Result
        /// For i = 0 To 9
        ///    coils(i) = 1
        /// Next i
        /// Result = WsmbsControl1.WriteUserDefinedCoils(1, 15, 0, 10, coils)
        /// </code>
        /// <code lang="cpp">
        /// array<bool>^ coils = gcnew array<bool>(10);
        /// ModbusMasterLib::Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    coils[i] = true;
        /// Result = wsmbsControl1->WriteUserDefinedCoils(1, 15, 0, 10, coils);
        /// </code>
        /// </example>

        public Result WriteUserDefinedCoils(byte unitId, byte function, ushort address, ushort quantity, bool[] coils)
        {
            return Res = Modbus.WriteFlags(unitId, function, address, quantity, coils, 0);
        }

        /// <summary>
        /// Write coils using a user defined Modbus function code.
        /// </summary>
        /// <param name="unitId">The Unit ID 0-255. 0 means broadcast.</param>
        /// <param name="function">The user function 1-127.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of coils to write 1-1968.</param>
        /// <param name="coils">Array to hold the coils to write.</param>
        /// <param name="offset">The zero-based offset in the coils parameter at which to begin copying coils to the port.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// <code lang="c#">
        /// bool[] coils = new bool[10];
        /// ModbusMasterLib.Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    coils[i] = true;
        /// Result = wsmbsControl1.WriteUserDefinedCoils(1, 15, 0, 10, coils, 0);
        /// </code>
        /// <code lang="vbnet">
        /// Dim coils(10) As Boolean
        /// Dim i As Integer
        /// Dim Result As ModbusMasterLib.Result
        /// For i = 0 To 9
        ///    coils(i) = 1
        /// Next i
        /// Result = WsmbsControl1.WriteUserDefinedCoils(1, 15, 0, 10, coils, 0)
        /// </code>
        /// <code lang="cpp">
        /// array<bool>^ coils = gcnew array<bool>(10);
        /// ModbusMasterLib::Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    coils[i] = true;
        /// Result = wsmbsControl1->WriteUserDefinedCoils(1, 15, 0, 10, coils, 0);
        /// </code>
        /// </example>

        public Result WriteUserDefinedCoils(byte unitId, byte function, ushort address, ushort quantity, bool[] coils, int offset)
        {
            return Res = Modbus.WriteFlags(unitId, function, address, quantity, coils, offset);
        }

        /// <summary>
        /// Write registers using a user defined Modbus function code.
        /// </summary>
        /// <param name="unitId">The Unit ID 0-255. 0 means broadcast.</param>
        /// <param name="function">The user function 1-127.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of registers to write 1-123.</param>
        /// <param name="registers">Array to hold the registers to write.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// <code lang="c#">
        /// Int16[] registers = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    registers[i] = i;
        /// Result = wsmbsControl1.WriteUserDefinedRegisters(1, 16, 0, 10, registers);
        /// </code>
        /// <code lang="vbnet">
        /// Dim registers(10) As Short
        /// Dim i As Integer
        /// Dim Result As ModbusMasterLib.Result
        /// For i = 0 To 9
        ///    registers(i) = i
        /// Next i
        /// Result = WsmbsControl1.WriteUserDefinedRegisters(1, 16, 0, 10, registers)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ registers = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    registers[i] = i;
        /// Result = wsmbsControl1->WriteUserDefinedRegisters(1, 16, 0, 10, registers);
        /// </code>
        /// </example>

        public Result WriteUserDefinedRegisters(byte unitId, byte function, ushort address, ushort quantity, short[] registers)
        {
            return Res = Modbus.WriteRegisters(unitId, function, address, quantity, registers, 0);
        }

        /// <summary>
        /// Write registers using a user defined Modbus function code.
        /// </summary>
        /// <param name="unitId">The Unit ID 0-255. 0 means broadcast.</param>
        /// <param name="function">The user function 1-127.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="quantity">Number of registers to write 1-123.</param>
        /// <param name="registers">Array to hold the registers to write.</param>
        /// <param name="offset">The zero-based offset in the registers parameter at which to begin copying registers to the port.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// <code lang="c#">
        /// Int16[] registers = new Int16[10];
        /// ModbusMasterLib.Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    registers[i] = i;
        /// Result = wsmbsControl1.WriteUserDefinedRegisters(1, 16, 0, 10, registers, 0);
        /// </code>
        /// <code lang="vbnet">
        /// Dim registers(10) As Short
        /// Dim i As Integer
        /// Dim Result As ModbusMasterLib.Result
        /// For i = 0 To 9
        ///    registers(i) = i
        /// Next i
        /// Result = WsmbsControl1.WriteUserDefinedRegisters(1, 16, 0, 10, registers, 0)
        /// </code>
        /// <code lang="cpp">
        /// array<Int16>^ registers = gcnew array<Int16>(10);
        /// ModbusMasterLib::Result Result;
        /// for (int i = 0; i < 10; i++)
        ///    registers[i] = i;
        /// Result = wsmbsControl1->WriteUserDefinedRegisters(1, 16, 0, 10, registers, 0);
        /// </code>
        /// </example>

        public Result WriteUserDefinedRegisters(byte unitId, byte function, ushort address, ushort quantity, short[] registers, int offset)
        {
            return Res = Modbus.WriteRegisters(unitId, function, address, quantity, registers, offset);
        }

        /// <summary>
        /// Modbus function 17 (0x11). Report slave ID.
        /// </summary>
        /// <param name="unitId">The Unit ID 1-255.</param>
        /// <param name="byteCount">The size of the device specific data.</param>
        /// <param name="deviceSpecific">Buffer to hold the data. Be sure this is large enough.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <example>
        /// This example show how to read the slave ID and device specific data.
        /// <code lang="c#">
        /// ModbusMasterLib.Result res;
        /// byte byteCount;                            // Hold the number of bytes returned in deviceSpecific
        /// byte[] deviceSpecific = new byte[100];     // Be sure the size of this array is big enough
        /// res = wsmbsControl1.ReportSlaveID(1, out byteCount, deviceSpecific);
        /// </code>
        /// <code lang="vbnet">
        /// Dim res As ModbusMasterLib.Result                    '  Hold the number of bytes returned in deviceSpecific
        /// Dim byteCount As Byte                      '  Be sure the size of this array is big enough
        /// Dim deviceSpecific(100) As Byte
        /// res = WsmbsControl1.ReportSlaveID(1, byteCount, deviceSpecific)
        /// </code>
        /// <code lang="cpp">
        /// ModbusMasterLib::Result res;
        /// Byte byteCount;                                                    // Hold the number of bytes returned in deviceSpecific
        /// array<Byte>^ deviceSpecific = gcnew array<Byte>(100);  // Be sure the size of this array is big enough
        /// res = wsmbsControl1->ReportSlaveID(1, byteCount, deviceSpecific);
        /// </code>
        /// </example>
        /// <remarks>byteCount receive the size of the slave ID data. deviceSpecific[0] = Slave ID, deviceSpecific[1] = Run indicator.</remarks>

        public Result ReportSlaveID(byte unitId, out byte byteCount, byte[] deviceSpecific)
        {
            return Res = Modbus.ReportSlaveID(unitId, out byteCount, deviceSpecific);
        }

        /// <summary>
        /// 22 (0x16) Mask Write Register.
        /// </summary>
        /// <param name="unitId">The Unit ID 0-255. 0 means broadcast.</param>
        /// <param name="address">Modbus address 0-65535.</param>
        /// <param name="andMask">The AND mask</param>
        /// <param name="orMask">The OR mask</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">WSMBSControl.Result</see>.</para>
        /// </returns>
        /// <remarks>This function code is used to modify the contents of a specified holding register using a
        /// combination of an AND mask, an OR mask, and the register's current contents. The function
        /// can be used to set or clear individual bits in the register.
        /// <para> Result = (Current Contents AND andMask) OR (orMask AND (NOT andMask))</para>
        /// <para>If the orMask value is zero, the result is simply the logical ANDing of the current contents and
        /// andMask. If the andMask value is zero, the result is equal to the orMask value.</para>
        /// </remarks>

        public Result MaskWriteRegister(byte unitId, ushort address, ushort andMask, ushort orMask)
        {
            return Res = Modbus.MaskWriteRegister(unitId, address, andMask, orMask);
        }

        /// <summary>
        /// Opens the serial comm. port.
        /// </summary>
        /// <returns>
        /// <para>If the function succeeds, the return value is zero.</para>
        /// <para>If the function fails, the return value is nonzero. See <see cref="T:ModbusMasterLib.Result">ModbusMasterLib.Result</see>.</para>
        /// </returns>
        /// <example>
        /// <code lang="c#">
        /// ModbusMasterLib.Result Result;
        /// wsmbsControl1.Mode = ModbusMasterLib.Mode.RTU;
        /// wsmbsControl1.PortName = "COM1";
        /// wsmbsControl1.BaudRate = 9600;
        /// wsmbsControl1.StopBits = 1;
        /// wsmbsControl1.Parity = ModbusMasterLib.Parity.None;
        /// wsmbsControl1.ResponseTimeout = 1000;
        /// Result = wsmbsControl1.Open();
        /// if (Result != ModbusMasterLib.Result.SUCCESS)
        ///    MessageBox.Show(wsmbsControl1.GetLastErrorString());
        /// </code>
        /// <code lang="vbnet">
        /// Dim Result As ModbusMasterLib.Result
        /// WsmbsControl1.Mode = ModbusMasterLib.Mode.RTU
        /// WsmbsControl1.CommPort = "COM1"
        /// WsmbsControl1.BaudRate = 9600
        /// WsmbsControl1.StopBits = 1
        /// WsmbsControl1.Parity = ModbusMasterLib.Parity.None
        /// WsmbsControl1.ResponseTimeout = 1000
        /// Result = WsmbsControl1.Open()
        /// If Result <> ModbusMasterLib.Result.SUCCESS Then
        ///    MessageBox.Show(WsmbsControl1.GetLastErrorString())
        /// End If
        /// </code>
        /// <code lang="cpp">
        /// ModbusMasterLib::Result Result;
        /// wsmbsControl1->Mode = ModbusMasterLib::Mode::RTU;
        /// wsmbsControl1->CommPort = "COM1";
        /// wsmbsControl1->BaudRate = 9600;
        /// wsmbsControl1->StopBits = 1;
        /// wsmbsControl1->Parity = ModbusMasterLib::Parity::None
        /// wsmbsControl1->ResponseTimeout = 1000;
        /// Result = wsmbsControl1->Open();
        /// if (Result != ModbusMasterLib::Result::SUCCESS)
        ///    MessageBox::Show (wsmbsControl1->GetLastErrorString ());
        /// </code>
        /// </example>
        /// <seealso cref="M:ModbusMasterLib.WSMBSControl.Close">Close</seealso>

        public ValueTask<Result> OpenAsync()
        {
            TxRx.Mode = Mode;
            try
            {
                port.ReadTimeout = _ResponseTimeout;
                port.BaudRate = BaudRate;
                port.StopBits = (StopBits)_StopBits;
                port.Parity = (System.IO.Ports.Parity)Parity;
                port.PortName = _PortName;
                port.RtsEnable = RTSEnable;
                port.DiscardNull = false;
                port.DtrEnable = DTREnable;
                port.Handshake = Handshake.None;
                if (TxRx.Mode == Mode.ASCII)
                {
                    if (_DataBitsSet)
                    {
                        port.DataBits = _DataBits;
                    }
                    else
                    {
                        port.DataBits = 7;
                    }
                }
                else
                {
                    _DataBits = 8;
                    port.DataBits = _DataBits;
                }
                port.Open();
            }
            catch (InvalidOperationException ex)
            {
                Error = ex.Message;
                return new ValueTask<Result>(Res = Result.NOT_AVAILABLE);
            }
            catch (Exception ex2)
            {
                Error = ex2.Message;
                return new ValueTask<Result>(Res = Result.NOT_AVAILABLE);
            }
            return new ValueTask<Result>(Res = Result.SUCCESS);
        }

        /// <summary>
        /// Close the serial comm. port.
        /// <para>The best practice for any application is to wait for some amount of time after calling the Close method before attempting to call the Open method, as the port may not be closed instantly.</para>
        /// </summary>
        /// <example>
        /// <code lang="c#">
        /// wsmbsControl1.Close();
        /// </code>
        /// <code lang="vbnet">
        /// WsmbsControl1.Close()
        /// </code>
        /// <code lang="cpp">
        /// wsmbsControl1->Close();
        /// </code>
        /// </example>
        /// <seealso cref="M:ModbusMasterLib.WSMBSControl.Open">Open</seealso>

        public void Close()
        {
            port.Close();
        }

        /// <summary>
        /// Returns the last error as a string.
        /// </summary>
        /// <returns>
        /// Error description as string.
        /// </returns>

        public string GetLastErrorString()
        {
            Result res = Res;
            if (res <= Result.FUNCTION)
            {
                switch (res)
                {
                    case Result.SUCCESS:
                        return "Success";
                    case Result.ILLEGAL_FUNCTION:
                        return "Illegal function.";
                    case Result.ILLEGAL_DATA_ADDRESS:
                        return "Illegal data address.";
                    case Result.ILLEGAL_DATA_VALUE:
                        return "Illegal data value.";
                    case Result.SLAVE_DEVICE_FAILURE:
                        return "Slave device failure.";
                    case Result.ACKNOWLEDGE:
                        return "Acknowledge.";
                    case Result.SLAVE_DEVICE_BUSY:
                        return "Slave device busy.";
                    case Result.NEGATIVE_ACKNOWLEDGE:
                        return "Negative acknowledge.";
                    case Result.MEMORY_PARITY_ERROR:
                        return "Memory parity error.";
                    default:
                        switch (res)
                        {
                            case Result.RESPONSE_TIMEOUT:
                                return "Response timeout.";
                            case Result.ISCLOSED:
                                return "Port not open.";
                            case Result.CRC:
                                return "CRC Error.";
                            case Result.RESPONSE:
                                return "Not the expected response received.";
                            case Result.BYTECOUNT:
                                return "Byte count error.";
                            case Result.QUANTITY:
                                return "Quantity is out of range.";
                            case Result.FUNCTION:
                                return "Modbus function code out of range. 1 - 127.";
                        }
                        break;
                }
            }
            else
            {
                switch (res)
                {
                    case Result.NOT_AVAILABLE:
                        return Error;
                    case Result.WRITE:
                        return "Write error. " + TxRx.GetErrorMessage();
                    case Result.READ:
                        return "Read error. " + TxRx.GetErrorMessage();
                    default:
                        break;
                }
            }
            return "Unknown Error - " + Res.ToString();
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from two Int16.
        /// </summary>
        /// <param name="hiReg">High order register.</param>
        /// <param name="loReg">Low order register.</param>
        /// <returns>A single-precision floating point number.</returns>

        public float RegistersToFloat(short hiReg, short loReg)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(loReg).Concat(BitConverter.GetBytes(hiReg)).ToArray<byte>(), 0);
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from two Int16.
        /// </summary>
        /// <param name="hiReg">High order register.</param>
        /// <param name="loReg">Low order register.</param>
        /// <returns>A 32-bit signed integer.</returns>

        public int RegistersToInt32(short hiReg, short loReg)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(loReg).Concat(BitConverter.GetBytes(hiReg)).ToArray<byte>(), 0);
        }

        /// <summary>
        /// Return two Int16 convereted from a float.
        /// </summary>
        /// <param name="value">A single-precision floating point number.</param>
        /// <returns>An array of two Int16.</returns>

        public short[] FloatToRegisters(float value)
        {
            short[] array = new short[2];
            byte[] bytes = BitConverter.GetBytes(value);
            array[1] = BitConverter.ToInt16(bytes, 0);
            array[0] = BitConverter.ToInt16(bytes, 2);
            return array;
        }

        /// <summary>
        /// Return two Int16 convereted from an Int32.
        /// </summary>
        /// <param name="value">A 32-bit signed integer.</param>
        /// <returns>An array of two Int16.</returns>

        public short[] Int32ToRegisters(int value)
        {
            short[] array = new short[2];
            byte[] bytes = BitConverter.GetBytes(value);
            array[1] = BitConverter.ToInt16(bytes, 0);
            array[0] = BitConverter.ToInt16(bytes, 2);
            return array;
        }

        /// <summary>
        /// Gets the transmit buffer.
        /// </summary>
        /// <param name="byteArray">A byte array to hold the bytes from the transmit buffer.</param>
        /// <returns>Returns the number of transmitted bytes in the byte array.</returns>

        public int GetTxBuffer(byte[] byteArray)
        {
            return TxRx.GetTxBuffer(byteArray);
        }

        /// <summary>
        /// Gets the receive buffer.
        /// </summary>
        /// <param name="byteArray">A byte array to hold the bytes from the receive buffer.</param>
        /// <returns>Returns the number of received bytes in the byte array.</returns>

        public int GetRxBuffer(byte[] byteArray)
        {
            return TxRx.GetRxBuffer(byteArray);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        private void InitializeComponent()
        {
            components = new Container();
        }

        public async ValueTask<DataResult<string>> DetectDeviceAsync(byte unitId)
        {
            return await Task.Run(() =>
            {
                var result = DetectDevice(unitId, out string name);
                return new DataResult<string>(result, name);
            });
        }



        private readonly Modbus Modbus;


        private readonly SerialPort port = new SerialPort();


        private readonly CTxRx TxRx;


        private int _ResponseTimeout = 1000;
        private int _StopBits = 1;


        private int _DataBits = 8;
        private string _PortName = "COM1";
        private string Error = "";


        private Result Res;


        private bool _DataBitsSet;

        /// <summary>
        /// Required designer variable.
        /// </summary>

        private IContainer components;
    }
}
