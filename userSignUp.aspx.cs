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
    public partial class userSignUp : System.Web.UI.Page
    {
        string stringcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
        
        }

        protected void SignUpButton_Click(object sender, EventArgs e)
        {
            if (!checkMemberExistance()) { signUpNewUser(); }
            else
            {
                Response.Write("<script>alert('Try Another Member ID, this one is already registered...');</script>");
            }
        }

        bool checkMemberExistance()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from member_master_tbl where member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@member_id", memberID.Text.Trim());
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

        void signUpNewUser()
        {
            try
            {
                SqlConnection con = new SqlConnection(stringcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO member_master_tbl(full_name,dob,contact_no,email,state,city,pincode,full_address,member_id,password,account_status)" +
                    "values (@full_name,@dob,@contact_no,@email,@state,@city,@pincode,@full_address,@member_id,@password,@account_status)", con);
                cmd.Parameters.AddWithValue("@full_name", FullName.Text.Trim());
                cmd.Parameters.AddWithValue("@dob", DOB.Text.Trim());
                cmd.Parameters.AddWithValue("@contact_no", Contact.Text.Trim());
                cmd.Parameters.AddWithValue("@email", email.Text.Trim());
                cmd.Parameters.AddWithValue("@state", State.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@city", city.Text.Trim());
                cmd.Parameters.AddWithValue("@pincode", pincode.Text.Trim());
                cmd.Parameters.AddWithValue("@full_address", fullAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@member_id", memberID.Text.Trim());
                cmd.Parameters.AddWithValue("@password", password.Text.Trim());
                cmd.Parameters.AddWithValue("@account_status", "Pending");
                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Sign Up Successful. Go to User Login to Login');</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}