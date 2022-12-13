using Core.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Ogis10 : IEntity
    {
        [Key]
        public string BolumKodu { get; set; }
        public string AdiTr { get; set; }
        public string AdiIng { get; set; }
        public string Un1 { get; set; }
        public string Un2 { get; set; }
        public string Un3 { get; set; }
        public string BolumYili { get; set; }
        public string Aktif { get; set; }
        public string BolumDili { get; set; }
        public DateTime MebOnayTarihi { get; set; }
        public DateTime YodakOnayTarihi { get; set; }
        public DateTime YokOnayTarihi { get; set; }
        public string FakulteKodu { get; set; }


    }
}
