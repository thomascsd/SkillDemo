var controlHelper = (function () {
    var transferContainerSelector = ".transferContainer";
    return {
        createCheckBox: function (value) {
            var span = $("<span />");
            var txt = $("<input type='text' class='listValue'/>");
            if (value) {
                txt.val(value);
            }
            span.append($("<input type='checkbox'/>").attr("disabled", "disabled")).append(txt);
            return span;
        },
        createGrid: function (value) {
            var div = $("<div/>");
            div.append(gridHelper.createRows()).append(gridHelper.createColumns(value));
            return div;
        },
        createImageList: function (li, value) {
            queObject.callAjax("../QuestionnaireService.asmx/GetImageType", "{}", function (data) {
                var categories = data.d;
                var option, select = $("<select class='qImageList' />");

                for (var i = 0; i < categories.length; i++) {
                    option = $("<option />").html(categories[i].DefValue).attr("value", categories[i].DefValue);

                    if (value == categories[i].DefValue) {
                        option.attr("selected", "selected");
                    }
                    select.append(option);
                }
                li.append($("<span/>").html("圖片類型").append(select)); //圖片類型
            });

        },
        createRadioButton: function (value, transNumber) {
            var span = $("<span />");
            var spanTrans = $("<span class='transferContainer'/>").html("跳題").hide(); //跳題
            var txt = $("<input type='text' class='listValue'/>");
            //跳題
            var hfTrans = $("<input type='hidden' class='listTransferNumber'/>").appendTo(spanTrans);
            var txtTrans = $("<input type='text' class='listTransfer'/>")
        .width(100)
        .click(function () {
            queObject.createTransferMenu($(this));
        }).appendTo(spanTrans);

            if (value) {
                txt.val(value);
            }
            if (transNumber && transNumber != "0") {
                hfTrans.val(transNumber);
                spanTrans.show();

                var topic = queObject.getTopic(transNumber);
                txtTrans.val(topic);
            }

            span.append($("<input type='radio'/>").attr("disabled", "disabled")).append(txt).append(spanTrans);
            return span;
        },
        createText: function () {
            return $("<input type='text'/>").attr("disabled", "disabled");
        },
        createTextArea: function () {
            return $("<textarea cols='30' rows='3' />").attr("disabled", "disabled");
        },
        createCalendar: function () {
            var span = $("<span/>");
            var txt = $('<input type="text"/>').attr("disabled", "disabled");
            var img = $("<img alt='calendar'/>").attr("src", "../images/calendar.gif").height(24);
            span.append(txt).append(img);
            return span;
        },
        createEMail: function () {
            return $("<input type='text' value='foo@sample.com'/>").attr("disabled", "disabled");
        }
    };
})();

var gridHelper = (function () {
    var gridTableSelector = "table.gridTable";
    var gridColumns = [{ text: '很滿意', score: 5 }, { text: '滿意', score: 4 }, { text: '一般', score: 3 }, { text: '不滿意', score: 2 }, { text: '很不滿意', score: 1}];

    var createFirstRow = function () {
        var tr = $("<tr><td>選項文字</td></tr>"); //選項文字
        return tr;
    };
    var createItem = function (text, parent) {
        var tr = $("<tr/>");
        var txtText, txtScore, td, btnAdd, btnDel;
        //選項
        txtText = $("<input type='text' class='gridText'/>").width(300);
        if (text) {
            txtText.val(text);
        }
        btnAdd = $("<input type='button' class='cButton gridAdd' />").css("margin-left", "10px").val("+").
        click(function () {
            var parentTable = $(this).parents($this.gridTableSelector);
            var tr = createItem('', parentTable);
            parentTable.append(tr);
            queObject.setSideMenuState();
        });
        btnDel = $("<input type='button' class='cButton gridDel' />").val("-").click(function () {
            $(this).parents("tr").remove();
            var parentTable = $(this).parents(gridTableSelector);

            if (parentTable.find(".gridDel").size() == 1) {
                parentTable.find(".gridDel:first").hide();
            }
            queObject.setSideMenuState();
        });
        td = $("<td/>").append(txtText).append(btnAdd).append(btnDel);
        if (parent) {
            if (parent.find(".gridAdd").size() >= 1) {
                parent.find(".gridDel:first").show();
            }
        }
        else {
            btnDel.hide();
        }
        tr.append(td);
        return tr;
    }
    var createTable = function (mode) {
        var table = $("<table class='gridTable' />").addClass(mode);
        return table;
    };
    var getGroups = function () {
        var groups = [];
        var datas = queObject.allDatas;
        for (var key in datas) {
            //取得子問題(群組)
            if (datas[key].GroupId != 0 && queObject.currentDetailPK == datas[key].GroupId) {
                groups.push(datas[key].AnswerDefine);
            }
        }
        return groups;
    };

    return {
        createRows: function () {
            var fieldset = $("<fieldset/>");
            var legend = $("<legend class='gridTitle' />").html("項目");
            var table = createTable("row");
            var tr;

            if (queObject.allDatas != null) {
                var groups = getGroups();
                for (var key in groups) {
                    tr = createItem(groups[key]);
                    table.append(tr);
                }

                if (groups.length == 0) {
                    tr = createItem();
                    table.append(tr);
                }

                if (table.find(".gridDel").size() > 1) {
                    table.find(".gridDel").show();
                }
            }
            else {
                tr = createItem();
                table.append(tr);
            }
            fieldset.append(legend).append(table);

            return fieldset;
        },
        createColumns: function (value) {
            var fieldset = $("<fieldset/>");
            var legend = $("<legend class='gridTitle' />").html("評比"); //評比
            var table = createTable("column"), tr;

            if (value) {
                var values = value.split(",");
                for (var key in values) {
                    if (values[key] != '') {
                        tr = createItem(values[key]);
                        table.append(tr);
                    }
                }

            }
            else {
                //預設
                for (var key in gridColumns) {
                    var text = gridColumns[key].text;
                    tr = createItem(text);
                    table.append(tr);
                }

            }
            table.find(".gridDel").show();
            fieldset.append(legend).append(table);

            return fieldset;
        }
    };
})();