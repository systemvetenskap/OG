<%@ Page Title="Logga in" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Group3WebProject.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <asp:DropDownList ID="ddlAllUser" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <br />
    <asp:Button ID="btnLogin" runat="server" Text="Button" OnClick="btnLogin_Click" />
</asp:Content>
