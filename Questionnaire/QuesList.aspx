<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="QuesList.aspx.cs" Inherits="QuesList" %>

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
        CssClass="ui-widget data" OnPreRender="gvList_PreRender">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink NavigateUrl="javascript:void(0);" ID="hlUpdate" Text="編輯" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Heading" HeaderText="標題" />
            <asp:BoundField DataField="Category" HeaderText="類型" />
            <asp:BoundField DataField="StartDate" DataFormatString="{0:d}" HeaderText="開始日期" />
            <asp:BoundField DataField="EndDate" DataFormatString="{0:d}" HeaderText="結束日期" />
        </Columns>
    </asp:GridView>
</asp:Content>
