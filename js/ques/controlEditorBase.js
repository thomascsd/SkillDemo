var controlEditorBase = {
    innerContainerSeletor: "div.innerContainer",
    elementType: [],
    allDatas: null,
    currentDetailPK: 0,
    currentSaveType: "saveButton",
    itemNumber: 0,
    otherKey: "_Other",
    getElementTypeListUrl: "",
    getDetailsUrl: "",
    updateSortUrl: "",
    deleteDataUrl: "",
    detailTopicName: "Topic",
    detailNeededName: "Needed",
    detailIdName: "Id",

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
    changeElementType: function () {
        var ckbTransfer, container;
        var type = $(this).val();
        var innerContainer = $(this).parents(controlEditorBase.innerContainerSeletor);
        innerContainer.find("li.divItem").remove();

        var divItem = controlEditorBase.createElement(type);
        innerContainer.find("ul.itemList").append(divItem);
        //跳題
        ckbTransfer = innerContainer.find(".transfer");
        container = innerContainer.find(".transferContainer");
        if (ckbTransfer.size() > 0) {
            if (ckbTransfer[0].checked) {
                if (type == "RadioButton") {
                    container.hide(); //不是RadioButton的，
                    container = divItem.find(controlHelper.transferContainerSelector);
                }
                container.show();
            }
            else {
                container.hide();
            }
        }
        controlEditorBase.setSideMenuState();
    },
    createAddOtherItem: function (eleType) {
        var divTemp = $("<li class='divItem'/>");
        var btnDel = this.createItemDelete(eleType);

        divTemp.append($("<span class='otherItem'/>").html("其他").append($("<input type='text'/>").attr("disabled", "disabled"))).append(btnDel.css("margin-left", "5px"));
        return divTemp;
    },
    createAddOtherLink: function (eleType) {
        var $this = this;
        //其它項目
        var link = $("<a href='javascript:void(0);' class='cOther'></a>").html("加入其他").click(function () {
            var innerTemp = $(this).parents($this.innerContainerSeletor);
            var divTemp = $this.createAddOtherItem(eleType);
            innerTemp.find("ul.itemList").append(divTemp);
            innerTemp.find(".cOther").remove();

            $this.setSideMenuState();
        });
        return link;
    },
    createAddOhter: function (innerContainer, eleType) {
        var link = this.createAddOtherLink(eleType);
        if (eleType == "Checkbox" || eleType == "RadioButton") {
            // 其它項目
            innerContainer.find(".cOther").remove();
            if (innerContainer.find("span.otherItem").size() == 0) {
                innerContainer.find("li.divItem:last").append(link);
            }

        }

    },
    createContent: function (masterPK) {
        var $this = this;
        var url = this.getDetailsUrl; //../QuestionnaireService.asmx/GetDetails
        if (masterPK == "0") {
            this.createInnerContent();
        }
        else {
            //更新，取回資料
            this.callAjax(url, "{'masterPK': '" + masterPK + "'}",
                function (data) {
                    var datas = data.d;
                    $this.allDatas = datas;
                    for (var i = 0; i < datas.length; i++) {
                        //GroupId不為零，是群組(Grid)
                        if (datas[i].GroupId == 0) {
                            $this.createInnerContent(datas[i]);
                        }
                    }
                    //無資料，建立一筆
                    if (datas.length == 0) {
                        $this.createInnerContent();
                    }

                    $this.sortItem();
                    $this.hoverItem();
                    //$this.createTransferMenu();
                });
        }

    },
    createControls: function (masterPK) {
        var $this = this;
        var url = this.getElementTypeListUrl; //../QuestionnaireService.asmx/GetElementTypeList
        this.callAjax(url, "{}", function (data) {
            for (var key in data.d) {
                var eleType = { Code: data.d[key].Code, Description: data.d[key].Description };
                $this.elementType.push(eleType);
            }
            $this.createContent(masterPK);

        });
    },

    createElement: function (eleType, value, transNumber, parent) {
        /// <summary>建立每個控制項</summary>
        /// <param name="eleType" type="String">類型</param>
        /// <param name="value" type="String">控制項的值</param>
        /// <param name="transNumber" type="String">跳題</param>
        var $this = this;
        var divItem = $("<li class='divItem'/>");
        var btnAdd, btnDel;
        var ele;

        switch (eleType) {
            case "Text":
                ele = controlHelper.createText();
                break;
            case "TextArea":
                ele = controlHelper.createTextArea();
                break;
            case "Checkbox":
                ele = controlHelper.createCheckBox(value);
                break;
            case "RadioButton":
                ele = controlHelper.createRadioButton(value, transNumber);
                break;
            case "Grid":
                ele = controlHelper.createGrid(value);
                break;
            case "Image":
                controlHelper.createImageList(divItem, value);
                break;
            case "Calendar":
                ele = controlHelper.createCalendar();
                break;
            case "EMail":
                ele = controlHelper.createEMail();
                break;
            default:
                break;
        }

        //加入新項目
        btnAdd = $("<input type='button' class='cButton cAdd' />").css("margin-left", "15px").val("+")
        .click(function () {
            var innerTemp = $(this).parents($this.innerContainerSeletor);
            var type = innerTemp.find("select").val();
            var divTemp = $this.createElement(type, '', '', innerTemp);
            var div, ul, ckbTransfer;

            ul = innerTemp.find("ul.itemList");
            //跳題
            ckbTransfer = innerTemp.find(".transfer");
            if (ckbTransfer.size() > 0) {
                if (ckbTransfer[0].checked) {
                    var container = divTemp.find(controlHelper.transferContainerSelector);
                    container.show();
                }
            }

            //其它項目為最下層
            if (innerTemp.find(".otherItem").size() > 0) {
                div = innerTemp.find(".otherItem").parent();
                div.before(divTemp);
            }
            else {
                ul.append(divTemp);
            }

            $this.createAddOhter(innerTemp, eleType);
            $this.setSideMenuState();
            $this.sortItem();
        });
        //移除項目
        btnDel = this.createItemDelete(eleType);
        divItem.append(ele);
        if (eleType == "Checkbox" || eleType == "RadioButton") {
            divItem.append(btnAdd).append(btnDel);
            if (parent) {
                if (parent.find(".cAdd").size() >= 1) {
                    parent.find(".divItem:first .cDel").show();
                }
            }
            else {
                btnDel.hide();
            }

        }

        return divItem;
    },

    createFooter: function (data) {
        var $this = this;
        var divFooter = $("<div class='footer'/>");
        var btnSave = $("<input type='button' />").val("儲存").click(function () {
            $this.currentSaveType = saveType.saveButton;
            var parent = $(this).parents($this.innerContainerSeletor);
            $this.saveData(parent);
        });
        var hfDetail = $("<input type='hidden' class='qDetail' />").val("0"); //明細PK
        var hfSort = $("<input type='hidden' class='qSort' />").val("0"); //排序

        if (data) {
            hfDetail.val(data[this.detailIdName]);
            hfSort.val(data.Sort);
        }

        divFooter.append(btnSave).append(hfDetail).append(hfSort);
        return divFooter;
    },
    //建立每個項目
    createInnerContent: function (data) {
        var container = $("#divContainer");
        var innerContainer = $("<div class='innerContainer' />");

        //標題、類型
        var table = this.createTable(data);
        innerContainer.append(table);
        //項目
        var select = table.find("select");
        var type = select.val();
        var divItem;
        var ul = $("<ul class='itemList'/>");

        if (data) {
            //將值填回控制項
            select.val(data.AnswerType);
            if (data[this.detailIdName] != 0) {
                this.currentDetailPK = data[this.detailIdName];
            }

            switch (data.AnswerType) {
                case "Checkbox":
                case "RadioButton":
                    var values = data.AnswerDefine.split(",");
                    var nextAnswers = data.NextAnswer.split(",");
                    var nextAnswer = '0';

                    for (var i = 0; i < values.length; i++) {
                        if (values[i] != '') {
                            if (values[i] == "其它" || values[i] == this.otherKey) {
                                divItem = this.createAddOtherItem(data.AnswerType);
                            }
                            else {
                                if (i < nextAnswers.length) {
                                    //只有RadionButton的NextAnswer為多個，其餘都為一個
                                    nextAnswer = nextAnswers[i];
                                }

                                divItem = this.createElement(data.AnswerType, values[i], nextAnswer);
                            }
                            ul.append(divItem);
                            innerContainer.append(ul);
                        }
                    }

                    if (data.AnswerDefine == '') {
                        divItem = this.createElement(data.AnswerType);
                        ul.append(divItem);
                        innerContainer.append(ul);
                    }
                    //加入其它
                    this.createAddOhter(innerContainer, data.AnswerType);
                    break;
                default:
                    divItem = this.createElement(data.AnswerType, data.AnswerDefine);
                    ul.append(divItem);
                    innerContainer.append(ul);
                    break;
            }
        }
        else {
            //無資料，新增一筆
            divItem = this.createElement(type);
            ul.append(divItem);
            innerContainer.append(ul)
            this.createAddOhter(innerContainer, type);
        }
        //刪除
        if (innerContainer.find(".cDel").size() > 1) {
            innerContainer.find(".cDel").show();
        }

        //儲存
        var divFooter = this.createFooter(data);
        innerContainer.append(divFooter);
        //側邊Menu
        var divSide = this.createSideMenu();
        innerContainer.append(divSide).appendTo(container);
        this.setSideMenuState();

        //問卷明細只有一個時，顯藏刪除
        if (container.find(this.innerContainerSeletor).size() == 1) {
            container.find("a.sideDel").hide();
        }
        else {
            container.find("a.sideDel").show();
        }

    },
    createItemDelete: function (eleType) {
        var $this = this;
        //移除項目
        var btnDel = $("<input type='button' class='cButton cDel' />").val("-").click(function () {
            var innerTemp = $(this).parents($this.innerContainerSeletor);

            $(this).parent().remove();
            //只有一個時顯藏按鈕
            if (innerTemp.find(".cAdd").size() == 1) {
                innerTemp.find(".divItem:first .cDel").hide();
            }

            $this.createAddOhter(innerTemp, eleType);
            $this.setSideMenuState();
        });
        return btnDel;
    },
    createTable: function (data) {
        var table = this.createTableMain(data);
        var tr = this.createTableTransferRow(data);
        table.append(tr);

        return table;
    },
    createTableMain: function (data) {
        var table = $("<table/>");
        var txtTitle = $("<input type='text' class='qEle' />");
        var selType = $("<select class='qEle' ></select>").change(this.changeElementType);
        var ckbNeeded = $("<input type='checkbox' class='needed'/>").attr("checked", function () { return true; });
        var $this = this;

        if (data) {
            txtTitle.val(data[this.detailTopicName]);
            var required = data[this.detailNeededName];
            ckbNeeded.attr("checked", function () { return required; });
        }

        //標題
        var span = $("<span></span>").html("標題");
        var tr = $("<tr/>").append($("<td/>").append(span)).append($("<td/>").append(txtTitle));
        table.append(tr);
        //類型
        span = $("<span></span>").html("類型");
        tr = $("<tr />").append($("<td/>").append(span));

        selType.html("");
        for (var key in this.elementType) {
            var code = this.elementType[key].Code;
            var desc = this.elementType[key].Description;
            var option = $("<option />").attr("value", code).html(desc);
            selType.append(option);
        }

        tr.append($("<td/>").append(selType));
        table.append(tr);
        //是否為必填
        span = $("<span></span>").html("是否為必填");
        tr = $("<tr/>").append($("<td/>").append(span)).append($("<td/>").append(ckbNeeded));
        table.append(tr);

        return table;
    },
    createTableTransferRow: function (data) {
        /// <summary>建立跳題的tr</summary>
        var $this = this;
        var ckbTransfer = $("<input type='checkbox' class='transfer'/>").css("float", "left");
        var divTransfer = $("<div class='transferContainer'/>").css("float", "left").hide();

        ckbTransfer.click(function () {
            //類型為RadioButton
            if (selType.val() == "RadioButton") {
                var parent = $(this).parents($this.innerContainerSeletor);
                divTransfer = $("ul li.divItem", parent).find(controlHelper.transferContainerSelector);
            }
            divTransfer.toggle();
            if (divTransfer.is(":hidden")) {
                $("#divTransferMenu").hide();
            }

        });

        var txtTransfer = $("<input type='text' class='forceTransfer'/>").click(function () {
            $this.createTransferMenu($(this));
        }).width(100).appendTo(divTransfer);
        var hfTransfer = $("<input type='hidden' class='forceTransferNumber'/>").appendTo(divTransfer);
        if (data) {
            if (data.NextAnswer != "0" && data.NextAnswer) {
                if (data.AnswerType != "RadioButton") {
                    var topic = this.getTopic(data.NextAnswer);
                    txtTransfer.val(topic);
                    hfTransfer.val(data.NextAnswer);
                    divTransfer.show();
                }

                ckbTransfer.attr("checked", function () { return true; });
            }

        }
        span = $("<span></span>").html("跳題");
        tr = $("<tr/>");
        tr.append($("<td/>").append(span)).append($("<td/>").append(ckbTransfer).append(divTransfer));
        return tr;
    },
    createTransferMenu: function (target) {
        var div = $("#divTransferMenu").empty().show();
        var ul, li, link, offset = target.offset(), hfNumber;

        ul = $("<ul class='itemList'/>").css("margin", "0px");
        for (var i = 0; i < this.allDatas.length; i++) {
            link = $("<a href='javascript:void(0)'/>").html(this.allDatas[i].Topic)
            .click(function () {
                var targetHidden, linkHidden;
                targetHidden = target.siblings();
                linkHidden = $(this).siblings(".transferNumber");
                target.val($(this).html());
                targetHidden.val(linkHidden.val());
                div.hide();
            });

            hfNumber = $("<input type='hidden' class='transferNumber'/>").val(this.allDatas[i].Id);
            li = $("<li/>").append(link).append(hfNumber);
            ul.append(li);
        }
        div.append(ul).css({ top: offset.top + target.height() + "px", left: offset.left + "px" });
    },
    createSideMenu: function () {
        var $this = this;
        var divSide = $("<div class='sideMenu'/>").hide();
        var addQuestion, delQuestion, buttonQuestion, btnCopyAdd;

        addQuestion = $("<a href='javascript:void(0);' class='sideCmd'></a>").html("加入").click(function () {
            $this.allDatas = null;
            $this.currentSaveType = saveType.sideMenu;

            $this.createInnerContent();
            $this.saveData($(this).parents($this.innerContainerSeletor));
        }).css("clear", "both");

        delQuestion = $("<a href='javascript:void(0);' class='sideCmd sideDel'></a>").html("刪除").click(function () {
            var parent = $(this).parents($this.innerContainerSeletor);
            $this.deleteData(parent);
        });

        buttonQuestion = $("<span href='javascript:void(0);' class='sideCmd ui-icon ui-icon-plusthick'></span>").
        click(function () {
            $(this).toggleClass("ui-icon-minusthick");
            $(this).parents($this.innerContainerSeletor).find("ul.itemList").toggle();
            $this.setSideMenuState();
        }).css("float", "right");

        btnCopyAdd = $("<a href='javascript:void(0);' class='sideCmd sideCopy'></a>").html("複製").
        click(function () {
            var parent = $(this).parents($this.innerContainerSeletor);
            $this.currentSaveType = saveType.copyAdd;
            $this.saveData(parent);

        });

        divSide.append(buttonQuestion).append(addQuestion).append(btnCopyAdd).append(delQuestion)
        return divSide;
    },
    createSortContent: function (masterPK) {
        /// <summary>建立排序內容</summary>
        /// <param name="masterPK" type="String">主檔PK</param>
        var $this = this;
        var table = $("<table id='tableSort' />");
        var btnSaveSort, url;
        table.append($("<tr><th>標題</th><th>排序</th></tr>"));
        url = this.getDetailsUrl;
        $this.callAjax(url, "{'masterPK': '" + masterPK + "'}",
                function (data) {
                    var datas = data.d;
                    for (var i = 0; i < datas.length; i++) {
                        if (datas[i].GroupId == 0) {
                            var tr = $this.createSortItem(datas[i]);
                            table.append(tr);
                        }
                    }

                    btnSaveSort = $("<input type='button' id='btnSaveSort' value='儲存'/>").click(function () {
                        $(this).attr("disabled", "disabled").val("儲存中");
                        $this.updateSort();
                    });
                    var tr = $("<tr/>").append($("<td colspan='2'/>").append(btnSaveSort));
                    table.append(tr).appendTo($("#divSort"));
                });

    },
    //建立排序項目
    createSortItem: function (data) {
        var tr = $("<tr/>");
        var tdTopic, tdSort, txtSort, hfId;

        tdTopic = $("<td />").html(data[this.detailTopicName]);
        tr.append(tdTopic);

        txtSort = $("<input type='text' class='textSort' />").val(data.Sort);
        hfId = $("<input type='hidden' class='hiddenSort'/>").val(data[this.detailIdName]);
        tdSort = $("<td/>").append(txtSort).append(hfId);
        tr.append(tdSort);
        return tr;
    },

    deleteData: function (parent) {
        var $this = this, url = "";
        var detailPK = parent.find(".qDetail").val();
        var container = $("#divContainer");

        url = this.deleteDataUrl;
        this.callAjax(url, "{'detailPK': '" + detailPK + "'}",
        function (data) {
            parent.remove();
            $this.setSideMenuState();

            if (container.find($this.innerContainerSeletor).size() == 1) {
                container.find("a.sideDel").hide();
            }
        });
    },
    getElementType: function (eleType, parent) {
        var $this = this;
        var itemText = '', type = '';  //'G'群組,'S獨立題
        var transNumber = '';
        var ckbTransfer = parent.find(".transfer"), checked = false;

        if (ckbTransfer.size() > 0) {
            checked = ckbTransfer[0].checked;
        }
        //除了RadioButton，其它是是強制跳題
        if (checked) {
            transNumber = parent.find(".forceTransferNumber").val();
        }

        if (eleType == "Checkbox" || eleType == "RadioButton") {
            parent.find("li.divItem").each(function () {
                var li = $(this);
                var input = li.find(".listValue");
                if (input.val()) {
                    itemText += input.val() + ",";
                }
                if (li.find(".otherItem").size() > 0) {
                    itemText += $this.otherKey + ","; // 其它
                }
                //跳題
                var hf = li.find(".listTransferNumber");
                if (checked) {
                    if (hf.size() > 0) {
                        transNumber += hf.val() + ",";
                    }
                }

            });
            type = "S";
        }
        else if (eleType == "Grid") {
            type = "G";
            //column，(滿意、不滿意…)
            parent.find("table.column .gridText").each(function () {
                itemText += $(this).val() + ",";
            });

        }
        else if (eleType == "Image") {
            type = "S";
            var imageCategory = parent.find(".qImageList").val();
            itemText = imageCategory;
        }
        else {
            type = "S";
        }

        return { itemText: itemText, type: type, transferNumber: transNumber };
    },
    getTopic: function (id) {
        var topic = '';
        for (var i = 0; i < this.allDatas.length; i++) {
            if (id == this.allDatas[i].Id) {
                topic = this.allDatas[i].Topic;
                break;
            }
        }
        return topic;
    },
    hoverItem: function () {
        $(".itemList:has(:checkbox) >li,  .itemList:has(:radio) >li").hover(function () {
            $(this).css("background-color", "#FFEFD5");
        }, function () {
            $(this).css("background-color", "#fff");
        });
    },
    loadData: function () {
        /// <summary>重新戴入資料</summary>
        var masterPK = $("#hfMaster").val();
        var $this = this, url;
        url = this.getDetailsUrl;

        this.callAjax(url, "{'masterPK': '" + masterPK + "'}",
                function (data) {
                    $this.allDatas = data.d;
                });
    },
    reloadAllDatas: function (parent) {
        /// <summary>重新戴入this.allDatas</summary>
        var $this = this;
        var masterPK = $("#hfMaster").val();
        var detailPk = parent.find(".qDetail").val();
        var detail = null, url;
        url = this.getDetailsUrl;

        $this.callAjax(url, "{'masterPK': '" + masterPK + "'}",
                function (data) {
                    $this.allDatas = data.d;

                    for (var key in $this.allDatas) {
                        if ($this.allDatas[key].Id == detailPk) {
                            detail = $this.allDatas[key];
                            break;
                        }
                    }

                    if (detail != null) {
                        $this.currentDetailPK = detailPk;
                        detail.Id = 0;
                        $this.createInnerContent(detail);
                        alert("已複製"); //已複製
                    }

                });

    },
    saveData: function (parent) {
    },
    setSideMenuState: function () {
        var container = $("#divContainer");
        //設定sideMenu的位置
        container.find("div.sideMenu").each(function () {
            var parent = $(this).parent();
            var offset = parent.offset();
            var width = offset.left + parent.width();
            $(this).css({ "left": width - 30 + "px", "top": offset.top + "px" });
        });
        //加入hover
        container.find(this.innerContainerSeletor).hover(function () {
            $(this).find("div.sideMenu").show();
        }, function () {
            $(this).find("div.sideMenu").hide();
        });
        this.hoverItem();
    },
    sortItem: function () {
        var $this = this;
        $(".itemList:has(:checkbox), .itemList:has(:radio)").sortable({
            axis: "y"
        }).find("li").css("cursor", "move"); //.disableSelection();

    },
    updateSort: function () {
        var value = '', url = "";
        $("#tableSort tr").each(function () {
            var pk = $(this).find(".hiddenSort").val();
            var sort = $(this).find(".textSort").val();

            if (pk && sort) {
                value += "Id:" + pk + ",Sort:" + sort + ";";
            }
        });
        url = this.updateSortUrl; //../QuestionnaireService.asmx/UpdateSort
        this.callAjax(url, "{'value': '" + value + "'}",
        function (data) {
            $("#btnSaveSort").removeAttr("disabled").val("儲存");
            alert("儲存成功"); //儲存成功
        });
    },
    validData: function () {
        var temp = $("input.qEle:first").val();
        var ret = true;
        var message = '';

        $("input.qEle").each(function (i) {
            var value = $(this).val();
            if (i == 0) {
                return true;
            }

            if (value == '') {
                return true;
            }

            if (temp.indexOf(value) != -1) {
                ret = false;
                message = "標題不可重覆"; //標題不可重覆
                return false;
            }

            temp += value + ",";
        });

        return { returnValue: ret, message: message };
    }

};

//儲存狀態
var saveType = {
    sideMenu: "sideMenu",       //在SideMenu上的新增
    saveButton: "saveButton",   //儲存按鈕
    copyAdd: "copyAdd"           //在SideMenu上的複製
};
