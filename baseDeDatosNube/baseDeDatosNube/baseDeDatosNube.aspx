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
            <div class="form-row mt-5">
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <div class="col-md-3 mb-2">
                <asp:TextBox ID="FechaFactura" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:CalendarExtender ID="ceFecha" runat="server" TargetControlID="FechaFactura" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
            <div class="col-md-3 mb-2">
                <asp:TextBox ID="cifCliente" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-3 mb-2">
                <asp:TextBox ID="FechaPago" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender" runat="server" TargetControlID="FechaPago" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
            <div class="col-md-3 mb-2">
                <asp:DropDownList ID="Estado" runat="server" OnSelectedIndexChanged="Estado_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control">
                    <asp:ListItem Text="Todos" Value="-1" />
                    <asp:ListItem Text="Impagada" Value="0" />
                    <asp:ListItem Text="Pendiente" Value="1" />
                    <asp:ListItem Text="Pagada" Value="2" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-12">
                <asp:Button ID="Button1" runat="server" Text="Filtrar" OnClick="FiltrarDatos" CssClass="btn btn-primary mb-2" />
                <asp:Button ID="Button2" runat="server" Text="Borrar filtros" OnClick="limpiarFiltro" CssClass="btn btn-danger mb-2" />
            </div>
        </div>
            <asp:GridView ID="TablaDatos" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
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
