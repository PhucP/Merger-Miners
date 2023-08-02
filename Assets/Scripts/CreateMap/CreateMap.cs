using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [Header("Size")]
    [SerializeField] private int _width;
    [SerializeField] private int _heightGrid;
    [SerializeField] private int _heightInv;
    [SerializeField] private float Space;


    [Header("Prefab")]
    [SerializeField] private GameObject _gridPrefab;
    [SerializeField] private GameObject _inventoryPrefab;

    [Header("Parent Transform")]
    [SerializeField] private Transform _parentGrid;
    [SerializeField] private Transform _parentInv;

    [SerializeField] private int _level;

    ///////////////////////////////////////
    private List<GameObject> _listInventory = new List<GameObject>();
    private List<GridBase> _listGrid = new List<GridBase>();

    public void Spawn()
    {

    }

    public void ReSpawn()
    {
        Reset();
        Spawn();
    }

    public void Reset()
    {
        foreach (var grid in _listGrid)
        {
            Destroy(grid.gameObject);
        }

        foreach (var inventory in _listInventory)
        {
            Destroy(inventory);
        }

        _listGrid.Clear();
        _listInventory.Clear();
    }

#if UNITY_EDITOR
    public void SaveData()
    {
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        string path = "Assets/Scripts/Config/LevelConfig/LevelData_" + _level + ".asset";

        List<GridData> gridDatas = new List<GridData>(); 
        foreach (var grid in _listGrid)
        {
            GridData newGrid = new GridData();
            newGrid.Type = grid.Type;
            newGrid.Position = grid.Position;

            gridDatas.Add(newGrid);
        }

        levelData.Width = _width;
        levelData.GridHeight = _heightGrid;
        levelData.InventoryHeight = _heightInv;
        levelData.Space = Space;
        levelData.ListGrid = gridDatas;
        levelData.AnchorsPosY = Camera.main.GetComponent<RectTransform>().anchoredPosition.y;
        levelData.FieldOfView = Camera.main.GetComponent<Camera>().fieldOfView; 

        AssetDatabase.CreateAsset(levelData, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif

    ///////////////////////////////////////

    // private void Start()
    // {
    //     Spawn();
    // }

    // public void Spawn()
    // {
    //     SpawnGrid();
    //     SpawnInventory();
    // }

    // private void SpawnGrid()
    // {
    //     SpawnMap(_heightGrid, _width, _parentGrid, _gridPrefab);
    //     //_parentGrid.rotation = Quaternion.Euler(-15f, 0, 0);
    // }

    // private void SpawnInventory()
    // {
    //     SpawnMap(_heightInv, _width, _parentInv, _inventoryPrefab, Game.Instance.ListInventory);
    // }

    // private void SpawnMap(int width, int height, Transform parent, GameObject prefab, List<Inventory> list = null)
    // {
    //     var startPos = CalucalteStartPos();
    //     for (int i = 0; i < height; i++)
    //     {
    //         for (int j = 0; j < width; j++)
    //         {
    //             var grid = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
    //             grid.transform.localPosition = new Vector3(startPos + i * Space, -j * Space, 0);

    //             list?.Add(grid.GetComponent<Inventory>());
    //         }
    //     }
    // }

    // private float CalucalteStartPos()
    // {
    //     float startX = 0;
    //     startX = (_width % 2 != 0) ? -_width / 2 * Space : -(_width / 2 - 0.5f) * Space;
    //     return startX;
    // }


}
