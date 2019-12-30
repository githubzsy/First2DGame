﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 管理本地化数据
/// </summary>
public static class SaveManager
{
    /// <summary>
    /// 保存到Save文件夹
    /// </summary>
    /// <param name="obj">数据</param>
    /// <param name="fileName">文件名称</param>
    public static void SaveToFile(this object obj, string fileName)
    {
        string path = Application.persistentDataPath + "/Save/" + fileName;
        string dir = Path.GetDirectoryName(path);
        if (Directory.Exists(dir) ==false)
        {
            Directory.CreateDirectory(dir);
        }
        string json = JsonUtility.ToJson(obj);
        File.WriteAllText(path, json);
    }


    /// <summary>
    /// 从Save文件中读取到对应类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <param name="readFromDefault">当读取不到存档时是否从默认状态中读取</param>
    /// <returns></returns>
    public static T ReadFormFile<T>(string fileName,bool readFromDefault=true)
    {
        T t = default;
        string path = Application.persistentDataPath + "/Save/" + fileName;
        t = ReadFromFileInner<T>(path);

        if (t == null && readFromDefault)
        {
            // streamingAssetsPath中读取
            path = Application.streamingAssetsPath + "/Default/" + fileName;

#if UNITY_ANDROID
            WWW www = new WWW(path);
            while (!www.isDone)
            {

            }
            t = JsonUtility.FromJson<T>(www.text);
#else
            t = ReadFromFileInner<T>(path);
#endif
        }
        return t;
    }

    private static T ReadFromFileInner<T>(string path)
    {
        T t = default;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            t = JsonUtility.FromJson<T>(json);
        }
        return t;
    }

    /// <summary>
    /// 保存游戏
    /// </summary>
    internal static void SaveGame()
    {
        PlayerManager.SavePlayer();
        CollectionManager.SaveCollections();
    }

    /// <summary>
    /// 删除存档
    /// </summary>
    internal static void DeleteSaveData()
    {
        string path = Application.persistentDataPath + "/Save/";
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
}
