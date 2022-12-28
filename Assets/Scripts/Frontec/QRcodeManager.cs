using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class QRcodeManager : MonoBehaviour
{
    [SerializeField] private Texture _texture = null;

    List<Component> componentList = new List<Component>();


    // Start is called before the first frame update
    void Start()
    {
        ShowQRCode();
        Component[] components = gameObject.GetComponents(typeof(Component));
        for (int i = 0; i < components.Length; i++)
        {
            componentList.Add(components[i]);
        }

        if (componentList != null)
            Debug.Log(componentList[componentList.Count - 1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowQRCode()
    {
        Vector3[] vertices = new Vector3[]
{
            new Vector3(-1f, 1f, -1f),
            new Vector3(1f, 1f, -1f),
            new Vector3(1f, -1f, -1f),
            new Vector3(-1f, -1f, -1f),
};

        int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        Mesh mesh = new Mesh();
        Vector2[] uvs = new Vector2[] {
            new Vector2(0f, 1f),
            new Vector2(1f,1f),
            new Vector2(1f, 0f),
            new Vector2(0f,0f),
           };


        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
 
      // Material material = new Material(Shader.Find("Standard"));
      // material.SetTexture("_MainTex", _texture);
      // GetComponent<MeshRenderer>().material = material;
        GetComponent<MeshRenderer>().receiveShadows = false; // 그림자 받지 않도록 
    }





}


