using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public enum Type
    {
        Stage,
        Level,
        Gold
    }
    public Type type;
    public int goal;
    public int reward;
}
