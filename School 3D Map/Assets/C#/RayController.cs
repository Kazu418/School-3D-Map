using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RaycastController : MonoBehaviour
{
    public Camera mainCamera; // メインカメラ
    public EventSystem eventSystem; // イベントシステム
    public SerachWindowController searchWindowController; // 検索ウィンドウのコントローラー
    public Image fadePanel; // フェード用UIパネル（Image）
    public float fadeDuration = 1.0f; // フェードの所要時間

    //フラグのデータ読み込み
    private Generate_Pointflagclone generate_Flag;

    private void OnEnable(){
        generate_Flag = GameObject.Find("Search_Flag_Master").GetComponent<Generate_Pointflagclone>();
    }

    void Update()
    {
        // 左クリックの入力を検知してRaycastを実行
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button pressed.");
        }

        if (Input.GetMouseButtonUp(0))
        {
            // UIがヒットしていないかチェック
            if (searchWindowController.isDragging == false)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // RaycastのヒットオブジェクトからLocations Listのインデックスを取得
                    GameObject touchedObject = hit.collider.gameObject;

                    var index = generate_Flag.Get_IndexNum(touchedObject);
                    print(index);

                    /*
                    Generate_Pointflagclone genFlagClone = gameObject.AddComponent<Generate_Pointflagclone>();
                    HandleRaycastHit(genFlagClone.Get_IndexNum(touchedObject));
                    */
                }
            }
            else
            {
                Debug.Log("Raycast was blocked by UI.");
            }
        }
    }

    // Raycastの結果を処理する
    void HandleRaycastHit(int index)
    {
        Debug.Log(index);
    }

    // フェード処理
    public IEnumerator Fade()
    {
        fadePanel.enabled = true; // パネルを有効化
        float elapsedTime = 0.0f; // 経過時間の初期化
        Color startColor = fadePanel.color; // フェードパネルの開始色
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードアウト時の色

        // フェードイン
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime; // 経過時間を増加
            float t = Mathf.Clamp01(elapsedTime / fadeDuration); // フェード進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を徐々に変化
            yield return null; // 1フレーム待機
        }

        fadePanel.color = endColor; // フェード終了時の色をセット

        elapsedTime = 0.0f; // 経過時間の初期化
        startColor = fadePanel.color; // 開始色を更新
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f); // フェードイン時の色

        // フェードアウト
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime; // 経過時間を増加
            float t = Mathf.Clamp01(elapsedTime / fadeDuration); // フェード進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を徐々に変化
            yield return null; // 1フレーム待機
        }

        fadePanel.enabled = false; // パネルを無効化
    }
}