<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <!-- Bootstrap -->
            <link rel="stylesheet" type="text/css" href="css/bootstrap.min.css">
            
            <!-- Main Style -->
            <link rel="stylesheet" type="text/css" href="css2/main.css">

            <!--Icon Fonts-->
            <link rel="stylesheet" media="screen" href="fonts/font-awesome/font-awesome.min.css" />
                        
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
      <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
      <script>
          $(document).ready(function () {
              

             
              DisplayAcitivies();
              $(document.body).on("click", "a.yanit", function () {
                  var etkinlikID = $(this).attr("id");
                  var yanitID = $(this).attr("yanitID");
              //    alert(etkinlikID + "-" + yanitID); //Etkinlik ID ve verilen yanıtın gösterildiği alert
                  JoinActivity(etkinlikID, yanitID);
                  
              });
            
              document.getElementById("loginText").innerHTML = '<%=HttpContext.Current.Session["username"].ToString()%>';
          });
              
        
          function DisplayAcitivies() {
              //velisorkun
              var dataAll = { "userName": '<%=HttpContext.Current.Session["username"].ToString()%>' };
              $.ajax({
                  type: "POST",
                  url: "http://localhost:51978/NeYapsak.asmx/DisplayActivities",
                  data: JSON.stringify(dataAll),
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: function (data) {
                   //   alert(data.d); //Gelen JSON veriyi gösteren alert

                      var jsonData = JSON.parse(data.d);

                      for (var i = 0; i < jsonData.gonderiler.length; i++) {
                          var counter = jsonData.gonderiler[i];

                          $('.pricing').append(
                            '<div class="col-md-6 col-sm-12 col-xs-12" id="' + counter.etkinlikGonderiID + '">'
                            + '<div class="pricing-table">'
                                + '<div class="pricing-header">'
                                    + '<p class="pricing-title">' + counter.etkinlikGonderiBaslik + '</p>'

                                     + '<a href="#" yanitID="1" id="' + counter.etkinlikGonderiID + '" class="btn btn-custom yanit">' + "Katıl" + '</a>'
                                     + '<a href="#" yanitID="2" id="' + counter.etkinlikGonderiID + '"  class="btn btn-custom yanit" style="margin-left:5px;">' + "Belki" + '</a>'
                                     + '<a href="#" yanitID="3" id="' + counter.etkinlikGonderiID + '"  class="btn btn-custom yanit" style="margin-left:5px;">' + "Olmaz" + '</a>'
                                 + '</div>'

                           + '<div class="pricing-list" style="padding-left:20px; padding-right:20px;">'
                           + '<ul>'
                           + '<li><i class="fa fa-envelope"></i>10.10.2016-da</li>'
                           + '<li><i class="fa fa-user"></i><span>' + counter.etkinlikGonderiIcerik + '</span></li">'
                           + '</ul>'
                           + '</div>'
                           + '</div>'
                           + '</div>'
                      );
                          // style="margin-left:1px;"   style="margin-left:3px;"

                         // alert(counter.etkinlikGonderiBaslik + " " + counter.etkinlikGonderiIcerik);
                      }
                  },
                  error: function () {
                      alert("Hata");
                  }
              });

          };


      <%-- "<%#HttpContext.Current.Session["username"].ToString()%>"--%>
          function JoinActivity(eid, yid) {
              var dataAll = { "username": "fg", "activityID": parseInt(eid), "activityResponseID": parseInt(yid) };
          
              $.ajax({
                  type: "POST",
                  url: "http://localhost:51978/NeYapsak.asmx/JoinActivity",
                  data: JSON.stringify(dataAll),
                  contentType: "application/json; charset:utf-8",
                  dataType: "json",
                  success: function (data) {
                      if (data.d == true) {
                         // alert("Aktiviteye katıldın");

                          
                          var parent = document.getElementById("topD1");
                          var child = document.getElementById(eid);
                          parent.removeChild(child);
                        //  alert("Silindi Olmalı.");
                      }
                      else
                          alert("HATA");
                      
                  }
              });

           
              
          };



        </script>
         <section id="pricing-table" style="margin-top:5%; background-image:url('images/bg.jpg')">
            <div class="container">
                <div class="row">
                    <div class="pricing" id="topD1">
                       
                  <%--  <div class="col-md-4 col-sm-12 col-xs-12">
                            <div class="pricing-table">
                                <div class="pricing-header">
                                    <p class="pricing-title">Halısahaya Gidiyoruz</p>
                                   
                                    <a href="#" class="btn btn-custom" >Katıl</a>
                                    <a href="#" class="btn btn-custom">Belki</a>
                                    <a href="#" class="btn btn-custom">Omas</a>
                                </div>

                                <div class="pricing-list" style="padding-left:4px; padding-right:4px;">
                                    <ul>
                                        <li><i class="fa fa-envelope"></i>10.10.2016'da</li>

                                    <li"><i class="fa fa-user" ></i><span>Saat 7 de Turgutlu halısahada toplanıyoruz...</span></li>
                                    </ul>
                                </div>
                            </div>
                        </div>--%>

                    </div>
                </div>
            </div>
        </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Icerik" runat="Server">
    <!-- Pricing Table Section -->
   
</asp:Content>