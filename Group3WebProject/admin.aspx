<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Group3WebProject.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="UTF-8" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents shadow clearfix">
        <div class="gvPrevious">
            <asp:GridView ID="gvPreviousTests" runat="server"></asp:GridView>
            <br />
        </div>
        <div class="gvUpcoming">
            <asp:GridView ID="gvUpcomingTests" runat="server"></asp:GridView>
            <br />
        </div>
        <div class="gvStats">
            <asp:GridView ID="gvStats" runat="server" OnRowDataBound="gvStats_RowDataBound"></asp:GridView>
        </div>
    </div>
</asp:Content>
