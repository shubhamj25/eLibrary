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
    public partial class adminauthormanagement : System.Web.UI.Page
    {
        string stringcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        protected void Go_Click(object sender, EventArgs e)
        {
            fetchAuthorName();
        }


        void fetchAuthorName()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from author_master_tbl where author_id=@author_id", con);
            cmd.Parameters.AddWithValue("@author_id", AuthorID.Text.Trim());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
                Author_Name.Text = dt.Rows[0][1].ToString();
            }
            else
            {
                Response.Write("<script>alert('No author found with this ID');</script>");
                AuthorID.Text = "";
                Author_Name.Text = "";
            }
        }
        bool checkAuthorExists()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from author_master_tbl where author_id=@author_id", con);
            cmd.Parameters.AddWithValue("@author_id", AuthorID.Text.Trim());
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

        void addAuthor()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed) { con.Open(); }

            if (!checkAuthorExists())
            {
                SqlCommand cmd = new SqlCommand("insert into author_master_tbl(author_id,author_name) values(@authorid,@author_name)", con);
                cmd.Parameters.AddWithValue("@authorid", AuthorID.Text.Trim());
                cmd.Parameters.AddWithValue("@author_name", Author_Name.Text.Trim());
                cmd.ExecuteNonQuery();
                AuthorID.Text = "";
                Author_Name.Text = "";
                Response.Write("<script>alert('Author Added Successfully Successful');</script>");
                GridView1.DataBind();
            }
            else
            {
                Response.Write("<script>alert('Author with this Id Already exsits !\nPlease add with a diffrent ID');</script>");
            }
            
        }

        void updateAuthorName()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            if (checkAuthorExists())
            {
                SqlCommand cmd = new SqlCommand("update author_master_tbl set author_name=@author_name where author_id=@authorid", con);
                cmd.Parameters.AddWithValue("@authorid", AuthorID.Text.Trim());
                cmd.Parameters.AddWithValue("@author_name", Author_Name.Text.Trim());
                cmd.ExecuteNonQuery();
                AuthorID.Text = "";
                Author_Name.Text = "";
                Response.Write("<script>alert('Update Successful');</script>");
                GridView1.DataBind();
            }
            else
            {
                Response.Write("<script>alert('Author with this ID doesn't exsits !\nPlease enter a valid Author ID');</script>");
            }
           
        }


        void deleteAuthor()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            if (checkAuthorExists())
            {
                SqlCommand cmd = new SqlCommand("delete from author_master_tbl where author_id=@authorid", con);
                cmd.Parameters.AddWithValue("@authorid", AuthorID.Text.Trim());
                cmd.Parameters.AddWithValue("@author_name", Author_Name.Text.Trim());
                cmd.ExecuteNonQuery();
                AuthorID.Text = "";
                Author_Name.Text = "";
                Response.Write("<script>alert('Delete Successful');</script>");
                GridView1.DataBind();
            }
            else
            {
                Response.Write("<script>alert('Author with this ID doesn't exsits !\nPlease enter a valid Author ID');</script>");
            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            addAuthor();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            updateAuthorName();
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            deleteAuthor();
           
        }

    }

}