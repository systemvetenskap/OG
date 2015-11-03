﻿<%@ Page Title="Logga in" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Group3WebProject.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents shadow">
        <div class="center_login">
            <h3 class="lblLogIn">Välkommen, vänligen logga in</h3>
            <asp:DropDownList ID="ddlAllUser"  runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            <br />
            <%--<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>--%>
            <br />
            <asp:Button ID="btnLogin" runat="server" Text="Logga in" OnClick="btnLogin_Click" CssClass="btn" />
        </div>
    </div>
</asp:Content>
