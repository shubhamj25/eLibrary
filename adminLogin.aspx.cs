using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eLibrary
{
    public partial class adminLogin : System.Web.UI.Page
    {
        string stringcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Login_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(stringcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("select * from admin_login_tbl where username=@admin_id and password=@pass", con);
                cmd.Parameters.AddWithValue("@admin_id", adminID.Text.Trim());
                cmd.Parameters.AddWithValue("@pass", password.Text.Trim());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count >= 1)
                {
                    //Response.Write("<script>alert('Welcome " + dt.Rows[0][2] + "!!!');</script>");
                    Session["username"] = dt.Rows[0][0].ToString();
                    Session["fullname"] = dt.Rows[0][2].ToString();
                    Session["role"] = "admin";
                    Response.Redirect("homepage.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Login Unsuccesful');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}