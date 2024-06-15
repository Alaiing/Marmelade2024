using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Marmelade/Object")]

public class ObjectData : ScriptableObject
{
    [ValueDropdown(nameof(GetTags))]
    public int tag;
    [Range(1, 3)]
    public int AborbAmount = 1;
    [Range(0f, 1f)]
    public float AttractionRate;
    [PreviewField(Height = 100)]
    public Sprite sprite;

    public static IEnumerable GetTags()
    {
        ValueDropdownList<int> value = new();

        for (int i = 0; i < GameData.DATA.objectTags.Length; i++)
        {
            string tag = GameData.DATA.objectTags[i];
            value.Add(tag, i);
        }

        return value;
    }
}
