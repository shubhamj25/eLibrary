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
    public partial class adminPublisherManagement : System.Web.UI.Page
    {
        string stringcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView2.DataBind();
        }

        //GO BUTTON
        protected void Button5_Click(object sender, EventArgs e)
        {
            fetchPublisherName();
        }

        //Add Button
        protected void Button6_Click(object sender, EventArgs e)
        {
            addpublisher();
        }

        //Update
        protected void Button7_Click(object sender, EventArgs e)
        {
            updatepublisherName();
        }

        //Delete
        protected void Button8_Click(object sender, EventArgs e)
        {
            deletepublisher();
        }


        void fetchPublisherName()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from publisher_master_tbl where publisher_id=@publisher_id", con);
            cmd.Parameters.AddWithValue("@publisher_id", publisherid.Text.Trim());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
                publisher_name.Text = dt.Rows[0][1].ToString();
            }
            else
            {
                Response.Write("<script>alert('No Publisher found with this ID');</script>");
                publisherid.Text = "";
                publisher_name.Text = "";
            }
        }
        bool checkpublisherExists()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from publisher_master_tbl where publisher_id=@publisher_id", con);
            cmd.Parameters.AddWithValue("@publisher_id", publisherid.Text.Trim());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void addpublisher()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed) { con.Open(); }

            if (!checkpublisherExists())
            {
                SqlCommand cmd = new SqlCommand("insert into publisher_master_tbl(publisher_id,publisher_name) values(@publisherid,@publisher_name)", con);
                cmd.Parameters.AddWithValue("@publisherid", publisherid.Text.Trim());
                cmd.Parameters.AddWithValue("@publisher_name", publisher_name.Text.Trim());
                cmd.ExecuteNonQuery();
                publisherid.Text = "";
                publisher_name.Text = "";
                Response.Write("<script>alert('Publisher Added Successfully Successful');</script>");
                GridView2.DataBind();
            }
            else
            {
                Response.Write("<script>alert('Publisher with this Id Already exsits !\nPlease add with a diffrent ID');</script>");
            }

        }

        void updatepublisherName()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            if (checkpublisherExists())
            {
                SqlCommand cmd = new SqlCommand("update publisher_master_tbl set publisher_name=@publisher_name where publisher_id=@publisherid", con);
                cmd.Parameters.AddWithValue("@publisherid", publisherid.Text.Trim());
                cmd.Parameters.AddWithValue("@publisher_name", publisher_name.Text.Trim());
                cmd.ExecuteNonQuery();
                publisherid.Text = "";
                publisher_name.Text = "";
                Response.Write("<script>alert('Update Successful');</script>");
                GridView2.DataBind();
            }
            else
            {
                Response.Write("<script>alert('Publisher with this ID doesn't exsits !\nPlease enter a valid publisher ID');</script>");
            }

        }


        void deletepublisher()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            if (checkpublisherExists())
            {
                SqlCommand cmd = new SqlCommand("delete from publisher_master_tbl where publisher_id=@publisherid", con);
                cmd.Parameters.AddWithValue("@publisherid", publisherid.Text.Trim());
                cmd.Parameters.AddWithValue("@publisher_name", publisher_name.Text.Trim());
                cmd.ExecuteNonQuery();
                publisherid.Text = "";
                publisher_name.Text = "";
                Response.Write("<script>alert('Delete Successful');</script>");
                GridView2.DataBind();
            }
            else
            {
                Response.Write("<script>alert('Publisher with this ID doesn't exsits !\nPlease enter a valid publisher ID');</script>");
            }
        }
    }
}