using System;
using System.Collections.Generic;

class Program
{
    static Dictionary<string, string> kullaniciVeritabani = new Dictionary<string, string>
    {
        { "kat1-1", "sifre1" },
        { "kat1-2", "sifre2" },
        // Diğer kullanıcılar...
        { "yonetici", "yonetici" }
    };

    static Dictionary<string, string> rolVeritabani = new Dictionary<string, string>
    {
        { "kat1-1", "katmaliki" },
        { "kat1-2", "katmaliki" },
        // Diğer roller...
        { "yonetici", "yonetici" }
    };

    static List<Daire> daireler = new List<Daire>();

    static void Main()
    {
        // Dairelerin oluşturulması
        for (int kat = 1; kat <= 8; kat++)
        {
            for (int daireNo = 1; daireNo <= 4; daireNo++)
            {
                Daire daire = new Daire
                {
                    Kat = kat,
                    DaireNo = daireNo,
                    Aidat = 125m,
                    OdendiMi = false,
                    Sikayet = "",
                    Sahip = "",
                    Rol = ""
                };

                daireler.Add(daire);
            }
        }

        bool cikis = false;

        while (!cikis)
        {
            Console.WriteLine("Kullanıcı Girişi");
            Console.WriteLine("----------------");

            string? girilenKullaniciAdi = null;
            string? girilenSifre = null;

            bool girisBasarili = false;

            while (!girisBasarili)
            {
                Console.Write("Kullanıcı Adı: ");
                girilenKullaniciAdi = Console.ReadLine();

                Console.Write("Şifre: ");
                girilenSifre = Console.ReadLine();

                if (KimlikDogrula(girilenKullaniciAdi, girilenSifre))
                {
                    girisBasarili = true;
                }
                else
                {
                    Console.WriteLine("Kullanıcı adı veya şifre hatalı! Tekrar deneyin.");
                }
            }

            string kullaniciRol = RolBilgisiniGetir(girilenKullaniciAdi);

            if (kullaniciRol == "katmaliki")
            {
                Console.WriteLine("Kat malikine özel ayrıcalıklı erişim sağlandı!");
                KatMalikiMenu(girilenKullaniciAdi);
            }
            else if (kullaniciRol == "kiraci")
            {
                Console.WriteLine("Kiracıya özel ayrıcalıklı erişim sağlandı!");
                KiraciMenu(girilenKullaniciAdi);
            }
            else if (kullaniciRol == "yonetici")
            {
                Console.WriteLine("Yöneticiye özel ayrıcalıklı erişim sağlandı!");
                YoneticiMenu();
            }
            else
            {
                Console.WriteLine("Bilinmeyen bir kullanıcı rolüne sahipsiniz. Çıkış yapılıyor...");
            }

            Console.WriteLine("Devam etmek için bir tuşa basın, çıkmak için 'Q' tuşuna basın.");
            if (Console.ReadKey().Key == ConsoleKey.Q)
            {
                cikis = true;
            }
        }
    }

    static bool KimlikDogrula(string kullaniciAdi, string sifre)
    {
        if (kullaniciVeritabani.ContainsKey(kullaniciAdi))
        {
            string dogruSifre = kullaniciVeritabani[kullaniciAdi];
            return sifre == dogruSifre;
        }

        return false;
    }

    static string RolBilgisiniGetir(string kullaniciAdi)
    {
        if (rolVeritabani.ContainsKey(kullaniciAdi))
        {
            return rolVeritabani[kullaniciAdi];
        }

        return "";
    }

    static void AidatBilgileriniGoruntule(string kullaniciAdi)
    {
        Daire kullaniciDairesi = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == kullaniciAdi);

        if (kullaniciDairesi != null)
        {
            Console.WriteLine("Aidat Bilgileri");
            Console.WriteLine("----------------");

            Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-15}", "Daire", "Sahip", "Aidat", "Ödeme Durumu");
            Console.WriteLine("-----------------------------------------------");

            Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-15}", $"{kullaniciDairesi.Kat}/{kullaniciDairesi.DaireNo}",
                kullaniciDairesi.Sahip, kullaniciDairesi.Aidat.ToString()+" TL", kullaniciDairesi.OdendiMi ? "Ödendi" : "Ödenmedi");
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı.");
        }
    }

    static void OdemeYap()
    {
        Console.WriteLine("Aidat Ödeme");
        Console.WriteLine("-----------");

        Console.Write("Daireyi girin (örn. 1-1): ");
        string daireAdi = Console.ReadLine();

        Daire odemeYapilanDaire = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == daireAdi);

        if (odemeYapilanDaire != null)
        {
            if (!odemeYapilanDaire.OdendiMi)
            {
                Console.WriteLine("Aidat miktarı: {0}", odemeYapilanDaire.Aidat);
                Console.Write("Ödeme yapmak istiyor musunuz? (E/H): ");
                string cevap = Console.ReadLine();

                if (cevap.ToLower() == "e")
                {
                    odemeYapilanDaire.OdendiMi = true;
                    Console.WriteLine("Ödeme işlemi başarıyla gerçekleştirildi.");
                }
                else
                {
                    Console.WriteLine("Ödeme işlemi iptal edildi.");
                }
            }
            else
            {
                Console.WriteLine("Bu daire için daha önce ödeme yapılmış.");
            }
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı. Ödeme işlemi gerçekleştirilemedi.");
        }
    }

    static void SikayetYap(string kullaniciAdi)
    {
        Console.WriteLine("Şikayet Yap");
        Console.WriteLine("-----------");

        Console.Write("Şikayetinizi girin: ");
        string sikayet = Console.ReadLine();

        Daire kullaniciDairesi = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == kullaniciAdi);

        if (kullaniciDairesi != null)
        {
            kullaniciDairesi.Sikayet = sikayet;
            Console.WriteLine("Şikayetiniz kaydedildi.");
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı. Şikayet kaydedilemedi.");
        }
    }

    static void DaireSahibiEkle()
    {
        Console.WriteLine("Daire Sahibi Ekle");
        Console.WriteLine("-----------------");

        Console.Write("Daireyi girin (örn. 1-1): ");
        string daireAdi = Console.ReadLine();

        Daire secilenDaire = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == daireAdi);

        if (secilenDaire != null)
        {
            if (string.IsNullOrEmpty(secilenDaire.Sahip))
            {
                Console.Write("Daire sahibinin adını ve soyadını girin: ");
                string sahipAdi = Console.ReadLine();

                secilenDaire.Sahip = sahipAdi;
                secilenDaire.Rol = "Sahip"; // Sahip rolünü atama

                Console.WriteLine("Daire sahibi başarıyla eklendi.");
            }
            else
            {
                Console.WriteLine("Bu daireye zaten bir sahip atanmış.");
            }
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı. Daire sahibi eklenemedi.");
        }
    }


    static void DaireSahibiSil()
    {
        Console.WriteLine("Daire Sahibi Sil");
        Console.WriteLine("----------------");

        Console.Write("Daireyi girin (örn. 1-1): ");
        string daireAdi = Console.ReadLine();

        Daire secilenDaire = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == daireAdi);

        if (secilenDaire != null)
        {
            if (!string.IsNullOrEmpty(secilenDaire.Sahip))
            {
                Console.WriteLine("Daire Sahibi: {0}", secilenDaire.Sahip);
                Console.Write("Daire sahibini silmek istiyor musunuz? (E/H): ");
                string cevap = Console.ReadLine();

                if (cevap.ToLower() == "e")
                {
                    secilenDaire.Sahip = "";
                    secilenDaire.Rol = ""; // Rolü sıfırlama

                    Console.WriteLine("Daire sahibi başarıyla silindi.");
                }
                else
                {
                    Console.WriteLine("Daire sahibi silme işlemi iptal edildi.");
                }
            }
            else
            {
                Console.WriteLine("Bu daireye zaten bir sahip atanmamış.");
            }
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı. Daire sahibi silinemedi.");
        }
    }


    static void DaireSahibiDegistir()
    {
        Console.WriteLine("Daire Sahibi Değiştir");
        Console.WriteLine("---------------------");

        Console.Write("Daireyi girin (örn. 1-1): ");
        string daireAdi = Console.ReadLine();

        Daire secilenDaire = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == daireAdi);

        if (secilenDaire != null)
        {
            if (!string.IsNullOrEmpty(secilenDaire.Sahip))
            {
                Console.WriteLine("Daire Sahibi: {0}", secilenDaire.Sahip);
                Console.Write("Yeni daire sahibinin adını ve soyadını girin: ");
                string yeniSahipAdi = Console.ReadLine();

                secilenDaire.Sahip = yeniSahipAdi;

                Console.WriteLine("Daire sahibi başarıyla değiştirildi.");
            }
            else
            {
                Console.WriteLine("Bu daireye zaten bir sahip atanmamış.");
            }
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı. Daire sahibi değiştirilemedi.");
        }
    }

    static void KatMalikiMenu(string kullaniciAdi)
    {
        bool cikis = false;

        while (!cikis)
        {
            Console.WriteLine();
            Console.WriteLine("Kat Maliki Menüsü");
            Console.WriteLine("-----------------");
            Console.WriteLine("1. Aidat Bilgilerini Görüntüle");
            Console.WriteLine("2. Aidat Ödeme");
            Console.WriteLine("3. Şikayet Yap");
            Console.WriteLine("4. Çıkış");

            Console.Write("Seçiminizi yapın (1-4): ");
            string secim = Console.ReadLine();

            switch (secim)
            {
                case "1":
                    AidatBilgileriniGoruntule(kullaniciAdi);
                    break;
                case "2":
                    OdemeYap();
                    break;
                case "3":
                    SikayetYap(kullaniciAdi);
                    break;
                case "4":
                    cikis = true;
                    break;
                default:
                    Console.WriteLine("Geçersiz seçim! Tekrar deneyin.");
                    break;
            }
        }
    }

    static void KiraciMenu(string kullaniciAdi)
    {
        bool cikis = false;

        while (!cikis)
        {
            Console.WriteLine();
            Console.WriteLine("Kiracı Menüsü");
            Console.WriteLine("--------------");
            Console.WriteLine("1. Şikayet Yap");
            Console.WriteLine("2. Çıkış");

            Console.Write("Seçiminizi yapın (1-2): ");
            string secim = Console.ReadLine();

            switch (secim)
            {
                case "1":
                    SikayetYap(kullaniciAdi);
                    break;
                case "2":
                    cikis = true;
                    break;
                default:
                    Console.WriteLine("Geçersiz seçim! Tekrar deneyin.");
                    break;
            }
        }
    }

    static void YoneticiMenu()
    {
        bool cikis = false;

        while (!cikis)
        {
            Console.WriteLine();
            Console.WriteLine("Yönetici Menüsü");
            Console.WriteLine("---------------");
            Console.WriteLine("1. Aidat Bilgilerini Görüntüle");
            Console.WriteLine("2. Şikayet Yap");
            Console.WriteLine("3. Kat Maliki Ekle");
            Console.WriteLine("4. Kat Maliki Sil");
            Console.WriteLine("5. Kiracı Ekle");
            Console.WriteLine("6. Kiracı Sil");
            Console.WriteLine("7. Çıkış");

            Console.Write("Seçiminizi yapın (1-11): ");
            string secim = Console.ReadLine();

            switch (secim)
            {
                case "1":
                    AidatBilgileriniGoruntuleYonetici();
                    break;              
                case "2":
                    SikayetYapYonetici();
                    break;
                case "3":
                    KatMalikiEkle();
                    break;
                case "4":
                    KatMalikiSil();
                    break;
                case "5":
                    KiraciEkle();
                    break;
                case "6":
                    KiraciSil();
                    break;
                case "7":
                    cikis = true;
                    break;
                default:
                    Console.WriteLine("Geçersiz seçim! Tekrar deneyin.");
                    break;
            }
        }
    }

    static void KatMalikiEkle()
    {
        Console.WriteLine("Kat Maliki Ekle");
        Console.WriteLine("-----------------");

        Console.Write("Daireyi girin (örn. 1-1): ");
        string daireAdi = Console.ReadLine();

        Daire secilenDaire = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == daireAdi);

        if (secilenDaire != null)
        {
            if (string.IsNullOrEmpty(secilenDaire.Rol))
            {
                Console.Write("Kat malikinin adını ve soyadını girin: ");
                string malikAdi = Console.ReadLine();

                secilenDaire.Rol = "Kat Maliki";
                secilenDaire.Sahip = malikAdi;

                Console.WriteLine("Kat maliki başarıyla eklendi.");
            }
            else
            {
                Console.WriteLine("Bu daireye zaten bir rol atanmış.");
            }
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı. Kat maliki eklenemedi.");
        }
    }

    static void KatMalikiSil()
    {
        Console.WriteLine("Kat Maliki Sil");
        Console.WriteLine("----------------");

        Console.Write("Daireyi girin (örn. 1-1): ");
        string daireAdi = Console.ReadLine();

        Daire secilenDaire = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == daireAdi);

        if (secilenDaire != null)
        {
            if (secilenDaire.Rol == "Kat Maliki")
            {
                Console.WriteLine("Kat Maliki: {0}", secilenDaire.Sahip);
                Console.Write("Kat malikini silmek istiyor musunuz? (E/H): ");
                string cevap = Console.ReadLine();

                if (cevap.ToLower() == "e")
                {
                    secilenDaire.Rol = "";
                    secilenDaire.Sahip = "";

                    Console.WriteLine("Kat maliki başarıyla silindi.");
                }
                else
                {
                    Console.WriteLine("Kat maliki silme işlemi iptal edildi.");
                }
            }
            else
            {
                Console.WriteLine("Bu daireye kat maliki atanmamış.");
            }
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı. Kat maliki silinemedi.");
        }
    }

    static void KiraciEkle()
    {
        Console.WriteLine("Kiracı Ekle");
        Console.WriteLine("-----------------");

        Console.Write("Daireyi girin (örn. 1-1): ");
        string daireAdi = Console.ReadLine();

        Daire secilenDaire = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == daireAdi);

        if (secilenDaire != null)
        {
            if (string.IsNullOrEmpty(secilenDaire.Rol))
            {
                Console.Write("Kiracının adını ve soyadını girin: ");
                string kiraciAdi = Console.ReadLine();

                secilenDaire.Rol = "Kiracı";
                secilenDaire.Sahip = kiraciAdi;

                Console.WriteLine("Kiracı başarıyla eklendi.");
            }
            else
            {
                Console.WriteLine("Bu daireye zaten bir rol atanmış.");
            }
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı. Kiracı eklenemedi.");
        }
    }

    static void KiraciSil()
    {
        Console.WriteLine("Kiracı Sil");
        Console.WriteLine("----------------");

        Console.Write("Daireyi girin (örn. 1-1): ");
        string daireAdi = Console.ReadLine();

        Daire secilenDaire = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == daireAdi);

        if (secilenDaire != null)
        {
            if (secilenDaire.Rol == "Kiracı")
            {
                Console.WriteLine("Kiracı: {0}", secilenDaire.Sahip);
                Console.Write("Kiracıyı silmek istiyor musunuz? (E/H): ");
                string cevap = Console.ReadLine();

                if (cevap.ToLower() == "e")
                {
                    secilenDaire.Rol = "";
                    secilenDaire.Sahip = "";

                    Console.WriteLine("Kiracı başarıyla silindi.");
                }
                else
                {
                    Console.WriteLine("Kiracı silme işlemi iptal edildi.");
                }
            }
            else
            {
                Console.WriteLine("Bu daireye kiracı atanmamış.");
            }
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı. Kiracı silinemedi.");
        }
    }

    static void AidatBilgileriniGoruntuleYonetici()
    {
        Console.WriteLine("Aidat Bilgileri");
        Console.WriteLine("----------------");

        Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-15} {4,-15}", "Daire", "Sahip", "Aidat", "Ödeme Durumu", "Rol");
        Console.WriteLine("-----------------------------------------------------------------");

        foreach (Daire daire in daireler)
        {
            Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-15} {4,-15}", $"{daire.Kat}/{daire.DaireNo}", daire.Sahip, daire.Aidat.ToString()+" TL",
                daire.OdendiMi ? "Ödendi" : "Ödenmedi", daire.Rol);
        }
    }

    static void OdemeYapYonetici()
    {
        Console.WriteLine("Aidat Ödeme");
        Console.WriteLine("-----------");

        Console.Write("Daireyi girin (örn. 1-1): ");
        string daireAdi = Console.ReadLine();

        Daire odemeYapilanDaire = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == daireAdi);

        if (odemeYapilanDaire != null)
        {
            Console.WriteLine("Aidat miktarı: {0}", odemeYapilanDaire.Aidat);
            Console.Write("Ödeme yapmak istiyor musunuz? (E/H): ");
            string cevap = Console.ReadLine();

            if (cevap.ToLower() == "e")
            {
                odemeYapilanDaire.OdendiMi = true;
                Console.WriteLine("Ödeme işlemi başarıyla gerçekleştirildi.");
            }
            else
            {
                Console.WriteLine("Ödeme işlemi iptal edildi.");
            }
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı. Ödeme işlemi gerçekleştirilemedi.");
        }
    }

    static void SikayetYapYonetici()
    {
        Console.WriteLine("Şikayet Yap");
        Console.WriteLine("-----------");

        Console.Write("Daireyi girin (örn. 1-1): ");
        string daireAdi = Console.ReadLine();

        Daire secilenDaire = daireler.Find(d => $"{d.Kat}-{d.DaireNo}" == daireAdi);

        if (secilenDaire != null)
        {
            Console.Write("Şikayetinizi girin: ");
            string sikayet = Console.ReadLine();

            secilenDaire.Sikayet = sikayet;
            Console.WriteLine("Şikayetiniz kaydedildi.");
        }
        else
        {
            Console.WriteLine("Daire bilgisi bulunamadı. Şikayet kaydedilemedi.");
        }
    }
}

class Daire
{
    public int Kat { get; set; }
    public int DaireNo { get; set; }
    public decimal Aidat { get; set; }
    public bool OdendiMi { get; set; }
    public string Sikayet { get; set; }
    public string Sahip { get; set; }
    public string Rol { get; set; } // Yeni özellik
}
