using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneControlObj : AnimationObj
{
    
    public Vector3 destPos; //도착지의 좌표 -CellPos? 내가 조작하는 플레이어는 이게 필요가 없어서 처리를 생각해보는것도 좋을듯

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Move()
    {
        Vector3 moveDir = destPos - transform.position;

        float dist = moveDir.magnitude;
        if (dist < speed * Time.deltaTime)
        {
            transform.position = destPos;
        }
        else
        {
            transform.position += moveDir.normalized * speed * Time.deltaTime;
            //상태는 오브젝트마다 다르니까 하위클래스에 직접 가서 관리하는게 좋을 것 같음
        }

    }
}
