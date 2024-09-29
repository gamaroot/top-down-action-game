using UnityEngine.InputSystem;

namespace Utils
{
    public static class Vibration
    {
        public static void StartHapticFeedback()
        {
            Gamepad.current?.SetMotorSpeeds(0.1f, 0.1f);
        }

        public static void StartLowVibration()
        {
            Gamepad.current?.SetMotorSpeeds(0.2f, 0.2f);
        }

        public static void StartMediumVibration()
        {
            Gamepad.current?.SetMotorSpeeds(0.5f, 0.5f);
        }

        public static void StartHighVibration()
        {
            Gamepad.current?.SetMotorSpeeds(1f, 1f);
        }

        public static void Stop()
        {
            Gamepad.current?.SetMotorSpeeds(0, 0);
        }
    }
}