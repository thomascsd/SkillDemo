var queObject = (function (base) {
    var init = function () {
        base.saveData = saveData;
        base.getElementTypeListUrl = "QuestionnaireService.asmx/GetElementTypeList";
        base.getDetailsUrl = "QuestionnaireService.asmx/GetDetails";
        base.updateSortUrl = "QuestionnaireService.asmx/UpdateSort";
        base.deleteDataUrl = "QuestionnaireService.asmx/DeleteData";
    };

    var saveData = function (parent) {
        var master = $("#hfMaster").val();
        var userPK = $("#hfUserPK").val();
        var detail = parent.find(".qDetail").val();
        saveMaster(master, detail, userPK, parent);
    }

    this.saveDetail = function (detailPK, masterPK, parent) {
        /// <summary>儲存明細</summary>
        var $this = this;
        var topic = parent.find("input.qEle").val();
        var eleType = parent.find("select.qEle").val();
        var item = base.getElementType(eleType, parent);
        var needed = parent.find(".needed")[0].checked ? "Y" : "N";
        var sort = parent.find(".qSort").val();

        base.callAjax("QuestionnaireService.asmx/SaveDetail",
    "{'detailPK': '" + detailPK + "', 'masterPK': '" + masterPK + "', 'type':'" + item.type + "', 'groupId': '0', 'topic': '" + topic + "', 'desc': '', 'answerType': '" + eleType + "', 'answerDef': '" + item.itemText + "', 'nextAnswer': '" + item.transferNumber + "', 'needed': '" + needed + "', 'sort': '" + sort + "' }",
    function (data) {
        parent.find(".qDetail").val(data.d);

        if (item.type == "G") {  //Grid(群組)
            $this.saveDetailGroup(data.d, masterPK, parent);
        }
        else {
            if (base.currentSaveType == saveType.saveButton) {
                alert("儲存成功"); //儲存成功
                base.loadData();
            }
            else if (base.currentSaveType == saveType.sideMenu) {
                //在SideMenu按下加入，將最後一個新增
                var p = parent.nextAll(":last");
                if (p.size() > 0) {
                    saveData(p);
                }
                else {
                    base.loadData();
                }
            }
            else if (base.currentSaveType == saveType.copyAdd) {
                base.reloadAllDatas(parent);
            }

        }
    });
    };

    this.saveDetailGroup = function (groupId, masterPK, parent) {
        /// <summary> 明細(群組)</summary>
        var value = '', $this = this;
        parent.find("table.row .gridText").each(function () {
            var topic = $(this).val();
            if (topic != '') {
                value += "Id:0,MasterId:" + masterPK + ",Type:S,GroupId:" + groupId + "," +
        "Topic:" + topic + ",Description:,AnswerType:Grid,AnswerDefine:" + topic + "," +
        "NextAnswer:0,Needed:Y;";
            }
        });

        base.callAjax("../QuestionnaireService.asmx/SaveDetailGroup",
        "{'value': '" + value + "'}",
         function (data) {
             if (base.currentSaveType == saveType.saveButton) {
                 alert("儲存成功"); //儲存成功
             }
             else if (base.currentSaveType == saveType.sideMenu) {
                 //在SideMenu按下加入
                 var p = parent.nextAll(":last");
                 if (p.size() > 0) {
                     $this.saveData(p);
                 }
             }
             else if ($this.currentSaveType == saveType.copyAdd) {
                 base.reloadAllDatas(parent);
             }
         });

    };

    var saveMaster = function (masterPK, detailPK, userPK, parent) {
        var $this = this;
        var heading = $("#txtHeading").val();
        var desc = $("#txtDesc").val();
        var startDate = $("#txtStartDate").val();
        var endDate = $("#txtEndDate").val();
        var comment = $("#txtComment").val();
        var category = $("#ddlCategory").val();
        var oneTime = $("#ckbOneTime").prop("checked");

        var valid = base.validData();
        if (!valid.returnValue) {
            alert(valid.message);
            return;
        }
        base.callAjax("QuestionnaireService.asmx/SaveMaster",
    "{'qPK': '" + masterPK + "', 'heading': '" + heading + "', 'desc': '" + desc + "', 'category':'" + category + "', 'status':'', 'startDate': '" + startDate + "', 'endDate': '" + endDate + "', 'userModulePK': '" + userPK + "', 'comment': '" + comment + "', 'oneTime': " + oneTime + "}",
    function (data) {
        $("#hfMaster").val(data.d);
        $this.saveDetail(detailPK, data.d, parent);
    });

    };

    return {
        createContent: function (masterPK) {
            init();
            base.createContent(masterPK);
        },
        createControls: function (masterPK) {
            init();
            base.createControls(masterPK);
        },
        createSortContent: function (masterPk) {
            init();
            base.createSortContent(masterPk);
        }
    };
})(controlEditorBase);