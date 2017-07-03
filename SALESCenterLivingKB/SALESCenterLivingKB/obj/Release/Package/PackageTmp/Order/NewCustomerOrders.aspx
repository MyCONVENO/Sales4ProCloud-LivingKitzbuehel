<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewCustomerOrders.aspx.cs" Inherits="SALESCenterLivingKB.Order.NewCustomerOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Neukunden Aufträge:</h3>
    <hr />
    <h4>Mandant:</h4>
    <asp:GridView runat="server" ID="GridViewClient"
        ItemType="SALESCenterLivingKB.Models.Client" DataKeyNames="ClientID"
        SelectMethod="GridViewClient_GetData"
        AutoGenerateColumns="false"
        AllowPaging="true"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false"
        SelectedIndex="0"
        OnSelectedIndexChanged="GridViewClient_SelectedIndexChanged">
        <Columns>
            <asp:ButtonField DataTextField="ClientName" CommandName="Select" HeaderText="Mandant" />
        </Columns>
        <SelectedRowStyle BackColor="LightCyan"
            ForeColor="DarkBlue"
            Font-Bold="true" />
    </asp:GridView>
    <h4>Aufträge:</h4>
    <asp:GridView runat="server" ID="GridViewShoppingCart"
        ItemType="SALESCenterLivingKB.Models.ShoppingCart" DataKeyNames="ShoppingCartID"
        AutoGenerateColumns="false"
        AutoGenerateEditButton="true"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false"
        OnSelectedIndexChanged="GridViewShoppingCart_SelectedIndexChanged"
        SelectMethod="GridViewShoppingCart_GetData"
        UpdateMethod="GridViewShoppingCart_UpdateItem">
        <Columns>
            <asp:ButtonField DataTextField="OrderNumber" CommandName="Select" HeaderText="Auftragsnummer" />
            <asp:DynamicField DataField="UserID" ReadOnly="true" HeaderText="Benutzer" />
            <asp:DynamicField DataField="OrderDate" DataFormatString="{0:d}" ReadOnly="true" HeaderText="Auftragsdatum" />
            <asp:DynamicField DataField="OrderTypeText" ReadOnly="true" HeaderText="Auftragsart" />
            <asp:DynamicField DataField="CustomerNumber" HeaderText="Kundennummer" />
            <asp:DynamicField DataField="CustomerName2" ReadOnly="true" HeaderText="Name 1" />
            <asp:DynamicField DataField="CustomerName1" ReadOnly="true" HeaderText="Name 2" />
            <asp:DynamicField DataField="CustomerStreet" ReadOnly="true" HeaderText="Straße" />
            <asp:DynamicField DataField="CustomerZIP" ReadOnly="true" HeaderText="PLZ" />
            <asp:DynamicField DataField="CustomerCity" ReadOnly="true" HeaderText="Ort" />
            <asp:DynamicField DataField="CustomerCountryName" ReadOnly="true" HeaderText="Land" />
            <asp:DynamicField DataField="CustomerTaxID" ReadOnly="true" HeaderText="UStID" />
            <asp:DynamicField DataField="CustomerPhone" ReadOnly="true" HeaderText="Telefon" />
            <asp:DynamicField DataField="CustomerFax" ReadOnly="true" HeaderText="Fax" />
            <asp:DynamicField DataField="CustomerEMail" ReadOnly="true" HeaderText="EMail" />
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
