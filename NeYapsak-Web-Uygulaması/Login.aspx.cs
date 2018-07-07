using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["username"]!=null && Session["username"]!="")
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    [WebMethod(EnableSession=true)]
    public static bool SesionPass(string sesion)
    {
        HttpContext.Current.Session.Add("username",sesion);
        return true;
    }

    [WebMethod]
    public static bool SesionRemove()
    {
        HttpContext.Current.Session["username"] = "";
        return true;
    }
}