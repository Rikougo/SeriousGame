using UnityEngine;
public class CameraController : MonoBehaviour
{
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    private const float dureeValue = 5.2F;
    private float duree = dureeValue;
    private float tempspasse = 0F;

    private void Update()
    {
        if (tempspasse < duree){
            
            transform.position = Vector3.Lerp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), tempspasse/duree);
            tempspasse += Time.deltaTime;
        }
    }

    public void MoveToNewTabGrid(Vector3 _newTabGrid)
    {
        duree = dureeValue;
        tempspasse = 0F;
        currentPosX = _newTabGrid.x;
    }
}