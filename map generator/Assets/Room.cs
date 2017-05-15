using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room{

    public static int sizeMinX = 15;
    public static int sizeMinY = 15;
    public static int sizeMaxX = 50;
    public static int sizeMaxY = 50;
    public static int doorSize = 2;
    public static int pathCount = 7;
    public static int modelRoomCount = 50;

    static GameObject pokoj = Resources.Load("pokoj") as GameObject;
    


    public int index;
    public int leftX, bottomY, rightX, topY;
    public List<Room> leftNeighbours = new List<Room>();
    public List<Room> topNeighbours = new List<Room>();
    public List<Room> rightNeighbours = new List<Room>();
    public List<Room> bottomNeighbours = new List<Room>();

    List<Vector2> pointsForNeighborsLT = new List<Vector2>();
    List<Vector2> pointsForNeighborsLB = new List<Vector2>();
    List<Vector2> pointsForNeighborsRT = new List<Vector2>();
    List<Vector2> pointsForNeighborsRB = new List<Vector2>();

    public List<Path> pathList = new List<Path>();

    List<GameObject> drawObjectList = new List<GameObject>();



    public Room(int leftX, int bottomY, int rightX, int topY,int index)
    {
        this.leftX = leftX;
        this.bottomY = bottomY;
        this.rightX = rightX;
        this.topY = topY;
        this.index = index;
        pathList.Add(new Path(this));

        for (int x = leftX + doorSize+1; x < rightX - doorSize-1; x++)
        {
            pointsForNeighborsLT.Add(new Vector2(x, bottomY-1));
            pointsForNeighborsRT.Add(new Vector2(x, bottomY-1));
            pointsForNeighborsLB.Add(new Vector2(x, topY+1));
            pointsForNeighborsRB.Add(new Vector2(x, topY+1));
        }
        for (int y = bottomY + doorSize+1; y < topY - doorSize-1; y++)
        {
            pointsForNeighborsLT.Add(new Vector2(rightX+1, y));
            pointsForNeighborsRT.Add(new Vector2(leftX-1, y));
            pointsForNeighborsLB.Add(new Vector2(rightX + 1, y));
            pointsForNeighborsRB.Add(new Vector2(leftX - 1, y));
        }

    }
    public Room(float leftX, float bottomY, float rightX, float topY, int index)
    {
        this.leftX = (int)leftX;
        this.bottomY = (int)bottomY;
        this.rightX = (int)rightX;
        this.topY = (int)topY;
        this.index = index;
        pathList.Add(new Path(this));


        for (int x = this.leftX + doorSize+1; x < rightX - doorSize-1; x++)
        {
            pointsForNeighborsLT.Add(new Vector2(x, bottomY - 1));
            pointsForNeighborsRT.Add(new Vector2(x, bottomY - 1));
            pointsForNeighborsLB.Add(new Vector2(x, topY + 1));
            pointsForNeighborsRB.Add(new Vector2(x, topY + 1));
        }
        for (int y = this.bottomY + doorSize+1; y < topY - doorSize-1; y++)
        {
            pointsForNeighborsLT.Add(new Vector2(rightX + 1, y));
            pointsForNeighborsLB.Add(new Vector2(rightX + 1, y));
            pointsForNeighborsRT.Add(new Vector2(leftX - 1, y));
            pointsForNeighborsRB.Add(new Vector2(leftX - 1, y));
        }
    }

    public static Room CreateModelRoom(float leftX, float bottomY, float rightX, float topY, List<Room> ModelRoomList)
    {
        foreach (Room room in ModelRoomList)
        {
            if (room.Colision(leftX, bottomY, rightX, topY))
                return null;
        }
        Room newModelRoom = new Room(leftX, bottomY, rightX, topY, ModelRoomList.Count);
        ModelRoomList.Add(newModelRoom);
        return newModelRoom;
    }

    public static Room createModelRoom(List<Room> ModelRoomList)
    {
        Vector2 roomSize = randomSize(sizeMinX,sizeMaxX,sizeMinY,sizeMaxY);
        if (ModelRoomList.Count==0)
        {
            return CreateModelRoom(0,0, roomSize.x, roomSize.y, ModelRoomList);
        }

        Room newModelRoom;
        foreach (Room item in ModelRoomList)
        {
            newModelRoom = item.createNeighbor(roomSize, ModelRoomList);
            if (newModelRoom != null)
                return newModelRoom;
        }
        return null;
    }

    public Room createNeighbor(Vector2 roomSize, List<Room> ModelRoomList)
    {
        
        List<Vector2> copyPointsForNeighborsLT = pointsForNeighborsLT;
        List<Vector2> copyPointsForNeighborsLB = pointsForNeighborsLB;
        List<Vector2> copyPointsForNeighborsRT = pointsForNeighborsRT;
        List<Vector2> copyPointsForNeighborsRB = pointsForNeighborsRB;
        int count = copyPointsForNeighborsLT.Count + copyPointsForNeighborsLB.Count + copyPointsForNeighborsRT.Count + copyPointsForNeighborsRB.Count;
        while (count!=0)
        {
            int rnd = Random.Range(0, count-1);
            Room newModelRoom;
            if (rnd < copyPointsForNeighborsLT.Count)
            {
                newModelRoom = CreateModelRoom(copyPointsForNeighborsLT[rnd].x, copyPointsForNeighborsLT[rnd].y-roomSize.y, copyPointsForNeighborsLT[rnd].x+roomSize.x, copyPointsForNeighborsLT[rnd].y, ModelRoomList);
                if (newModelRoom != null)
                    return newModelRoom;
                copyPointsForNeighborsLT.RemoveAt(rnd);
                count = copyPointsForNeighborsLT.Count + copyPointsForNeighborsLB.Count + copyPointsForNeighborsRT.Count + copyPointsForNeighborsRB.Count;
                continue;

            }

            rnd -= copyPointsForNeighborsLT.Count;
            if (rnd < copyPointsForNeighborsLB.Count)
            {
                newModelRoom = CreateModelRoom(copyPointsForNeighborsLB[rnd].x, copyPointsForNeighborsLB[rnd].y, copyPointsForNeighborsLB[rnd].x+ roomSize.x, copyPointsForNeighborsLB[rnd].y+ roomSize.y, ModelRoomList);
                if (newModelRoom != null)
                    return newModelRoom;
                copyPointsForNeighborsLB.RemoveAt(rnd);
                count = copyPointsForNeighborsLT.Count + copyPointsForNeighborsLB.Count + copyPointsForNeighborsRT.Count + copyPointsForNeighborsRB.Count;
                continue;
            }

            rnd -= copyPointsForNeighborsLB.Count;
            if (rnd < copyPointsForNeighborsRT.Count)
            {
                newModelRoom = CreateModelRoom(copyPointsForNeighborsRT[rnd].x- roomSize.x, copyPointsForNeighborsRT[rnd].y- roomSize.y, copyPointsForNeighborsRT[rnd].x, copyPointsForNeighborsRT[rnd].y, ModelRoomList);
                if (newModelRoom != null)
                    return newModelRoom;
                copyPointsForNeighborsRT.RemoveAt(rnd);
                count = copyPointsForNeighborsLT.Count + copyPointsForNeighborsLB.Count + copyPointsForNeighborsRT.Count + copyPointsForNeighborsRB.Count;
                continue;
            }

            rnd -= copyPointsForNeighborsRT.Count;
            if (rnd < copyPointsForNeighborsRB.Count)
            {
                newModelRoom = CreateModelRoom(copyPointsForNeighborsRB[rnd].x- roomSize.x, copyPointsForNeighborsRB[rnd].y, copyPointsForNeighborsRB[rnd].x, copyPointsForNeighborsRB[rnd].y+ roomSize.y, ModelRoomList);
                if (newModelRoom != null)
                    return newModelRoom;
                copyPointsForNeighborsRB.RemoveAt(rnd);
                count = copyPointsForNeighborsLT.Count + copyPointsForNeighborsLB.Count + copyPointsForNeighborsRT.Count + copyPointsForNeighborsRB.Count;
                continue;
            }
        }
        //clearPointsForNeighbors();   //jednak niepotrzebne
        return null;
    }

    void clearPointsForNeighbors(List<Room> ModelRoomList)
    {
        List<Vector2> pointsForNeighborsLT = new List<Vector2>();
        List<Vector2> pointsForNeighborsLB = new List<Vector2>();
        List<Vector2> pointsForNeighborsRT = new List<Vector2>();
        List<Vector2> pointsForNeighborsRB = new List<Vector2>();
        for (int i = 0; i < pointsForNeighborsLT.Count; i++)
        {
            foreach (Room room in ModelRoomList)
            {
                if (room.Colision(pointsForNeighborsLT[i].x, pointsForNeighborsLT[i].y - sizeMinY, pointsForNeighborsLT[i].x + sizeMinX, pointsForNeighborsLT[i].y))
                {
                    pointsForNeighborsLT.RemoveAt(i);
                    i--;
                    break;
                }
            }

        }

        for (int i = 0; i < pointsForNeighborsLB.Count; i++)
        {
            foreach (Room room in ModelRoomList)
            {
                if (room.Colision(pointsForNeighborsLB[i].x, pointsForNeighborsLB[i].y, pointsForNeighborsLB[i].x + sizeMinX, pointsForNeighborsLB[i].y + sizeMinY))
                {
                    pointsForNeighborsLT.RemoveAt(i);
                    i--;
                    break;
                }
            }

        }

        for (int i = 0; i < pointsForNeighborsRT.Count; i++)
        {
            foreach (Room room in ModelRoomList)
            {
                if (room.Colision(pointsForNeighborsRT[i].x - sizeMinX, pointsForNeighborsRT[i].y - sizeMinY, pointsForNeighborsRT[i].x, pointsForNeighborsRT[i].y))
                {
                    pointsForNeighborsLT.RemoveAt(i);
                    i--;
                    break;
                }
            }

        }

        for (int i = 0; i < pointsForNeighborsRB.Count; i++)
        {
            foreach (Room room in ModelRoomList)
            {
                if (room.Colision(pointsForNeighborsRB[i].x - sizeMinX, pointsForNeighborsRB[i].y, pointsForNeighborsRB[i].x, pointsForNeighborsRB[i].y + sizeMinY))
                {
                    pointsForNeighborsLT.RemoveAt(i);
                    i--;
                    break;
                }
            }

        }
    }

    public void findAllPath()  
    {
        for (int i = 0; i < pathList.Count; i++)
        {
            foreach (Room room in pathList[i].target.leftNeighbours)
            {
                addPath(room, pathList[i].target, pathList[i].roomsBetween);
            }
            foreach (Room room in pathList[i].target.rightNeighbours)
            {
                addPath(room, pathList[i].target, pathList[i].roomsBetween);
            }
            foreach (Room room in pathList[i].target.topNeighbours)
            {
                addPath(room, pathList[i].target, pathList[i].roomsBetween);
            }
            foreach (Room room in pathList[i].target.bottomNeighbours)
            {
                addPath(room, pathList[i].target, pathList[i].roomsBetween);
            }
        }
    }

    void addPath(Room room, Room roomBetween=null, List<Room> listRoomsBetween=null)
    {
        if (roomBetween == this)
            roomBetween = null;
        List<Room> list;
        for (int i = 0; i < pathList.Count; i++)           
        {
            if (pathList[i].target == room)
            {
                int length = 0;
                if (roomBetween != null)
                    length++;
                if (listRoomsBetween != null)
                    length += listRoomsBetween.Count;
                    if (pathList[i].length() > length)
                {
                    list = new List<Room>();
                    if (listRoomsBetween != null)
                        foreach (Room item in listRoomsBetween)
                        {
                            list.Add(item);
                        }
                    if (roomBetween != null)
                        list.Add(roomBetween);
                    
                    pathList[i].roomsBetween = list;
                }
                return;
            }
        }

        list = new List<Room>();
        if (listRoomsBetween != null)
            foreach (Room item in listRoomsBetween)
            {
                list.Add(item);
            }
        if (roomBetween != null)
            list.Add(roomBetween);

        pathList.Add(new Path(room,list));

    }

    public void findNeighbours(List<Room> ModelRoomList)
    {
        foreach (Room room in ModelRoomList)
        {
            if(room.Colision(leftX + doorSize + 1, bottomY - 1, rightX - doorSize - 1, bottomY - 1))
            {
                addBottomNeighbours(room);
            }
            if (room.Colision(leftX + doorSize + 1, topY + 1, rightX - doorSize - 1, topY + 1))
            {
                addTopNeighbours(room);
            }


            if (room.Colision(leftX -1, bottomY + doorSize + 1, leftX - 1, topY - doorSize - 1))
            {
                addLeftNeighbours(room);
            }
            if (room.Colision(rightX + 1, bottomY + doorSize + 1, rightX + 1, topY - doorSize - 1))
            {
                addRightNeighbours(room);
            }
        }

    }
    
    

    public void addLeftNeighbours(Room room)
    {
        if (!leftNeighbours.Contains(room))
        {
            leftNeighbours.Add(room);
        }
    }
    public void addBottomNeighbours(Room room)
    {
        if (!bottomNeighbours.Contains(room))
            bottomNeighbours.Add(room);
    }
    public void addTopNeighbours(Room room)
    {
        if (!topNeighbours.Contains(room))
            topNeighbours.Add(room);
    }
    public void addRightNeighbours(Room room)
    {
        if (!rightNeighbours.Contains(room))
            rightNeighbours.Add(room);
    }

    public int isNeighbour(Room room)
    {
        if (rightNeighbours.Contains(room))
            return 1;
        if (topNeighbours.Contains(room))
            return 3;
        if (bottomNeighbours.Contains(room))
            return 4;
        if (leftNeighbours.Contains(room))
            return 2;
        return 0;
    }

    public void draw() //only to test
    {
        clear();

        GameObject tmp;

        for (int x = leftX; x <= rightX; x++)
        {

            for (int y = bottomY; y <= topY; y++)
            {

                tmp = MonoBehaviour.Instantiate(pokoj);
                tmp.transform.position = new Vector2(x, y);
                tmp.GetComponent<GUIText>().text = text();
                Color tmpColor = new Color((float)index / 100, (float)index / 100, (float)index / 100);
                tmp.GetComponent<SpriteRenderer>().color = tmpColor;
                drawObjectList.Add(tmp);
            }
        }
    }



    public bool Colision(Room room)
    {
        bool collisionX = false;
        bool collisionY = false;

        if (room.leftX < leftX)
        {
            if (room.rightX >= leftX)
            {
                collisionX = true;
            }
        }
        else if (room.leftX <= rightX)
        {
            collisionX = true;
        }


        if (room.bottomY < bottomY)
        {
            if (room.topY >= bottomY)
            {
                collisionY = true;
            }
        }
        else if (room.bottomY <= topY)
        {
            collisionY = true;
        }
        if (collisionX && collisionY)
        {
            return true;
        }
        return false;

    }
    public bool Colision(float leftX, float bottomY, float rightX, float topY)
    {
        bool collisionX = false;
        bool collisionY = false;

        if (leftX < this.leftX)
        {
            if (rightX >= this.leftX)
            {
                collisionX = true;
            }
        }
        else if (leftX <= this.rightX)
        {
            collisionX = true;
        }


        if (bottomY < this.bottomY)
        {
            if (topY >= this.bottomY)
            {
                collisionY = true;
            }
        }
        else if (bottomY <= this.topY)
        {
            collisionY = true;
        }
        if (collisionX && collisionY)
        {
            return true;
        }
        return false;

    }
    


    string text() //only to test
    {
        string text = "nr: " + index+ " sasiedzi: L: ";
        foreach (Room room in leftNeighbours)
        {
            text += room.index + " ";
        }
        text += "R: ";
        foreach (Room room in rightNeighbours)
        {
            text += room.index + " ";
        }
        text += "T: ";
        foreach (Room room in topNeighbours)
        {
            text += room.index + " ";
        }
        text += "B: ";
        foreach (Room room in bottomNeighbours)
        {
            text += room.index + " ";
        }
        return text;
    }

    public void clear() //only to test
    {
        foreach (GameObject item in drawObjectList)
        {
            MonoBehaviour.Destroy(item);
        }
        drawObjectList.Clear();
    }


    public string pathsNaString()    // to test
    {
        string tmp = "" + index + ":  ";
        foreach (Path path in pathList)
        {
            tmp += path.target.index + "-" + path.length() + "  ; ";
        }
        return tmp;
    }

    
        static Vector2 randomSize(int leftX, int rightX, int bottomY, int topY)
    {
        Vector2 size;
        float radius;
        do
        {
            radius = (rightX - leftX / 2);
            size.x = (int)(NextGaussianDouble() * radius + leftX + radius);
        } while (size.x > rightX || size.x < leftX);
        do
        {
            radius = (topY - bottomY / 2);
            size.y = (int)(NextGaussianDouble() * radius + bottomY + radius);
        } while (size.y > topY || size.y < bottomY);
        return size;

    }

    static float NextGaussianDouble()
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













public class Path
{
    public List<Room> roomsBetween;
    public Room target;
    public Path(Room room, List<Room> list=null)
    {
        this.target = room;
        if (list != null)
        {
            roomsBetween = list;
        }
        else
        {
            roomsBetween = new List<Room>();
        }
    }
    public int length()
    {
        if (roomsBetween != null)
            return roomsBetween.Count + 1;
        else return 1;
    }
    public string texttt() //only to test
    {
        string tmp  = "do " +target.index + " podrodze: ";
        for (int i = 0; i < roomsBetween.Count; i++)
        {
            tmp += roomsBetween[i].index + " ,  ";
        }
        return tmp;
    }

}