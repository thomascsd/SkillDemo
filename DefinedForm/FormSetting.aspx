<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormSetting.aspx.cs" Inherits="DefinedForm_FormSetting" %>

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
    <title>自定表單維護</title>
    <meta name="viewport" content="width=device-width,initial-scale=1">
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.16/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://jqueryui.com/themeroller/themeswitchertool/"></script>
    <script src="../js/libs/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/ques/controlEditorBase.js" type="text/javascript"></script>
    <script src="../js/ques/controlHelper.js" type="text/javascript"></script>
    <script src="../js/ques/definedFormSetting.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var hfMaster = $("#hfMaster");
            formObject.createControls(hfMaster.val());

            $("#tabs").tabs({
                select: function (event, ui) {
                    switch (ui.index) {
                        case 0:  //自定表單
                            $("#divContainer").html("");
                            formObject.createContent(hfMaster.val());
                            break;
                        case 1: //自定表單排序
                            $("#divSort").html("");
                            formObject.createSortContent(hfMaster.val());
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
        div.sideMenu { position: absolute; /*border: solid 1px #999;*/ width: 50px; text-align: center; }
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
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">
                <asp:Label ID="Label1" Text="自定表單" runat="server" CssClass="" />
            </a></li>
            <li><a href="#tabs-2">
                <asp:Label ID="Label2" Text="自定表單排序" runat="server" CssClass="" />
            </a></li>
        </ul>
        <div id="tabs-1">
            <div style="width: 100%">
                <div style="width: 50%; margin: 10px auto;">
                    <asp:Label ID="Label3" Text="表單標題" runat="server" />
                    <asp:TextBox ID="txtTitle" runat="server" Width="350" />
                </div>
            </div>
            <hr />
            <div id="divContainer">
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
