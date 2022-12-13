using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfStudentDal : EfEntityRepositoryBase<Ogis01, ContextDb>,
       IStudentDal
    {
        public List<Student> GetListDto(string sOgrenciNo)
        {
            using (var context = new ContextDb())
            {
                var result = from Ogis01 in context.Ogis01.Where(x => x.Ogrno == sOgrenciNo)
                             join Ogis09 in context.Ogis09 on Ogis01.Fakulte equals Ogis09.FakulteKodu
                             join Ogis10 in context.Ogis10 on Ogis01.Bolum equals Ogis10.BolumKodu
                             select new Student
                             {
                                Ogrno = Ogis01.Ogrno,
                                Adi = Ogis01.Adi,
                                Soyadi = Ogis01.Soyadi,
                                BabaAdi = Ogis01.BabaAdi,
                                Anneadi = Ogis01.Anneadi,
                                DogumTarihi = Ogis01.DogumTarihi,
                                DogumYeri = Ogis01.DogumYeri,
                                Uyruk = Ogis01.Uyruk,
                                Cinsiyet = Ogis01.Cinsiyet,
                                KayitYili = Ogis01.KayitYili,
                                KayitDonemi = Ogis01.KayitDonemi,
                                BabaMeslek = Ogis01.BabaMeslek,
                                FakulteIng = Ogis09.FakulteadiIng,
                                FakulteTr = Ogis09.FakulteAdiTr,
                                BolumIng = Ogis10.AdiIng,
                                BolumTr = Ogis10.AdiTr,
                                KayitTarihi = Ogis01.KayitTarihi,
                                Sinif = Ogis01.Sinif,
                                AktifDurum = Ogis01.AktifDurum,
                                PasaportNo = Ogis01.PasaportNo,
                                KimlikNo = Ogis01.KimlikNo
                             };
                return result.ToList();
            }
        }
    }
}
