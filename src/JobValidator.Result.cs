namespace WeAreUnited;

partial struct JobValidator
{
    public readonly ref struct Result
    {
        public Result(bool success, TaggedString? reason = null)
        {
            Success = success;
            Reason = reason;
            if(!success && reason is not null) JobFailReason.Is(this);
        }

        public readonly bool Success;
        public readonly TaggedString? Reason;
        
        public static implicit operator Result(bool success) => new(success);
        public static implicit operator Result(TaggedString reason) => new(false, reason);
        public static implicit operator Result(string reason) => new(false, reason);

        public static implicit operator bool(Result result) => result.Success;
        public static implicit operator string(Result result) => result.ToString();

        public override string ToString() => Reason ?? "";
    }  
}