using UnityEngine;
using UnityEditor;

public class ScreenshotTrigger : MonoBehaviour 
{

#if UNITY_EDITOR
    [SerializeField]
    string _path = "";

    int _screenShotCount;

    [SerializeField]
    int screenShotcount
    {
        get
        {
            _screenShotCount = PlayerPrefs.GetInt("lastScreenNumber", 0);
            return _screenShotCount;
        }
        set
        {
            _screenShotCount = value;
            PlayerPrefs.SetInt("lastScreenNumber", _screenShotCount);
        }
    }

    [SerializeField]
    KeyCode _key;


    void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            TakeScreenshot();
        }
    }

    public void TakeScreenshot()
    {
        screenShotcount++;
        ScreenCapture.CaptureScreenshot(_path + screenShotcount.ToString() + ".png");
    }
#endif
}