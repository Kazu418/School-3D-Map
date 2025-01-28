using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RaycastController : MonoBehaviour
{
  public Camera mainCamera;
  public EventSystem eventSystem;
  public SerachWindowController searchWindowController;  // 参照フィールド追加
  public Image fadePanel;             // フェード用のUIパネル（Image）
  public float fadeDuration = 1.0f;   // フェードの完了にかかる時間

  void Update()
  {
    if (Input.GetMouseButtonDown(0)) // 左クリックでRaycast
    {
      Debug.Log("Left mouse button pressed.");
    }
    if (Input.GetMouseButtonUp(0))
    {
      // UIがヒットしているかチェック
      if (searchWindowController.isDragging == false)
      {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
          // Raycastが当たったオブジェクトのLocations Listのインデックスを取得
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
    fadePanel.enabled = true;                 // パネルを有効化
    float elapsedTime = 0.0f;                 // 経過時間を初期化
    Color startColor = fadePanel.color;       // フェードパネルの開始色を取得
    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

    // フェードアウトアニメーションを実行
    while (elapsedTime < fadeDuration)
    {
      elapsedTime += Time.deltaTime;                        // 経過時間を増やす
      float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
      fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
      yield return null;                                     // 1フレーム待機
    }

    fadePanel.color = endColor;  // フェードが完了したら最終色に設定

    elapsedTime = 0.0f;                 // 経過時間を初期化
    startColor = fadePanel.color;       // フェードパネルの開始色を取得
    endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

    // フェードアウトアニメーションを実行
    while (elapsedTime < fadeDuration)
    {
      elapsedTime += Time.deltaTime;                        // 経過時間を増やす
      float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
      fadePanel.color = Color.Lerp(fadePanel.color, new Color(0, 0, 0, 0), t); // パネルの透明度を変更してフェード
      yield return null;                                     // 1フレーム待機
    }
    fadePanel.enabled = false;                 // パネルを無効化
  }
}