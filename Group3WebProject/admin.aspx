<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Group3WebProject.admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="UTF-8" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents shadow clearfix">
        <%--<div class="gvPrevious">--%>
            <%--<h3 class="lblAdmin">Föregående test</h3>--%>
            <%--<asp:GridView ID="gvPreviousTests" runat="server"></asp:GridView>--%>
            <%--<br />
            <br />--%>
        <%--</div>--%>

        <h1 class="contentpages_h1">Provledare</h1>
        <h2 class="contentpages_h2">Här kan du som provleadre se föregående och kommande test för dina teammedlemmar. Även statistik finns längre ned.</h2>
        <div class="prev_upcoming">
            <div class="stats_div tests_div previoustesttabell">
                <div class="outer">
                    <h3 class="lblAdmin">Föregående test</h3>
                    <div class="inner" id="prev" runat="server">
                    </div>
                </div>
            </div>
            <div class="gvUpcoming">
                <%--<h3 class="lblAdmin">Kommande test</h3>--%>
                <%--<asp:GridView ID="gvUpcomingTests" runat="server"></asp:GridView>--%>
                <br />
                <br />
            </div>
            <div class="stats_div tests_div">
                <div class="outer">
                    <h3 class="lblAdmin">Kommande test</h3>
                    <div class="inner" id="upcom" runat="server">
                    </div>
                </div>
            </div>
        </div>

        <%--<div id="gvS">
            <asp:GridView ID="gvStats" runat="server" OnRowDataBound="gvStats_RowDataBound" OnRowCreated="gvStats_RowCreated">
            </asp:GridView>
        </div>--%>
        <div class="stats_div">
            <h3 class="lblAdmin">Statistik</h3>
            <asp:DropDownList ID="ddlTests" runat="server" Autopostback="True" OnSelectedIndexChanged="ddlTests_SelectedIndexChanged"></asp:DropDownList>
            <asp:Button ID="btnShowTest" runat="server" Text="Visa" OnClick="btnShowTest_Click" CssClass="btn" />
            <div class="outer">
                <div class="inner" id="filen" runat="server">
                </div>
            </div>
        </div>
    </div>

    <%--</div>--%>
</asp:Content>
