<%@ Page Language="C#" AutoEventWireup="true" CodeFile="baseDeDatosNube.aspx.cs" Inherits="Proyecto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Base de Datos en la Nube</title>
</head>
<body>
    <form id="formulario" runat="server" class="container">
        <div>
            <h3>Filtrar facturas</h3>
            <div class="form-row mt-1">
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <div class="col-md-3 mb-2">
                <asp:TextBox ID="FechaFactura" runat="server" placeholder="Fecha de la factura" CssClass="form-control"></asp:TextBox>
                <asp:CalendarExtender ID="ceFecha" runat="server" TargetControlID="FechaFactura" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
            <div class="col-md-3 mb-2">
                <asp:TextBox ID="cifCliente" runat="server" placeholder="Cliente" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-3 mb-2">
                <asp:TextBox ID="FechaPago" runat="server" placeholder="Fecha de cobro" CssClass="form-control"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender" runat="server" TargetControlID="FechaPago" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
            <div class="col-md-2 mb-2">
                <asp:DropDownList ID="Estado" runat="server" OnSelectedIndexChanged="Estado_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control">
                    <asp:ListItem Text="Todos" Value="-1" />
                    <asp:ListItem Text="Impagada" Value="0" />
                    <asp:ListItem Text="Pagada" Value="1" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-12">
                <asp:Button ID="Button1" runat="server" Text="Filtrar" OnClick="FiltrarDatos" CssClass="btn btn-primary mb-2" />
                <asp:Button ID="Button2" runat="server" Text="Borrar filtros" OnClick="limpiarFiltro" CssClass="btn btn-danger mb-2" />
            </div>
            <div>
                <h3>Actualizar estado de factura</h3>
                <asp:TextBox ID="idFactura" runat="server" placeholder="Id de la factura" CssClass="form-control"></asp:TextBox>
                <asp:Button ID="Button3" runat="server" Text="Actualizar estado" OnClick="modificarEstado" CssClass="btn btn-primary mt-2 mb-2" />
            </div>
        </div>
        <div>
        <h3>Añadir Nueva Factura</h3>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label for="txtNumFactura">Número de Factura:</label>
                    <asp:TextBox ID="txtNumFactura" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label for="txtFechaDeFactura">Fecha de Factura:</label>
                    <asp:TextBox ID="txtFechaDeFactura" runat="server"  CssClass="form-control"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaDeFactura" Format="dd/MM/yyyy"></asp:CalendarExtender>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label for="txtCifCliente">CIF Cliente:</label>
                    <asp:TextBox ID="txtCifCliente" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label for="txtNombreApellidos">Nombre y Apellidos:</label>
                    <asp:TextBox ID="txtNombreApellidos" runat="server" CssClass="form-control" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label for="txtImporte">Importe:</label>
                    <asp:TextBox ID="txtImporte" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label for="txtImporteIVA">Importe con IVA:</label>
                    <asp:TextBox ID="txtImporteIVA" runat="server" CssClass="form-control" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label for="txtMoneda">Moneda:</label>
                    <asp:TextBox ID="txtMoneda" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label for="ddlEstado">Estado:</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Impagada" Value="Impagada" />
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <asp:Button ID="btnAñadirFactura" runat="server" Text="Añadir Factura" OnClick="AnadirFactura_Click" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <asp:Label ID="lblMensaje" runat="server" ForeColor="Green" />
                </div>
            </div>
        </div>
            <asp:GridView ID="TablaDatos" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="text-center">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
