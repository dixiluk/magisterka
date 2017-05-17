using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelPath
{
    public List<ModelRoom> ModelRoomsBetween;
    public ModelRoom target;
    public ModelPath(ModelRoom ModelRoom, List<ModelRoom> list = null)
    {
        this.target = ModelRoom;
        if (list != null)
        {
            ModelRoomsBetween = list;
        }
        else
        {
            ModelRoomsBetween = new List<ModelRoom>();
        }
    }
    public int length()
    {
        if (ModelRoomsBetween != null)
            return ModelRoomsBetween.Count + 1;
        else return 1;
    }
    public string texttt() //only to test
    {
        string tmp = "do " + target.index + " podrodze: ";
        for (int i = 0; i < ModelRoomsBetween.Count; i++)
        {
            tmp += ModelRoomsBetween[i].index + " ,  ";
        }
        return tmp;
    }

}
