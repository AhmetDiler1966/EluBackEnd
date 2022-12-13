using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Student : IEntity
    {
        public string Ogrno { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string KayitYili { get; set; }
        public string KayitDonemi { get; set; }
        public string BabaAdi { get; set; }
        public string Anneadi { get; set; }
        public string DogumYeri { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string Uyruk { get; set; }
        public string Cinsiyet { get; set; }
        public string BabaMeslek { get; set; }
        public string FakulteIng { get; set; }
        public string FakulteTr { get; set; }
        public string BolumIng { get; set; }
        public string BolumTr { get; set; }
        public string Sinif { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string AktifDurum { get; set; }
        public string PasaportNo { get; set; }
        public string KimlikNo { get; set; }
    }
}
