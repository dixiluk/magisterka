using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorPhysics : MonoBehaviour {

    List<GameObject> physicsObjectList;

    List<Vector3> oldPosition;

    public GameObject physicsObject;

    public int step = 0;


    public List<Room> roomList = new List<Room>();
    
    public void Update () {
        if (step == 1)
            if (checkPhysicsObjects())
            {
                step = 2;
                roomList = toRoomList();
                findNeighbours();
                DrawAll();
                Time.timeScale = 1;
            }

    }
    

    void findNeighbours()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            findNeighbours(i);
        }
    }
    
    void findNeighbours(int roomId)
    {
        int minLength = 3;  //blokada rogow
        Room neighbour;

        for (int x = (int)roomList[roomId].minX + minLength; x <= (int)roomList[roomId].maxX - minLength; x++)
        {
            neighbour = pointInRoom(new Vector2(x, roomList[roomId].minY - 1));
            if (neighbour != null)
                roomList[roomId].addBottomNeighbours(neighbour);

            neighbour = pointInRoom(new Vector2(x, roomList[roomId].maxY + 1));
            if (neighbour != null)
                roomList[roomId].addTopNeighbours(neighbour);
        }

        for (int y = (int)roomList[roomId].minY + minLength; y <= (int)roomList[roomId].maxY - minLength; y++)
        {
            neighbour = pointInRoom(new Vector2(roomList[roomId].minX - 1, y));
            if (neighbour != null)
                roomList[roomId].addLeftNeighbours(neighbour);

            neighbour = pointInRoom(new Vector2(roomList[roomId].maxX + 1, y));
            if (neighbour != null)
                roomList[roomId].addRightNeighbours(neighbour);
        }
        
    }

    Room pointInRoom(Vector2 point)
    {

        foreach (Room room in roomList)
        {
            if (room.pointInRoom(point))
                return room;
        }
        return null;
    }

    public void DrawAll()
    {
        foreach (Room room in roomList)
        {
            room.draw();
        }
    }

    public void run(int count)
    {
        foreach (Room room in roomList)
        {
            room.clear();
        }
        roomList.Clear();


        Time.timeScale = count/5;

        if (physicsObject == null)
            physicsObject = Resources.Load("GeneratePhysicsObject") as GameObject;
        step = 1;
        oldPosition = new List<Vector3>();

        if (physicsObjectList != null)
        {
            if (physicsObjectList.Count != 0)
            {
                foreach (GameObject item in physicsObjectList)
                {
                    Destroy(item);
                }
                physicsObjectList.Clear();
            }
        }
        else
            physicsObjectList = new List<GameObject>();
        generatePhysicsObjects(count);
    }

    bool checkPhysicsObjects()
    {
        if (oldPosition.Count != 0)
        {
            int counter = 0;
            for (int i = 0; i < physicsObjectList.Count; i++)
            {
                if (Vector3.Distance(physicsObjectList[i].transform.position, oldPosition[i]) < 0.001)
                    counter++;
            }
            if (counter == physicsObjectList.Count)
                return true;
            
        }

        oldPosition.Clear();
        foreach (GameObject item in physicsObjectList)
        {
            oldPosition.Add(item.transform.position);
        }
        return false;
    }
    List<Room> toRoomList()
    {
        if (physicsObjectList.Count == 0)
            return null;
        List<Room> list = new List<Room>();
        Bounds tmpBounds;
        int counter = 0;
        foreach (GameObject item in physicsObjectList)
        {
            tmpBounds = item.GetComponent<BoxCollider2D>().bounds;
            Room room = new Room(Mathf.Ceil(tmpBounds.min.x), Mathf.Ceil(tmpBounds.min.y), Mathf.Floor(tmpBounds.max.x), Mathf.Floor(tmpBounds.max.y), counter);
            Destroy(item);
            list.Add(room);
            counter++;
        }


        return list;
    }
    void generatePhysicsObjects(int count)
    {
        foreach (GameObject item in physicsObjectList)
        {
            Destroy(item);
        }
        physicsObjectList.Clear();
        GameObject tmpGameObject;
        for (int i = 0; i < count; i++)
        {
            tmpGameObject = Instantiate(physicsObject);
            tmpGameObject.transform.position = (Random.insideUnitCircle * 5);
            tmpGameObject.GetComponent<BoxCollider2D>().size = randomSize(8, 20, 8, 20);
            tmpGameObject.GetComponent<BoxCollider2D>().enabled = true;
            tmpGameObject.GetComponent<Renderer>().enabled = true;
            physicsObjectList.Add(tmpGameObject);
        }


    }

    Vector2 randomSize(int minX, int maxX, int minY, int maxY)
    {
        Vector2 size;
        float radius;
        do
        {
            radius = (maxX - minX / 2);
            size.x = (int)(NextGaussianDouble() * radius + minX + radius);
        } while (size.x > maxX || size.x < minX);
        do
        {
            radius = (maxY - minY / 2);
            size.y = (int)(NextGaussianDouble() * radius + minY + radius);
        } while (size.y > maxY || size.y < minY);
        return size;

    }

    float NextGaussianDouble()
    {
        float u, v, S;

        do
        {
            u = 2.0f * Random.value - 1.0f;
            v = 2.0f * Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0);

        float fac = Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
        return u * fac;
    }
}
