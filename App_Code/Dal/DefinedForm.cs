using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// 自定表單主檔
/// </summary>
public class DefinedFormManager : DALBaseClass
{
    /// <summary>
    /// 取得所有資料
    /// </summary>
    /// <returns></returns>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
    public virtual List<DefinedFormData> GetAllData()
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        DefinedFormData data;
        List<DefinedFormData> datas = new List<DefinedFormData>();
        sqlStr = "Select * From DefinedForm";
        cmd = new SqlCommand(sqlStr, this.mCon);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetDefindedForm(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    /// <summary>
    /// 依Pk(主索值)，取得資料
    /// </summary>
    /// <param name="userModulePk"></param>
    /// <returns></returns>
    public DefinedFormData GetDataByPk(string pk)
    {
        var datas = this.GetDataBykey("PK", pk);
        if (datas.Count > 0)
        {
            return datas[0];
        }
        return null;
    }



    private List<DefinedFormData> GetDataBykey(string key, string value)
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        DefinedFormData data;
        List<DefinedFormData> datas = new List<DefinedFormData>();
        sqlStr = string.Format("Select * From DefinedForm Where {0} = @{0}", key);
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@" + key, value);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        for (; dr.Read(); )
        {
            data = this.GetDefindedForm(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    private DefinedFormData GetDefindedForm(SqlDataReader dr)
    {
        DefinedFormData data = new DefinedFormData();
        data.PK = ((int)(dr["PK"]));
        data.CompanyId = ((int)(dr["CompanyId"]));
        data.UserModulePK = ((int)(dr["UserModulePK"]));
        data.CreateDate = ((DateTime)(dr["CreateDate"]));
        data.Title = dr["Title"].ToString();
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="data"></param>
    [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Insert)]
    public string Insert(DefinedFormData data)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = "Insert Into DefinedForm(CompanyId,UserModulePK,CreateDate,Title) Values(@Compan" +
        "yId,@UserModulePK,@CreateDate, @Title) SET @QPk = SCOPE_IDENTITY()";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@CompanyId", data.CompanyId);
        cmd.Parameters.AddWithValue("@UserModulePK", data.UserModulePK);
        cmd.Parameters.AddWithValue("@CreateDate", data.CreateDate);
        cmd.Parameters.AddWithValue("@Title", data.Title);

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
    public virtual void Update(DefinedFormData data)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = "Update DefinedForm Set CompanyId= @CompanyId,UserModulePK= @UserModulePK,CreateD" +
        "ate= @CreateDate, Title=@Title Where PK =@PK";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@CompanyId", data.CompanyId);
        cmd.Parameters.AddWithValue("@UserModulePK", data.UserModulePK);
        cmd.Parameters.AddWithValue("@CreateDate", data.CreateDate);
        cmd.Parameters.AddWithValue("@Title", data.Title);
        cmd.Parameters.AddWithValue("@PK", data.PK);
        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();
    }

}

/// <summary>
/// 自定單表主檔資料
/// </summary>
public class DefinedFormData
{
    #region property

    private int mPK;

    private int mCompanyId;

    private int mUserModulePK;

    private System.DateTime mCreateDate;
    /// <summary>
    /// 主索值
    /// </summary>
    public virtual int PK
    {
        get
        {
            return this.mPK;
        }
        set
        {
            this.mPK = value;
        }
    }
    /// <summary>
    /// 公司序號
    /// </summary>
    public virtual int CompanyId
    {
        get
        {
            return this.mCompanyId;
        }
        set
        {
            this.mCompanyId = value;
        }
    }
    /// <summary>
    /// 自定模組編號
    /// </summary>
    public virtual int UserModulePK
    {
        get
        {
            return this.mUserModulePK;
        }
        set
        {
            this.mUserModulePK = value;
        }
    }
    /// <summary>
    /// 建立日期
    /// </summary>
    public virtual System.DateTime CreateDate
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
    /// <summary>
    /// 表單標題
    /// </summary>
    public string Title { get; set; }
    #endregion
    /// <summary>
    /// 建構式
    /// </summary>
    public DefinedFormData()
    {
    }
}

