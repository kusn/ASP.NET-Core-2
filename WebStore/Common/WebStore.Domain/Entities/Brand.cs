using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Domain.Entities
{
    [Index(nameof(Name)/*, nameof(Order)*/, IsUnique = true)]
    //[Table("Brandsss")]
    public class Brand : NamedEntity, IOrderedEntity
    {
        //[Column("BrandOrder")]
        public int Order { get; set; }
    }
}
