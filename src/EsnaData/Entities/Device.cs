namespace EsnaData.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    [Table(nameof(Device))]
    public class Device : BaseEntity<long>
    {
        public string Code { get; set; }

        public string ExteraInfornamtion { get; set; }

        public byte FirstRegister { get; set; }

        public bool IsActive { get; set; }

        public string MacAddress { get; set; }

        public byte Offset { get; set; }

        [JsonIgnore]
        public IList<Recorde> Recordes { get; set; }

        public byte UnitId { get; set; }
    }
}