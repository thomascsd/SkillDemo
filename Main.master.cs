using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Main : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CreateMenu();
    }

    private void CreateMenu()
    {
        SiteMapProvider provider = SiteMap.Provider;
        ListMenu lm = new ListMenu
        {
            ULClass = "clear"
        };

        this.CreateMenuRecursively(provider.RootNode.ChildNodes, lm.Items);
        litNavbar.Text = lm.ToString();
    }

    private void CreateMenuRecursively(SiteMapNodeCollection nodes, ListMenuItemCollection items)
    {
        ListMenuItem item;

        foreach (SiteMapNode node in nodes)
        {
            item = new ListMenuItem(node.Title, node.Url);
            items.Add(item);
            this.CreateMenuRecursively(node.ChildNodes, item.ChildItems);
        }

    }

}
