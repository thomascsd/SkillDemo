using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;


/// <summary>
/// 資料連結層的基礎類別
/// </summary>
public class DALBaseClass
{
    /// <summary>
    /// 連線物件
    /// </summary>
    protected SqlConnection mCon;

    /// <summary>
    /// 查詢用參數
    /// </summary>
    public Dictionary<string, string> Parameters { get; set; }

    /// <summary>
    /// 建構式
    /// </summary>
    public DALBaseClass()
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["ss"].ConnectionString;

        this.mCon = new SqlConnection(connectionstring);
        this.Parameters = new Dictionary<string, string>();
    }

}
