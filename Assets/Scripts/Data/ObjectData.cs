using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Marmelade/Object")]

public class ObjectData : ScriptableObject
{
    [Range(1, 3)]
    public int AborbAmount = 1;
    [Range(0f, 1f)]
    public float AttractionRate;
    [PreviewField(Height = 100)]
    public Sprite sprite;
}
