namespace EsnaData.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Command))]
    public class Command : BaseEntity<long>
    {
        public byte[] Data { get; set; }

        public virtual Device Device { get; set; }

        public long DeviceId { get; set; }

        public bool Executed { get; set; }

        public bool ExecutedOnUtc { get; set; }

        public byte Function { get; set; }
    }
}