export default class DeleteCourseUserModel {
    courseuserId: number;
    userId?: string

    constructor(courseuserId: number) {
        this.courseuserId = courseuserId;
    }
}