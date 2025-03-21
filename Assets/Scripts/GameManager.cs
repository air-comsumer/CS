using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   
    void Start()
    {
        NetMgr.Instance.Connect("127.0.0.1", 8080);     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
