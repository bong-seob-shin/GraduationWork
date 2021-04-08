using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StateMachine
{
    //현재 상태를 나타내는 변수
    public IState CurrentState { get; private set; }
    
    //기본 상태를 생성시에 설정하게 만드는 생성자
    public StateMachine(IState defultState)
    {
        CurrentState = defultState;
    }
    
    //상태를 바꿔주는 함수
    public void SetState(IState state)
    {
        //이미 하고있는 행동으로는 바뀌지 않게 처리

        if (CurrentState == state)
        {
            return;
        }
        
        //이전 상태의 Exit을 호출
        CurrentState.OperateExit();

        //상태 바꿈
        CurrentState = state;
        
        //바뀐 상태의 Enter를 호출
        CurrentState.OperateEnter();
        
        
    }

    public IState GetCurrentState()
    {
        return CurrentState;
    }
    
    public void ExecuteUpdate()
    {
        CurrentState.OperateUpdate();
    }
    
}
