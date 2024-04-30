export default class CreateCourseUserModel {
    courseLink: string;
    userId?: string;

    constructor(courseLink: string, userId?: string) {
        this.courseLink = courseLink;
        this.userId = userId;
    }
}