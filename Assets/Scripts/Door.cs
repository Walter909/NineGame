using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

   [SerializeField] private Transform PreviousRoom;
   [SerializeField] private Transform NextRoom;

   [SerializeField] private CameraController Camera;


   private void OnTriggerEnter2D(Collider2D collision)
   {
       if (collision.tag == "Player")
       {
           if (collision.transform.position.y < transform.position.y)
           {
               Camera.MoveToNewRoom(PreviousRoom);
           }
           else
           {
               Camera.MoveToNewRoom(NextRoom);
           }
       }
   }
}
