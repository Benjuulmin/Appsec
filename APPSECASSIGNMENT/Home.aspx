<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="APPSECASSIGNMENT.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            First Name:
            <asp:Label ID="Fname" runat="server" Text="Label"></asp:Label>
            <br />
            Last Name:
            <asp:Label ID="lname" runat="server" Text="Label"></asp:Label>
            <br />
            Credit Card
            <asp:Label ID="creditcard" runat="server" Text="Label"></asp:Label>
            <br />
            Email:
            <asp:Label ID="email" runat="server" Text="Label"></asp:Label>
            <br />
            Date of birth:
            <asp:Label ID="Dob" runat="server" Text="Label"></asp:Label>
            <br />
            <br />
            <asp:Button ID="LogOut" runat="server" Text="Log Out " OnClick="LogoutMe" />
        </div>
    </form>
</body>
</html>
