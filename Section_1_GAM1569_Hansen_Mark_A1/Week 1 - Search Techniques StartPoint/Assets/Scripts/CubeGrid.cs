using UnityEngine;
using System.Collections.Generic;

public class CubeGrid : MonoBehaviour
{
    public int GridDimX = 15;
    public int GridDimY = 10;
    public float GridSpacing = 1.2f;
    public GameObject CubePrefab;

    public bool CalculatedCluster;

    void Start()
    {
        //Create a 2D array to hold the cubes, then generate the cubes in it
        m_Grid = new ClickableCube[GridDimX, GridDimY];

        //Create a grid of visited cells
        m_VisitedCells = new bool[GridDimX, GridDimY];

        GenerateCubes();
        
        m_CubeClusters = new List<CubeCluster>();

        CalculatedCluster = false;
        

    }

    void Update()
    {
        //RecalcuateClusters();
    }

    public void RecalcuateClusters()
    {
        
        m_CubeClusters.Clear();
        
        ClearVisitedCells();
        
        List<IntVector2> needsAVisit = new List<IntVector2>();

        IntVector2 StartCoords;

        while(FindNonVisitedCoord(out StartCoords))
        {
            CubeCluster cubecluster = new CubeCluster();

            m_CubeClusters.Add(cubecluster);

            needsAVisit.Add(StartCoords);

            while(needsAVisit.Count > 0)
            {
                int indexToRemove = needsAVisit.Count - 1;
                IntVector2 currentCoords = needsAVisit[indexToRemove];

                needsAVisit.RemoveAt(indexToRemove);

                if(m_VisitedCells[currentCoords.x,currentCoords.y])
                {
                    continue;
                }

                m_VisitedCells[currentCoords.x, currentCoords.y] = true;

                ClickableCube currentCube = m_Grid[currentCoords.x, currentCoords.y];
                currentCube.setCluster(cubecluster);

                if (!currentCube.Activated)
                {
                    continue;
                }

                cubecluster.AddCube(currentCube);

                AddCoordsIfNeeded(currentCoords, new IntVector2(1, 0), ref needsAVisit, currentCube.GetColourGroup());//right
                AddCoordsIfNeeded(currentCoords, new IntVector2(-1, 0), ref needsAVisit, currentCube.GetColourGroup());//left
                AddCoordsIfNeeded(currentCoords, new IntVector2(0, 1), ref needsAVisit, currentCube.GetColourGroup());//up
                AddCoordsIfNeeded(currentCoords, new IntVector2(0, -1), ref needsAVisit, currentCube.GetColourGroup());//down


            }
        }

       
    }

    //A helper function to add new coordinates to check in our search.
    //It will first create the new coords, then double check if the coordinates are valid before adding 
    //them to the list
    void AddCoordsIfNeeded(IntVector2 coords, IntVector2 checkDir, ref List<IntVector2> coordsToVisit, ColourGroup colourGroup)
    {
        IntVector2 nextCoords = coords + checkDir;

        if (AreCoordsValid(nextCoords))
        {
            ClickableCube Tempcheck = m_Grid[nextCoords.x, nextCoords.y];
            if (Tempcheck != null)
            {
                if (colourGroup == Tempcheck.GetColourGroup())
                {
                    coordsToVisit.Add(nextCoords);
                }
            }
        }
    }

    public void DeactivateCluster(ClickableCube clickedCube)
    {
        //m_CubeClusters.Clear();
        //ClearVisitedCells();

        // List<ClickableCube> needsToBeDestroyed = new List<ClickableCube>();
        // needsToBeDestroyed.Add(clickedCube);
        clickedCube.GetCluster().DeactivateCubes();

    }

    //This is a helper function to check if a set of coordinates are valid.  (out of bounds)
    bool AreCoordsValid(IntVector2 coords)
    {
        return coords.x >= 0 && coords.y >= 0 &&
            coords.x < m_Grid.GetLength(0) && coords.y < m_Grid.GetLength(1);
    }

    //Sets all of the visited cells back to non-visited
    void ClearVisitedCells()
    {
        for (int x = 0; x < GridDimX; ++x)
        {
            for (int y = 0; y < GridDimY; ++y)
            {
                m_VisitedCells[x, y] = false;
            }
        }
    }

    //Finds a non-visited coordinate where we can start a search
    //it will return true if a non-vistited coordinate was found where we can start searching
    //
    //Note: This starts looking at the start of the grid every time.  One potential optimization
    //      could be to keep track of where you stopped looking last time, and then pick up from
    //      this location next time the function is executed.
    bool FindNonVisitedCoord(out IntVector2 nonVisitedCoord)
    {
        for (int x = 0; x < GridDimX; ++x)
        {
            for (int y = 0; y < GridDimY; ++y)
            {
                if (m_Grid[x, y] != null)
                {
                    if (m_Grid[x, y].Activated && !m_VisitedCells[x, y])
                    {
                        nonVisitedCoord = new IntVector2(x, y);

                        return true;
                    }
                }
            }
        }

        //No non-visited activated coords found.  Set the value to an invalid coordinate
        //and return false
        nonVisitedCoord = new IntVector2(-1, -1);

        return false;
    }

    //Creates the cubes in the right position and puts them in the grid    
    private void GenerateCubes()
    {
        for (int x = 0; x < GridDimX; ++x)
        {
            for (int y = 2; y < GridDimY; ++y)
            {
                Vector3 offset = new Vector3(x * GridSpacing, y * GridSpacing, 0.0f);

                GameObject cubeObj = (GameObject)GameObject.Instantiate(CubePrefab);

                cubeObj.transform.position = offset + transform.position;

                cubeObj.transform.parent = transform;

                m_Grid[x, y] = cubeObj.GetComponent<ClickableCube>();
                cubeObj.GetComponent<ClickableCube>().SetCoords(new IntVector2(x, y));
                DebugUtils.Assert(m_Grid[x, y] != null, "Could not find clickableCube component.");
            }
        }
    }

    public void AddCubeToGrid(IntVector2 coords, ColourGroup CG, Color C, Vector2 diff)
    {
        int newYCoord = coords.y;
        int newXCoord = coords.x;
        if (diff.y >= 0.5f)
        {
            newYCoord = coords.y - 1;
        }
        else if (diff.x >= 0.5f)
        {
            newXCoord = coords.x - 1;
        }
        else if (diff.x <= -0.5f)
        {
            newXCoord = coords.x + 1;
        }

        Vector3 offset = new Vector3(newXCoord * GridSpacing, newYCoord * GridSpacing, 0.0f);

                GameObject cubeObj = (GameObject)GameObject.Instantiate(CubePrefab);

                cubeObj.transform.position = offset + transform.position;

                cubeObj.transform.parent = transform;

                m_Grid[newXCoord, newYCoord] = cubeObj.GetComponent<ClickableCube>();
                cubeObj.GetComponent<ClickableCube>().SetCoords(new IntVector2(newXCoord, newYCoord));
                cubeObj.GetComponent<ClickableCube>().SetColorStuff(CG , C);
               
                RecalcuateClusters();
                if (cubeObj.GetComponent<ClickableCube>().cluster.GetListSize() > 1)
                {
                   
                cubeObj.GetComponent<ClickableCube>().cluster.DeactivateCubes();

                }

        DebugUtils.Assert(m_Grid[newXCoord, newYCoord] != null, "Could not find clickableCube component.");
            
        
    }

    //Using a 2D array to represent the grid
    ClickableCube[,] m_Grid;
    bool[,] m_VisitedCells;

    List<CubeCluster> m_CubeClusters;
}
