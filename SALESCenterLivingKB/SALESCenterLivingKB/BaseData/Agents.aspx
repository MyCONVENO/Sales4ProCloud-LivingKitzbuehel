<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Agents.aspx.cs" Inherits="SALESCenterLivingKB.BaseData.Agents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h3>alle Vertreter:</h3>
    <asp:GridView runat="server" ID="agentsGrid"
        ItemType="SALESCenterLivingKB.Models.Agent" DataKeyNames="AgentID"
        SelectMethod="agentsGrid_GetData"
        AutoGenerateColumns="false"        
        CssClass="table table-striped table-bordered table-hover"        
        AllowSorting="True">
        <Columns>            
            <asp:HyperLinkField DataTextField="AgentNumber" HeaderText="Gebiet" SortExpression="AgentNumber" DataNavigateUrlFormatString="AddEditAgent.aspx?AgentID={0}" DataNavigateUrlFields="AgentID" />
            <asp:DynamicField DataField="AgentName1" SortExpression="AgentName1" HeaderText="Vorname" />
            <asp:DynamicField DataField="AgentName2" SortExpression="AgentName2" HeaderText="Nachname" />
            <asp:DynamicField DataField="AgentStreet" SortExpression="AgentStreet" HeaderText="Straße" />
            <asp:DynamicField DataField="AgentZIP" SortExpression="AgentZIP" HeaderText="PLZ" />
            <asp:DynamicField DataField="AgentCity" SortExpression="AgentCity" HeaderText="Ort" />
            <asp:DynamicField DataField="AgentPhone" SortExpression="AgentPhone" HeaderText="Telefon" />
            <asp:DynamicField DataField="AgentFax" SortExpression="AgentFax" HeaderText="Fax" />
            <asp:DynamicField DataField="AgentMobile" SortExpression="AgentMobile" HeaderText="Mobile" />
            <asp:DynamicField DataField="AgentEMail" SortExpression="AgentEMail" HeaderText="Email" />
            <asp:DynamicField DataField="Mandant" SortExpression="AgentClientID" HeaderText="Mandant" />
            <asp:DynamicField DataField="AgentConfirmEmail" SortExpression="ConfirmEmail" HeaderText="Bestätigungsemailadressen" />
        </Columns>
    </asp:GridView>
</asp:Content>
