
var formUI = (function () {
    var validator = null;
    var callAjax = function (url, jsonData, callbackOnSuccess) {
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
    };
    var createControls = function (datas) {
        var ul = $("<ul class='definedForm'/>").appendTo("#pnlForm"), li;
        for (var i = 0; i < datas.length; i++) {
            li = $("<li class='formItem'/>");
            //title
            $("<div class='title'/>").html(datas[i].Title).appendTo(li);
            //control
            var div = createControlInner(datas[i]);
            li.append(div).appendTo(ul);
        }

        li = $("<li class='cmd'/>").appendTo(ul);
        //儲存
        var saveButton = $("<input type='button'/>").val("儲存").click(saveData);
        li.append(saveButton);

        //將標題的寬度設為一樣
        var maxWidth = 0;
        var divTitle = $("div.title", ul);
        divTitle.each(function () {
            var width = $(this).width();
            if (width > maxWidth) {
                maxWidth = width;
            }
        });
        divTitle.width(maxWidth);
    };

    var createControlInner = function (data) {
        var hfDetailPK = $("<input type='hidden' class='qDetailPK' />").val(data.PK);
        var hfAnswerPK = $("<input type='hidden' class='qAnwserPK'/>").val("0");
        var hfEleType = $("<input type='hidden' class='qEleType'/>").val(data.AnswerType);
        var hfRequired = $("<input type='hidden' class='qNeeded'/>").val(data.Required);
        var ele, div;

        div = $("<div class='content'/>");
        switch (data.AnswerType) {
            case "Checkbox":
            case "RadioButton":
                var values = data.AnswerDefine.split(',');
                for (var i = 0; i < values.length; i++) {
                    if (values[i] != '') {
                        ele = controlUIHelper.createListControl(data.AnswerType, data.PK, values[i], data.Required, "0");
                        div.append(ele);
                    }

                }
                if (data.Required) {
                    $(":checkbox:first", div).addClass("required");
                    $(":radio:first", div).addClass("required");
                }
                break;
            case "Text":
                ele = controlUIHelper.createText(data.PK, data.Required);
                div.append(ele);
                break;
            case "TextArea":
                ele = controlUIHelper.createTextArea(data.PK, data.Required);
                div.append(ele);
                break;
            case "Calendar":
                ele = controlUIHelper.createCalendar(data.PK, data.Required);
                div.append(ele);
                break;
            case "EMail":
                ele = controlUIHelper.createEMail(data.PK, data.Required);
                div.append(ele);
                break;
            default:
                break;

        }
        div.append(hfDetailPK).append(hfAnswerPK).append(hfEleType).append(hfRequired);
        return div;
    };

    var getAnswerValue = function (parent, tag, keyTag) {
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
            case "EMail":
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
            case "Calendar":
                value = parent.find(".dateCalendar").val();
                break;
            default:
                break;
        }

        return { value: value, text: text };
    }

    var saveData = function () {
        var detailPK, value = "";

        if (validator.form()) {
            $("li.formItem").each(function () {
                var item = getAnswerValue($(this), "$&", "$#");
                detailPK = $(this).find(".qDetailPK").val();
                value += "PK:0,DetailPK:" + detailPK + ",AnswerValue:" + item.value + ",AnswerText:" + item.text + ";";
            });
            callAjax("DefinedForm/DefindedFormService.asmx/SaveDataUI", "{'value':'" + value + "'}", function (data) {
 
                $("#pnlForm").hide();
                $("#pnlMessage").show();
            });

        }
        else {
            return false;
        }

    }

    return {
        createContent: function (masterPK) {
            //驗證
            validator = $("#form1").validate({
                errorPlacement: function (error, element) {
                    element.parents("div.content").append(error);
                }
            });
            callAjax("DefinedForm/DefindedFormService.asmx/GetDetails", "{'masterPK':'" + masterPK + "'}", function (data) {
                createControls(data.d);
            });
        }
    };
})();