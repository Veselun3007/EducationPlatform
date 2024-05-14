import AdminInfoModel from './AdminInfoModel';
import CourseModel from './CourseModel';
import UserInfoModel from './UserInfoModel';

export default interface CourseInfoModel {
    course: CourseModel;
    userInfo: UserInfoModel;
    adminInfo: AdminInfoModel;
}
