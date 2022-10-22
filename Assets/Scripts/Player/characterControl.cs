using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class characterControl : MonoBehaviour
    {
        private PlayerInput m_input;
        private characterMovement m_movement;
        private characterJump m_jump;

        private void Awake()
        {
            m_input = GetComponent<PlayerInput>();
            m_movement = GetComponent<characterMovement>();
            m_jump = GetComponent<characterJump>();
        }

        private void OnEnable()
        {
            m_input.actions["Move"].performed += m_movement.OnMovement;
            m_input.actions["Move"].canceled += m_movement.OnMovement;

            m_input.actions["Test"].started += m_jump.OnJump;
            m_input.actions["Test"].canceled += m_jump.OnJump;
            
            m_input.actions["Dash"].started += m_movement.OnDash;
        }
        
        private void OnDisable() {
            m_input.actions["Move"].performed -= m_movement.OnMovement;
            m_input.actions["Move"].canceled -= m_movement.OnMovement;

            m_input.actions["Test"].started -= m_jump.OnJump;
            m_input.actions["Test"].canceled -= m_jump.OnJump;
            
            m_input.actions["Dash"].started -= m_movement.OnDash;
        }
    }
}