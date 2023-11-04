using UnityEngine;

public class TorusGenerator : MonoBehaviour
{
    public float radius = 2.0f;       // Radius of the torus
    public float tubeRadius = 0.3f;  // Radius of the tube

    private MeshFilter meshFilter;
    private Mesh mesh;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        GenerateTorus();
    }

    private void Update()
    {
        SetTorusDimensions(radius, tubeRadius);
    }

    private void GenerateTorus()
    {
        // Rotate the torus by 90 degrees around the X axis to make it lie flat
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        int segments = 64; // Increase the number of segments for smoother appearance
        int numVertices = (segments + 1) * (segments + 1);
        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];
        int[] triangles = new int[segments * segments * 6];

        float twoPi = 2 * Mathf.PI;

        for (int i = 0; i <= segments; i++)
        {
            float u = (float)i / segments;
            float theta = u * twoPi;

            for (int j = 0; j <= segments; j++)
            {
                float v = (float)j / segments;
                float phi = v * twoPi;

                float x = (radius + tubeRadius * Mathf.Cos(phi)) * Mathf.Cos(theta);
                float y = (radius + tubeRadius * Mathf.Cos(phi)) * Mathf.Sin(theta);
                float z = tubeRadius * Mathf.Sin(phi);

                vertices[i * (segments + 1) + j] = new Vector3(x, y, z);
                uv[i * (segments + 1) + j] = new Vector2(u, v);
            }
        }

        int index = 0;
        for (int i = 0; i < segments; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                int a = i * (segments + 1) + j;
                int b = a + 1;
                int c = (i + 1) * (segments + 1) + j;
                int d = c + 1;

                triangles[index++] = a;
                triangles[index++] = c;
                triangles[index++] = b;
                triangles[index++] = b;
                triangles[index++] = c;
                triangles[index++] = d;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private void SetTorusDimensions(float newRadius, float newTubeRadius)
    {
        radius = newRadius;
        tubeRadius = newTubeRadius;
        GenerateTorus(); // Regenerate the torus mesh with the updated dimensions
    }
}
