using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// 問卷答案主檔
/// </summary>
public class QuestionnaireAnswerMManager : DALBaseClass
{
    /// <summary>
    /// 取得全部資料
    /// </summary>
    /// <returns></returns>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
    public virtual List<QuestionnaireAnswerMData> GetAllData()
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        QuestionnaireAnswerMData data;
        List<QuestionnaireAnswerMData> datas = new List<QuestionnaireAnswerMData>();

        sqlStr = "Select * From QuestionnaireAnswerM";
        cmd = new SqlCommand(sqlStr, this.mCon);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetQuestionnaireAnswerM(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    /// <summary>
    /// 依UserId及MasterPK，取得資料
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="masterPK"></param>
    /// <returns></returns>
    public QuestionnaireAnswerMData GetDataByUserId(string userId, string masterPK)
    {
        var dic = new Dictionary<string, string>
            {
                {"userId", userId},
                {"masterId", masterPK}
            };

        var datas = this.GetDataBykey(dic);

        if (datas.Count > 0)
        {
            return datas[0];
        }
        return null;

    }

    /// <summary>
    /// 依MasterId(問卷主檔PK)，取得資料
    /// </summary>
    /// <param name="masterPK"></param>
    /// <returns></returns>
    public List<QuestionnaireAnswerMData> GetDataByMasterPK(string masterPK)
    {
        var dic = new Dictionary<string, string>
            {
                {"masterId", masterPK},
                {"status" , "complete"}            
            };

        var datas = this.GetDataBykey(dic);
        return datas;
    }

    /// <summary>
    /// 依CompanyCode，取得資料
    /// </summary>
    /// <param name="companyCode"></param>
    /// <returns></returns>
    public List<QuestionnaireAnswerMData> GetDataByCompanyCode(string companyCode)
    {
        var dic = new Dictionary<string, string>
            {
                {"companyCode", companyCode},
            };

        var datas = this.GetDataBykey(dic);
        return datas;
    }

    private List<QuestionnaireAnswerMData> GetDataBykey(Dictionary<string, string> dic)
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        QuestionnaireAnswerMData data;
        List<QuestionnaireAnswerMData> datas = new List<QuestionnaireAnswerMData>();

        sqlStr = "Select * From QuestionnaireAnswerM Where 1=1";

        using (cmd = new SqlCommand(sqlStr, this.mCon))
        {
            foreach (string key in dic.Keys)
            {
                sqlStr += string.Format(" And {0} =@{0}", key);
                cmd.Parameters.AddWithValue("@" + key, dic[key]);
            }
            cmd.CommandText = sqlStr;

            this.mCon.Open();
            using (dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    data = this.GetQuestionnaireAnswerM(dr);
                    datas.Add(data);
                }
                dr.Close();
            }
            this.mCon.Close();
        }
        return datas;
    }


    private QuestionnaireAnswerMData GetQuestionnaireAnswerM(SqlDataReader dr)
    {
        QuestionnaireAnswerMData data = new QuestionnaireAnswerMData();
        data.Id = ((int)(dr["id"]));
        data.MasterId = dr["masterId"].ToString();
        data.Step = ((int)(dr["step"]));
        data.Status = dr["status"].ToString();
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Insert)]
    public string Insert(QuestionnaireAnswerMData data)
    {
        string sqlStr, answerPK;
        SqlCommand cmd;
        SqlParameter para;

        sqlStr = "Insert Into QuestionnaireAnswerM(masterId,step,st" +
        "atus) Values(@masterId,@step,@st" +
        "atus) SET @AnswerPK = SCOPE_IDENTITY()";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@masterId", data.MasterId);
        cmd.Parameters.AddWithValue("@step", data.Step);
        cmd.Parameters.AddWithValue("@status", data.Status);

        para = new SqlParameter("@AnswerPK", System.Data.SqlDbType.Int);
        para.Direction = System.Data.ParameterDirection.Output;
        cmd.Parameters.Add(para);

        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();

        answerPK = para.Value.ToString();
        return answerPK;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="data"></param>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Update)]
    public virtual void Update(QuestionnaireAnswerMData data)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = "Update QuestionnaireAnswerM Set " +
        "masterId= @masterId,step= @step,status= @status " +
        " Where id =@id";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@masterId", data.MasterId);
        cmd.Parameters.AddWithValue("@step", data.Step);
        cmd.Parameters.AddWithValue("@status", data.Status);
        cmd.Parameters.AddWithValue("@id", data.Id);
        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();
    }

}

/// <summary>
/// 問卷答案主檔資料
/// </summary>
public class QuestionnaireAnswerMData
{
    #region"property"
    private int mid;

    private string mmasterId;

    private int mstep;

    private string mstatus;

    /// <summary>
    /// PK，職別值
    /// </summary>
    public virtual int Id
    {
        get
        {
            return this.mid;
        }
        set
        {
            this.mid = value;
        }
    }
    /// <summary>
    /// 問卷主檔PK
    /// </summary>
    public virtual string MasterId
    {
        get
        {
            return this.mmasterId;
        }
        set
        {
            this.mmasterId = value;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public virtual int Step
    {
        get
        {
            return this.mstep;
        }
        set
        {
            this.mstep = value;
        }
    }
    /// <summary>
    /// 狀態
    /// </summary>
    public virtual string Status
    {
        get
        {
            return this.mstatus;
        }
        set
        {
            this.mstatus = value;
        }
    }

    #endregion
    /// <summary>
    /// 建構式
    /// </summary>
    public QuestionnaireAnswerMData()
    {
    }

}


