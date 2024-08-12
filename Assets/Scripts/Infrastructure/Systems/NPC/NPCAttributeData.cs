using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NPCAttributeData 
{
    private int effectAmount;
    private NPCAttributeType attributeType;
    private string attributeName;
    private Dictionary<NPCStatType, int> effectAmountByStat;

    public int EffectAmount { get => effectAmount; set => effectAmount = value; }
    public NPCAttributeType AttributeType { get => attributeType; set => attributeType = value; }
    public string AttributeName { get => attributeName; set => attributeName = value; }
    public Dictionary<NPCStatType, int> EffectAmountByStat { get => effectAmountByStat; set => effectAmountByStat = value; }
}


