using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Wood", menuName = "Inventory/Resource/Wood")]
public class Wood : Item
{
    public override void Use()
    {
        Debug.Log("this is a wood");
    }
}