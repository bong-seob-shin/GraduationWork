using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : MonoBehaviour
{
   
   public int id { get; set; }

   public float speed = 0f;

   public float dirX;
   public float dirZ;

   public int MaxHP;
   public int HP;
   
   protected virtual void Move() 
   {
      
   }

   protected virtual void UpdateAnim()
   {
      
   }
   
   //피격 함수
   public void hit(int damage)
   {
      HP -= damage;
      Debug.Log(transform.name + ":" + HP);
   }

   protected virtual void Dead()
   {
      
   }
}
