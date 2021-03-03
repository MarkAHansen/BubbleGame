using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
    private const float PointerRayDist = 1000.0f;

    void Start()
    {
        m_CubeGrid = GameObject.Find("CubeGrid").GetComponent<CubeGrid>();
      
    }

    void Update()
    {
        //Handling input
        if (Input.GetMouseButton(0))
        {
       //     HandleMouseClick();
        }
        else
        {
            //Clear the cube we were clicking on
            m_CurrentCube = null;
        }
    }

   private void HandleMouseClick()
   {
       //Use a ray cast to figure what cube we are clicking
       Ray pointerRay = Camera.main.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
   
       RaycastHit hitInfo;
       if (!Physics.Raycast(pointerRay, out hitInfo, PointerRayDist))
       {
           return;
       }
   
       //Get the cube object or ignore the object if it isn't one.
       ClickableCube clickableCube = hitInfo.collider.gameObject.GetComponent<ClickableCube>();
       if (clickableCube == null)
       {
           return;
       }
   
       //If this is the first cube we clicked on with out lifting the mouse button, remember
       //the activated state we will set it to.  This is to make dragging the mouse to set the
       //cubes more convenient.
       if (m_CurrentCube == null)
       {
           m_ActivateCubes = !clickableCube.Activated;
       }
   
       //If the cube is a new one, set the active state
       if (clickableCube != m_CurrentCube)
       {
        //   clickableCube.Activated = m_ActivateCubes;
   
           m_CurrentCube = clickableCube;
   
           m_CubeGrid.RecalcuateClusters();
            m_CubeGrid.DeactivateCluster(clickableCube);
       }
   }

    CubeGrid m_CubeGrid;
    ClickableCube m_CurrentCube;
    bool m_ActivateCubes;
}
