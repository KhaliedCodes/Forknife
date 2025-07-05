using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerData
{
    public int Health;
    public int Damage;
    public Vector3 position;
    public Quaternion rotation;
    public List<string> weapons;

}