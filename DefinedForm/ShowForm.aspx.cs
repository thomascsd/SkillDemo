using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DefinedForm_ShowForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LoadData();
            pnlRight.Visible = false;
        }

        Stool.RegisterSimpleModal(PageType.AJAX, this);
    }

    private void LoadData()
    {
        DefinedFormManager dfm = new DefinedFormManager();
        var datas = dfm.GetAllData();

        gvForm.DataKeyNames = new string[] { "PK" };
        gvForm.DataSource = datas;
        gvForm.DataBind();
    }

    private void ShowForm(string pk)
    {
        DefinedFormManager dfm = new DefinedFormManager();
        DefinedFormData form;

        //取得UserModulePK
        form = dfm.GetDataByPk(pk);
        if (form != null)
        {
            hfFormPk.Value = form.PK.ToString();
            litTitle.Text = form.Title;
        }
    }

    protected void gvForm_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string pk, url;
        HyperLink hlResult;

        url = this.ResolveUrl("FormResult.aspx");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            hlResult = (HyperLink)e.Row.FindControl("hlResult");
            pk = gvForm.DataKeys[e.Row.RowIndex].Value.ToString();
            url += "?p=" + pk;
            hlResult.Attributes["onclick"] = Stool.GetSimpleModalInitFunction(url);

        }
    }

    protected void gvForm_SelectedIndexChanged(object sender, EventArgs e)
    {
        string pk = gvForm.SelectedDataKey.Value.ToString();
        pnlRight.Visible = true;
        this.ShowForm(pk);

    }
    protected void gvForm_PreRender(object sender, EventArgs e)
    {
        if (gvForm.HeaderRow != null)
        {
            gvForm.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

    }
}