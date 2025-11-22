using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    public List<Node> currentPath;

    public void SetPath(List<Node> path)
    {
        currentPath = path;
    }

    void OnDrawGizmos()
    {
        if (currentPath == null) return;

        Gizmos.color = Color.yellow;
        foreach (var n in currentPath)
        {
            Gizmos.DrawCube(n.worldPos, Vector3.one * 0.1f);
        }
    }
}
