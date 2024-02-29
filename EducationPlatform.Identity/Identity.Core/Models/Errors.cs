namespace Identity.Core.Models
{
    public static class Errors
    {
        public static class Identity
        {
            public static Error UsernameExist(string email) =>
                new("identity.username.exist", $"User with email {email} already exists");

            public static Error CodeMismatch() =>
                new("identity.code.mismatch", "Code is not correct");

            public static Error ExpiredCode() =>
                new("identity.code.mismatch", "Code is expired");
        }

        public static class General
        {
            public static Error NotFound() =>
                new("record.not.found", "No object with this key was found");
        }
    }
}
