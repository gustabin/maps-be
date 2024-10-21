<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Default.Master" AutoEventWireup="true" CodeBehind="Chequeo.aspx.cs" Inherits="Isban.Maps.Host.Webapi.Web.Chequeo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titles">Información
    </h1>
    <div style="color: red;">
        <asp:Label runat="server" ID="lblError" />
    </div>
    <div class="grid grid-table">
        <h2>
            
        </h2>
         <div style="font-size: 16px;">
        <label>Estado:</label>&nbsp;<label runat="server" id="lblEstado"> </label>    
        </div>
        <asp:GridView ID="gvResultado" runat="server" DataKeyNames="BasedeDatos" AutoGenerateColumns="False"
            Width="100%" CssClass="tabled">
            <AlternatingRowStyle CssClass="columnalter" />
            <Columns>
                <asp:BoundField DataField="BasedeDatos" HeaderText="Base de Datos" />
                <asp:BoundField DataField="ServidorDB" HeaderText="Servidor DB" />
                <asp:BoundField DataField="UsuarioDB" HeaderText="Usuario DB" />
                <asp:BoundField DataField="ServidorWin" HeaderText="Servidor OS" />
                <asp:BoundField DataField="UsuarioWin" HeaderText="Usuario OS" />
                <asp:BoundField DataField="ConnectionString" HeaderText="ConnectionString" />
                <asp:BoundField DataField="Hash" HeaderText="Hash" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
