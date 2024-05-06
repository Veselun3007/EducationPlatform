using StudentResult.Application.Abstractions;

namespace StudentResult.Application.Queries.GetFileLink {
    public class GetFileLinkQuery : IQuery<string> {
        public GetFileLinkQuery() { }
        public GetFileLinkQuery(int fileId, string userId) {
            FileId = fileId;
            UserId = userId;
        }
        public int FileId { get; set; }
        public string UserId { get; set; }
    }
}
