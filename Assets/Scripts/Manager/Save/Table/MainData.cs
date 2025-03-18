using System;
using SQLite4Unity3d;

public class MainData
{
    #region Save
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    #region Map
    public int Seed { get; set; }
    public float Lacunarity { get; set; }
    public float WaterRatio { get; set; }
    public DateTime InitTime { get; set; }
    public TimeSpan TotalTime { get; set; }
    #endregion

    #region Player
    public string Name { get; set; }
    public int HP { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public DateTime SurviveTime { get; set; }
    #endregion

    #endregion
}
