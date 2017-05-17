using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModelGenerator : MonoBehaviour
{

    public static int pathCount = 7;
    public static int modelModelRoomCount = 50;

    private static List<ModelRoom> ModelRoomList = new List<ModelRoom>();
    public static List<ModelRoom> MapModelRoomList = new List<ModelRoom>();


    System.DateTime time1;
    void Update()
    {



        if (Input.GetKeyDown("space"))
        {
            ClearAll();
            ModelRoomList.Clear();
            time1 = System.DateTime.Now;
            generateMapModelRoomList();
            Debug.Log((System.DateTime.Now - time1).TotalMilliseconds);
            DrawMap();
            // DrawAll();


        }
        if (Input.GetKeyDown("a"))
        {
            generateMapModelRoomList();
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
            // wszystkieScieszki(mapGeneratorPhysics.ModelRoomList);
        }
    }


    public void generateMapModelRoomList()
    {
        MapModelRoomList.Clear();
        for (int i = 0; i < modelModelRoomCount; i++)
        {
            if (ModelRoom.createModelModelRoom(ModelRoomList) == null)
                Debug.Log("jakis blad 212");
        }
        findAllNeighbours();
        Debug.Log("1");
        ModelRoom startModelRoom;
        do
        {
            startModelRoom = ModelRoomList[(int)(UnityEngine.Random.value * (ModelRoomList.Count - 1))];
            startModelRoom.findAllPath();
        } while (startModelRoom.pathList.Count < ModelRoomList.Count);


        List<ModelRoom> newModelRoomList = new List<ModelRoom>();
        List<ModelPath> pathList = startModelRoom.pathList;
        pathList.RemoveAt(0);
        MapModelRoomList.Add(startModelRoom);
        for (int i = 0; i < pathCount; i++)
        {
            if (pathList.Count == 0)
                break;


            ModelPath longestPath = pathList[0];
            foreach (ModelPath path in pathList)
            {
                if (longestPath.length() < path.length())
                    longestPath = path;
            }


            MapModelRoomList.Add(longestPath.target);
            foreach (ModelRoom ModelRoom in longestPath.ModelRoomsBetween)
            {
                if (!MapModelRoomList.Contains(ModelRoom))
                    MapModelRoomList.Add(ModelRoom);
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
                foreach (ModelRoom ModelRoom in longestPath.ModelRoomsBetween)
                {
                    if (pathList[j].target == ModelRoom)
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
                if (longestPath.target.isNeighbour(pathList[j].target))   //usuniecie sasiadow konca ciezki
                {
                    pathList.RemoveAt(j);
                    j--;
                    continue;

                }
                for (int k = longestPath.ModelRoomsBetween.Count - 1; k > longestPath.ModelRoomsBetween.Count - 4; k--)
                {
                    if (k < 0)
                        break;
                    if (longestPath.ModelRoomsBetween[k].isNeighbour(pathList[j].target))
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
        ClearNeighbours(ModelRoomList, MapModelRoomList);
        createDoors(MapModelRoomList);
    }
    

    public void DrawModel()
    {
        foreach (ModelRoom ModelRoom in ModelRoomList)
        {
            ModelRoom.draw();
        }
    }
    public void DrawMap()
    {
        foreach (ModelRoom ModelRoom in MapModelRoomList)
        {
            ModelRoom.draw();
        }
    }
    public void ClearAll()
    {
        foreach (ModelRoom ModelRoom in ModelRoomList)
        {
            ModelRoom.clear();
        }
    }
    public void findAllNeighbours()
    {
        for (int i = 0; i < ModelRoomList.Count; i++)
        {
            ModelRoomList[i].findNeighbours(ModelRoomList);
        }
    }

    public void ClearNeighbours(List<ModelRoom> ModelRoomList, List<ModelRoom> newModelRoomList)  //usuwa wszystkich sasiadow ktorzy nei sa w nowej lsicie a byli w starej
    {
        foreach (ModelRoom ModelRoom in ModelRoomList)
        {
            if (!newModelRoomList.Contains(ModelRoom))
            {
                foreach (ModelRoom ModelRoom1 in newModelRoomList)
                {
                    ModelRoom1.removeNeighbour(ModelRoom);
                }
            }
        }
    }

    public void createDoors(List<ModelRoom> roomList)
    {
        foreach (ModelRoom room in roomList)
        {
            room.createDoorsToNeighbours();
        }
    }
}
