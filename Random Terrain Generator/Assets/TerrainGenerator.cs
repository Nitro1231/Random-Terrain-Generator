using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour {

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    // Map Size
    public int size = 50;

	// Use this for initialization
	void Start () {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        StartCoroutine(createShape());
        //updateMesh();
	}

    void Update() {
        updateMesh();
    }

    IEnumerator createShape() {
        vertices = new Vector3[(size + 1) * (size + 1)];

        // Set all points
        for (int i = 0, z = 0; z <= size; z++) {
            for (int x = 0; x <= size; x++) {
                vertices[i] = new Vector3(x, -1, z);
                i++;
            }
        }

        int vert = 0, tris = 0;
        triangles = new int[size * size * 6];

        // Set a triangle points with a clockwise direction.
        for (int z = 0; z < size; z++) {
            for (int x = 0; x < size; x++) {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + size + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + size + 1;
                triangles[tris + 5] = vert + size + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        // Debug Data for Desmos graphing.
        string debugData = "";

        // Midpoint
        int midpoint = Mathf.RoundToInt(size / 2);
        int rMax = midpoint - 2; 
        int rMin = Mathf.RoundToInt(midpoint / 2);

        // Random size circle generating algorithm.
        for (int i = 0; i < 360; i += 1) {
            // Get radian angle from degree angle.
            float angle = i * (Mathf.PI / 180);
            int r = Mathf.RoundToInt(Random.Range(rMax, rMin));

            // Get x and z point based on radius and angle.
            int x = midpoint + Mathf.RoundToInt(r * Mathf.Cos(angle));
            int z = midpoint + Mathf.RoundToInt(r * Mathf.Sin(angle));

            debugData += "(" + x + ", " + z + ")\n";
            Debug.Log("r: " + r + " | point: ( " + x + ", " + z + ")");

            // Get the corresponding index number based on coordinates that we previously got.
            int index = z * (size + 1) + x;
            vertices[index] = new Vector3(vertices[index].x, 0, vertices[index].z);
            
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log(debugData);
    }

    void updateMesh() {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}