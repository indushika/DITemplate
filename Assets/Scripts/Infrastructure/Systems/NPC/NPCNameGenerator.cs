using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NPCNameGenerator 
{

    private readonly Dictionary<int, List<string>> firstNamesByCharacterCount;
    private readonly Dictionary<int, List<string>> lastNamesByCharacterCount;


    public NPCNameGenerator(Dictionary<int, List<string>> firstNamesByCharacterCount, Dictionary<int, List<string>> lastNamesByCharacterCount)
    {
        this.firstNamesByCharacterCount = firstNamesByCharacterCount;

        this.lastNamesByCharacterCount = lastNamesByCharacterCount;
    }



    #region API
    public string GetName(int maxCharacterLimit, int minCharacterLimitPerName, int seedId)
    {
        if (firstNamesByCharacterCount == null || lastNamesByCharacterCount == null || firstNamesByCharacterCount.Count == 0
            || lastNamesByCharacterCount.Count == 0)
        {
            throw new KeyNotFoundException("NPCNameGenerator : GetName: Generator data not found.");
        }
        
        var random = new Random(seedId);

        int firstNameCharacterCount = random.Next(minCharacterLimitPerName, maxCharacterLimit);

        int listIndex = firstNamesByCharacterCount.Keys.Last(x => x <= firstNameCharacterCount);

        var firstNameList = firstNamesByCharacterCount[listIndex];

        var pickedFirstName = firstNameList[random.Next(0, firstNameList.Count)];

        int lastNameMaxCharacterLimit = maxCharacterLimit - listIndex + 1;

        if (lastNameMaxCharacterLimit < minCharacterLimitPerName)
        {
            return pickedFirstName;
        }

        int lastNameCharacterCount = random.Next(minCharacterLimitPerName, lastNameMaxCharacterLimit);

        listIndex = lastNamesByCharacterCount.Keys.Last (x => x <= lastNameCharacterCount);

        var lastNameList = lastNamesByCharacterCount[listIndex];

        var pickedLastName = lastNameList[random.Next(0, lastNameList.Count)];

        return (pickedFirstName + " " + pickedLastName);
    }

    #endregion

    #region Implementation
    #endregion
}
