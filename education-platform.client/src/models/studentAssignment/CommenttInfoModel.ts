import CommentModel from "./CommentModel";
import UserInfoModel from "./UserInfoModel";

export default interface CommentInfoModel {
    comment: CommentModel;
    userInfo: UserInfoModel;
}