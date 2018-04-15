using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

public partial class DefinedForm_FormResult : System.Web.UI.Page
{
    private string PK;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.PK = Request.QueryString["p"];

        this.LoadData();
    }

    private void LoadData()
    {
        DefinedFormManager masterManager = new DefinedFormManager();
        DefinedFormDetailManager detailManager = new DefinedFormDetailManager();
        DefinedFormAnswerManager answerManager = new DefinedFormAnswerManager();
        List<DefinedFormDetailData> details = new List<DefinedFormDetailData>();
        DataTable dt;

        var master = masterManager.GetDataByPk(this.PK);
        if (master != null)
        {
            //取得Detail
            details = detailManager.GetDataByMasterPK(master.PK.ToString());
            //取得Answer
            var answers = answerManager.GetDataByDetailPK(master.PK.ToString());

            dt = this.CreateDataTable(details, answers);

            gvResult.AutoGenerateColumns = false;
            foreach (DataColumn col in dt.Columns)
            {
                BoundField bf = new BoundField
                {
                    HtmlEncode = false,
                    DataField = col.ColumnName,
                    HeaderText = col.ColumnName
                };
                gvResult.Columns.Add(bf);
            }
            gvResult.DataSource = dt;
            gvResult.DataBind();
        }

    }

    private DataTable CreateDataTable(List<DefinedFormDetailData> details, List<DefinedFormAnswerData> answers)
    {
        DataTable dt = new DataTable();
        DataRow dtRow = null;
        int maxCount;

        //加入Column
        foreach (var detail in details)
        {
            DataColumn col = new DataColumn(detail.Title)
            {
                Caption = detail.PK.ToString()
            };
            dt.Columns.Add(col);
        }

        //加入資料
        maxCount = dt.Columns.Count;
        for (var i = 0; i < answers.Count; i++)
        {
            var answer = answers[i];
            if (i % maxCount == 0)
            {
                dtRow = dt.NewRow();
            }

            foreach (DataColumn col in dt.Columns)
            {
                if (col.Caption == answer.DetailPK.ToString())
                {
                    var value = answer.AnswerValue;
                    value = value.Replace("&", "");
                    value = value.Replace("$", "<br/>");
                    dtRow[col] = value;
                }

            }
            if (i % maxCount == 0)
            {
                dt.Rows.Add(dtRow);
            }

        }
        dt.AcceptChanges();
        return dt;
    }

}