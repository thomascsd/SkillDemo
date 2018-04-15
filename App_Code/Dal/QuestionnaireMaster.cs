using System;
using System.Collections.Generic;
using System.Data.SqlClient;


/// <summary>
/// 問卷主檔
/// </summary>
public class QuestionnaireMasterManager : DALBaseClass
{
    /// <summary>
    /// 取得所有資料
    /// </summary>
    /// <returns></returns>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
    public virtual List<QuestionnaireMasterData> GetAllData()
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        QuestionnaireMasterData data;
        List<QuestionnaireMasterData> datas = new List<QuestionnaireMasterData>();

        sqlStr = "Select * From QuestionnaireMaster";
        cmd = new SqlCommand(sqlStr, this.mCon);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetQuestionnaireMaster(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    /// <summary>
    /// 依UserModulePK(使用者模組PK)，取得資料
    /// </summary>
    /// <param name="userPK"></param>
    /// <returns></returns>
    public QuestionnaireMasterData GetDataByUserModulePK(string userPK)
    {
        var datas = this.GetDataBykey("userModulePk", userPK);

        if (datas.Count > 0)
        {
            return datas[0];
        }

        return null;
    }

    /// <summary>
    /// 以Id(pk)，取得資料
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public QuestionnaireMasterData GetDataById(string id)
    {
        var datas = this.GetDataBykey("id", id);

        if (datas.Count > 0)
        {
            return datas[0];
        }

        return null;
    }

    /// <summary>
    /// 取得PK
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="createDate"></param>
    /// <returns></returns>
    public string GetPKByCreateDate(string companyId, DateTime createDate)
    {
        var datas = this.GetDataBykey("CompanyID", companyId);

        var data = datas.Find(d => d.CreateDate.ToString("yyyyMMddHHmmss") == createDate.ToString("yyyyMMddHHmmss"));

        return data.Id.ToString();
    }

    private List<QuestionnaireMasterData> GetDataBykey(string key, string value)
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        QuestionnaireMasterData data;
        List<QuestionnaireMasterData> datas = new List<QuestionnaireMasterData>();

        sqlStr = string.Format("Select * From QuestionnaireMaster Where {0} = @{0}", key);
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@" + key, value);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetQuestionnaireMaster(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    private QuestionnaireMasterData GetQuestionnaireMaster(SqlDataReader dr)
    {
        QuestionnaireMasterData data = new QuestionnaireMasterData();
        data.Id = ((int)(dr["id"]));
        data.Category = dr["category"].ToString();
        data.Heading = dr["heading"].ToString();
        data.Description = dr["description"].ToString();
        data.CreateDate = ((System.DateTime)(dr["createDate"]));
        data.UpdateTime = dr["updateTime"].Equals(DBNull.Value) ? DateTime.MaxValue : (DateTime)dr["updateTime"];
        data.StartDate = ((System.DateTime)(dr["startDate"]));
        data.EndDate = ((System.DateTime)(dr["endDate"]));
        data.Status = dr["status"].ToString();
        data.Comment = dr["comment"].ToString();
        data.OneTime = dr["OneTime"].ToString() == "Y";
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="data"></param>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Insert)]
    public string Insert(QuestionnaireMasterData data)
    {
        string sqlStr, qPk;
        SqlCommand cmd;
        sqlStr = @"Insert Into QuestionnaireMaster(category,heading,description,createDate,startDate,endDate,status,  comment,OneTime) Values(@category,@heading,@description,@createDate,@startDate,@endDate,@status, @comment, @OneTime) SET @QPk = SCOPE_IDENTITY()";
        cmd = new SqlCommand(sqlStr, this.mCon);

        cmd.Parameters.AddWithValue("@category", data.Category);
        cmd.Parameters.AddWithValue("@heading", data.Heading);
        cmd.Parameters.AddWithValue("@description", data.Description);
        cmd.Parameters.AddWithValue("@createDate", data.CreateDate);
        cmd.Parameters.AddWithValue("@startDate", data.StartDate);
        cmd.Parameters.AddWithValue("@endDate", data.EndDate);
        cmd.Parameters.AddWithValue("@status", data.Status);
        cmd.Parameters.AddWithValue("@comment", data.Comment);
        cmd.Parameters.AddWithValue("@OneTime", data.OneTime ? "Y" : "N");

        var para = new SqlParameter("@QPk", System.Data.SqlDbType.Int);
        para.Direction = System.Data.ParameterDirection.Output;
        cmd.Parameters.Add(para);

        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();

        qPk = para.Value.ToString();
        return qPk;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="data"></param>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Update)]
    public virtual void Update(QuestionnaireMasterData data)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = @"Update QuestionnaireMaster Set category= @category,heading= @heading,description= @description,updateTime= @updateTime,startDate= @startDate,endDate= @endDate,status= @status, comment= @comment, OneTime=@OneTime Where id =@id";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@category", data.Category);
        cmd.Parameters.AddWithValue("@heading", data.Heading);
        cmd.Parameters.AddWithValue("@description", data.Description);
        cmd.Parameters.AddWithValue("@updateTime", data.UpdateTime);
        cmd.Parameters.AddWithValue("@startDate", data.StartDate);
        cmd.Parameters.AddWithValue("@endDate", data.EndDate);
        cmd.Parameters.AddWithValue("@status", data.Status);
        cmd.Parameters.AddWithValue("@comment", data.Comment);
        cmd.Parameters.AddWithValue("@OneTime", data.OneTime ? "Y" : "N");
        cmd.Parameters.AddWithValue("@id", data.Id);
        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();
    }

}

/// <summary>
/// 問卷主檔
/// </summary>
public class QuestionnaireMasterData
{
    #region"Property"

    private int mid;

    private string mcategory;

    private string mheading;

    private string mdescription;

    private System.DateTime mcreateDate;

    private System.DateTime mupdateTime;

    private System.DateTime mstartDate;

    private System.DateTime mendDate;

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
    ///
    /// </summary>
    public virtual string Category
    {
        get
        {
            return this.mcategory;
        }
        set
        {
            this.mcategory = value;
        }
    }
    /// <summary>
    /// 標題
    /// </summary>
    public virtual string Heading
    {
        get
        {
            return this.mheading;
        }
        set
        {
            this.mheading = value;
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
    /// 建立日期
    /// </summary>
    public virtual System.DateTime CreateDate
    {
        get
        {
            return this.mcreateDate;
        }
        set
        {
            this.mcreateDate = value;
        }
    }
    /// <summary>
    /// 更新日期
    /// </summary>
    public virtual System.DateTime UpdateTime
    {
        get
        {
            return this.mupdateTime;
        }
        set
        {
            this.mupdateTime = value;
        }
    }
    /// <summary>
    /// 開始日期
    /// </summary>
    public virtual System.DateTime StartDate
    {
        get
        {
            return this.mstartDate;
        }
        set
        {
            this.mstartDate = value;
        }
    }
    /// <summary>
    /// 結束日期
    /// </summary>
    public virtual System.DateTime EndDate
    {
        get
        {
            return this.mendDate;
        }
        set
        {
            this.mendDate = value;
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

    /// <summary>
    /// 問卷結果顯示文字
    /// </summary>
    public string Comment { get; set; }
    /// <summary>
    /// 是否問卷只能填寫一次，預設為否
    /// </summary>
    public bool OneTime { get; set; }

    #endregion

    /// <summary>
    /// 建構式
    /// </summary>
    public QuestionnaireMasterData()
    {
    }

}

