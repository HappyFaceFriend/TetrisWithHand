using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static Vector2 spawnCoord;
    int [,] map;
    Block[,] blockMap;
    public const int height = 20;
    public const int width = 10;
    public GameObject cube;
    // Start is called before the first frame update
    void Awake()
    {
        map = new int[height, width];
        blockMap = new Block[height,width];
        for (int i = 0; i < height; i++)
        {
            for (int j=0; j<width; j++)
            {
                map[i, j] = 0;
                Instantiate(cube, PosOf(j, i) + new Vector3(0,0,0.6f), Quaternion.identity);
            }
        }
    }
    public int ValueAt(int x, int y)
    {
        return map[y, x];
    }
    public int ValueAt(Vector2Int point)
    {
        return ValueAt(point.x,point.y);
    }
    public Vector3 PosOf(int x, int y)
    {
        return new Vector3((x) * Block.size, (y) * Block.size, 0) + transform.position;
    }
    public Vector3 PosOf(Vector2Int point)
    {
        return PosOf(point.x, point.y);
    }
    public Vector2Int PosToPoint(Vector3 position)
    {
        Vector3 point = (position - transform.position) / Block.size;
        return new Vector2Int(Mathf.RoundToInt(point.x), Mathf.CeilToInt(point.y));
    }
    public void AddBlock(Block block, Vector2Int point)
    {
        blockMap[point.y, point.x] = block;
        map[point.y, point.x] = 1;
        block.transform.position = PosOf(point);
        block.transform.parent = this.transform;
    }
}
