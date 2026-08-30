namespace MM.Inspector.Editor
{
    public readonly struct MMValidationResult
    {
        public readonly bool IsValid;
        public readonly MMValidationSeverity Severity;
        public readonly string Message;

        private MMValidationResult(bool isValid, MMValidationSeverity severity, string message)
        {
            IsValid = isValid;
            Severity = severity;
            Message = message;
        }

        public static MMValidationResult Valid => new MMValidationResult(true, MMValidationSeverity.Info, null);

        public static MMValidationResult Error(string message) => new MMValidationResult(false, MMValidationSeverity.Error, message);

        public static MMValidationResult Warning(string message) => new MMValidationResult(false, MMValidationSeverity.Warning, message);

        public static MMValidationResult Info(string message) => new MMValidationResult(false, MMValidationSeverity.Info, message);
    }
}
