using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Questionnaire_ShowQues : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            pnlRight.Visible = false;
            this.LoadData();
        }
    }

    private void LoadData()
    {
        QuestionnaireMasterManager qm = new QuestionnaireMasterManager();
        var datas = qm.GetAllData();

        gvList.DataKeyNames = new string[] { "Id" };
        gvList.DataSource = datas;
        gvList.DataBind();
    }

    protected void gvList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string masterId = gvList.SelectedDataKey.Value.ToString();
        pnlRight.Visible = true;
        this.ShowQuestionnaire(masterId);

    }

    private void ShowQuestionnaire(string masterId)
    {
        string js;
        QuestionnaireType qt;
        QuestionnaireMasterManager qm = new QuestionnaireMasterManager();
        var master = qm.GetDataById(masterId);

        if (master != null)
        {
            hfMasterPK.Value = master.Id.ToString();
            hlChart.Attributes["onclick"] = Stool.GetSimpleModalInitFunction("QuestionnareChart.aspx?pk=" + master.Id.ToString());
            lblHeading.Text = master.Heading;
            litChartResult.Text = master.Heading;
            lblDescription.Text = master.Description.Replace("/r/n", "<br/>");
            hfComment.Value = master.Comment;

            //問卷類型
            qt = (QuestionnaireType)Enum.Parse(typeof(QuestionnaireType), master.Category);
            if (qt == QuestionnaireType.Process)
            {
                pnlList.Visible = false;

            }
            else
            {
                //清單(List)的顯示狀態和程序(process)是一樣
               // pnlList.Visible = pnlQues.Visible;
                pnlList.Visible = true;
                pnlQues.Visible = false;
            }
            hfQuestionnaireType.Value = qt.ToString();

            //是否到期
            if (DateTime.Now > master.EndDate)
            {
                pnlMain.Visible = false;
                //lblMessage.Visible = true;
                lblMessage.Style["display"] = "block;";
                hfShowChart.Value = "Y";
            }

            //註冊javascript，建立問卷UI
            js = string.Format(" quesUIObject.createContent('{5}','{0}','{1}', '{2}', '{3}', '{4}');", hfComment.ClientID, hfBeginShowChart.Value, hfChartResult.Value, hfShowChart.Value, hfQuestionnaireType.Value, master.Id);
            js += "function setCookie(){";
            if (master.OneTime)
            {
                var completed = "此問卷已填寫";
                js += " var value = $.cookie(quesUIObject.qStatus + \"_\" + quesUIObject.masterPK);";
                js += " if (value == \"complete\") {";
                js += "     $(\"div.visible\").hide();";
                js += string.Format("$(\"#{0}\").show().html('{1}');", lblMessage.ClientID, completed);
                js += "}";
            }
            js += "}";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "one", js, true);
        }

    }

    protected void gvList_PreRender(object sender, EventArgs e)
    {
        gvList.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

}