using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[CreateAssetMenu(menuName = "Flags")]
public class Flags : ScriptableObject
{
    [SerializeField] FlagDictionary flagDictionary;


    public FlagDictionary getFlagDictionary()
    {
        return flagDictionary;
    }

    [Serializable]
    public class FlagDictionary : SerializableDictionaryBase<string, string> { }
}


