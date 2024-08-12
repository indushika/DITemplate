using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //Grid entities List<IGridEntities> 
    //Cell Data List; data about all the individual cells of the grid 

    //Grid Generator; generate a grid layout at the start of each session according to game settings (world size, etc etc)
        //data about each grid entity that's in the grid (from past sessions); save and load this data 

    //Grid Entity Allocator
        //check if the Grid Entity can be placed; returns if it's possible 
        //allocates the Grid Entity Grid space and updates Grid Data 

        //when placing an entity on the grid we must check to see if it's a buildable area
        //cells are marked buildable or not
        //if there's a grid entity, the neighbouring cells of any one direction should be free at all times? 
        //Cell Data  

    //Grid Entity Spawner 
        //Spawns Entities on the grid, buildings/structures/etc 
}
