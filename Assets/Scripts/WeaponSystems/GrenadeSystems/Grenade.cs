using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : ExplosiveWeapon
{
    protected override void CalculateExplosiveFunnel()
    {
        Vector3Int position = new Vector3Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y + 0.5f),
            Mathf.RoundToInt(transform.position.z)
        );
        _tilemap.SetTile(_tilemap.WorldToCell(position), null);

        position = new Vector3Int(
            Mathf.RoundToInt(transform.position.x - 1),
            Mathf.RoundToInt(transform.position.y + 0.5f),
            Mathf.RoundToInt(transform.position.z)
        );
        _tilemap.SetTile(_tilemap.WorldToCell(position), null);

        position = new Vector3Int(
           Mathf.RoundToInt(transform.position.x + 1),
           Mathf.RoundToInt(transform.position.y + 0.5f),
           Mathf.RoundToInt(transform.position.z)
       );
        _tilemap.SetTile(_tilemap.WorldToCell(position), null);

        position = new Vector3Int(
          Mathf.RoundToInt(transform.position.x + 1),
          Mathf.RoundToInt(transform.position.y - 0.5f),
          Mathf.RoundToInt(transform.position.z)
      );
        _tilemap.SetTile(_tilemap.WorldToCell(position), null);
    }
}
