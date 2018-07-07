<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateActivity.aspx.cs" Inherits="CreateActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="css/girisyap.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Icerik" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="ortala">
        <div class="formDiv">
            <div class="girisyap">
                <div class="baslikfont">ETKİNLİK OLUŞTUR </div>
                <hr style="width: 90%;" />
                <div id="giristext">
                    <h2>Etkinlik Başlığı</h2>
                    <input type="text" name="etkinlikBaslik" maxlength="100" size="50" class="girisler" id="txtEtkinlikBaslik" />

                    <h2>Etkinlik İçeriği</h2>
                    <input type="text" name="etkinlikIcerik" maxlength="100" size="50" class="girisler" id="txtEtkinlikIcerik" />

                    <h2>Etkinlik Tarihi</h2>
                    <input type="date" name="etkinlikTarih" class="girisler" id="txtEtkinlikTarih" />

                </div>

                <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

                <script>
                    function CreateActivity() {

                        var activityHeader = $("#txtEtkinlikBaslik").val();
                        var activityContent = $("#txtEtkinlikIcerik").val();
                        var activityDate = $("#txtEtkinlikTarih").val();
                        if (activityHeader == "" || activityContent == "" || activityDate == "")
                            alert("Tüm alanları doldurunuz...!");
                        else {
                            var dataAll = { "userName": "fg", "activityHeader": activityHeader, "activityContent": activityContent, "activityDate": activityDate };

                            $.ajax({
                                type: "POST",
                                url: "http://localhost:51978/NeYapsak.asmx/CreateActivity",
                                data: JSON.stringify(dataAll),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    if (data.d === true) {
                                        alert("Girişiniz Yapıldı...");
                                        session = mail;
                                        if (session != undefined && session != null) {
                                            alert("Session is : " + session);


                                            var url = "Default.aspx?username=" + encodeURIComponent(session);
                                            window.location.href = url;

                                            //window.location = '/test_App_CS_Web(Ne_Yapiyor)_v2/Default.aspx?username=' + escape(session);
                                        }
                                    }
                                    else {
                                        alert("Birşeyler ters gitti...!");
                                    }
                                }
                            });
                        }
                    };
                </script>

                <div id="girisbtndiv">
                    <%--  <asp:Button ID="Button1" runat="server" Text="Giriş Yap" CssClass="girisaybtn" OnClientClick="LoginControl()" />--%>
                    <input id="createActivityButton" class="girisaybtn" type="button" value="Oluştur" onclick="CreateActivity()" />
                </div>

            </div>
        </div>
    </div>

</asp:Content>

