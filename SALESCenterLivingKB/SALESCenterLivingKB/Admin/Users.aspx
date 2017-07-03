<%@ Page Title="SALESCenterLivingKB" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="SALESCenterLivingKB.Admin.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h3>alle Benutzer:</h3>
    <asp:GridView runat="server" ID="usersGrid"
        ItemType="SALESCenterLivingKB.Models.User" DataKeyNames="UserID"
        SelectMethod="usersGrid_GetData"
        AutoGenerateColumns="false"
        AllowPaging="false"
        CssClass="table table-striped table-bordered table-hover"
        AllowSorting="True"        
        DeleteMethod="usersGrid_DeleteItem">
        <Columns>
            <asp:HyperLinkField DataTextField="UserID" HeaderText="Microsoft Account" SortExpression="UserID" DataNavigateUrlFormatString="EditUser.aspx?UserId={0}" DataNavigateUrlFields="UserID" />
            <asp:DynamicField DataField="Name1" SortExpression="Name1" HeaderText="Vorname" />
            <asp:DynamicField DataField="Name2" SortExpression="Name2" HeaderText="Nachname" />
            <asp:DynamicField DataField="Street" SortExpression="Street" HeaderText="Straße" />
            <asp:DynamicField DataField="ZIP" SortExpression="ZIP" HeaderText="PLZ" />
            <asp:DynamicField DataField="City" SortExpression="City" HeaderText="Ort" />
            <asp:DynamicField DataField="Phone" SortExpression="Phone" HeaderText="Telefon" />
            <asp:DynamicField DataField="Fax" SortExpression="Fax" HeaderText="Fax" />
            <asp:DynamicField DataField="Mobile" SortExpression="Mobile" HeaderText="Mobile" />
            <asp:DynamicField DataField="Email" SortExpression="Email" HeaderText="Email" />
            <asp:DynamicField DataField="ConfirmEmail" SortExpression="ConfirmEmail" HeaderText="Bestätigungsemailadressen" />
        </Columns>
    </asp:GridView>
</asp:Content>
