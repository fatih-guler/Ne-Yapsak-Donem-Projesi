﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="JoinedActivities.aspx.cs" Inherits="JoinedActivities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Icerik" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script>
        $(document).ready(function () {
            $(document.body).on("click", "a.vazgec", function () {
                alert("insiddeee");
                var activityId = $(this).attr("id");
                giveUpJoinActivity(activityId);
            });
            function giveUpJoinActivity(activityID) {
                var dataAll = { "userName": '<%=HttpContext.Current.Session["username"].ToString()%>', "activityID": parseInt(activityID) };
                $.ajax({
                    type: "POST",
                    url: "http://localhost:51978/NeYapsak.asmx/GiveUpByJoiningActivity",
                    data: JSON.stringify(dataAll),
                    contentType: "application/json; charset:utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d == true)
                            alert("Etkinlikten vazgeçildi");
                        else
                            alert("HATA");
                    }
                });
            };

            var dataAll = { "userName": "fg" };
            $.ajax({
                type: "POST",
                url: "http://localhost:51978/NeYapsak.asmx/DisplayJoinedActivities",
                data: JSON.stringify(dataAll),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    //alert(data.d);



                    var jsonData = JSON.parse(data.d);

                    for (var i = 0; i < jsonData.yanitlananGonderiler.length; i++) {
                        var counter = jsonData.yanitlananGonderiler[i];


                        $('.pricing').append(
        '<div class="col-md-4 col-sm-12 col-xs-12">'
        + '<div class="pricing-table">'
            + '<div class="pricing-header">'
                + '<p class="pricing-title">' + counter.yGonderiBaslik + '</p>'

                 //+ '<a href="#" yanitID="1" id="' + counter.etkinlikGonderiID + '" class="btn btn-custom yanit">' + "Katıl" + '</a>'
                 + '<a href="#"  id="' + counter.yGonderiID + '"  class="btn btn-custom duzenle" style="margin-left:5px;">' + "Düzenle" + '</a>'
                 + '<a href="#" id="' + counter.yGonderiID + '"  class="btn btn-custom vazgec" style="margin-left:5px;">' + "Vazgeç" + '</a>'
             + '</div>'

       + '<div class="pricing-list" style="padding-left:20px; padding-right:20px;">'
       + '<ul>'
       + '<li><i class="fa fa-envelope"></i>10.10.2016-da</li>'
       + '<li><i class="fa fa-user"></i><span>' + counter.yGonderiIcerik + '</span></li">'
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
            <li><a href="Followers.aspx">Takipçiler</a></li>
            <li class="active"><a href="JoinedActivities.aspx">Yanıt Verilenler</a></li>
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

