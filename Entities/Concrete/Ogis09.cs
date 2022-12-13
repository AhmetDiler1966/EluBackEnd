using Core.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Ogis09 : IEntity
    {
        // [FakulteKodu], [FakulteAdiTr], [FakulteadiIng], [MebOnayTarihi], [YodakOnayTarihi], [YokOnayTarihi]
        [Key]
        public string FakulteKodu { get; set; }
        public string FakulteAdiTr { get; set; }
        public string FakulteadiIng { get; set; }
        public DateTime MebOnayTarihi { get; set; }
        public DateTime YodakOnayTarihi { get; set; }
        public DateTime YokOnayTarihi { get; set; }
    }
}
