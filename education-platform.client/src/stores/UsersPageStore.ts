import { action, makeObservable, observable, runInAction } from "mobx";
import CourseUserService from "../services/CourseUserService";
import RootStore from "./RootStore";
import CourseUserInfoModel from "../models/courseUser/CourseUserInfoModel";
import LoginRequiredError from "../errors/LoginRequiredError";
import { NavigateFunction } from "react-router-dom";
import { enqueueAlert } from "../components/Notification/NotificationProvider";
import ServiceError from "../errors/ServiceError";
import { SelectChangeEvent } from "@mui/material";
import UpdateCourseUserModel from "../models/courseUser/UpdateCourseUserModel";
import DeleteCourseUserModel from "../models/courseUser/DeleteCourseUserModel";

export default class UsersPageStore {

    private readonly _rootStore: RootStore;
    private readonly _courseUserService: CourseUserService;

    users: CourseUserInfoModel[] = [];

    isLoading = true;
    isAdmin = false;

    constructor(rootStore: RootStore, courseUserService: CourseUserService) {
        this._rootStore = rootStore;
        this._courseUserService = courseUserService;

        makeObservable(this, {
            isAdmin: observable,
            users: observable,
            isLoading: observable,

            init: action.bound,
            onUserRoleChange: action.bound,
            kickUser: action.bound,
            reset: action.bound,
        });
    }

    async init(courseId: number, navigate: NavigateFunction) {
        try {
            const users = await this._courseUserService.getUsers(courseId);
            runInAction(() => {
                this.users = users;
                this.isLoading = false;

                const course = this._rootStore.courseStore.coursesInfo.find(c => c.course.courseId === courseId);
                if (!course) navigate('/');
                this.isAdmin = course?.userInfo.role === 0;
            });
        } catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                navigate(`/course/${courseId}`);
                enqueueAlert((error as ServiceError).message, 'error');
            }
        }
    }

    async onUserRoleChange(event: SelectChangeEvent<number>, navigate: NavigateFunction, courseUserId: number) {
        try {
            let role: number;
            if (typeof (event.target.value) === 'string') {
                role = Number.parseInt(event.target.value);
            } else {
                role = event.target.value;
            }
            const updatedUser = await this._courseUserService.updateCourseUser(
                new UpdateCourseUserModel(courseUserId, role)
            );

            runInAction(() => {
                const index = this.users.findIndex(u => u.courseuserId === updatedUser.courseuserId);
                this.users[index] = updatedUser;
                enqueueAlert('glossary.editSuccess', 'success');
            });

        } catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                enqueueAlert((error as ServiceError).message, 'error');
            }
        }
    }

    async kickUser(courseUserId: number, navigate: NavigateFunction) {
        try {
            await this._courseUserService.deleteCourseUser(new DeleteCourseUserModel(courseUserId));

            runInAction(() => {
                const index = this.users.findIndex(u => u.courseuserId === courseUserId);
                this.users.splice(index, 1);
                enqueueAlert('glossary.kickUserSuccess', 'success');
            });

        } catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                enqueueAlert((error as ServiceError).message, 'error');
            }
        }
    }

    reset() {
        this.isLoading = true;
        this.isAdmin = false;
        this.users = [];
    }
}