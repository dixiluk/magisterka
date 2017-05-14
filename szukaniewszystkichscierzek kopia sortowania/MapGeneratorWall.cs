using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorWall : MonoBehaviour {

    public List<Room> roomList = new List<Room>();

    List<List<Vector3>> listaPrzyBokach = new List<List<Vector3>>(); // z odpowiada zastrone sasiada 1 - prawa, 2 - lewa, 3 - gora, 4 - dol

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void run(int count)
    {
        foreach (Room room in roomList)
        {
            room.clear();
        }
        roomList.Clear();
        listaPrzyBokach.Clear();

        createRoom(new Vector2(0, 0), randomSize(8, 20, 8, 20), 1,-1);
        count--;

        bool match;
        Vector2 roomSize;
        int rotation;
        for (int i = 0; i < count; i++)
        {
            roomSize = randomSize(8, 20, 8, 20);
            for (int roomId = 0; roomId < listaPrzyBokach.Count; roomId++)
            {
                match = false;
                for (int j = 0; j < listaPrzyBokach[roomId].Count; j++)
                {
                    if (matchInThisPosition(listaPrzyBokach[roomId][j], new Vector2(3, 3)) == 0)
                    {
                        listaPrzyBokach[roomId].RemoveAt(j);
                        j--;
                    }
                    else
                    {
                        rotation = matchInThisPosition(listaPrzyBokach[roomId][j], roomSize);
                        if (rotation != 0)
                        {
                            createRoom(listaPrzyBokach[roomId][j], roomSize, rotation, roomId);
                            match = true;
                            break;
                        }
                    }
                }

                if (match)
                    break;
            }

        }
        findNeighbours();
    }

    int matchInThisPosition(Vector2 position, Vector2 size)
    {
        bool matchRT = true;
        bool matchRB = true;
        bool matchLT = true;
        bool matchLB = true;
        Vector2[] predictableRT = new Vector2[2];
        Vector2[] predictableRB = new Vector2[2];
        Vector2[] predictableLT = new Vector2[2];
        Vector2[] predictableLB = new Vector2[2];

        predictableRT[0] = position;
        predictableRT[1] = position + size;

        predictableRB[0] = new Vector2(position.x, position.y - size.y);
        predictableRB[1] = new Vector2(position.x + size.x, position.y);

        predictableLT[0] = new Vector2(position.x - size.x, position.y);
        predictableLT[1] = new Vector2(position.x, position.y + size.y);

        predictableLB[0] = position - size;
        predictableLB[1] = position;

        foreach (Room room in roomList)
        {
            if (room.roomColision(predictableRT[0][0], predictableRT[0][1], predictableRT[1][0], predictableRT[1][1]))
            {
                matchRT = false;
            }
            if (room.roomColision(predictableRB[0][0], predictableRB[0][1], predictableRB[1][0], predictableRB[1][1]))
                matchRB = false;
            if (room.roomColision(predictableLT[0][0], predictableLT[0][1], predictableLT[1][0], predictableLT[1][1]))
                matchLT = false;
            if (room.roomColision(predictableLB[0][0], predictableLB[0][1], predictableLB[1][0], predictableLB[1][1]))
                matchLB = false;
        }

        if (matchRT)
            return 1;
        if (matchRB)
            return 2;
        if (matchLT)
            return 3;
        if (matchLB)
            return 4;
        return 0;
    }

    
    int counter = 0;
    Room createRoom(Vector3 position, Vector2 size, int rotation,int neighboursId)
    {
        Room newRoom = null;
        switch (rotation)
        {
            case 1:
                {
                    newRoom = new Room(position.x, position.y, position.x + size.x, position.y + size.y, counter);
                    break;
                }
            case 2:
                {
                    newRoom = new Room(position.x, position.y - size.y, position.x + size.x, position.y, counter);
                    break;
                }
            case 3:
                {
                    newRoom = new Room(position.x - size.x, position.y, position.x, position.y + size.y, counter);
                    break;
                }
            case 4:
                {
                    newRoom = new Room(position.x - size.x, position.y - size.y, position.x, position.y, counter);
                    break;
                }
            default:
                break;
        }
        if (neighboursId != -1)
        {
            switch ((int)position.z)  // z odpowiada zastrone sasiada 1 - prawa, 2 - lewa, 3 - gora, 4 - dol    //niepotrzbne, nie dzialalo
            {
                case 1:
                    {
                        newRoom.addLeftNeighbours(roomList[neighboursId]);
                        roomList[neighboursId].addRightNeighbours(newRoom);
                        break;
                    }
                case 2:
                    {
                        newRoom.addRightNeighbours(roomList[neighboursId]);
                        roomList[neighboursId].addLeftNeighbours(newRoom);
                        break;
                    }
                case 3:
                    {
                        newRoom.addBottomNeighbours(roomList[neighboursId]);
                        roomList[neighboursId].addTopNeighbours(newRoom);
                        break;
                    }
                case 4:
                    {
                        newRoom.addTopNeighbours(roomList[neighboursId]);
                        roomList[neighboursId].addBottomNeighbours(newRoom);
                        break;
                    }
                default:
                    break;
            }
        }
        roomList.Add(newRoom);
        naSasiadow(newRoom);
        counter++;
        return newRoom;
    }

   void naSasiadow(Room room)  //dodaje do listy punkty przy brzegach pomieszzeenia
    {

        List<Vector3> pointList = new List<Vector3>();  // z odpowiada zastrone sasiada 1 - prawa, 2 - lewa, 3 - gora, 4 - dol
        for (int j = room.minX + 3; j < room.maxX - 3; j++)
        {
            pointList.Add(new Vector3(j, room.minY - 1 , 4));
            pointList.Add(new Vector3(j, room.maxY + 1 , 3));
        }

        for (int j = room.minY + 3; j < room.maxY - 3; j++)
        {
            pointList.Add(new Vector3(room.minX - 1, j , 2));
            pointList.Add(new Vector3(room.maxX + 1, j , 1));
        }


        List<Vector3> pointList2 = new List<Vector3>();
        while (pointList.Count != 0)
        {
            int randomPosition = (int)(Random.value * (pointList.Count - 1));
            if (randomPosition > pointList.Count - 1 || pointList.Count < 0)
            {
                Debug.Log("blad w mieszaniu punktow");
            }
            pointList2.Add(pointList[randomPosition]);
            pointList.RemoveAt(randomPosition);
        }

        listaPrzyBokach.Add(pointList2);
    }




    public void DrawAll()
    {
        foreach (Room room in roomList)
        {
            room.draw();
        }
    }
    public void ClearAll()
    {
        foreach (Room room in roomList)
        {
            room.clear();
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
}
