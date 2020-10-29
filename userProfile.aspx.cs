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
    public partial class userProfile : System.Web.UI.Page
    {
        string stringcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string old_member_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDataSource1.SelectCommand = "SELECT book_id,book_name,issue_date,due_date FROM book_issue_tbl WHERE member_id='"+Session["username"]+"'";
            GridView1.DataBind();
            old_member_id = member_id.Text.Trim();
            fetchDetails();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           
            if (old_member_id != member_id.Text.Trim())
            {
                if (checkMemberExists())
                {
                    updateDetails();
                    Response.Write("<script>alert('Details Updated Successfully !');</script>");
                }
            }
            else
            {
                updateDetails();
                Response.Write("<script>alert('Details Saved Successfully !');</script>");
            }
        }


        void updateDetails()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("update member_master_tbl set full_name=@fullname,dob=@dob,contact_no=@contact,email=@email,state=@state,city=@city,pincode=@pincode,full_address=@fullAddress,member_id=@member_id,password=@password where member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@fullname",fullname.Text.Trim());
            cmd.Parameters.AddWithValue("@dob", dob.Text.Trim());
            cmd.Parameters.AddWithValue("@contact", contact.Text.Trim());
            cmd.Parameters.AddWithValue("@email", email.Text.Trim());
            cmd.Parameters.AddWithValue("@state", state.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@city", city.Text.Trim());
            cmd.Parameters.AddWithValue("@pincode", pincode.Text.Trim());
            cmd.Parameters.AddWithValue("@fullAddress", fullAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@member_id", member_id.Text.Trim());
            if(new_password.Text!="" && new_password.Text != null && new_password.Text==pass.Text)
            {
                cmd.Parameters.AddWithValue("@password",pass.Text.Trim());
            }
            else
            {
                cmd.Parameters.AddWithValue("@password",new_password.Text.Trim());
                Response.Write("<script>alert('Password Changed Successfully !');</script>");
            }
            cmd.ExecuteNonQuery();
            fetchDetails();
            con.Close();
        }


        bool checkMemberExists()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from member_master_tbl where member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@member_id", member_id.Text.Trim());
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Check your condition here
                    DateTime dt = Convert.ToDateTime(e.Row.Cells[3].Text);
                    DateTime today = DateTime.Today;
                    if (today > dt)
                    {
                        e.Row.BackColor = System.Drawing.Color.PaleVioletRed;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }


        void fetchDetails()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from member_master_tbl where member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@member_id", Session["username"].ToString());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
                account_status.Text = dt.Rows[0][10].ToString().Trim();
                fullname.Text = dt.Rows[0][0].ToString().Trim();
                dob.Text = dt.Rows[0][1].ToString().Trim();
                contact.Text = dt.Rows[0][2].ToString().Trim();
                email.Text = dt.Rows[0][3].ToString().Trim();
                state.Text = dt.Rows[0][4].ToString().Trim();
                city.Text = dt.Rows[0][5].ToString().Trim();
                pincode.Text = dt.Rows[0][6].ToString().Trim();
                fullAddress.Text = dt.Rows[0][7].ToString().Trim();
                member_id.Text = dt.Rows[0][8].ToString().Trim();
                pass.Text = dt.Rows[0][9].ToString().Trim();
            }
            else
            {
                Response.Write("<script>alert('Session not Found :(');</script>");
            }
        }

    }
}