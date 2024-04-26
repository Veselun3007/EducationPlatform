/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { action, computed, makeObservable, observable } from 'mobx';
import RootStore from './RootStore';
import CourseModel from '../models/course/CourseModel';
import ValidationError from '../helpers/validation/ValidationError';
import CreateUpdateCourseModel from '../models/course/CreateUpdateCourseModel';
import debounce from '../helpers/debounce';
import { enqueueAlert } from '../components/Notification/NotificationProvider';
import { NavigateFunction } from 'react-router-dom';
import CreateUpdateTopicModel from '../models/topic/CreateUpdateTopicModel';
import CreateUpdateMaterialModel from '../models/material/CreateUpdateMaterialModel';
import CreateUpdateAssignmentModel from '../models/assignment/CreateUpdateAssignmentModel';
import TopicModel from '../models/topic/TopicModel';
import { SelectChangeEvent } from '@mui/material';
import { Dayjs } from 'dayjs';
import AssignmentModel from '../models/assignment/AssignmentModel';
import MaterialModel from '../models/material/MaterialModel';

export default class CoursePageStore {
    private readonly _rootStore: RootStore;

    course: CourseModel | null = null;
    topics: TopicModel[] | null = [];
    assignments: AssignmentModel[] | null = null;
    materials: MaterialModel[] | null = null;

    courseMenuAnchor: null | HTMLElement = null;
    contentMenuAnchor: null | HTMLElement = null;

    editCourseOpen = false;
    createTopicOpen = false;
    createMaterialOpen = false;
    createAssignmentOpen = false;
    editTopicOpen = false;

    courseData: CreateUpdateCourseModel | null = null;
    courseErrors: Record<string, ValidationError | null> = {
        name: null,
        meta: null,
    };

    createTopicData: CreateUpdateTopicModel | null = null;
    createTopicErrors: Record<string, ValidationError | null> = {
        title: null,
        meta: null,
    };

    editTopicData: CreateUpdateTopicModel | null = null;
    editTopicErrors: Record<string, ValidationError | null> = {
        title: null,
        meta: null,
    };
    editTopicId: number | null = null;

    materialData: CreateUpdateMaterialModel | null = null;
    materialErrors: Record<string, ValidationError | null> = {
        materialName: null,
        materialFiles: null,
        materialLinks: null,
        meta: null,
    };

    assignmentData: CreateUpdateAssignmentModel | null = null;
    assignmentErrors: Record<string, ValidationError | null> = {
        assignmentName: null,
        maxMark: null,
        minMark: null,
        isRequired: null,
        assignmentDeadline: null,
        assignmentFiles: null,
        assignmentLinks: null,
        meta: null,
    };

    isLoading = true;

    constructor(rootStore: RootStore) {
        this._rootStore = rootStore;

        makeObservable(this, {
            isLoading: observable,
            editTopicId: observable,
            editTopicData: observable,
            editTopicErrors: observable,
            editTopicOpen: observable,
            createMaterialOpen: observable,
            topics: observable,
            createAssignmentOpen: observable,
            courseData: observable,
            courseErrors: observable,
            course: observable,
            courseMenuAnchor: observable,
            contentMenuAnchor: observable,
            editCourseOpen: observable,
            createTopicData: observable,
            createTopicErrors: observable,
            createTopicOpen: observable,
            materialData: observable,
            materialErrors: observable,
            assignmentData: observable,
            assignmentErrors: observable,
            assignments: observable,
            materials: observable,

            isCourseValid: computed,
            isCreateTopicValid: computed,
            isMaterialValid: computed,
            isAssignmentValid: computed,
            isEditTopicValid: computed,

            init: action.bound,
            onNameChange: action.bound,
            onDescriptionChange: action.bound,
            openCourseMenu: action.bound,
            closeCourseMenu: action.bound,
            handleEditCourseOpen: action.bound,
            handleEditCourseClose: action.bound,
            reset: action.bound,
            submitCourse: action.bound,
            deleteCourse: action.bound,
            onCreateTopicTitleChange: action.bound,
            submitCreateTopic: action.bound,
            openContentMenu: action.bound,
            closeContentMenu: action.bound,
            handleCreateTopicOpen: action.bound,
            handleCreateTopicClose: action.bound,
            resetAssignment: action.bound,
            resetCourse: action.bound,
            resetMaterial: action.bound,
            resetCreateTopic: action.bound,
            handleCreateMaterialOpen: action.bound,
            handleCreateMaterialClose: action.bound,
            handleCreateAssignmentClose: action.bound,
            handleCreateAssignmentOpen: action.bound,
            submitMaterial: action.bound,
            submitAssignment: action.bound,
            onMaterialNameChange: action.bound,
            onMaterialDescriptionChange: action.bound,
            onMaterialTopicChange: action.bound,
            onMaterialFileAdd: action.bound,
            onMaterialFileDelete: action.bound,
            onMaterialLinkAdd: action.bound,
            onMaterialLinkDelete: action.bound,
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
            handleEditTopicOpen: action.bound,
            handleEditTopicClose: action.bound,
            resetEditTopic: action.bound,
            onEditTopicTitleChange: action.bound,
            submitEditTopic: action.bound,
            deleteTopic: action.bound,
        });
    }

    get isCourseValid(): boolean {
        return this.courseData!.validateName().length === 0;
    }

    get isCreateTopicValid(): boolean {
        return this.createTopicData!.validateTitle().length === 0;
    }

    get isEditTopicValid(): boolean {
        return this.editTopicData!.validateTitle().length === 0;
    }

    get isMaterialValid(): boolean {
        return (
            this.materialData!.validateMaterialName().length === 0 &&
            this.materialData!.validateMaterialFiles().length === 0 &&
            this.materialData!.validateMaterialLinks().length === 0
        );
    }

    get isAssignmentValid(): boolean {
        return (
            this.assignmentData!.validateAssignmentDeadline().length === 0 &&
            this.assignmentData!.validateAssignmentFiles().length === 0 &&
            this.assignmentData!.validateAssignmentLinks().length === 0 &&
            this.assignmentData!.validateAssignmentName().length === 0 &&
            this.assignmentData!.validateIsRequired().length === 0 &&
            this.assignmentData!.validateMaxMark().length === 0 &&
            this.assignmentData!.validateMinMark().length === 0
        );
    }

    init() {
        this.course = {
            courseId: 1,
            courseName: 'Course name 1 ВАП ВФ ІАР ПВІАПВАП ВАПВА ВІПІПІВП',
            courseDescription:
                'ThiThis is description for cource 1s is description for cource 1 This is description for cource 1, This is description for cource 1 This is description for cource 1This is description for cource 1',
            courseLink: 'link 1',
        };

        this.courseData = new CreateUpdateCourseModel(
            this.course.courseName,
            this.course.courseDescription,
        );

        this.createTopicData = new CreateUpdateTopicModel(this.course.courseId, '');
        this.materialData = new CreateUpdateMaterialModel(
            this.course.courseId,
            '',
            [],
            [],
            new Date(Date.now()),
        );
        this.assignmentData = new CreateUpdateAssignmentModel(
            this.course.courseId,
            '',
            100,
            0,
            false,
            new Date(Date.now() + 86400000),
            [],
            [],
            new Date(Date.now()),
        );
        this.editTopicData = new CreateUpdateTopicModel(this.course.courseId, '');
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

        this.assignments = [
            {
                id: 1,
                topicId: 1,
                assignmentName: 'Introduction to JavaScript',
                assignmentDescription:
                    'Write a simple JavaScript program to calculate the factorial of a number.',
                maxMark: 10,
                minMark: 0,
                isRequired: true,
                assignmentDatePublication: new Date(Date.now()),
                assignmentDeadline: new Date('2024-05-05'),
                isEdited: false,
                editedTime: undefined,
                assignmentfiles: [
                    {
                        id: 1,
                        assignmentFile:
                            'https://docs.google.com/document/d/1MAp9AFXbTTa-nRQW1_eacevqBuRb188_j_NGr_oBddU/edit?usp=sharing',
                    },
                ],
                assignmentlinks: [
                    {
                        id: 1,
                        assignmentLink:
                            'https://developer.mozilla.org/en-US/docs/Web/JavaScript',
                    },
                ],
            },
            {
                id: 2,
                topicId: 2,
                assignmentName: 'Introduction to HTML',
                maxMark: 5,
                minMark: 0,
                isRequired: false,
                assignmentDatePublication: new Date(Date.now()),
                assignmentDeadline: new Date('2024-05-10'),
                isEdited: false,
                editedTime: undefined,
                assignmentfiles: [
                    {
                        id: 1,
                        assignmentFile:
                            'https://docs.google.com/document/d/1MAp9AFXbTTa-nRQW1_eacevqBuRb188_j_NGr_oBddU/edit?usp=sharing',
                    },
                ],
                assignmentlinks: [
                    { id: 1, assignmentLink: 'https://www.w3schools.com/html/' },
                ],
            },
            {
                id: 3,
                assignmentName: 'Research Paper on Artificial Intelligence',
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
                            'https://docs.google.com/document/d/1MAp9AFXbTTa-nRQW1_eacevqBuRb188_j_NGr_oBddU/edit?usp=sharing',
                    },
                ],
                assignmentlinks: [{ id: 1, assignmentLink: 'https://ai.stanford.edu/' }],
            },
        ];

        this.materials = [
            {
                id: 1,
                topicId: 1,
                materialName: 'Introduction to Algorithms',
                materialDescription: 'Overview of basic algorithms and their analysis.',
                materialDatePublication: new Date('2024-04-20'),
                isEdited: false,
                editedTime: undefined,
                materialfiles: [
                    {
                        id: 1,
                        materialFile:
                            'https://docs.google.com/document/d/1MAp9AFXbTTa-nRQW1_eacevqBuRb188_j_NGr_oBddU/edit?usp=sharing',
                    },
                ],
                materiallinks: [
                    {
                        id: 1,
                        materialLink:
                            'https://mitpress.mit.edu/books/introduction-algorithms',
                    },
                ],
            },
            {
                id: 2,
                topicId: 2,
                materialName: 'Database Design Basics',
                materialDatePublication: new Date('2024-04-22'),
                isEdited: false,
                editedTime: undefined,
                materialfiles: [
                    {
                        id: 1,
                        materialFile:
                            'https://docs.google.com/document/d/1MAp9AFXbTTa-nRQW1_eacevqBuRb188_j_NGr_oBddU/edit?usp=sharing',
                    },
                ],
                materiallinks: [
                    {
                        id: 1,
                        materialLink:
                            'https://www.tutorialspoint.com/dbms/database_design.htm',
                    },
                ],
            },
            {
                id: 3,
                materialName: 'Introduction to Machine Learning',
                materialDescription:
                    'An overview of machine learning concepts and algorithms.',
                materialDatePublication: new Date('2024-04-18'),
                isEdited: true,
                editedTime: new Date('2024-04-23'),
                materialfiles: [
                    {
                        id: 1,
                        materialFile:
                            'https://docs.google.com/document/d/1MAp9AFXbTTa-nRQW1_eacevqBuRb188_j_NGr_oBddU/edit?usp=sharing',
                    },
                ],
                materiallinks: [
                    {
                        id: 1,
                        materialLink: 'https://www.coursera.org/learn/machine-learning',
                    },
                ],
            },
        ];
        this.isLoading = false;
    }

    onNameChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.courseData!.name = e.target.value;
        debounce(
            action(() => {
                const nameErrors = this.courseData!.validateName();
                this.courseErrors.name = nameErrors.length !== 0 ? nameErrors[0] : null;
            }),
        )();
    }

    onDescriptionChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.courseData!.description = e.target.value;
    }

    onCreateTopicTitleChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.createTopicData!.title = e.target.value;
        debounce(
            action(() => {
                const titleErrors = this.createTopicData!.validateTitle();
                this.createTopicErrors.title =
                    titleErrors.length !== 0 ? titleErrors[0] : null;
            }),
        )();
    }

    onMaterialNameChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.materialData!.materialName = e.target.value;
        debounce(
            action(() => {
                const errors = this.materialData!.validateMaterialName();
                this.materialErrors.materialName = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onMaterialTopicChange(event: SelectChangeEvent) {
        const value = Number.parseInt(event.target.value);
        if (value !== 0) {
            this.materialData!.topicId = value;
        } else {
            this.materialData!.topicId = undefined;
        }
    }
    onMaterialFileAdd(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.files) {
            this.materialData!.materialFiles.push(...Array.from(e.target.files));
        }
        debounce(
            action(() => {
                const errors = this.materialData!.validateMaterialFiles();
                this.materialErrors.materialFiles =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }
    onMaterialFileDelete(id: number) {
        this.materialData?.materialFiles.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.materialData!.validateMaterialFiles();
                this.materialErrors.materialFiles =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onMaterialLinkAdd(link: string) {
        this.materialData?.materialLinks.push(link);
        debounce(
            action(() => {
                const errors = this.materialData!.validateMaterialLinks();
                this.materialErrors.materialLinks =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onMaterialLinkDelete(id: number) {
        this.materialData?.materialLinks.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.materialData!.validateMaterialLinks();
                this.materialErrors.materialLinks =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onMaterialDescriptionChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.materialData!.materialDescription = e.target.value;
    }

    onAssignmentNameChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.assignmentData!.assignmentName = e.target.value;
        debounce(
            action(() => {
                const errors = this.assignmentData!.validateAssignmentName();
                this.assignmentErrors.assignmentName =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onAssignmentDescriptionChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.assignmentData!.assignmentDescription = e.target.value;
    }

    onAssignmentMaxMarkChange(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.value) {
            this.assignmentData!.maxMark = Number.parseInt(e.target.value);
            debounce(
                action(() => {
                    const errors = this.assignmentData!.validateMaxMark();
                    this.assignmentErrors.maxMark =
                        errors.length !== 0 ? errors[0] : null;
                }),
            )();
        }
    }

    onAssignmentMinMarkChange(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.value) {
            this.assignmentData!.minMark = Number.parseInt(e.target.value);
            debounce(
                action(() => {
                    const errors = this.assignmentData!.validateMinMark();
                    this.assignmentErrors.minMark =
                        errors.length !== 0 ? errors[0] : null;
                }),
            )();
        }
    }

    onAssignmentTopicChange(event: SelectChangeEvent) {
        const value = Number.parseInt(event.target.value);
        if (value !== 0) {
            this.assignmentData!.topicId = value;
        } else {
            this.assignmentData!.topicId = undefined;
        }
    }

    onAssignmentIsRequiredChange(event: SelectChangeEvent) {
        const value = Number.parseInt(event.target.value);
        if (value === 0) {
            this.assignmentData!.isRequired = false;
        } else {
            this.assignmentData!.isRequired = true;
        }
    }

    onAssignmentDeadlineChange(date: Dayjs | null) {
        if (date) {
            const value = date.toDate();
            this.assignmentData!.assignmentDeadline = value;
            debounce(
                action(() => {
                    const errors = this.assignmentData!.validateAssignmentDeadline();
                    this.assignmentErrors.assignmentDeadline =
                        errors.length !== 0 ? errors[0] : null;
                }),
            )();
        }
    }

    onAssignmentFileAdd(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.files) {
            this.assignmentData!.assignmentFiles.push(...Array.from(e.target.files));
        }
        debounce(
            action(() => {
                const errors = this.assignmentData!.validateAssignmentFiles();
                this.assignmentErrors.assignmentFiles =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }
    onAssignmentFileDelete(id: number) {
        this.assignmentData?.assignmentFiles.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.assignmentData!.validateAssignmentFiles();
                this.assignmentErrors.assignmentFiles =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onAssignmentLinkAdd(link: string) {
        this.assignmentData?.assignmentLinks.push(link);
        debounce(
            action(() => {
                const errors = this.assignmentData!.validateAssignmentLinks();
                this.assignmentErrors.assignmentLinks =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onAssignmentLinkDelete(id: number) {
        this.assignmentData?.assignmentLinks.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.assignmentData!.validateAssignmentLinks();
                this.assignmentErrors.assignmentLinks =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onEditTopicTitleChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.editTopicData!.title = e.target.value;
        debounce(
            action(() => {
                const titleErrors = this.editTopicData!.validateTitle();
                this.editTopicErrors.title =
                    titleErrors.length !== 0 ? titleErrors[0] : null;
            }),
        )();
    }

    submitCourse() {
        this.handleEditCourseClose();
    }

    submitCreateTopic() {
        this.handleCreateTopicClose();
    }

    submitMaterial() {
        this.handleCreateMaterialClose();
    }

    submitAssignment() {
        this.handleCreateAssignmentClose();
    }

    submitEditTopic() {
        this.handleEditTopicClose();
    }

    openCourseMenu(event: React.MouseEvent<HTMLButtonElement>) {
        this.courseMenuAnchor = event.currentTarget;
    }

    closeCourseMenu() {
        this.courseMenuAnchor = null;
    }

    openContentMenu(event: React.MouseEvent<HTMLButtonElement>) {
        this.contentMenuAnchor = event.currentTarget;
    }

    closeContentMenu() {
        this.contentMenuAnchor = null;
    }

    handleEditCourseOpen() {
        this.editCourseOpen = true;
        this.closeCourseMenu();
    }

    handleEditCourseClose() {
        this.resetCourse();
        this.editCourseOpen = false;
        this.closeCourseMenu();
    }

    handleEditTopicOpen(id: number) {
        const topic = this.topics!.find((c) => c.id === id);
        this.editTopicData!.title = topic!.title;
        this.editTopicId = topic!.id;
        this.editTopicOpen = true;
    }

    handleEditTopicClose() {
        this.resetEditTopic();
        this.editTopicOpen = false;
    }

    handleCreateTopicOpen() {
        this.createTopicOpen = true;
        this.closeContentMenu();
    }

    handleCreateTopicClose() {
        this.resetCreateTopic();
        this.createTopicOpen = false;
    }

    handleCreateMaterialOpen() {
        this.createMaterialOpen = true;
        this.closeContentMenu();
    }

    handleCreateMaterialClose() {
        this.resetMaterial();
        this.createMaterialOpen = false;
        this.closeContentMenu();
    }

    handleCreateAssignmentOpen() {
        this.createAssignmentOpen = true;
        this.closeContentMenu();
    }

    handleCreateAssignmentClose() {
        this.resetAssignment();
        this.createAssignmentOpen = false;
        this.closeContentMenu();
    }

    deleteCourse(navigate: NavigateFunction) {
        enqueueAlert('glossary.deleteCourseSuccess', 'success');
        navigate('/dashboard');
        this.closeCourseMenu();
    }

    deleteTopic(id: number) {
        this.assignments!.forEach((a) => {
            if (a.topicId === id) {
                a.topicId = undefined;
            }
        });

        this.materials!.forEach((m) => {
            if (m.topicId === id) {
                m.topicId = undefined;
            }
        });
        const index = this.topics!.findIndex((t) => t.id === id);

        this.topics!.splice(index, 1);
        enqueueAlert('glossary.deleteTopicSuccess', 'success');
    }

    resetEditTopic() {
        this.editTopicData!.title = '';
        this.editTopicId = null;
        Object.keys(this.editTopicErrors).forEach(
            (key) => (this.editTopicErrors[key] = null),
        );
    }

    resetCreateTopic() {
        this.createTopicData!.title = '';
        Object.keys(this.createTopicErrors).forEach(
            (key) => (this.createTopicErrors[key] = null),
        );
    }

    resetCourse() {
        this.courseData!.name = this.course!.courseName;
        this.courseData!.description = this.course!.courseDescription;
        Object.keys(this.courseErrors).forEach((key) => (this.courseErrors[key] = null));
    }

    resetAssignment() {
        this.assignmentData!.assignmentName = '';
        this.assignmentData!.assignmentDeadline = new Date(Date.now() + 86400000);
        this.assignmentData!.assignmentDescription = '';
        this.assignmentData!.assignmentDatePublication = new Date(Date.now());
        this.assignmentData!.assignmentFiles = [];
        this.assignmentData!.assignmentLinks = [];
        this.assignmentData!.isRequired = false;
        this.assignmentData!.maxMark = 100;
        this.assignmentData!.minMark = 0;
        this.assignmentData!.topicId = undefined;

        Object.keys(this.assignmentErrors).forEach(
            (key) => (this.assignmentErrors[key] = null),
        );
    }

    resetMaterial() {
        this.materialData!.materialDatePublication = new Date(Date.now());
        this.materialData!.materialDescription = '';
        this.materialData!.materialFiles = [];
        this.materialData!.materialLinks = [];
        this.materialData!.materialName = '';
        this.materialData!.topicId = undefined;
        Object.keys(this.materialErrors).forEach(
            (key) => (this.materialErrors[key] = null),
        );
    }

    reset(): void {
        this.isLoading = true;
        this.resetAssignment();
        this.resetCourse();
        this.resetMaterial();
        this.resetCreateTopic();
        this.resetEditTopic();
        //this.course = null;

        this.courseMenuAnchor = null;
        this.contentMenuAnchor = null;

        this.editCourseOpen = false;
        this.createTopicOpen = false;
        this.createMaterialOpen = false;
        this.createAssignmentOpen = false;
        this.editTopicOpen = false;
    }
}
