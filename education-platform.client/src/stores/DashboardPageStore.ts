import { action, computed, makeObservable, observable } from 'mobx';
import IStore from './common/IStore';
import AuthService from '../services/AuthService';
import RootStore from './RootStore';
import CourseModel from '../models/course/CourseModel';

export default class DashboardPageStore implements IStore {
    private readonly _authService: AuthService;
    private readonly _rootStore: RootStore;

    courses: CourseModel[] = [
        {
            courseId: 1,
            courseName: 'Course name 1 ВАП ВФ ІАР ПВІАПВАП ВАПВА ВІПІПІВП',
            courseDescription:
                'ThiThis is description for cource 1s is description for cource 1 This is description for cource 1, This is description for cource 1 This is description for cource 1This is description for cource 1',
            courseLink: 'link 1',
        },
        {
            courseId: 2,
            courseName: 'Course name 2',
            courseDescription: 'This is description for cource 2',
            courseLink: 'link 2',
        },
        {
            courseId: 3,
            courseName: 'Course name 3',
            courseDescription: 'This is description for cource 3',
            courseLink: 'link 3',
        },
        {
            courseId: 4,
            courseName: 'Course name 4',
            courseDescription: 'This is description for cource 4',
            courseLink: 'link 4',
        },
        {
            courseId: 5,
            courseName: 'Course name 5',
            courseDescription: 'This is description for cource 5',
            courseLink: 'link 5',
        },
        {
            courseId: 6,
            courseName: 'Course name 6',
            courseDescription: 'This is description for cource 5',
            courseLink: 'link 5',
        },
        {
            courseId: 7,
            courseName: 'Course name 7',
            courseDescription: 'This is description for cource 5',
            courseLink: 'link 5',
        },
        {
            courseId: 8,
            courseName: 'Course name 8',
            courseDescription: 'This is description for cource 5',
            courseLink: 'link 5',
        },
        {
            courseId: 9,
            courseName: 'Course name 9',
            courseDescription: 'This is description for cource 5',
            courseLink: 'link 5',
        },
        {
            courseId: 10,
            courseName: 'Course name 10',
            courseDescription: 'This is description for cource 5',
            courseLink: 'link 5',
        },
        {
            courseId: 11,
            courseName: 'Course name 11',
            courseDescription: 'This is description for cource 5',
            courseLink: 'link 5',
        },
        {
            courseId: 12,
            courseName: 'Course name 12',
            courseDescription: 'This is description for cource 5',
            courseLink: 'link 5',
        },
        {
            courseId: 13,
            courseName: 'Course name 13',
            courseDescription: 'This is description for cource 5',
            courseLink: 'link 5',
        },
        {
            courseId: 14,
            courseName: 'Course name 14',
            courseDescription: 'This is description for cource 5',
            courseLink: 'link 5',
        },
    ];

    constructor(rootStore: RootStore, authService: AuthService) {
        this._authService = authService;
        this._rootStore = rootStore;

        makeObservable(this, {
            courses: observable,

            reset: action.bound,
        });
    }

    reset(): void {
        //this.courses = [];
        console.log('reset');
    }
}
