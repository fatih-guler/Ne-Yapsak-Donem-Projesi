<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Following.aspx.cs" Inherits="Following" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- Bootstrap -->
    <link rel="stylesheet" type="text/css" href="css/bootstrap.min.css">

    <!-- Main Style -->
    <link rel="stylesheet" type="text/css" href="css2/main.css">

    <!--Icon Fonts-->
    <link rel="stylesheet" media="screen" href="fonts/font-awesome/font-awesome.min.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Icerik" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $(document.body).on("click", "img.removefriend", function () {
                alert("insiddeee");
                var friendID = $(this).attr("id");
                removeFriend(friendID);
            });
            function removeFriend(friendID) {
                var dataAll = { "userName": '<%=HttpContext.Current.Session["username"].ToString()%>', "unfollowedUserID": parseInt(friendID) };
                $.ajax({
                    type: "POST",
                    url: "http://localhost:51978/NeYapsak.asmx/Unfollow",
                    data: JSON.stringify(dataAll),
                    contentType: "application/json; charset:utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d == true)
                            alert("Takip bırakıldı");
                        else
                            alert("HATA");
                    }
                });
            };

            var dataAll = { "userName": "fg" };
            $.ajax({
                type: "POST",
                url: "http://localhost:51978/NeYapsak.asmx/DisplayFollowing",
                data: JSON.stringify(dataAll),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    //alert(data.d);



                    var jsonData = JSON.parse(data.d);

                    for (var i = 0; i < jsonData.takipEdilen.length; i++) {
                        var counter = jsonData.takipEdilen[i];


                        $('.pricing').append(
                          '<div class="col-md-4 col-sm-12 col-xs-12">'
                          + '<div class="pricing-table">'


                         + '<div class="pricing-list" style="padding-left:20px; padding-right:20px;">'
                         + '<ul>'

                         + '<li><i class="fa fa-user"></i><span>' + counter.takipEdilenAdi + " " + counter.takipEdilenSoyadi + '</span></li">'
                         + '<img src="images/removefriend.png" id="' + counter.takipEdilenKullaniciID + '" width="20" height="20" class="removefriend"/>'
                         + '</ul>'
                         + '</div>'
                         + '</div>'
                         + '</div>'
                    );
                        // style="margin-left:1px;"   style="margin-left:3px;"

                        //    alert(counter.etkinlikGonderiBaslik + " " + counter.etkinlikGonderiIcerik);
                    }
                },
                error: function () {
                    alert("Hata");
                }
            });

        });



    </script>



    <div class="container" style="margin-top: 100px">
        <h3></h3>
        <ul class="nav nav-tabs">
            <li><a href="Profilim.aspx">Etkinliklerim</a></li>
            <li class="active"><a href="Following.aspx">Takip Edilenler</a></li>
            <li><a href="Followers.aspx">Takipçiler</a></li>
            <li><a href="JoinedActivities.aspx">Yanıt Verilenler</a></li>
        </ul>
        <br>
        <div>
            <section id="pricing-table" style="margin-top: -5%;">
                <div class="container">
                    <div class="row">
                        <div class="pricing">
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

</asp:Content>

