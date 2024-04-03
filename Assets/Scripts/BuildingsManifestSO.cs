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
            EditorUtility.DisplayDialog("��ʾ", "��Ҫ�ȱ��浱ǰ����", "ȷ��");
            return;
        }
        if (!EditorUtility.DisplayDialog("��ʾ", "ȷ��Ҫ����ǰ������Ϊ���õ��룿��ֻ�ᵼ��Ԥ���壩", "ȷ��", "ȡ��")) return;

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
        string path = EditorUtility.SaveFilePanel("ѡ��洢·��", "", "BuildingManifest" + Time.time, ".txt");
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
        string path = EditorUtility.OpenFilePanel("ѡ�������ļ�", "", ".txt");
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
            Debug.LogError("�ļ�����ʧ�ܣ������ʽ");
        }
    }
}
