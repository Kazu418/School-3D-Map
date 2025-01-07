using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOutSceneLoader : MonoBehaviour
{
  public Image fadeoutPanel;             // �t�F�[�h�p��UI�p�l���iImage�j
  public float fadeoutDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��

  private void Start()
  {
    StartCoroutine(FadeOutAndLoadScene());
  }

  public IEnumerator FadeOutAndLoadScene()
  {
    fadeoutPanel.enabled = true;                 // �p�l����L����
    float elapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
    Color startColor = fadeoutPanel.color;       // �t�F�[�h�p�l���̊J�n�F���擾
    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

    // �t�F�[�h�A�E�g�A�j���[�V���������s
    while (elapsedTime < fadeoutDuration)
    {
      elapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
      float t = Mathf.Clamp01(elapsedTime / fadeoutDuration);  // �t�F�[�h�̐i�s�x���v�Z
      fadeoutPanel.color = Color.Lerp(startColor, endColor, t); // �p�l���̐F��ύX���ăt�F�[�h�A�E�g
      yield return null;                                     // 1�t���[���ҋ@
    }

    fadeoutPanel.color = endColor;  // �t�F�[�h������������ŏI�F�ɐݒ�
    SceneManager.LoadScene("Menu"); // �V�[�������[�h���ĕʃV�[���ɑJ��
  }
}