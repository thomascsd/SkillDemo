using System;

/// <summary>
/// 物件字串轉換
/// </summary>
public class Element
{
    /// <summary>
    /// 建構式
    /// </summary>
    public Element()
    {
    }

    /// <summary>
    /// 將字串轉成相對應的物件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T ToElement<T>(T element, string value)
    {
        string[] vals = value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        string[] subs;
        int num;
        DateTime date;

        foreach (string item in vals)
        {
            subs = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (subs.Length == 2)
            {
                var prop = typeof(T).GetProperty(subs[0]);
                switch (prop.PropertyType.Name)
                {
                    case "Int32":
                        int.TryParse(subs[1], out num);
                        prop.SetValue(element, num, null);
                        break;
                    case "DateTime":
                        DateTime.TryParse(subs[1], out date);
                        prop.SetValue(element, date, null);
                        break;
                    case "Boolean":
                        prop.SetValue(element, subs[1] == "Y", null);
                        break;
                    default:
                        prop.SetValue(element, subs[1], null);
                        break;
                }

            }

        }
        return element;
    }

    /// <summary>
    ///將物件屬性轉換成字串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ele"></param>
    /// <returns></returns>
    public static string ToElementString<T>(T ele)
    {
        System.Reflection.PropertyInfo[] props = typeof(T).GetProperties();
        string ret = string.Empty;

        foreach (var prop in props)
        {
            ret += string.Format("{0}:{1},", prop.Name, prop.GetValue(ele, null));
        }

        return ret;
    }

}