using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeCluster
{

    public CubeCluster()
    {
       
        m_Cubes = new List<ClickableCube>();

        if (GameObject.Find("CubeGrid").GetComponent<CubeGrid>())
        {
            //if(m_Cube)
            // //Choosing a random color for the cluster
            // m_ClusterColor = new Color(
            //     UnityEngine.Random.Range(0.25f, 1.0f),
            //     UnityEngine.Random.Range(0.25f, 1.0f),
            //     UnityEngine.Random.Range(0.25f, 1.0f)
            //     );

            
            GameObject.Find("CubeGrid").GetComponent<CubeGrid>().CalculatedCluster = true;
           
        }

    }

    public void AddCube(ClickableCube cube)
    {
        m_Cubes.Add(cube);
        if(cube.GetColourGroup() == ColourGroup.Red)
        {

            m_ClusterColor = Color.red;
        }
        if (cube.GetColourGroup() == ColourGroup.Blue)
        {

            m_ClusterColor = Color.blue;
        }
        if (cube.GetColourGroup() == ColourGroup.Green)
        {

            m_ClusterColor = Color.green;
        }
        if (cube.GetColourGroup() == ColourGroup.Yellow)
        {

            m_ClusterColor = Color.yellow;
        }
        cube.ActivatedColor = m_ClusterColor;
    }
    public void DeactivateCubes()
    {
        foreach (ClickableCube cube in m_Cubes)
        {
            cube.ToggleActivated();
            // cube.transform.position = new Vector3(0, -100000000, 0);
            
        }
       
    }
    public int GetListSize()
    {
        return m_Cubes.Count;
    }
    

    

    List<ClickableCube> m_Cubes;
    
    Color m_ClusterColor;
}
