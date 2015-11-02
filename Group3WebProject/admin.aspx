<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Group3WebProject.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="UTF-8" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents shadow">
        <asp:GridView ID="gvPreviousTests" runat="server"></asp:GridView>
        <br />
        <asp:GridView ID="gvUpcomingTests" runat="server"></asp:GridView>
        <br />
        <asp:GridView ID="gvStats" runat="server" OnRowDataBound="gvStats_RowDataBound"></asp:GridView>
    </div>
</asp:Content>
