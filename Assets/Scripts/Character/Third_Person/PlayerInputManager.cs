using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Third_Person
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput _playerInput;
        [SerializeField]
        private Third_Person.PlayerController _inputHandler;

        private void Awake()
        {
            if (_playerInput == null)
            {
                _playerInput = new PlayerInput();
                return;
            }

            if (!_playerInput.Player.enabled)
            {
                _playerInput.Player.Enable();
            }

        }


        private void OnEnable()
        {
            _playerInput.Player.Move.performed += OnMovePerformed;
            _playerInput.Player.Move.canceled += OnMoveCanceled;
            _playerInput.Player.Move.Enable();

            _playerInput.Player.Jump.performed += OnJumpPerformed;
            _playerInput.Player.Jump.canceled += OnJumpCanceled;
            _playerInput.Player.Jump.Enable();

            _playerInput.Player.Vaccum.performed += OnVaccumPerformed;
            _playerInput.Player.Vaccum.canceled += OnVaccumCanceled;
            _playerInput.Player.Vaccum.Enable();
            
            _playerInput.Player.Interact.performed += OnInteractPerformed;
            _playerInput.Player.Interact.Enable();
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        private void OnVaccumCanceled(InputAction.CallbackContext context)
        {
            handleVaccumToggle();
        }

        private void OnVaccumPerformed(InputAction.CallbackContext context)
        {
            handleVaccumToggle();
        }

        private void handleVaccumToggle()
        {
            _inputHandler.ToggleVaccum();
        }

        private void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            Vector2 moveValue = ctx.ReadValue<Vector2>();
            _inputHandler.HandleMovementInput(moveValue);
        }

        private void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            Vector2 moveValue = Vector2.zero;
            _inputHandler.HandleMovementInput(moveValue);
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            Debug.Log("Jump Performed");
            _inputHandler.HandleJumpInput(true);
        }

        private void OnJumpCanceled(InputAction.CallbackContext context)
        {
            _inputHandler.HandleJumpInput(false);
        }
    }
}