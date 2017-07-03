<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Discounts.aspx.cs" Inherits="SALESCenterLivingKB.BaseData.Discounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Sonderrabette:</h3>
    <asp:GridView runat="server" ID="SpecialDiscountGrid"
        ItemType="SALESCenterLivingKB.Models.SpecialDiscount" DataKeyNames="SpecialDiscountID"
        SelectMethod="SpecialDiscountGrid_GetData"
        AutoGenerateColumns="false"
        CssClass="table table-striped table-bordered table-hover"
        AutoGenerateEditButton="true"
        UpdateMethod="SpecialDiscountGrid_UpdateItem"
        OnRowUpdating="SpecialDiscountGrid_RowUpdating"
        AllowSorting="True">
        <Columns>
            <asp:DynamicField DataField="DiscountType" SortExpression="DiscountType" HeaderText="Typ" ReadOnly="true" />
            <asp:DynamicField DataField="Mandant" SortExpression="SpecialDiscountClientID" HeaderText="Mandant" ReadOnly="true" />
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
            <asp:DynamicField DataField="Discount" SortExpression="Discount" HeaderText="Rabatt" DataFormatString="{0:p2}" />
        </Columns>
    </asp:GridView>
</asp:Content>
