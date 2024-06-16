using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GameData", menuName = "Marmelade/Game Data")]
public class GameData : ScriptableObject
{
    private static GameData _instance;

    public static GameData DATA
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameData>(typeof(GameData).ToString());
            }

            return _instance;
        }
    }

    [System.Serializable]
    public struct StarData
    {
        public int Threshold;
        public int Gravity;
        public float PulsationPeriod;
    }

    public StarData[] StarDatas;

    public float MinAttractionDistance;
    public float MaxAttractionDistance;

    [System.Serializable]
    public struct TagInfo
    {
        public string name;
        public Color color;
    }
    public TagInfo[] objectTags;
    public ObjectData[] objects;

#if UNITY_EDITOR
    [Button]
    public void UpdateObjectList()
    {
        List<ObjectData> list = new();
        string[] assets = UnityEditor.AssetDatabase.FindAssets("t:ObjectData");
        foreach (string asset in assets)
        {
            ObjectData objectData = UnityEditor.AssetDatabase.LoadAssetAtPath<ObjectData>(UnityEditor.AssetDatabase.GUIDToAssetPath(asset));
            if (objectData != null && objectData.name != "Player")
            {
                list.Add(objectData);
            }
        }
        objects = list.ToArray();
    }
#endif
}
