using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using IS_KASSE;
using System.Data;


namespace IS_KASSE
{
    /*
     * bool Update : class update ve insert için kullanıldığından update  için i insert için mi çağrıldı?
     * bool Sonuc: return ile akisi kesemediğimden akışı kesmek için 
     * 
     * 
     */

    public class User
    {
        MySqlConnection myConn = new MySqlConnection();
        db baglanti = new db();
        private int _userKod;
        public bool update = false;
        public bool Sonuc = true;

        public int UserKod
        {
            get { return _userKod; }
            set
            {
                _userKod = value; 
                /* if (this.update == false)
                 {
                     myConn = baglanti.myconn();
                     if (myConn.State == System.Data.ConnectionState.Closed)
                     {
                         myConn.Open();
                     }
                     MySqlDataAdapter daUser = new MySqlDataAdapter("select * from user where kod=" + value, myConn);
                     DataTable dtUser = new DataTable("user");
                     dtUser.Clear();
                     daUser.Fill(dtUser);

                     int kaysay = dtUser.Rows.Count;
                     if (kaysay > 0)
                     {
                         System.Windows.Forms.MessageBox.Show("BU KULLLANICI KODUNU KULLANAMAZSINIZ!");
                         Sonuc = false;
                         _userKod = -1;
                         return;
                     }
                     else
                     {
                         _userKod = value;
                         Sonuc = true;
                     }
                 }
                 else
                 {
                     _userKod = value;
                 }*/
            }
        }
        private string _userAd;

        public string UserAd
        {
            get { return _userAd; }
            set { _userAd = value; }
        }
        private string _userSoyad;

        public string UserSoyad
        {
            get { return _userSoyad; }
            set { _userSoyad = value; }
        }
        private string _sehir;

        public string Sehir
        {
            get { return _sehir; }
            set { _sehir = value; }
        }
        private string _strase;

        public string Strase
        {
            get { return _strase; }
            set { _strase = value; }
        }
        private int _plz;

        public int Plz
        {
            get { return _plz; }
            set { _plz = value; }
        }
        private string _ceptel;

        public string Ceptel
        {
            get { return _ceptel; }
            set { _ceptel = value; }
        }

        private string _sabitTel;

        public string SabitTel
        {
            get { return _sabitTel; }
            set { _sabitTel = value; }
        }
        private int _yonetici;

        public int Yonetici
        {
            get { return _yonetici; }
            set { _yonetici = value; }
        }

        private int _yetki;

        public int Yetki
        {
            get { return _yetki; }
            set { _yetki = value; }
        }
        private long _userid;

        public long Userid
        {
            get { return _userid; }
            set { _userid = value; }
        }

        private string _adSoyad;

        public string AdSoyad
        {
            get { return _adSoyad; }
            set { _adSoyad = value; }
        }
        private string _appID;

        public string AppID
        {
            get { return _appID; }
            set { _appID = value; }
        }
        private int _zYetki;
        public int ZYetki
        {
            get { return _zYetki; }
            set { _zYetki = value; }
        }
        public string UserBarkod { get; set; }
        internal void Kaydet()
        {
            myConn = baglanti.myconn();
            if (myConn.State == System.Data.ConnectionState.Closed)
            {
                myConn.Open();
            }
            MySqlCommand coUser = new MySqlCommand("insert into user (kod,ad,soyad,strase,plz, sehir, ceptel, sabittel, yonetici) values (" + _userKod + ",'" + _userAd + "','" + _userSoyad + "','" + _strase + "'," + _plz + ",'" + _sehir + "','" + _ceptel + "','" + _sabitTel + "'," + Yonetici + " )", myConn);
            if (coUser.ExecuteNonQuery() > 0)
            {
                long userid = coUser.LastInsertedId;
                System.Windows.Forms.MessageBox.Show("Kullanıcı Kaydedildi!");
                if (Yetki == 1)
                {
                    MySqlDataAdapter daYetki = new MySqlDataAdapter("SELECT * FROM yetkiler", myConn);
                    DataTable dtYetki = new DataTable("yetkiler");
                    dtYetki.Rows.Clear();
                    daYetki.Fill(dtYetki);
                    string yetkiSQL = "";
                    if (dtYetki.Rows.Count > 0)
                    {
                        yetkiSQL = "INSERT into useryetki (yetkiid, userid, durum) values ";
                        for (int i = 0; i < dtYetki.Rows.Count; i++)
                        {
                            yetkiSQL += "(" + dtYetki.Rows[i].ItemArray[0] + ", " + userid + ",1 ),";
                        }
                        yetkiSQL = yetkiSQL.Substring(0, yetkiSQL.Length - 1);
                        MySqlCommand coYetkiInsert = new MySqlCommand(yetkiSQL, myConn);
                        try
                        {
                            coYetkiInsert.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            System.Windows.Forms.MessageBox.Show("Bir Hata Oluştu! Hata KOdu:" + e.Message);
                        }
                    }

                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Bir Sorun Var!");
            }
            myConn.Close();
        }

        internal void Guncelle(int grupID)
        {
            myConn = baglanti.myconn();
            if (myConn.State == System.Data.ConnectionState.Closed)
            {
                myConn.Open();
            }
            string upDateSql = "UPDATE user SET kod=" + _userKod + ",ad ='" + _userAd + "', soyad='" + _userSoyad + "', strase='" + _strase + "', plz=" + _plz + ", sehir='" + _sehir + "', ceptel='" + _ceptel + "', sabittel='" + _sabitTel + "', yonetici=" + this.Yonetici + " WHERE userid=" + grupID;
            MySqlCommand coUser = new MySqlCommand(upDateSql, myConn);
            if (coUser.ExecuteNonQuery() > 0)
            {
                System.Windows.Forms.MessageBox.Show("Kullanıcı Bilgileri Güncellendi!");

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Bir Sorun Var!");
            }
            myConn.Close();
        }

        internal void Sil(int grupID)
        {
            myConn = baglanti.myconn();
            if (myConn.State == System.Data.ConnectionState.Closed)
            {
                myConn.Open();
            }
            string upDateSql = "DELETE FROM user WHERE userid=" + grupID;
            MySqlCommand coUser = new MySqlCommand(upDateSql, myConn);
            if (coUser.ExecuteNonQuery() > 0)
            {
                System.Windows.Forms.MessageBox.Show("Kullanıcı Bilgileri Silindi!");

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Bir Sorun Var!");
            }
            myConn.Close();
        }
        public string UserBul(long userid)
        {
            myConn = baglanti.myconn();
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();

            }
            /*CONCAT_WS : sorgu içinde alanları birleştirir)*/
            MySqlDataAdapter daUserBul = new MySqlDataAdapter("SELECT CONCAT_WS(' ',ad,soyad) as isim from USER where userid=" + userid, myConn);
            DataTable dtUserBul = new DataTable("user");
            dtUserBul.Rows.Clear();
            daUserBul.Fill(dtUserBul);
            if (dtUserBul.Rows.Count > 0)
            {
                myConn.Close();
                return dtUserBul.Rows[0].ItemArray[0].ToString();
            }
            else
            {
                myConn.Close();
                return "Yok";
            }

        }
        public string UserBulKOD(long userid)
        {
            myConn = baglanti.myconn();
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();

            }
            /*CONCAT_WS : sorgu içinde alanları birleştirir)*/
            MySqlDataAdapter daUserBul = new MySqlDataAdapter("SELECT CONCAT_WS(' ',ad,soyad) as isim from USER where kod=" + userid, myConn);
            DataTable dtUserBul = new DataTable("user");
            dtUserBul.Rows.Clear();
            daUserBul.Fill(dtUserBul);
            if (dtUserBul.Rows.Count > 0)
            {
                myConn.Close();
                return dtUserBul.Rows[0].ItemArray[0].ToString();
            }
            else
            {
                myConn.Close();
                return "Yok";
            }

        }
        public List<User>  GetUserList()
        {
             myConn = baglanti.myconn();
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();

            }
            List<User> KullaniciListesi = new List<User>();
            /*CONCAT_WS : sorgu içinde alanları birleştirir)*/
            MySqlDataAdapter daUserBul = new MySqlDataAdapter("SELECT CONCAT_WS(' ',ad,soyad)  as isim, user.* from USER" , myConn);
            DataTable dtUserBul = new DataTable("user");
            dtUserBul.Rows.Clear();
            daUserBul.Fill(dtUserBul);
            if (dtUserBul.Rows.Count > 0)
            {
                for (int i = 0; i < dtUserBul.Rows.Count; i++)
                {
                    User user = new User();
                    user.AdSoyad = dtUserBul.Rows[i].ItemArray[0].ToString();
                    user.Userid = Convert.ToInt32(dtUserBul.Rows[i].ItemArray[1]);
                    user.update = true;
                    user.UserKod = Convert.ToInt32(dtUserBul.Rows[i].ItemArray[4]);
                    KullaniciListesi.Add(user);

                }
                return KullaniciListesi;

            }
            else
            {
                return null;
            }

        }
        public User GetUser(string userPIN)
        {
            myConn = baglanti.myconn();
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();

            }
           
            /*CONCAT_WS : sorgu içinde alanları birleştirir)*/
            MySqlDataAdapter daUserBul = new MySqlDataAdapter("SELECT CONCAT_WS(' ',ad,soyad)  as isim, user.* from USER", myConn);
            DataTable dtUserBul = new DataTable("user");
            dtUserBul.Rows.Clear();
            daUserBul.Fill(dtUserBul);
            if (dtUserBul.Rows.Count > 0)
            {
                for (int i = 0; i < dtUserBul.Rows.Count; i++)
                {
                    User user = new User();
                    user.AdSoyad = dtUserBul.Rows[i].ItemArray[0].ToString();
                    user.Userid = Convert.ToInt32(dtUserBul.Rows[i].ItemArray[1]);
                    user.UserAd = dtUserBul.Rows[i].ItemArray[1].ToString();
                    user.UserSoyad = dtUserBul.Rows[i].ItemArray[2].ToString();
                    user.update = true;
                    user.UserKod = Convert.ToInt32(dtUserBul.Rows[i].ItemArray[4]);
                    user.ZYetki= Convert.ToInt32(dtUserBul.Rows[i].ItemArray[15]);
                    user.AppID= dtUserBul.Rows[i].ItemArray[14].ToString();
                    user.UserBarkod= dtUserBul.Rows[i].ItemArray[13].ToString();
                    return user;
                }
                

            }
            else
            {
                return null;
            }
            return null;

        }
    }
}
