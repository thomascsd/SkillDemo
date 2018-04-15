using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;

/// <summary>
/// 清單選單
/// </summary>
public class ListMenu
{
    private ListMenuItemCollection mItems;
    public ListMenuItemCollection Items
    {
        get { return mItems; }
    }

    /// <summary>
    /// Html的項目ul的class
    /// </summary>
    public string ULClass { get; set; }
    /// <summary>
    /// Html的項目ul的樣式
    /// </summary>
    public string ULStyle { get; set; }

    public string DivClass { get; set; }

    public string ID { get; set; }
    /// <summary>
    /// ListMenu的階層
    /// </summary>
    private int mLevel;

    /// <summary>
    /// 控項項的配置
    /// </summary>
    public Orientation Orientation { get; set; }

    public ListMenu()
    {
        this.mItems = new ListMenuItemCollection();
        this.Orientation = Orientation.Vertical;
        this.mLevel = 0;
    }

    /// <summary>
    ///  遞歸建立Element
    /// </summary>
    /// <param name="items"></param>
    /// <param name="sb"></param>
    private void BuildElementRecursive(ListMenuItemCollection items, StringBuilder sb, int index)
    {
        string target = string.Empty, className, id, style;
        this.mLevel += 1;
        index++;

        if (items.Count > 0)
        {
            //sb.AppendFormat("<div id=\"{0}\" class=\"{0}\">", this.ID, this.DivClass).AppendLine();
            if (this.mLevel == 1)
            {
                if (string.IsNullOrEmpty(this.ULClass))
                {
                    className = string.Empty;
                }
                else
                {
                    className = string.Format("class=\"{0}\"", this.ULClass);
                }

            }
            else
            {
                className = string.Empty;
            }

            id = string.IsNullOrEmpty(this.ID) ? "" : string.Format("id=\"{0}\" ", index > 1 ? this.ID + "_" + index : this.ID);
            style = string.IsNullOrEmpty(this.ULStyle) ? "" : string.Format("style=\"{0}\"", this.ULStyle);

            sb.AppendFormat("<ul {0} {1} {2}>", className, id, style).AppendLine();
        }

        foreach (ListMenuItem item in items)
        {
            sb.AppendLine(item.Start());

            //遞歸建立
            this.BuildElementRecursive(item.ChildItems, sb, index);
            sb.AppendLine(item.End());
        }

        if (items.Count > 0)
        {
            sb.AppendLine("</ul>");
            //sb.AppendLine("</div>");
        }

    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        int index = 0;
        this.BuildElementRecursive(this.mItems, sb, index);

        return sb.ToString();
    }


}

/// <summary>
/// 清單選單項目
/// </summary>
public class ListMenuItem
{
    public string Text { get; set; }

    public string Value { get; set; }

    public string Target { get; set; }
    /// <summary>
    /// 設定或取得樣式
    /// </summary>
    public string Style { get; set; }

    /// <summary>
    /// 已選的li的class名稱
    /// </summary>
    public string SelectedLICssClass { get; set; }

    /// <summary>
    /// li的class名稱
    /// </summary>
    public string LICssClass { get; set; }
    /// <summary>
    /// Image的樣式名稱
    /// </summary>
    public string ImageCssClass { get; set; }
    /// <summary>
    /// 設定或取得是否已選取
    /// </summary>
    public bool Selected { get; set; }

    public ListMenuItemCollection ChildItems { get; set; }
    /// <summary>
    /// 選單項目
    /// </summary>
    public ListMenuItemType ItemType { get; set; }

    /// <summary>
    /// a的class名稱
    /// </summary>
    public string LinkCssClass { get; set; }
    /// <summary>
    /// 當ItemType為ListMenuItemType.Image時的圖檔路徑
    /// </summary>
    public string ImageUrl { get; set; }
    /// <summary>
    /// 提示
    /// </summary>
    public string Tooltip { get; set; }
    /// <summary>
    /// 多國語言的Key
    /// </summary>
    public string ResourceKey { get; set; }

    public ListMenuItem()
    {
        this.ChildItems = new ListMenuItemCollection();
        this.ItemType = ListMenuItemType.Link;
        this.ImageUrl = string.Empty;
        this.ResourceKey = string.Empty;
        this.Text = string.Empty;
    }

    public ListMenuItem(string text)
        : this()
    {
        this.Text = text;
        this.Value = text;
    }

    public ListMenuItem(string text, string value)
        : this()
    {
        this.Text = text;
        this.Value = value;
    }

    /// <summary>
    /// ListMenu開始
    /// </summary>
    /// <returns></returns>
    public string Start()
    {
        string href, target = string.Empty, className, style, imageUrl = string.Empty;
        string imageClass = string.Empty, text;
        StringBuilder sb = new StringBuilder();

        className = string.IsNullOrEmpty(this.LICssClass) ? "" : string.Format("class= \"{0}\"", this.LICssClass);
        if (this.Selected)
        {
            className = string.Format("class=\"{0}\" ", this.SelectedLICssClass);
        }

        style = string.IsNullOrEmpty(this.Style) ? "" : string.Format("style=\"{0}\"", this.Style);

        sb.AppendFormat("  <li {0} {1}>", className, style).AppendLine();

        if (!string.IsNullOrEmpty(this.Target))
        {
            target = string.Format("target=\"{0}\"", this.Target);
        }

        className = string.Empty;
        if (!string.IsNullOrEmpty(this.LinkCssClass))
        {
            className = string.Format("class=\"{0}\" ", this.LinkCssClass);
        }

        href = string.IsNullOrEmpty(this.Value) ? "#" : this.Value;
        if (!string.IsNullOrEmpty(this.ImageUrl))
        {
            imageUrl = VirtualPathUtility.ToAbsolute(this.ImageUrl);
        }
        text = this.Text;

        //項目類型
        switch (this.ItemType)
        {
            case ListMenuItemType.Link:
                sb.AppendFormat(" <a href=\"{0}\" {2} {3} title=\"{4}\">{1}</a>", href, text, target, className, this.Tooltip);
                break;
            case ListMenuItemType.Label:
                sb.Append(text);
                break;
            case ListMenuItemType.Image:
                sb.AppendFormat(" <a href=\"{0}\" {1} {2}><img src=\"{3}\" alt=\"menu\" title=\"{4}\"/></a>", href, target, className, imageUrl, this.Tooltip);
                break;
            case ListMenuItemType.ImageAndText:
                imageClass = string.IsNullOrEmpty(this.ImageCssClass) ? "" : string.Format("class=\"{0}\"", this.ImageCssClass);
                sb.AppendFormat(" <a href=\"{0}\" {1} {2} ><img src=\"{3}\" {5} alt=\"menu\" title=\"{6}\"/>{4}</a>", href, target, className, imageUrl, text, imageClass, this.Tooltip);
                break;
            default:
                break;
        }

        return sb.ToString();
    }

    /// <summary>
    /// ListMenu結束
    /// </summary>
    /// <returns></returns>
    public string End()
    {
        return " </li>";
    }

}

/// <summary>
/// ListMenuItem集合
/// </summary>
public class ListMenuItemCollection : List<ListMenuItem>
{

}

/// <summary>
/// 項目類型
/// </summary>
public enum ListMenuItemType
{
    /// <summary>
    /// 連結
    /// </summary>
    Link,
    /// <summary>
    /// 文字
    /// </summary>
    Label,
    /// <summary>
    /// 圖片，包含連結
    /// </summary>
    Image,
    /// <summary>
    /// 圖片及連結文字
    /// </summary>
    ImageAndText
}