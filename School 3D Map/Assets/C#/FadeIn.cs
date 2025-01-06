using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSceneLoader : MonoBehaviour
{
  public Image fadePanel;             // フェード用のUIパネル（Image）
  public float fadeDuration = 5.0f;   // フェードの完了にかかる時間

  private void Start()
  {
    StartCoroutine(FadeAndLoadScene());
  }

  public IEnumerator FadeAndLoadScene()
  {
    
    float elapsedTime = 0.0f;                 // 経過時間を初期化
    Color startColor = fadePanel.color;       // フェードパネルの開始色を取得

    // フェードアウトアニメーションを実行
    while (elapsedTime < fadeDuration)
    {
      elapsedTime += Time.deltaTime;                        // 経過時間を増やす
      float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
      fadePanel.color = Color.Lerp(fadePanel.color, new Color(0, 0, 0, 0), t); // パネルの透明度を変更してフェード
      yield return null;                                     // 1フレーム待機
    }

    fadePanel.enabled = false;    // パネルを無効化
  }
}
