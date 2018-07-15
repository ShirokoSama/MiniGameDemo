using UnityEngine;

/// <summary>
/// 用于管理当暂停键被按下时触发的事件
/// </summary>
/// <remarks>
/// 2018.6.18: NAiveD创建
/// </remarks>

public class PauseButtonListener : MonoBehaviour {

	public void ButtonClick()
    {
        PauseMenuListener.instance.MenuOn();
    }
}
