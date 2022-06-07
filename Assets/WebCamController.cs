using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用
using UnityEngine.InputSystem;

public class WebCamController : MonoBehaviour
{
    Vector3 Pos;
    int width = 640;
    int height = 480;
    int fps = 30;
    WebCamTexture webcamTexture;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Web Cam Start !");
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, this.width, this.height, this.fps);
        GetComponent<Renderer>().material.mainTexture = webcamTexture;
        webcamTexture.Play();
        Debug.Log("Web Cam  width: " + this.width + " height: " + this.height + " fps: " + this.fps);
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            // Debug.Log("キーボード");
            if (keyboard.qKey.wasPressedThisFrame)
            {
                // カメラの停止
                webcamTexture.Stop();
                // スタートメニューに切り替える
                SceneManager.LoadScene("StartHere");
            }
        }

    }
}
