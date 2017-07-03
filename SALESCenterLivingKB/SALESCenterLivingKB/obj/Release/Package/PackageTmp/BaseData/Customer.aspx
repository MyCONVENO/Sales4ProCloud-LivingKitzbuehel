<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="SALESCenterLivingKB.BaseData.Customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Kundenstamm:</h3>
    <hr />
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
    <h4>Kunden:</h4>
    <table>
        <tr>
            <td>
                <asp:TextBox ID="tbFilter" runat="server" CssClass="form-control"/>
            </td>
            <td>
                <asp:Button ID="FilterCommand" runat="server" OnClick="FilterCommand_Click" Text="suchen" CssClass="btn btn-default" />
            </td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="GridViewCustomer"
        ItemType="SALESCenterLivingKB.Models.Customer" DataKeyNames="CustomerID"
        AutoGenerateColumns="false"
        AllowPaging="true"
        CssClass="table table-striped table-bordered table-hover"
        ShowHeaderWhenEmpty="false"        
        OnPageIndexChanging="GridViewCustomer_PageIndexChanging"
        OnSelectedIndexChanged="GridViewCustomer_SelectedIndexChanged">
        <Columns>
            <asp:ButtonField DataTextField="CustomerNumber" CommandName="Select" HeaderText="Kundennummer" />            
            <asp:DynamicField DataField="CustomerName2" />
            <asp:DynamicField DataField="CustomerName1" />
            <asp:DynamicField DataField="CustomerName3" />
            <asp:DynamicField DataField="CustomerStreet" />
            <asp:DynamicField DataField="CustomerZIP" />
            <asp:DynamicField DataField="CustomerCity" />
        </Columns>
        <SelectedRowStyle BackColor="LightCyan"
            ForeColor="DarkBlue"
            Font-Bold="true" />
    </asp:GridView>   
</asp:Content>
