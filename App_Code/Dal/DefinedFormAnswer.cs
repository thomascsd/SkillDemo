using System;
using System.Collections.Generic;
using System.Data.SqlClient;


/// <summary>
///
/// </summary>
public class DefinedFormAnswerManager : DALBaseClass
{
    /// <summary>
    /// 取得所有資料
    /// </summary>
    /// <returns></returns>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
    public virtual List<DefinedFormAnswerData> GetAllData()
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        DefinedFormAnswerData data;
        List<DefinedFormAnswerData> datas = new List<DefinedFormAnswerData>();
        sqlStr = "Select * From DefinedFormAnswer";
        cmd = new SqlCommand(sqlStr, this.mCon);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetDefinedFormAnswer(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    /// <summary>
    /// 以DefinedFromDetail(明細檔)的PK，取得屬於這個主檔的答案
    /// </summary>
    /// <param name="definedFormPK"></param>
    /// <returns></returns>
    public List<DefinedFormAnswerData> GetDataByDetailPK(string definedFormPK)
    {
        string sqlStr;
        List<DefinedFormAnswerData> datas = new List<DefinedFormAnswerData>();

        sqlStr = "Select * From DefinedFormAnswer Where DetailPK In (Select PK From DefinedFormDetail Where MasterPK =@MasterPK)";

        using (SqlCommand cmd = new SqlCommand(sqlStr, this.mCon))
        {
            cmd.Parameters.AddWithValue("@MasterPK", definedFormPK);
            this.mCon.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var data = this.GetDefinedFormAnswer(dr);
                    datas.Add(data);
                }
                dr.Close();
            }
            this.mCon.Close();
        }
        return datas;
    }

    private List<DefinedFormAnswerData> GetDataBykey(string key, string value)
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        DefinedFormAnswerData data;
        List<DefinedFormAnswerData> datas = new List<DefinedFormAnswerData>();
        sqlStr = string.Format("Select * From DefinedFormAnswer Where {0} = @{0}", key);
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@" + key, value);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetDefinedFormAnswer(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    private DefinedFormAnswerData GetDefinedFormAnswer(SqlDataReader dr)
    {
        DefinedFormAnswerData data = new DefinedFormAnswerData();
        data.PK = ((int)(dr["PK"]));
        data.DetailPK = ((int)(dr["DetailPK"]));
        data.AnswerValue = dr["AnswerValue"].ToString();
        data.AnswerText = dr["AnswerText"].ToString();
        data.CreateDate = ((System.DateTime)(dr["CreateDate"]));
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="data"></param>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Insert)]
    public virtual void Insert(DefinedFormAnswerData data)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = "Insert Into DefinedFormAnswer(DetailPK,AnswerValue,AnswerText,CreateDate) Valu" +
        "es(@DetailPK,@AnswerValue,@AnswerText,@CreateDate)";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@DetailPK", data.DetailPK);
        cmd.Parameters.AddWithValue("@AnswerValue", data.AnswerValue);
        cmd.Parameters.AddWithValue("@AnswerText", data.AnswerText);
        cmd.Parameters.AddWithValue("@CreateDate", data.CreateDate);
        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();
    }

}

/// <summary>
/// 自定表單答案的資料
/// </summary>
public class DefinedFormAnswerData
{
    #region property

    private int mPK;

    private int mDetailPK;

    private string mAnswerValue;

    private string mAnswerText;

    private System.DateTime mCreateDate;

    /// <summary>
    /// 主索引值
    /// </summary>
    public virtual int PK
    {
        get { return this.mPK; }
        set { this.mPK = value; }
    }

    /// <summary>
    /// DefinedFormDetail的PK
    /// </summary>
    public virtual int DetailPK
    {
        get { return this.mDetailPK; }
        set { this.mDetailPK = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public virtual string AnswerValue
    {
        get
        {
            return this.mAnswerValue;
        }
        set
        {
            this.mAnswerValue = value;
        }
    }

    ///
    public virtual string AnswerText
    {
        get
        {
            return this.mAnswerText;
        }
        set
        {
            this.mAnswerText = value;
        }
    }

    /// <summary>
    /// 建立日期
    /// </summary>
    public virtual DateTime CreateDate
    {
        get
        {
            return this.mCreateDate;
        }
        set
        {
            this.mCreateDate = value;
        }
    }

    #endregion property

    /// <summary>
    /// 建構式
    /// </summary>
    public DefinedFormAnswerData()
    {
        this.AnswerText = string.Empty;
    }

}

