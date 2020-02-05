// ModbusMasterLib.CTxRx
using System;
using System.IO.Ports;
using System.Threading;
using ModbusUtility;

internal class CTxRx
{
    private readonly SerialPort port;
    private string Error = "";
    private int Time;
    private readonly byte[] _txBuf = new byte[600];
    private readonly byte[] _rxBuf = new byte[600];
    private int _txBufSize;
    private int _rxBufSize;
    private readonly bool _removeEcho;

    public Mode Mode { get; set; }

    public CTxRx(SerialPort _port)
    {
        port = _port;
    }

    public string GetErrorMessage()
    {
        return Error;
    }

    public int GetTxBuffer(byte[] byteArray)
    {
        if (byteArray.GetLength(0) >= _txBufSize)
        {
            Array.Copy(_txBuf, byteArray, _txBufSize);
            return _txBufSize;
        }
        return 0;
    }

    public int GetRxBuffer(byte[] byteArray)
    {
        if (byteArray.GetLength(0) >= _rxBufSize)
        {
            Array.Copy(_rxBuf, byteArray, _rxBufSize);
            return _rxBufSize;
        }
        return 0;
    }

    public Result TxRx(byte[] TXBuf, int QueryLength, byte[] RXBuf, int ResponseLength)
    {
        int num = Environment.TickCount & int.MaxValue;
        _txBufSize = 0;
        _rxBufSize = 0;
       
        if (num - Time < 20)
        {
            Thread.Sleep(4);
        }
        if (!port.IsOpen)
        {
            return Result.ISCLOSED;
        }
        var result = Mode switch
        {
            Mode.RTU => TxRxRTU(TXBuf, QueryLength, RXBuf, ResponseLength),
            Mode.ASCII => TxRxAscii(TXBuf, QueryLength, RXBuf, ResponseLength),
            _ => Result.SUCCESS,
        };
        Time = Environment.TickCount;
        return result;
    }

    private Result TxRxRTU(byte[] TXBuf, int QueryLength, byte[] RXBuf, int ResponseLength)
    {
        int num = 0;
        byte[] array = CRC16.GenereatCRC16(TXBuf, QueryLength);
        TXBuf[QueryLength] = array[0];
        TXBuf[QueryLength + 1] = array[1];
        port.DiscardInBuffer();
        port.DiscardOutBuffer();
        try
        {
            port.Write(TXBuf, 0, QueryLength + 2);
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            return Result.WRITE;
        }
        _txBufSize = QueryLength + 2;
        Array.Copy(TXBuf, _txBuf, _txBufSize);
        if (_removeEcho)
        {
            try
            {
                do
                {
                    int num2 = port.Read(RXBuf, 0, _txBufSize - num);
                    num += num2;
                }
                while (_txBufSize - num > 0);
            }
            catch (TimeoutException ex2)
            {
                Error = ex2.Message;
                return Result.RESPONSE_TIMEOUT;
            }
            catch (Exception ex3)
            {
                Error = ex3.Message;
                return Result.READ;
            }
            num = 0;
        }
        if (TXBuf[0] == 0)
        {
            return Result.SUCCESS;
        }
        try
        {
            do
            {
                int num3 = port.Read(RXBuf, num, 5 - num);
                num += num3;
            }
            while (5 - num > 0);
        }
        catch (TimeoutException ex4)
        {
            Error = ex4.Message;
            return Result.RESPONSE_TIMEOUT;
        }
        catch (Exception ex5)
        {
            Error = ex5.Message;
            return Result.READ;
        }
        finally
        {
            _rxBufSize = num;
            Array.Copy(RXBuf, _rxBuf, _rxBufSize);
        }
        if (RXBuf[1] > 128)
        {
            byte[] array2 = CRC16.GenereatCRC16(RXBuf, 5);
            if (array2[0] != 0 || array2[1] != 0)
            {
                return Result.CRC;
            }
            return (Result)RXBuf[2];
        }
        if (ResponseLength == int.MaxValue)
        {
            if (RXBuf[1] != 17)
            {
                return Result.RESPONSE;
            }
            ResponseLength = RXBuf[2] + 3;
        }
        try
        {
            int num4 = ResponseLength + 2;
            do
            {
                int num5 = port.Read(RXBuf, num, num4 - num);
                num += num5;
            }
            while (num4 - num > 0);
        }
        catch (TimeoutException ex6)
        {
            Error = ex6.Message;
            return Result.RESPONSE_TIMEOUT;
        }
        catch (Exception ex7)
        {
            Error = ex7.Message;
            return Result.READ;
        }
        finally
        {
            _rxBufSize = num;
            Array.Copy(RXBuf, _rxBuf, _rxBufSize);
        }
        byte[] array3 = CRC16.GenereatCRC16(RXBuf, ResponseLength + 2);
        if (array3[0] != 0 || array3[1] != 0)
        {
            return Result.CRC;
        }
        return Result.SUCCESS;
    }

    private Result TxRxAscii(byte[] TXBuf, int QueryLength, byte[] RXBuf, int ResponseLength)
    {
        int num = 0;
        byte[] array = new byte[531];
        byte[] array2 = new byte[523];
        Ascii.RTU2ASCII(TXBuf, QueryLength, array);
        byte n = Ascii.LRC(TXBuf, QueryLength);
        array[0] = 58;
        array[QueryLength * 2 + 1] = Ascii.Num2Ascii(ByteAccess.HI4BITS(n));
        array[QueryLength * 2 + 2] = Ascii.Num2Ascii(ByteAccess.LO4BITS(n));
        array[QueryLength * 2 + 3] = 13;
        array[QueryLength * 2 + 4] = 10;
        port.DiscardInBuffer();
        port.DiscardOutBuffer();
        try
        {
            port.Write(array, 0, QueryLength * 2 + 5);
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            return Result.WRITE;
        }
        _txBufSize = QueryLength * 2 + 5;
        Array.Copy(array, _txBuf, _txBufSize);
        if (TXBuf[0] == 0)
        {
            return Result.SUCCESS;
        }
        if (_removeEcho)
        {
            try
            {
                do
                {
                    int num2 = port.Read(RXBuf, 0, _txBufSize - num);
                    num += num2;
                }
                while (_txBufSize - num > 0);
            }
            catch (TimeoutException ex2)
            {
                Error = ex2.Message;
                return Result.RESPONSE_TIMEOUT;
            }
            catch (Exception ex3)
            {
                Error = ex3.Message;
                return Result.READ;
            }
            num = 0;
        }
        try
        {
            do
            {
                int num3 = port.Read(array2, num, 11 - num);
                num += num3;
            }
            while (11 - num > 0);
        }
        catch (TimeoutException ex4)
        {
            Error = ex4.Message;
            return Result.RESPONSE_TIMEOUT;
        }
        catch (Exception ex5)
        {
            Error = ex5.Message;
            return Result.READ;
        }
        finally
        {
            _rxBufSize = num;
            Array.Copy(array2, _rxBuf, _rxBufSize);
        }
        if (Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(array2[3]), Ascii.Ascii2Num(array2[4])) > 128)
        {
            if (!Ascii.VerifyRespLRC(array2, 11))
            {
                return Result.CRC;
            }
            return (Result)Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(array2[5]), Ascii.Ascii2Num(array2[6]));
        }
        if (ResponseLength == int.MaxValue)
        {
            if (Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(array2[3]), Ascii.Ascii2Num(array2[4])) != 17)
            {
                return Result.RESPONSE;
            }
            ResponseLength = Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(array2[5]), Ascii.Ascii2Num(array2[6])) + 3;
        }
        try
        {
            int num4 = ResponseLength * 2 + 5;
            do
            {
                int num5 = port.Read(array2, num, num4 - num);
                num += num5;
            }
            while (num4 - num > 0);
        }
        catch (TimeoutException ex6)
        {
            Error = ex6.Message;
            return Result.RESPONSE_TIMEOUT;
        }
        catch (Exception ex7)
        {
            Error = ex7.Message;
            return Result.READ;
        }
        finally
        {
            _rxBufSize = num;
            Array.Copy(array2, _rxBuf, _rxBufSize);
        }
        if (!Ascii.VerifyRespLRC(array2, num))
        {
            return Result.CRC;
        }
        if (array2[num - 2] != 13 || array2[num - 1] != 10)
        {
            return Result.RESPONSE;
        }
        int num6 = (num - 5) / 2;
        for (int i = 0; i < num6; i++)
        {
            RXBuf[i] = Ascii.HiLo4BitsToByte(Ascii.Ascii2Num(array2[1 + i * 2]), Ascii.Ascii2Num(array2[2 + i * 2]));
        }
        return Result.SUCCESS;
    }
}
