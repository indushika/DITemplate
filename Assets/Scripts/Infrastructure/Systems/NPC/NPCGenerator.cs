using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator
{
    private ReadOnlyGameData persistentData;

    #region Generators
    private NPCNameGenerator nameGenerator;
    private NPCAttributeGenerator attributeGenerator;
    private NPCStatGenerator statGenerator;
    #endregion

    #region Constants
    //later it should come from a GameSettings object
    private const int minCharacterLengthPerName = 3;
    private const int maxCharacterLengthPerName = 5;
    private const int minAttributeCount = 0;
    private const int maxAttributeCount = 2;
    #endregion


    public NPCGenerator(ReadOnlyGameData persistentData)
    {
        this.persistentData = persistentData;

        InitializeGenerators();
    }

    #region API

    public NPCData GenerateNPCData(int npcID, RuntimeNPCData runtimeData)
    {
        var name = nameGenerator.GetName(maxCharacterLengthPerName, minCharacterLengthPerName, npcID);

        var attributes = attributeGenerator.GetAttributes(minAttributeCount, maxAttributeCount, npcID);

        var stats = statGenerator.GetBaseStats(attributes, npcID);

        if (runtimeData == null) runtimeData = new RuntimeNPCData();
        
        NPCData npcData = new NPCData(npcID, name, attributes, stats, runtimeData);

        return npcData;
    }

    #endregion

    #region Implementation

    #region Initializers

    private void InitializeGenerators()
    {
        InitializeNameGenerator();

        InitializeAttributeGenerator();

        InitializeStatGenerator();
    }

    private void InitializeNameGenerator()
    {
        var firstNamesByCharacterCount = (Dictionary<int, List<string>>)persistentData.FirstNamesCollectionByLength;

        var lastNamesByCharacterCount = (Dictionary<int, List<string>>)persistentData.LastNamesCollectionByLength;

        nameGenerator = new NPCNameGenerator(firstNamesByCharacterCount, lastNamesByCharacterCount);
    }

    private void InitializeAttributeGenerator()
    {
        var attributesByType = (Dictionary<NPCAttributeType, NPCAttributeData>)persistentData.AttributesByType;

        attributeGenerator = new NPCAttributeGenerator(attributesByType);
    }

    private void InitializeStatGenerator()
    {
        var statsByType = (Dictionary<NPCStatType, NPCStatData>)persistentData.StatsByType;

        var attributesByType = (Dictionary<NPCAttributeType, NPCAttributeData>)persistentData.AttributesByType;

        statGenerator = new NPCStatGenerator(statsByType, attributesByType);
    }
    #endregion


    #endregion



}
