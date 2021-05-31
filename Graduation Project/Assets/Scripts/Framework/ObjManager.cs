using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : MonoBehaviour
{

   public int id;

   public float speed = 0f;

   public float dirX;
   public float dirZ;

   public int MaxHP;
   public float HP;
   public int armor;
   protected virtual void Move() 
   {
      
   }

   protected virtual void UpdateAnim()
   {
      
   }
   
   //피격 함수
   public virtual void hit(float _damage, float penetration)
   {
      if (armor - penetration >= 70)
      {
         _damage = _damage * 0.1f;
      }
      else if (armor - penetration >= 50)
      {

         _damage = _damage * 0.3f;
      }
      else if (armor - penetration >= 30)
      {
         _damage = _damage * 0.5f;
      }
      else if (armor - penetration >= 10)
      {
         _damage = _damage * 0.7f;
      }
      else
      {
         _damage = _damage * 1f;
      }

      HP -= _damage;
   }

   protected virtual void Dead()
   {
      
   }
}
