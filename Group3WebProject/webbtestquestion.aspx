<%@ Page Title="Webbtestquestion" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="webbtestquestion.aspx.cs" Inherits="Group3WebProject.webbtestquestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            <script src="scripts/getTime.js" type="text/javascript">   </script>
       <link rel="stylesheet" href="Style/styleForWEB.css" />
</asp:Content>

<%-- Body --%>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents shadow" >
        <div>
             <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            
            <p>
                Tid kvar: <b> <span id="timeLeft"></span> </b>                                
                
            </p>

        </div>
        <div>
            <asp:Label ID="Label1" runat="server" Text="Label">Välj frågor: </asp:Label>
            <asp:DropDownList ID="cmbChooseQue" runat="server" CssClass="dropdown-toggle" OnSelectedIndexChanged="cmbChooseQue_SelectedIndexChanged" AutoPostBack="True" Visible="False"></asp:DropDownList>
            <br />
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

