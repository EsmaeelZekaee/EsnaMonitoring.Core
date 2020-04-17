namespace EsnaData.Entities
{
    using System.Text.Json;

    public static class BaseEntityExtentions
    {
        public static string AsJson<T>(this T entity)
            where T : BaseEntity<long>
        {
            return JsonSerializer.Serialize(entity, new JsonSerializerOptions());
        }
    }
}