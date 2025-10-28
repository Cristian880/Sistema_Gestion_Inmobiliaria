namespace Sis_Inmobiliaria.Core.Domain.Common
{
    public class BasicEntity<TKey>
    {
        public required TKey Id { get; set; }
    }
}
