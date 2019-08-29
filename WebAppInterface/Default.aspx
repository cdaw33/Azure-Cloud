<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebAppInterface._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PDF Converter</title>
    <link rel="stylesheet" href="https://www.w3.org/StyleSheets/Core/Chocolate" type="text/css"/>
    <style type="text/css">
        .auto-style1 {
            font-size: xx-large;
        }
        .auto-style2 {
            color: #FFFFFF;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <div class="auto-style1">
            <span class="auto-style2">CRAIG'S</span> PDF CONVERTER <span class="auto-style2">WEB APP</span>
        </div>
        <br />
        <div>
            Upload Word Document:
            <asp:FileUpload ID="upload" runat="server" />
            <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="submitButton_Click" />
        </div>
        <br />
            <asp:Label runat="server" ID="ErrorMsg" Text="Please use a .txt file" Visible="false"/>
        <div>
            <asp:Button ID="RefreshButton" runat="server" Text="Refresh" OnClick="refresh_Click"/>
        <br />
            <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <br />
                    <asp:ListView ID="PDFDisplayControl" runat="server"  >
                        <ItemTemplate>                            
                            <!--Fills Hyperlink with link to stored PDF's and displays the name of the saved file-->
                            <asp:HyperLink id="hyperlink1" runat="server" NavigateUrl='<%# Eval("Url") %>' Text='<%# Eval("Title") %>' />
                            <br />
                        </ItemTemplate>
                    </asp:ListView>                   
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>