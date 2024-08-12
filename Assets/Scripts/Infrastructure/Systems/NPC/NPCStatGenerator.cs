using System;
using System.Collections;
using System.Collections.Generic;


public class NPCStatGenerator 
{
    private Dictionary<NPCStatType, NPCStatData> statsByType;
    private Dictionary<NPCAttributeType, NPCAttributeData> attributesByType;

    private Dictionary<NPCStatType, int> baseValuesByStatType;

    public NPCStatGenerator(Dictionary<NPCStatType, NPCStatData> statsByType, 
        Dictionary<NPCAttributeType, NPCAttributeData> attributesByType)
    {
        this.statsByType = statsByType;
        this.attributesByType = attributesByType;
    }


    #region API

    public Dictionary<NPCStatType, int> GetBaseStats(List<NPCAttributeType> attributes, int seedID)
    {
        
        baseValuesByStatType = GenerateBaseValuesByStatType(seedID);

        ApplyAttributesToBaseValues(attributes);

        return baseValuesByStatType;
    }
    #endregion

    #region Implementation
    private Dictionary<NPCStatType, int> GenerateBaseValuesByStatType(int seedId)
    {
        var random = new Random(seedId);

        baseValuesByStatType = new Dictionary<NPCStatType, int>();

        if (statsByType.Count <= 0)
        {
            throw new Exception("NPCStatGenerator: GetBaseStats: Stat Data not found.");
        }

        foreach (var stat in statsByType)
        {
            var statData = stat.Value;

            int baseValue = random.Next(statData.MinBaseValue, statData.MaxBaseValue);

            baseValuesByStatType.Add(stat.Key, baseValue);
        }

        return baseValuesByStatType;

    }

    private void ApplyAttributesToBaseValues(List<NPCAttributeType> attributes)
    {
        if (attributesByType.Count <= 0)
        {
            //throw new Exception("NPCStatGenerator: ApplyAttributesToBaseValues: " +
            //    "Attribute Data missing from Persistent Data.");

            return;
        }

        if (baseValuesByStatType == null || baseValuesByStatType.Count <= 0)
        {
            throw new Exception("NPCStatGenerator: ApplyAttributesToBaseValues: Stat Base Values not initialized.");
        }

        var baseStatsToBeProcessed = baseValuesByStatType;

        foreach (var stat in baseStatsToBeProcessed)
        {
            foreach (var attribute in attributes)
            {
                if (attributesByType.TryGetValue(attribute, out NPCAttributeData attributeData))
                {
                    var stats = attributeData.EffectAmountByStat;

                    if (stats.TryGetValue(stat.Key, out int value))
                    {
                        var multiplier = 1 + (value / 100f);

                        int processedValue = (int)(stat.Value * multiplier);

                        baseValuesByStatType[stat.Key] = processedValue;
                    }
                }
            }
        }
    }
    #endregion
}
