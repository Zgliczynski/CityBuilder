using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacementManager placementManager;
    public RoadFixer roadFixer;

    public List<Vector3Int> temporaryPlacementPosition = new List<Vector3Int>();
    public List<Vector3Int> roadPositionToRecheck = new List<Vector3Int>();

    public GameObject roadStraight;

    private void Start()
    {
        roadFixer = GetComponent<RoadFixer>();
    }

    public void PlaceRoad(Vector3Int position)
    {
        if (placementManager.CheckIfPositionInBound(position) == false)
            return;

        if (placementManager.CheckIfPositionIsFree(position) == false)
            return;

        temporaryPlacementPosition.Clear();
        temporaryPlacementPosition.Add(position);

        placementManager.PlaceTemporaryStructure(position, roadStraight, CellType.Road);

        FixRoadPrefabs();
    }

    private void FixRoadPrefabs()
    {
        foreach (var temporaryPosition in temporaryPlacementPosition)
        {
            roadFixer.FixRoadAtPosition(placementManager, temporaryPosition);
            var neighbours = placementManager.GetNeighbourTypesFor(temporaryPosition, CellType.Road);
            foreach (var roadposition in neighbours)
            {
                roadPositionToRecheck.Add(roadposition);
            }
        }

        foreach (var positionToFix in roadPositionToRecheck)
        {
            roadFixer.FixRoadAtPosition(placementManager, positionToFix);
        }

    }
}
