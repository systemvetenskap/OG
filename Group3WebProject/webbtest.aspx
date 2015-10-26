<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="webbtest.aspx.cs" Inherits="Group3WebProject.webbtest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Vänlighen välj ett test i dropdown lista nedan</h2>
    <p>
        När du väl startat ett test så har du bara 30minuter på dig så gäller att du har tiden för dig. Om du inte lyckas med testet så får du vänta en vecka innan du kan göra testet igen. 
        Du får göra ett test nu och snabbt.
    </p>
    <asp:GridView runat="server" ID="gridTest">

    </asp:GridView>
    <asp:Button runat="server" Text="Välj test" OnClick="Unnamed1_Click"/><br /><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
</asp:Content>
