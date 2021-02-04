<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="STConnect_192152K.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            height: 26px;
            width: 1359px;
        }
        .auto-style3 {
            width: 230px;
        }
        .auto-style4 {
            height: 26px;
            width: 230px;
        }
        .auto-style6 {
            height: 26px;
            width: 168px;
        }
        .auto-style7 {
            width: 230px;
            height: 31px;
        }
        .auto-style8 {
            width: 168px;
            height: 31px;
        }
        .auto-style9 {
            height: 31px;
            width: 1359px;
        }
        .auto-style11 {
            width: 168px;
        }
        .auto-style13 {
            width: 1359px;
        }
    </style>

    <script type="text/javascript">
        function validatepass() {
            var pass = document.getElementById("<%= tb_pass.ClientID %>").value; 

            if (pass.length < 8) {
                document.getElementById("lbl_pass_status").innerHTML = "Password need to have at least 8 characters";
                document.getElementById("lbl_pass_status").style.color = "Red";
            }
            else if (pass.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pass_status").innerHTML = "Password need to have at least 1 lower case character";
                document.getElementById("lbl_pass_status").style.color = "Red";
            }
            else if (pass.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pass_status").innerHTML = "Password need to have at least 1 Upper case character";
                document.getElementById("lbl_pass_status").style.color = "Red";
            }
            else if (pass.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pass_status").innerHTML = "Password need to have at least 1 Numeral character";
                document.getElementById("lbl_pass_status").style.color = "Red";
            }
            else if (pass.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById("lbl_pass_status").innerHTML = "Password need to have at least 1 Special character";
                document.getElementById("lbl_pass_status").style.color = "Red";
            }
            else {
                document.getElementById("lbl_pass_status").innerHTML = "Excellecnt Password";
                document.getElementById("lbl_pass_status").style.color = "Green";
            }
        }

        function matchpass() {
            var pass = document.getElementById("<%= tb_pass.ClientID %>").value; 
            if (pass != document.getElementById("<%= tb_cfmpass.ClientID %>").value) {
                document.getElementById("lbl_cfmpass_status").innerHTML = "Passwords do not match";
                document.getElementById("lbl_cfmpass_status").style.color = "Red";
            }
            else {
                document.getElementById("lbl_cfmpass_status").innerHTML = "Passwords match";
                document.getElementById("lbl_cfmpass_status").style.color = "green";
            }
        }

        function validatefname() {
            var fname = document.getElementById("<%= tb_fname.ClientID %>").value;

            if (fname.length < 3) {
                document.getElementById("lbl_fname_status").innerHTML = "First name need to have at least 3 characters";
                document.getElementById("lbl_fname_status").style.color = "Red";
            }
            else if (fname.search(/^[A-Za-z]/) == -1) {
                document.getElementById("lbl_lname_status").innerHTML = "First name only contains alphabets";
                document.getElementById("lbl_lname_status").style.color = "Red";
            }
            else if (fname.length > 20) {
                document.getElementById("lbl_fname_status").innerHTML = "First name cannot be more than 20 characters";
                document.getElementById("lbl_fname_status").style.color = "Red";
            }
            else {
                document.getElementById("lbl_fname_status").innerHTML = "Valid name";
                document.getElementById("lbl_fname_status").style.color = "Green";
            }
            
        }

        function validatelname() {
            var fname = document.getElementById("<%= tb_lname.ClientID %>").value;

            if (fname.length < 3) {
                document.getElementById("lbl_lname_status").innerHTML = "last name need to have at least 3 characters";
                document.getElementById("lbl_lname_status").style.color = "Red";
            }
            else if (fname.search(/^[A-Za-z]/) == -1) {
                document.getElementById("lbl_lname_status").innerHTML = "last name only contains alphabets";
                document.getElementById("lbl_lname_status").style.color = "Red";
            }
            else if (fname.length > 20) {
                document.getElementById("lbl_lname_status").innerHTML = "last name cannot be more than 20 characters";
                document.getElementById("lbl_lname_status").style.color = "Red";
            }
            else {
                document.getElementById("lbl_lname_status").innerHTML = "Valid name";
                document.getElementById("lbl_lname_status").style.color = "Green";
            }

        }

        function validateEmail() {
            var Email = document.getElementById("<%= tb_email.ClientID %>").value;

          
            if (Email.search(/^([A-Za-z0-9_\-\.]+)@([A-Za-z0-9_\-\.]+)\.([A-Za-z]{1,})$/) == -1) {
                document.getElementById("lbl_email_status").innerHTML = "Invalid Email";
                document.getElementById("lbl_email_status").style.color = "Red";
            }
            
            else {
                    document.getElementById("lbl_email_status").innerHTML = "Valid Email";
                    document.getElementById("lbl_email_status").style.color = "Green";
            }

        }

        function validateCardNo() {
            var CardNo = document.getElementById("<%= tb_credit_card_no.ClientID %>").value;


            if (CardNo.search(/^[0-9]/) == -1) {
                document.getElementById("lbl_credit_status").innerHTML = "Credit card Number can only have numerals";
                document.getElementById("lbl_credit_status").style.color = "Red";
            }

            else if (CardNo.length < 16 || CardNo.length > 16) {
                document.getElementById("lbl_credit_status").innerHTML = "Credit card Number can only have 16 characters";
                document.getElementById("lbl_credit_status").style.color = "Red";
            }

            else {
                document.getElementById("lbl_credit_status").innerHTML = "Valid Card Number";
                document.getElementById("lbl_credit_status").style.color = "Green";
            }

        }

        function validateCVC() {
            var CVC = document.getElementById("<%= tb_credit_card_cvc.ClientID %>").value;


            if (CVC.search(/^[0-9]/) == -1) {
                document.getElementById("lbl_cvc_status").innerHTML = "Credit card CVC can only have numerals";
                document.getElementById("lbl_cvc_status").style.color = "Red";
            }

            else if (CVC.length < 3 ) {
                document.getElementById("lbl_cvc_status").innerHTML = "Credit card CVC can not have less than 3 characters";
                document.getElementById("lbl_cvc_status").style.color = "Red";
            }

            else if (CVC.length > 4) {
                document.getElementById("lbl_cvc_status").innerHTML = "Credit card CVC can not have more than 4 characters";
                document.getElementById("lbl_cvc_status").style.color = "Red";
            }

            else {
                document.getElementById("lbl_cvc_status").innerHTML = "Valid CVC";
                document.getElementById("lbl_cvc_status").style.color = "Green";
            }

        }
    </script>
     
    <script src="https://www.google.com/recaptcha/api.js?render=6Lezv0kaAAAAAF9NQ5KCuf0mDwGlt276IcZ6Bibl"></script>
     
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Registration</h1>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Name" runat="server" Text="First Name"></asp:Label>
                        :</td>
                    <td class="auto-style11">
                        <asp:TextBox ID="tb_fname" runat="server" onkeyup="javascript:validatefname()"></asp:TextBox>
                    </td>
                    <td class="auto-style13">
                        <asp:Label ID="lbl_fname_status" runat="server"></asp:Label>
                    </td>
                </tr>
                 <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label8" runat="server" Text="Last Name"></asp:Label>
                        :</td>
                    <td class="auto-style11">
                        <asp:TextBox ID="tb_lname" runat="server" onkeyup="javascript:validatelname()"></asp:TextBox>
                    </td>
                    <td class="auto-style13">
                        <asp:Label ID="lbl_lname_status" runat="server"></asp:Label>
                     </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label7" runat="server" Text="Email"></asp:Label>
                        :</td>
                    <td class="auto-style11">
                        <asp:TextBox ID="tb_email" runat="server" onkeyup="javascript:validateEmail()"></asp:TextBox>
                    </td>
                    <td class="auto-style13">
                        <asp:Label ID="lbl_email_status" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Date of birth:</td>
                    <td class="auto-style11">
                        <asp:TextBox ID="tb_dob" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style13">
                        <asp:Label ID="lbl_dob_status" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Password" runat="server" Text="Password" ></asp:Label>
                        :</td>
                    <td class="auto-style11">
                        <asp:TextBox ID="tb_pass" runat="server" onkeyup="javascript:validatepass()"></asp:TextBox>
                    </td>
                    <td class="auto-style13">
                        <asp:Label ID="lbl_pass_status" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style7">
                        <asp:Label ID="Label3" runat="server" Text="Confirm Password"></asp:Label>
                        :</td>
                    <td class="auto-style8">
                        <asp:TextBox ID="tb_cfmpass" runat="server" onkeyup="javascript:matchpass()"></asp:TextBox>
                    </td>
                    <td class="auto-style9">
                        <asp:Label ID="lbl_cfmpass_status" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label4" runat="server" Text="Credit card number"></asp:Label>
                        :</td>
                    <td class="auto-style11">
                        <asp:TextBox ID="tb_credit_card_no" runat="server" onkeyup="javascript:validateCardNo()"></asp:TextBox>
                    </td>
                    <td class="auto-style13">
                        <asp:Label ID="lbl_credit_status" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="Label5" runat="server" Text="Credit card Expiry Date"></asp:Label>
                        :</td>
                    <td class="auto-style6">
                        <asp:TextBox ID="tb_credit_card_expire" runat="server" ></asp:TextBox>
                    </td>
                    <td class="auto-style2">
                        <asp:Label ID="lbl_expiry_status" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label6" runat="server" Text="Credit card CVV"></asp:Label>
                        :</td>
                    <td class="auto-style11">
                        <asp:TextBox ID="tb_credit_card_cvc" runat="server" onkeyup="javascript:validateCVC()"></asp:TextBox>
                    </td>
                    <td class="auto-style13">
                        <asp:Label ID="lbl_cvc_status" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        &nbsp;</td>
                    <td class="auto-style11">
                        <asp:Button ID="btn_submit" runat="server" Text="Submit" OnClick="btn_submit_Click" />
                    </td>
                    <td class="auto-style13">
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
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
