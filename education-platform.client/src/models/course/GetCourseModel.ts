export default class GetAllCourses {
    userId: string;
    courseId: number;

    constructor(userId: string, courseId: number) {
        this.userId = userId;
        this.courseId = courseId;
    }
}