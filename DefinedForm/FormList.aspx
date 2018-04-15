<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="FormList.aspx.cs" Inherits="DefinedForm_FormList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        div.add { width: 75%; margin-left: 20px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="add">
        <asp:HyperLink ID="hlAdd" NavigateUrl="javascript:void(0);" runat="server">新增</asp:HyperLink>
    </div>
    <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvList_RowDataBound"
        CssClass="data" Width="50%" onprerender="gvList_PreRender" >
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink NavigateUrl="javascript:void(0);" ID="hlUpdate" Text="修改" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Title" HeaderText="標題" />
            <asp:BoundField DataField="CreateDate" HeaderText="建立日期" 
                DataFormatString="{0:yyyy/MM/dd HH:mm}" />
        </Columns>
    </asp:GridView>
</asp:Content>
