<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEditAgent.aspx.cs" Inherits="SALESCenterLivingKB.BaseData.AddEditAgent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Vertreter:</h3>
    <table>
        <tr>
            <td>
                <asp:Label ID="labelName1" runat="server">Name1:</asp:Label></td>
            <td>
                <asp:TextBox ID="tbName1" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="label3" runat="server">Name2:</asp:Label></td>
            <td>
                <asp:TextBox ID="tbName2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="label4" runat="server">Straße:</asp:Label></td>
            <td>
                <asp:TextBox ID="tbStreet" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="label5" runat="server">PLZ:</asp:Label></td>
            <td>
                <asp:TextBox ID="tbZIP" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="label6" runat="server">Ort:</asp:Label></td>
            <td>
                <asp:TextBox ID="tbCity" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="label7" runat="server">Telefon:</asp:Label></td>
            <td>
                <asp:TextBox ID="tbPhone" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="label8" runat="server">Fax:</asp:Label></td>
            <td>
                <asp:TextBox ID="tbFax" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="label9" runat="server">Email:</asp:Label></td>
            <td>
                <asp:TextBox ID="tbEMail" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="label10" runat="server">Bestätigungsemail:</asp:Label></td>
            <td>
                <asp:TextBox Width="800" ID="tbConfirm" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="saveButton" runat="server" Text="speichern" OnClick="saveButton_Click" CausesValidation="true" CssClass="btn btn-default" />
            </td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="RemoveAgentButton" runat="server" Text="löschen" OnClientClick="return confirm('Soll der Vertreter wirklich gelöscht werden?');" OnClick="RemoveAgentButton_Click" CausesValidation="false" />
    <asp:Label ID="LabelAddStatus" runat="server" Text=""></asp:Label>
</asp:Content>
