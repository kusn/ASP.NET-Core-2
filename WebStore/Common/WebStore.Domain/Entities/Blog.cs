using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    public class Blog : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
    }
}
