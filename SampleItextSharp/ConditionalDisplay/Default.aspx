<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="fr.cedricmartel.SampleItextSharp.ConditionalDisplay.Default" MasterPageFile="../Master.Master" %>

<asp:Content ContentPlaceHolderID="PageContent" runat="server">
    <h2>Conditionnal display
    </h2>
    <asp:CheckBox runat="server" ID="Condition" Text="Show conditional content" Checked="true" />

    <asp:Button runat="server" Text="Generate PDF" OnClick="GenerationPdf" CssClass="btn btn-primary btn-sm btn-lg" />

    <p class="well">
        <asp:Label runat="server" ID="Resulat"></asp:Label>
    </p>
</asp:Content>
