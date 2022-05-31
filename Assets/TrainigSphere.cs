using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


//
// Sphere100 �̃R���g���[��
//


public class TrainigSphere : MonoBehaviour
{
    // transform
    //    Transform myTransform;

    // ���݂̈ʒu
    Vector3 currentPos;

    // �C���p
    Vector3 cPos = new Vector3(1.0f, 1.0f, 1.0f);

    // ���݂̃R���g���[�� L �̈ʒu�E�p��
    Vector3 controllerPos;
    Quaternion controllerOri;
    // �^�[�Q�b�g�R���g���[�� L �̈ʒu�E�p��
    Vector3 targetPos;
    Quaternion targetOri;
    // �ړ���
    Vector3 offsetPos;

    // Start is called before the first frame update
    void Start()
    {
        //�uMODE�v�Ƃ����L�[�ŕۑ�����Ă���Int�l��ǂݍ���
        int runMode = PlayerPrefs.GetInt("MODE");
        // �ǂݍ���
        string DataFile = "C:/Users/raspberry/UTfolder/PosController.json";
        string datastr = "";
        StreamReader reader;
        try
        {
            reader = new StreamReader(DataFile);
            datastr = reader.ReadToEnd();
            reader.Close();
        }
        catch (System.IO.IOException ex)
        {
            Debug.Log("�t�@�C�����J���Ƃ��ɃG���[�ɂȂ�܂���" + ex);
        }
        ControllerPos initialControllerPos = JsonUtility.FromJson<ControllerPos>(datastr);
        if (runMode == 1) // ���ʂ̃f�[�^������
        {

            // Oculus���猩���R���g���[���̑��Έʒu
            targetPos = initialControllerPos.sitPosL;
            targetOri = initialControllerPos.sitRotationL;
        }
        else // ���ʂ̃f�[�^������
        {
            // Oculus���猩���R���g���[���̑��Έʒu
            targetPos = initialControllerPos.standPosL;
            targetOri = initialControllerPos.standRotationL;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �����ړ�������
        // Oculus���猩���R���g���[���̑��Έʒu
        controllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        controllerOri = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        // �ړ��ʂ̌v�Z�@���̈ʒu�ɖ߂邽�߂̃x�N�g����
        offsetPos = controllerPos - targetPos;

        // ���̈ʒu
        currentPos = this.transform.position;  //

        string offset2 = PlayerPrefs.GetString("OFFSET").Trim('(', ')');
        string[] offsetStr = offset2.Split(',');

        // store as a Vector3
        Vector3 offset = new Vector3(
            float.Parse(offsetStr[0]),
            float.Parse(offsetStr[1]),
            float.Parse(offsetStr[2]));


        // �ړ�
        this.transform.position = offsetPos +offset;  // ���W��ݒ�

/*
        var keyboard = Keyboard.current;
        if (keyboard.qKey.wasPressedThisFrame)
        {
            // �X�^�[�g���j���[�ɐ؂�ւ���
            SceneManager.LoadScene("StartHere");
        }

        Debug.Log("Now: " + currentPos + " Offset: " + offsetPos + " Next: " + this.transform.position + " Scale: " + this.transform.localScale);
*/

    }


    //
    // class ���쐬����
    //
    [System.Serializable]
class ControllerPos
    {
        public Vector3 sitPosR = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion sitRotationR = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        public Vector3 sitPosL = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion sitRotationL = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        public Vector3 standPosR = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion standRotationR = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        public Vector3 standPosL = new Vector3(0.0f, 0.0f, 0.0f);
        public Quaternion standRotationL = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
    }
}
