<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forma1.aspx.cs" Inherits="Laboras2.Forma1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="App_Themes/TemaA/Style.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 732px">
            &nbsp;&nbsp;
            Domantas Greičius IFF 8/3
            <br />
            &nbsp;
            L2-U9 Prenumerata<br />
            &nbsp;
            <asp:Label ID="Label4" runat="server" Text="Įveskite, kurio mėnesio duomenis norite matyti:"></asp:Label>
&nbsp;&nbsp;
            <asp:TextBox ID="TextBox1" type="number" min="1" max="12" runat="server" Height="16px" Width="65px"></asp:TextBox>
            <br />
            &nbsp;
            <asp:Label ID="Label5" runat="server" Text="Įveskite minimalų to mėnesio krūvį:"></asp:Label>
&nbsp;
            <asp:TextBox ID="TextBox2" type="number" min="1" max="30" runat="server" Height="16px" Width="65px" ></asp:TextBox>
            <br />
            <br />
&nbsp;<asp:Button ID="Button1" runat="server" Height="41px" OnClick="Button1_Click" Text="Vykdyti programą!" Width="235px" />
&nbsp;&nbsp;  
            <br />        
            <asp:Label ID="Label1" runat="server" Text="Pradiniai duomenys"></asp:Label>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Agentų sąrašas"></asp:Label>
&nbsp;<asp:Table ID="Table1" runat="server" BorderStyle="Solid" GridLines="Both" Height="50px" Width="53px" BackColor="White">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Kodas</asp:TableCell>
                    <asp:TableCell runat="server">pavarde</asp:TableCell>
                    <asp:TableCell runat="server"></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
&nbsp;<asp:Label ID="Label3" runat="server" Text="Prenumeratorių sąrašas"></asp:Label>
            <asp:Table ID="Table2" runat="server" GridLines="Both" Height="47px" Width="50px" BackColor="White" BorderStyle="Solid">
            </asp:Table>
            <br />
            <asp:Label ID="Label8" runat="server"></asp:Label>
            <br />
            <asp:Label ID="Label6" runat="server" Text="Surikiuotas agentų sąrašas, kurie dirba daugiau nei vidutinis krūvis šitame mėnesyje:"></asp:Label>
            <br />
            <asp:Table ID="Table3" runat="server" BackColor="White" BorderStyle="Solid" GridLines="Both">
            </asp:Table>
            <br />
            <br />
            <asp:Label ID="Label9" runat="server" Text="Sarašas agentų su to mėnesio prenumeratoriais"></asp:Label>
            <asp:Table ID="Table5" runat="server" BackColor="White" GridLines="Both" BorderStyle="Solid">
            </asp:Table>
            <br />
            <asp:Label ID="Label7" runat="server" Text="Agentai, kuriems pasikeitė krūvis, paskirsčius pašalintų agentų krūvį:"></asp:Label>
            <asp:Table ID="Table4" runat="server" BackColor="White" BorderStyle="Solid" GridLines="Both">
            </asp:Table>
        &nbsp;
            <asp:Button ID="Button2" runat="server" Height="47px" OnClick="Button2_Click" Text="Reset" Width="107px" />
        </div>
       </form>     
</body>
</html>
