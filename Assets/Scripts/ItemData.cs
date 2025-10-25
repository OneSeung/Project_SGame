using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public Material Material;
    public Sprite Icon;
    public float Duration;
}
