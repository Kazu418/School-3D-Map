using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RaycastController : MonoBehaviour
{
  public Camera mainCamera;
  public EventSystem eventSystem;
  public SerachWindowController searchWindowController;  // �Q�ƃt�B�[���h�ǉ�
  public Image fadePanel;             // �t�F�[�h�p��UI�p�l���iImage�j
  public float fadeDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��

  void Update()
  {
    if (Input.GetMouseButtonDown(0)) // ���N���b�N��Raycast
    {
      Debug.Log("Left mouse button pressed.");
    }
    if (Input.GetMouseButtonUp(0))
    {
      // UI���q�b�g���Ă��邩�`�F�b�N
      if (searchWindowController.isDragging == false)
      {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
          // Raycast�����������I�u�W�F�N�g��Locations List�̃C���f�b�N�X���擾
          GameObject touchedobject = hit.collider.gameObject;
          Generate_Pointflagclone gen_flagclone = gameObject.AddComponent<Generate_Pointflagclone>();
          HandleRaycastHit(gen_flagclone.Get_IndexNum(touchedobject));
        }
      }
      else
      {
        Debug.Log("Raycast was blocked by UI.");
      }
    }
  }

  void HandleRaycastHit(int index)
  {
    Debug.Log(index);
  }
  public IEnumerator Fade()
  {
    fadePanel.enabled = true;                 // �p�l����L����
    float elapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
    Color startColor = fadePanel.color;       // �t�F�[�h�p�l���̊J�n�F���擾
    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

    // �t�F�[�h�A�E�g�A�j���[�V���������s
    while (elapsedTime < fadeDuration)
    {
      elapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
      float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // �t�F�[�h�̐i�s�x���v�Z
      fadePanel.color = Color.Lerp(startColor, endColor, t); // �p�l���̐F��ύX���ăt�F�[�h�A�E�g
      yield return null;                                     // 1�t���[���ҋ@
    }

    fadePanel.color = endColor;  // �t�F�[�h������������ŏI�F�ɐݒ�

    elapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
    startColor = fadePanel.color;       // �t�F�[�h�p�l���̊J�n�F���擾
    endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

    // �t�F�[�h�A�E�g�A�j���[�V���������s
    while (elapsedTime < fadeDuration)
    {
      elapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
      float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // �t�F�[�h�̐i�s�x���v�Z
      fadePanel.color = Color.Lerp(fadePanel.color, new Color(0, 0, 0, 0), t); // �p�l���̓����x��ύX���ăt�F�[�h
      yield return null;                                     // 1�t���[���ҋ@
    }
    fadePanel.enabled = false;                 // �p�l���𖳌���
  }
}