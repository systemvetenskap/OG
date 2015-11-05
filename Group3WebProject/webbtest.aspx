<%@ Page Title="Webbtest" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="webbtest.aspx.cs" Inherits="Group3WebProject.webbtest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<%-- Body --%>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <script type="text/javascript">
        
    </script>
    <div class="all_contents shadow">
        <h1 class="contentpages_h1">Webbtest</h1>
        <br />
        <%--<h2 class="contentpages_h2" id="doTest"></h2>--%>
        <p>
            När du startat ett test så har du bara <b><u>30</u></b> minuter på dig att genomföra testet. <br />
            Om du inte blir godkänd på testet så måste du vänta minst en vecka innan du kan göra testet igen.
            <br />
            <br />
        </p>
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <asp:Button runat="server" Text="Välj test" OnClick="Unnamed1_Click" ID="btnTest" CssClass="btn" /><asp:Button ID="btnSeeLastTest" runat="server" Text="Se senaste provet" CssClass="btn" OnClick="btnSeeLastTest_Click" /><br />
        <%--<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>--%>
    </div>
</asp:Content>
