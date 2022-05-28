using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using final.Models;
using final.ViewModel;

namespace final.Controllers
{
    public class ServisController : ApiController
    {
        DB02Entities1 db = new DB02Entities1();
        SonucModel sonuc = new SonucModel();

        #region Kullanici

        [HttpGet]
        [Route("api/kullaniciliste")]
        public List<KullaniciModel> KullaniciListe()
        {
            List<KullaniciModel> liste = db.Kullanici.Select(x => new KullaniciModel()
            { 
                KulAdSoyad = x.KulAdSoyad,
                KulMail = x.KulMail,
                KulDogTarihi = x.KulDogTarihi, 
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/adminkullaniciliste")]
        public List<KullaniciModel> AdminKullaniciListe()
        {
            List<KullaniciModel> liste = db.Kullanici.Select(x => new KullaniciModel()
            {
                KulId = x.KulId,
                KulAdSoyad = x.KulAdSoyad,
                KulMail = x.KulMail,
                KulDogTarihi = x.KulDogTarihi,
                KulSifre = x.KulSifre, 
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/kullanicibyid/{KulId}")]
        public KullaniciModel KullaniciById(string KulId)
        {
            KullaniciModel kayit = db.Kullanici.Where(s => s.KulId == KulId).Select(x => new KullaniciModel()
            {
                KulId = x.KulId,
                KulAdSoyad = x.KulAdSoyad,
                KulMail = x.KulMail,
                KulDogTarihi = x.KulDogTarihi, 
            }).SingleOrDefault();
            return kayit;
        }


        [HttpPost]
        [Route("api/kullaniciekle")]
        public SonucModel KullaniciEkle(KullaniciModel model)
        {
             

            Kullanici yeni = new Kullanici();
            yeni.KulId = Guid.NewGuid().ToString();
            yeni.KulAdSoyad = model.KulAdSoyad;
            yeni.KulMail = model.KulMail;
            yeni.KulSifre = model.KulSifre;
            yeni.KulDogTarihi = model.KulDogTarihi;
            db.Kullanici.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kullanici Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kullaniciduzenle")]
        public SonucModel KullaniciDuzenle(KullaniciModel model)
        {
            Kullanici kayit = db.Kullanici.Where(s => s.KulId == model.KulId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunmadı!";
                return sonuc;
            }

            kayit.KulAdSoyad = model.KulAdSoyad;
            kayit.KulMail = model.KulMail;
            kayit.KulDogTarihi = model.KulDogTarihi;
            kayit.KulSifre = model.KulSifre; 

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kullanicisil/{KulId}")]
        public SonucModel KullaniciSil(string KulId)
        {
            Kullanici kayit = db.Kullanici.Where(s => s.KulId == KulId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunmadı!";
                return sonuc;
            }

            if (db.Kayit.Count(c => c.kayitKulId == KulId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Dosyası olan Kullanıcı Silinemez!";
                return sonuc;
            }


            db.Kullanici.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı Silndi";
            return sonuc;
        }

         

       











        #endregion

        #region Dosya

        [HttpGet]
        [Route("api/dosyaliste")]
        public List<DosyaModel> DosyaListe()
        {
            List<DosyaModel> liste = db.Dosya.Select(x => new DosyaModel()
            {
                dosyaId = x.dosyaId,
                dosyaAdı = x.dosyaAdı,
                dosyaIcerigi = x.dosyaIcerigi,
                dosyaTuru = x.dosyaTuru,
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/dosyabyid/{dosyaId}")]
        public DosyaModel DosyaById(string dosyaId)
        {
            DosyaModel kayit = db.Dosya.Where(s => s.dosyaId == dosyaId).Select(x => new DosyaModel()
            {
                dosyaId = x.dosyaId,
                dosyaAdı = x.dosyaAdı,
                dosyaIcerigi = x.dosyaIcerigi,
                dosyaTuru = x.dosyaTuru,
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/dosyaekle")]
        public SonucModel DosyaEkle(DosyaModel model)
        {
            if (db.Dosya.Count(c => c.dosyaIcerigi == model.dosyaIcerigi) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Dosya Kayıtlıdır!";
                return sonuc;
            }

            Dosya yeni = new Dosya();
            yeni.dosyaId = Guid.NewGuid().ToString();
            yeni.dosyaAdı = model.dosyaAdı;
            yeni.dosyaIcerigi = model.dosyaIcerigi;
            yeni.dosyaTuru = model.dosyaTuru;
            db.Dosya.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Dosya Eklendi";

            return sonuc;
        }





        [HttpPut]
        [Route("api/dosyaduzenle")]
        public SonucModel DosyaDuzenle(DosyaModel model)
        {
            Dosya kayit = db.Dosya.Where(s => s.dosyaId == model.dosyaId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Dosya Bulunamadı!";
                return sonuc;
            }

            kayit.dosyaAdı = model.dosyaAdı;
            kayit.dosyaIcerigi = model.dosyaIcerigi;
            kayit.dosyaTuru = model.dosyaTuru;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Dosya Düzenlendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/dosyasil/{dosyaId}")]
        public SonucModel DosyaSil(string dosyaId)
        {
            Dosya kayit = db.Dosya.Where(s => s.dosyaId == dosyaId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }


            db.Dosya.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Dosya Silindi";

            return sonuc;
        }

        #endregion

        #region Kayit
        [HttpGet]
        [Route("api/profil/{KulId}")]
        public List<KayitModel> KullaniciDosyaListe(string KulId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitKulId == KulId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitDosyaId = x.kayitDosyaId,
                kayitKulId = x.kayitKulId
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.kulBilgi = KullaniciById(kayit.kayitKulId);
                kayit.dosBilgi = DosyaById(kayit.kayitDosyaId);

            }
            return liste;

        }

        #endregion


        #region Kayit


        [HttpPost]
        [Route("api/kayitekle")]
        public SonucModel KayitEkle(KayitModel model)
        {
            if (db.Kayit.Count(c => c.kayitDosyaId == model.kayitDosyaId & c.kayitKulId == model.kayitKulId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kullanıcı daha önce kaydoldu!";
                return sonuc;
            }

            Kayit yeni = new Kayit();
            yeni.kayitId = Guid.NewGuid().ToString();
            yeni.kayitKulId = model.kayitKulId;
            yeni.kayitDosyaId = model.kayitDosyaId;
            db.Kayit.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kayıt Eklendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kayitsil/{kayitId}")]
        public SonucModel KayitSil(string kayitId)
        {
            Kayit kayit = db.Kayit.Where(s => s.kayitId == kayitId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }


            db.Kayit.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kayıt Silindi";

            return sonuc;
        }


        #endregion
    }
}
