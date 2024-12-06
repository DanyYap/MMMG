using UnityEngine;
using System.Collections.Generic;

public class GridVisualizer : MonoBehaviour
{
    public float cellSize = 1.0f; // Size of each grid cell
    public GameObject plane; // Reference to the plane that defines the grid area
    private List<GameObject> gridCells = new List<GameObject>(); // Object pool for grid cells

    private int previousGridWidth = 0;
    private int previousGridHeight = 0;

    void Start()
    {
        if (plane != null)
        {
            CreateGrid();
        }
        else
        {
            Debug.LogError("Plane reference is not set.");
        }
    }

    void Update()
    {
        // If the grid dimensions change (e.g., plane size changes), recreate the grid
        if (HasGridSizeChanged())
        {
            ClearGrid();
            CreateGrid();
        }
    }

    void CreateGrid()
    {
        // Get the dimensions of the plane based on its scale and size
        Vector3 planeSize = plane.GetComponent<Renderer>().bounds.size;

        // Calculate the number of cells along the X and Z axes based on the plane size
        int gridWidth = Mathf.CeilToInt(planeSize.x / cellSize);
        int gridHeight = Mathf.CeilToInt(planeSize.z / cellSize);

        // Get the position of the plane to center the grid over it
        Vector3 planePosition = plane.transform.position;

        // Get the height of the plane based on its bounds and position
        float planeHeight = planePosition.y - (planeSize.y / 2);

        // Calculate the starting position of the grid (center it over the plane)
        Vector3 startPosition = new Vector3(
            planePosition.x - (planeSize.x / 2) + (cellSize / 2),
            planeHeight + (cellSize / 2), // Ensure the grid is placed at the correct height
            planePosition.z - (planeSize.z / 2) + (cellSize / 2)
        );

        // Loop through grid positions and instantiate cells (cubes)
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Calculate the position for each grid cell based on the starting position
                Vector3 position = new Vector3(
                    startPosition.x + (x * cellSize),
                    startPosition.y, // Keep the y-coordinate consistent
                    startPosition.z + (z * cellSize)
                );

                // Get or create a cube from the pool
                GameObject gridCell = GetPooledCell();
                gridCell.transform.position = position;
                gridCell.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
                gridCell.SetActive(true); // Make the cube active
            }
        }

        // Deactivate any unused cells
        DeactivateUnusedCells(gridWidth, gridHeight);

        // Update the previous grid dimensions
        previousGridWidth = gridWidth;
        previousGridHeight = gridHeight;
    }

    GameObject GetPooledCell()
    {
        // Find an inactive cell in the pool
        foreach (var cell in gridCells)
        {
            if (!cell.activeInHierarchy)
            {
                return cell;
            }
        }

        // If no inactive cell, instantiate a new one, add it to the pool, and return it
        GameObject newCell = CreateCube();
        gridCells.Add(newCell);
        return newCell;
    }

    void DeactivateUnusedCells(int gridWidth, int gridHeight)
    {
        // Calculate the maximum number of cells needed
        int maxCells = gridWidth * gridHeight;

        // Deactivate any cells that are no longer in use
        for (int i = maxCells; i < gridCells.Count; i++)
        {
            gridCells[i].SetActive(false);
        }
    }

    void ClearGrid()
    {
        // Deactivate all grid cells before recreating the grid
        foreach (var cell in gridCells)
        {
            cell.SetActive(false);
        }
    }

    bool HasGridSizeChanged()
    {
        // Check if the grid size has changed (based on the plane size or cell size)
        return previousGridWidth != Mathf.CeilToInt(plane.GetComponent<Renderer>().bounds.size.x / cellSize) ||
               previousGridHeight != Mathf.CeilToInt(plane.GetComponent<Renderer>().bounds.size.z / cellSize);
    }

    GameObject CreateCube()
    {
        // Create a new cube GameObject with a trigger collider
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.AddComponent<BoxCollider>().isTrigger = true; // Add the collider and set it as a trigger

        // Initially disable the cube's MeshRenderer
        MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.enabled = false; // Disable the MeshRenderer so the cube is not visible
        }

        // Initially disable the cube
        cube.SetActive(false);

        return cube;
    }
}
