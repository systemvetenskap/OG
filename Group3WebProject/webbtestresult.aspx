﻿<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="webbtestresult.aspx.cs" Inherits="Group3WebProject.webbtestresult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents shadow">
        <h1 class="contentpages_h1">Resultat för senaste testet </h1>
        <br />

        <asp:Label ID="lblRes" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:GridView ID="GRID" runat="server"></asp:GridView>
        <br />
        <br />
        <asp:Panel ID="panData" runat="server">
        </asp:Panel>
    </div>
</asp:Content>
