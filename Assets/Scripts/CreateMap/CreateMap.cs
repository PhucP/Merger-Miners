using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [SerializeField] private int _heightGrid;
    [SerializeField] private int _widthGrid;

    [SerializeField] private int _heightInv;
    [SerializeField] private int _widthInv;

    [SerializeField] private GameObject _gridPrefab;
    [SerializeField] private Transform _parentGrid;
    [SerializeField] private Transform _parentInv;
    [SerializeField] private GameObject _inventoryPrefab;
    [SerializeField] private float Scale;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        SpawnGrid();
        SpawnInventory();
    }

    private void SpawnGrid()
    {
        SpawnMap(_heightGrid, _widthGrid, _parentGrid, _gridPrefab);
        //_parentGrid.rotation = Quaternion.Euler(-15f, 0, 0);
    }

    private void SpawnInventory()
    {
        SpawnMap(_heightInv, _widthInv, _parentInv, _inventoryPrefab);
    }

    private void SpawnMap(int width, int height, Transform parent, GameObject prefab)
    {
        var startPos = CalucalteStartPos();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var grid = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
                grid.transform.localPosition = new Vector3(startPos + i * Scale, -j * Scale, 0);
            }
        }
    }

    private float CalucalteStartPos()
    {
        float startX = 0;
        startX = (_heightGrid % 2 == 0) ? -_heightGrid / 2 * Scale : -(_heightGrid / 2 - 0.5f) * Scale;
        return startX;
    }
}
