namespace EsnaMonitoring.Services.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO.Ports;
    using global::AutoMapper;

    using ModbusUtility;

    using Parity = System.IO.Ports.Parity;

    // [AutoMap(typeof(Configuration), ReverseMap = true)]
    public class ConfigurationModel
    {
        public long Id { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public int BaudRate { get; set; }

        [Required]
        public int DataBits { get; set; }

        [Required]
        public Mode Mode { get; set; }

        [Required]
        public Parity Parity { get; set; }

        [Required]
        public string PortName { get; set; }

        [Required]
        public StopBits StopBits { get; set; }

        [Required]
        public Timeout Timeout { get; set; }
    }
}