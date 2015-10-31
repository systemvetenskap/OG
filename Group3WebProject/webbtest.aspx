<%@ Page Title="Webbtest" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="webbtest.aspx.cs" Inherits="Group3WebProject.webbtest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<%-- Body --%>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <script type="text/javascript">
        
    </script>
    <div class="all_contents">
        <h1>Webbtest</h1>
        <h2 id="doTest"></h2>
        <p>
            När du väl startat ett test så har du bara 30minuter på dig så gäller att du har tiden för dig. Om du inte lyckas med testet så får du vänta en vecka innan du kan göra testet igen. 
            Du får göra ett test nu. 
        </p>
    
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:Button runat="server" Text="Välj test" OnClick="Unnamed1_Click" ID="btnTest"/><br /><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </div>
</asp:Content>
