using System;
using UnityEngine.EventSystems;

public static class GlobalEvents
{
    public static class DragAndDrop
    {
        public delegate void DragDelegate(IDragHandler dragHandler, PointerEventData data);
        public delegate void DragHandlerDelegate(IDragHandler dragHandler);
        
        public static event DragDelegate DragStartedEvent;
        public static event DragDelegate DragEndedEvent;
        public static event DragHandlerDelegate DragReturnedEvent;
        
        public static void InvokeDragStarted(IDragHandler dragHandler, PointerEventData data) => DragStartedEvent?.Invoke(dragHandler, data);
        public static void InvokeDragEnded(IDragHandler dragHandler, PointerEventData data) => DragEndedEvent?.Invoke(dragHandler, data);
        public static void InvokeDragReturned(IDragHandler dragHandler) => DragReturnedEvent?.Invoke(dragHandler);
    }

    public static class UI
    {
        // public delegate void ShowPopupDelegate(PopupsManager.PopupInfo info);
        public delegate void WindowDelegate(WinType winType, object data);
        
        // public static event ShowPopupDelegate ShowPopupDelegateEvent;
        public static event Action NotEnoughResourcesEvent;
        public static event Action CloseCurrentWindowEvent;
        public static event WindowDelegate OpenWindowEvent;
        
        // public static void InvokeShowPopup(PopupsManager.PopupInfo info) => ShowPopupDelegateEvent?.Invoke(info);
        public static void InvokeNotEnoughResources() => NotEnoughResourcesEvent?.Invoke();
        public static void InvokeCloseCurrentWindow() => CloseCurrentWindowEvent?.Invoke();
        public static void InvokeOpenWindow(WinType winType, object data = null) => OpenWindowEvent?.Invoke(winType, data);
    }
}
