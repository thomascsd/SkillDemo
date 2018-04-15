using System;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// 問卷明細
/// </summary>
public class QuestionnaireDetailManager : DALBaseClass
{
    /// <summary>
    /// 取得所有資料
    /// </summary>
    /// <returns></returns>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
    public virtual List<QuestionnaireDetailData> GetAllData()
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        QuestionnaireDetailData data;
        List<QuestionnaireDetailData> datas = new List<QuestionnaireDetailData>();

        sqlStr = "Select * From QuestionnaireDetail Order By sort, id";
        cmd = new SqlCommand(sqlStr, this.mCon);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetQuestionnaireDetail(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    /// <summary>
    /// 依MasterPK(主檔PK)，取得資料，並且GroupId為零
    /// </summary>
    /// <param name="masterPK"></param>
    /// <returns></returns>
    public List<QuestionnaireDetailData> GetDataByMasterPK(string masterPK)
    {
        var datas = this.GetDataBykey("masterId", masterPK);
        return datas;//.FindAll(d => d.GroupId == 0);
    }

    private List<QuestionnaireDetailData> GetDataBykey(string key, string value)
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        QuestionnaireDetailData data;
        List<QuestionnaireDetailData> datas = new List<QuestionnaireDetailData>();

        sqlStr = string.Format("Select * From QuestionnaireDetail Where {0} = @{0} Order By sort, id", key);
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@" + key, value);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetQuestionnaireDetail(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }


    private QuestionnaireDetailData GetQuestionnaireDetail(SqlDataReader dr)
    {
        QuestionnaireDetailData data = new QuestionnaireDetailData();
        data.Id = ((int)(dr["id"]));
        data.MasterId = ((int)(dr["masterId"]));
        data.GroupId = ((int)(dr["groupId"]));
        data.Topic = dr["topic"].ToString();
        data.Description = dr["description"].ToString();
        data.AnswerType = dr["answerType"].ToString();
        data.AnswerDefine = dr["answerDefine"].ToString();
        data.NextAnswer = dr["nextAnswer"].ToString();
        data.Needed = dr["needed"].ToString() == "Y";
        data.Sort = ((int)(dr["sort"]));
        data.Type = dr["type"].ToString();
        data.No = dr["no"].Equals(DBNull.Value) ? 0 : (int)dr["no"];

        return data;
    }

    /// <summary>
    /// 新增，並回傳PK
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Insert)]
    public string Insert(QuestionnaireDetailData data)
    {
        string sqlStr;
        SqlCommand cmd;
        SqlParameter para;
        string detailPK;

        sqlStr = "Insert Into QuestionnaireDetail(masterId,groupId,topic,description,answerType," +
        "answerDefine,nextAnswer,needed,sort, type) Values(@masterId,@groupId,@topic,@descr" +
        "iption,@answerType,@answerDefine,@nextAnswer,@needed,@sort, @type) SET @detailPK = SCOPE_IDENTITY()";
        cmd = new SqlCommand(sqlStr, this.mCon);

        cmd.Parameters.AddWithValue("@masterId", data.MasterId);
        cmd.Parameters.AddWithValue("@groupId", data.GroupId);
        cmd.Parameters.AddWithValue("@topic", data.Topic);
        cmd.Parameters.AddWithValue("@description", data.Description ?? "");
        cmd.Parameters.AddWithValue("@answerType", data.AnswerType);
        cmd.Parameters.AddWithValue("@answerDefine", data.AnswerDefine);
        cmd.Parameters.AddWithValue("@nextAnswer", data.NextAnswer);
        cmd.Parameters.AddWithValue("@needed", data.Needed ? "Y" : "N");
        cmd.Parameters.AddWithValue("@sort", data.Sort);
        cmd.Parameters.AddWithValue("@type", data.Type);

        para = new SqlParameter("@detailPK", System.Data.SqlDbType.Int);
        para.Direction = System.Data.ParameterDirection.Output;
        cmd.Parameters.Add(para);

        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();

        detailPK = para.Value.ToString();
        return detailPK;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="data"></param>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Update)]
    public virtual void Update(QuestionnaireDetailData data)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = "Update QuestionnaireDetail Set masterId= @masterId,groupId= @groupId,topic= @topi" +
        "c,description= @description,answerType= @answerType,answerDefine= @answerDefine," +
        "nextAnswer= @nextAnswer,needed= @needed,sort= @sort, type= @type Where id =@id";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@masterId", data.MasterId);
        cmd.Parameters.AddWithValue("@groupId", data.GroupId);
        cmd.Parameters.AddWithValue("@topic", data.Topic);
        cmd.Parameters.AddWithValue("@description", data.Description);
        cmd.Parameters.AddWithValue("@answerType", data.AnswerType);
        cmd.Parameters.AddWithValue("@answerDefine", data.AnswerDefine);
        cmd.Parameters.AddWithValue("@nextAnswer", data.NextAnswer);
        cmd.Parameters.AddWithValue("@needed", data.Needed ? "Y" : "N");
        cmd.Parameters.AddWithValue("@sort", data.Sort);
        cmd.Parameters.AddWithValue("@type", data.Type);
        cmd.Parameters.AddWithValue("@id", data.Id);
        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();
    }

    /// <summary>
    /// 批次更新排序
    /// </summary>
    /// <param name="datas"></param>
    public void UpdateSortBatch(List<QuestionnaireDetailData> datas)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        SqlCommand cmd;

        foreach (var data in datas)
        {
            sb.AppendFormat("Update QuestionnaireDetail Set sort= '{0}' Where id ='{1}';", data.Sort, data.Id);
        }

        using (cmd = new SqlCommand(sb.ToString(), this.mCon))
        {
            this.mCon.Open();
            cmd.ExecuteNonQuery();
            this.mCon.Close();
        }

    }



    /// <summary>
    /// 依GroupId，刪除
    /// </summary>
    /// <param name="groupId"></param>
    public void DeleteByGroupId(string groupId)
    {
        string sqlStr;
        SqlCommand cmd;

        sqlStr = "Delete From QuestionnaireDetail Where groupId= @groupId";

        using (cmd = new SqlCommand(sqlStr, this.mCon))
        {
            cmd.Parameters.AddWithValue("@groupId", groupId);
            this.mCon.Open();
            cmd.ExecuteNonQuery();
            this.mCon.Close();
        }

    }

    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="pk"></param>
    public void Delete(string pk)
    {
        string sqlStr;
        SqlCommand cmd;
        QuestionnaireAnswerManager qam = new QuestionnaireAnswerManager();

        qam.DeleteByDetailId(pk);

        sqlStr = "Delete From QuestionnaireDetail Where id= @id";

        using (cmd = new SqlCommand(sqlStr, this.mCon))
        {
            cmd.Parameters.AddWithValue("@id", pk);

            this.mCon.Open();
            cmd.ExecuteNonQuery();
            this.mCon.Close();
        }

    }

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="sourcePK"></param>
    /// <param name="targetPK"></param>
    public void Sort(string sourcePK, string targetPK)
    {
        SqlCommand cmd;

        using (cmd = new SqlCommand("sp_QuestionnaireSort", this.mCon))
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@sourcePk", sourcePK);
            cmd.Parameters.AddWithValue("@targetPk", targetPK);

            this.mCon.Open();
            cmd.ExecuteNonQuery();
            this.mCon.Close();

        }
    }

}

/// <summary>
/// 問卷明細資料
/// </summary>
public class QuestionnaireDetailData
{
    #region"Property"
    private int mid;

    private int mmasterId;

    private int mgroupId;

    private string mtopic;

    private string mdescription;

    private string manswerType;

    private string manswerDefine;

    private string mnextAnswer;

    private bool mneeded;

    private int msort;
    /// <summary>
    /// 明細PK
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
    /// 主檔PK
    /// </summary>
    public virtual int MasterId
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
    /// 群組
    /// </summary>
    public virtual int GroupId
    {
        get
        {
            return this.mgroupId;
        }
        set
        {
            this.mgroupId = value;
        }
    }
    /// <summary>
    /// 標題
    /// </summary>
    public virtual string Topic
    {
        get
        {
            return this.mtopic;
        }
        set
        {
            this.mtopic = value;
        }
    }
    /// <summary>
    /// 說明
    /// </summary>
    public virtual string Description
    {
        get
        {
            return this.mdescription;
        }
        set
        {
            this.mdescription = value;
        }
    }
    /// <summary>
    /// 類型(ChekckBox、Text、TextArea)
    /// </summary>
    public virtual string AnswerType
    {
        get
        {
            return this.manswerType;
        }
        set
        {
            this.manswerType = value;
        }
    }
    /// <summary>
    /// 問卷的值
    /// </summary>
    public virtual string AnswerDefine
    {
        get
        {
            return this.manswerDefine;
        }
        set
        {
            this.manswerDefine = value;
        }
    }
    /// <summary>
    /// 跳題
    /// </summary>
    public string NextAnswer
    {
        get
        {
            return this.mnextAnswer;
        }
        set
        {
            this.mnextAnswer = value;
        }
    }
    /// <summary>
    /// 是否為必填
    /// </summary>
    public virtual bool Needed
    {
        get
        {
            return this.mneeded;
        }
        set
        {
            this.mneeded = value;
        }
    }
    /// <summary>
    /// 排序
    /// </summary>
    public virtual int Sort
    {
        get
        {
            return this.msort;
        }
        set
        {
            this.msort = value;
        }
    }

    /// <summary>
    /// 'G'群組,'S獨立題
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 題號
    /// </summary>
    public int No { get; set; }
    #endregion
    /// <summary>
    /// 建構式
    /// </summary>
    public QuestionnaireDetailData()
    {
    }

}


