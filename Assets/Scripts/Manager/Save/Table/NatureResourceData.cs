using System;
using SQLite4Unity3d;
public class NatureResourceData 
{
    #region Save
    public int SaveID { get; set; }

    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Class { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public float OffsetX { get; set; }
    public float OffsetY { get; set; }
    public string Status { get; set; }
    public DateTime InitTime { get; set; }
    #endregion
}
