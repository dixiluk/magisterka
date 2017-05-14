using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    // Use this for initialization
    public GameObject pokoj;
    GameObject tmpObject;
      

    MapGeneratorPhysics mapGeneratorPhysics = new MapGeneratorPhysics();
    MapGeneratorWall mapGeneratorWall = new MapGeneratorWall();

    void Start () {
        
    }
    System.DateTime time1;
    // Update is called once per frame

    private void FixedUpdate()
    {
        mapGeneratorPhysics.Update();
    }

    void Update () {

        
        if (Input.GetKeyDown("space"))
        {
        mapGeneratorWall.generateRoomModel(50);
            /* mapGeneratorWall.roomList[0].FloydWarshall();
             foreach (Path path in mapGeneratorWall.roomList[0].pathList)
             {
                 Debug.Log(path.texttt());
             }*/
            //  wszystkieScieszki(mapGeneratorWall.roomList);
            Generate(mapGeneratorWall.roomList);


        }
        if (Input.GetKeyDown("a"))
        {
            time1 = System.DateTime.Now;
            mapGeneratorWall.generateRoomModel(20);
            Debug.Log((System.DateTime.Now - time1));
        }
        if (Input.GetKeyDown("s"))
        {
            mapGeneratorWall.ClearAll();
        }
        if (Input.GetKeyDown("d"))
        {
            mapGeneratorWall.DrawAll();
        }
        if (Input.GetKeyDown("f"))
        {
            Debug.Log("end");
        }
        if (Input.GetKeyDown("g"))
        {
            mapGeneratorWall.ClearAll();
            Generate(mapGeneratorWall.roomList);
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
    

 void Generate(List<Room> roomList)
    {
        Room startRoom;
        do
        {
            startRoom = roomList[(int)(Random.value * (roomList.Count - 1))];
            startRoom.findAllPath();
        } while (startRoom.pathList.Count<roomList.Count);


        List<Room> newRoomList = new List<Room>();
        List<Path> pathList = startRoom.pathList;
        pathList.RemoveAt(0);
        newRoomList.Add(startRoom);
        for (int i = 0; i < 6; i++)
        {
            if (pathList.Count == 0)
                break;


            Path longestPath = pathList[0];
            foreach (Path path in pathList)
            {
                if (longestPath.length() < path.length())
                    longestPath = path;
            }


            newRoomList.Add(longestPath.target);
            foreach (Room room in longestPath.roomsBetween)
            {
                if(!newRoomList.Contains(room))
                    newRoomList.Add(room);
            }

            for (int j = 0; j < pathList.Count; j++)  
            {
                bool delete = false;
                if (pathList[j].target== longestPath.target)   //usuniecie istniejacych pokoi z list celow nowych scierzek
                {
                    pathList.RemoveAt(j);
                    j--;
                    continue;

                }
                foreach (Room room in longestPath.roomsBetween)
                {
                    if (pathList[j].target == room)
                    {
                        delete = true;
                        break;
                    }
                }
                if (delete)
                {
                    pathList.RemoveAt(j);
                    j--;
                    continue;
                }
                if (longestPath.target.isNeighbour(pathList[j].target)!=0)   //usuniecie sasiadow konca ciezki
                {
                    pathList.RemoveAt(j);
                    j--;
                    continue;

                }
                for (int k = longestPath.roomsBetween.Count-1; k > longestPath.roomsBetween.Count - 4; k--)
                {
                    if (k < 0)
                        break;
                    if (longestPath.roomsBetween[k].isNeighbour(pathList[j].target)!=0)
                    {
                        delete = true;
                        break;
                    }
                }
                if (delete)
                {
                    pathList.RemoveAt(j);
                    j--;
                    continue;
                }

            }
            Debug.Log(longestPath.texttt());

        }

        
        foreach (Room item in newRoomList)
        {
            item.draw();
        }
       

    }
        

}
