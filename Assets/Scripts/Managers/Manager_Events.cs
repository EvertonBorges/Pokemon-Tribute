using UnityEngine;
using static Observer;
using Event = Observer.Event;

public static class Manager_Events
{

    public static class Player
    {
        public static Event<Vector2> OnMovement = new();
        public static Event OnButtonA = new();
        public static Event OnButtonB = new();
        public static Event OnButtonPause = new();
        public static Event OnButtonSelect = new();
    }

    public static class GameManager
    {
        public static Event Pause = new();
        public static Event Unpause = new();
    }

}
