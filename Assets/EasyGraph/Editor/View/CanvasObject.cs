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
            switch (@event.type)
            {
                // mouse
                case EventType.MouseDown:
                    if (@event.button == 0)
                    {
                        return OnMouseDown(@event.mousePosition);
                    }
                    break;
                case EventType.MouseUp:
                    if (@event.button == 0)
                    {
                        return OnMouseUp(@event.mousePosition);
                    }
                    break;
                case EventType.ContextClick:
                    return OnContextClick(@event.mousePosition);
                case EventType.MouseDrag:
                    if (@event.button == 0)
                    {
                        return OnMouseDrag(@event.mousePosition, @event.delta);
                    }
                    break;
                case EventType.ScrollWheel:
                    return OnScroll(@event.mousePosition, @event.delta.y);
                // key
                case EventType.KeyDown:
                    return OnKeyDown(@event.keyCode);
                case EventType.KeyUp:
                    return OnKeyUp(@event.keyCode);
            }
            return false;
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
