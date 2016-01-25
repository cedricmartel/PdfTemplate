<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="fr.cedricmartel.SampleItextSharp.Default" MasterPageFile="~/Master.Master" %>

<asp:Content ContentPlaceHolderID="PageContent" runat="server">
    <br/>
    <p>
        This project is a sample of the following libraries usage :
    </p>
    <ul>
        <li>ITextSharp <a href="http://sourceforge.net/projects/itextsharp/">http://sourceforge.net/projects/itextsharp/</a>
        </li>
    </ul>
    <ul>
        <li>ITextSharp PdfTemplates <a href="https://pdftemplate.codeplex.com/">https://pdftemplate.codeplex.com/</a>
        </li>
    </ul>
    <p>
        Pdf files are generated in Output folder 
    </p>
    <p>
        <asp:Button runat="server" ID="EmptyOutput" Text="Empty output folder" CssClass="btn btn-primary btn-sm btn-lg" OnClick="EmptyOutput_OnClick" />
        <asp:Label runat="server" ID="EmptyOutputResult"></asp:Label>
    </p>
</asp:Content>
