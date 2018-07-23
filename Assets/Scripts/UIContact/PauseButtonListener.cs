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
        if (GameController.instance.gameState == GameController.State.Play)
        {
            AudioController.instance.PlaySetting();
            PauseMenuListener.instance.MenuOn();
        }
        else
        {
            GameController.instance.TapStart();
        }
    }
}
