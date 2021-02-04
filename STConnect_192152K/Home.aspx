<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="STConnect_192152K.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

           <h1> <asp:Label ID="lbl_login_status" runat="server" Text="Login"></asp:Label></h1>
            <asp:Label ID="lbl_welcome" runat="server" Text="Welcome:" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="lbl_email" runat="server" Text="Email" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="lbl_DOB" runat="server" Text="DOB" Visible="False"></asp:Label>
            <br />
            <asp:HyperLink ID="hl_register" runat="server" NavigateUrl="~/Register" Visible="False">Register</asp:HyperLink>
            <br />
            <asp:Button ID="btn_logout" runat="server" OnClick="btn_logout_Click" Text="Log Out" Visible="False" />
            <br />
            <asp:Button ID="btn_Login" Text="login" runat="server" OnClick="btn_Login_Click" Visible="False" Width="84px"  />
        </div>
    </form>
</body>
</html>
