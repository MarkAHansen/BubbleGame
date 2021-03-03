using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    float Dir;
    bool fire;
    float RotSpeed;
    float maxRange;
    float minRange;
    float LookingAngle;
    // Start is called before the first frame update
    void Start()
    {
        Dir = 0;
        minRange = -60;
        maxRange = 60;
        RotSpeed = 30;
        LookingAngle = 0;
        fire = false;

        NewProjectileRoll(); 



    }

    // Update is called once per frame
    void Update()
    {
        rotate();
        shoot();
        ShowNextcolor.GetComponent<Renderer>().material.color = NextShotscolour;
    }
    void Makeprojectile()
    {
        if (ProctilePrefab != null)
        {
            GameObject projectileObj = (GameObject)GameObject.Instantiate(ProctilePrefab);
            projectileObj.transform.position = SpawnPoint.transform.position;
            projectileObj.transform.rotation = SpawnPoint.transform.rotation;
            projectileObj.GetComponent<Bullet>().colourG = nextShotsColourGroup;
            projectileObj.GetComponent<Bullet>().color = NextShotscolour;
        }
    }
    void rotate()
    {
        if(Input.GetKey(KeyCode.A))
        {
            Dir = 1;
            
        }
        else if(Input.GetKey(KeyCode.D))
        {
            Dir = -1;
            
        }
        else
        {
            Dir = 0;
        }
       
     
        if(LookingAngle >= maxRange)
        {
            LookingAngle = maxRange - 1;
        }
        if(LookingAngle <= minRange)
        {
            LookingAngle = minRange + 1;
        }
        LookingAngle += ((RotSpeed * Time.deltaTime )* Dir);
        Quaternion rot = Quaternion.Euler(0, 0, LookingAngle);

        transform.rotation = rot;
    }

    void shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) && fire == false)
        {
            print("pew");
            fire = true;
            Makeprojectile();
            NewProjectileRoll();
        }
        else if (Input.GetKeyUp(KeyCode.Space) && fire == true) 
        {
            fire = false;
        }
    }
   void NewProjectileRoll()
    {
        int colourg = UnityEngine.Random.Range(0, 4);
        if (colourg == 0)
        {
            nextShotsColourGroup = ColourGroup.Red;
            NextShotscolour = Color.red;
        }
        if (colourg == 1)
        {
            nextShotsColourGroup = ColourGroup.Blue;
            NextShotscolour = Color.blue;
        }
        if (colourg == 2)
        {
            nextShotsColourGroup = ColourGroup.Green;
            NextShotscolour = Color.green;
        }
        if (colourg == 3)
        {
            nextShotsColourGroup = ColourGroup.Yellow;
            NextShotscolour = Color.yellow;
        }
    }

  public ColourGroup GiveColorGroup()
    {
        return nextShotsColourGroup;
    }
    public Color GiveColor()
    {
        return NextShotscolour;
    }


    public GameObject ProctilePrefab;
    public GameObject ShowNextcolor;
    public GameObject SpawnPoint;
     ColourGroup nextShotsColourGroup;
     Color NextShotscolour;
}
