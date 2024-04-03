using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "BuildingsManifest")]
public class BuildingsManifestSO : ScriptableObject
{
    Scene scene;
    public string ManifestName = "";
    public List<Building> Buildings = new ();
    Dictionary<GameObject, Building> PrefabCollections = new ();

    private void OnValidate()
    {

    }

    public void GenerateFromScene()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "需要先保存当前场景", "确定");
            return;
        }
        if (!EditorUtility.DisplayDialog("提示", "确定要将当前场景作为配置导入？（只会导入预制体）", "确定", "取消")) return;

        Buildings.Clear();
        PrefabCollections.Clear();
        foreach(var s in scene.GetRootGameObjects())
        {
            if (s.GetPrefabDefinition() != null)
            {
                var prefab = (GameObject)s.GetPrefabDefinition();
                if (PrefabCollections.ContainsKey(prefab))
                {
                    PrefabCollections.TryGetValue(prefab, out Building building);
                    building.count++;
                } else
                {
                    var b = new Building(Buildings.Count, prefab);
                    PrefabCollections.Add(prefab, b);
                    Buildings.Add(b);
                }
            }
        }

    }

    public void OutputManifest()
    {
        string path = EditorUtility.SaveFilePanel("选择存储路径", "", "BuildingManifest" + Time.time, ".txt");
        string s = "";
        foreach(var b in Buildings)
        {
            s += JsonUtility.ToJson(b) + "/*/";
        }
        s = s.Substring(0, s.Length - "/*/".Length);
        File.WriteAllText(path, s);
    }

    public void InputManifest()
    {
        string path = EditorUtility.OpenFilePanel("选择配置文件", "", ".txt");
        try
        {
            string txt = File.ReadAllText(path);
            var s = txt.Split("/*/");
            Buildings.Clear();
            foreach (var item in s)
            {
                var buliding = JsonUtility.FromJson<Building>(item);
                Buildings.Add(buliding);
            }
        } catch
        {
            Debug.LogError("文件解析失败，请检查格式");
        }
    }
}
