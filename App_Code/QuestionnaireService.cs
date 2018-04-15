using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// QuestionnaireService 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
[System.Web.Script.Services.ScriptService]
public class QuestionnaireService : System.Web.Services.WebService
{
    public QuestionnaireService()
    {
        //如果使用設計的元件，請取消註解下行程式碼
        //InitializeComponent();
    }

    [WebMethod]
    public List<CodeDefData> GetElementTypeList()
    {
        CodeDefManager cd = new CodeDefManager();
        List<CodeDefData> cds;

        cds = cd.GetDataByCodeType("ElementType");

        return cds;
    }

    [WebMethod]
    public List<QuestionnaireDetailData> GetDetails(string masterPK)
    {
        QuestionnaireDetailManager qd = new QuestionnaireDetailManager();
        var datas = qd.GetDataByMasterPK(masterPK);
        return datas;
    }

    [WebMethod(EnableSession = true)]
    public string SaveMaster(string qPK, string heading, string desc, string category, string status, string startDate,
                       string endDate, string userModulePK, string comment, bool oneTime)
    {
        string companyCode = string.Empty;
        QuestionnaireMasterManager qmm = new QuestionnaireMasterManager();
        DateTime sDate, eDate;
        int pk, uPK;

        int.TryParse(userModulePK, out uPK);
        DateTime.TryParse(startDate, out sDate);
        DateTime.TryParse(endDate, out eDate);

        if (qPK == "0")
        {
            var createDate = DateTime.Now;

            qPK = qmm.Insert(new QuestionnaireMasterData
                 {
                     Description = desc,
                     Heading = heading,
                     StartDate = sDate,
                     EndDate = eDate,
                     CreateDate = createDate,
                     Category = category,
                     Status = string.Empty,
                     Comment = comment,
                     OneTime = oneTime
                 });

        }
        else
        {
            int.TryParse(qPK, out pk);
            var updateTime = DateTime.Now;

            qmm.Update(new QuestionnaireMasterData
            {
                Id = pk,
                Description = desc,
                Heading = heading,
                StartDate = sDate,
                EndDate = eDate,
                UpdateTime = updateTime,
                Category = category,
                Status = string.Empty,
                Comment = comment,
                OneTime = oneTime
            });
        }

        return qPK;
    }

    [WebMethod]
    public string SaveDetail(string detailPK, string masterPK, string type, string groupId, string topic, string desc,
                             string answerType, string answerDef, string nextAnswer, string needed, string sort)
    {
        QuestionnaireDetailManager qdm = new QuestionnaireDetailManager();
        int dPK, mPK, gId, s;

        int.TryParse(detailPK, out dPK);
        int.TryParse(masterPK, out mPK);
        int.TryParse(groupId, out gId);
        int.TryParse(sort, out s);

        if (detailPK == "0")
        {
            detailPK = qdm.Insert(new QuestionnaireDetailData
            {
                MasterId = mPK,
                Type = type,
                GroupId = gId,
                Topic = topic,
                Description = desc,
                AnswerType = answerType,
                AnswerDefine = answerDef,
                NextAnswer = nextAnswer,
                Needed = needed == "Y",
                Sort = s
            });

        }
        else
        {
            qdm.Update(new QuestionnaireDetailData
            {
                Id = dPK,
                MasterId = mPK,
                Type = type,
                GroupId = gId,
                Topic = topic,
                Description = desc,
                AnswerType = answerType,
                AnswerDefine = answerDef,
                NextAnswer = nextAnswer,
                Needed = needed == "Y",
                Sort = s
            });

        }

        return detailPK;
    }

    [WebMethod]
    public string SaveDetailGroup(string value)
    {
        QuestionnaireDetailManager qdm = new QuestionnaireDetailManager();
        List<QuestionnaireDetailData> qds = new List<QuestionnaireDetailData>();
        string[] values = value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var val in values)
        {
            var qd = new QuestionnaireDetailData();
            qd = Element.ToElement(qd, val);

            qds.Add(qd);
        }

        if (qds.Count > 0)
        {
            //先刪除
            qdm.DeleteByGroupId(qds[0].GroupId.ToString());
            //新增
            foreach (var qd in qds)
            {
                qdm.Insert(qd);
            }
        }

        return "Y";
    }

    [WebMethod(EnableSession = true)]
    public string SaveAnswerMaster(string answerMasterPK, string masterPK, string step, string status, string desc)
    {
        QuestionnaireAnswerMManager qmm = new QuestionnaireAnswerMManager();
        string companyCode = string.Empty;
        int amPK, mPK, iStep;

        int.TryParse(answerMasterPK, out amPK);
        int.TryParse(masterPK, out mPK);
        int.TryParse(step, out iStep);

        if (answerMasterPK == "0")
        {
            answerMasterPK = qmm.Insert(new QuestionnaireAnswerMData
            {
                MasterId = mPK.ToString(),
                Status = status,
                Step = iStep,
            });
        }
        else
        {
            qmm.Update(new QuestionnaireAnswerMData
            {
                Id = amPK,
                MasterId = mPK.ToString(),
                Status = status,
                Step = iStep,
            });
        }

        return answerMasterPK;
    }

    [WebMethod(EnableSession = true)]
    public string SaveAnswer(string answerPK, string detailPK, string answerMasterPK, string answerValue, string answerText)
    {
        QuestionnaireAnswerManager qam = new QuestionnaireAnswerManager();
        int aPK, dPK, amPK;

        int.TryParse(answerPK, out aPK);
        int.TryParse(detailPK, out dPK);
        int.TryParse(answerMasterPK, out amPK);

        if (answerPK == "0")
        {
            answerPK = qam.Insert(new QuestionnaireAnswerData
            {
                AnswerValue = answerValue,
                AnswerText = answerText,
                DetailId = dPK,
                CreateDate = DateTime.Now,
                QAMId = amPK
            });

        }
        else
        {
            qam.Update(new QuestionnaireAnswerData
            {
                Id = aPK,
                AnswerValue = answerValue,
                AnswerText = answerText,
                DetailId = dPK,
                CreateDate = DateTime.Now,
                QAMId = amPK
            });
        }

        return answerPK;
    }

    [WebMethod(EnableSession = true)]
    public string SaveAnswerGroup(string value)
    {
        List<QuestionnaireAnswerData> qads = new List<QuestionnaireAnswerData>();
        QuestionnaireAnswerManager qam = new QuestionnaireAnswerManager();
        QuestionnaireAnswerData qad;
        string[] values = value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var val in values)
        {
            qad = new QuestionnaireAnswerData();
            qad = Element.ToElement(qad, val);
            //設定其餘屬性
            qad.CreateDate = DateTime.Now;
            if (qad.AnswerValue.IndexOf("$&") != -1)
            {
                qad.AnswerValue = qad.AnswerValue.Replace("$&", ",");
            }

            if (qad.AnswerText.IndexOf("$&") != -1)
            {
                qad.AnswerText = qad.AnswerText.Replace("$&", ",");
            }
            if (qad.AnswerText.IndexOf("$#") != -1)
            {
                qad.AnswerText = qad.AnswerText.Replace("$#", ":");
            }

            qads.Add(qad);
        }
        qam.InsertGroups(qads);

        return "Y";
    }

    [WebMethod]
    public string UpdateSort(string value)
    {
        QuestionnaireDetailManager qm = new QuestionnaireDetailManager();
        List<QuestionnaireDetailData> qds = new List<QuestionnaireDetailData>();
        QuestionnaireDetailData qd;
        string[] values = value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var val in values)
        {
            qd = new QuestionnaireDetailData();
            qd = Element.ToElement(qd, val);
            qds.Add(qd);

        }

        qm.UpdateSortBatch(qds);

        return "Y";
    }

    [WebMethod]
    public string DeleteData(string detailPK)
    {
        QuestionnaireDetailManager qdm = new QuestionnaireDetailManager();
        qdm.Delete(detailPK);

        return "Y";
    }

    [WebMethod]
    public void SortItem(string sourcePK, string targetPK)
    {
        QuestionnaireDetailManager qdm = new QuestionnaireDetailManager();
        qdm.Sort(sourcePK, targetPK);

    }

    /*[WebMethod(EnableSession = true)]
    public List<UserDefData> GetImageType()
    {
        string companyCode = WebUtility.GetCompanyCode(new System.Web.UI.StateBag());
        UserDefManager um = new UserDefManager();

        var uds = um.GetDataByCompanyCode(companyCode, UserDefData.ImageTypeKey);

        return uds;
    }

    [WebMethod(EnableSession = true)]
    public List<ImageManageData> GetImages(string category)
    {
        ImageManager im = new ImageManager();
        string companyCode = WebUtility.GetCompanyCodeFront(new System.Web.UI.StateBag());
        var ims = im.GetDataByCompanyCode(companyCode, category);

        string path = System.Web.VirtualPathUtility.ToAbsolute(WebUtility.CompanyAnimationImagePath);
        string thumbPath = System.Web.VirtualPathUtility.ToAbsolute(WebUtility.CompanyAnimationThumbPath);

        ims.ForEach(image => image.Path = path);
        ims.ForEach(image => image.ThumbPath = thumbPath);

        return ims;

    }*/
}
