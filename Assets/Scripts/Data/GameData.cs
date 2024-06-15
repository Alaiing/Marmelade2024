using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string[] objectTags;
    public ObjectData[] objects;
}
