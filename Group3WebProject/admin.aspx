<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Group3WebProject.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents">
        <asp:GridView ID="previousTests" runat="server"></asp:GridView>
        <br />
        <asp:GridView ID="upcomingTests" runat="server"></asp:GridView>
    </div>
</asp:Content>
