using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public abstract class CanvasObject
    {

        public static void Draw(CanvasObject canvasObject)
        {
            if(canvasObject != null)
            {
                canvasObject.OnDraw();
            }
        }

        protected abstract void OnDraw();


        public bool HandleGUIEvent(Event @event)
        {
            bool handled = false;
            switch (@event.type)
            {
                // mouse
                case EventType.MouseDown:
                    if (@event.button == 0)
                    {
                        handled = OnMouseDown(@event.mousePosition);
                    }
                    break;
                case EventType.MouseUp:
                    if (@event.button == 0)
                    {
                        handled = OnMouseUp(@event.mousePosition);
                    }
                    break;
                case EventType.ContextClick:
                    handled = OnContextClick(@event.mousePosition);
                    break;
                case EventType.MouseDrag:
                    if (@event.button == 2)
                    {
                        handled = OnMouseDrag(@event.mousePosition, @event.delta);
                    }
                    break;
                case EventType.ScrollWheel:
                    handled = OnScroll(@event.mousePosition, @event.delta.y);
                    break;
                // key
                case EventType.KeyDown:
                    handled = OnKeyDown(@event.keyCode);
                    break;
                case EventType.KeyUp:
                    handled = OnKeyUp(@event.keyCode);
                    break;
            }
            if(handled)
            {
                @event.Use();
            }
            return handled;
        }

        protected virtual bool OnKeyUp(KeyCode keyCode)
        {
            return false;
        }

        protected virtual bool OnKeyDown(KeyCode keyCode)
        {
            return false;
        }

        protected virtual bool OnMouseDown(Vector2 pos)
        {
            return false;
        }

        protected virtual bool OnMouseUp(Vector2 pos)
        {
            return false;
        }

        protected virtual bool OnContextClick(Vector2 pos)
        {
            return false;
        }

        protected virtual bool OnMouseDrag(Vector2 pos, Vector2 delta)
        {
            return false;
        }

        protected virtual bool OnScroll(Vector2 pos, float delta)
        {
            return false;
        }

    }

}
