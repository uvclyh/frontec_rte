using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noah;

namespace Noah
{

    [Serializable]
    public class Edge
    {
        public string serialNum;
    }
    [Serializable]
    public class NetworkConfig
    {
        public string port;
        public int baudRate;
        public string parity;
        public int stopBit;
        public string ip;
    }
    [Serializable]
    public class Profile
    {
        public string name;
        public List<Request> requests;
        public string connection_type;
        public string protocol;
    }
    [Serializable]
    public class Record
    {
        public int id;
        public string name;
        public object model;
        public string manufacturer;
        public string location;
        public int edgeId;
        public int profileId;
        public string connectionType;
        public NetworkConfig networkConfig;
        public object imgPath;
        public string description;
        public int? logicalStationNumber;
        public object deployModel;
        public int userId;
        public int groupId;
        public int dataCollectHistoryId;
        public SecurityCertification securityCertification;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int EdgeId;
        public int ProfileId;
        public Profile Profile;
        public Edge Edge;
    }
    [Serializable]
    public class Request
    {
        public object tagId;
        public string name;
        public string korName;
        public string memoryType;
        public string memoryAddr;
    }
    [Serializable]
    public class SecurityCertification
    {
    }

    [Serializable]
    public class AddressSpaceData : ILoader<string, Profile>
    {
        public int status;
        public string code;
        public string message;
        public List<Record> record;
        public object remark;

        public Dictionary<string, Profile> MakeDic()
        {
            Dictionary<string, Profile> dict = new Dictionary<string, Profile>();
            foreach (Record rcd in record )
                dict.Add(rcd.Profile.name, rcd.Profile);
            return dict;
        }

        public bool Validate()
        {
            throw new NotImplementedException();
        }
        
    }



}