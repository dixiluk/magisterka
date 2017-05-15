using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{

    public static int pathCount = 7;
    public static int modelRoomCount = 50;

    public static List<Room> ModelRoomList = new List<Room>();
    public static List<Room> MapRoomList = new List<Room>();


    System.DateTime time1;
    void Update()
    {



        if (Input.GetKeyDown("space"))
        {
            ClearAll();
            ModelRoomList.Clear();
            time1 = System.DateTime.Now;
            generate();
            Debug.Log((System.DateTime.Now - time1).TotalMilliseconds);
            DrawMap();
            // DrawAll();


        }
        if (Input.GetKeyDown("a"))
        {
            generate();
        }
        if (Input.GetKeyDown("s"))
        {
            DrawMap();
        }
        if (Input.GetKeyDown("d"))
        {
            DrawModel();
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


    public void generate()
    {
        MapRoomList.Clear();
        for (int i = 0; i < modelRoomCount; i++)
        {
            if (Room.createModelRoom(ModelRoomList) == null)
                Debug.Log("jakis blad 212");
        }
        findAllNeighbours();
        Debug.Log("1");
        Room startRoom;
        do
        {
            startRoom = ModelRoomList[(int)(UnityEngine.Random.value * (ModelRoomList.Count - 1))];
            startRoom.findAllPath();
        } while (startRoom.pathList.Count < ModelRoomList.Count);


        List<Room> newRoomList = new List<Room>();
        List<Path> pathList = startRoom.pathList;
        pathList.RemoveAt(0);
        MapRoomList.Add(startRoom);
        for (int i = 0; i < pathCount; i++)
        {
            if (pathList.Count == 0)
                break;


            Path longestPath = pathList[0];
            foreach (Path path in pathList)
            {
                if (longestPath.length() < path.length())
                    longestPath = path;
            }


            MapRoomList.Add(longestPath.target);
            foreach (Room room in longestPath.roomsBetween)
            {
                if (!MapRoomList.Contains(room))
                    MapRoomList.Add(room);
            }

            for (int j = 0; j < pathList.Count; j++)
            {
                bool delete = false;
                if (pathList[j].target == longestPath.target)   //usuniecie istniejacych pokoi z list celow nowych scierzek
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
                if (longestPath.target.isNeighbour(pathList[j].target) != 0)   //usuniecie sasiadow konca ciezki
                {
                    pathList.RemoveAt(j);
                    j--;
                    continue;

                }
                for (int k = longestPath.roomsBetween.Count - 1; k > longestPath.roomsBetween.Count - 4; k--)
                {
                    if (k < 0)
                        break;
                    if (longestPath.roomsBetween[k].isNeighbour(pathList[j].target) != 0)
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

        }
    }

    public void DrawModel()
    {
        foreach (Room room in ModelRoomList)
        {
            room.draw();
        }
    }
    public void DrawMap()
    {
        foreach (Room room in MapRoomList)
        {
            room.draw();
        }
    }
    public void ClearAll()
    {
        foreach (Room room in ModelRoomList)
        {
            room.clear();
        }
    }
    public void findAllNeighbours()
    {
        for (int i = 0; i < ModelRoomList.Count; i++)
        {
            ModelRoomList[i].findNeighbours(ModelRoomList);
        }
    }

}
