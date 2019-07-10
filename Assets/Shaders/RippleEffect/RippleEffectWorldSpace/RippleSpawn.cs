using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleSpawn : MonoBehaviour
{
    public RippleState state;

     ///DEBUG
    /*  void Update()
      {
          if(Input.GetMouseButtonDown(0))
          {
              var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
              RaycastHit hit;
              if(Physics.Raycast(ray, out hit))
              {
                  state.RippleOrigin = hit.point;
              }
          }
      }*/
    public void SpawnRippleAtPoint(Transform origin)
    {
        state.RippleOrigin = origin.position;
    }
}
