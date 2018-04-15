using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class QuesSetting : System.Web.UI.Page
{
    private string MasterId;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.MasterId = Request.QueryString["p"] ?? "0";
        this.LoadData();
    }

    private void LoadData()
    {
        QuestionnaireMasterManager qm = new QuestionnaireMasterManager();
        QuestionnaireMasterData qmd;

        qmd = qm.GetDataById(this.MasterId);
        this.FillDropDown();

        if (qmd == null)
        {
            hfMaster.Value = "0";
            txtStartDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            txtEndDate.Text = DateTime.Now.AddMonths(1).ToString("yyyy/MM/dd");
            txtComment.Text = "感謝填寫問卷";
            ddlCategory.SelectedValue = QuestionnaireType.Process.ToString();
            ckbOneTime.Checked = false;
        }
        else
        {
            hfMaster.Value = qmd.Id.ToString();
            txtHeading.Text = qmd.Heading;
            txtDesc.Text = qmd.Description;
            txtComment.Text = qmd.Comment;
            txtStartDate.Text = qmd.StartDate.ToString("yyyy/MM/dd");
            txtEndDate.Text = qmd.EndDate.ToString("yyyy/MM/dd");
            ddlCategory.SelectedValue = qmd.Category;
            ckbOneTime.Checked = qmd.OneTime;
        }

    }

    private void FillDropDown()
    {
        CodeDefManager cm = new CodeDefManager();
        var cds = cm.GetDataByCodeType("qType");

        ddlCategory.DataSource = cds;
        ddlCategory.DataTextField = "Description";
        ddlCategory.DataValueField = "Code";
        ddlCategory.DataBind();
    }

}