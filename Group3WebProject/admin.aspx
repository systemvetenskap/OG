<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Group3WebProject.admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="UTF-8" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyn" runat="server">
    <div class="all_contents shadow clearfix">
        <div class="gvPrevious">
            <h3 class="lblAdmin">Föregående test</h3>
            <asp:GridView ID="gvPreviousTests" runat="server"></asp:GridView>
            <br />
            <br />
        </div>
        <div class="gvUpcoming">
            <h3 class="lblAdmin">Kommande test</h3>
            <asp:GridView ID="gvUpcomingTests" runat="server"></asp:GridView>
            <br />
            <br />
        </div>

        <div id="gvS">
            <asp:GridView ID="gvStats" runat="server" OnRowDataBound="gvStats_RowDataBound" OnRowCreated="gvStats_RowCreated">
            </asp:GridView>
        </div>

        <%--            <div id="gvS">
                <asp:GridView ID="gvStats" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvStats_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Namn" HeaderText="Namn">
                            <HeaderStyle CssClass="locked col1"></HeaderStyle>
                            <ItemStyle CssClass="locked col1"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Provtyp" HeaderText="Provtyp">
                        <HeaderStyle CssClass="locked col2"></HeaderStyle>
                            <ItemStyle CssClass="locked col2"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Produkt" HeaderText="Produkt">
                        <HeaderStyle CssClass="scrolled"></HeaderStyle>
                            <ItemStyle CssClass="scrolled"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Ekonomi" HeaderText="Ekonomi">
                        <HeaderStyle CssClass="scrolled"></HeaderStyle>
                            <ItemStyle CssClass="scrolled"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Etik" HeaderText="Etik">
                        <HeaderStyle CssClass="scrolled"></HeaderStyle>
                            <ItemStyle CssClass="scrolled"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Total" HeaderText="Total">
                        <HeaderStyle CssClass="scrolled"></HeaderStyle>
                            <ItemStyle CssClass="scrolled"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Godkänd" HeaderText="Godkänd">
                        <HeaderStyle CssClass="scrolled"></HeaderStyle>
                            <ItemStyle CssClass="scrolled"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Giltigt t.o.m." HeaderText="Giltigt t.o.m.">
                        <HeaderStyle CssClass="scrolled"></HeaderStyle>
                            <ItemStyle CssClass="scrolled"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>--%>
        <%--<dx:ASPxGridView ID="gvStats" AutoGenerateColumns="false" DataSourceID="gvStats" OnRowDataBound="gvStats_RowDataBound">

            </dx:ASPxGridView>--%>
<%--        <p>
            <br />
        </p>--%>
        <div class="stats_div">
            <h3 class="lblAdmin">Statistik</h3>
            <div class="outer">
                <div class="inner" id="filen" runat="server">
                </div>
            </div>
        </div>
    </div>

    <%--</div>--%>
</asp:Content>
