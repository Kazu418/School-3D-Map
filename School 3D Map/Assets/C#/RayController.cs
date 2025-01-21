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
      if (searchWindowController.isDragging == true)
      {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
          // Raycastが当たったオブジェクトのFlagsListを取得
          FlagData someScript = hit.collider.GetComponent<FlagData>();

          if (someScript != null)
          {
            FlagsList flagdata = someScript.flagdata;
            if (flagdata != null)
            {
              // flagdataに基づいて処理を行う
              HandleRaycastHit(flagdata);
            }
          }
        }
      }
      else
      {
        Debug.Log("Raycast was blocked by UI.");
      }
    }
  }

  void HandleRaycastHit(FlagsList flagsList)
  {
    // FlagsListのType, ID, Statusを基に異なる動きをする
    switch (flagsList.Type)
    {
      case 0:
        // 校舎別の処理
        if (flagsList.Status == 0)
        {
          Debug.Log($"校舎 {flagsList.ID} は不可です");
          // 例えば不可の場合、オブジェクトを非表示にする
          // hit.collider.gameObject.SetActive(false);
        }
        else
        {
          Debug.Log($"校舎 {flagsList.ID} は可です");
          Fade();
          // 例えば可の場合、オブジェクトを表示する
          // hit.collider.gameObject.SetActive(true);
        }
        break;

      case 1:
        // 階層別の処理
        if (flagsList.Status == 0)
        {
          Debug.Log($"階層 {flagsList.ID} は不可です");
          // 例えば不可の場合、オブジェクトを非表示にする
          // hit.collider.gameObject.SetActive(false);
        }
        else
        {
          Debug.Log($"階層 {flagsList.ID} は可です");
          // 例えば可の場合、オブジェクトを表示する
          // hit.collider.gameObject.SetActive(true);
        }
        break;

      default:
        Debug.LogWarning("Unknown Type");
        break;
    }
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