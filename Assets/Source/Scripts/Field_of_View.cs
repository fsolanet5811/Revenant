using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field_of_View : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    [SerializeField] private Vector3 origin;
    private float startingAngle = 0f;
    [SerializeField] private float fov;
    private float startingFov = 45f;
    [SerializeField] private float viewDistance;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = this.transform.position;
        fov = startingFov;
        viewDistance = 5f;

    }

    // Update is called once per frame
    void LateUpdate()
    {

        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        viewDistance = 5f;
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);

            if (raycastHit2D.collider == null)
            {
                //No Hit
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                //Hit
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        //angle = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    public static float GetAngleFromPlayer(PlayerController dir, int fov)
    {
        float lastDirection = 0f;
        float x = Mathf.Acos(dir.lookHorizontal) * Mathf.Rad2Deg + fov;
        float y = Mathf.Asin(dir.lookVertical) * Mathf.Rad2Deg + fov;
        if (Mathf.Abs(dir.lookHorizontal) > 0)
        {
            lastDirection = x;
            return x;
        }
        else if (Mathf.Abs(dir.lookVertical) > 0)
        {
            lastDirection = y;
            return y;
        }
        else
        {
            return lastDirection;
        }

    }


    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(PlayerController aimDirection)
    {
        startingAngle = GetAngleFromPlayer(aimDirection, (int)fov) - fov / 2f;
    }

    public IEnumerator IncreaseFovCoRoutine(float newFov, float timeInSec) //CoRoutine is great for things that happen over a period of time
    {
        fov = newFov;
        yield return new WaitForSeconds(timeInSec);
        fov = startingFov;
    }
}
