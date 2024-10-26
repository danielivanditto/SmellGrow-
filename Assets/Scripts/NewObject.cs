using UnityEngine;

public enum ObjectType
{
    item,
    obj,
    block
}

[CreateAssetMenu(fileName = "SmellGrow Object", menuName = "New SmellGrow Object")]

public class NewObject : ScriptableObject
{
    public uint ID = 000;
    public string objectFullName = "name";
    [TextArea] public string description = "description";
    public Sprite objSprite;
    public GameObject obj;
    public ObjectType type = new ObjectType();
}
