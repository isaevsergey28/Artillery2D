using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
   private bool _isReserved = false;

   public bool GetStatus()
   {
      return _isReserved;
   }
   public void ReservePoint()
   {
      _isReserved = true;
   }
}
