using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.HighDefinition;

public class MonsterManager : ObjManager
{ 
    public NavMeshAgent nav;
    [HideInInspector]public bool isDead = false;
    public Rigidbody rigid;
    
    protected override void Dead()
    {
        isDead = true;
        nav.enabled = false;
        rigid.velocity = Vector3.zero;
        //anim.SetTrigger("Dead");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}