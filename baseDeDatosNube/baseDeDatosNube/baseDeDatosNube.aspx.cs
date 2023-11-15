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
        // Solo cargar datos si no hay una solicitud de filtrado (PostBack)
        if (!IsPostBack)
        {
            string conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(conexion))
            {
                string consulta = "SELECT " + 
                            " numFactura as ID, " + 
                            "date_format(fechaDeFactura, '%d-%m-%Y') as 'Fecha de factura', " +
                            "cifCliente as Cliente, " +
                            "nombreApellidos as 'Nombre y Apellidos', " +
                            "importe as Importe, " +
                            "importeIVA as 'Importe con IVA', " +
                            "moneda as Moneda, " +
                            "COALESCE(date_format(fechacobro, '%d-%m-%Y'), 'N/A') as 'Fecha de cobro', " +
                            "COALESCE(estado, 'N/A') as 'Estado de la factura' " +
                            "FROM facturas";
                using (MySqlDataAdapter da = new MySqlDataAdapter(consulta, con))
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

    protected void FiltrarDatos(object sender, EventArgs e)
    {
        string conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

        using (MySqlConnection con = new MySqlConnection(conexion))
        {
            // Modificar la consulta SQL con los parámetros del filtro
            string consulta = "SELECT " +
                            " numFactura as ID, " +
                            "date_format(fechaDeFactura, '%d-%m-%Y') as 'Fecha de factura', " +
                            "cifCliente as Cliente, " +
                            "nombreApellidos as 'Nombre y Apellidos', " +
                            "importe as Importe, " +
                            "importeIVA as 'Importe con IVA', " +
                            "moneda as Moneda, " +
                            "COALESCE(date_format(fechacobro, '%d-%m-%Y'), 'N/A') as 'Fecha de cobro', " +
                            "COALESCE(estado, 'N/A') as 'Estado de la factura' " +
                            "FROM facturas WHERE 1=1";  // Condición siempre verdadera para evitar errores de sintaxis

            if (!string.IsNullOrEmpty(FechaFactura.Text))
            {
                try
                {
                    DateTime fechaFactura = DateTime.Parse((string)FechaFactura.Text);
                    string fechaFormateada = fechaFactura.ToString("yyyy-MM-dd");
                    consulta += " AND fechaDeFactura = '" + fechaFormateada + "'" ;
                }
                catch (FormatException ex)
                {
                    // Manejar la excepción de formato de fecha
                    Response.Write("Error al convertir la fecha. Asegúrate de ingresarla en el formato correcto.<br>");
                }
            }

            if (!string.IsNullOrEmpty(cifCliente.Text))
            {
                consulta += " AND cifCliente = '" + cifCliente.Text + "'";
            }

            if (!string.IsNullOrEmpty(FechaPago.Text))
            {
                try
                {
                    DateTime fechaFactura = DateTime.Parse((string)FechaPago.Text);
                    string fechaFormateada = fechaFactura.ToString("yyyy-MM-dd");
                    consulta += " AND fechaDeFactura = '" + fechaFormateada + "'";
                }
                catch (FormatException ex)
                {
                    // Manejar la excepción de formato de fecha
                    Response.Write("Error al convertir la fecha. Asegúrate de ingresarla en el formato correcto.<br>");
                }
            }

            string condicionEstado = procesarEstado(Estado.SelectedValue);
            if (!string.IsNullOrEmpty(condicionEstado))
            {
                consulta += condicionEstado;
            }

            Response.Write("Consulta SQL: " + consulta + "<br>");
            using (MySqlDataAdapter da = new MySqlDataAdapter(consulta, con))
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

    protected void Estado_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    protected string procesarEstado(string estadoSeleccionado)
    {
        string condicionEstado = "";
        // Convierte el valor seleccionado a un tipo específico, por ejemplo, int
        if (int.TryParse(estadoSeleccionado, out int estado))
        {

            switch (estado)
            {
                case -1:
                    break;
                case 0:
                    condicionEstado = " AND estado = 'Impagada'";
                    break;
                case 1:
                    condicionEstado = " AND estado = 'Pagada'";
                    break;
                default:

                    break;
            }

        }
        else
        {
            // Manejar la conversión fallida si es necesario
        }

        return condicionEstado;
    }

    protected void limpiarFiltro(object sender, EventArgs e)
    {
        string conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

        using (MySqlConnection con = new MySqlConnection(conexion))
        {
            string consulta = "SELECT " +
                            " numFactura as ID, " +
                            "date_format(fechaDeFactura, '%d-%m-%Y') as 'Fecha de factura', " +
                            "cifCliente as Cliente, " +
                            "nombreApellidos as 'Nombre y Apellidos', " +
                            "importe as Importe, " +
                            "importeIVA as 'Importe con IVA', " +
                            "moneda as Moneda, " +
                            "COALESCE(date_format(fechacobro, '%d-%m-%Y'), 'N/A') as 'Fecha de cobro', " +
                            "COALESCE(estado, 'N/A') as 'Estado de la factura' " +
                            "FROM facturas";
            using (MySqlDataAdapter da = new MySqlDataAdapter(consulta, con))
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

                FechaFactura.Text = "";
                cifCliente.Text = "";
                FechaPago.Text = "";
                Estado.SelectedValue = "-1";

                TablaDatos.DataSource = ds;
                TablaDatos.DataBind();
            }
        }
    }
}