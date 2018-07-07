<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Followers.aspx.cs" Inherits="Followers" %>

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
            $(document.body).on("click", "img.addfriend", function () {
                alert("insiddeee");
                var friendID = $(this).attr("id");
                addFriend(friendID);
            });
            function removeFriend(friendID) {
                var dataAll = { "userName": "fg", "unfollowedUserID": parseInt(friendID) };
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
            function addFriend(friendID) {
                var dataAll = { "userName": "fg", "followedUserID": parseInt(friendID) };
                alert("inside");
                $.ajax({
                    type: "POST",
                    url: "http://localhost:51978/NeYapsak.asmx/Follow",
                    data: JSON.stringify(dataAll),
                    contentType: "application/json; charset:utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d == true)
                            alert("Takip Edilmeye başlandı");
                        else
                            alert("HATA");
                    }
                });
            };
            var dataAll = { "userName": "fg" };
            $.ajax({
                type: "POST",
                url: "http://localhost:51978/NeYapsak.asmx/DisplayFollowers",
                data: JSON.stringify(dataAll),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    //alert(data.d);



                    var jsonData = JSON.parse(data.d);

                    for (var i = 0; i < jsonData.takipciler.length; i++) {
                        var counter = jsonData.takipciler[i];
                        var imgPath;
                        var onClickEvent;
                        if (counter.takipEdilenMi == "1") {
                            imgPath = "images/removefriend.png";
                            onClickEvent = "removefriend";
                        }
                        else if (counter.takipEdilenMi == "0") {
                            imgPath = "images/addfriend.png";
                            onClickEvent = "addfriend";
                        }

                        $('.pricing').append(
                          '<div class="col-md-4 col-sm-12 col-xs-12">'
                          + '<div class="pricing-table">'


                         + '<div class="pricing-list" style="padding-left:20px; padding-right:20px;">'
                         + '<ul>'
                         + '<img src="' + imgPath + '" class="' + onClickEvent + '" width="30" height="30">'
                         + '<li><i class="fa fa-user"></i><span>' + counter.takipciKullaniciEmail + '</span></li">'
                          + '<li><span>' + counter.takipciAdi + '</span></li">'
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
            <li><a href="Following.aspx">Takip Edilenler</a></li>
            <li class="active"><a href="Followers.aspx">Takipçiler</a></li>
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

