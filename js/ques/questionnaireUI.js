///<reference path="jquery.js">
/// <reference path="jquery.cookie.js" />

var quesUIObject = {
    index: 0,
    lastIndex: -1,
    sld: [],
    len: 0,
    progress: 100,
    ht: 500,
    speed: 500,
    gridGroupData: null,
    currentDetailPK: 0,
    masterPK: 0,
    cookieOption: { expires: 1000, path: '/' },
    qStatus: "qStatus",
    qStep: "qStep",
    showChartResult: false,
    qType: '',   //問卷類型
    validator: null,
    transferNumbers: [],

    callAjax: function (url, jsonData, callbackOnSuccess) {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: jsonData,
            success: callbackOnSuccess,
            error: function (data) {
                alert(data.responseText);
            }

        });
    },
    checkButtion: function () {
        if (this.index <= 0) {
            $("#prev").addClass("noprev");
        } else {
            $("#prev").removeClass("noprev");
        }
        if (!this.sld.contains(this.index)) {
            $("#next").addClass("nonext");
        } else {
            $("#next").removeClass("nonext");
        }
    },
    checkItem: function (container) {
        var $this = this;

        if (!container) {
            container = $("#issue");
        }

        $(".qText", container).keyup(function () {
            if (!$this.sld.contains($this.index)) {
                $this.sld.push($this.index);
            }

            $this.checkButtion();
        });

        $(".qList", container).click(function () {
            if (!$this.sld.contains($this.index)) {
                $this.sld.push($this.index);
            }
            if ($this.qType == questionnaireType.process) {
                if ($(this).is(":radio") && $(this).siblings(".qListText").size() == 0) {
                    var hfTrans = $(this).parents("span").find(".qTransferNumber");
                    var trans = $this.getIndex($this.index, hfTrans.val());
                    var index = 1;

                    $this.lastIndex = $this.index;
                    if (trans.isTransfer) {
                        index = trans.index;
                    }

                    $this.saveData();
                    $this.setProcess(index, trans.isTransfer);
                }
            }

            $this.checkButtion();
        });

        $(".qGrid", container).click(function () {
            if (!$this.sld.contains($this.index)) {
                $this.sld.push($this.index);
            }
            $this.checkButtion();
        });

    },
    createContent: function (masterPK, commentId, beginShowChart, chartResult, showChart, qType) {
        var $this = this;
        var divIssue = $("#issue");
        var divResult = $("<div id='result' class='result'/>");
        var isBeginShowChart = beginShowChart == "Y"; //是否先顯示問卷統計圖表
        var isChartResult = chartResult == "Y"; //是否公開圖表
        var showChart = showChart == "Y";
        this.qType = qType;

        if (!masterPK) {
            masterPK = 0;
        }
        this.masterPK = masterPK;

        //在Client，判斷此問卷是否已填寫完畢
        setCookie();
        /*var value = $.cookie(this.qStatus + "_" + this.masterPK);
        if (value == "complete") {
        $("div.visible").hide();
        return;
        }*/

        $("div.aspShowChart").hide();
        $("#ChartResult").hide();

        if (showChart && isChartResult) {
            $("div.aspShowChart").show();
        }

        if (isBeginShowChart) {
            //是否先顯示問卷統計圖表
            $("#ChartResult").show();

            $("div.main").hide();
            $("#iframeChartResult").attr("src", "QuestionnareChart.aspx?pk=" + masterPK);
            if (!isChartResult) {
                $("#iframeChartResult").hide();
            }

            $("#showQuestionnaire").click(function () {
                $("div.main").show();
                $("#ChartResult").hide();
            }).show();
        }
        //是否公開圖表
        this.showChartResult = isChartResult;

        this.callAjax("Questionnaire/QuestionnaireService.asmx/GetDetails", "{'masterPK': '" + masterPK + "'}",
		function (data) {
		    $this.gridGroupData = data.d;

		    for (var i = 0; i < data.d.length; i++) {
		        if (data.d[i].GroupId == 0) {
		            $this.currentDetailPK = data.d[i].Id;
		            var div = $this.createContentInner(data.d[i]);
		            divIssue.append(div);
		        }
		    }
		    //問卷結果
		    var comment = $("#" + commentId).val();
		    if (comment) {
		        comment = comment.replace(/\r\n/g, "<br/>");
		    }

		    var divResultComment = $("<div id='divResultComment'/>").html(comment);
		    divResult.append(divResultComment);

		    //#region 類型為清單
		    if ($this.qType == questionnaireType.list) {
		        var spanWrapper = $("<span class='art-button-wrapper'><span class='l'></span><span class='r'></span></span>");
		        var btnSave = $("<input id='btnSaveList' type='button' class='art-button' />").val("儲存").
				click(function () {
				    $(this).attr("disabled", "disabled");
				    $this.saveData();
				}).css("cursor", "pointer");

		        divResultComment.hide();
		        spanWrapper.append(btnSave);
		        divResult.append(spanWrapper);

		        //驗證
		        $.validator.addMethod("pageRequired", function (value, element) {
		            var divCnt = $(element).parents("div.cnt");
		            if (divCnt.is(":visible")) {
		                return !this.optional(element);
		            }
		            return true;
		        }, $.validator.messages.required);

		        $this.validator = $("#form1").validate({
		            errorPlacement: function (error, element) {
		                if (element.parent().is("td")) {
		                    element.parents("tr").append($("<td/>").css({ "white-space": "nowrap", "padding": "5px" }).append(error));
		                }
		                else {
		                    element.parents("div.cnt").find("h3").append(error);
		                }

		            }
		        });

		        $this.transferForList(divResult);
		    }
		    else {
		        //類型為Process
		        $this.len = $("#issue").find("div.cnt").size();
		        $this.checkItem();
		        $this.setProcess($this.index);
		    }
		    //#endregion

		    divIssue.append(divResult);
		    //加題號
		    $("div.cnt", divIssue).each(function (i) {
		        var h = $(this).find("h3");
		        h.prepend(i + 1 + ".");
		    });

		});

    },
    createContentInner: function (data) {
        var divCnt = $("<div class='cnt'/>");
        var topic = $("<h3 />").html(data.Topic);
        var ul = $("<ul/>");
        var li, ele;
        var hfDetailPK = $("<input type='hidden' class='qDetailPK' />").val(data.Id);
        var hfAnswerPK = $("<input type='hidden' class='qAnwserPK'/>").val("0");
        var hfEleType = $("<input type='hidden' class='qEleType'/>").val(data.AnswerType);
        var hfNeeded = $("<input type='hidden' class='qNeeded'/>").val(data.Needed);
        var hfForceTransfer = $("<input type='hidden' class='qForceTransfer' />"); //跳題的DetailId
        var hfIsTransfer = $("<input type='hidden' class='qIsTransfer'/>"); //是否跳題

        //建立控制項
        switch (data.AnswerType) {
            case "Checkbox":
            case "RadioButton":
                var values = data.AnswerDefine.split(',');
                var nextAnswers = data.NextAnswer.split(",");
                var nextAnswer = '0';

                for (var i = 0; i < values.length; i++) {
                    li = $("<li/>");
                    if (values[i] != '') {
                        if (i < nextAnswers.length) {
                            nextAnswer = nextAnswers[i];
                        }
                        var ctl = controlUIHelper.createListControl(data.AnswerType, data.Id, values[i], data.Needed, nextAnswer);
                        ul.append(li.append(ctl));
                    }
                }
                ul.find("li:last").append(hfDetailPK).append(hfAnswerPK).append(hfEleType).append(hfNeeded);
                if (data.Needed) {
                    ul.find(":radio:first").addClass("required");
                    ul.find(":checkbox:first").addClass("required");
                }
                break;
            case "Text":
                ele = controlUIHelper.createText(data.Id, data.Needed);
                break;
            case "TextArea":
                ele = controlUIHelper.createTextArea(data.Id, data.Needed);
                break;
            case "Grid":
                ele = controlUIHelper.createGrid(data.Id, data.AnswerDefine);
                break;
            case "Image":
                imageList.createContent(divCnt, data.AnswerDefine, data.Needed);
                break;
            default:
                break;
        }

        if (data.AnswerType != "Checkbox" && data.AnswerType != "RadioButton") {
            li = $("<li/>").append(ele);
            li.append(hfDetailPK).append(hfAnswerPK).append(hfEleType).append(hfNeeded);
            ul.append(li);
        }

        if (data.Needed) {
            //必填
            topic.append($("<span class='needed'/>").html("＊"));
        }
        //跳題
        if (data.NextAnswer && data.NextAnswer != "0") {
            hfForceTransfer.val(data.NextAnswer);
            hfIsTransfer.val("Y");
        }
        else {
            hfForceTransfer.val("0");
            hfIsTransfer.val("N");
        }

        divCnt.append(topic).append(ul).append(hfForceTransfer).append(hfIsTransfer);

        return divCnt;
    },
    ceateTransferButton: function () {
        /// <summary>建立跳題的按鈕</summary>
        //加入下一頁
        var $this = this;
        var nextButton = $("<input type='button' class='art-button nextButton'/>").width(100).val("下一頁");
        var spanWrapper = $("<span class='art-button-wrapper'><span class='l'></span><span class='r'></span></span>");

        nextButton.click(function () {
            var divs = $("div.cnt");
            var div = divs.eq($this.index);
            var targetDetailId;
            var index = divs.size();

            //取得跳題的DetailId
            targetDetailId = $this.getTransferDetailId(div);

            if ($this.validator.form()) {
                //重設
                divs.hide();
                $(".nextButton").remove();
                var trans = $this.getIndex($this.index, targetDetailId);

                //取得下一頁的有跳題的索引
                for (var i = trans.index + 1; i < divs.size(); i++) {
                    var isTransfer = divs.eq(i).find(".qIsTransfer").val() == "Y";

                    if (isTransfer) {
                        index = i;
                        break;
                    }
                }

                //只顯示跳題與跳題之間的部份
                for (var i = trans.index; i <= index; i++) {
                    divs.eq(i).show();
                }

                $this.index = index;

                if (index == divs.size()) {
                    $("#btnSaveList").show();
                }
                else {
                    //指定下一個的下一頁的按鈕
                    div = divs.eq(index);
                    nextButton = $this.ceateTransferButton();
                    div.after(nextButton);
                }

            }

        }).appendTo(spanWrapper);

        return spanWrapper;
    },
    getAnswerValue: function (parent, tag, keyTag) {
        var value = '', text = '';
        var eleType = parent.find(".qEleType").val();

        if (!tag) {
            tag = ",";
        }
        if (!keyTag) {
            keyTag = ":";
        }

        switch (eleType) {
            case "Text":
            case "TextArea":
                value = parent.find(".qText").val();
                break;
            case "Checkbox":
            case "RadioButton":
                parent.find(".qList").each(function () {
                    var checked = this.checked;
                    if (checked) {
                        value += $(this).val() + tag;
                    }
                });
                if (parent.find(".qListText").size() > 0) {
                    text = parent.find(".qListText").val();
                }
                break;
            case "Grid":
                parent.find(".qGridTable tr").each(function () {
                    var key = $(this).find("td:first").html();

                    $(this).find("td :radio").each(function () {
                        var checked = this.checked;
                        if (checked) {
                            text += key + keyTag + $(this).val() + tag;
                        }
                    });

                });
                break;
            case "Image":
                var table = parent.find(imageList.imageListContainer);
                $(imageList.radioSelector, table).each(function () {
                    if (this.checked) {
                        value = $(this).next().val();
                    }
                });

                break;
            default:
                break;
        }

        return { value: value, text: text };
    },
    getIndex: function (currentIndex, targetDetailId) {
        var divs = $("div.cnt");
        var isTransfer = divs.eq(currentIndex).find(".qIsTransfer").val() == "Y";
        var index = 0;

        if (isTransfer) {
            $(".qDetailPK", divs).each(function (i) {
                if ($(this).val() == targetDetailId) {
                    index = i;
                    return false;
                }
            });
        }

        return { index: index, isTransfer: isTransfer };
    },
    getTransferDetailId: function (div) {
        /// <summary>取得有跳題的DetailId</summary>
        var targetDetailId, radios, index;

        //取得跳題的DetailId
        radios = div.find(":radio");
        if (radios.size() > 0) {
            //是RadioButton
            radios.each(function () {
                if (this.checked) {
                    var hf = $(this).parents("span").find(".qTransferNumber");
                    targetDetailId = hf.val();
                    return false;
                }
            });

        }
        else {
            var hf = div.find(".qForceTransfer");
            targetDetailId = hf.val();
        }

        return targetDetailId;
    },
    setProcess: function (j, isTransfer) {
        /// <summary>建立換頁效果</summary>
        if (isTransfer) {
            this.index = j;
        }
        else {
            this.index += j;
        }

        this.index = (this.index < 0) ? 0 : this.index;
        var divCnt = $("div.cnt:eq(" + this.index + ")");
        var detailPK = divCnt.find(".qDetailPK").val();
        var needed = divCnt.find(".qNeeded").val() == "true";

        $("#tips span").html((this.index + 1 > this.len ? this.len : this.index + 1) + "\/" + this.len);

        //Processbar的進度
        if ($("#tips").size() > 0) {
            var wh = $("#tips")[0].offsetWidth;
            var ress = Math.round(this.index * this.progress / this.len);
            $("#ress").css({ "width": ress + "px" });
            $("#tips").css({ "left": ress - Math.round(wh / 2) + "px" });
            $("#issue").animate({ "top": -this.index * this.ht + "px" }, 500);
        }

        if (!needed && this.index < this.len) {
            this.sld.push(this.index);
        }

        this.checkButtion();

    },
    saveAnswerMaster: function (answerMasterPK, parent) {
        var $this = this;
        var answerPK = parent.find(".qAnwserPK").val();
        var detailPK = parent.find(".qDetailPK").val();
        var status = '';

        if (this.index + 1 == this.len) {
            status = 'complete';
            //記錄在cookie，問卷已填寫完成
            $.cookie(this.qStatus + "_" + this.masterPK, "complete", this.cookieOption);

            this.showResult();
        }
        else if (this.qType == questionnaireType.list) {
            if (this.validator.form()) {
                status = "complete";
                //記錄在cookie，問卷已填寫完成
                $.cookie(this.qStatus + "_" + this.masterPK, "complete", this.cookieOption);
            }
            else {
                $("#btnSaveList").removeAttr("disabled");
                return false;
            }

        }

        $.cookie(this.qStep + "_" + this.masterPK, this.index + 1, this.cookieOption);

        this.callAjax("Questionnaire/QuestionnaireService.asmx/SaveAnswerMaster",
		"{'answerMasterPK': '" + answerMasterPK + "', 'masterPK': '" + this.masterPK + "', 'step': '" + (this.index + 1) + "', 'status': '" + status + "', 'desc': ''}",
		function (data) {
		    $("#hfAnswerMasterPK").val(data.d);

		    if ($this.qType == questionnaireType.process) {
		        $this.saveAnswer(answerPK, detailPK, data.d, parent);
		    }
		    else {
		        $this.saveAnswerList();
		    }

		});
    },
    saveAnswer: function (answerPK, detailPK, answerMasterPK, parent) {
        /// <summary>儲存答案明細</summary>
        var $this = this;
        var answer = this.getAnswerValue(parent);
        var eleType = parent.find(".qEleType").val();

        this.callAjax("Questionnaire/QuestionnaireService.asmx/SaveAnswer",
		"{'answerPK': '" + answerPK + "', 'detailPK':'" + detailPK + "', 'answerValue': '" + answer.value + "', 'answerText': '" + answer.text + "', 'answerMasterPK':'" + answerMasterPK + "'}",
		function (data) {
		    parent.find(".qAnwserPK").val(data.d);

		    /*if (eleType == "Grid") {
		    $this.saveAnswerGroup(answerPK, detailPK, parent);
		    }*/
		});
    },
    saveAnswerList: function () {
        /// <summary>儲存問卷答案(清單式)</summary>
        var $this = this;
        var value = '', detailPK, answerMPK;

        answerMPK = $("#hfAnswerMasterPK").val();
        $("div.cnt").each(function () {
            var item = $this.getAnswerValue($(this), "$&", "$#");
            detailPK = $(this).find(".qDetailPK").val();
            value += "Id:0,DetailId:" + detailPK + ",AnswerValue:" + item.value + ",AnswerText:" + item.text + ",QAMId:" + answerMPK + ";";
        });

        this.callAjax("Questionnaire/QuestionnaireService.asmx/SaveAnswerGroup",
		"{'value': '" + value + "'}",
		 function (data) {
		     $("#btnSaveList").removeAttr("disabled");
		     $("div.cnt").hide();
		     $("#btnSaveList").hide();
		     $("#divResultComment").show();
		     $this.showResult();
		 });

    },
    saveAnswerGroup: function (answerPK, detailPK, parent) {
        var table = parent.find(".qGridTable");
        var value = '';

        table.find("tr").each(function () {
            $(this).find(".qGrid").each(function () {
                var checked = this.checked;
                if (checked) {
                    value += "Id:0,DetailId:" + $(this).attr("rel") + ",AnswerValue:" + $(this).val() + ";";
                }
            });

        });

        this.callAjax("Questionnaire/QuestionnaireService.asmx/SaveAnswerGroup",
		"{'value': '" + value + "'}",
		 function (data) {

		 });

    },
    saveData: function () {
        var divCnt = $("div.cnt:eq(" + this.index + ")");
        var answerMasterPK = $("#hfAnswerMasterPK").val();

        this.saveAnswerMaster(answerMasterPK, divCnt);
    },
    showResult: function () {
        if (this.showChartResult) {
            $("div.aspShowChart").show();
            $("a.aspChartResult").show();
            $("a.showQuestionnaire").hide();
        }
        else {
            $("div.aspShowChart").hide();
        }

    },
    transferForList: function (divResult) {
        /// <summary>類型為清單(單層)的跳題</summary>
        var divs = $("div.cnt");
        var div, nextButton;
        var $this = this;
        this.index = 0;

        divs.each(function (i) {
            var isTransfer = $(this).find(".qIsTransfer").val() == "Y";

            if (isTransfer) {
                $this.index = i;
                div = $(this);
                return false;
            }
        });

        //初始化
        if (div) {
            var count = divs.size();
            for (var i = $this.index + 1; i < count; i++) {
                divs.eq(i).hide();
            }

            nextButton = this.ceateTransferButton();
            div.after(nextButton);
            $("#btnSaveList", divResult).hide();
            $(".required").addClass("pageRequired").removeClass("required");
        }

    }

};

var controlUIHelper = {
    elementIndex: 0,

    createText: function (detailPK, needed, value) {
        var txt = $("<input type='text' class='qText'/>")
		.attr({ "needed": needed, name: "txt" + detailPK, id: "txt" + detailPK }).width(300);
        if (needed) {
            txt.addClass("required");
        }

        if (value) {
            txt.val(value);
        }
        return txt;
    },
    createTextArea: function (detailPK, needed, value) {
        var txt = $("<textarea cols='30' rows='6' class='qText' />")
		.attr({ "needed": needed, name: "txtarea" + detailPK, id: "txtarea" + detailPK }).width(300);
        if (needed) {
            txt.addClass("required");
        }

        if (value) {
            txt.val(value);
        }
        return txt;
    },
    createListControl: function (elementType, detailPK, value, needed, transferNumber) {
        var eleType = '', cHtml;
        var span = $("<span/>");
        var label = $("<label/>");
        var hf = $("<input type='hidden' class='qTransferNumber'/>").val(transferNumber);
        this.elementIndex += 1;

        if (elementType == "Checkbox") {
            eleType = "checkbox";
        }
        else {
            eleType = "radio"
        }

        cHtml = "<input type='" + eleType + "' class='qList' name='q" + detailPK + "' value='" + value + "' id='lb" + this.elementIndex + "' />";
        var checkbox = $(cHtml);

        if (value == "_Other") {
            value = "其它";
        }

        label.attr("for", "lb" + this.elementIndex).html(value).prepend(checkbox);
        span.append(label).append(hf);

        if (value == "其它") {//其它
            label.css("display", "inline");
            span.append($("<input type='text' class='qListText' />"));
        }

        return span;
    },

    createGrid: function (detailPK, value) {
        return gridUIHelper.createContent(detailPK, value);
    }

};

//#region gridUIHelper
var gridUIHelper = {
    needed: false,
    createContent: function (detailPK, value) {
        var table = $("<table class='qGridTable' />").width("50%");
        var rows, columns;
        rows = value.split(',');
        columns = this.getGroups();

        for (var i = 0; i < columns.length + 1; i++) {
            var tr = $("<tr/>");

            for (var j = 0; j < rows.length + 1; j++) {
                var td = $("<td/>").attr({ align: "center" }).
				css({ "white-space": "nowrap", "padding": "5px" });

                if (rows[j - 1] == '') {
                    continue;
                }

                if (i == 0 && j == 0) {
                    tr.append(td);
                }
                else if (i == 0 && j != 0) {
                    td.text(rows[j - 1]);
                    tr.append(td);
                }
                else if (j == 0) {
                    td.text(columns[i - 1].value);
                    tr.append(td);
                }
                else {
                    var rbnHtml, rbn;

                    rbnHtml = "<input type='radio' class='qGrid' name='qgrid" + columns[i - 1].pk + "' value='" + rows[j - 1] + "' rel='" + columns[i - 1].pk + "' />"
                    rbn = $(rbnHtml);

                    tr.append(td.append(rbn));
                }

            }
            table.append(tr);
        }

        if (this.needed) {
            $("tr", table).each(function () {
                $(this).find("td:eq(1) :radio").addClass("required");
            });

        }
        return table;
    },
    getGroups: function () {
        var groups = [];
        var datas = quesUIObject.gridGroupData;

        for (var key in datas) {
            //取得子問題(群組)
            if (datas[key].GroupId != 0 && quesUIObject.currentDetailPK == datas[key].GroupId) {
                groups.push({ pk: datas[key].Id, value: datas[key].AnswerDefine });
            }

            //此題是否為必填
            if (datas[key].Id == quesUIObject.currentDetailPK) {
                this.needed = datas[key].Needed;
            }
        }
        return groups;

    }

};
//#endregion

Array.prototype.contains = function (element) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == element) {
            return true;
        }
    }
    return false;
}

var questionnaireType = {
    list: "List",
    process: "Process"
};