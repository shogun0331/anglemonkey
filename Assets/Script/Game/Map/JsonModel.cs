using System;

[Serializable]
public class ModelBrick
{
    public int Type;
    public string Tag;
    public float Hp;
    public int Score;

    public float Mass;
    public float AngularDrag;
    public float GravityScale;
}

[Serializable]
public class ModelGround
{
    public int Type;
}

[Serializable]
public class ModelHinge
{
    public int ItemID;
    public int JointID;
    public string Anchor;
    public string ConnectedAnchor;
}

[Serializable]
public class ModelMonkey
{
    public int Type;
}

[Serializable]
public class ModelBanana
{
    public int Type;
    public float Hp;

    public float Mass;
    public float AngularDrag;
    public float GravityScale;
}


