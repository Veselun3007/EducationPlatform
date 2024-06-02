/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { action, computed, makeObservable, observable, runInAction } from 'mobx';
import RootStore from './RootStore';
import ValidationError from '../helpers/validation/ValidationError';
import AssignmentModel from '../models/assignment/AssignmentModel';
import debounce from '../helpers/debounce';
import { enqueueAlert } from '../components/Notification/NotificationProvider';
import { NavigateFunction } from 'react-router-dom';
import { SelectChangeEvent } from '@mui/material';
import { Dayjs } from 'dayjs';
import TopicModel from '../models/topic/TopicModel';
import LoginRequiredError from '../errors/LoginRequiredError';
import ServiceError from '../errors/ServiceError';
import UpdateAssignmentModel from '../models/assignment/UpdateAssignmentModel';
import AssignmentService from '../services/AssignmentService';
import TopicService from '../services/TopicService';
import FileValidator from '../helpers/validation/FileValidator';
import StringValidator from '../helpers/validation/StringValidator';
import UpdateWorkModel from '../models/studentAssignment/UpdateWorkModel';
import StudentAssignmentService from '../services/StudentAssignmentService';
import SAInfoModel from '../models/studentAssignment/SAInfoModel';
import CreateCommentModel from '../models/studentAssignment/CreateComentModel';

export default class AssignmentPageStore {
    private readonly _rootStore: RootStore;
    private readonly _assignmentService: AssignmentService;
    private readonly _topicService: TopicService;
    private readonly _saService: StudentAssignmentService;

    editAssignmentData: UpdateAssignmentModel | null = null;
    editAssignmentErrors: Record<string, ValidationError | null> = {
        assignmentName: null,
        maxMark: null,
        minMark: null,
        isRequired: null,
        assignmentDeadline: null,
        assignmentFiles: null,
        assignmentLinks: null,
        meta: null,
    };

    saInfo: SAInfoModel | null = null;

    work: UpdateWorkModel | null = null;
    workErrors: Record<string, ValidationError | null> = {
        workFiles: null,
        meta: null,
    };

    topics: TopicModel[] | null = [];

    assignmentMenuAnchor: null | HTMLElement = null;

    editAssignmentOpen = false;

    assignment: AssignmentModel | null = null;

    isFileViewerOpen = false;
    fileLink = '';

    isLoading = true;

    linkAdd = '';
    isLinkAddOpen = false;

    isTeacher = false;

    isEditWork = true;
    currentUser: number | null = null;

    constructor(
        rootStore: RootStore,
        assignmentService: AssignmentService,
        topicService: TopicService,
        saService: StudentAssignmentService,
    ) {
        this._rootStore = rootStore;
        this._assignmentService = assignmentService;
        this._topicService = topicService;
        this._saService = saService;
        makeObservable(this, {
            currentUser: observable,
            isEditWork: observable,
            isTeacher: observable,
            linkAdd: observable,
            isLinkAddOpen: observable,
            topics: observable,
            isLoading: observable,
            editAssignmentOpen: observable,
            assignmentMenuAnchor: observable,
            editAssignmentData: observable,
            editAssignmentErrors: observable,
            assignment: observable,
            workErrors: observable,
            work: observable,
            isFileViewerOpen: observable,
            fileLink: observable,
            saInfo: observable,

            isEditAssignmentValid: computed,
            isWorkValid: computed,

            init: action.bound,
            onWorkFileAdd: action.bound,
            onWorkFileDelete: action.bound,
            submitEditAssignment: action.bound,
            reset: action.bound,
            onFileClick: action.bound,
            resetEditAssignment: action.bound,
            onFileViewerClose: action.bound,
            openAssignmentMenu: action.bound,
            closeAssignmentMenu: action.bound,
            handleEditAssignmentClose: action.bound,
            handleEditAssignmentOpen: action.bound,
            deleteAssignment: action.bound,
            onAssignmentDeadlineChange: action.bound,
            onAssignmentDescriptionChange: action.bound,
            onAssignmentFileAdd: action.bound,
            onAssignmentFileDelete: action.bound,
            onAssignmentIsRequiredChange: action.bound,
            onAssignmentLinkAdd: action.bound,
            onAssignmentLinkDelete: action.bound,
            onAssignmentMaxMarkChange: action.bound,
            onAssignmentMinMarkChange: action.bound,
            onAssignmentNameChange: action.bound,
            onAssignmentTopicChange: action.bound,
            onLinkAddChange: action.bound,
            handleLinkAddClose: action.bound,
            handleLinkAddOpen: action.bound,
            onWorkFileClick: action.bound,
            enableEditMode: action.bound,
            disableEditMode: action.bound,
            submitWorkUpdate: action.bound,
            resetEditWork: action.bound,
            sendComment: action.bound,
        });
    }

    get isEditAssignmentValid(): boolean {
        return (
            this.editAssignmentData!.validateAssignmentDeadline().length === 0 &&
            this.editAssignmentData!.validateAssignmentName().length === 0 &&
            this.editAssignmentData!.validateIsRequired().length === 0 &&
            this.editAssignmentData!.validateMaxMark().length === 0 &&
            this.editAssignmentData!.validateMinMark().length === 0
        );
    }

    get isWorkValid(): boolean {
        return this.work!.validateAssignmentFiles().length === 0;
    }

    async init(courseId: number, assignmentId: number, navigate: NavigateFunction) {
        try {
            runInAction(() => {
                const course = this._rootStore.courseStore.coursesInfo.find(
                    (c) => c.course.courseId === courseId,
                );
                if (!course) navigate('/');
                this.currentUser = course!.userInfo.courseuserId;
                this.isTeacher =
                    course?.userInfo.role === 0 || course?.userInfo.role === 1;
            });

            const assignment =
                await this._assignmentService.getAssignmentById(assignmentId);
            const topics = await this._topicService.getTopics(courseId);
            if (!this.isTeacher) {
                const saInfo = await this._saService.getStudentAssignment(assignmentId);
                runInAction(() => {
                    this.saInfo = saInfo;
                    this.isEditWork = !saInfo.studentAssignment.isDone;
                    if (saInfo.studentAssignment.currentMark === null) {
                        this.work = new UpdateWorkModel(
                            [],
                            saInfo.studentAssignment.studentassignmentId,
                        );
                    }
                });
            }

            runInAction(() => {
                this.assignment = assignment;
                this.topics = topics;

                this.editAssignmentData = new UpdateAssignmentModel(
                    this.assignment!.id,
                    courseId,
                    this.assignment!.assignmentName,
                    this.assignment!.maxMark,
                    this.assignment!.minMark,
                    this.assignment!.isRequired,
                    this.assignment!.assignmentDeadline,
                    this.assignment!.assignmentDatePublication,
                    this.assignment!.topicId,
                    this.assignment!.assignmentDescription
                        ? this.assignment!.assignmentDescription
                        : '',
                );

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

    handleEditAssignmentOpen() {
        this.editAssignmentOpen = true;
        this.closeAssignmentMenu();
    }

    handleEditAssignmentClose() {
        this.resetEditAssignment();
        this.editAssignmentOpen = false;
    }

    openAssignmentMenu(event: React.MouseEvent<HTMLButtonElement>) {
        this.assignmentMenuAnchor = event.currentTarget;
    }

    closeAssignmentMenu() {
        this.assignmentMenuAnchor = null;
    }

    async deleteAssignment(courseId: number, navigate: NavigateFunction) {
        try {
            await this._assignmentService.deleteAssignment(this.assignment!.id);
            runInAction(() => {
                enqueueAlert('glossary.deleteAssignmentSuccess', 'success');
                this.closeAssignmentMenu();
                navigate(`/course/${courseId}`);
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

    onWorkFileAdd(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.files) {
            this.work!.assignmentFiles.push(...Array.from(e.target.files));
        }
        debounce(
            action(() => {
                const errors = this.work!.validateAssignmentFiles();
                this.workErrors.workFiles = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onWorkFileDelete(id: number) {
        this.work!.assignmentFiles.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.work!.validateAssignmentFiles();
                this.workErrors.workFiles = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    async onFileClick(id: number, navigate: NavigateFunction) {
        try {
            const link = await this._assignmentService.getAssignmentFileById(id);
            runInAction(() => {
                this.isFileViewerOpen = true;
                this.fileLink = link;
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

    onFileViewerClose() {
        this.isFileViewerOpen = false;
        this.fileLink = '';
    }

    onAssignmentNameChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.editAssignmentData!.assignmentName = e.target.value;
        debounce(
            action(() => {
                const errors = this.editAssignmentData!.validateAssignmentName();
                this.editAssignmentErrors.assignmentName =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onAssignmentDescriptionChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.editAssignmentData!.assignmentDescription = e.target.value;
    }

    onAssignmentMaxMarkChange(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.value) {
            this.editAssignmentData!.maxMark = Number.parseInt(e.target.value);
            debounce(
                action(() => {
                    const errors = this.editAssignmentData!.validateMaxMark();
                    this.editAssignmentErrors.maxMark =
                        errors.length !== 0 ? errors[0] : null;
                }),
            )();
        }
    }

    onAssignmentMinMarkChange(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.value) {
            this.editAssignmentData!.minMark = Number.parseInt(e.target.value);
            debounce(
                action(() => {
                    const errors = this.editAssignmentData!.validateMinMark();
                    this.editAssignmentErrors.minMark =
                        errors.length !== 0 ? errors[0] : null;
                }),
            )();
        }
    }

    onAssignmentTopicChange(event: SelectChangeEvent) {
        const value = Number.parseInt(event.target.value);
        if (value !== 0) {
            this.editAssignmentData!.topicId = value;
        } else {
            this.editAssignmentData!.topicId = undefined;
        }
    }

    onAssignmentIsRequiredChange(event: SelectChangeEvent) {
        const value = Number.parseInt(event.target.value);
        if (value === 0) {
            this.editAssignmentData!.isRequired = false;
        } else {
            this.editAssignmentData!.isRequired = true;
        }
    }

    onAssignmentDeadlineChange(date: Dayjs | null) {
        if (date) {
            const value = date.toDate();
            this.editAssignmentData!.assignmentDeadline = value;
            debounce(
                action(() => {
                    const errors = this.editAssignmentData!.validateAssignmentDeadline();
                    this.editAssignmentErrors.assignmentDeadline =
                        errors.length !== 0 ? errors[0] : null;
                }),
            )();
        }
    }

    async onAssignmentFileAdd(
        e: React.ChangeEvent<HTMLInputElement>,
        navigate: NavigateFunction,
    ) {
        if (!e.target.files) return;

        const validator = new FileValidator(e.target.files[0]);
        validator.validateFileExtension([
            'png',
            'jpg',
            'jpeg',
            'doc',
            'pdf',
            'docx',
            'pptx',
            'ppt',
            'xls',
            'xlsx',
        ]);

        if (validator.errors.length > 0) {
            enqueueAlert(
                validator.errors[0].errorKey,
                'error',
                validator.errors[0].options,
            );
            return;
        }

        try {
            const addedFile = await this._assignmentService.addFile(
                e.target.files[0],
                this.assignment!.id,
            );
            runInAction(() => {
                this.assignment!.assignmentfiles!.push(addedFile);
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
    async onAssignmentFileDelete(fileId: number, navigate: NavigateFunction) {
        try {
            await this._assignmentService.deleteFileById(fileId);
            runInAction(() => {
                const index = this.assignment!.assignmentfiles!.findIndex(
                    (f) => f.id == fileId,
                );
                this.assignment!.assignmentfiles!.splice(index, 1);
                enqueueAlert('glossary.deleteFileSuccess', 'success');
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

    async onAssignmentLinkAdd(navigate: NavigateFunction) {
        if (!this.linkAdd) return;

        const validator = new StringValidator(this.linkAdd);
        validator.isLink();
        if (validator.errors.length > 0) {
            enqueueAlert(
                validator.errors[0].errorKey,
                'error',
                validator.errors[0].options,
            );
            return;
        }

        try {
            const addedLink = await this._assignmentService.addLink(
                this.assignment!.id,
                this.linkAdd,
            );
            runInAction(() => {
                this.assignment!.assignmentlinks!.push(addedLink);
                enqueueAlert('glossary.editSuccess', 'success');
                this.handleLinkAddClose();
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

    async onAssignmentLinkDelete(linkId: number, navigate: NavigateFunction) {
        try {
            await this._assignmentService.deleteLinkById(linkId);
            runInAction(() => {
                const index = this.assignment!.assignmentlinks!.findIndex(
                    (l) => l.id == linkId,
                );
                this.assignment!.assignmentlinks!.splice(index, 1);
                enqueueAlert('glossary.deleteLinkSuccess', 'success');
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

    handleLinkAddOpen() {
        this.isLinkAddOpen = true;
    }

    handleLinkAddClose() {
        this.linkAdd = '';
        this.isLinkAddOpen = false;
    }

    onLinkAddChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.linkAdd = e.target.value;
    }

    async submitEditAssignment(navigate: NavigateFunction) {
        try {
            const updatedAssignment = await this._assignmentService.updateAssignment(
                this.editAssignmentData!,
            );
            runInAction(() => {
                this.assignment = updatedAssignment;
                this.handleEditAssignmentClose();
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

    async onWorkFileClick(id: number, navigate: NavigateFunction) {
        try {
            const link = await this._saService.getFileLink(id);
            runInAction(() => {
                this.isFileViewerOpen = true;
                this.fileLink = link;
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

    enableEditMode() {
        if (!this.saInfo?.studentAssignment.currentMark) {
            this.isEditWork = true;
        }
    }

    disableEditMode() {
        if (this.saInfo?.studentAssignment.isDone) {
            this.resetEditWork();
        }
    }

    async submitWorkUpdate(navigate: NavigateFunction) {
        try {
            const saInfo = await this._saService.updateWork(this.work!);
            runInAction(() => {
                this.saInfo = saInfo;
                this.disableEditMode();
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

    async sendComment(comment: CreateCommentModel, navigate: NavigateFunction) {
        try {
            const commentInfo = await this._saService.createComment(comment);
            runInAction(() => {
                this.saInfo!.comments = commentInfo;
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

    resetEditAssignment() {
        this.editAssignmentData!.assignmentDatePublication =
            this.assignment?.assignmentDatePublication;
        this.editAssignmentData!.assignmentDeadline = this.assignment!.assignmentDeadline;
        this.editAssignmentData!.assignmentDescription =
            this.assignment!.assignmentDescription;
        this.editAssignmentData!.assignmentName = this.assignment!.assignmentName;
        this.editAssignmentData!.id = this.assignment!.id;
        this.editAssignmentData!.isRequired = this.assignment!.isRequired;
        this.editAssignmentData!.maxMark = this.assignment!.maxMark;
        this.editAssignmentData!.minMark = this.assignment!.minMark;
        this.editAssignmentData!.topicId = this.assignment!.topicId;

        Object.keys(this.editAssignmentErrors).forEach(
            (key) => (this.editAssignmentErrors[key] = null),
        );
    }

    resetEditWork() {
        this.work!.assignmentFiles = [];
        this.isEditWork = false;
    }

    reset(): void {
        this.isLoading = true;
        this.saInfo = null;
        this.editAssignmentData = null;
        Object.keys(this.editAssignmentErrors).forEach(
            (key) => (this.editAssignmentErrors[key] = null),
        );

        this.work = null;
        Object.keys(this.workErrors).forEach((key) => (this.workErrors[key] = null));

        this.isFileViewerOpen = false;

        this.assignment = null;
        this.fileLink = '';
        this.editAssignmentOpen = false;
        this.assignmentMenuAnchor = null;

        this.linkAdd = '';
        this.isLinkAddOpen = false;

        this.isTeacher = false;
        this.isEditWork = true;
        this.currentUser = null;
    }
}
