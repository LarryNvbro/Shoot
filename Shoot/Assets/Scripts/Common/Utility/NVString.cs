using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NVString
{
    /// <summary>
    /// __참조할테이블명:검색할컬럼:검색키:출력컬럼__
    /// </summary>
    //public static string Format(string str, BIDataContainer container)
    //{
    //    int formatIndex = 0;
    //    List<string> replaceStrings = new List<string>();
    //    while (true)
    //    {
    //        int sIndex = str.IndexOf("__");
    //        if (sIndex < 0) break;
    //        int eIndex = str.IndexOf("__", sIndex + 1);
    //        if (eIndex < 0) break;

    //        string ex = str.Substring(sIndex, eIndex - sIndex + 2);
    //        string[] keys = ex.Substring(2, ex.Length - 4).Split(':');

    //        /*
    //         * keys[0] = 테이블명
    //         * keys[1] = 비교할 Column명
    //         * keys[2] = 찾을 값
    //         * keys[3] = Column명
    //         */
    //        var data =  (IList)container.GetType().GetField(keys[0]).GetValue(container);
    //        if (data == null)
    //        {
    //            Debug.Log("NVString.Format() => Can't find table : " + keys[0]);
    //            str = str.Replace(ex, "##WrongTable##");
    //            continue;
    //        }

    //        for (int i = 0; i < data.Count; ++i)
    //        {
    //            var item = data[i];
    //            string checkstring = item.GetType().GetField(keys[1]).GetValue(item).ToString();
    //            if (checkstring.Equals(keys[2]))
    //            {
    //                string val = item.GetType().GetField(keys[3]).GetValue(item).ToString();
    //                if (val == null || val.Equals(""))
    //                    val = "##WrongColumn##";
    //                string rStr = "{" + formatIndex.ToString() + "}";
    //                formatIndex++;
    //                str = str.Replace(ex, rStr);
    //                Debug.Log(str);
    //                replaceStrings.Add(val);
    //                break;
    //            }
    //        }

    //        if (str.Contains(ex))
    //        {
    //            string rStr = "{" + formatIndex.ToString() + "}";
    //            formatIndex++;
    //            str = str.Replace(ex, rStr);
    //            replaceStrings.Add("##UnknownData##");
    //        }
    //    }
    //    str = string.Format(str, replaceStrings.ToArray());
    //    return str;
    //}
}
