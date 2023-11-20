using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text;

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
        FechaFactura.Text = "";
        cifCliente.Text = "";
        FechaPago.Text = "";
        Estado.SelectedValue = "-1";

        Response.Redirect(Request.RawUrl);
    }


    protected void modificarEstado(object sender, EventArgs e)
    {
        string conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        string numFactura = idFactura.Text;  // Asumiendo que idFactura.Text es una cadena.

        using (MySqlConnection con = new MySqlConnection(conexion))
        {
            string consulta = "SELECT estado FROM facturas WHERE numFactura = @NumFactura";

            using (MySqlCommand cmd = new MySqlCommand(consulta, con))
            {
                cmd.Parameters.AddWithValue("@NumFactura", numFactura);

                try
                {
                    con.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string estado = reader["estado"].ToString();

                            if (estado == "Impagada")
                            {
                                reader.Close(); // Cerrar el lector antes de intentar otra operación.

                                // Actualizar el estado y la fecha de cobro
                                string updateQuery = "UPDATE facturas SET estado = 'Pagada', fechacobro = NOW() WHERE numFactura = @NumFactura";

                                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, con))
                                {
                                    updateCmd.Parameters.AddWithValue("@NumFactura", numFactura);
                                    int rowsAffected = updateCmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        // Mostrar mensaje de éxito
                                        Response.Write("Factura actualizada correctamente.");
                                        // Redirigir a la misma página después de la actualización
                                        Response.Redirect(Request.RawUrl);
                                    }
                                    else
                                    {
                                        // Mostrar mensaje de error
                                        Response.Write("No se pudo actualizar la factura.");
                                    }
                                }
                            }
                            else
                            {
                                // Mostrar mensaje de que no se puede actualizar el estado de la factura
                                Response.Write("No se puede actualizar el estado de la factura porque no está en estado 'Impagada'.");
                            }
                        }
                        else
                        {
                            // Mostrar mensaje de que no se encontró la factura
                            Response.Write("No se encontró la factura con el número proporcionado.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejar errores y mostrar mensaje de error
                    Response.Write("Error al procesar la solicitud: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }

    protected void AñadirFactura_Click(object sender, EventArgs e)
    {
        try
        {
            string conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            string numFactura = txtNumFactura.Text;
            DateTime fechaDeFactura = DateTime.Parse(txtFechaDeFactura.Text); // Formato 'dd/MM/yyyy'
            string cifCliente = txtCifCliente.Text;
            string nombreApellidos = txtNombreApellidos.Text;
            decimal importe = decimal.Parse(txtImporte.Text);
            decimal importeIVA = decimal.Parse(txtImporteIVA.Text);
            string moneda = txtMoneda.Text;
            string estado = ddlEstado.SelectedValue; // Asumiendo que es un DropDownList

            if (string.IsNullOrEmpty(numFactura) || string.IsNullOrEmpty(cifCliente) || string.IsNullOrEmpty(nombreApellidos))
            {
                Response.Write("Por favor, completa todos los campos obligatorios.");
                return;
            }

            using (MySqlConnection con = new MySqlConnection(conexion))
            {
                con.Open();

                string checkQuery = "SELECT EXISTS(SELECT 1 FROM facturas WHERE numFactura = @NumFactura)";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@NumFactura", numFactura);
                    bool exists = Convert.ToBoolean(checkCmd.ExecuteScalar());

                    if (exists)
                    {
                        Response.Write("Ya existe una factura con el mismo número. Por favor, elige otro número de factura.");
                        return;
                    }
                }

                string insertQuery = @"INSERT INTO facturas 
                                   (numFactura, fechaDeFactura, cifCliente, nombreApellidos, importe, importeIVA, moneda, estado) 
                                   VALUES 
                                   (@NumFactura, @FechaDeFactura, @CifCliente, @NombreApellidos, @Importe, @ImporteIVA, @Moneda, @Estado)";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@NumFactura", numFactura);
                    cmd.Parameters.AddWithValue("@FechaDeFactura", fechaDeFactura);
                    cmd.Parameters.AddWithValue("@CifCliente", cifCliente);
                    cmd.Parameters.AddWithValue("@NombreApellidos", nombreApellidos);
                    cmd.Parameters.AddWithValue("@Importe", importe);
                    cmd.Parameters.AddWithValue("@ImporteIVA", importeIVA);
                    cmd.Parameters.AddWithValue("@Moneda", moneda);
                    cmd.Parameters.AddWithValue("@Estado", estado);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        Response.Write("Factura añadida correctamente.");
                    }
                    else
                    {
                        Response.Write("No se pudo añadir la factura.");
                    }
                }

                con.Close();
            }
        }
        catch (FormatException)
        {
            Response.Write("Error de formato. Asegúrate de ingresar datos válidos.");
        }
        catch (MySqlException ex)
        {
            Response.Write("Error de MySQL: " + ex.Message);
        }
        catch (Exception ex)
        {
            Response.Write("Error: " + ex.Message);
        }
    }
}