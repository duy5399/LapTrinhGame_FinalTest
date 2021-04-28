using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroyStone();
    }

    public void DestroyStone()
    {
        if (gameObject.transform.position.y <= -9)
            Destroy(gameObject);
    }
}
