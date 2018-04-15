using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DefinedForm_FormSetting : System.Web.UI.Page
{
    private string Pk;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Pk = Request.QueryString["p"] ?? "0";
        this.LoadData();
    }

    private void LoadData()
    {
        DefinedFormManager dfm = new DefinedFormManager();
        var data = dfm.GetDataByPk(this.Pk);

        hfUserPK.Value = "0";
        if (data == null)
        {
            hfMaster.Value = "0";
        }
        else
        {
            hfMaster.Value = data.PK.ToString();
            txtTitle.Text = data.Title;
        }

    }

}