using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(BuildingsManifestSO))]
public class Buildings_Editor : Editor
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    public override VisualElement CreateInspectorGUI()
    {
        var target = (BuildingsManifestSO)this.target;
        var Buildings = target.Buildings;
        // Each editor window contains a root VisualElement object
        VisualElement root =  m_VisualTreeAsset.Instantiate();
        
        Button GenerateFromSceneBtn = root.Q<Button>("GenerateFromSceneBtn");
        GenerateFromSceneBtn.RegisterCallback<ClickEvent>((e) =>
        {
            bool isSure = EditorUtility.DisplayDialog("��ʾ", "ȷ��Ҫʹ�ó�����Ԥ���帲���������ã�", "ȷ��", "ȡ��");
            if (isSure)
            {
                target.GenerateFromScene();
            }
        });
        Button InputBtn = root.Q<Button>("Input");
        InputBtn.RegisterCallback<ClickEvent>((e) =>
        {
            target.InputManifest();
        });
        Button OutputBtn = root.Q<Button>("Output");
        OutputBtn.RegisterCallback<ClickEvent>((e) =>
        {
            target.OutputManifest();
        });

        return root;
    }

}
