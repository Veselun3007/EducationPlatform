/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { action, makeObservable, observable, runInAction } from "mobx";
import AssignmentService from "../services/AssignmentService";
import StudentAssignmentService from "../services/StudentAssignmentService";
import RootStore from "./RootStore";
import { NavigateFunction } from "react-router-dom";
import AssignmentModel from "../models/assignment/AssignmentModel";
import SAInfoModel from "../models/studentAssignment/SAInfoModel";
import LoginRequiredError from "../errors/LoginRequiredError";
import { enqueueAlert } from "../components/Notification/NotificationProvider";
import ServiceError from "../errors/ServiceError";
import UpdateMarkModel from "../models/studentAssignment/UpdateMarkModel";
import CreateCommentModel from "../models/studentAssignment/CreateComentModel";

export default class MarkWorksPageStore {
    private readonly _rootStore: RootStore;
    private readonly _assignmentService: AssignmentService;
    private readonly _saService: StudentAssignmentService;

    assignment: AssignmentModel | null = null;
    saInfo: SAInfoModel[] | null = null

    isLoading = true;
    currentUser: number | null = null;

    isFileViewerOpen = false;
    fileLink = '';

    constructor(rootStore: RootStore, assignmentService: AssignmentService, saService: StudentAssignmentService) {
        this._rootStore = rootStore;
        this._assignmentService = assignmentService;
        this._saService = saService;
        makeObservable(this, {
            assignment: observable,
            saInfo: observable,
            isLoading: observable,
            currentUser: observable,
            isFileViewerOpen: observable,
            fileLink: observable,

            init: action.bound,
            reset: action.bound,
            submitMark: action.bound,
            sendComment: action.bound,
            onWorkFileClick: action.bound,
            onFileViewerClose: action.bound,
        })
    }

    async init(courseId: number, assignmentId: number, navigate: NavigateFunction) {
        try {
            const course = this._rootStore.courseStore.coursesInfo.find(c => c.course.courseId === courseId);
            if (!course) navigate('/');
            const user = course!.userInfo;
            if (user.role === 2) {
                navigate(`/course/${courseId}`);
                return;
            }

            runInAction(() => {
                this.currentUser = user.courseuserId;
            });

            const assignment = await this._assignmentService.getAssignmentById(assignmentId);
            const saInfo = await this._saService.getStudentAssignments(assignmentId)
            runInAction(() => {
                this.saInfo = saInfo;
                this.assignment = assignment;
                this.isLoading = false;
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

    async onWorkFileClick(id: number, navigate: NavigateFunction) {
        try {
            const link = await this._saService.getFileLink(id)
            runInAction(() => {
                this.isFileViewerOpen = true;
                this.fileLink = link;
            })
        } catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                enqueueAlert((error as ServiceError).message, 'error');
            }
        }
    }

    onFileViewerClose() {
        this.isFileViewerOpen = false;
        this.fileLink = '';
    }

    async submitMark(mark: UpdateMarkModel, navigate: NavigateFunction) {
        try {
            const sa = await this._saService.updateMark(mark);
            runInAction(() => {
                const index = this.saInfo!.findIndex(s => s.studentAssignment.studentassignmentId === sa.studentAssignment.studentassignmentId)
                this.saInfo![index] = sa;
            });
        }
        catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                enqueueAlert((error as ServiceError).message, 'error');
            }
        }
    }

    async sendComment(comment: CreateCommentModel, navigate: NavigateFunction) {
        try {
            const commentInfo = await this._saService.createComment(comment);
            runInAction(() => {
                const index = this.saInfo!.findIndex(s => s.studentAssignment.studentassignmentId === comment.studentAssignmentId);
                this.saInfo![index].comments = commentInfo
            })
        }
        catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                enqueueAlert((error as ServiceError).message, 'error');
            }
        }
    }

    reset() {
        this.saInfo = null;
        this.assignment = null;
        this.isLoading = true;
        this.currentUser = null;

        this.isFileViewerOpen = false;
        this.fileLink = '';
    }
}