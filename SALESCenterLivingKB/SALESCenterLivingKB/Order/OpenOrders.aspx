<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OpenOrders.aspx.cs" Inherits="SALESCenterLivingKB.Order.OpenOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>offene Aufträge:</h3>
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
        AllowPaging="true"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false"
        SelectMethod="GridViewShoppingCart_GetData">
        <Columns>
            <asp:ButtonField DataTextField="OrderNumber" CommandName="Select" HeaderText="Auftragsnummer" />
            <asp:DynamicField DataField="UserID" />
            <asp:DynamicField DataField="OrderDate" DataFormatString="{0:d}" />
            <asp:DynamicField DataField="OrderTypeText" />
            <asp:DynamicField DataField="CustomerNumber" />
            <asp:DynamicField DataField="CustomerName2" />
            <asp:DynamicField DataField="CustomerName1" />
            <asp:DynamicField DataField="CustomerZIP" />
            <asp:DynamicField DataField="CustomerCity" />
            <asp:DynamicField DataField="CustomerStreet" />
        </Columns>
        <SelectedRowStyle BackColor="LightCyan"
            ForeColor="DarkBlue"
            Font-Bold="true" />
    </asp:GridView>

    <h4>Positionen:</h4>
    <asp:GridView runat="server" ID="GridViewShoppingCartItem"
        ItemType="SALESCenterLivingKB.Models.ShoppingCartItem" DataKeyNames="ShoppingCartItemID"
        AutoGenerateColumns="false"
        AllowPaging="true"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false">
        <Columns>
            <asp:ButtonField DataTextField="ArticleNumber" CommandName="Select" HeaderText="Artikelnummer" />
            <asp:DynamicField DataField="ArticleName" HeaderText="Artikelname" />
            <asp:DynamicField DataField="ArticleMaterial" HeaderText="Artikelmaterial" />
            <asp:DynamicField DataField="ArticleGroupe1" HeaderText="Artikelgruppe 1" />
            <asp:DynamicField DataField="ArticleGroupe2" HeaderText="Artikelgruppe 2" />
            <asp:DynamicField DataField="ArticleHeelTypeNumber" />
            <asp:DynamicField DataField="ArticleClosingTypeNumber" />
            <asp:DynamicField DataField="ArticleClosingType" />
            <asp:DynamicField DataField="ArticleTypeNumber" />
            <asp:DynamicField DataField="ArticleTypeName" />
            <asp:DynamicField DataField="ArticleEASMaterial" />
            <asp:DynamicField DataField="ArticleEASMaterialNumber" />
            <asp:DynamicField DataField="ArticleEASTypeName" />
            <asp:DynamicField DataField="ArticleEASTypeNumber" />
            <asp:DynamicField DataField="ArticleHeelTypeName" />
            <asp:DynamicField DataField="ArticleCategory1" />
            <asp:DynamicField DataField="ArticleCategory2" />
            <asp:DynamicField DataField="ArticleCategory3" />
            <asp:DynamicField DataField="ArticleImage" />
            <asp:DynamicField DataField="ArticleInfoText1" />
            <asp:DynamicField DataField="ArticleInfoText2" />
        </Columns>
        <SelectedRowStyle BackColor="LightCyan"
            ForeColor="DarkBlue"
            Font-Bold="true" />
    </asp:GridView>
</asp:Content>
