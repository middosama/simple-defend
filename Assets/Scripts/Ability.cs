using System.Collections;
using UnityEngine;
using System;

[Serializable]
public abstract class Ability
{
    public Sprite image;
    public abstract string description { get; }
    public abstract int price { get; }

}
