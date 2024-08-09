namespace WebSocketMain.Utility.Messages
{
    public class WebSocketCloseStatusCodesMessage
    {
        public const string NormalClosure = "1000: Normal Closure";
        public const string GoingAway = "1001: Going Away";
        public const string ProtocolError = "1002: Protocol Error";
        public const string UnsupportedData = "1003: Unsupported Data";
        public const string NoStatusReceived = "1005: No Status Received";
        public const string AbnormalClosure = "1006: Abnormal Closure";
        public const string InvalidFramePayloadData = "1007: Invalid Frame Payload Data";
        public const string PolicyViolation = "1008: Policy Violation";
        public const string MessageTooBig = "1009: Message Too Big";
        public const string MissingExtension = "1010: Missing Extension";
        public const string InternalServerError = "1011: Internal Server Error";
        public const string ServiceRestart = "1012: Service Restart";
        public const string TryAgainLater = "1013: Try Again Later";
        public const string BadGateway = "1014: Bad Gateway";
        public const string TLSHandshake = "1015: TLS Handshake";
    }
}
