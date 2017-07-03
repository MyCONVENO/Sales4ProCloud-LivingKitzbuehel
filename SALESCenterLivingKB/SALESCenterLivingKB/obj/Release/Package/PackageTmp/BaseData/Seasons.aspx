<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Seasons.aspx.cs" Inherits="SALESCenterLivingKB.BaseData.Seasons" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h3>Sonderrabette:</h3>
    <asp:GridView runat="server" ID="SeasonGrid"
        ItemType="SALESCenterLivingKB.Models.Season" DataKeyNames="SeasonID"
        SelectMethod="SeasonGrid_GetData"
        AutoGenerateColumns="false"
        CssClass="table table-striped table-bordered table-hover"
        AutoGenerateEditButton="true"
        UpdateMethod="SeasonGrid_UpdateItem"
        OnRowUpdating="SeasonGrid_RowUpdating"
        AllowSorting="True">
        <Columns>
            <asp:DynamicField DataField="SeasonName" SortExpression="SeasonName" HeaderText="Saison Name" ReadOnly="true" />
            <asp:DynamicField DataField="SeasonNumber" SortExpression="SeasonNumber" HeaderText="Saison Nummer" ReadOnly="true" />
            <asp:DynamicField DataField="SeasonLongName" SortExpression="SeasonLongName" HeaderText="Saison Name (lange Bezeichnung)" />
            <asp:TemplateField HeaderText="Beginn" ItemStyle-VerticalAlign="Top">

                <ItemTemplate>
                    <%#Eval("StartDateText")%>
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:TextBox
                        Text='<%#Eval("StartDateTextPlain")%>'
                        ID="tbStartDate"
                        TextMode="Date"
                        runat="server" />
                </EditItemTemplate>

            </asp:TemplateField>
             <asp:TemplateField HeaderText="Ende" ItemStyle-VerticalAlign="Top">

                <ItemTemplate>
                    <%#Eval("EndDateText")%>
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:TextBox
                        Text='<%#Eval("EndDateTextPlain")%>'
                        ID="tbEndDate"
                        TextMode="Date"
                        runat="server" />
                </EditItemTemplate>

            </asp:TemplateField>            
        </Columns>
    </asp:GridView>
</asp:Content>
