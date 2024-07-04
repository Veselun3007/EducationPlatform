namespace CourseContent.Tests.Config
{
    public static class Setup
    {
        public const string Token = "eyJraWQiOiJFSEd1RUFMdlA0XC82VjlmRTVjM1NRWUFjS0Q1bVRaYStQaTJFbmw0ZHB2VT0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiI5NDU4NjRlOC0zMGUxLTcwMTAtNjM3Ny03OWQzOWUwYzMyNjEiLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAudXMtZWFzdC0xLmFtYXpvbmF3cy5jb21cL3VzLWVhc3QtMV9QbGVtQzFDUzUiLCJjbGllbnRfaWQiOiI0bWloOG9pb3Nta2dubThibmw3YzVhZGQzMiIsIm9yaWdpbl9qdGkiOiJhMTc3NWViMC1kNjM0LTQyYjQtYTZiMC1lYjE5YTY3ODlhNTgiLCJldmVudF9pZCI6IjhhMTVjZmE0LTc5NGQtNGNiMS1iZjc2LWU3NjMzN2M3YzdmMSIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4iLCJhdXRoX3RpbWUiOjE3MTkzODE3MDYsImV4cCI6MTcxOTQ2ODEwNiwiaWF0IjoxNzE5MzgxNzA2LCJqdGkiOiIxMmE1NDYwNS0zZWRjLTQ1MmYtODdkMS1jNTA0OWY1ZTZmZjgiLCJ1c2VybmFtZSI6Ijk0NTg2NGU4LTMwZTEtNzAxMC02Mzc3LTc5ZDM5ZTBjMzI2MSJ9.APgMYhw_hJxVGBTdvFE0D3QRbGIQEaYSt1MZ1XGbBVq38Wt1de5vAltuw2dJXgpdd5BIQquyNIJLsqXbNTkWF6X0mRJwhVJh5qzST1blnQ7By0GUWmIf_8UVtiaOKKUzoOZPgZUtrlGUltmJ3tE8VWFG1RMtWFAPdjD9-FEABPO1xqrPjI3JFqU6K66W9d4Ra9XTS8Q5Xzl5l9b-tTC7sbI5j8Q0NiBHvN8rD50rdHFqodF89iELo8LwYnokfK5JwQVszcbauJ30Zzjoo8ywZtUxo40XkeIcDwkx1FWqUQzBZVIMw-Qqyp7NgnIHChmogbyhdLrdKb8IYZ8a6jGu0g";
        public const string BucketName = "educationplatform";
        public const ushort LocalStackPort = 4566;
        public const string Region = "us-east-1";
        public const string AwsAccessKey = "dummy-access-key";
        public const string AwsSecretKey = "dummy-secret-key";
        public const bool ForcePathStyle = true;
        public const string AssignmentBaseURL = "https://localhost:5002/api/assignment";
        public const string MaterialBaseURL = "https://localhost:5002/api/material";
        public const string TopicBaseURL = "https://localhost:5002/api/topic";

        public static string ToAbsolute(string path) => Path.GetFullPath(path);
    }
}
