using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[GenerateNativeData]
public struct NPCAttributeTypeData 
{
    public int effectAmount;
    public NPCAttributeType attributeType;
    public string attributeName;
    public Dictionary<NPCStatType, int> effectAmountByStat;

    public int EffectAmount { get => effectAmount; set => effectAmount = value; }
    public NPCAttributeType AttributeType { get => attributeType; set => attributeType = value; }
    public string AttributeName { get => attributeName; set => attributeName = value; }
    public Dictionary<NPCStatType, int> EffectAmountByStat { get => effectAmountByStat; set => effectAmountByStat = value; }
}


