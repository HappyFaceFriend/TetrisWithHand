using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControler : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    public void FeedVector(Vector3 inputVector)
    {
        transform.rotation = Quaternion.Euler(inputVector);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
