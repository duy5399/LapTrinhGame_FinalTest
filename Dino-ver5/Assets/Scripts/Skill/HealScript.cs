using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class HealScript : MonoBehaviour
{
    public float destroyTime = 2f;
    //public Vector3 ps;
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, destroyTime - Time.deltaTime);    //bất cứ khi nào đạn được khởi tạo nó sẽ chờ 2 giây sau đó hủy các cú Kick
    }

    // Update is called once per frame
    void Update()
    {
        Destroy();
    }

    [PunRPC]
    public void UpdatePosition(Vector3 position)
    {
        //ps = position;
        gameObject.transform.position = position;
    }

    //[PunRPC]
    public void Destroy()
    {
        if (destroyTime > 0)
        {
            destroyTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
