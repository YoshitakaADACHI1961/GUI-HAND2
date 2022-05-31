using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class CylinderR : MonoBehaviour
{
    // ���s���[�h
    int runMode = 0;
    // �R���g���[���̈ʒu
    ControllerPos controllerPos;

    // Start is called before the first frame update
    void Start()
    {
        //�uMODE�v�Ƃ����L�[�ŕۑ�����Ă���Int�l��ǂݍ���
        runMode = PlayerPrefs.GetInt("MODE");

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
            Debug.Log("1 �t�@�C�����J���Ƃ��ɃG���[�ɂȂ�܂���" + ex);
        }

        controllerPos = JsonUtility.FromJson<ControllerPos>(datastr);

        if (runMode == 1) // ���ʂ̃f�[�^������
        {
            transform.position = controllerPos.sitPosR;
            Debug.Log("���ʁ@Cylinder R = " + transform.position);
        }
        else // ���ʂ̃f�[�^������
        {
            transform.position = controllerPos.standPosR;
            Debug.Log("���ʁ@Cylinder R = " + transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    //
    // class ���쐬����
    //
    [System.Serializable]
    public class ControllerPos
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
