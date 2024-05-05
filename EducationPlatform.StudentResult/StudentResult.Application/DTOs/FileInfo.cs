using StudentResult.Domain.Entities;

namespace StudentResult.Application.DTOs {
    public class FileInfo {
        public FileInfo() {}
        public FileInfo(AttachedFile attachedFile) {
            AttachedFileId = attachedFile.AttachedFileId;
            AttachedFileName = attachedFile.AttachedFileName;
        }
        public int AttachedFileId { get; set; }
        public string AttachedFileName { get; set; }

        public static List<FileInfo> ToList(ICollection<AttachedFile> attachedFiles) {
            List<FileInfo> fileInfos = new List<FileInfo>();
            foreach (AttachedFile file in attachedFiles) {
                FileInfo fileInfo = new FileInfo(file);
                fileInfos.Add(fileInfo);
            }
            return fileInfos;
        }
    }
}
