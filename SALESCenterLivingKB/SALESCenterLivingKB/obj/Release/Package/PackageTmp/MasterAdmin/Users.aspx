<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="SALESCenterLivingKB.MasterAdmin.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <h3>Benutzer:</h3>
    <hr />
    <table>
        <tr>
            <td>
                <asp:Label runat="server">Filter:</asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownRoles" runat="server"
                    ItemType="SALESCenterLivingKB.Models.SimpleRole"
                    DataTextField="Name" DataValueField="Id">
                </asp:DropDownList>                
            </td>
            <td >
                 <asp:Label style="margin-left:20px" runat="server">Suche:</asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbSearchPattern"/>
            </td>
            <td>
                <asp:Button style="margin-left:20px" ID="filterButton" runat="server" OnClick="filterButton_Click" Text="Filtern" CssClass="btn btn-default" />
            </td>
        </tr>
    </table>
    <br />
    <asp:Button ID="addButton" runat="server" OnClick="addButton_Click" Text="hinzufügen" CssClass="btn btn-default" />
    <asp:GridView runat="server" ID="usersGrid"
        ItemType="SALESCenterLivingKB.Models.ApplicationUser" DataKeyNames="Id"
        AutoGenerateColumns="false"        
        AllowPaging="true"
        OnPageIndexChanging="usersGrid_PageIndexChanging"
        CssClass="table table-striped table-bordered table-hover"
        OnRowCommand="usersGrid_RowCommand"
        OnRowDeleting="usersGrid_RowDeleting">
        <Columns>
            <asp:ButtonField HeaderText ="" ButtonType="Button" CommandName="delete" Text="löschen"/>
            <asp:HyperLinkField DataTextField="Email" HeaderText="Benutzername" SortExpression="Email" DataNavigateUrlFormatString="AddEditAccounts.aspx?UserId={0}" DataNavigateUrlFields="Id" />            
        </Columns>
    </asp:GridView>
</asp:Content>
