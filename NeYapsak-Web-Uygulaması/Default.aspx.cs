using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    public string sessionFromSS;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (String.IsNullOrEmpty(Request.QueryString["username"]))
        //    Response.Write("Login.aspx");
        //else
        //    sessionFromSS = Request.QueryString["username"].ToString();
        if (!IsPostBack)
        {

            if (Session["username"] == null ||Session["username"]=="")
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                
             //   Response.Write(" <script>alert('" + Session["username"].ToString() + "'); </script>");
            }
        }
    }
}