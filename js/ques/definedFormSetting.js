/// <reference path="controlEditorBase.js" />

var formObject = (function (base) {
    var init = function () {
        base.createTable = createTable;
        base.saveData = saveData;
        base.getElementTypeListUrl = "DefindedFormService.asmx/GetElementTypeList";
        base.getDetailsUrl = "DefindedFormService.asmx/GetDetails";
        base.updateSortUrl = "DefindedFormService.asmx/UpdateSort";
        base.deleteDataUrl = "DefindedFormService.asmx/DeleteData";
        base.detailTopicName = "Title";
        base.detailNeededName = "Required";
        base.detailIdName = "PK";
    };

    var createTable = function (data) {
        var table = base.createTableMain(data);
        return table;
    };

    var saveData = function (parent) {
        var master = $("#hfMaster").val();
        var userPK = $("#hfUserPK").val();
        var detail = parent.find(".qDetail").val();
        saveMaster(master, detail, userPK, parent);
    };

    var saveMaster = function (masterPK, detailPK, userPK, parent) {
        var $this = this;
        var title = $("#txtTitle").val();
        base.callAjax("DefindedFormService.asmx/SaveMaster",
        "{'pk':'" + masterPK + "','userPk':'" + userPK + "','title':'" + title + "'}",
        function (data) {
            $("#hfMaster").val(data.d);
            masterPK = data.d;
            $this.saveDetail(detailPK, masterPK, parent);
        });
    };

    this.saveDetail = function (detailPk, masterPk, parent) {
        var $this = this;
        var title = parent.find("input.qEle").val();
        var eleType = parent.find("select.qEle").val();
        var item = base.getElementType(eleType, parent);
        var required = parent.find(".needed")[0].checked;
        var sort = parent.find(".qSort").val();
        var json = "{'data': {'PK':" + detailPk + ", 'MasterPK':" + masterPk + ",'title':'" + title + "',";
        json += "'AnswerType': '" + eleType + "','AnswerDefine':'" + item.itemText + "','Required':'" + required + "', 'Sort': '" + sort + "'}}";

        base.callAjax("DefindedFormService.asmx/SaveDetail", json,
        function (data) {
            parent.find(".qDetail").val(data.d);
            if (base.currentSaveType == saveType.saveButton) {
                alert("儲存成功"); 
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