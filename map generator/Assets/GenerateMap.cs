using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    
    public static int pathCount = 7;
    public static int modelRoomCount = 50;

    System.DateTime time1;
    void Update()
    {



        if (Input.GetKeyDown("space"))
        {
            Room.ClearAll();
            Room.ModelRoomList.Clear();
            time1 = System.DateTime.Now;
            Room.GenerateMap();
            Debug.Log((System.DateTime.Now - time1).TotalMilliseconds);
            Room.DrawMap();
            // DrawAll();


        }
        if (Input.GetKeyDown("a"))
        {
            Room.GenerateMap();
        }
        if (Input.GetKeyDown("s"))
        {
            Room.DrawMap();
        }
        if (Input.GetKeyDown("d"))
        {
            Room.DrawModel();
        }
        if (Input.GetKeyDown("f"))
        {
        }
        if (Input.GetKeyDown("g"))
        {
        }
        if (Input.GetKeyDown("q"))
        {
            // mapGeneratorPhysics.run(50);
        }
        if (Input.GetKeyDown("w"))
        {
            // wszystkieScieszki(mapGeneratorPhysics.roomList);
        }
    }


    
}
