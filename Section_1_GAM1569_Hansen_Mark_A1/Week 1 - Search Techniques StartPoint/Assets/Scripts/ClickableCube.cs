using UnityEngine;
using System.Collections;
public enum ColourGroup
{
    na,
    Red,
    Blue,
    Green,
    Yellow
}
public class ClickableCube : MonoBehaviour
{
   
    void Start()
    {
        m_CubeGrid = GameObject.Find("CubeGrid").GetComponent<CubeGrid>();


        if (colourGroup == ColourGroup.na)
        {
            Activated = true;

            int colourg = UnityEngine.Random.Range(0, 4);
            if (colourg == 0)
            {
                colourGroup = ColourGroup.Red;
                m_ActivatedColor = Color.red;
            }
            if (colourg == 1)
            {
                colourGroup = ColourGroup.Blue;
                m_ActivatedColor = Color.blue;
            }
            if (colourg == 2)
            {
                colourGroup = ColourGroup.Green;
                m_ActivatedColor = Color.green;
            }
            if (colourg == 3)
            {
                colourGroup = ColourGroup.Yellow;
                m_ActivatedColor = Color.yellow;
            }

            UpdateVisuals();
        }
    }

    void Update()
    {

    }

    public bool Activated
    {
        get
        {
            return m_Activated;
        }

        set
        {
            m_Activated = value;

            //Update the visuals since the activated value changes
            UpdateVisuals();
        }
    }

    public Color ActivatedColor
    {
        get
        {
            return m_ActivatedColor;
        }

        set
        {
            m_ActivatedColor = value;

            //Update the visuals since the color changed
            UpdateVisuals();
        }
    }

    public void ToggleActivated()
    {
        m_Activated = !m_Activated;

        //Update the visuals since the activated value changes
        UpdateVisuals();
    }
    private void OnTriggerEnter(Collider other)
    {

        //   m_CubeGrid.DeactivateCluster(this);
        if (other.gameObject.name == "projectile(Clone)")
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();

            if (m_Activated)
            {
                Vector2 diff = new Vector2(transform.position.x - bullet.transform.position.x, transform.position.y - bullet.transform.position.y);
                m_CubeGrid.AddCubeToGrid(coords, bullet.colourG,bullet.color, diff);
                UpdateVisuals();
                
 

                other.gameObject.SetActive(false);

                

            }
        }

    }

        private void UpdateVisuals()
    {
        if (GetComponent<Renderer>().material == null)
        {
            return;
        }

        //Set the material color 
        if (m_Activated)
        {
            GetComponent<Renderer>().material.color = m_ActivatedColor;
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
    }

    public void setCluster(CubeCluster Cluster)
    {
        cluster = Cluster;
    }
    public CubeCluster GetCluster()
    {
        return cluster;
    }

    public void SetCoords(IntVector2 newPos)
    {
        coords = newPos;
    }
    public IntVector2 GetCoords()
    {
        return coords;
    }
    public void SetColorStuff(ColourGroup CG, Color C)
    {
        colourGroup = CG;
        m_Activated = true;
        ActivatedColor = C;
        
    }
    public ColourGroup GetColourGroup()
    {
        return colourGroup;
    }

    private bool m_Activated;
    private Color m_ActivatedColor;
    private ColourGroup colourGroup;
    private IntVector2 coords;
    public CubeCluster cluster;
    CubeGrid m_CubeGrid;
}
