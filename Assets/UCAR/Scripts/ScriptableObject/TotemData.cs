using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Totam", menuName="game/totam", order=1)]
public class TotemData : ScriptableObject
{
    public GameObject Prefab;
    public Vector3 Location;
}
