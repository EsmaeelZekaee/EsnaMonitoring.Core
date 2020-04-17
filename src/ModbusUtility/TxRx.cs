// ModbusMasterLib.CTxRx

using System;
using System.IO.Ports;
using System.Threading;

using ModbusUtility;

internal class CTxRx
{
    private readonly bool _removeEcho;

    private readonly byte[] _rxBuf = new byte[600];

    private readonly byte[] _txBuf = new byte[600];

    private readonly SerialPort port;

    private int _rxBufSize;

    private int _txBufSize;

    private string Error = string.Empty;

    private int Time;

    public CTxRx(SerialPort _port)
    {
        this.port = _port;
    }

    public Mode Mode { get; set; }

    public string GetErrorMessage()
    {
        return this.Error;
    }

    public int GetRxBuffer(byte[] byteArray)
    {
        if (byteArray.GetLength(0) >= this._rxBufSize)
        {
            Array.Copy(this._rxBuf, byteArray, this._rxBufSize);
            return this._rxBufSize;
        }

        return 0;
    }

    public int GetTxBuffer(byte[] byteArray)
    {
        if (byteArray.GetLength(0) >= this._txBufSize)
        {
            Array.Copy(this._txBuf, byteArray, this._txBufSize);
            return this._txBufSize;
        }

        return 0;
    }

    public Result TxRx(byte[] TXBuf, int QueryLength, byte[] RXBuf, int ResponseLength)
    {
        var num = Environment.TickCount & int.MaxValue;
        this._txBufSize = 0;
        this._rxBufSize = 0;

        if (num - this.Time < 20) Thread.Sleep(4);
        if (!this.port.IsOpen) return Result.ISCLOSED;
        var result = this.Mode switch
            {
                Mode.RTU => this.TxRxRTU(TXBuf, QueryLength, RXBuf, ResponseLength),
                Mode.ASCII => this.TxRxAscii(TXBuf, QueryLength, RXBuf, ResponseLength),
                _ => Result.SUCCESS
            };
        this.Time = Environment.TickCount;
        return result;
    }

    private Result TxRxAscii(byte[] TXBuf, int QueryLength, byte[] RXBuf, int ResponseLength)
    {
        var num = 0;
        var array = new byte[531];
        var array2 = new byte[523];
        Ascii.RTU2ASCII(TXBuf, QueryLength, array);
        var n = Ascii.LRC(TXBuf, QueryLength);
        array[0] = 58;
        array[QueryLength * 2 + 1] = Ascii.Num2Ascii(ByteAccess.HI4BITS(n));
        array[QueryLength * 2 + 2] = Ascii.Num2Ascii(ByteAccess.LO4BITS(n));
        array[QueryLength * 2 + 3] = 13;
        array[QueryLength * 2 + 4] = 10;
        this.port.DiscardInBuffer();
        this.port.DiscardOutBuffer();
        try
        {
            this.port.Write(array, 0, QueryLength * 2 + 5);
        }
        catch (Exception ex)
        {
            this.Error = ex.Message;
            return Result.WRITE;
        }

        this._txBufSize = QueryLength * 2 + 5;
        Array.Copy(array, this._txBuf, this._txBufSize);
        if (TXBuf[0] == 0) return Result.SUCCESS;
        if (this._removeEcho)
        {
            try
            {
                do
                {
                    var num2 = this.port.Read(RXBuf, 0, this._txBufSize - num);
                    num += num2;
                }
                while (this._txBufSize - num > 0);
            }
            catch (TimeoutException ex2)
            {
                this.Error = ex2.Message;
                return Result.RESPONSE_TIMEOUT;
            }
            catch (Exception ex3)
            {
                this.Error = ex3.Message;
                return Result.READ;
            }

            num = 0;
        }

        try
        {
            do
            {
                var num3 = this.port.Read(array2, num, 11 - num);
                num += num3;
            }
            while (11 - num > 0);
        }
        catch (TimeoutException ex4)
        {
            this.Error = ex4.Message;
            return Result.RESPONSE_TIMEOUT;
        }
        catch (Exception ex5)
        {
            this.Error = ex5.Message;
            return Result.READ;
        }
        finally
        {
            this._rxBufSize = num;
            Array.Copy(array2, this._rxBuf, this._rxBufSize);
        }

        if (Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(array2[3]), Ascii.Ascii2Num(array2[4])) > 128)
        {
            if (!Ascii.VerifyRespLRC(array2, 11)) return Result.CRC;
            return (Result)Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(array2[5]), Ascii.Ascii2Num(array2[6]));
        }

        if (ResponseLength == int.MaxValue)
        {
            if (Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(array2[3]), Ascii.Ascii2Num(array2[4])) != 17)
                return Result.RESPONSE;
            ResponseLength = Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(array2[5]), Ascii.Ascii2Num(array2[6])) + 3;
        }

        try
        {
            var num4 = ResponseLength * 2 + 5;
            do
            {
                var num5 = this.port.Read(array2, num, num4 - num);
                num += num5;
            }
            while (num4 - num > 0);
        }
        catch (TimeoutException ex6)
        {
            this.Error = ex6.Message;
            return Result.RESPONSE_TIMEOUT;
        }
        catch (Exception ex7)
        {
            this.Error = ex7.Message;
            return Result.READ;
        }
        finally
        {
            this._rxBufSize = num;
            Array.Copy(array2, this._rxBuf, this._rxBufSize);
        }

        if (!Ascii.VerifyRespLRC(array2, num)) return Result.CRC;
        if (array2[num - 2] != 13 || array2[num - 1] != 10) return Result.RESPONSE;
        var num6 = (num - 5) / 2;
        for (var i = 0; i < num6; i++)
            RXBuf[i] = Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(array2[1 + i * 2]), Ascii.Ascii2Num(array2[2 + i * 2]));
        return Result.SUCCESS;
    }

    private Result TxRxRTU(byte[] TXBuf, int QueryLength, byte[] RXBuf, int ResponseLength)
    {
        var num = 0;
        var array = CRC16.GenereatCRC16(TXBuf, QueryLength);
        TXBuf[QueryLength] = array[0];
        TXBuf[QueryLength + 1] = array[1];
        this.port.DiscardInBuffer();
        this.port.DiscardOutBuffer();
        try
        {
            this.port.Write(TXBuf, 0, QueryLength + 2);
        }
        catch (Exception ex)
        {
            this.Error = ex.Message;
            return Result.WRITE;
        }

        this._txBufSize = QueryLength + 2;
        Array.Copy(TXBuf, this._txBuf, this._txBufSize);
        if (this._removeEcho)
        {
            try
            {
                do
                {
                    var num2 = this.port.Read(RXBuf, 0, this._txBufSize - num);
                    num += num2;
                }
                while (this._txBufSize - num > 0);
            }
            catch (TimeoutException ex2)
            {
                this.Error = ex2.Message;
                return Result.RESPONSE_TIMEOUT;
            }
            catch (Exception ex3)
            {
                this.Error = ex3.Message;
                return Result.READ;
            }

            num = 0;
        }

        if (TXBuf[0] == 0) return Result.SUCCESS;
        try
        {
            do
            {
                var num3 = this.port.Read(RXBuf, num, 5 - num);
                num += num3;
            }
            while (5 - num > 0);
        }
        catch (TimeoutException ex4)
        {
            this.Error = ex4.Message;
            return Result.RESPONSE_TIMEOUT;
        }
        catch (Exception ex5)
        {
            this.Error = ex5.Message;
            return Result.READ;
        }
        finally
        {
            this._rxBufSize = num;
            Array.Copy(RXBuf, this._rxBuf, this._rxBufSize);
        }

        if (RXBuf[1] > 128)
        {
            var array2 = CRC16.GenereatCRC16(RXBuf, 5);
            if (array2[0] != 0 || array2[1] != 0) return Result.CRC;
            return (Result)RXBuf[2];
        }

        if (ResponseLength == int.MaxValue)
        {
            if (RXBuf[1] != 17) return Result.RESPONSE;
            ResponseLength = RXBuf[2] + 3;
        }

        try
        {
            var num4 = ResponseLength + 2;
            do
            {
                var num5 = this.port.Read(RXBuf, num, num4 - num);
                num += num5;
            }
            while (num4 - num > 0);
        }
        catch (TimeoutException ex6)
        {
            this.Error = ex6.Message;
            return Result.RESPONSE_TIMEOUT;
        }
        catch (Exception ex7)
        {
            this.Error = ex7.Message;
            return Result.READ;
        }
        finally
        {
            this._rxBufSize = num;
            Array.Copy(RXBuf, this._rxBuf, this._rxBufSize);
        }

        var array3 = CRC16.GenereatCRC16(RXBuf, ResponseLength + 2);
        if (array3[0] != 0 || array3[1] != 0) return Result.CRC;
        return Result.SUCCESS;
    }
}