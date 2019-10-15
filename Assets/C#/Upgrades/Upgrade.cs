using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Health = 1,
    Movement = 1,
    Interaction = 1,
    Gather = 2,
    Melee = 2,
    Range = 2,
    Monument = 3
}

public struct Upgrade
{
    Type type;
    int tier;

    
}
