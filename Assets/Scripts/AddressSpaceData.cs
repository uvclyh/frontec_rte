using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noah;


[Serializable]
public class Record
{
    public int id;
    public string edgeName;
    public List<Message> message;
}

[Serializable]
public class Message
{
    public string tagId;
    public string name;
    public int value;
}


[Serializable]
public class AddressSpaceData : ILoader<int, Record>
{
    public int status;
    public string code;
    public string message;
    public List<Record> record;
    public object remark;

    public Dictionary<int, Record> MakeDic()
    {
        Dictionary<int, Record> dict = new Dictionary<int, Record>();
        foreach  (Record rcd in record)
            dict.Add(rcd.id, rcd);
        return dict;
    }

    public bool Validate()
    {
        throw new NotImplementedException();
    }
}


