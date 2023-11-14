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
    DateTime fecha; // Declarar la variable en el ámbito de la clase

    protected void Page_Load(object sender, EventArgs e)
    {
        // Solo cargar datos si no hay una solicitud de filtrado (PostBack)
        if (!IsPostBack)
        {
            string conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(conexion))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM dataFormatted", con))
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

                    TablaDatos.DataSource = ds;
                    TablaDatos.DataBind();
                }
            }
        }
    }

    protected void filtrar(object sender, EventArgs e)
    {
        string conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

        using (MySqlConnection con = new MySqlConnection(conexion))
        {
            // Modificar la consulta SQL con los parámetros del filtro
            string consulta = "SELECT * FROM dataFormatted " +
                              "WHERE 1=1";  // Condición siempre verdadera para evitar errores de sintaxis

            using (MySqlDataAdapter da = new MySqlDataAdapter(consulta, con))
            {
                // Agregar parámetros al comando solo si se proporciona un valor para fechaDeFacturaFiltro
                if (!string.IsNullOrEmpty(txtFecha.Text))
                {
                    if (DateTime.TryParseExact(txtFecha.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
                    {
                        string fechaFormateada = fecha.ToString("dd-MM-yyyy");

                        consulta += " AND fechaDeFactura = @FechaDeFactura";
                        Response.Write("Fecha ingresada: " + txtFecha.Text + "<br>");
                        Response.Write("Fecha formateada: " + fechaFormateada + "<br>");
                        Response.Write("Consulta SQL: " + consulta + "<br>");
                        da.SelectCommand.Parameters.AddWithValue("@FechaDeFactura", fechaFormateada);
                    }
                    else
                    {
                        // Manejar el caso en que la fecha ingresada no sea válida
                        Response.Write("Fecha ingresada no válida.<br>");
                        return;
                    }
                }

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

                TablaDatos.DataSource = ds;
                TablaDatos.DataBind();
            }
        }
    }
}