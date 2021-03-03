using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += this.transform.TransformDirection(Vector3.up) * 10 * Time.deltaTime;


        
    }
    private void OnTriggerEnter(Collider other)
    {

    }


    public ColourGroup colourG;
    public Color color;
  
}
