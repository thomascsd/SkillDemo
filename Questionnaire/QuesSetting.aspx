<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QuesSetting.aspx.cs" Inherits="QuesSetting" %>

<!doctype html>
<!--[if lt IE 7]> <html class="no-js ie6 oldie" lang="en"> <![endif]-->
<!--[if IE 7]>    <html class="no-js ie7 oldie" lang="en"> <![endif]-->
<!--[if IE 8]>    <html class="no-js ie8 oldie" lang="en"> <![endif]-->
<!--[if gt IE 8]><!-->
<html class="no-js" lang="zh">
<!--<![endif]-->
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>問卷維護</title>
    <meta name="viewport" content="width=device-width,initial-scale=1">
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.16/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://jqueryui.com/themeroller/themeswitchertool/"></script>
    <script src="../js/libs/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/ques/controlEditorBase.js" type="text/javascript"></script>
    <script src="../js/ques/controlHelper.js" type="text/javascript"></script>
    <script src="../js/ques/quesSettingD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#switcher').themeswitcher({
                loadTheme: "sunny"
            });
            var hfMaster = $("#hfMaster");
            queObject.createControls(hfMaster.val());
            var options = {
                showOn: 'button',
                buttonImage: '../images/calendar.gif',
                buttonImageOnly: true,
                showAnim: "slideDown"
            };
            $("#txtStartDate,#txtEndDate").datepicker(options);
            $(".CommentForm textarea.CommentInputBox").maxlength({ maxCharacters: 500, slider: true, statusText: "" });
            $("#tabs").tabs({
                select: function (event, ui) {
                    switch (ui.index) {
                        case 0:  //問卷內容
                            $("#divContainer").html("");
                            queObject.createContent(hfMaster.val());
                            break;
                        case 1: //問卷內容排序
                            $("#divSort").html("");
                            queObject.createSortContent(hfMaster.val());
                            break;
                        default:
                            return false;
                            break;
                    }

                }
            });

        });    
    
    </script>
    <link type="text/css" rel="stylesheet" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.16/themes/sunny/jquery-ui.css" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #divContainer, #divSort { width: 100%; }
        div.innerContainer { background-color: #fff; border: dotted 2px #999; width: 50%; margin: 10px auto; padding: 8px; }
        #tableSort { width: 50%; margin: 0 auto; }
        .cButton { width: 30px; }
        div.footer { margin-top: 6px; }
        div.sideMenu { position: absolute; width: 50px; text-align: center; }
        div.sideMenu a.sideCmd { display: block; margin: 2px; }
        .style1 { width: 100%; }
        input.qEle { width: 300px; }
        .gridTitle { font-weight: bold; color: Blue; }
        ul.itemList { list-style-type: none; padding-left: 0px; }
        #divTransferMenu { background-color: #fff; border: 1px solid #999; position: absolute; z-index: 100; width: 100px; padding: 5px; display: none; }
        #divTransferMenu a { text-decoration: none; font-size: 11px; display: block; width: 100%; }
        div.ui-datepicker { font-size: 12px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="switcher">
    </div>
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">問卷內容 </a></li>
            <li><a href="#tabs-2">問卷內容排序 </a></li>
        </ul>
        <div id="tabs-1">
            <div class="CommentForm" style="width: 100%;">
                <table class="style1">
                    <tr>
                        <td align="right">
                            問卷標題
                        </td>
                        <td>
                            <asp:TextBox ID="txtHeading" runat="server" CssClass="CommentInputBox" MaxLength="150"
                                Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            說明
                        </td>
                        <td>
                            <asp:TextBox ID="txtDesc" runat="server" Columns="40" Rows="4" TextMode="MultiLine"
                                CssClass="CommentInputBox" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            問卷結果顯示文字
                        </td>
                        <td>
                            <asp:TextBox ID="txtComment" runat="server" Columns="40" Rows="4" TextMode="MultiLine"
                                CssClass="CommentInputBox" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            開始日期
                        </td>
                        <td>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="CommentInputBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            結束日期
                        </td>
                        <td>
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="CommentInputBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            問卷類型</td>
                        <td>
                            <asp:DropDownList ID="ddlCategory" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            題否問卷只填寫一次</td>
                        <td>
                            <asp:CheckBox ID="ckbOneTime" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divContainer">
            </div>
            <div id="divTransferMenu">
            </div>
        </div>
        <div id="tabs-2">
            <div id="divSort">
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfMaster" runat="server" Value="0" />
    <asp:HiddenField ID="hfUserPK" runat="server" />
    </form>
</body>
</html>
