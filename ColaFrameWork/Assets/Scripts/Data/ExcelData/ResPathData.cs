//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 资源路径信息类
/// </summary>
[Serializable]
public class ResPathData :LocalDataBase
{
    /// <summary>
    /// 资源的路径
    /// </summary>
    public string resPath;

    /// <summary>
    /// 资源的加载模式(0:Resources;1:bundle)
    /// </summary>
    public int resLoadMode;

    /// <summary>
    /// 资源的生存时间
    /// </summary>
    public int resWaitSec;

    public override void InitWithStr(string strData, char splitChar = ',')
    {
        //去除最后的\r
        if (strData.EndsWith("\r"))
        {
            strData = strData.Substring(0, strData.Length - 1);
        }
        string[] strs = strData.Split(splitChar);
        id = this.GetInt(strs[0]);
        resPath = strs[1];
        resLoadMode = this.GetInt(strs[2]);
        resWaitSec = this.GetInt(strs[3]);
    }
}


/// <summary>
/// ResPathData的数据集类
/// </summary>
public class ResPathDataMap : ILocalDataMapBase
{
    public Dictionary<int,ResPathData> resPathDataList = new Dictionary<int, ResPathData>();

    public void SetMapCsv(string[] rows)
    {
        resPathDataList.Clear();

        for (int i = 4; i < rows.Length; i++)
        {
            ResPathData data = new ResPathData();
            data.InitWithStr(rows[i]);
            resPathDataList.Add(data.id,data);
        }
    }

    public ResPathData GetDataById(int id)
    {
        ResPathData data = null;
        if (null != resPathDataList)
        {
            resPathDataList.TryGetValue(id, out data);
        }
        return data;
    }
}