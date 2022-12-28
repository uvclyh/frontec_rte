using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noah;
using System;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering;

namespace Noah
{

    [Serializable]
    public class VectorInfo
    {
        public float x;
        public float y;
        public float z;

        public VectorInfo(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [Serializable]
    public class EditedObject
    {
        public int id;
        public VectorInfo position;
        public VectorInfo rotation;
        public VectorInfo scale;
        public string content;

        public EditedObject(int id, VectorInfo position, VectorInfo rotation, VectorInfo scale, string content)
        {
            this.id = id;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.content = content;
        }
    }

    [Serializable]
    public class Project
    {
        
        public string Name;
        public string projectType;
        public string Date;
        public byte[] pngImage;
        public List<EditedObject> EditedObjects;

        public Project(string name, string projectType, string date, byte[] pngImage, List<EditedObject> EditedObjects)
        {
            Name = name;
            this.projectType = projectType;
            Date = date;
            this.pngImage = pngImage;
            this.EditedObjects = EditedObjects;
        }
    }

    [Serializable]
    public class ProjectData : ILoader<string, Project>
    {

        public List<Project> projects;

        public ProjectData(List<Project> projects)
        {
            this.projects = projects;
        }

        public Dictionary<string, Project> MakeDic()
        {
            Dictionary<string, Project> dict = new Dictionary<string, Project>();

            foreach (Project project in projects)
                dict.Add(project.Name, project);

            return dict;
        }

        public bool Validate()
        {
            throw new System.NotImplementedException();
        }
    }

}
