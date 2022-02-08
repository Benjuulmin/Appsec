<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="APPSECASSIGNMENT.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Form </title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LduC2IeAAAAAMI3iHrmgoClSyDQZHxWMWA8jWBf"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Email<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        </div>
        Password<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
       
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
        </p>
         <asp:Label ID="balls" runat="server" EnableViewState="false"></asp:Label>
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LduC2IeAAAAAMI3iHrmgoClSyDQZHxWMWA8jWBf', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
