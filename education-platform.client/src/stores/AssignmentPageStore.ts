/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { action, computed, makeObservable, observable, runInAction } from 'mobx';
import RootStore from './RootStore';
import ValidationError from '../helpers/validation/ValidationError';
import CreateAssignmentModel from '../models/assignment/CreateAssignmentModel';
import AssignmentModel from '../models/assignment/AssignmentModel';
import debounce from '../helpers/debounce';
import CreateUpdateWorkModel from '../models/work/CreateUpdateWorkModel';
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
import CommonService from '../services/common/CommonService';

export default class AssignmentPageStore {
    private readonly _rootStore: RootStore;
    private readonly _assignmentService: AssignmentService;
    private readonly _topicService: TopicService;
    private readonly _commonService: CommonService

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

    work: CreateUpdateWorkModel = new CreateUpdateWorkModel([]);
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
    isEditLoading = true;

    constructor(rootStore: RootStore, assignmentService: AssignmentService, topicService: TopicService, commonService: CommonService) {
        this._rootStore = rootStore;
        this._assignmentService = assignmentService;
        this._topicService = topicService;
        this._commonService = commonService
        makeObservable(this, {
            isEditLoading: observable,
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

            isEditAssignmentValid: computed,
            isWorkValid: computed,

            init: action.bound,
            onWorkFileAdd: action.bound,
            onWorkFileDelete: action.bound,
            submitEditAssignment: action.bound,
            reset: action.bound,
            onFileClick: action.bound,
            resetEditAssignment: action.bound,
            resetWork: action.bound,
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
        });
    }

    get isEditAssignmentValid(): boolean {
        return (
            this.editAssignmentData!.validateAssignmentDeadline().length === 0 &&
            this.editAssignmentData!.validateAssignmentFiles().length === 0 &&
            this.editAssignmentData!.validateAssignmentLinks().length === 0 &&
            this.editAssignmentData!.validateAssignmentName().length === 0 &&
            this.editAssignmentData!.validateIsRequired().length === 0 &&
            this.editAssignmentData!.validateMaxMark().length === 0 &&
            this.editAssignmentData!.validateMinMark().length === 0
        );
    }

    get isWorkValid(): boolean {
        return this.work!.validateWorkFiles().length === 0;
    }

    async init(courseId: number, assignmentId: number, navigate: NavigateFunction) {
        try {
            const assignment = await this._assignmentService.getAssignmentById(assignmentId);
            const topics = await this._topicService.getTopics(courseId);

            runInAction(() => {
                this.assignment = assignment;
                this.topics = topics;
                this.isLoading = false;
            })

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

    async handleEditAssignmentOpen(courseId: number) {
        runInAction(() => {
            this.editAssignmentOpen = true;
            this.closeAssignmentMenu();
        });
        const assignmentFiles: File[] = [];
        if (this.assignment!.assignmentfiles) {
            for (const file of this.assignment!.assignmentfiles) {
                const link = await this._assignmentService.getAssignmentFileById(file.id);
                const loadedFile = await this._commonService.loadFile(link);
                if (loadedFile) assignmentFiles.push(loadedFile);
            }
        };

        const assignmentLinks: string[] = [];
        if (this.assignment!.assignmentlinks) {
            this.assignment!.assignmentlinks.forEach(link => assignmentLinks.push(link.assignmentLink))
        }
        runInAction(() => {
            this.editAssignmentData = new UpdateAssignmentModel(
                this.assignment!.id,
                courseId,
                this.assignment!.assignmentName,
                this.assignment!.maxMark,
                this.assignment!.minMark,
                this.assignment!.isRequired,
                this.assignment!.assignmentDeadline,
                assignmentFiles,
                assignmentLinks,
                this.assignment!.assignmentDatePublication,
                this.assignment!.topicId,
                this.assignment!.assignmentDescription ? this.assignment!.assignmentDescription : '',
            );

            this.isEditLoading = false;

        })

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
            await this._assignmentService.deleteAssignment(this.assignment!.id)
            runInAction(() => {
                enqueueAlert('glossary.deleteAssignmentSuccess', 'success');
                navigate(`/course/${courseId}`);
                this.closeAssignmentMenu();
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

    onWorkFileAdd(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.files) {
            this.work.workFiles.push(...Array.from(e.target.files));
        }
        debounce(
            action(() => {
                const errors = this.work!.validateWorkFiles();
                this.workErrors.workFiles = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onWorkFileDelete(id: number) {
        this.work.workFiles.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.work!.validateWorkFiles();
                this.workErrors.workFiles = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    async onFileClick(id: number) {
        const link = await this._assignmentService.getAssignmentFileById(id)
        runInAction(() => {
            this.isFileViewerOpen = true;
            this.fileLink = link;
        })
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

    onAssignmentFileAdd(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.files) {
            this.editAssignmentData!.assignmentFiles.push(...Array.from(e.target.files));
        }
        debounce(
            action(() => {
                const errors = this.editAssignmentData!.validateAssignmentFiles();
                this.editAssignmentErrors.assignmentFiles =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }
    onAssignmentFileDelete(id: number) {
        this.editAssignmentData?.assignmentFiles.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.editAssignmentData!.validateAssignmentFiles();
                this.editAssignmentErrors.assignmentFiles =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onAssignmentLinkAdd(link: string) {
        this.editAssignmentData?.assignmentLinks.push(link);
        debounce(
            action(() => {
                const errors = this.editAssignmentData!.validateAssignmentLinks();
                this.editAssignmentErrors.assignmentLinks =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onAssignmentLinkDelete(id: number) {
        this.editAssignmentData?.assignmentLinks.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.editAssignmentData!.validateAssignmentLinks();
                this.editAssignmentErrors.assignmentLinks =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    async submitEditAssignment(navigate: NavigateFunction) {
        try {
            const updatedAssignment = await this._assignmentService.updateAssignment(this.editAssignmentData!);
            runInAction(() => {
                this.assignment = updatedAssignment;
                this.handleEditAssignmentClose();
                enqueueAlert('glossary.editSuccess', 'success');
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

    resetWork() {
        this.work.workFiles = [];
        Object.keys(this.workErrors).forEach((key) => (this.workErrors[key] = null));
    }

    resetEditAssignment() {
        this.isEditLoading = true;
        this.editAssignmentData = null;
        Object.keys(this.editAssignmentErrors).forEach(
            (key) => (this.editAssignmentErrors[key] = null),
        );
    }

    reset(): void {
        this.resetEditAssignment();
        this.editAssignmentData = null;
        Object.keys(this.editAssignmentErrors).forEach(
            (key) => (this.editAssignmentErrors[key] = null),
        );
        this.resetWork();
        this.isLoading = true;
        this.isFileViewerOpen = false;

        this.assignment = null;
        this.fileLink = '';
        this.editAssignmentOpen = false;
        this.assignmentMenuAnchor = null;
    }
}
