import AttachedFileModel from './AttachedFileModel';
import CommentInfoModel from './CommenttInfoModel';
import StudentAssignmentModel from './StudentAssinmentModel';
import UserInfoModel from './UserInfoModel';

export default interface SAInfoModel {
    studentAssignment: StudentAssignmentModel;
    comments: CommentInfoModel[];
    files: AttachedFileModel[];
    userInfo: UserInfoModel;
}
