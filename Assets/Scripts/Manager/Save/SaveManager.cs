using System;
using UnityEngine;
using SQLite4Unity3d;
using System.Collections.Generic;
using System.Linq;
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public SQLiteConnection connection;

    public int SaveID;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        ConnectDB();

        SaveID = 1;//default;
    }

    private void Start()
    {
        InitTables();
    }

    #region Connection
    private void ConnectDB()
    {
        string dataBasePath = Application.streamingAssetsPath + "/SqliteDatabase.db";
        connection = new SQLiteConnection(dataBasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    }
    #endregion

    #region Initialization
    private void InitTables()
    {
        InitSaveTable();
        
        InitBuildingResourceTable();
    }

    private void InitSaveTable() => connection.CreateTable<MainData>();
    public void InitNatureResourceDB() => connection.CreateTable<NatureResourceData>();//Called In ItemGenerator
    private void InitBuildingResourceTable() => connection.CreateTable<BuildingResourceData>();
    #endregion
   
    #region Table NatureResource
    public void InsertNatureResource(string _class, int _x, int _y, float _offsetX, float _offsetY)
    {
        NatureResourceData natureResourceData = new NatureResourceData();
        natureResourceData.SaveID = this.SaveID;
        natureResourceData.Class = _class;
        natureResourceData.X = _x;
        natureResourceData.Y = _y;
        natureResourceData.OffsetX = _offsetX;
        natureResourceData.OffsetY = _offsetY;
        natureResourceData.Status = "Available";
        natureResourceData.InitTime = DateTime.Now;

        connection.Insert(natureResourceData);
    }
    public NatureResourceData GetNatureResource(int _x, int _y) => connection.Query<NatureResourceData>("SELECT * FROM NatureResourceData WHERE SaveID = ? AND X = ? AND Y = ? LIMIT 1", this.SaveID, _x, _y).FirstOrDefault();

    public bool ExistNatureResource(int _x, int _y) => connection.Table<NatureResourceData>().Where(t => t.SaveID == this.SaveID && t.X == _x && t.Y == _y).Any();

    public List<NatureResourceData> NatureResourceTable() => new List<NatureResourceData>(connection.Table<NatureResourceData>());
    #endregion

    #region Table BuildingResource
    public void InsertBuildingResource(string _class, float _x, float _y)
    {
        BuildingResourceData buildingResourceData = new BuildingResourceData();
        buildingResourceData.SaveID = this.SaveID;
        buildingResourceData.Class = _class;
        buildingResourceData.X = _x;
        buildingResourceData.Y = _y;
        buildingResourceData.Status = "Available";
        buildingResourceData.InitTime = DateTime.Now;

        connection.Insert(buildingResourceData);
    }
    #endregion

    public bool ExistTable(string tableName)
    {
        List<int> result = connection.Query<int>($"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{tableName}'");

        return result.Count > 0 && result[0] > 0;
    }
}
