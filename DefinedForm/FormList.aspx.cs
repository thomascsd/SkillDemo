using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DefinedForm_FormList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LoadData();
        }

        string url = this.ResolveUrl("~/DefinedForm/FormSetting.aspx");
        Stool.RegisterSimpleModal(PageType.AJAX, this, hlAdd.Attributes, url);
    }

    private void LoadData()
    {
        DefinedFormManager fm = new DefinedFormManager();
        var fds = fm.GetAllData();

        gvList.DataSource = fds;
        gvList.DataBind();
    }

    protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        HyperLink hlUpdate;
        string pk;
        string url = this.ResolveUrl("~/DefinedForm/FormSetting.aspx");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            hlUpdate = (HyperLink)e.Row.FindControl("hlUpdate");
            pk = DataBinder.Eval(e.Row.DataItem, "PK").ToString();
            url += "?p=" + pk;
            hlUpdate.Attributes["onclick"] = Stool.GetSimpleModalInitFunction(url);
        }

    }
    protected void gvList_PreRender(object sender, EventArgs e)
    {
        if (gvList.HeaderRow != null)
        {
            gvList.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
}