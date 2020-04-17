namespace EsnaData.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Configuration))]
    public class Configuration : BaseEntity<long>
    {
        public bool Active { get; set; }

        public int BaudRate { get; set; }

        public int DataBits { get; set; }

        public int Mode { get; set; }

        public int Parity { get; set; }

        public string PortName { get; set; }

        public virtual IEnumerable<Recorde> Recordes { get; set; }

        public int StopBits { get; set; }

        public int Timeout { get; set; }
    }
}