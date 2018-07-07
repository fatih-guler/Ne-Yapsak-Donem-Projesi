<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <link rel="stylesheet" type="text/css" href="css/girisyap.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Icerik" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form runat="server">
        <div class="ortala">
            <div class="formDiv">
                <div class="girisyap">
                    <div class="baslikfont">GİRİŞ YAP </div>
                    <hr style="width: 90%;" />
                    <div id="giristext">
                        <h2>Adınız</h2>
                        <input type="text" name="name" maxlength="100" size="50" class="girisler" id="txtName" />
                        <h2>Soyadınız</h2>
                        <input type="text" name="surname" maxlength="100" size="50" class="girisler" id="txtSurname" />
                        <h2>Doğum Tarihiniz</h2>
                        <input type="text" name="birthDate" maxlength="100" size="50" class="girisler" id="txtBirthDate" />
                        <h2>Mailiniz</h2>
                        <input type="text" name="mail" maxlength="100" size="50" class="girisler" id="txtMail" />
                        <h2>Kullanıcı Adınız</h2>
                        <input type="text" name="username" maxlength="100" size="50" class="girisler" id="txtUsername" />
                        <h2>Şifreniz</h2>
                        <input type="text" name="password" maxlength="100" size="50" class="girisler" id="txtPassword" />
                    </div>

                    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

                    <script>
                        function Register() {

                            var name = $("#txtName").val();
                            var surname = $("#txtSurname").val();
                            var birthDate = $("#txtBirthDate").val();
                            var mail = $("#txtMail").val();
                            var username = $("#txtUsername").val();
                            var password = $("#txtPassword").val();

                            var dataAll = { "name": name, "surname": surname, "birthDate": birthDate, "userEmail": mail, "userName": username, "userPassword": password };

                            console.log(dataAll);
                            $.ajax({
                                type: "POST",
                                url: "http://localhost:51978/NeYapsak.asmx/Register",
                                data: JSON.stringify(dataAll),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    if (data.d === true) {
                                        alert("Kayıt Başarılı...");
                                        var url = "Login.aspx";
                                        window.location.href = url;
                                    }
                                    else {
                                        alert("Servise gittim bişeyler oldu orda...");
                                    }
                                },
                                error: function () {
                                    alert("Birşey dönmedi");
                                }
                            });
                        };
                    </script>

                    <div id="girisbtndiv">
                        <%--  <asp:Button ID="Button1" runat="server" Text="Giriş Yap" CssClass="girisaybtn" OnClientClick="LoginControl()" />--%>
                        <input id="loginButton" class="girisaybtn" type="button" value="Kayıt Ol" onclick="Register()" />
                    </div>

                </div>
            </div>
        </div>
    </form>
</asp:Content>

