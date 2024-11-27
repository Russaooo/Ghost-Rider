using System.Collections.Generic;
using UnityEngine;
public class CoordinateAndRotation
{
    public Vector3 Coordinate { get; set; }
    public Quaternion Rotation { get; set; }
    
    public CoordinateAndRotation(Vector3 coordinate,Quaternion rotation)
    {
        this.Coordinate = coordinate;
        this.Rotation = rotation;
    }
}
public static class GhostRiderCoordinates {
    // для хранения координат пути игрока для механизма гонки с призраком (элементы хранят попарно координату и поворот)
    
    public static LinkedList<CoordinateAndRotation> CurrentPlayerPath = new LinkedList<CoordinateAndRotation>();
    public static LinkedList<CoordinateAndRotation> OldPlayerPath = new LinkedList<CoordinateAndRotation>();

    public static void WriteCoordinates(Vector3 position,Quaternion rotation)
    {
        CurrentPlayerPath.AddLast(new CoordinateAndRotation(position,rotation));
    }

    // позиция гонщика с предыдущего кадра
    public static Vector3 inRacePrevPos;
    // объекты колёс
    public static Transform wheelFL;
    public static Transform wheelFR;

    // перенос новых координат пути игрока в область старых координат для призрачного гонщика
    public static void DropPreviousCoordinates()
    {
        OldPlayerPath.Clear();
        if (CurrentPlayerPath != null)
        {
            foreach (var c in CurrentPlayerPath)
            {
                OldPlayerPath.AddLast(c);
            }
            CurrentPlayerPath.Clear();
        }
        
    }
}
