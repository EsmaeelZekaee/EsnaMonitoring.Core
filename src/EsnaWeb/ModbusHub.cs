using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsnaWeb
{
    public class ModbusHub : Hub<ISendMessage>
    {
        public ModbusHub()
        {

        }
        public async Task SendMessage()
        {
            await Clients.All.SendMessage(0, "AU-4W-2", "12345", Array.Empty<short>());
        }
    }

    public interface ISendMessage
    {
        Task SendMessage(byte unitId, string code, string macAddress, short[] data);
    }
}
