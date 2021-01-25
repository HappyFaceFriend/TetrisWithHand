using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static Vector2 spawnCoord;
    int [,] map;
    const int height = 20;
    const int width = 10;
    public GameObject cube;
    // Start is called before the first frame update
    void Awake()
    {
        map = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j=0; j<width; j++)
            {
                map[i, j] = 0;
                Instantiate(cube, PosOf(j, i) + new Vector3(0,0,0.6f), Quaternion.identity);
            }
        }
    }
    public Vector3 PosOf(int x, int y)
    {
        return new Vector3((x) * Block.size, (y) * Block.size, 0) + transform.position;
    }
    public Vector2Int PosToPoint(Vector3 position)
    {
        Vector3 point = (position - transform.position) / Block.size;
        return new Vector2Int((int)point.x, (int)point.y);
    }
}
