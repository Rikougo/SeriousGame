using UnityEngine;

//This script is used by both movement and jump to detect when the character is touching the ground

[ExecuteInEditMode]
public class characterGround : MonoBehaviour
{
        private bool onGround;
        private bool againstWall;
       
        [Header("Collider Settings")]
        [SerializeField][Tooltip("Length of the ground-checking collider")] private float groundLength = 0.95f;

        [SerializeField] [Tooltip("Length of the wall-checking collider")] private float wallLength = 0.95f;
        [SerializeField][Tooltip("Distance between the ground-checking colliders")] private Vector3 groundOffset;
        [SerializeField][Tooltip("Distance between the ground-checking colliders")] private Vector3 wallOffset;
        

        [Header("Layer Masks")]
        [SerializeField][Tooltip("Which layers are read as the ground")] private LayerMask groundLayer;
 

        private void Update()
        {
            Vector3 l_position = transform.position;
            //Determine if the player is stood on objects on the ground layer, using a pair of raycasts
            onGround = Physics2D.Raycast(l_position - groundOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(l_position + groundOffset, Vector2.up, groundLength, groundLayer);
            
            // left && right
            againstWall = Physics2D.Raycast(l_position - wallOffset, Vector2.left, groundLength, groundLayer) || Physics2D.Raycast(l_position + wallOffset, Vector2.right, wallLength, groundLayer);
        }

        private void OnDrawGizmos()
        {
            //Draw the ground colliders on screen for debug purposes

            Vector3 l_pos = transform.position;
            if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
            Gizmos.DrawLine(l_pos + groundOffset,l_pos + groundOffset + Vector3.up * groundLength);
            Gizmos.DrawLine(l_pos - groundOffset,l_pos - groundOffset + Vector3.down * groundLength);
            
            if (againstWall) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
            Gizmos.DrawLine(l_pos + wallOffset, l_pos + wallOffset + Vector3.right * wallLength);
            Gizmos.DrawLine(l_pos - wallOffset, l_pos - wallOffset + Vector3.left * wallLength);
        }

        //Send ground detection to other scripts
        public bool GetOnGround() { return onGround; }

        public bool GetAgainstWall() { return againstWall; }
}