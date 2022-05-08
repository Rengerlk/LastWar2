using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
{
  public class Heroinputreader : MonoBehaviour
{
      [SerializeField] private Creatures.Hero.Hero _hero;
      
      
     
   
  
  public void OnMovement(InputAction.CallbackContext context)
  {
     var direction = context.ReadValue<Vector2>();
     _hero.SetDirection(direction);
  }
 


public void OnIteract(InputAction.CallbackContext context)
{
  if (context.performed)
  {
    _hero.Interact();
  }
}
public void OnAttack(InputAction.CallbackContext context)
{
  if (context.performed)
  {
    _hero.Attack();
  }
}


public void OnThrow(InputAction.CallbackContext context)
{
  if (context.started)
  {
    _hero.StartThrowing();
    
  }

  if (context.canceled)
  {
    _hero.PerfomThrowing();
  }
}

public void OnNextItem(InputAction.CallbackContext context)
{
  if (context.performed)
    _hero.NextItem();
}
}
}