<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="STConnect_192152K.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 202px;
        }
        .auto-style2 {
            width: 100%;
        }
    </style>
    <script src="https://www.google.com/recaptcha/api.js?render="></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Login</h1>
            <table class="auto-style2">
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="Label1" runat="server" Text="Email"></asp:Label>
                        :</td>
                    <td>
                        <asp:TextBox ID="tb_login_email" runat="server"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
                        :</td>
                    <td>
                        <asp:TextBox ID="tb_login_pass" runat="server" ></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td>
                        <asp:Label ID="lbl_msg" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td>
                        <asp:Button ID="btn_login" runat="server" Text="Login" OnClick="btn_login_Click" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <script>
                            function onClick(e) {
                                e.preventDefault();
                                grecaptcha.ready(function () {
                                    grecaptcha.execute('6Lezv0kaAAAAAF9NQ5KCuf0mDwGlt276IcZ6Bibly', { action: 'submit' }).then(function (token) {
                                    // Add your logic to submit to your backend server here.
                                        document.getElementById("g-recaptcha-response").value = token;
                                    });
                                });
                            }
            </script>
                         <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
        </div>
    </form>
</body>
</html>

