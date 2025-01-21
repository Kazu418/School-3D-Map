using UnityEngine;
using UnityEditor;

public class MoveToCenterOfParent : MonoBehaviour
{
    [MenuItem("MyTools/Move To Center Of Parent")]
    public static void MoveToCenter(){
        Transform parent = Selection.activeTransform.parent;
        if(parent == null){
            Debug.Log("No Parent Found");
            return;
        }

        Renderer renderer = parent.GetComponent<Renderer>();
        if(renderer == null){
            Debug.Log("No Renderer Found");
            return;
        }
        print("Old pos:"+Selection.activeTransform.position);
        Selection.activeTransform.position = renderer.bounds.center;
        print("New pos:"+Selection.activeTransform.position);
    }
}
