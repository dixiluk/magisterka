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
        mapGeneratorWall.run(200);
        wszystkieScieszki(mapGeneratorWall.roomList);
        test(mapGeneratorWall.roomList);


        }
        if (Input.GetKeyDown("a"))
        {
            time1 = System.DateTime.Now;
            mapGeneratorWall.run(50);
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
            wszystkieScieszki(mapGeneratorWall.roomList);
            Debug.Log("end");
        }
        if (Input.GetKeyDown("g"))
        {
            mapGeneratorWall.ClearAll();
            test(mapGeneratorWall.roomList);
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

    void wszystkieScieszki(List<Room> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            foreach (Room room in roomList)
            {
                room.FloydWarshall();
            }

        }
        while(roomList[0].pathList.Count< roomList.Count)
        {
            int min = roomList.Count;
            int minId=0;
            for (int i = 1; i < roomList.Count; i++)
            {
                if(roomList[i].pathList.Count< min)
                {
                    min = roomList[i].pathList.Count;
                    minId = i;
                }
            }
            List<Path> deleteList = roomList[minId].pathList;
            foreach (Path path in deleteList)
            {
                path.target.clear();
                roomList.Remove(path.target);
            }
        }
/*
        foreach (Room room in roomList)
        {
            Debug.Log(room.pathList.Count);
        }*/
    }

 void test(List<Room> roomList)
    {
        Room startRoom = roomList[(int)(Random.value * (roomList.Count - 1))];
        List<Room> newRoomList = new List<Room>();
        List<Path> listascierzek = startRoom.pathList;
        listascierzek.RemoveAt(0);
        newRoomList.Add(startRoom);
        for (int i = 0; i < 6; i++)
        {
            if (listascierzek.Count == 0)
                break;


            Path longestPath = listascierzek[0];
            foreach (Path path in listascierzek)
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

            for (int j = 0; j < listascierzek.Count; j++)  
            {
                bool delete = false;
                if (listascierzek[j].target== longestPath.target)   //usuniecie istniejacych pokoi z list celow nowych scierzek
                {
                    listascierzek.RemoveAt(j);
                    j--;
                    continue;

                }
                foreach (Room room in longestPath.roomsBetween)
                {
                    if (listascierzek[j].target == room)
                    {
                        delete = true;
                        break;
                    }
                }
                if (delete)
                {
                    listascierzek.RemoveAt(j);
                    j--;
                    continue;
                }
                if (longestPath.target.isNeighbour(listascierzek[j].target)!=0)   //usuniecie sasiadow konca ciezki
                {
                    listascierzek.RemoveAt(j);
                    j--;
                    continue;

                }
                for (int k = longestPath.roomsBetween.Count-1; k > longestPath.roomsBetween.Count - 4; k--)
                {
                    if (k < 0)
                        break;
                    if (longestPath.roomsBetween[k].isNeighbour(listascierzek[j].target)!=0)
                    {
                        delete = true;
                        break;
                    }
                }
                if (delete)
                {
                    listascierzek.RemoveAt(j);
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
