<%@ Page Title="Webbtestquestion" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="webbtestquestion.aspx.cs" Inherits="Group3WebProject.webbtestquestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            <script src="scripts/getTime.js" type="text/javascript">   </script>
       <link rel="stylesheet" href="Style/styleForWEB.css" />
</asp:Content>

<%-- Body --%>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents shadow" style="-webkit-touch-callout: none; -webkit-user-select: none; -khtml-user-select: none; -moz-user-select: none; -ms-user-select: none; user-select: none;">
        <h1 class="contentpages_h1">Webbtestquestion</h1>
        <div>
             <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <h2 class="contentpages_h2">Välkommen till test tryck på start för att påbörja</h2>
            <p>
                Tid kvar: <span id="timeLeft"></span>         
                                  
                
            </p>

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
            <p id="notRight">&nbsp;</p>
            <asp:Button ID="btnPrevious" runat="server" Text="Föregående" OnClick="btnPrevious_Click" CssClass="btn" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnNext" runat="server" Text="Nästa" OnClick="btnNext_Click" CssClass="btn" />

        </div>
    </div>
</asp:Content>

