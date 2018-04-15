<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="ShowQues.aspx.cs" Inherits="Questionnaire_ShowQues" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js"
        type="text/javascript"></script>
    <script src="js/ques/questionnaireUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#hfAnswerMasterPK").val("0");
            $("a.showQuestionnaire").hide();

            $("#prev").click(function () {
                var index = -1, isTransfer = false;
                quesUIObject.checkButtion();

                if (quesUIObject.index > 0) {
                    if (quesUIObject.lastIndex != -1) {
                        index = quesUIObject.lastIndex;
                        isTransfer = true;
                    }
                    quesUIObject.setProcess(index, isTransfer);
                }
            })
            $("#next").click(function () {
                quesUIObject.checkButtion();

                if (quesUIObject.sld.contains(quesUIObject.index)) {
                    var div = $("div.cnt:eq(" + quesUIObject.index + ")");
                    var hfTransfer = div.find(".qForceTransfer");
                    var trans = quesUIObject.getIndex(quesUIObject.index, hfTransfer.val());
                    var index = 1;

                    quesUIObject.lastIndex = quesUIObject.index;
                    if (trans.isTransfer) {
                        index = trans.index;
                    }

                    quesUIObject.saveData();
                    quesUIObject.setProcess(index, trans.isTransfer)
                }
            })

            $("#issue li").click(function () {
                var radio = $(this).find(".qList");

                if (radio.is(":radio") && radio.siblings(".qListText").size() == 0) {
                    quesUIObject.setProcess(1);
                }
            }).hover(function () {
                $(this).addClass("hover")
            }, function () {
                $(this).removeClass("hover")
            });

        });
    </script>
    <link href="css/questionnaireUI.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        div.left { float: left; width: 30%; margin-right:20px; }
        div.right { float: left; width: 50%</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="left">
        <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" OnPreRender="gvList_PreRender"
            OnSelectedIndexChanged="gvList_SelectedIndexChanged" CssClass="data" Width="100%">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton Text="選取" CommandName="Select" ID="btnSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Heading" HeaderText="標題" />
                <asp:BoundField DataField="Category" HeaderText="類型" />
                <asp:BoundField DataField="StartDate" DataFormatString="{0:d}" HeaderText="開始日期" />
                <asp:BoundField DataField="EndDate" DataFormatString="{0:d}" HeaderText="結束日期" />
            </Columns>
        </asp:GridView>
    </div>
    <asp:Panel runat="server" ID="pnlRight" CssClass="right">
        <asp:Panel ID="pnlMain" CssClass="main" runat="server">
            <div style="margin: 0 auto; padding: 0; width: 100%; overflow: hidden;">
                <div style="margin: 0 0 10px 0; width: 100%;">
                    <table border="0" style="margin: 0; width: 100%;">
                        <tr style="background: #4f90c2;">
                            <td align="left" valign="middle" style="margin: 0; padding-left: 5px; color: #ffffff;
                                font-size: 22px; font-weight: bold; line-height: 1.8em;">
                                <asp:Label ID="lblHeading" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding-left: 5px; line-height: 1.5em; font-size: 13px;">
                                <div style="width: 100%; line-height: 1.5em; font-size: 13px; overflow: hidden;">
                                    <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="pnlQues" CssClass="warp visible" runat="server">
                    <div class="issue" id="issue">
                    </div>
                    <div class="ctrl">
                        <div class="btns">
                            <span title="Previous" class="prev" id="prev">Previous</span> <span title="Next"
                                class="next" id="next">Next</span>
                        </div>
                        <div class="prog" id="prog">
                            <div class="ptip" id="tips">
                                <span></span>
                            </div>
                            <div class="ress" id="ress">
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlList" CssClass="visible" runat="server">
                    <div id="issue" class="qtList">
                    </div>
                </asp:Panel>
            </div>
            <asp:Panel ID="pnlChart" runat="server" CssClass="aspShowChart">
                <asp:HyperLink ID="hlChart" NavigateUrl="javascript:void(0);" CssClass="aspChartResult art-button ShowQuestionnaireResult"
                    runat="server" Style="cursor: pointer;">顯示問卷統計結果</asp:HyperLink>
            </asp:Panel>
            <asp:HiddenField ID="hfMasterPK" runat="server" />
            <asp:HiddenField ID="hfComment" runat="server" />
            <asp:HiddenField ID="hfBeginShowChart" runat="server" />
            <asp:HiddenField ID="hfChartResult" runat="server" />
            <asp:HiddenField ID="hfShowChart" runat="server" />
            <asp:HiddenField ID="hfQuestionnaireType" runat="server" />
            <input id="hfAnswerMasterPK" type="hidden" value="0" />
        </asp:Panel>
        <div id="ChartResult">
            <h1 style="text-align: center; font-size: 14pt;">
                <asp:Literal ID="litChartResult" runat="server"></asp:Literal>
            </h1>
            <iframe id="iframeChartResult" frameborder="0" width="850px" height="650px"></iframe>
            <div>
                <span class="art-button-wrapper"><span class="l"></span><span class="r"></span><a
                    href="javascript:void(0);" class="art-button" id="showQuestionnaire" style="cursor: pointer;">
                    <asp:Label ID="Label1" Text="填寫問卷" CssClass="CompleteQuestionnaire" runat="server" />
                </a></span>
            </div>
        </div>
        <asp:Label ID="lblMessage" runat="server" CssClass="QuestionnaireExpired" Text="此問卷已失效"
            Style="display: none;" Font-Size="14px"></asp:Label>
    </asp:Panel>
</asp:Content>
