using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// DefindedFormService 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
[System.Web.Script.Services.ScriptService]
public class DefindedFormService : System.Web.Services.WebService
{

    public DefindedFormService()
    {

        //如果使用設計的元件，請取消註解下行程式碼 
        //InitializeComponent(); 
    }

    [WebMethod]
    public List<DefinedFormDetailData> GetDetails(string masterPK)
    {
        DefinedFormDetailManager ddm = new DefinedFormDetailManager();
        var datas = ddm.GetDataByMasterPK(masterPK);
        return datas;
    }

    [WebMethod(EnableSession = true)]
    public string SaveMaster(string pk, string userPk, string title)
    {
        DefinedFormManager dfm = new DefinedFormManager();
        int iPk, iUserPk;

        int.TryParse(pk, out iPk);
        int.TryParse(userPk, out iUserPk);

        var data = new DefinedFormData
        {
            PK = iPk,
            CompanyId = 0,
            CreateDate = DateTime.Now,
            UserModulePK = iUserPk,
            Title = title
        };

        if (pk == "0")
        {
            pk = dfm.Insert(data);
        }
        else
        {
            dfm.Update(data);
        }

        return pk;
    }

    [WebMethod]
    public string SaveDetail(DefinedFormDetailData data)
    {
        string pk = data.PK.ToString();
        DefinedFormDetailManager ddm = new DefinedFormDetailManager();
        //data.Sort = 0;
        data.CreateDate = DateTime.Now;

        if (data.PK == 0)
        {
            pk = ddm.Insert(data);
        }
        else
        {
            ddm.Update(data);
        }

        return pk;
    }

    [WebMethod]
    public string UpdateSort(string value)
    {
        DefinedFormDetailManager ddm = new DefinedFormDetailManager();
        List<DefinedFormDetailData> datas = new List<DefinedFormDetailData>();
        DefinedFormDetailData data;
        string[] values = value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string val in values)
        {
            data = new DefinedFormDetailData();
            var str = val.Replace("Id", "PK");

            data = Element.ToElement(data, str);
            datas.Add(data);
        }

        ddm.UpdateSortBatch(datas);
        return "Y";
    }

    [WebMethod]
    public string DeleteData(string detailPK)
    {
        DefinedFormDetailManager ddm = new DefinedFormDetailManager();
        ddm.Delete(detailPK);

        return "Y";
    }

    [WebMethod]
    public string SaveDataUI(string value)
    {
        DefinedFormAnswerManager dfm = new DefinedFormAnswerManager();
        List<DefinedFormAnswerData> datas = new List<DefinedFormAnswerData>();
        DefinedFormAnswerData data;
        string[] values = value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string val in values)
        {
            data = new DefinedFormAnswerData();
            data = Element.ToElement(data, val);
            data.CreateDate = DateTime.Now;
            if (data.AnswerValue == null)
            {
                data.AnswerValue = string.Empty;
            }
            datas.Add(data);
        }

        foreach (var item in datas)
        {
            dfm.Insert(item);
        }
        return "Y";
    }

    [WebMethod]
    public List<CodeDefData> GetElementTypeList()
    {
        CodeDefManager cm = new CodeDefManager();
        var cds = cm.GetDataByCodeType("ControlElementType");
        return cds;
    }

}
