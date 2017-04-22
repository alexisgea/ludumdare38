using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildableType {crate, turret}

public class Buildable : MonoBehaviour {

    public BuildableType Type = BuildableType.crate;
    public int Cost = 0;

}
