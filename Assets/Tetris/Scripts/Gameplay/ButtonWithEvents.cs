using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tetris.Gameplay
{
  public class ButtonWithEvents : Button
  {
    public event Action<bool> OnStatePress;
    
    public override void OnPointerDown(PointerEventData eventData)
    {
      base.OnPointerDown(eventData);
      OnStatePress?.Invoke(true);
    }
    
    public override void OnPointerUp(PointerEventData eventData)
    {
      base.OnPointerDown(eventData);
      OnStatePress?.Invoke(false);
    }
  }
}