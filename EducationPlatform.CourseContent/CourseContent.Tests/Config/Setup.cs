namespace CourseContent.Tests.Config
{
    public static class Setup
    {
        public const string Token = "eyJraWQiOiJFSEd1RUFMdlA0XC82VjlmRTVjM1NRWUFjS0Q1bVRaYStQaTJFbmw0ZHB2VT0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiI5NDU4NjRlOC0zMGUxLTcwMTAtNjM3Ny03OWQzOWUwYzMyNjEiLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAudXMtZWFzdC0xLmFtYXpvbmF3cy5jb21cL3VzLWVhc3QtMV9QbGVtQzFDUzUiLCJjbGllbnRfaWQiOiI0bWloOG9pb3Nta2dubThibmw3YzVhZGQzMiIsIm9yaWdpbl9qdGkiOiJjNTYxYjVmNC0xNTdjLTRmYjYtYWQ4ZS1lMGUyMmU4ZGM1NjMiLCJldmVudF9pZCI6ImJlODBlOTZlLWIxZjktNGE1MC1iM2Q0LTcyYjA2YzhmY2ZlMCIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4iLCJhdXRoX3RpbWUiOjE3MTkyMzYzNzYsImV4cCI6MTcxOTMyMjc3NiwiaWF0IjoxNzE5MjM2Mzc2LCJqdGkiOiJiY2MyYTcwYi04YWI5LTQ5MDktOWRiYS1mNjdmMjQzMjY1MDIiLCJ1c2VybmFtZSI6Ijk0NTg2NGU4LTMwZTEtNzAxMC02Mzc3LTc5ZDM5ZTBjMzI2MSJ9.vjEYF5i6OepRO-Qv67ABNXMyYL5NEa4sTdj6sBwvzENL_rjZma60mKyxn3hwlF5riiB7Iy5FgbfJOEFWjY12aKNOnY5LTr4VjNYUUJgi9AqCulaTqpfPSNulHNUCtL-Oqg9edsIZxxlPC9aUyBJq1TuLzF9ogqjOWPdDsjB3ea_ZCiTZMIqwX5oNUmnWNcG9RkNDJBp5B0-myGONhh1L_sdE9m6DpV_P7jXwNgXATcp7GVegM3RfQRqB_tOfvTseDr0KfWGbyEjkwWca-XByPae1Miw2c7l8EtNFYWUW134OqVKnd7pEZqh4f6qTsWEC1THe5hL29_e9UUTEk8rJJg";
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
