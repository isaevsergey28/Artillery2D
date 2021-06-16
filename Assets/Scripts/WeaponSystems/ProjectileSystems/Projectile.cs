using UnityEngine;


public class Projectile : ExplosiveWeapon
{
    protected override void CalculateExplosiveFunnel()
    {
        Vector3Int position = new Vector3Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y + 0.5f),
            Mathf.RoundToInt(transform.position.z)
        );
        _tilemap.SetTile(_tilemap.WorldToCell(position), null);
    }
}