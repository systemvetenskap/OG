<%@ Page Title="Webbtestquestion" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="webbtestquestion.aspx.cs" Inherits="Group3WebProject.webbtestquestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<%-- Body --%>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents">
        <h1>Webbtestquestion</h1>
        <div>
            <h2>Välkommen till test tryck på start för att påbörja</h2>
            <p>Tid kvar: <span id="timeLeft">30:00</span> </p>
        </div>
        <div>
            <asp:Label ID="Label1" runat="server" Text="Label">Välj frågor: </asp:Label>
            <asp:DropDownList ID="cmbChooseQue" runat="server" CssClass="dropdown-toggle" OnSelectedIndexChanged="cmbChooseQue_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Spara" CssClass="btn-primary" />
        </div>
        <div id="qu">
            <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label><asp:Label ID="lblChoose" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:CheckBoxList ID="chkQuestionList" runat="server"></asp:CheckBoxList>
            <asp:RadioButtonList ID="rbQuestionList" runat="server" OnUnload="rbQuestionList_Unload"></asp:RadioButtonList>
        </div>
        <div>
            <asp:Button ID="btnPrevious" runat="server" Text="Föregående" OnClick="btnPrevious_Click" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnNext" runat="server" Text="Nästa" OnClick="btnNext_Click" />

        </div>
    </div>
</asp:Content>

