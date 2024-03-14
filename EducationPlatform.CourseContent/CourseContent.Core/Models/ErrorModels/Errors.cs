namespace Identity.Core.Models
{
    public static class Errors
    {
        public static class General
        {
            public static Error NotFound() =>
                new("record.not.found", "No object with this key was found");
        }
    }
}
