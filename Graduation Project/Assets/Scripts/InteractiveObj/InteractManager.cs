using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractManager : MonoBehaviour
{
    [SerializeField]
    private int buttonNum;
    private int _lastButtonNum;
    private int _buttonPacket;


   
    private bool _buttonOnPacket;
    
    private bool isValueChanged;
    // Start is called before the first frame update


    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

        buttonNum = _buttonPacket;
        if (buttonNum > 0)
        {
            InteractObj.buttonList[buttonNum - 1].currentButtonOn =
                _buttonOnPacket; // 누른 사람의 클라이언트에서는 쓸모없는 줄이지만 누르지 않은 사람들이 패킷을 전달받기 위한 줄

            if (buttonNum != _lastButtonNum) // 이전의 버튼값과 서버동기화된 버튼의 값이 같은지 확인 다르면 밑에 기능을 수행한다
            {
                isValueChanged = true;
            }
            else
            {
                if (InteractObj.buttonList[buttonNum - 1].lateButtonOn !=
                    InteractObj.buttonList[buttonNum - 1].currentButtonOn) //현재의 온오프상태와 과거의 온오프상태가 다르면 값이 바뀐걸로 판정
                {
                    isValueChanged = true;
                }
            }

            if (isValueChanged) //값이 바뀌었으면 버튼 상호작용실행
            {
                for (int i = 0; i < InteractObj.buttonList.Count; i++)
                {
                    if (InteractObj.buttonList[i].buttonId == buttonNum)
                    {
                        InteractObj.buttonList[i].InteractObjs();
                        _lastButtonNum = buttonNum;
                        InteractObj.buttonList[i].lateButtonOn = InteractObj.buttonList[i].currentButtonOn;
                        isValueChanged = false;
                    }
                }
            }
        }
        
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log( InteractObj.buttonList.Count);
        }
        #endif
    }


    public void SetButton(int key)
    {
        _buttonPacket = key;
        _buttonOnPacket = !InteractObj.buttonList[key-1].currentButtonOn; // 선택한 버튼의 on off상태를 받아와서 그 반대를 패킷에 담아서 전송함
        //여기서 버튼 패킷을 보낸다? 버튼패킷은 int btnNum, bool _ButtonOn
        //먼저 플레이어에서 누른 버튼의 id로 이함수에 들어와서 버튼 패킷에 들어오고 그게 서버로가서 브로드캐스팅이되면 값이 변경되었는지 체크해서
        //값이 변경되었으면 버튼누르는 기능을 수행하게하였음
    }
}
