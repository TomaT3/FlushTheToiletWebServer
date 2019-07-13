namespace FlushTheToiletWebServer.CF01
{
    public static class PinAssignment
    {
        // Motor
        public const byte FLUSH_OUTPUT = 20;
        public const byte STOP_FLUSH_OUTPUT = 21;

        // LEDs
        public const byte RED_LED = 22;
        public const byte YELLOW_LED = 23;
        public const byte GREEN_LED = 24;

        // HCSR04 (supre sonic sensor)
        public const byte TRIGGER_PIN = 17;
        public const byte ECHO_PIN = 18;

    }
}
