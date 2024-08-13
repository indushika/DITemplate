using System;
using System.Collections;
using System.Collections.Generic;

public class AssignmentIdProvider 
{
    private static readonly Random random = new Random();

    #region API
    public int GetAssignmentId(List<int> assignmentIds)
    {
        if (assignmentIds == null)
        {
            throw new ArgumentNullException("AssignmentIdProvider: GetAssignmentId: Assignment Ids not initiated");
        }

        int id;

        do
        {
            id = random.Next(10000);
        }
        while (assignmentIds.Contains(id));


        return id;
    }

    #endregion

    #region Implementation
    #endregion
}
