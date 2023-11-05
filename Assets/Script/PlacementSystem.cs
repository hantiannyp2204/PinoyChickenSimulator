using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public GameObject mouseIndicator, cellIndicator;

    public InputManager inputManager;

    public Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    private GridData floorData;

    private Renderer previewRenderer;

    private List<GameObject> placedGameObjetcs = new();
    private void Start()
    {
        StopPlacement();
        floorData= new();
        previewRenderer= cellIndicator.GetComponentInChildren<Renderer>();
    }
    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        //no ID hence dont run
        if(selectedObjectIndex < 0)
        {
            return;
        }
        cellIndicator.SetActive(true);
        inputManager.OnClicked += Placestructure;
        inputManager.OnExit += StopPlacement;
    }

    private void Placestructure()
    {
        if(inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePos = inputManager.getSelectedMapPos();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        bool placementValidity = checkPlacementValidity(gridPosition, selectedObjectIndex);

        if (placementValidity == false)
        {
            return;
        }

        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);//forcing the indicator prefab to the grid (snapping effect)
        placedGameObjetcs.Add(newObject);

        floorData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placedGameObjetcs.Count-1);

    }

    private bool checkPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {

        return floorData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
       selectedObjectIndex= -1;
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= Placestructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if(selectedObjectIndex<0)
        {
            return;
        }
        Vector3 mousePos = inputManager.getSelectedMapPos();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        bool placementValidity = checkPlacementValidity(gridPosition, selectedObjectIndex);

        previewRenderer.material.color = placementValidity ? Color.white : Color.red;

        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);//forcing the indicator prefab to the grid (snapping effect)
       
    }
}
