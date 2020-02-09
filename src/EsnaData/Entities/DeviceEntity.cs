using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EsnaData.Entities
{
    [Table(nameof(Configuration))]
    public class Configuration : BaseEntity<long>
    {
        public int BaudRate { get; set; }

        public int Timeout { get; set; }

        public int DataBits { get; set; }

        public int Parity { get; set; }

        public string PortName { get; set; }

        public int StopBits { get; set; }

        public int Mode { get; set; }

        public bool Active { get; set; }

        public virtual IEnumerable<Recorde> Recordes { get; set; }
    }

    [Table(nameof(Device))]
    public class Device : BaseEntity<long>
    {
        public string MacAddress { get; set; }

        public byte FirstRegister { get; set; }

        public byte Offset { get; set; }

        public byte UnitId { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

        public string ExteraInfornamtion { get; set; }

        [JsonIgnore]
        public IList<Recorde> Recordes { get; set; }


    }



    [Table(nameof(Recorde))]
    public class Recorde : BaseEntity<long>
    {
        public virtual Device Device { get; set; }

        public long DeviceId { get; set; }

        public byte[] Data { get; set; }

        [NotMapped, JsonIgnore]
        public short[] ShortData
        {
            get
            {
                short[] data = new short[Data.Length / sizeof(short)];
                Buffer.BlockCopy(Data, 0, data, 0, Data.Length);
                return data;
            }
        }
    }

    [Table(nameof(Command))]
    public class Command : BaseEntity<long>
    {
        public virtual Device Device { get; set; }

        public long DeviceId { get; set; }

        public byte Function { get; set; }

        public bool Executed { get; set; }

        public bool ExecutedOnUtc { get; set; }

        public byte[] Data { get; set; }
    }

    public abstract class BaseEntity<T>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }

        public DateTime CreatedOnUtc { get; set; }


    }

    public static class BaseEntityExtentions
    {
        public static string AsJson<T>(this T entity)
            where T : BaseEntity<long>
        {
            return JsonSerializer.Serialize(entity, new JsonSerializerOptions() { });
        }
    }
}
