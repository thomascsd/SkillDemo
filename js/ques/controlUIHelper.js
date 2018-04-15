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
	},

	createCalendar: function (detailPK, needed, value) {
		var txt = $("<input type='text'/>").attr({ "needed": needed, name: "txt" + detailPK, id: "txt" + detailPK }).width(120).addClass("date dateCalendar");
		if (needed) {
			txt.addClass("required");
		}
		if (value) {
			txt.val(value);
		}

		var options = {
			//showOn: 'button',
			//buttonImage: 'images/calendar.gif',
			//buttonImageOnly: true,
			showAnim: "slideDown",
			changeMonth: true,
			changeYear: true,
			yearRange: '1920:c+5'
		};
		var name = $("#_cultureName").val();
		options = $.extend(options, $.datepicker.regional[name]);

		txt.datepicker(options);
		return txt;
	},
	createEMail: function (detailPK, needed, value) {
		var txt = $("<input type='text' class='qText'/>")
		.attr({ "needed": needed, name: "txt" + detailPK, id: "txt" + detailPK }).width(300).addClass("email");
		if (needed) {
			txt.addClass("required");
		}
		if (value) {
			txt.val(value);
		}
		return txt;
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