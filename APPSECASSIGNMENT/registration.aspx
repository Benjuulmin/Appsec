<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registration.aspx.cs" Inherits="APPSECASSIGNMENT.registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            First Name<asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
        </div>
        Last Name<asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
        <p>
            Credit Card Detail<asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
        </p>
        <p>
            Email<asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
        </p>
        <p>
            Password<asp:TextBox ID="TextBox4" runat="server" onkeyup="validpassword()">

                    </asp:TextBox >
            <asp:Label ID="msg" runat="server"></asp:Label>
            </p>
        <p>
            DoB<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        </p>
        <p>
            Photo<asp:FileUpload ID="FileUpload1" runat="server" />
        </p>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
    </form>
</body>
</html>
<script type="text/javascript">
    function validpassword() {
        var vpass = document.getElementById('TextBox4').value;
        if (vpass.length <= 12) {
            document.getElementById('<%=msg.ClientID%>').innerHTML = "length is not longer than 12 inches";
            document.getElementById('<%=msg.ClientID%>').style.color = "Red";
            return ("too_short");
        }
        else if (vpass.search(/[0-9]/) == -1) {
            document.getElementById('<%=msg.ClientID%>').innerHTML = "need 1 number ";
            document.getElementById('<%=msg.ClientID%>').style.color = "Red";
            return "No number"

        }
        else if (vpass.search(/[$&+,:;=?@#|'<>.^*()%!-]/) == -1) {
            document.getElementById('<%=msg.ClientID%>').innerHTML = "Password must contain at least 1 special character";
            document.getElementById('<%=msg.ClientID%>').style.color = "Red";
            return "no special needs";

        }
        else if (vpass.search(/[A-Z]/) == -1) {
            document.getElementById('<%=msg.ClientID%>').innerHTML = "Password must contain at least 1 uppercase letter";
            document.getElementById('<%=msg.ClientID%>').style.color = "Red";
            return "No uppercase";
        }
        else if (vpass.search(/[a-z]/) == -1) {
            document.getElementById('<%=msg.ClientID%>').innerHTML = "Password must contain at least 1 lowercase letter";
            document.getElementById('<%=msg.ClientID%>').style.color = "Red";
            return "No lowercase ";
        }
        else {
            document.getElementById("msg").innerHTML = "Very excellent"
            document.getElementById('<%=msg.ClientID%>').style.color = "Blue";
        }

    }
</script>
