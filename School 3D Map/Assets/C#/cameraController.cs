using UnityEngine;

public class SmoothCameraRotation : MonoBehaviour
{
    [Header("ターゲット設定")]
    public Transform target; // カメラが回転する中心となるオブジェクト（Field）

    [Header("回転設定")]
    public float rotationSpeed = 200f; // マウス入力に対する回転速度の感度
    public float damping = 5f; // 減衰速度

    private float rotationVelocity = 0f; // 現在の回転速度
    private Vector3 previousMousePosition;
    private float instantaneousSpeed = 0f; // スワイプの瞬間速度
    private bool isDragging = false;

    void Update()
    {
        if (target == null)
        {
            Debug.LogError("ターゲットが設定されていません。");
            return;
        }

        HandleInput();
        ApplyRotation();
    }

    /// <summary>
    /// ユーザー入力の処理
    /// </summary>
    private void HandleInput()
    {
        // 入力開始
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            previousMousePosition = Input.mousePosition;
        }

        // 入力中
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = Input.mousePosition - previousMousePosition;

            // マウス移動速度（瞬間速度）の計算
            instantaneousSpeed = mouseDelta.x / Time.deltaTime;

            // スワイプ速度が一定以上の場合のみ速度を更新
            if (Mathf.Abs(instantaneousSpeed) > Mathf.Abs(rotationVelocity))
            {
                rotationVelocity = instantaneousSpeed * rotationSpeed * Time.deltaTime;
            }

            // マウスが止まっている場合は減速処理を適用
            if (Mathf.Abs(mouseDelta.x) < 0.01f)
            {
                rotationVelocity = Mathf.Lerp(rotationVelocity, 0, damping * Time.deltaTime);
            }

            previousMousePosition = Input.mousePosition;
        }
        else
        {
            // マウスを離したとき
            if (isDragging)
            {
                isDragging = false;
            }

            // スワイプ速度が現在の速度を下回ったときは減速処理を適用
            if (Mathf.Abs(rotationVelocity) > Mathf.Abs(instantaneousSpeed))
            {
                rotationVelocity = Mathf.Lerp(rotationVelocity, 0, damping * Time.deltaTime);

                if (Mathf.Abs(rotationVelocity) < 0.01f)
                {
                    rotationVelocity = 0f;
                }
            }

            instantaneousSpeed = 0f; // スワイプ速度をリセット
        }
    }

    /// <summary>
    /// カメラの回転を適用
    /// </summary>
    private void ApplyRotation()
    {
        if (rotationVelocity != 0f)
        {
            // カメラをターゲットの周りで回転
            transform.RotateAround(target.position, Vector3.up, rotationVelocity);
        }
    }
}