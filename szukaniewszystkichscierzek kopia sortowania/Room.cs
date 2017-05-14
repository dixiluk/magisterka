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

    public string sciezkiNaString()
    {
        string tmp = ""+index+":  ";
        foreach (Path path in pathList)
        {
            tmp += path.target.index + "-" + path.length() + "  ; ";
        }
        return tmp;
    }

    public void FloydWarshall()
    {
        for (int i = 0; i < pathList.Count; i++)
        {
            foreach (Path path2 in pathList[i].target.pathList)
            {
                addPath(path2.target, pathList[i].length()+path2.length(), pathList[i].target, pathList[i].roomsBetween, path2.roomsBetween);
            }
        }
    }

    void addPath(Room room, int length, Room roomBetween=null, List<Room> list1=null, List<Room> list2=null)
    {
        List<Room> list;
        for (int i = 0; i < pathList.Count; i++)                        //do optymalizacji, nigdy nie znajdzie krotszej, tylko nowa, // wystarczy jechac tylko po sasiadach
        {
            if (pathList[i].target == room)
            {
                if (pathList[i].length() > length)
                {
                    list = new List<Room>();
                    if (list1!=null)
                        foreach (Room item in list1)
                        {
                            list.Add(item);
                        }
                    if (roomBetween != null)
                        list.Add(roomBetween);
                    if (list2 != null)
                        foreach (Room item in list2)
                        {
                            list.Add(item);
                        }
                    
                    pathList[i].roomsBetween = list;
                }
                return;
            }
        }

        list = new List<Room>();
        if (list1 != null)
            foreach (Room item in list1)
            {
                list.Add(item);
            }
        if (roomBetween != null)
            list.Add(roomBetween);
        if (list2 != null)
            foreach (Room item in list2)
            {
                list.Add(item);
            }

        pathList.Add(new Path(room,list));

    }

    public void addLeftNeighbours(Room room)
    {
        if (!leftNeighbours.Contains(room))
        {
            leftNeighbours.Add(room);
        }
        addPath(room, 1);
    }
    public void addBottomNeighbours(Room room)
    {
        if (!bottomNeighbours.Contains(room))
            bottomNeighbours.Add(room);
        addPath(room, 1);
    }
    public void addTopNeighbours(Room room)
    {
        if (!topNeighbours.Contains(room))
            topNeighbours.Add(room);
        addPath(room, 1);
    }
    public void addRightNeighbours(Room room)
    {
        if (!rightNeighbours.Contains(room))
            rightNeighbours.Add(room);
        addPath(room, 1);
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

    public void draw()
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

    string text()
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

    public void clear()
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
    public string texttt()
    {
        string tmp  = "do " +target.index + " podrodze: ";
        for (int i = 0; i < roomsBetween.Count; i++)
        {
            tmp += roomsBetween[i].index + " ,  ";
        }
        return tmp;
    }
}