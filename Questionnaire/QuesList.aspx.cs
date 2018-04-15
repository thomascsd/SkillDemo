using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class QuesList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LoadData();
        }

        string url = this.ResolveUrl("~/Questionnaire/QuesSetting.aspx");
        Stool.RegisterSimpleModal(PageType.PostBack, this, hlAdd.Attributes, url);
    }


    private void LoadData()
    {
        QuestionnaireMasterManager qm = new QuestionnaireMasterManager();
        var masters = qm.GetAllData();

        gvList.DataSource = masters;
        gvList.DataBind();
    }

    protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int id;
        string url = this.ResolveUrl("~/Questionnaire/QuesSetting.aspx");
        HyperLink hlUpdate;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            hlUpdate = (HyperLink)e.Row.FindControl("hlUpdate");
            id = (int)DataBinder.Eval(e.Row.DataItem, "Id");

            url += "?p=" + id;
            hlUpdate.Attributes["onclick"] = Stool.GetSimpleModalInitFunction(url);
        }

    }
    protected void gvList_PreRender(object sender, EventArgs e)
    {
        gvList.UseAccessibleHeader = true;
        gvList.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
}