namespace EsnaData.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    [Table(nameof(Recorde))]
    public class Recorde : BaseEntity<long>
    {
        public byte[] Data { get; set; }

        public virtual Device Device { get; set; }

        public long DeviceId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public short[] ShortData
        {
            get
            {
                var data = new short[this.Data.Length / sizeof(short)];
                Buffer.BlockCopy(this.Data, 0, data, 0, this.Data.Length);
                return data;
            }
        }
    }
}