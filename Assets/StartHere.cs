using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class StartHere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.bKey.wasPressedThisFrame)
            {
                // ���ʂł̏����ʒu���v�����Ĉʒu�p���f�[�^��ۑ�����
                // 
                PlayerPrefs.SetInt("MODE", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MeasurementScene");

            }
            else if (keyboard.mKey.wasPressedThisFrame)
            {
                // ���ʂł̏����ʒu���v�����Ĉʒu�p���f�[�^��ۑ�����
                // 
                PlayerPrefs.SetInt("MODE", 2);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MeasurementScene");
            }
            else if (keyboard.cKey.wasPressedThisFrame)
            {
                // ���ʂ̃L�����u���[�V����
                PlayerPrefs.SetInt("MODE", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("InitPosition");
            }
            else if (keyboard.zKey.wasPressedThisFrame)
            {
                // ���ʂ̃L�����u���[�V����
                PlayerPrefs.SetInt("MODE", 2);
                PlayerPrefs.Save();
                SceneManager.LoadScene("InitPosition");
            }
            else if (keyboard.rKey.wasPressedThisFrame)
            {
                // ���ʂ̎��s
                PlayerPrefs.SetInt("MODE", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("TrainingScene");
            }
            else if (keyboard.yKey.wasPressedThisFrame)
            {
                // ���ʂ̎��s
                PlayerPrefs.SetInt("MODE", 2);
                PlayerPrefs.Save();
                SceneManager.LoadScene("TrainingScene");
            }
            else if (keyboard.oKey.wasPressedThisFrame)
            {
                // ���ʂ̎��s
                PlayerPrefs.SetInt("MODE", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MovieScene");
            }
            else if (keyboard.uKey.wasPressedThisFrame)
            {
                // ���ʂ̎��s
                PlayerPrefs.SetInt("MODE", 2);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MovieScene");
            }
        }
    }
}
