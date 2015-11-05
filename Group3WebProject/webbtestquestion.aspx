﻿<%@ Page Title="Webbtestquestion" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="webbtestquestion.aspx.cs" Inherits="Group3WebProject.webbtestquestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<%-- Body --%>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents shadow">
        <h1 class="contentpages_h1">Webbtestquestion</h1>
        <div>
             <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <h2 class="contentpages_h2">Välkommen till test tryck på start för att påbörja</h2>
            <p>
                Tid kvar: <span id="timeLeft">                    
                    <asp:Timer ID="time1" runat="server" Interval="100" OnTick="time1_Tick"></asp:Timer>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="time1" EventName="Tick" />
                        </Triggers>
                        <ContentTemplate>
                              <asp:Label ID="lblTime" runat="server" Text="Label"></asp:Label>
                        </ContentTemplate>                      
                    </asp:UpdatePanel>                   
                </span>
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
            <asp:Button ID="btnPrevious" runat="server" Text="Föregående" OnClick="btnPrevious_Click" CssClass="btn" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnNext" runat="server" Text="Nästa" OnClick="btnNext_Click" CssClass="btn" />

        </div>
    </div>
</asp:Content>

