using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private ActivateObject objectActivator;
    public GameObject[] thermalObjects;

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject objet in thermalObjects)
        {
            objet.SetActive(true);
            Vector3 dir = objet.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag != "thermal")
                {
                    objet.SetActive(false);
                }
            }
        }
    }
}
