<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="fr.cedricmartel.SampleItextSharp.DynColumns.Default" MasterPageFile="~/Master.Master" %>

<asp:Content ContentPlaceHolderID="PageContent" runat="server">
    <h2>
        Dynamic columns
        <asp:Button runat="server" Text="Generate PDF" OnClick="GenerationPdf" CssClass="btn btn-primary btn-sm btn-lg" />
    </h2>
    <p class="well">
        <asp:Label runat="server" ID="Resulat"></asp:Label>
    </p>
</asp:Content>
