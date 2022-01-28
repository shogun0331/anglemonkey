using System;

[Serializable]
public class Item 
{
    public int ID;
    public string Type;
    public string Position;
    public string Rotation;
    public string Scale;

    public string Json;
}

[Serializable]
public class Items
{
    public Item[] items;
}
