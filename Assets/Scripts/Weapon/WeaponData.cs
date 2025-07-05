using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponData", menuName = "FPS/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public Vector3 Position;
    public Quaternion Rotation;
}
