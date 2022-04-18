namespace FlushTheToiletWebServer.CF01
{
    public static class PinAssignment
    {
        // Motor
        public const byte FLUSH_OUTPUT = 24;
        public const byte STOP_FLUSH_OUTPUT = 23;

        // LEDs
        public const byte RED_LED = 16;
        public const byte YELLOW_LED = 20;
        public const byte GREEN_LED = 21;
        public const byte BLUE_LED = 26;

        // HCSR04 (supre sonic sensor)
        public const byte TRIGGER_PIN = 13;
        public const byte ECHO_PIN = 18;

    }
}
