using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eLibrary
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty((string)Session["role"]))
                {
                    LinkButton1.Visible = true;//userlogin
                    LinkButton2.Visible = true;//userSignUp
                    LinkButton3.Visible = false;//logout
                    LinkButton7.Visible = false;//profile
                    LinkButton6.Visible = true;//adminlogin
                    LinkButton11.Visible = false;//author
                    LinkButton12.Visible = false;//publisher
                    LinkButton8.Visible = false;//book inventory
                    LinkButton9.Visible = false;//book issue
                    LinkButton10.Visible = false;//member management
                }
                else if (Session["role"].Equals("user"))
                {
                    LinkButton1.Visible = false;//userlogin
                    LinkButton2.Visible = false;//userSignUp
                    LinkButton3.Visible = true;//logout

                    LinkButton7.Text = "Hello " + Session["username"].ToString();
                    LinkButton7.Visible = true;//profile

                    LinkButton6.Visible = true;//adminlogin
                    LinkButton11.Visible = false;//author
                    LinkButton12.Visible = false;//publisher
                    LinkButton8.Visible = false;//book inventory
                    LinkButton9.Visible = false;//book issue
                    LinkButton10.Visible = false;//member management
                }
                else if (Session["role"].Equals("admin"))
                {
                    LinkButton1.Visible = false;//userlogin
                    LinkButton2.Visible = false;//userSignUp
                    LinkButton3.Visible = true;//logout

                    LinkButton7.Text = "Hello Admin";
                    LinkButton7.Visible = true;//profile

                    LinkButton6.Visible = false;//adminlogin
                    LinkButton11.Visible = true;//author
                    LinkButton12.Visible = true;//publisher
                    LinkButton8.Visible = true;//book inventory
                    LinkButton9.Visible = true;//book issue
                    LinkButton10.Visible = true;//member management
                }
            }
            catch(Exception ex)
            {
                Response.Write(ex.Message);
            }
           
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("userlogin.aspx");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("userSignUp.aspx");
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            Session["username"] = "";
            Session["fullname"] = "";
            Session["role"] = "";
            Session["status"] = "";
            LinkButton1.Visible = true;//userlogin
            LinkButton2.Visible = true;//userSignUp
            LinkButton3.Visible = false;//logout
            LinkButton7.Visible = false;//profile
            LinkButton6.Visible = true;//adminlogin
            LinkButton11.Visible = false;//author
            LinkButton12.Visible = false;//publisher
            LinkButton8.Visible = false;//book inventory
            LinkButton9.Visible = false;//book issue
            LinkButton10.Visible = false;//member management
            Response.Redirect("homepage.aspx");
        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            Response.Redirect("viewBooks.aspx");
        }


        protected void LinkButton7_Click(object sender, EventArgs e)
        {
            Response.Redirect("userProfile.aspx");
        }



        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminLogin.aspx");
        }

        protected void LinkButton11_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminauthormanagement.aspx");
        }

        protected void LinkButton12_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminPublisherManagement.aspx");
        }

        protected void LinkButton8_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminBookInventory.aspx");
        }

        protected void LinkButton9_Click(object sender, EventArgs e)
        {
            Response.Redirect("bookIssue.aspx");
        }

        protected void LinkButton10_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminMemberManagement.aspx");
        }
    }
}