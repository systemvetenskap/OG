<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Group3WebProject.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="UTF-8" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents shadow clearfix">
        <div class="gvPrevious">
            <h3 class="lblAdmin">Föregående test</h3>
            <%--<asp:Label class="lblAdmin" ID="lblGvPrev" runat="server" Text="Föregående test"></asp:Label>--%>
            <asp:GridView ID="gvPreviousTests" runat="server"></asp:GridView>
            <br />
            <br />
        </div>
        <div class="gvUpcoming">
            <h3 class="lblAdmin">Kommande test</h3>
            <%--<asp:Label class="lblAdmin" ID="lblGvUp" runat="server" Text="Kommande test"></asp:Label>--%>
            <asp:GridView ID="gvUpcomingTests" runat="server"></asp:GridView>
            <br />
            <br />
        </div>
        <div class="gvS">
            <h3 class="lblAdmin">Föregående test</h3>
            <%--<asp:Label class="lblAdmin" ID="lblGvStat" runat="server" Text="Statistik"></asp:Label>--%>
            <asp:GridView ID="gvStats" runat="server" OnRowDataBound="gvStats_RowDataBound"></asp:GridView>
        </div>
    </div>
</asp:Content>
