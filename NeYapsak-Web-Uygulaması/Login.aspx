<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="css/girisyap.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="ortala">
            <div class="formDiv">
                <div class="girisyap">
                    <div class="baslikfont">GİRİŞ YAP </div>
                    <hr style="width: 90%;" />
                    <div id="giristext">
                        <h2>Kullanıcı Adı</h2>
                        <input type="text" name="kullaniciadimail" maxlength="100" size="50" class="girisler" id="txtMail" />
                        <h2>Parola</h2>
                        <input type="password" name="girissifre" maxlength="100" size="50" class="girisler" id="txtSifre" />
                    </div>

                    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

                    <script>                                                        
                        $(document).ready(function () {                             
                            var parent = document.getElementById("topDivID");       //  Kullanıcını giriş yapmaması durumunda üstteki menünün silindiği script
                            var child = document.getElementById("box");             //
                            parent.removeChild(child);                              //           

                           
                        });                                                         
                    </script>

                    <script>


                        function LoginControl() {
                            var mail = $("#txtMail").val();
                            var sifre = $("#txtSifre").val();
                            var session;
                            var dataAll = { "userName": mail, "userPassword": sifre };

                            $.ajax({
                                type: "POST",
                                url: "http://localhost:51978/NeYapsak.asmx/Login",
                                data: JSON.stringify(dataAll),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    if (data.d === true) {
                                        //alert("Girişiniz Yapıldı...");
                                        session = mail;
                                        if (session != undefined && session != null) {

                                            //  alert("Geldi");

                                            //var url = "Default.aspx?username=" + encodeURIComponent(session);
                                            var url = "Default.aspx";

                                            SesionAdd();
                                            window.location.href = url;
                                            //window.location = '/test_App_CS_Web(Ne_Yapiyor)_v2/Default.aspx?username=' + escape(session);
                                        }
                                    }
                                    else {
                                        alert("Kullanıcı Adınız veya Şifreniz Hatalı...");
                                    }
                                }
                            });
                        };

                        function SesionAdd() {
                            var dataAll2 = { "sesion": $("#txtMail").val() }
                            $.ajax({
                                type: "POST",
                                url: "Login.aspx/SesionPass",
                                data: JSON.stringify(dataAll2),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                }
                            });

                        };
                    </script>

                    <div id="girisbtndiv">
                        <%--  <asp:Button ID="Button1" runat="server" Text="Giriş Yap" CssClass="girisaybtn" OnClientClick="LoginControl()" />--%>
                        <input id="loginButton" class="girisaybtn" type="button" value="Giriş Yap" onclick="LoginControl()" />


                    </div>

                </div>
            </div>
        </div>
    </form>
</asp:Content>

