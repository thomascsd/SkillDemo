using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

/// <summary>
/// 公用工具
/// </summary>
public static class Stool
{
    /// <summary>
    /// pageBind
    /// </summary>
    public static readonly string PageBind = "pageBind";
    /// <summary>
    /// 穩藏欄位值，__EVENTTARGET
    /// </summary>
    public static readonly string EventTarget = "__EVENTTARGET";
    /// <summary>
    /// 穩藏欄位值，__EVENTARGUMENT
    /// </summary>
    public static readonly string EventArgument = "__EVENTARGUMENT";
    /// <summary>
    /// 穩藏欄位值，WindowConfirm
    /// </summary>
    public static readonly string WindowConfirm = "WindowConfirm";

    /// <summary>
    /// 註冊關閉視窗並會產生PostBack的javascript
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    /// <param name="isParentWindowAjax">是否父視窗有套用Ajax</param>
    /// <param name="fieldValue">穩藏欄位的值</param>
    /// <param name="isWindowClose">子視窗是否關閉</param>
    public static void RegisterScript(PageType pageType, Page page, bool isParentWindowAjax,
                                      string fieldValue, bool isWindowClose)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine("if (window.opener) { ");

        if (pageType == PageType.AJAX && isParentWindowAjax)
        {
            sb.AppendLine("var divId = findId();");
            sb.AppendFormat("window.opener.__doPostBack(divId,'{0}');", fieldValue).AppendLine();
        }
        else
        {
            sb.AppendFormat("  window.opener.document.getElementById('__EVENTARGUMENT').value = '{0}';", fieldValue).AppendLine();
            sb.AppendLine("  window.opener.document.getElementById('__EVENTTARGET').value = '';");
            sb.AppendLine("  window.opener.document.forms[0].submit();");
        }

        sb.AppendLine("}");

        if (isWindowClose)
        {
            sb.AppendLine("window.close();");
        }

        sb.AppendLine("function findId(){");
        sb.AppendLine("	var divs = window.opener.document.getElementsByTagName('div');");
        sb.AppendLine("	var divId= '';");
        sb.AppendLine("	for(var i = 0 ;i < divs.length; i++){");
        sb.AppendLine("		var dv = divs[i];");
        sb.AppendLine("		if(dv.id.indexOf('UpdatePanel') != -1){");
        sb.AppendLine("			divId = dv.id;");
        sb.AppendLine("			break;");
        sb.AppendLine("		}");
        sb.AppendLine("	}");
        sb.AppendLine("	return divId;");
        sb.AppendLine("}");


        if (pageType == PageType.PostBack)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "Clsoe", sb.ToString(), true);
        }
        else
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), "close", sb.ToString(), true);

        }
    }

    /// <summary>
    ///  註冊關閉視窗並會產生PostBack的javascript，穩藏欄位的值為pageBind，子視窗為關閉
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    public static void RegisterScript(PageType pageType, Page page)
    {
        RegisterScript(pageType, page, false, "pageBind", true);
    }

    /// <summary>
    /// 註冊關閉視窗並會產生PostBack的javascript，穩藏欄位的值為pageBind
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    /// <param name="isWindowClose">子視窗是否關閉</param>
    public static void RegisterScript(PageType pageType, Page page, bool isWindowClose)
    {
        RegisterScript(pageType, page, false, "pageBind", isWindowClose);
    }

    /// <summary>
    /// 註冊關閉視窗並會產生PostBack的javascript，子視窗關閉
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    /// <param name="fieldValue">穩藏欄位的值</param>
    public static void RegisterScript(PageType pageType, Page page, string fieldValue)
    {
        RegisterScript(pageType, page, false, fieldValue, true);
    }

    /// <summary>
    /// 註冊關閉視窗並會產生PostBack的javascript，子視窗為關閉
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    /// <param name="isParentWindowAjax">是否父視窗有套用Ajax</param>
    /// <param name="fieldValue">穩藏欄位的值</param>
    public static void RegisterScript(PageType pageType, Page page, bool isParentWindowAjax,
                                      string fieldValue)
    {
        RegisterScript(pageType, page, isParentWindowAjax, fieldValue, true);
    }

    /// <summary>
    /// 註冊關閉視窗並會產生PostBack的javascript，無附加Script
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    /// <param name="fieldValue">穩藏欄位的值</param>
    /// <param name="isWindowClose">子視窗是否關閉</param>
    public static void RegisterScript(PageType pageType, Page page, string fieldValue, bool isWindowClose)
    {
        RegisterScript(pageType, page, false, fieldValue, isWindowClose);
    }

    /// <summary>
    /// 是由jQuery plugin SimpleModal來產生非回應視窗，註冊會產生PostBack的javascript，
    /// 穩藏欄位的值為pageBind，子視窗為關閉(http://www.ericmmartin.com/projects/simplemodal/)
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    public static void RegisterScriptOnSimpleModal(PageType pageType, Page page)
    {
        Stool.RegisterScriptOnSimpleModal(pageType, page, "pageBind");
    }

    /// <summary>
    /// 是由jQuery plugin SimpleModal來產生非回應視窗(http://www.ericmmartin.com/projects/simplemodal/)
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="isParentWindowAjax">是否父視窗有套用Ajax</param>
    public static void RegisterScriptOnSimpleModal(PageType pageType, Page page, bool isParentWindowAjax)
    {
        Stool.RegisterScriptOnSimpleModal(pageType, page, "pageBind", true, isParentWindowAjax);
    }

    /// <summary>
    /// 是由jQuery plugin SimpleModal來產生非回應視窗，註冊會產生PostBack的javascript，子視窗關閉
    /// (http://www.ericmmartin.com/projects/simplemodal/)
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="fieldValue">穩藏欄位的值</param>
    public static void RegisterScriptOnSimpleModal(PageType pageType, Page page, string fieldValue)
    {
        Stool.RegisterScriptOnSimpleModal(pageType, page, fieldValue, true, false);
    }

    /// <summary>
    /// 是由jQuery plugin SimpleModal來產生非回應視窗，註冊會產生PostBack的javascript
    /// (http://www.ericmmartin.com/projects/simplemodal/)
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="fieldValue">穩藏欄位的值</param>
    /// <param name="isWindowClose">子視窗是否關閉</param>
    /// <param name="isParentWindowAjax">是否父視窗有套用Ajax</param>
    public static void RegisterScriptOnSimpleModal(PageType pageType, Page page, string fieldValue, bool isWindowClose,
                                                   bool isParentWindowAjax)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine(" function doParent() {");
        sb.AppendFormat("  window.parent.document.getElementById('__EVENTARGUMENT').value = '{0}';", fieldValue).AppendLine();
        sb.AppendLine("  window.parent.document.getElementById('__EVENTTARGET').value = '';");

        if (isParentWindowAjax)
        {
            sb.AppendLine("var divId = findId();");
            sb.AppendLine(" if(divId != '') {");
            sb.AppendFormat("window.parent.__doPostBack(divId,'{0}');", fieldValue).AppendLine();
            sb.AppendLine(" } else {");
            sb.AppendLine("  window.parent.document.forms[0].submit();");
            sb.AppendLine(" }");
        }
        else
        {
            sb.AppendLine("  window.parent.document.forms[0].submit();");
        }

        if (isWindowClose)
        {
            sb.AppendLine("window.parent.closeModal();");
        }
        sb.AppendLine("}");

        sb.AppendLine("function findId(){");
        sb.AppendLine("	var divs = window.parent.document.getElementsByTagName('div');");
        sb.AppendLine("	var divId= '';");
        sb.AppendLine("	for(var i = 0 ;i < divs.length; i++){");
        sb.AppendLine("		var dv = divs[i];");
        sb.AppendLine("		if(dv.id.indexOf('UpdatePanel') != -1){");
        sb.AppendLine("			divId = dv.id;");
        sb.AppendLine("			break;");
        sb.AppendLine("		}");
        sb.AppendLine("	}");
        sb.AppendLine("	return divId;");
        sb.AppendLine("}");

        sb.AppendLine("doParent();");

        if (pageType == PageType.PostBack)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "OnSimpleModal", sb.ToString(), true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "OnSimpleModal", sb.ToString(), true);
        }

    }

    /// <summary>
    /// 是由jQuery plugin SimpleModal來產生非回應視窗，並用Ajax的方式，呼叫url
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="url"></param>
    public static void RegisterUrlOnSimpleModal(PageType pageType, Page page, string url)
    {
        Stool.RegisterUrlOnSimpleModal(pageType, page, url, string.Empty, string.Empty);
    }

    /// <summary>
    /// 是由jQuery plugin SimpleModal來產生非回應視窗，並用Ajax的方式，呼叫url
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <param name="callbackOnSuccess">執行成功時，所呼叫的函式</param>
    public static void RegisterUrlOnSimpleModal(PageType pageType, Page page, string url, string data,
                                                string callbackOnSuccess)
    {
        string js;

        if (string.IsNullOrEmpty(data))
        {
            data = "{}";
        }
        if (string.IsNullOrEmpty(callbackOnSuccess))
        {
            callbackOnSuccess = "function(){}";
        }

        js = string.Format("$.post('{0}', {1}, {2});", url, data, callbackOnSuccess);
        js += "window.parent.closeModal();";

        if (pageType == PageType.PostBack)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "RegisterUrlOnSimpleModal", js, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "RegisterUrlOnSimpleModal", js, true);
        }
    }

    /// <summary>
    /// 註冊jQuery plugin SimpleModal，不指定網址，由GetSimpleModalInitFunction各別指定網址
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    public static void RegisterSimpleModal(PageType pageType, Page page)
    {
        Stool.RegisterSimpleModal(pageType, page, null, string.Empty, false);
    }

    /// <summary>
    ///  註冊jQuery plugin SimpleModal
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="attributes"></param>
    /// <param name="url"></param>
    public static void RegisterSimpleModal(PageType pageType, Page page, AttributeCollection attributes,
                                          string url)
    {
        Stool.RegisterSimpleModal(pageType, page, attributes, url, false);
    }

    /// <summary>
    /// 註冊jQuery plugin SimpleModal
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="attributes"></param>
    /// <param name="url"></param>
    /// <param name="refreshOnClose">視窗關閉時，是否更新頁面</param>
    public static void RegisterSimpleModal(PageType pageType, Page page, AttributeCollection attributes,
                                           string url, bool refreshOnClose)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (attributes != null)
        {
            attributes.Add("onclick", string.Format("initModal('{0}');", url));
        }

        sb.Append("    function initModal(url) {");
        sb.Append("var width, height, modalWidth, modalHeight;");
        sb.Append("width = getDocWidth();");
        sb.Append("height = getDocHeight();");
        sb.Append("modalHeight = height - 20;");
        sb.Append("modalWidth = width - 110;");
        sb.Append("   $.modal('<iframe src=\"'+ url +'\" frameborder=\"0\"  width=\"' + modalWidth + 'px\" height=\"' + modalHeight + 'px\" style=\"border:0\" />',");
        sb.Append("{");
        sb.Append("                appendTo: 'form:first',");
        sb.Append("                position: ['10px', '50px'],");
        sb.Append("                zIndex: 9000,");
        sb.Append("                minWidth: modalWidth,");
        sb.Append("                minHeight: modalHeight,");
        sb.Append("                containerCss :{");
        sb.Append("                 width: modalWidth,");
        sb.Append("                 height: modalHeight");
        sb.Append("                }");
        if (refreshOnClose)
        {
            sb.Append("              ,");
            sb.Append("        onClose: function(dialog){");
            sb.Append("           var divId = findId();");
            sb.Append("           __doPostBack(divId,'');");
            sb.Append("           $.modal.close();");
            sb.Append("        }");
        }

        sb.Append("            });");

        sb.AppendLine("    }");
        sb.Append("   function  getDocHeight(){");
        sb.Append("    return Math.min(");
        sb.Append("        $(document).height(),");
        sb.Append("        $(window).height(),");
        sb.Append("        document.documentElement.clientHeight");
        sb.Append("    );");
        sb.AppendLine("}");
        sb.Append("  function getDocWidth(){");
        sb.Append("    return Math.min(");
        sb.Append("        $(document).width(),");
        sb.Append("        $(window).width(),");
        sb.Append("        document.documentElement.clientWidth");
        sb.Append("    );");
        sb.AppendLine("}");

        sb.Append("function closeModal() {");
        sb.Append("   $.modal.close();");
        sb.AppendLine("}");

        sb.Append("function findId(){");
        sb.Append("	var divs = document.getElementsByTagName('div');");
        sb.Append("	var divId= '';");
        sb.Append("	for(var i = 0 ;i < divs.length; i++){");
        sb.Append("		var dv = divs[i];");
        sb.Append("		if(dv.id.indexOf('UpdatePanel') != -1){");
        sb.Append("			divId = dv.id;");
        sb.Append("			break;");
        sb.Append("		}");
        sb.Append("	}");
        sb.Append("	return divId;");
        sb.AppendLine("}");

        if (pageType == PageType.PostBack)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "close_Modal", sb.ToString(), true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "close_Modal", sb.ToString(), true);
        }

    }

    /// <summary>
    /// 不在RegisterSimpleModal中指定網址，就用此方法設定SimpleModal初始函式
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetSimpleModalInitFunction(string url)
    {
        return string.Format("initModal('{0}');", url);
    }


    /// <summary>
    ///註冊一警告視窗，以作顯示訊息用
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="message"></param>
    public static void ShowMessage(PageType pageType, Page page, string message)
    {
        ShowMessage(pageType, page, message, string.Empty);
    }

    /// <summary>
    /// 註冊一警告視窗，以作顯示訊息用，並轉至另一網址
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="message">訊息</param>
    /// <param name="url">網址</param>
    public static void ShowMessage(PageType pageType, Page page, string message, string url)
    {
        if (string.IsNullOrEmpty(message))
        {
            message = string.Empty;
        }
        message = message.Replace("'", "");
        message = message.Replace("\r\n", "\\n");
        message = message.Replace("<br/>", "\\n");
        message = message.Replace("\n", "\\n");

        string js = "window.alert('" + message + "');";

        if (!string.IsNullOrEmpty(url))
        {
            if (url.IndexOf("~") != -1)
            {
                url = VirtualPathUtility.ToAbsolute(url);
            }

            js += string.Format("window.location.href = '{0}';", url);
            //js += string.Format("__doPostBack('ShowMessage', '{0}');", url);
        }

        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "_Message", js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "_Message", js, true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 註冊一確認視窗，按確認後，並PostBack，欄位名稱為WindowConfirm
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="message"></param>
    /// <param name="isPostBackOnCancel">是否在按下取消時，產生PostBack</param>
    public static void ShowConfirm(PageType pageType, Page page, string message,
                                   bool isPostBackOnCancel)
    {
        message = message.Replace("\r\n", "\\n");
        string js;

        Stool.InitFunction(pageType, page, isPostBackOnCancel);

        js = string.Format("confirmData('{0}');", message);

        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "confirm001", js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "confirm001", js, true);
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// 註冊window.confirm的javascript
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="isPostBackOnCancel">是否在按下取消時，產生PostBack</param>
    private static void InitFunction(PageType pageType, Page page, bool isPostBackOnCancel)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("function confirmData(message)");
        sb.Append("{");
        sb.Append("	var field = document.getElementById('WindowConfirm');");
        sb.Append("	if(window.confirm(message))");
        sb.Append("	{");
        sb.Append("		field.value = 'y';");

        if (!isPostBackOnCancel)
        {
            sb.Append("		document.forms[0].submit();");
        }

        sb.Append("	}");

        if (isPostBackOnCancel)
        {
            sb.Append("else{");
            sb.Append("		field.value = 'n';");
            sb.Append("}");
            sb.Append("		document.forms[0].submit();");
        }
        sb.Append("}");

        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterHiddenField("WindowConfirm", string.Empty);
                page.ClientScript.RegisterStartupScript(page.GetType(), "hiddenFielD", sb.ToString(), true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterHiddenField(page, "WindowConfirm", string.Empty);
                ScriptManager.RegisterStartupScript(page, page.GetType(), "hiddenFielD", sb.ToString(), true);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 用分隔符號(tag)來組合字串
    /// </summary>
    /// <param name="IdList">要組合的字串陣列</param>
    /// <param name="tag">分隔符號</param>
    /// <returns></returns>
    public static string CombineString(string[] IdList, string tag)
    {
        string ret = "";
        for (int i = 0; i <= IdList.GetUpperBound(0); i++)
        {
            if (i == IdList.GetUpperBound(0))
            {
                ret += IdList[i];
            }
            else
            {
                ret += IdList[i] + tag;
            }
        }
        return ret;
    }
    /// <summary>
    /// 用分隔符號(tag)來組合字串
    /// </summary>
    /// <param name="IdList"></param>
    /// <param name="tag">分隔符號</param>
    /// <returns></returns>
    public static string CombineString(List<string> IdList, string tag)
    {
        if (IdList.Count > 0)
        {
            return Stool.CombineString(IdList.ToArray(), tag);
        }
        else
        {
            return string.Empty;
        }

    }


    /// <summary>
    /// 去空白
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string Ctrim(object value)
    {
        string ret = value.ToString().Trim();
        return ret;
    }

    /// <summary>
    /// 遞迴尋找指定的控制項
    /// </summary>
    /// <param name="id">控制項的ID</param>
    /// <param name="parentControl"></param>
    /// <param name="type">控制項的形別</param>
    /// <returns></returns>
    public static Control RecurionFindControl(string id, Control parentControl, Type type)
    {
        Control retCon = null;

        retCon = parentControl.FindControl(id);

        if (retCon == null)
        {
            foreach (Control con in parentControl.Controls)
            {
                retCon = Stool.RecurionFindControl(id, con, type);
                if (retCon != null)
                {
                    if (retCon.GetType().ToString() == type.ToString())
                    {
                        break;
                    }
                }

            }
        }

        return retCon;
    }

    /// <summary>
    /// Base64的編碼
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Base64Encode(string str)
    {
        byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
        return Convert.ToBase64String(encbuff);
    }

    /// <summary>
    /// Base64的解碼
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Base64Decode(string str)
    {
        byte[] decbuff = Convert.FromBase64String(str);
        return System.Text.Encoding.UTF8.GetString(decbuff);
    }

    /// <summary>
    /// 取得顯示執行中的效果的javascript
    /// </summary>
    /// <param name="btn"></param>
    /// <returns></returns>
    public static void GetButtonClickString(Button btn)
    {
        string js;
        btn.UseSubmitBehavior = false;

        js = "this.disabled = true;this.value = '執行中...';";

        btn.Attributes["onclick"] = js;
    }

}

/// <summary>
/// 頁面的處理方法，是PostBack或AJAX
/// </summary>
public enum PageType
{
    /// <summary>
    /// postback
    /// </summary>
    PostBack,
    /// <summary>
    /// Ajax
    /// </summary>
    AJAX
}

/// <summary>
/// SimpleModal的屬性
/// </summary>
public class SimpleModalOptions
{
    /// <summary>
    /// 
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Unit Top { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Unit Left { get; set; }
    /// <summary>
    /// 寬
    /// </summary>
    public string Width { get; set; }
    /// <summary>
    /// 高
    /// </summary>
    public string Height { get; set; }

    public SimpleModalOptions()
    {
        this.Top = Unit.Pixel(10);
        this.Left = Unit.Percentage(25);
    }
}
