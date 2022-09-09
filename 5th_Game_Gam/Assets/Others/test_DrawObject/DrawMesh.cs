using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMesh : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float blockHalfHeights;
    [SerializeField] private GameObject emptyObjPrefab;

    private List<GameObject> lineList = new List<GameObject>();

    public ClickToGetWorldPos getPosScript;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete) && lineList.Count > 0) Delete();
        if (Input.GetKeyDown(KeyCode.Backspace) && lineList.Count > 0) Undo();

        if (Input.GetMouseButtonUp(0))
        {
            if (getPosScript.ClickPosList.Count <= 1) return;
            lineList.Add(DrawNewLine(getPosScript.ClickPosList.ToArray()));
            getPosScript.ResetClickPos();

            /*
            lineList.Add(DrawNewLine(new Vector3[] {
                new Vector3(3 * Mathf.Sin(1), 3 * Mathf.Cos(1), 0),
                new Vector3(3 * Mathf.Sin(0.5f), 3 * Mathf.Cos(0.5f), 0),
                new Vector3(3 * Mathf.Sin(0), 3 * Mathf.Cos(0), 0),
                new Vector3(3 * Mathf.Sin(-0.5f), 3 * Mathf.Cos(-0.5f), 0),
                new Vector3(3 * Mathf.Sin(-1), 3 * Mathf.Cos(-1), 0)}));
            */
        }
    }

    private GameObject DrawNewLine(Vector3[] posArr)
    {
        if (posArr.Length <= 1) return null;

        GameObject parent = new GameObject("ParentBlock");
        Transform parentTrans = parent.transform;
        for (int i = 0; i < posArr.Length - 1; i++)
        {
            GameObject child = DrawNewBlock(posArr[i], posArr[i + 1]);
            child.transform.SetParent(parentTrans);
        }

        return parent;
    }

    private GameObject DrawNewBlock(Vector3 beginPos, Vector3 endPos)
    {
        GameObject obj = Instantiate(emptyObjPrefab);
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        obj.GetComponent<MeshRenderer>();
        MeshCollider meshCollider = obj.GetComponent<MeshCollider>();

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[8];
        int[] triangles = new int[36];

        // 2点間の角度
        Vector2 dt = beginPos - endPos;
        float radian = Mathf.Atan2(dt.x, dt.y);
        float angle = radian * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        // 頂点座標 (左下,右下,左上,右上,奥左下,奥右下,奥左上,奥右上)
        //t.position = Quaternion.Euler(0, 0, 360 - r) * new Vector3(0, radius, 0);
        //vertices[0] = beginPos + Quaternion.Euler(0, 0, angle) * new Vector3(-blockHalfHeights, 0, 0);
        //vertices[1] = endPos + Quaternion.Euler(0, 0, angle) * new Vector3(-blockHalfHeights, 0, 0);
        //vertices[2] = beginPos + Quaternion.Euler(0, 0, angle) * new Vector3(-blockHalfHeights, 0, 0) + Vector3.up;
        //vertices[3] = endPos + Quaternion.Euler(0, 0, angle) * new Vector3(-blockHalfHeights, 0, 0) + Vector3.up;
        //vertices[4] = beginPos + Quaternion.Euler(0, 0, angle) * new Vector3(-blockHalfHeights, 0, 0) + Vector3.right;
        //vertices[5] = endPos + Quaternion.Euler(0, 0, angle) * new Vector3(-blockHalfHeights, 0, 0) + Vector3.right;
        //vertices[6] = beginPos + Quaternion.Euler(0, 0, angle) * new Vector3(-blockHalfHeights, 0, 0) + new Vector3(0, 1, 1);
        //vertices[7] = endPos + Quaternion.Euler(0, 0, angle) * new Vector3(-blockHalfHeights, 0, 0) + new Vector3(0, 1, 1);

        vertices[0] = beginPos + new Vector3(-blockHalfHeights, 0, 0);
        vertices[1] = endPos + new Vector3(-blockHalfHeights, 0, 0);
        vertices[2] = beginPos + new Vector3(blockHalfHeights, blockHalfHeights, 0);
        vertices[3] = endPos + new Vector3(blockHalfHeights, blockHalfHeights, 0);
        vertices[4] = beginPos + new Vector3(-blockHalfHeights, 0, 1);
        vertices[5] = endPos + new Vector3(-blockHalfHeights, 0, 1);
        vertices[6] = beginPos + new Vector3(blockHalfHeights, blockHalfHeights, 1);
        vertices[7] = endPos + new Vector3(blockHalfHeights, blockHalfHeights, 1);

        mesh.SetVertices(vertices);

        // 頂点配列 (各辺はサイコロの展開図上で各面の左上→右下で区切る)
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;
        triangles[6] = 2;
        triangles[7] = 6;
        triangles[8] = 3;
        triangles[9] = 6;
        triangles[10] = 7;
        triangles[11] = 3;
        triangles[12] = 2;
        triangles[13] = 0;
        triangles[14] = 6;
        triangles[15] = 0;
        triangles[16] = 4;
        triangles[17] = 6;
        triangles[18] = 6;
        triangles[19] = 4;
        triangles[20] = 7;
        triangles[21] = 4;
        triangles[22] = 5;
        triangles[23] = 7;
        triangles[24] = 7;
        triangles[25] = 5;
        triangles[26] = 3;
        triangles[27] = 5;
        triangles[28] = 1;
        triangles[29] = 3;
        triangles[30] = 4;
        triangles[31] = 0;
        triangles[32] = 5;
        triangles[33] = 0;
        triangles[34] = 1;
        triangles[35] = 5;
        mesh.SetTriangles(triangles, 0);

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;

        obj.transform.position = Vector3.zero;

        return obj;
    }

    private void Undo()
    {
        int lastIndex = lineList.Count - 1;
        GameObject lastIndexObj = lineList[lastIndex];
        lineList.RemoveAt(lastIndex);
        Destroy(lastIndexObj);
    }

    private void Delete()
    {
        lineList.ForEach(obj => Destroy(obj));
        lineList.Clear();
        getPosScript.ResetClickPos();
    }
}
