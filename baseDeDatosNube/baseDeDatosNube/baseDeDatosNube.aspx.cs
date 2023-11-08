using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

public partial class Proyecto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string ejemplo = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

        using (MySqlConnection con = new MySqlConnection(ejemplo))
        {
            using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM facturas", con))
            {
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    // Manejar errores
                }
                finally
                {
                    con.Close();
                }

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        // Asumiendo que la columna que contiene la fecha se llama "fecha"
                        if (row["fechaDeFactura"] != DBNull.Value)
                        {
                            row["fechaDeFactura"] = Convert.ToDateTime(row["fechaDeFactura"]).ToString("yyyy-MM-dd");
                        }
                    }
                }

                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
        }
    }
}