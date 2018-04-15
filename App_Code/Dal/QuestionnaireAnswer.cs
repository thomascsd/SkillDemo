using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

/// <summary>
/// 問題答案
/// </summary>
public class QuestionnaireAnswerManager : DALBaseClass
{
    /// <summary>
    /// 取得所有資料
    /// </summary>
    /// <returns></returns>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
    public virtual List<QuestionnaireAnswerData> GetAllData()
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        QuestionnaireAnswerData data;
        List<QuestionnaireAnswerData> datas = new List<QuestionnaireAnswerData>();

        sqlStr = "Select * From QuestionnaireAnswer";
        cmd = new SqlCommand(sqlStr, this.mCon);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetQuestionnaireAnswer(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    /// <summary>
    ///  依QAMId(答案主檔PK)，取得資料
    /// </summary>
    /// <param name="qamId"></param>
    /// <returns></returns>
    public List<QuestionnaireAnswerData> GetDataByQamId(string qamId)
    {
        return this.GetDataBykey("QAMId", qamId);
    }

    /// <summary>
    /// 依問卷答案主檔，取得資料
    /// </summary>
    /// <param name="masters"></param>
    /// <returns></returns>
    public List<QuestionnaireAnswerData> GetDataByMaster(List<QuestionnaireAnswerMData> masters)
    {
        SqlCommand cmd;
        SqlDataReader dr;
        List<QuestionnaireAnswerData> datas = new List<QuestionnaireAnswerData>();
        string sqlStr = "Select * From QuestionnaireAnswer Where QAMId In (";

        using (cmd = new SqlCommand(sqlStr, this.mCon))
        {
            for (int i = 0; i < masters.Count; i++)
            {
                sqlStr += string.Format("@p{0},", i);
                cmd.Parameters.AddWithValue("@p" + i, masters[i].Id);
            }
            sqlStr = sqlStr.Substring(0, sqlStr.Length - 1);
            sqlStr += ")";
            cmd.CommandText = sqlStr;

            this.mCon.Open();
            using (dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var data = this.GetQuestionnaireAnswer(dr);
                    datas.Add(data);
                }
            }
            this.mCon.Close();
        }

        return datas;
    }


    private List<QuestionnaireAnswerData> GetDataBykey(string key, string value)
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        QuestionnaireAnswerData data;
        List<QuestionnaireAnswerData> datas = new List<QuestionnaireAnswerData>();

        sqlStr = string.Format("Select * From QuestionnaireAnswer Where {0} = @{0}", key);
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@" + key, value);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetQuestionnaireAnswer(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }


    private QuestionnaireAnswerData GetQuestionnaireAnswer(SqlDataReader dr)
    {
        QuestionnaireAnswerData data = new QuestionnaireAnswerData();
        data.Id = ((int)(dr["id"]));
        data.DetailId = ((int)(dr["detailId"]));
        data.QAMId = (int)dr["QAMId"];
        data.AnswerValue = dr["answerValue"].ToString();
        data.AnswerText = dr["answerText"].ToString();
        data.CreateDate = ((System.DateTime)(dr["createDate"]));
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="data"></param>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Insert)]
    public string Insert(QuestionnaireAnswerData data)
    {
        string sqlStr, answerPK;
        SqlCommand cmd;
        SqlParameter paramAnswer;
        sqlStr = "Insert Into QuestionnaireAnswer(detailId,QAMId,answerValue,answerText,createDa" +
"te) Values(@detailId,@QAMId,@answerValue,@answerText,@createDate) SET @AnswerPK = SCOPE_IDENTITY()";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@detailId", data.DetailId);
        cmd.Parameters.AddWithValue("@QAMId", data.QAMId);
        cmd.Parameters.AddWithValue("@answerValue", data.AnswerValue);
        cmd.Parameters.AddWithValue("@answerText", data.AnswerText);
        cmd.Parameters.AddWithValue("@createDate", data.CreateDate);

        paramAnswer = new SqlParameter("@AnswerPK", System.Data.SqlDbType.Int)
        {
            Direction = System.Data.ParameterDirection.Output
        };
        cmd.Parameters.Add(paramAnswer);

        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();

        answerPK = paramAnswer.Value.ToString();
        return answerPK;
    }

    /// <summary>
    /// 批次新增
    /// </summary>
    /// <param name="datas"></param>
    public void InsertGroups(List<QuestionnaireAnswerData> datas)
    {
        StringBuilder sb = new StringBuilder();
        SqlCommand cmd;

        foreach (var data in datas)
        {
            sb.AppendFormat("Insert Into QuestionnaireAnswer(detailId,QAMId,answerValue,answerText,createDate) Values('{0}' ,'{1}' ,'{2}' ,'{3}' ,'{4}');", data.DetailId, data.QAMId, data.AnswerValue, data.AnswerText, data.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"));
        }

        using (cmd = new SqlCommand(sb.ToString(), this.mCon))
        {
            this.mCon.Open();
            cmd.ExecuteNonQuery();
            this.mCon.Close();
        }

    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="data"></param>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Update)]
    public virtual void Update(QuestionnaireAnswerData data)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = "Update QuestionnaireAnswer Set detailId= @detailId,QAMId= @QAMId,answerValue= @an" +
        "swerValue,answerText= @answerText,createDate= @createDate Where id =@id";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@detailId", data.DetailId);
        cmd.Parameters.AddWithValue("@QAMId", data.QAMId);
        cmd.Parameters.AddWithValue("@answerValue", data.AnswerValue);
        cmd.Parameters.AddWithValue("@answerText", data.AnswerText);
        cmd.Parameters.AddWithValue("@createDate", data.CreateDate);
        cmd.Parameters.AddWithValue("@id", data.Id);
        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();
    }

    /// <summary>
    /// 以DetailId刪除資料
    /// </summary>
    /// <param name="detailId"></param>
    public void DeleteByDetailId(string detailId)
    {
        string sqlStr;
        SqlCommand cmd;

        sqlStr = "Delete From QuestionnaireAnswer Where detailId= @detailId";

        using (cmd = new SqlCommand(sqlStr, this.mCon))
        {
            cmd.Parameters.AddWithValue("@detailId", detailId);

            this.mCon.Open();
            cmd.ExecuteNonQuery();
            this.mCon.Close();
        }

    }
}

/// <summary>
/// 問題答案資料
/// </summary>
public class QuestionnaireAnswerData
{
    #region"Property"
    private int mid;

    private int mdetailId;

    private string manswerValue;

    private string manswerText;

    private System.DateTime mdate;
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
    /// 問卷明細PK
    /// </summary>
    public virtual int DetailId
    {
        get
        {
            return this.mdetailId;
        }
        set
        {
            this.mdetailId = value;
        }
    }

    /// <summary>
    /// 輸入答案值
    /// </summary>
    public virtual string AnswerValue
    {
        get
        {
            return this.manswerValue;
        }
        set
        {
            this.manswerValue = value;
        }
    }
    /// <summary>
    /// 輔助答案值
    /// </summary>
    public virtual string AnswerText
    {
        get
        {
            return this.manswerText;
        }
        set
        {
            this.manswerText = value;
        }
    }
    /// <summary>
    /// 建立日期
    /// </summary>
    public virtual System.DateTime CreateDate
    {
        get
        {
            return this.mdate;
        }
        set
        {
            this.mdate = value;
        }
    }
    /// <summary>
    /// 問卷答案主檔PK
    /// </summary>
    public int QAMId { get; set; }
    #endregion
    /// <summary>
    /// 建構式
    /// </summary>
    public QuestionnaireAnswerData()
    {
        this.manswerText = string.Empty;
        this.manswerValue = string.Empty;
    }

}

