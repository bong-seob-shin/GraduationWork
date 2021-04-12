using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObj : ObjManager //애니메이션을 하는 오브젝트를 관리함
{
    public Animator anim;

     
   private void Start()
   {
       anim =  anim = gameObject.GetComponent<Animator>(); //애니메이터 받아오기
   }

   private void Update()
   {
      
   }
}
