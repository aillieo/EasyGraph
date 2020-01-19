using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public abstract class CanvasObject
    {

        public static void Draw(CanvasObject canvasObject)
        {
            if (canvasObject != null)
            {
                canvasObject.OnDraw();
            }
        }

        protected abstract void OnDraw();

        protected abstract bool RectContainsPoint(Vector2 pos);

        public bool HandleGUIEvent(Event evt)
        {
            bool handled = false;

            if(evt.isMouse && EasyGraphWindow.Instance.ViewRect.Contains(evt.mousePosition) && RectContainsPoint(evt.mousePosition))
            {
                switch (evt.type)
                {
                    // mouse
                    case EventType.MouseDown:
                        handled = OnMouseDown(evt);
                        break;
                    case EventType.MouseUp:
                        handled = OnMouseUp(evt);
                        break;
                    case EventType.ContextClick:
                        handled = OnContextClick(evt);
                        break;
                    case EventType.MouseDrag:
                        handled = OnMouseDrag(evt);
                        break;
                }
            }
            else
            {
                switch (evt.type)
                {
                    case EventType.ScrollWheel:
                        handled = OnScroll(evt);
                        break;
                    // key
                    case EventType.KeyDown:
                        handled = OnKeyDown(evt);
                        break;
                    case EventType.KeyUp:
                        handled = OnKeyUp(evt);
                        break;
                }
            }

            if (handled)
            {
                evt.Use();
            }
            return handled;
        }

        protected virtual bool OnKeyUp(Event evt)
        {
            return false;
        }

        protected virtual bool OnKeyDown(Event evt)
        {
            return false;
        }

        protected virtual bool OnMouseDown(Event evt)
        {
            return false;
        }

        protected virtual bool OnMouseUp(Event evt)
        {
            return false;
        }

        protected virtual bool OnContextClick(Event evt)
        {
            return false;
        }

        protected virtual bool OnMouseDrag(Event evt)
        {
            return false;
        }

        protected virtual bool OnScroll(Event evt)
        {
            return false;
        }

    }

}
