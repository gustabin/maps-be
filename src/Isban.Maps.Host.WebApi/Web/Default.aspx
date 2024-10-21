<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Isban.Maps.Host.Webapi.Web.Default" %>


<div>
    <ul>
        <li><%= System.DateTime.Now.ToString() %></li>
        <li><%Response.Write("Current Culture is " + System.Globalization.CultureInfo.CurrentCulture.Name); %></li>
        <%Isban.Mercados.DataAccess.OracleClient.HelperDataAccess.ClearCache(); %>
        <%Isban.Mercados.WebApiClient.WebApiUriHelper.Instance.Clear(); %>
        <%Oracle.ManagedDataAccess.Client.OracleConnection.ClearAllPools();%>
        <%Isban.Maps.Entity.Helpers.Cache.Clear(); %>
    </ul>
</div>

