export default class UpdateCourseUserModel {
    userId?: string;
    courseuserId: number;
    role: number;

    constructor(courseuserId: number, role: number,userId?: string) {
        this.userId = userId;
        this.courseuserId = courseuserId;
        this.role = role;
    }
}