using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
namespace FSharpWcfServiceApplicationTemplate
{
    /// <summary>
    /// Summary description for NeYapsak
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.ScriptService]
    public class NeYapsak : System.Web.Services.WebService
    {

        SqlConnection dbConnection = new SqlConnection(@"SERVER=CODE-PC;database=NeYapiyor_DB;Trusted_Connection=yes;MultipleActiveResultSets=true");
        SqlCommand command;
        [WebMethod]
        public bool ConnectTheDatabase()
        {
            try
            {
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                    dbConnection.Open();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        //TODO : kullanici silindiğinde, kurduğu ve katıkdığı etkinliklerden de silinsin
        [WebMethod]
        public bool DeleteUser(string userName)
        {
            bool result = false;
            ConnectTheDatabase();
            try
            {

                SqlCommand cmdSil = new SqlCommand("DELETE FROM tblKisiBilgileri WHERE kullanici_ID =(SELECT kullanici_ID FROM tblKullaniciBilgileri WHERE kullaniciAdi=@kullaniciAdi)", dbConnection);
                cmdSil.Parameters.AddWithValue("@kullaniciAdi", userName);
                cmdSil.ExecuteNonQuery();

                cmdSil = new SqlCommand("DELETE FROM tblKullaniciBilgileri WHERE kullaniciAdi=@kullaniciAdi", dbConnection);
                cmdSil.Parameters.AddWithValue("@kullaniciAdi", userName);
                cmdSil.ExecuteNonQuery();

                cmdSil = new SqlCommand("DELETE FROM tblKatilimDurumu WHERE kullanici_ID=(SELECT kullanici_ID FROM tblKullaniciBilgileri WHERE kullaniciAdi=@kadi)", dbConnection);
                cmdSil.Parameters.AddWithValue("@kadi", userName);
                cmdSil.ExecuteNonQuery();

                cmdSil = new SqlCommand("DELETE FROM tblEtkinlikGonderileri WHERE kullanici_ID=(SELECT kullanici_ID FROM tblKullaniciBilgileri WHERE kullaniciAdi=@kadi)", dbConnection);
                cmdSil.Parameters.AddWithValue("@kadi", userName);
                cmdSil.ExecuteNonQuery();
                result = true;
            }
            catch (Exception)
            {

                result = false;
            }
            return result;
        }
        [WebMethod]
        public bool Register(string name, string surname, string birthDate, string userEmail, string userName, string userPassword)
        {

            bool result = false;
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
            SqlCommand cmdControl = new SqlCommand("SELECT COUNT(*) FROM tblKullaniciBilgileri WHERE kullaniciAdi='" + userName + "' OR kullaniciEMail='" + userEmail + "'", dbConnection);
            int count = (int)cmdControl.ExecuteScalar();
            if (count > 0)
                result = false;
            else
            {

                try
                {
                    result = true;
                    SqlCommand cmd = new SqlCommand("INSERT INTO tblKullaniciTakip(takipci_ID) VALUES(@takipciID)", dbConnection);
                    cmd.Parameters.AddWithValue("@takipciID", userName);
                    cmd.ExecuteNonQuery();
                    int kullaniciTakipID = (int)(new SqlCommand("SELECT kullaniciTakip_ID FROM tblKullaniciTakip WHERE takipci_ID='" + userName + "'", dbConnection)).ExecuteScalar();

                    cmd = new SqlCommand("INSERT INTO tblKullaniciBilgileri(kullaniciAdi, kullaniciSifresi, kullaniciEMail,kullaniciTakip_ID) VALUES(@kad, @sifre, @email,@kullaniciTakipID)", dbConnection);
                    cmd.Parameters.AddWithValue("@kad", userName);
                    cmd.Parameters.AddWithValue("@sifre", userPassword);
                    cmd.Parameters.AddWithValue("@email", userEmail);
                    cmd.Parameters.AddWithValue("@kullaniciTakipID", kullaniciTakipID);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("INSERT INTO tblKisiBilgileri(kisiAdi, kisiSoyadi, kisiDTarihi, kullanici_ID) VALUES(@ad, @soyad, @dTarih, @kid)", dbConnection);
                    cmd.Parameters.AddWithValue("@ad", name);
                    cmd.Parameters.AddWithValue("@soyad", surname);
                    cmd.Parameters.AddWithValue("@dTarih", Convert.ToDateTime(birthDate.ToString()));
                    cmd.Parameters.AddWithValue("@kid", (int)(new SqlCommand("SELECT kullanici_ID FROM tblKullaniciBilgileri WHERE kullaniciAdi='" + userName + "'", dbConnection)).ExecuteScalar());
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                    result = false;
                }
            }

            return result;
        }
        [WebMethod]
        public bool UpdateUser(string userName, string name, string surname, string birthDate, string userPassword)
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
            bool result = false;
            try
            {
                SqlCommand cmdGuncelle = new SqlCommand("UPDATE tblKullaniciBilgileri SET kullaniciSifresi=@ksifre WHERE kullaniciAdi=@userName", dbConnection);
                cmdGuncelle.Parameters.AddWithValue("@ksifre", userPassword);
                cmdGuncelle.Parameters.AddWithValue("@userName", userName);
                cmdGuncelle.ExecuteNonQuery();
                cmdGuncelle = new SqlCommand("UPDATE tblKisiBilgileri SET kisiAdi=@kadi, kisiSoyadi=@ksoyadi, kisiDTarihi=@kDTarihi WHERE kullanici_ID=(select kullanici_ID from tblKullaniciBilgileri Where kullaniciAdi=@userName)", dbConnection);
                cmdGuncelle.Parameters.AddWithValue("@kadi", name);
                cmdGuncelle.Parameters.AddWithValue("@ksoyadi", surname);
                cmdGuncelle.Parameters.AddWithValue("@kDTarihi", DateTime.Parse(birthDate));
                cmdGuncelle.Parameters.AddWithValue("@userName", userName);
                cmdGuncelle.ExecuteNonQuery();
                result = true;
            }
            catch (Exception)
            {

                result = false;
            }

            return result;
        }
        [WebMethod]
        public bool Login(string userName, string userPassword)
        {
            if (dbConnection.State == System.Data.ConnectionState.Closed)
                dbConnection.Open();
            bool kayitVarmi = false;
            try
            {
                command = new SqlCommand("Select Count(*) From tblKullaniciBilgileri Where kullaniciAdi=@kullaniciAdi and kullaniciSifresi=@sifre", dbConnection);
                command.Parameters.AddWithValue("@kullaniciAdi", userName);
                command.Parameters.AddWithValue("@sifre", userPassword);
                if ((int)command.ExecuteScalar() > 0)
                    kayitVarmi = true;
                else
                    kayitVarmi = false;
            }
            catch (Exception)
            {

                kayitVarmi = false;
            }
            dbConnection.Close();
            return kayitVarmi;

        }
        [WebMethod]
        public bool Follow(int userID, int followedUserID)
        {
            bool result = false;
            if (dbConnection.State == System.Data.ConnectionState.Closed)
                dbConnection.Open();
            try
            {
                command = new SqlCommand("insert into tblTakipciler Values((select kullaniciAdi from tblKullaniciBilgileri where kullanici_ID='" + (userID) + "'), @kullaniciID)", dbConnection);

                command.Parameters.AddWithValue("@kullaniciID", (followedUserID));
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception)
            {

                result = false;
            }
            dbConnection.Close();
            return result;
        }
        [WebMethod]
        public bool Unfollow(string userName, int unfollowedUserID)
        {
            bool result = false;
            if (dbConnection.State == System.Data.ConnectionState.Closed)
                dbConnection.Open();
            try
            {
                command = new SqlCommand("DELETE FROM tblTakipciler WHERE kullanici_ID=(Select kullanici_ID from tblKullaniciBilgileri Where kullaniciAdi=@kadi) AND takipci_ID=(SELECT kullaniciAdi FROM tblKullaniciBilgileri WHERE kullanici_ID=@k_id)", dbConnection);
                command.Parameters.AddWithValue("@kadi", userName);
                command.Parameters.AddWithValue("@kid", (unfollowedUserID));
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception)
            {

                result = false;
            }
            dbConnection.Close();
            return result;
        }
        [WebMethod]
        public bool CreateActivity(string userName, string activityHeader, string activityContent, string activityDate)
        {
            bool result = false;
            if (dbConnection.State == System.Data.ConnectionState.Closed)
                dbConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO tblEtkinlikGonderileri(etkinlikGonderiBaslik,etkinlikGonderiIcerik,etkinlikGonderiTarih,etkinlikEklemeTarihi,kullanici_ID) VALUES(@baslik,@icerik,@tarih,GetDate(), (Select kullanici_ID from tblKullaniciBilgileri Where kullaniciAdi=@kadi))", dbConnection);
                cmd.Parameters.AddWithValue("@baslik", activityHeader);
                cmd.Parameters.AddWithValue("@icerik", activityContent);
                cmd.Parameters.AddWithValue("@tarih", DateTime.Parse(activityDate));
                cmd.Parameters.AddWithValue("@kadi", userName);
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception)
            {

                result = false;
            }

            return result;
        }
        [WebMethod]
        public bool DeleteActivity(int activityID)
        {
            bool result = false;
            if (dbConnection.State == System.Data.ConnectionState.Closed)
                dbConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM tblKatilimDurumu WHERE etkinlikGonderi_ID=@id", dbConnection);
                cmd.Parameters.AddWithValue("@id", activityID);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("DELETE FROM tblEtkinlikGonderileri WHERE etkinlikGonderi_ID=@id", dbConnection);
                cmd.Parameters.AddWithValue("@id", activityID);
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception)
            {

                result = false;
            }

            return result;
        }
        [WebMethod]
        public bool JoinActivity(string userName, int activityID, int activityResponseID)
        {
            bool result = false;
            if (dbConnection.State == System.Data.ConnectionState.Closed)
                dbConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("declare @sayi int Select @sayi=COUNT(*) from tblKatilimDurumu where kullanici_ID = (select kullanici_ID from tblKullaniciBilgileri where kullaniAdi=@kadi) and etkinlikGonderi_ID = @eid" +
                "if(@sayi<1) begin	insert into tblKatilimDurumu(kullanici_ID, etkinlikGonderi_ID, gonderiYanit_ID) values((select kullanici_ID from tblKullaniciBilgileri where kullaniciAdi=@kadi),@eid,@gyid)	end" +
                "else  begin update tblKatilimDurumu set gonderiYanit_ID = @gyid where kullanici_ID = (select kullanici_ID from tblKullaniciBilgileri where kullaniciAdi=@kadi) and etkinlikGonderi_ID = @eid end", dbConnection);
                cmd.Parameters.AddWithValue("@kadi", userName);
                cmd.Parameters.AddWithValue("@eid", activityID);
                cmd.Parameters.AddWithValue("@gyid", activityResponseID);
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
        [WebMethod]
        public bool GiveUpByJoiningActivity(string userName, int activityID)
        {
            bool result = false;
            if (dbConnection.State == System.Data.ConnectionState.Closed)
                dbConnection.Open();
            try
            {
                SqlCommand cmdSil = new SqlCommand("DELETE FROM tblKatilimDurumu WHERE kullanici_ID=(Select kullanici_ID from tblKullaniciBilgileri Where kullaniciAdi=@kadi) AND etkinlikGonderi_ID=@egid", dbConnection);
                cmdSil.Parameters.AddWithValue("@kadi", userName);
                cmdSil.Parameters.AddWithValue("@egid", activityID);
                cmdSil.ExecuteNonQuery();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;

        }
        //[WebMethod]
        //public string DisplayActivities(string followerID) //takipciID = kullaniciAdi
        //{
        //    //Takip edilen kişilerin paylaştığı etkinlikler
        //    if (dbConnection.State == ConnectionState.Closed)
        //        dbConnection.Open();

        //    DataSet ds = new DataSet();
        //    ds.Tables.Add("tblActivities");
        //    SqlDataAdapter adapter = new SqlDataAdapter("Select EG.etkinlikGonderiBaslik, EG.etkinlikGonderiIcerik, EG.etkinlikGonderiTarih" +
        //                                                " from tblTakipciler T inner join tblKullaniciTakip KT on KT.takipci_ID = T.takipci_ID" +
        //                                                " inner join tblKullaniciBilgileri KB on Kb.kullaniciTakip_ID = KT.kullaniciTakip_ID" +
        //                                                " inner join tblEtkinlikGonderileri EG on T.kullanici_ID = EG.kullanici_ID" +
        //                                                " Where T.takipci_ID='" + followerID + "'", dbConnection);
        //    adapter.Fill(ds, "tblActivities");
        //    return ds.GetXml().ToString();
        //}
        [WebMethod]
        public string DisplayActivitiesOfUser(string username)
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
            SqlCommand cmd = new SqlCommand("Select * from tblEtkinlikGonderileri Where kullanici_ID=(SELECT kullanici_ID FROM tblKullaniciBilgileri WHERE kullaniciAdi=@kid)", dbConnection);
            cmd.Parameters.AddWithValue("@kid", username);
            SqlDataReader read = cmd.ExecuteReader();
            var txt = "{\"gonderiler\" : [";
            SqlCommand cmdRowCount = new SqlCommand("Select COUNT(*) From tblEtkinlikGonderileri Where kullanici_ID=(SELECT kullanici_ID FROM tblKullaniciBilgileri WHERE kullaniciAdi=@kid)", dbConnection);
            cmdRowCount.Parameters.AddWithValue("@kid", username);
            int rowCount = (int)cmdRowCount.ExecuteScalar();
            int counter = 0;
            while (read.Read())
            {


                txt += "{\"etkinlikGonderiBaslik\" : \"" +
                    read["etkinlikGonderiBaslik"].ToString() +
                    "\",\"etkinlikGonderiIcerik\" : \"" + read["etkinlikGonderiIcerik"].ToString() +
                    "\",\"etkinlikGonderiID\" : \"" + read["etkinlikGonderi_ID"].ToString() +
                    "\",\"etkinlikGonderiTarih\" : \"" + read["etkinlikGonderiTarih"].ToString() + "\" }";
                if (counter != rowCount - 1)
                    txt += ",";


                counter++;
            }
            txt += "]}";
            read.Close();
            return txt;
        }
        //Son kullanılan aşağıdaki



        [WebMethod]
        public string DisplayActivities(string userName)
        {

            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
            SqlCommand cmd = new SqlCommand("Select EG.etkinlikGonderiBaslik, EG.etkinlikGonderiIcerik, EG.etkinlikGonderiTarih, EG.etkinlikGonderi_ID" +
                                                       " from tblTakipciler T inner join tblKullaniciTakip KT on KT.takipci_ID = T.takipci_ID" +
                                                       " inner join tblKullaniciBilgileri KB on Kb.kullaniciTakip_ID = KT.kullaniciTakip_ID" +
                                                       " inner join tblEtkinlikGonderileri EG on T.kullanici_ID = EG.kullanici_ID" +
                                                       " Where T.takipci_ID='" + userName + "' and EG.etkinlikGonderi_ID not in(select etkinlikGonderi_ID from tblKatilimDurumu where kullanici_ID = (select kullanici_ID from tblKullaniciBilgileri where kullaniciAdi='" + userName + "'))", dbConnection);
            SqlDataReader read = cmd.ExecuteReader();
            var txt = "{\"gonderiler\" : [";
            SqlCommand cmdRowCount = new SqlCommand("Select COUNT(EG.etkinlikGonderiBaslik)" +
                                                       " from tblTakipciler T inner join tblKullaniciTakip KT on KT.takipci_ID = T.takipci_ID" +
                                                       " inner join tblKullaniciBilgileri KB on Kb.kullaniciTakip_ID = KT.kullaniciTakip_ID" +
                                                       " inner join tblEtkinlikGonderileri EG on T.kullanici_ID = EG.kullanici_ID" +
                                                       " Where T.takipci_ID='" + userName + "' and EG.etkinlikGonderi_ID not in(select etkinlikGonderi_ID from tblKatilimDurumu where kullanici_ID = (select kullanici_ID from tblKullaniciBilgileri where kullaniciAdi='" + userName + "'))", dbConnection);

            int rowCount = (int)cmdRowCount.ExecuteScalar();
            int counter = 0;
            while (read.Read())
            {


                txt += "{\"etkinlikGonderiBaslik\" : \"" + read["etkinlikGonderiBaslik"].ToString() +
                    "\",\"etkinlikGonderiIcerik\" : \"" + read["etkinlikGonderiIcerik"].ToString() +
                    "\",\"etkinlikGonderiID\" : \"" + read["etkinlikGonderi_ID"].ToString() +
                    "\",\"etkinlikGonderiTarih\" : \"" + read["etkinlikGonderiTarih"].ToString() + "\" }";
                if (counter != rowCount - 1)
                    txt += ",";


                counter++;
            }
            txt += "]}";
            read.Close();
            return txt;

        }
        [WebMethod]
        public string DisplayFollowing(string userName)//userName tarafından takip edilenler
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
            SqlCommand cmd = new SqlCommand("select T.kullanici_ID, K.kullaniciAdi, K.kullaniciEMail, KB.kisiAdi, KB.kisiSoyadi from tblTakipciler T inner join tblKullaniciBilgileri K on T.kullanici_ID=K.kullanici_ID inner join tblKisiBilgileri KB on KB.kullanici_ID=k.kullanici_ID Where T.takipci_ID = '" + userName + "'", dbConnection);
            SqlDataReader read = cmd.ExecuteReader();
            var txt = "{\"takipEdilen\" : [";
            SqlCommand cmdRowCount = new SqlCommand("select Count(T.kullanici_ID) from tblTakipciler T inner join tblKullaniciBilgileri K on T.kullanici_ID=K.kullanici_ID inner join tblKisiBilgileri KB on KB.kullanici_ID=k.kullanici_ID Where T.takipci_ID = '" + userName + "'", dbConnection);

            int rowCount = (int)cmdRowCount.ExecuteScalar();
            int counter = 0;
            while (read.Read())
            {


                txt += "{\"takipEdilenKullaniciID\" : \"" + read["kullanici_ID"].ToString() +
                    "\",\"takipEdilenKullaniciAdi\" : \"" + read["kullaniciAdi"].ToString() +
                    "\",\"takipEdilenAdi\" : \"" + read["kisiAdi"].ToString() +
                    "\",\"takipEdilenSoyadi\" : \"" + read["kisiSoyadi"].ToString() +
                    "\",\"takipEdilenKullaniciEmail\" : \"" + read["kullaniciEMail"].ToString() + "\" }";
                if (counter != rowCount - 1)
                    txt += ",";


                counter++;
            }
            txt += "]}";
            read.Close();
            return txt;

        }
        //[WebMethod]
        //public string DisplayFollowers(string userName)
        //{
        //    if (dbConnection.State == ConnectionState.Closed)
        //        dbConnection.Open();
        //    SqlCommand cmd = new SqlCommand("select T.takipci_ID, K.kullanici_ID, K.kullaniciAdi, K.kullaniciEMail,KB.kisiAdi, KB.kisiSoyadi from tblKullaniciBilgileri K inner join tblTakipciler T on k.kullaniciAdi = T.takipci_ID inner join tblKisiBilgileri KB on KB.kullanici_ID=K.kullanici_ID where t.kullanici_ID = (select kullanici_ID From tblKullaniciBilgileri WHERE kullaniciAdi='" + userName + "')", dbConnection);
        //    SqlDataReader read = cmd.ExecuteReader();
        //    var txt = "{\"takipciler\" : [";
        //    SqlCommand cmdRowCount = new SqlCommand("select Count(T.takipci_ID) from tblKullaniciBilgileri K inner join tblTakipciler T on k.kullaniciAdi = T.takipci_ID inner join tblKisiBilgileri KB on KB.kullanici_ID=K.kullanici_ID where t.kullanici_ID = (select kullanici_ID From tblKullaniciBilgileri WHERE kullaniciAdi='" + userName + "')", dbConnection);
        //    int rowCount = (int)cmdRowCount.ExecuteScalar();
        //    int counter = 0;
        //    while (read.Read())
        //    {


        //        txt += "{\"takipciKullaniciID\" : \"" + read["kullanici_ID"].ToString() +
        //            "\",\"takipciKullaniciAdi\" : \"" + read["kullaniciAdi"].ToString() +
        //            "\",\"takipciSoyadi\" : \"" + read["kisiAdi"].ToString() +
        //            "\",\"takipciAdi\" : \"" + read["kisiSoyadi"].ToString() +
        //            "\",\"takipciKullaniciEmail\" : \"" + read["kullaniciEMail"].ToString() + "\" }";
        //        if (counter != rowCount - 1)
        //            txt += ",";


        //        counter++;
        //    }
        //    txt += "]}";
        //    read.Close();
        //    return txt;

        //}
        [WebMethod]
        public string DisplayProfile(string userName)
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
            var txt = "{\"profil\" : [";
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblKullaniciBilgileri WHERE kullaniciAdi=@userName", dbConnection);
            cmd.Parameters.AddWithValue("@userName", userName);
            SqlDataReader read = cmd.ExecuteReader();
            read.Read();
            txt += "{\"kullaniciAdi\" :  \"" + read["kullaniciAdi"].ToString() +
                "\", \"kullaniciEmail\" : \"" + read["kullaniciEmail"].ToString() + "\",";

            cmd = new SqlCommand("SELECT * FROM tblKisiBilgileri WHERE kullanici_ID=(select kullanici_ID from tblKullaniciBilgileri Where kullaniciAdi=@userName)", dbConnection);
            cmd.Parameters.AddWithValue("@userName", userName);
            read = cmd.ExecuteReader();
            read.Read();

            cmd = new SqlCommand("SELECT COUNT(*) FROM tblEtkinlikGonderileri WHERE kullanici_ID=(select kullanici_ID from tblKullaniciBilgileri Where kullaniciAdi=@userName)", dbConnection);
            cmd.Parameters.AddWithValue("@userName", userName);
            int gonderiSayisi = (int)cmd.ExecuteScalar();

            cmd = new SqlCommand("SELECT COUNT(*) FROM tblKatilimDurumu WHERE kullanici_ID=(select kullanici_ID from tblKullaniciBilgileri Where kullaniciAdi=@userName)", dbConnection);
            cmd.Parameters.AddWithValue("@userName", userName);
            int yanitVerilenler = (int)cmd.ExecuteScalar();

            cmd = new SqlCommand("SELECT COUNT(*) FROM tblTakipciler WHERE kullanici_ID=(select kullanici_ID from tblKullaniciBilgileri Where kullaniciAdi=@userName)", dbConnection);
            cmd.Parameters.AddWithValue("@userName", userName);
            int takipciSayisi = (int)cmd.ExecuteScalar();

            cmd = new SqlCommand("SELECT COUNT(*) FROM tblTakipciler T inner join tblKullaniciBilgileri K on T.takipci_ID=K.kullaniciAdi WHERE T.kullanici_ID=(select kullanici_ID from tblKullaniciBilgileri Where kullaniciAdi=@userName)", dbConnection);
            cmd.Parameters.AddWithValue("@userName", userName);
            int takipEdilenSayisi = (int)cmd.ExecuteScalar();

            txt += "\"kisiAdi\" : \"" + read["kisiAdi"].ToString() + "\",\"gonderiSayisi\" : \"" + gonderiSayisi.ToString() + "\",\"yanitVerilenSayisi\" : \"" + yanitVerilenler.ToString() + "\",\"takipciSayisi\" : \"" + takipciSayisi.ToString() + "\",\"TakipEdilenSayisi\" : \"" + takipEdilenSayisi.ToString() + "\" ,\"kisiSoyadi\" : \"" + read["kisiSoyadi"].ToString() + "\"}]}";
            return txt;
        }
        [WebMethod]
        public string DisplayJoinedActivities(string userName)
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
            SqlCommand cmd = new SqlCommand("Select EG.etkinlikGonderi_ID, GD.gonderiYanit, EG.etkinlikGonderiBaslik,EG.etkinlikGonderiIcerik, EG.etkinlikGonderiTarih from tblKatilimDurumu KD inner join tblGonderiYanitlari GD on GD.gonderiYanit_ID=KD.gonderiYanit_ID inner join tblEtkinlikGonderileri EG on KD.etkinlikGonderi_ID=EG.etkinlikGonderi_ID Where KD.kullanici_ID=(Select kullanici_ID from tblKullaniciBilgileri where kullaniciAdi=@userName)", dbConnection);
            cmd.Parameters.AddWithValue("@userName", userName);
            SqlDataReader read = cmd.ExecuteReader();
            var txt = "{\"yanitlananGonderiler\" : [";
            SqlCommand cmdRowCount = new SqlCommand("Select Count(EG.etkinlikGonderi_ID) from tblKatilimDurumu KD inner join tblGonderiYanitlari GD on GD.gonderiYanit_ID=KD.gonderiYanit_ID inner join tblEtkinlikGonderileri EG on KD.etkinlikGonderi_ID=EG.etkinlikGonderi_ID Where KD.kullanici_ID=(Select kullanici_ID from tblKullaniciBilgileri where kullaniciAdi='" + userName + "')", dbConnection);


            int rowCount = (int)cmdRowCount.ExecuteScalar();
            int counter = 0;
            while (read.Read())
            {


                txt += "{\"yGonderiBaslik\" : \"" +
                    read["etkinlikGonderiBaslik"].ToString() +
                    "\",\"yGonderiIcerik\" : \"" + read["etkinlikGonderiIcerik"].ToString() +
                    "\",\"yGonderiID\" : \"" + read["etkinlikGonderi_ID"].ToString() +
                    "\",\"yGonderiYanit\" : \"" + read["gonderiYanit"].ToString() +
                    "\",\"yGonderiTarih\" : \"" + read["etkinlikGonderiTarih"].ToString() + "\" }";
                if (counter != rowCount - 1)
                    txt += ",";


                counter++;
            }
            txt += "]}";
            read.Close();
            return txt;

        }


        [WebMethod]
        public string DisplayFollowers(string userName)
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();
            SqlCommand cmd = new SqlCommand("select T.takipci_ID, K.kullanici_ID, K.kullaniciAdi, K.kullaniciEMail,KB.kisiAdi, KB.kisiSoyadi from tblKullaniciBilgileri K inner join tblTakipciler T on k.kullaniciAdi = T.takipci_ID inner join tblKisiBilgileri KB on KB.kullanici_ID=K.kullanici_ID where t.kullanici_ID = (select kullanici_ID From tblKullaniciBilgileri WHERE kullaniciAdi='" + userName + "')", dbConnection);
            SqlDataReader read = cmd.ExecuteReader();
            var txt = "{\"takipciler\" : [";
            SqlCommand cmdRowCount = new SqlCommand("select Count(T.takipci_ID) from tblKullaniciBilgileri K inner join tblTakipciler T on k.kullaniciAdi = T.takipci_ID inner join tblKisiBilgileri KB on KB.kullanici_ID=K.kullanici_ID where t.kullanici_ID = (select kullanici_ID From tblKullaniciBilgileri WHERE kullaniciAdi='" + userName + "')", dbConnection);
            int rowCount = (int)cmdRowCount.ExecuteScalar();
            int counter = 0;
            string takipEdilenMi = "";
            int followedC = -1;
            while (read.Read())
            {
                followedC = -1;
                string userFollowed = read["kullaniciAdi"].ToString();
                cmd = new SqlCommand("Select COUNT(*) FROM tblTakipciler Where takipci_ID=@tid and kullanici_ID=(Select kullanici_ID FROM tblKullaniciBilgileri WHERE kullaniciAdi=@kid)", dbConnection);
                cmd.Parameters.AddWithValue("@tid", userName);
                cmd.Parameters.AddWithValue("@kid", userFollowed);
                followedC = (int)cmd.ExecuteScalar();
                if (followedC == 1)
                    takipEdilenMi = "1";
                else
                    takipEdilenMi = "0";

                txt += "{\"takipciKullaniciID\" : \"" + read["kullanici_ID"].ToString() +
                    "\",\"takipciKullaniciAdi\" : \"" + read["kullaniciAdi"].ToString() +
                    "\",\"takipciSoyadi\" : \"" + read["kisiAdi"].ToString() +
                    "\",\"takipciAdi\" : \"" + read["kisiSoyadi"].ToString() +
                    "\",\"takipEdilenMi\" : \"" + takipEdilenMi.ToString() +
                    "\",\"takipciKullaniciEmail\" : \"" + read["kullaniciEMail"].ToString() + "\" }";
                if (counter != rowCount - 1)
                    txt += ",";


                counter++;
            }
            txt += "]}";
            read.Close();
            return txt;

        }
    }
}
