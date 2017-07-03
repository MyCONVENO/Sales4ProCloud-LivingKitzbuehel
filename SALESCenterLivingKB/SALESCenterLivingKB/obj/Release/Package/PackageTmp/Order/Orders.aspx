<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="SALESCenterLivingKB.Order.Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Aufträge:</h3>
    <hr />
    <table>
        <tr>
            <td>Auftragsstatus: </td>
            <td style="padding-left: 20px">
                <asp:DropDownList runat="server" ID="ddOrderStatus">
                    <asp:ListItem Value="10">offene</asp:ListItem>
                    <asp:ListItem Value="0">abgeschlossene</asp:ListItem>
                    <asp:ListItem Value="-4">übertragen</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button runat="server" ID="FilterButton" Text="Filtern" OnClick="FilterButton_Click" /></td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="GridViewShoppingCart"
        ItemType="SALESCenterLivingKB.Models.ShoppingCart" DataKeyNames="ShoppingCartID"
        AutoGenerateColumns="false"
        AllowPaging="true"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false"
        OnSelectedIndexChanged="GridViewShoppingCart_SelectedIndexChanged"
        SelectMethod="GridViewShoppingCart_GetData">
        <Columns>
            <asp:ButtonField DataTextField="OrderNumber" CommandName="Select" HeaderText="Auftragsnummer" />
            <asp:DynamicField DataField="UserID" HeaderText="Benutzer" />
            <asp:DynamicField DataField="OrderDate" HeaderText="Auftragsdatum" DataFormatString="{0:d}" />
            <asp:DynamicField DataField="OrderTypeText" HeaderText="Auftragsart" />
            <asp:DynamicField DataField="CustomerNumber" HeaderText="Kundennummer" />
            <asp:DynamicField DataField="CustomerName2" HeaderText="Name1" />
            <asp:DynamicField DataField="CustomerName1" HeaderText="Name2" />
            <asp:DynamicField DataField="CustomerStreet" HeaderText="Straße" />
            <asp:DynamicField DataField="CustomerZIP" HeaderText="PLZ" />
            <asp:DynamicField DataField="CustomerCity" HeaderText="Ort" />
            <asp:DynamicField DataField="SentDateTime" HeaderText="übertragen am" DataFormatString="{0:d}" />
        </Columns>
        <SelectedRowStyle BackColor="LightCyan"
            ForeColor="DarkBlue"
            Font-Bold="true" />
    </asp:GridView>

    <h4>Positionen:</h4>
    <asp:GridView runat="server" ID="GridViewShoppingCartItem"
        ItemType="SALESCenterLivingKB.Models.ShoppingCartItem" DataKeyNames="ShoppingCartItemID"
        AutoGenerateColumns="false"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false">
        <Columns>
            <asp:DynamicField DataField="ArticleNumber" HeaderText="Artikelnummer" />
            <asp:DynamicField DataField="ColorNumber" HeaderText="Farbnummer" />
            <asp:DynamicField DataField="ColorName" HeaderText="Farbname" />
            <asp:DynamicField DataField="TotalQty" HeaderText="Paarzahl" />
            <asp:DynamicField DataField="DeliveryDecadeText" HeaderText="Liefertermin" />
        </Columns>
        <SelectedRowStyle BackColor="LightCyan"
            ForeColor="DarkBlue"
            Font-Bold="true" />
    </asp:GridView>
</asp:Content>
