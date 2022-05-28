using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace final.ViewModel
{
    public class KayitModel
    {
        public string kayitId { get; set; }
        public string kayitKulId { get; set; }
        public string kayitDosyaId { get; set; }

        public KullaniciModel kulBilgi { get; set; }
        public DosyaModel dosBilgi { get; set; }
    }
}