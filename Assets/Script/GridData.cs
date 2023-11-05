using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, placementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedobjectIndex)
    {
        List<Vector3Int>positionToOccupy = CalculatePosition(gridPosition,objectSize);
        placementData data = new placementData(positionToOccupy, ID, placedobjectIndex);
        foreach(var position in positionToOccupy)
        {
            if(placedObjects.ContainsKey(position))
            {
                throw new Exception($"Dictionary already contains this cell position {position}");

            }
            placedObjects[position] = data;
        }
    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for(int x=0; x<objectSize.x;x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition,objectSize);
        foreach(var pos in positionToOccupy)
        {
            if(placedObjects.ContainsKey(pos))
            {
                return false;
            }
            
        }
        return true;
    }
}
public class placementData
{
    public List<Vector3Int> occupiedPositions;



    public int ID { 
        get; 
        private set; 
    }
    public int PlacedObjectIndex
    {
        get;
        private set;
    }
    public placementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}