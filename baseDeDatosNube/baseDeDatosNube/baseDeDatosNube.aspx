<%@ Page Language="C#" AutoEventWireup="true" CodeFile="baseDeDatosNube.aspx.cs" Inherits="Proyecto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <asp:TextBox ID="FechaFactura" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="ceFecha" runat="server" TargetControlID="FechaFactura" Format="dd/MM/yyyy"></asp:CalendarExtender>
            <asp:TextBox ID="cifCliente" runat="server"></asp:TextBox>
            <asp:TextBox ID="FechaPago" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender" runat="server" TargetControlID="FechaPago" Format="dd/MM/yyyy"></asp:CalendarExtender>
            <asp:DropDownList ID="Estado" runat="server" OnSelectedIndexChanged="Estado_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Text="Todos" Value="-1" />
                <asp:ListItem Text="Impagada" Value="0" />
                <asp:ListItem Text="Pendiente" Value="1" />
                <asp:ListItem Text="Pagada" Value="2" />
            </asp:DropDownList>
            <asp:Button ID="aplicarFiltro" runat="server" Text="Filtrar" OnClick="FiltrarDatos"/>
            <asp:Button ID="borrarFiltro" runat="server" Text="Borrar filtros" OnClick="limpiarFiltro"/>
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
