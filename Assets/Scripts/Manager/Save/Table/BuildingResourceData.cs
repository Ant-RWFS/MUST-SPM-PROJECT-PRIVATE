using System;
using SQLite4Unity3d;
public class BuildingResourceData
{
    #region Save
    public int SaveID { get; set; }

    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Class { get; set; }
    public int Level { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public string Status { get; set; }
    public DateTime InitTime { get; set; }
    #endregion
}
