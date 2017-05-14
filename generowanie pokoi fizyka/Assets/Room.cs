using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

    static GameObject pokoj = Resources.Load("pokoj") as GameObject;

    public int index;

    public int minX, minY, maxX, maxY;
    public List<Room> leftNeighbours = new List<Room>();
    public List<Room> topNeighbours = new List<Room>();
    public List<Room> rightNeighbours = new List<Room>();
    public List<Room> bottomNeighbours = new List<Room>();

    public List<Path> pathList = new List<Path>();

    List<GameObject> drawObjectList = new List<GameObject>();

    public Room(int minX, int minY, int maxX, int maxY,int index)
    {
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
        this.index = index;
        pathList.Add(new Path(this));
    }
    public Room(float minX, float minY, float maxX, float maxY, int index)
    {
        this.minX = (int)minX;
        this.minY = (int)minY;
        this.maxX = (int)maxX;
        this.maxY = (int)maxY;
        this.index = index;
        pathList.Add(new Path(this));
    }

    public string pathsNaString()    // to test
    {
        string tmp = ""+index+":  ";
        foreach (Path path in pathList)
        {
            tmp += path.target.index + "-" + path.length() + "  ; ";
        }
        return tmp;
    }

    public void findAllPath()   //zmienic nazwe
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

        for (int x = minX; x <= maxX; x++)
        {

            for (int y = minY; y <= maxY; y++)
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

    public bool pointInRoom(Vector2 point)
    {
        if (minX <= point.x && point.x <= maxX)
            if (minY <= point.y && point.y <= maxY)
                return true;
        return false;
    }


    public bool roomColision(Room room)
    {
        bool collisionX = false;
        bool collisionY = false;

        if (room.minX < minX)
        {
            if (room.maxX >= minX)
            {
                collisionX = true;
            }
        }
        else if (room.minX <= maxX)
        {
            collisionX = true;
        }


        if (room.minY < minY)
        {
            if (room.maxY >= minY)
            {
                collisionY = true;
            }
        }
        else if (room.minY <= maxY)
        {
            collisionY = true;
        }
        if (collisionX && collisionY)
        {
            return true;
        }
        return false;

    }
    public bool roomColision(float minX, float minY, float maxX, float maxY)
    {
        bool collisionX = false;
        bool collisionY = false;

        if (minX < this.minX)
        {
            if (maxX >= this.minX)
            {
                collisionX = true;
            }
        }
        else if (minX <= this.maxX)
        {
            collisionX = true;
        }


        if (minY < this.minY)
        {
            if (maxY >= this.minY)
            {
                collisionY = true;
            }
        }
        else if (minY <= this.maxY)
        {
            collisionY = true;
        }
        if (collisionX && collisionY)
        {
            return true;
        }
        return false;

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