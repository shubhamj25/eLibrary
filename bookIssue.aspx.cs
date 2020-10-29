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
    public partial class bookIssue : System.Web.UI.Page
    {
        string stringcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView_Issue_History.DataBind();
            
        }

        protected void CheckMemberIssueHistory_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from book_master_tbl where book_id=@book_id", con);
            cmd.Parameters.AddWithValue("@book_id", BookID.Text.Trim());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
                book_name.Text = dt.Rows[0][1].ToString();
            }
            else
            {
                Response.Write("<script>alert('No Book found with this ID');</script>");
                BookID.Text = "";
                book_name.Text = "";
            }
            cmd = new SqlCommand("select * from member_master_tbl where member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@member_id", memberID.Text.Trim());
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
                member_name.Text = dt.Rows[0][0].ToString();
            }
            else
            {
                Response.Write("<script>alert('No Member found with this ID');</script>");
                memberID.Text = "";
                member_name.Text = "";
            }
        }

        bool checkBookExists()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from book_master_tbl where book_id=@book_id and current_stock > 0", con);
            cmd.Parameters.AddWithValue("@book_id", BookID.Text.Trim());
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

        bool checkMemberExists()
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

        bool checkMemberisDefaulter()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from book_issue_tbl where member_id=@member_id and CURRENT_TIMESTAMP > due_date", con);
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

        protected void ISSUE_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            if (start_date.Text != "" && start_date.Text != null && end_date.Text != null && end_date.Text != "")
            {
                if (checkBookExists() && checkMemberExists())
                {
                    if (checkBookAlreadyWithMember())
                    {
                        Response.Write("<script>alert('Current Member already has a copy of this Book !');</script>");
                    }
                    else
                    {
                        if (checkMemberisDefaulter())
                        {
                            Response.Write("<script>alert('Current Member is a defaulter please return the book first');</script>");
                        }
                        else
                        {
                            issueBook();
                            GridView_Issue_History.DataBind();
                        }

                    }
                }
                else
                {
                    Response.Write("<script>alert('Wrong MemberID or BookID');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please fill the dates first !');</script>");
            }

        }

        void issueBook()
        {
            try
            {
                SqlConnection con = new SqlConnection(stringcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                
                  SqlCommand cmd = new SqlCommand("INSERT INTO book_issue_tbl(member_id,member_name,book_id,book_name,issue_date,due_date) " +
"values(@member_id,@member_name,@book_id,@book_name,@issue_date,@due_date)", con);
                    cmd.Parameters.AddWithValue("@member_id", memberID.Text.Trim());
                    cmd.Parameters.AddWithValue("@member_name", member_name.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_id", BookID.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_name", book_name.Text.Trim());
                    cmd.Parameters.AddWithValue("@issue_date", start_date.Text.Trim());
                    cmd.Parameters.AddWithValue("@due_date", end_date.Text.Trim());
                    cmd.ExecuteNonQuery();
                    SqlCommand cmd2 = new SqlCommand("update book_master_tbl set current_stock=current_stock-1 where book_id=@book_id", con);
                    cmd2.Parameters.AddWithValue("@book_id", BookID.Text.Trim());
                    cmd2.ExecuteNonQuery();
                    Response.Write("<script>alert('Book Issued Sucessfully !');</script>");
                    GridView_Issue_History.DataBind();
                    con.Close();
                    clearFields();
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert("+ex.Message+");</script>");
            }

        }

        bool checkBookAlreadyWithMember()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from book_issue_tbl where book_id=@book_id and member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@book_id", BookID.Text.Trim());
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

        
        protected void RETURN_Click(object sender, EventArgs e)
        {
            if (checkBookExists() && checkMemberExists())
            {
                if (checkBookAlreadyWithMember())
                {
                    returnBook();
                    GridView_Issue_History.DataBind();
                }
                else
                {
                    Response.Write("<script>alert('This Entry does not exist');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Wrong Book ID or Member ID');</script>");
            }
        }
         void returnBook()
        {
            try
            {
                SqlConnection con = new SqlConnection(stringcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("delete from book_issue_tbl where book_id=@book_id", con);
                cmd.Parameters.AddWithValue("@book_id", BookID.Text.Trim());
                cmd.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand("update book_master_tbl set current_stock=current_stock+1 where book_id=@book_id", con);
                cmd2.Parameters.AddWithValue("@book_id", BookID.Text.Trim());
                cmd2.ExecuteNonQuery();
                Response.Write("<script>alert('Book Returned Sucessfully !');</script>");
                GridView_Issue_History.DataBind();
                con.Close();
                clearFields();
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert(" + ex.Message + ");</script>");
            }
        }
        void clearFields()
        {
            BookID.Text = "";
            book_name.Text = "";
            member_name.Text = "";
            memberID.Text = "";
            start_date.Text = "";
            end_date.Text = "";
        }
        protected void GridView_Issue_History_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Check your condition here
                    DateTime dt = Convert.ToDateTime(e.Row.Cells[5].Text);
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

    }
}