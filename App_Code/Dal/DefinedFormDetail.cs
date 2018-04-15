using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


/// <summary>
/// 自定表單明細
/// </summary>
public class DefinedFormDetailManager : DALBaseClass
{
    /// <summary>
    /// 取得所有資料
    /// </summary>
    /// <returns></returns>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
    public virtual List<DefinedFormDetailData> GetAllData()
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        DefinedFormDetailData data;
        List<DefinedFormDetailData> datas = new List<DefinedFormDetailData>();
        sqlStr = "Select * From DefinedFormDetail";
        cmd = new SqlCommand(sqlStr, this.mCon);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetDefindedFormDetail(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    /// <summary>
    /// 由MasterPK取得資料
    /// </summary>
    /// <param name="masterPK"></param>
    /// <returns></returns>
    public List<DefinedFormDetailData> GetDataByMasterPK(string masterPK)
    {
        var datas = this.GetDataBykey("MasterPK", masterPK);
        return datas;
    }

    private List<DefinedFormDetailData> GetDataBykey(string key, string value)
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        DefinedFormDetailData data;
        List<DefinedFormDetailData> datas = new List<DefinedFormDetailData>();
        sqlStr = string.Format("Select * From DefinedFormDetail Where {0} = @{0} Order By Sort", key);
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@" + key, value);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetDefindedFormDetail(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    private DefinedFormDetailData GetDefindedFormDetail(SqlDataReader dr)
    {
        DefinedFormDetailData data = new DefinedFormDetailData();
        data.PK = ((int)(dr["PK"]));
        data.MasterPK = ((int)(dr["MasterPK"]));
        data.Title = dr["Title"].ToString();
        data.AnswerType = dr["AnswerType"].ToString();
        data.AnswerDefine = dr["AnswerDefine"].ToString();
        data.Required = dr["Required"].ToString() == "Y";
        data.Sort = ((int)(dr["Sort"]));
        data.CreateDate = ((DateTime)(dr["CreateDate"]));
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="data"></param>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Insert)]
    public string Insert(DefinedFormDetailData data)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = "Insert Into DefinedFormDetail(MasterPK,Title,AnswerType,AnswerDefine,Required" +
        ",Sort,CreateDate) Values(@MasterPK,@Title,@AnswerType,@AnswerDefine,@Require" +
        "d,@Sort,@CreateDate) SET @QPk = SCOPE_IDENTITY()";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@MasterPK", data.MasterPK);
        cmd.Parameters.AddWithValue("@Title", data.Title);
        cmd.Parameters.AddWithValue("@AnswerType", data.AnswerType);
        cmd.Parameters.AddWithValue("@AnswerDefine", data.AnswerDefine);
        cmd.Parameters.AddWithValue("@Required", data.Required ? "Y" : "N");
        cmd.Parameters.AddWithValue("@Sort", data.Sort);
        cmd.Parameters.AddWithValue("@CreateDate", data.CreateDate);

        var para = new SqlParameter("@QPk", SqlDbType.Int);
        para.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(para);

        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();

        return para.Value.ToString();
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="data"></param>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Update)]
    public virtual void Update(DefinedFormDetailData data)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = "Update DefinedFormDetail Set MasterPK= @MasterPK,Title= @Title,AnswerType= @Answ" +
        "erType,AnswerDefine= @AnswerDefine,Required= @Required,Sort= @Sort,CreateDate= @" +
        "CreateDate Where PK =@PK";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@MasterPK", data.MasterPK);
        cmd.Parameters.AddWithValue("@Title", data.Title);
        cmd.Parameters.AddWithValue("@AnswerType", data.AnswerType);
        cmd.Parameters.AddWithValue("@AnswerDefine", data.AnswerDefine);
        cmd.Parameters.AddWithValue("@Required", data.Required ? "Y" : "N");
        cmd.Parameters.AddWithValue("@Sort", data.Sort);
        cmd.Parameters.AddWithValue("@CreateDate", data.CreateDate);
        cmd.Parameters.AddWithValue("@PK", data.PK);
        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();
    }

    /// <summary>
    /// 批次更新排序
    /// </summary>
    /// <param name="datas"></param>
    public void UpdateSortBatch(List<DefinedFormDetailData> datas)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        SqlCommand cmd;

        foreach (var data in datas)
        {
            sb.AppendFormat("Update DefinedFormDetail Set sort= '{0}' Where PK ='{1}';", data.Sort, data.PK);
        }

        using (cmd = new SqlCommand(sb.ToString(), this.mCon))
        {
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

        sqlStr = "Delete From DefinedFormDetail Where PK =@PK";

        using (cmd = new SqlCommand(sqlStr, this.mCon))
        {
            cmd.Parameters.AddWithValue("@PK", pk);
            this.mCon.Open();
            cmd.ExecuteNonQuery();
            this.mCon.Close();
        }

    }

}

/// <summary>
/// 自定表單明細資料
/// </summary>
public class DefinedFormDetailData
{
    #region property

    private int mPK;

    private int mMasterPK;

    private string mTitle;

    private string mAnswerType;

    private string mAnswerDefine;

    private bool mRequired;

    private int mSort;

    private System.DateTime mCreateDate;

    /// <summary>
    /// 主索引
    /// </summary>
    public virtual int PK
    {
        get { return this.mPK; }
        set { this.mPK = value; }
    }

    /// <summary>
    /// 主檔PK(DefindedForm)
    /// </summary>
    public virtual int MasterPK
    {
        get { return this.mMasterPK; }
        set { this.mMasterPK = value; }
    }

    /// <summary>
    /// 標題
    /// </summary>
    public virtual string Title
    {
        get { return this.mTitle; }
        set { this.mTitle = value; }
    }

    /// <summary>
    /// 類型
    /// </summary>
    public virtual string AnswerType
    {
        get { return this.mAnswerType; }
        set { this.mAnswerType = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public virtual string AnswerDefine
    {
        get { return this.mAnswerDefine; }
        set { this.mAnswerDefine = value; }
    }

    /// <summary>
    /// 是否必填
    /// </summary>
    public virtual bool Required
    {
        get { return this.mRequired; }
        set { this.mRequired = value; }
    }

    /// <summary>
    /// 排序
    /// </summary>
    public virtual int Sort
    {
        get { return this.mSort; }
        set { this.mSort = value; }
    }

    /// <summary>
    /// 建立日期
    /// </summary>
    public virtual DateTime CreateDate
    {
        get { return this.mCreateDate; }
        set { this.mCreateDate = value; }
    }

    /// <summary>
    /// 為了相容問卷明細資料，群組序號
    /// </summary>
    public int GroupId { get; set; }
    /// <summary>
    /// 為了相容問卷明細資料
    /// </summary>
    public string NextAnswer { get; set; }
    #endregion property

    /// <summary>
    /// 建構式
    /// </summary>
    public DefinedFormDetailData()
    {
        this.GroupId = 0;
        this.NextAnswer = string.Empty;
    }

}

