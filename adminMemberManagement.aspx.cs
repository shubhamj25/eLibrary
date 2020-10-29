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
    public partial class adminMemberManagement : System.Web.UI.Page
    {
        string stringcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView3.DataBind();
        }

        protected void Activate_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {
                SqlConnection con = new SqlConnection(stringcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("update member_master_tbl set account_status='Active' where member_id=@member_id", con);
                cmd.Parameters.AddWithValue("@member_id", memberid.Text.Trim());
                cmd.ExecuteNonQuery();
                account_status.Text = "Active";
            }
            else
            {
                Response.Write("<script>alert('Please enter an existing Member ID');</script>");
                clearfields();
            }
        }

        protected void Pause_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {
                SqlConnection con = new SqlConnection(stringcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("update member_master_tbl set account_status='Paused' where member_id=@member_id", con);
                cmd.Parameters.AddWithValue("@member_id", memberid.Text.Trim());
                cmd.ExecuteNonQuery();
                account_status.Text = "Paused";
            }
            else
            {
                Response.Write("<script>alert('Please enter an existing Member ID');</script>");
                clearfields();
            }
        }

        protected void Block_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {
                SqlConnection con = new SqlConnection(stringcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("update member_master_tbl set account_status='Blocked' where member_id=@member_id", con);
                cmd.Parameters.AddWithValue("@member_id", memberid.Text.Trim());
                cmd.ExecuteNonQuery();
                account_status.Text = "Blocked";
            }
            else
            {
                Response.Write("<script>alert('Please enter an existing Member ID');</script>");
                clearfields();
            }
        }

        protected void check_Click(object sender, EventArgs e)
        {
            fetchMemberDetails();
        }

        protected void delete_user_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {
                SqlConnection con = new SqlConnection(stringcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("delete from member_master_tbl where member_id=@member_id", con);
                cmd.Parameters.AddWithValue("@member_id", memberid.Text.Trim());
                clearfields();
            }
            else
            {
                Response.Write("<script>alert('Member with this ID does not exists !');</script>");
                clearfields();
            }
        }


        bool checkMemberExists()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from member_master_tbl where member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@member_id", memberid.Text.Trim());
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

        void fetchMemberDetails()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from member_master_tbl where member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@member_id", memberid.Text.Trim());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
                account_status.Text = dt.Rows[0][10].ToString();
                fullname.Text = dt.Rows[0][0].ToString();
                dob.Text = dt.Rows[0][1].ToString();
                contact.Text = dt.Rows[0][2].ToString();
                email.Text = dt.Rows[0][3].ToString();
                state.Text = dt.Rows[0][4].ToString();
                city.Text = dt.Rows[0][5].ToString();
                pincode.Text = dt.Rows[0][6].ToString();
                full_address.Text = dt.Rows[0][7].ToString();

            }
            else
            {
                Response.Write("<script>alert('No Publisher found with this ID');</script>");
                clearfields();
            }
        }


        void clearfields()
        {
            account_status.Text = "";
            fullname.Text = "";
            dob.Text = "";
            contact.Text = "";
            email.Text = "";
            state.Text = "";
            city.Text = "";
            pincode.Text = "";
            full_address.Text = "";
            memberid.Text = "";
        }

    }
}