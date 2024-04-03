using System.Numerics;

interface IBuilding
{
    Building prototype { get; set; }
    Vector3 position { get; set; }
    Quaternion rotation { get; set; }
}
