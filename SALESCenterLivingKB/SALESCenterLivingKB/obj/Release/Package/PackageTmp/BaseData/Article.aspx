<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Article.aspx.cs" Inherits="SALESCenterLivingKB.BaseData.Article" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Artikelstamm:</h3>
    <hr />
    <%--<table>
        <tr>
            <td>
                <asp:TextBox ID="tbSearch" runat="server"></asp:TextBox></td>
            <td style="padding-left:20px">
                <asp:Button runat="server" Text="suchen" ID="searchButton" OnClick="searchButton_Click" /></td>
        </tr>
    </table>--%>
    <h4>Mandant:</h4>
    <asp:GridView runat="server" ID="GridViewClient"
        ItemType="SALESCenterLivingKB.Models.Client" DataKeyNames="ClientID"
        SelectMethod="GridViewClient_GetData"
        AutoGenerateColumns="false"
        AllowPaging="true"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false"
        OnSelectedIndexChanged="GridViewClient_SelectedIndexChanged">
        <Columns>
            <asp:ButtonField DataTextField="ClientName" CommandName="Select" HeaderText="Mandant" />
        </Columns>
        <SelectedRowStyle BackColor="LightCyan"
            ForeColor="DarkBlue"
            Font-Bold="true" />
    </asp:GridView>
    <h4>Modelle:</h4>
    <asp:GridView runat="server" ID="GridViewModel"
        ItemType="SALESCenterLivingKB.Models.Model" DataKeyNames="ModelID"
        AutoGenerateColumns="false"
        AllowPaging="true"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false"
        OnSelectedIndexChanged="GridViewModel_SelectedIndexChanged">
        <Columns>
            <asp:ButtonField DataTextField="ModelName" CommandName="Select" HeaderText="Modelname" />
            <asp:ButtonField DataTextField="ModelNumber" CommandName="Select" HeaderText="Modelnumber" />
            <asp:DynamicField DataField="ModelImage" />
            <asp:DynamicField DataField="FormName" />
            <asp:DynamicField DataField="FormNumber" />
            <asp:DynamicField DataField="ProductGroup" />
            <asp:DynamicField DataField="Width" />
            <asp:DynamicField DataField="SoleTypeName" />
        </Columns>
        <SelectedRowStyle BackColor="LightCyan"
            ForeColor="DarkBlue"
            Font-Bold="true" />
    </asp:GridView>

    <h4>Artikel:</h4>
    <asp:GridView runat="server" ID="articleGrid"
        ItemType="SALESCenterLivingKB.Models.Article" DataKeyNames="ArticleID"
        AutoGenerateColumns="false"
        AllowPaging="true"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false"
        OnSelectedIndexChanged="articleGrid_SelectedIndexChanged">
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

    <h4>Farben:</h4>
    <asp:GridView runat="server" ID="GridViewColor"
        ItemType="SALESCenterLivingKB.Models.Color" DataKeyNames="ColorID"
        AutoGenerateColumns="false"
        AllowPaging="true"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false"
        OnSelectedIndexChanged="GridViewColor_SelectedIndexChanged">
        <Columns>
            <asp:ButtonField DataTextField="ColorNumber" CommandName="Select" HeaderText="Farbnummer" />
            <asp:DynamicField DataField="ColorName" />
            <asp:DynamicField DataField="StockArticle" />
            <asp:DynamicField DataField="ProductionArticle" />
            <asp:DynamicField DataField="DeliveryDateStart" DataFormatString="{0:d}" />
            <asp:DynamicField DataField="DeliveryDateEnd" DataFormatString="{0:d}" />
            <asp:DynamicField DataField="EASColorNumber" />
            <asp:DynamicField DataField="EASColorName" />
            <asp:DynamicField DataField="ColorImage" />
            <asp:DynamicField DataField="CollectionName" />
            <asp:DynamicField DataField="ColorNumberText" />
            <asp:DynamicField DataField="ColorNumberSearchText" />
            <asp:DynamicField DataField="ColorStatus" />
            <asp:DynamicField DataField="ColorLiningName" />
            <asp:DynamicField DataField="ColorLiningName2" />
            <asp:DynamicField DataField="ColorLiningNumber" />
            <asp:DynamicField DataField="ColorLiningNumber2" />
        </Columns>
        <SelectedRowStyle BackColor="LightCyan"
            ForeColor="DarkBlue"
            Font-Bold="true" />
    </asp:GridView>

    <h4>Größengang:</h4>
    <asp:GridView runat="server" ID="GridViewSizerun"
        ItemType="SALESCenterLivingKB.Models.Sizerun" DataKeyNames="SizerunID"
        AutoGenerateColumns="false"
        AllowPaging="true"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false"
        OnSelectedIndexChanged="GridViewSizerun_SelectedIndexChanged">
        <Columns>
            <asp:ButtonField DataTextField="SizerunName" CommandName="Select" HeaderText="Größengang Name" />
            <asp:DynamicField DataField="SizerunNumber" />
            <asp:DynamicField DataField="Size01" />
            <asp:DynamicField DataField="Size02" />
            <asp:DynamicField DataField="Size03" />
            <asp:DynamicField DataField="Size04" />
            <asp:DynamicField DataField="Size05" />
            <asp:DynamicField DataField="Size06" />
            <asp:DynamicField DataField="Size07" />
            <asp:DynamicField DataField="Size08" />
            <asp:DynamicField DataField="Size09" />
            <asp:DynamicField DataField="Size10" />
            <asp:DynamicField DataField="Size11" />
            <asp:DynamicField DataField="Size12" />
            <asp:DynamicField DataField="Size13" />
            <asp:DynamicField DataField="Size14" />
            <asp:DynamicField DataField="Size15" />
            <asp:DynamicField DataField="Size16" />
            <asp:DynamicField DataField="Size17" />
            <asp:DynamicField DataField="Size18" />
            <asp:DynamicField DataField="Size19" />
            <asp:DynamicField DataField="Size20" />
            <asp:DynamicField DataField="Size21" />
            <asp:DynamicField DataField="Size22" />
            <asp:DynamicField DataField="Size23" />
            <asp:DynamicField DataField="Size24" />
            <asp:DynamicField DataField="Size25" />
            <asp:DynamicField DataField="Size26" />
            <asp:DynamicField DataField="Size27" />
            <asp:DynamicField DataField="Size28" />
            <asp:DynamicField DataField="Size29" />
            <asp:DynamicField DataField="Size30" />
        </Columns>
        <SelectedRowStyle BackColor="LightCyan"
            ForeColor="DarkBlue"
            Font-Bold="true" />
    </asp:GridView>
</asp:Content>
