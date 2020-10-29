using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eLibrary
{
    public partial class adminBookInventory : System.Web.UI.Page
    {
        string stringcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        static string global_filepath;
        static int global_actual_stock, global_current_stock, global_issued_books;

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView_Book_Inventory.DataBind();
            if (!IsPostBack)
            {
                fillAuthorPublisherValues();
            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            if (checkBookExists())
            {
                Response.Write("<script>alert('Book with this ID already exists ! Please enter another Id');</script>");
            }
            else
            {
                addBook();
                clearfields();
            }
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            if (checkBookExists())
            {
                string genres = "";
                foreach (int i in Genre.GetSelectedIndices())
                {
                    genres = genres + Genre.Items[i] + ",";
                }
                genres = genres.Remove(genres.Length - 1);

                string filepath = "~/book_inventory/books1";
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (filename == "" || filename == null)
                {
                    filepath = global_filepath;
                }
                else
                {
                    FileUpload1.SaveAs(Server.MapPath("book_inventory/" + filename));
                    filepath = "~/book_inventory/" + filename;
                }

                SqlCommand cmd = new SqlCommand("UPDATE book_master_tbl set book_name=@book_name, genre=@genre, author_name=@author_name, publisher_name=@publisher_name, publish_date=@publish_date, language=@language, edition=@edition, book_cost=@book_cost, no_of_pages=@no_of_pages, book_description=@book_description, actual_stock=@actual_stock, current_stock=@current_stock, book_img_link=@book_img_link where book_id='" + bookID.Text.Trim() + "'", con);

                cmd.Parameters.AddWithValue("@book_name", Book_Name.Text.Trim());
                cmd.Parameters.AddWithValue("@genre", genres);
                cmd.Parameters.AddWithValue("@author_name", Author.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@publisher_name", Publisher.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@publish_date", Publish_Date.Text.Trim());
                cmd.Parameters.AddWithValue("@language", Lang.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@edition",Edition.Text.Trim());
                cmd.Parameters.AddWithValue("@book_cost", Cost.Text.Trim());
                cmd.Parameters.AddWithValue("@no_of_pages", Pages.Text.Trim());
                cmd.Parameters.AddWithValue("@book_description", Desc.Text.Trim());
                cmd.Parameters.AddWithValue("@actual_stock", Actual_Stock.ToString());
                cmd.Parameters.AddWithValue("@current_stock", Current_Stock.ToString());
                cmd.Parameters.AddWithValue("@book_img_link", filepath);
                cmd.ExecuteNonQuery();
                con.Close();
                GridView_Book_Inventory.DataBind();
                Response.Write("<script>alert('Book Updated Successfully');</script>");
            }
            else
            {
                Response.Write("<script>alert('Book with this ID doesn't exsits !\nPlease enter a valid Book ID');</script>");
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            if (checkBookExists())
            {
                try
                {
                    SqlConnection con = new SqlConnection(stringcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("DELETE from book_master_tbl WHERE book_id='" + bookID.Text.Trim() + "'", con);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Write("<script>alert('Book Deleted Successfully');</script>");

                    GridView_Book_Inventory.DataBind();

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }

            }
            else
            {
                Response.Write("<script>alert('Invalid Book ID');</script>");
            }
        }

        protected void CheckBookExists_Click(object sender, EventArgs e)
        {
            fetchBookDetails();
        }

        void addBook()
        {
            string genre = "";
            foreach (int i in Genre.GetSelectedIndices())
            {
                genre += Genre.Items[i] + ",";
            }

            if (genre.Length > 0) {
                genre.Remove(genre.Length - 1);
            }
           
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            FileUpload1.SaveAs(Server.MapPath("book_inventory/" + fileName));
            string filePath = "~/book_inventory/" + fileName;
        
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed) { con.Open(); }

            if (!checkBookExists())
            {
                SqlCommand cmd = new SqlCommand("insert into book_master_tbl(book_id,book_name,genre,author_name,publisher_name,publish_date,language,edition,book_cost,no_of_pges,book_description,actual_stock,current_stock,book_img_link) " +
                    "values(@book_id,@book_name,@genre,@author_name,@publisher_name,@publish_date,@language,@edition,@book_cost,@no_of_pges,@book_description,@actual_stock,@current_stock,@book_img_link)", con);
                cmd.Parameters.AddWithValue("@book_id", bookID.Text.Trim());
                cmd.Parameters.AddWithValue("@book_name", Book_Name.Text.Trim());
                cmd.Parameters.AddWithValue("@genre", genre);
                cmd.Parameters.AddWithValue("@author_name", Author.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@publisher_name", Publisher.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@publish_date", Publish_Date.Text.Trim());
                cmd.Parameters.AddWithValue("@language", Lang.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@edition", Edition.Text.Trim());
                cmd.Parameters.AddWithValue("@book_cost", Cost.Text.Trim());
                cmd.Parameters.AddWithValue("@no_of_pges", Pages.Text.Trim());
                cmd.Parameters.AddWithValue("@book_description", Desc.Text.Trim());
                cmd.Parameters.AddWithValue("@actual_stock", Actual_Stock.Text.Trim());
                cmd.Parameters.AddWithValue("@current_stock", Actual_Stock.Text.Trim());
                cmd.Parameters.AddWithValue("@book_img_link", filePath);
                cmd.ExecuteNonQuery();
                clearfields();
                Response.Write("<script>alert('Book Added Successfully Successful');</script>");
                GridView_Book_Inventory.DataBind();
            }
            else
            {
                Response.Write("<script>alert('Book with this Id Already exsits !\nPlease add with a diffrent ID');</script>");
            }

        }

        bool checkBookExists()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from book_master_tbl where book_id=@book_id", con);
            cmd.Parameters.AddWithValue("@book_id", bookID.Text.Trim());
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

        void fetchBookDetails()
        {
            SqlConnection con = new SqlConnection(stringcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from book_master_tbl where book_id=@book_id", con);
            cmd.Parameters.AddWithValue("@book_id", bookID.Text.Trim());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
                Book_Name.Text = dt.Rows[0][1].ToString();
                string[] genreSelected=dt.Rows[0][2].ToString().Split(',').ToArray<string>();
                Genre.ClearSelection();
                for(int i=0;i < Genre.Items.Count;i++)
                {
                    if (genreSelected.Contains(Genre.Items[i].Value))
                    {
                        Genre.Items[i].Selected = true;
                    }
                }
                Author.SelectedValue = dt.Rows[0][3].ToString().Trim();
                Publisher.SelectedValue = dt.Rows[0][4].ToString().Trim();
                Publish_Date.Text = dt.Rows[0][5].ToString().Trim();
                Lang.SelectedValue = dt.Rows[0][6].ToString().Trim();
                Edition.Text = dt.Rows[0][7].ToString().Trim();
                Cost.Text = dt.Rows[0][8].ToString().Trim();
                Pages.Text = dt.Rows[0][9].ToString().Trim();
                Desc.Text = dt.Rows[0][10].ToString();
                Actual_Stock.Text = dt.Rows[0][11].ToString().Trim();
                Current_Stock.Text = dt.Rows[0][12].ToString().Trim();
                Issued_Books.Text =""+(Convert.ToInt32(dt.Rows[0]["actual_stock"].ToString()) - Convert.ToInt32(dt.Rows[0]["current_stock"].ToString()));
               

                global_actual_stock = Convert.ToInt32(dt.Rows[0]["actual_stock"].ToString().Trim());
                global_current_stock = Convert.ToInt32(dt.Rows[0]["current_stock"].ToString().Trim());
                global_issued_books = global_actual_stock - global_current_stock;
                global_filepath = dt.Rows[0]["book_img_link"].ToString();
            }
            else
            {
                Response.Write("<script>alert('No Book found with this ID');</script>");
                clearfields();
            }
        }


        void fillAuthorPublisherValues()
        {
            try
            {
                SqlConnection con = new SqlConnection(stringcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("select author_name from author_master_tbl", con);
                SqlCommand cmd2 = new SqlCommand("select publisher_name from publisher_master_tbl", con);
                SqlDataAdapter da1, da2;
                da1 = new SqlDataAdapter(cmd);
                da2 = new SqlDataAdapter(cmd2);
                DataTable dt1 = new DataTable(), dt2 = new DataTable();
                da1.Fill(dt1); da2.Fill(dt2);
                Author.DataSource = dt1;
                Author.DataValueField = "author_name";
                Publisher.DataSource = dt2;
                Publisher.DataValueField = "publisher_name";
                Author.DataBind(); Publisher.DataBind();
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('" + e.Message + "');</script>");
            }

        }

        void clearfields()
        {
            bookID.Text = "";
            Book_Name.Text = "";
            Genre.ClearSelection();
            Publish_Date.Text = "";
            Edition.Text = "";
            Cost.Text = "";
            Pages.Text = "";
            Desc.Text = "";
            Actual_Stock.Text = "";
            Current_Stock.Text = "";
        }

      
    }
}