/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { action, computed, makeObservable, observable } from 'mobx';
import RootStore from './RootStore';
import ValidationError from '../helpers/validation/ValidationError';
import CreateUpdateAssignmentModel from '../models/assignment/CreateUpdateAssignmentModel';
import AssignmentModel from '../models/assignment/AssignmentModel';
import debounce from '../helpers/debounce';
import CreateUpdateWorkModel from '../models/work/CreateUpdateWorkModel';
import { enqueueAlert } from '../components/Notification/NotificationProvider';
import { NavigateFunction } from 'react-router-dom';
import { SelectChangeEvent } from '@mui/material';
import { Dayjs } from 'dayjs';
import TopicModel from '../models/topic/TopicModel';

export default class AssignmentPageStore {
    private readonly _rootStore: RootStore;

    editAssignmentData: CreateUpdateAssignmentModel | null = null;
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

    constructor(rootStore: RootStore) {
        this._rootStore = rootStore;
        makeObservable(this, {
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

    init(courseId: number, assignmentId: number) {
        this.assignment = {
            id: 3,
            topicId: 2,
            assignmentName: 'Research Paper on Artificial Intelligence',
            assignmentDescription:
                'Research Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial IntelligenceResearch Paper on Artificial Intelligence',
            maxMark: 20,
            minMark: 0,
            isRequired: true,
            assignmentDatePublication: new Date(Date.now()),
            assignmentDeadline: new Date('2024-05-15'),
            isEdited: true,
            editedTime: new Date('2024-04-23'),
            assignmentfiles: [
                {
                    id: 1,
                    assignmentFile:
                        'https://cdn.discordapp.com/attachments/1147229890538639370/1232728676240457768/IMG_20231205_194138_748.png?ex=662a838b&is=6629320b&hm=4d6c883c4ce6c372c05a099c370c06e46eb0bb88731eeca2cec84626af4422c4&',
                },
            ],
            assignmentlinks: [{ id: 1, assignmentLink: 'https://ai.stanford.edu/' }],
        };

        this.topics = [
            {
                courseId: 1,
                id: 1,
                title: 'topic1',
            },
            {
                courseId: 1,
                id: 2,
                title: 'topic2',
            },
            {
                courseId: 1,
                id: 3,
                title: 'topic3',
            },
        ];

        this.editAssignmentData = new CreateUpdateAssignmentModel(
            courseId,
            this.assignment.assignmentName,
            this.assignment.maxMark,
            this.assignment.minMark,
            this.assignment.isRequired,
            this.assignment.assignmentDeadline,
            [],
            [],
            this.assignment.assignmentDatePublication,
            this.assignment.topicId,
            this.assignment.assignmentDescription,
        );

        //add autofill for files getAssihnmentfile -> load file -> convert it to file

        this.isLoading = false;
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

    deleteAssignment(courseId: number, navigate: NavigateFunction) {
        enqueueAlert('glossary.deleteAssignmentSuccess', 'success');
        navigate(`/course/${courseId}`);
        this.closeAssignmentMenu();
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

    onFileClick(id: number) {
        this.isFileViewerOpen = true;
        this.fileLink = this.assignment!.assignmentfiles![id].assignmentFile!;
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

    submitEditAssignment(...params: unknown[]): void {
        throw new Error('Method not implemented.');
    }

    resetWork() {
        this.work.workFiles = [];
        Object.keys(this.workErrors).forEach((key) => (this.workErrors[key] = null));
    }

    resetEditAssignment() {
        this.editAssignmentData!.assignmentDatePublication =
            this.assignment!.assignmentDatePublication;
        this.editAssignmentData!.assignmentDeadline = this.assignment!.assignmentDeadline;
        this.editAssignmentData!.assignmentDescription =
            this.assignment!.assignmentDescription;
        //this.editAssignmentData!.assignmentFiles add logic
        //this.editAssignmentData?.assignmentLinks add logic
        this.editAssignmentData!.assignmentName = this.assignment!.assignmentName;
        this.editAssignmentData!.isRequired = this.assignment!.isRequired;
        this.editAssignmentData!.maxMark = this.assignment!.maxMark;
        this.editAssignmentData!.minMark = this.assignment!.minMark;
        this.editAssignmentData!.topicId = this.assignment!.topicId;
        Object.keys(this.editAssignmentErrors).forEach(
            (key) => (this.editAssignmentErrors[key] = null),
        );
    }

    reset(): void {
        this.resetEditAssignment();
        this.resetWork();
        this.isLoading = true;
        this.isFileViewerOpen = false;

        this.assignment = null;
        this.fileLink = '';
        this.editAssignmentOpen = false;
        this.assignmentMenuAnchor = null;
    }
}
