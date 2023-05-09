using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{

    public bool enable = false;
    // Start is called before the first frame update
    void Start()
    {
       this.gameObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            this.gameObject.SetActive(true); 
        }
        else
        {
            this.gameObject.SetActive(false); 
        }
    }
}
