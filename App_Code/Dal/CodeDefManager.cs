using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;


/// <summary>
/// 代碼定義管理
/// </summary>
[DataObject]
public class CodeDefManager : DALBaseClass
{

    /// <summary>
    /// 取得所有資料
    /// </summary>
    /// <returns></returns>
    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<CodeDefData> GetAllData()
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        CodeDefData data;
        List<CodeDefData> datas = new List<CodeDefData>();

        sqlStr = "Select * From CodeDef Order By Sort";
        cmd = new SqlCommand(sqlStr, this.mCon);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            data = this.GetCodeDef(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }

    /// <summary>
    /// 依CodeType取得資料
    /// </summary>
    /// <param name="codeType"></param>
    /// <returns></returns>
    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<CodeDefData> GetDataByCodeType(string codeType)
    {
        return this.GetDataBykey("CodeType", codeType);
    }

    private List<CodeDefData> GetDataBykey(string key, string value)
    {
        string sqlStr;
        SqlCommand cmd;
        SqlDataReader dr;
        CodeDefData data;
        List<CodeDefData> datas = new List<CodeDefData>();

        sqlStr = string.Format("Select * From CodeDef Where {0} = @{0} Order By Sort", key);
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@" + key, value);
        this.mCon.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            data = this.GetCodeDef(dr);
            datas.Add(data);
        }
        dr.Close();
        cmd.Dispose();
        this.mCon.Close();
        return datas;
    }


    private CodeDefData GetCodeDef(SqlDataReader dr)
    {
        CodeDefData data = new CodeDefData();
        data.CodePK = ((int)(dr["CodePK"]));
        data.CodeType = dr["CodeType"].ToString();
        data.Description = dr["Description"].ToString();
        data.Sort = dr["Sort"].Equals(DBNull.Value) ? 0 : (int)dr["Sort"];
        data.Code = dr["Code"].ToString();
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="data"></param>
    [DataObjectMethod(DataObjectMethodType.Insert)]
    public virtual void Insert(CodeDefData data)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = "Insert Into CodeDef(CodeType,Sort, Description, Code) Values(@CodeType,@Sort, @Description, @Code)";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@CodeType", data.CodeType);
        cmd.Parameters.AddWithValue("@Sort", data.Sort);
        cmd.Parameters.AddWithValue("@Description", data.Description);
        cmd.Parameters.AddWithValue("@Code", data.Code);
        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="data"></param>
    [DataObjectMethod(DataObjectMethodType.Update)]
    public virtual void Update(CodeDefData data)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = "Update CodeDef Set CodeType= @CodeType,Sort= @Sort, Description= @Description, Code=@Code Where CodePK =@CodePK";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@CodeType", data.CodeType);
        cmd.Parameters.AddWithValue("@Sort", data.Sort);
        cmd.Parameters.AddWithValue("@Description", data.Description);
        cmd.Parameters.AddWithValue("@Code", data.Code);
        cmd.Parameters.AddWithValue("@CodePK", data.CodePK);
        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();
    }

    /// <summary>
    /// 更新排序
    /// </summary>
    /// <param name="codeType"></param>
    /// <param name="code"></param>
    /// <param name="sort"></param>
    public void UpdateSort(string codeType, string code, int sort)
    {
        string sqlStr;
        SqlCommand cmd;
        sqlStr = "Update CodeDef Set Sort= @Sort Where CodeType =@CodeType And Code =@Code";
        cmd = new SqlCommand(sqlStr, this.mCon);
        cmd.Parameters.AddWithValue("@Sort", sort);
        cmd.Parameters.AddWithValue("@CodeType", codeType);
        cmd.Parameters.AddWithValue("@Code", code);
        this.mCon.Open();
        cmd.ExecuteNonQuery();
        this.mCon.Close();
        cmd.Dispose();
    }


}

/// <summary>
/// 代碼定義資料
/// </summary>
[Serializable]
public class CodeDefData
{
    #region"Propety And Field"
    private int mCodePK;

    private string mCodeType;

    private int mSort;
    /// <summary>
    /// 建構式
    /// </summary>
    public CodeDefData()
    {
    }
    /// <summary>
    /// 樣板分類PK
    /// </summary>
    public int CodePK
    {
        get { return this.mCodePK; }
        set { this.mCodePK = value; }
    }
    /// <summary>
    /// Code Type
    /// </summary>
    public virtual string CodeType
    {
        get { return this.mCodeType; }
        set { this.mCodeType = value; }
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
    /// 描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 代號
    /// </summary>
    public string Code { get; set; }
    #endregion
}

