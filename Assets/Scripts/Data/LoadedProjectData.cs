using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noah;


namespace Noah
{
    [Serializable]
    public class LoadedProjectData
    {
        public int id;
        public string imageName;
        public string name;
        public string description;
    }

    [Serializable]
    public class LoadedProjectDataResponse
    {
        public int status;
        public string code;
        public string message;
        public List<LoadedProjectData> record;
        public object remark;
    }

}

