using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOutSceneLoader : MonoBehaviour
{
  public Image fadeoutPanel;             // フェード用のUIパネル（Image）
  public float fadeoutDuration = 1.0f;   // フェードの完了にかかる時間

  private void Start()
  {
    StartCoroutine(FadeOutAndLoadScene());
  }

  public IEnumerator FadeOutAndLoadScene()
  {
    fadeoutPanel.enabled = true;                 // パネルを有効化
    float elapsedTime = 0.0f;                 // 経過時間を初期化
    Color startColor = fadeoutPanel.color;       // フェードパネルの開始色を取得
    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

    // フェードアウトアニメーションを実行
    while (elapsedTime < fadeoutDuration)
    {
      elapsedTime += Time.deltaTime;                        // 経過時間を増やす
      float t = Mathf.Clamp01(elapsedTime / fadeoutDuration);  // フェードの進行度を計算
      fadeoutPanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
      yield return null;                                     // 1フレーム待機
    }

    fadeoutPanel.color = endColor;  // フェードが完了したら最終色に設定
    SceneManager.LoadScene("Menu"); // シーンをロードして別シーンに遷移
  }
}