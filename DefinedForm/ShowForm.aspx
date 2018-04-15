<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="ShowForm.aspx.cs" Inherits="DefinedForm_ShowForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.7/jquery.validate.min.js"
        type="text/javascript"></script>
    <script src="js/ques/controlUIHelper.js" type="text/javascript"></script>
    <script src="js/ques/definedFormUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#pnlMessage").hide();
            $("#tabs").tabs({});
            var masterPK = $("#hfFormPk").val();
            if (masterPK) {
                formUI.createContent(masterPK);
            }

        });
        
    </script>
    <link href="css/DefinedForm.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        div.left { float: left; width: 30%; margin-right: 15px; }
        div.right { float: left; width: 50%; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="left">
        <asp:GridView ID="gvForm" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvForm_RowDataBound"
            OnSelectedIndexChanged="gvForm_SelectedIndexChanged" OnPreRender="gvForm_PreRender"
            CssClass="data" Width="100%">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton Text="選擇" CommandName="Select" ID="btnSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink NavigateUrl="javascript:void(0)" Text="結果" ID="hlResult" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Title" HeaderText="標題" />
            </Columns>
        </asp:GridView>
    </div>
    <asp:Panel runat="server" ID="pnlRight" CssClass="right">
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1">
                    <h2>
                        <asp:Literal ID="litTitle" runat="server" />
                    </h2>
                </a></li>
            </ul>
        </div>
        <div id="tabs-1">
            <asp:Panel ID="pnlForm" runat="server" ClientIDMode="Static">
            </asp:Panel>
        </div>
        <asp:Panel runat="server" ID="pnlMessage" ClientIDMode="Static">
            <asp:Label ID="Label1" Text="已填寫完成" runat="server" />
        </asp:Panel>
        <asp:HiddenField ID="hfFormPk" ClientIDMode="Static" Value="0" runat="server" />
    </asp:Panel>
</asp:Content>
