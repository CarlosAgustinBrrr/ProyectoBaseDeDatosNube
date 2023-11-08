using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string ejemplo = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

        using (MySqlConnection con = new MySqlConnection(ejemplo))
        {
            using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM personas", con))
            {
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    da.Fill(ds, "personas");
                }
                catch (Exception ex)
                {
                    // Manejar errores
                }
                finally
                {
                    con.Close();
                }

                GridView1.DataSource = ds.Tables["personas"];
                GridView1.DataBind();
            }
        }
    }
}