/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { action, computed, makeObservable, observable, runInAction } from 'mobx';
import RootStore from './RootStore';
import CourseModel from '../models/course/CourseModel';
import ValidationError from '../helpers/validation/ValidationError';
import CreateCourseModel from '../models/course/CreateCourseModel';
import debounce from '../helpers/debounce';
import { enqueueAlert } from '../components/Notification/NotificationProvider';
import { NavigateFunction } from 'react-router-dom';
import CreateTopicModel from '../models/topic/CreateTopicModel';
import CreateUpdateMaterialModel from '../models/material/CreateMaterialModel';
import CreateAssignmentModel from '../models/assignment/CreateAssignmentModel';
import TopicModel from '../models/topic/TopicModel';
import { SelectChangeEvent } from '@mui/material';
import { Dayjs } from 'dayjs';
import AssignmentModel from '../models/assignment/AssignmentModel';
import MaterialModel from '../models/material/MaterialModel';
import CourseService from '../services/CourseService';
import AssignmentService from '../services/AssignmentService';
import MaterialService from '../services/MaterialService';
import TopicService from '../services/TopicService';
import LoginRequiredError from '../errors/LoginRequiredError';
import ServiceError from '../errors/ServiceError';
import CourseInfoModel from '../models/course/CourseInfoModel';
import UpdateCourseModel from '../models/course/UpdateCourseModel';
import UpdateTopicModel from '../models/topic/UpdateTopicModel';

export default class CoursePageStore {
    private readonly _rootStore: RootStore;
    private readonly _courseService: CourseService;
    private readonly _assignmetnService: AssignmentService;
    private readonly _materialService: MaterialService;
    private readonly _topicService: TopicService;

    course: CourseInfoModel | null = null;
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

    courseData: UpdateCourseModel | null = null;
    courseErrors: Record<string, ValidationError | null> = {
        name: null,
        meta: null,
    };

    createTopicData: CreateTopicModel | null = null;
    createTopicErrors: Record<string, ValidationError | null> = {
        title: null,
        meta: null,
    };

    editTopicData: UpdateTopicModel | null = null;
    editTopicErrors: Record<string, ValidationError | null> = {
        title: null,
        meta: null,
    };
    materialData: CreateUpdateMaterialModel | null = null;
    materialErrors: Record<string, ValidationError | null> = {
        materialName: null,
        materialFiles: null,
        materialLinks: null,
        meta: null,
    };

    assignmentData: CreateAssignmentModel | null = null;
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

    constructor(rootStore: RootStore, courseService: CourseService, assignmentService: AssignmentService, materialService: MaterialService, topicService: TopicService) {
        this._rootStore = rootStore;
        this._assignmetnService = assignmentService;
        this._materialService = materialService;
        this._courseService = courseService;
        this._topicService = topicService;

        makeObservable(this, {
            isLoading: observable,
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

    async init(courseId: number, navigate: NavigateFunction) {
        try {
            const course = await this._courseService.getCourse(courseId);
            const assignments = await this._assignmetnService.getAssignments(course.course.courseId);
            const materials = await this._materialService.getMaterials(course.course.courseId);
            const topics = await this._topicService.getTopics(course.course.courseId);

            runInAction(() => {
                this.course = course;
                this.assignments = assignments;
                this.materials = materials;
                this.topics = topics;

                this.courseData = new UpdateCourseModel(this.course.course.courseId,
                    this.course.course.courseName,
                    this.course.course.courseDescription);

                this.createTopicData = new CreateTopicModel(this.course.course.courseId, '');

                this.materialData = new CreateUpdateMaterialModel(
                    this.course.course.courseId,
                    '',
                    [],
                    [],
                    new Date(Date.now()),
                );

                this.assignmentData = new CreateAssignmentModel(
                    this.course.course.courseId,
                    '',
                    100,
                    0,
                    false,
                    new Date(Date.now() + 86400000),
                    [],
                    [],
                    new Date(Date.now()),
                );

                this.editTopicData = new CreateTopicModel(this.course.course.courseId, '');

                this.isLoading = false;
            });
        } catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                navigate('/dashboard');
                enqueueAlert((error as ServiceError).message, 'error');
            }
        }
    }

    onNameChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.courseData!.courseName = e.target.value;
        debounce(
            action(() => {
                const nameErrors = this.courseData!.validateName();
                this.courseErrors.name = nameErrors.length !== 0 ? nameErrors[0] : null;
            }),
        )();
    }

    onDescriptionChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.courseData!.courseDescription = e.target.value;
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
            this.assignmentData!.AssignmentFiles.push(...Array.from(e.target.files));
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
        this.assignmentData?.AssignmentFiles.splice(id, 1);
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

    async submitCourse(navigate: NavigateFunction) {
        try {
            const updatedCourse = await this._courseService.updateCourse(this.courseData!);

            runInAction(() => {
                const index = this._rootStore.courseStore.coursesInfo.findIndex(c => c.course.courseId === updatedCourse.course.courseId);
                this._rootStore.courseStore.coursesInfo[index] = updatedCourse;
                this.course = updatedCourse;
                this.handleEditCourseClose();
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

    async submitCreateTopic(navigate: NavigateFunction) {
        try {
            const createdTopic = await this._topicService.createTopic(this.createTopicData!);
            runInAction(() => {
                this.topics!.push(createdTopic);
                this.handleCreateTopicClose();
                enqueueAlert('glossary.topicCreateSuccess', 'success');
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

    async submitMaterial(navigate: NavigateFunction) {
        try {
            const createdMaterial = await this._materialService.createMaterial(this.materialData!);
            runInAction(() => {
                this.materials!.push(createdMaterial);
                this.handleCreateMaterialClose();
                enqueueAlert('glossary.materialCreateSuccess', 'success');
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

    async submitAssignment(navigate: NavigateFunction) {
        try {
            const createdAssignment = await this._assignmetnService.createAssignment(this.assignmentData!);
            runInAction(() => {
                this.assignments!.push(createdAssignment);
                this.handleCreateAssignmentClose();
                enqueueAlert('glossary.assignmentCreateSuccess', 'success');
            })
        } catch (error) {
            this.handleCreateAssignmentClose();
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                enqueueAlert((error as ServiceError).message, 'error');
            }
        }
    }

    async submitEditTopic(navigate: NavigateFunction) {
        try {
            const updatedTopic = await this._topicService.updateTopic(this.editTopicData!);
            runInAction(() => {
                const index = this.topics!.findIndex(t => t.id === updatedTopic.id);
                this.topics![index] = updatedTopic;
                this.handleEditTopicClose();
                enqueueAlert('glossary.editSuccess', 'success');
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
        this.editTopicData!.id = topic!.id;
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
        try {
            this._courseService.deleteCourse(this.course!.course.courseId);
            runInAction(() => {
                const index = this._rootStore.courseStore.coursesInfo.findIndex(c => c.course.courseId === this.course!.course.courseId);
                this._rootStore.courseStore.coursesInfo.splice(index, 1);
                enqueueAlert('glossary.deleteCourseSuccess', 'success');
                navigate('/dashboard');
                this.closeCourseMenu();
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

    async deleteTopic(id: number, navigate: NavigateFunction) {
        try {
            await this._topicService.deleteTopic(id);
            runInAction(() => {
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

    resetEditTopic() {
        this.editTopicData!.title = '';
        this.editTopicData!.id = undefined;
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
        this.courseData!.courseName = this.course!.course.courseName;
        this.courseData!.courseDescription = this.course!.course.courseDescription;
        Object.keys(this.courseErrors).forEach((key) => (this.courseErrors[key] = null));
    }

    resetAssignment() {
        this.assignmentData!.assignmentName = '';
        this.assignmentData!.assignmentDeadline = new Date(Date.now() + 86400000);
        this.assignmentData!.assignmentDescription = '';
        this.assignmentData!.assignmentDatePublication = new Date(Date.now());
        this.assignmentData!.AssignmentFiles = [];
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

        this.course = null;
        this.assignments = null;
        this.topics = null;
        this.materials = null;

        Object.keys(this.materialErrors).forEach(
            (key) => (this.materialErrors[key] = null),
        );
        Object.keys(this.assignmentErrors).forEach(
            (key) => (this.assignmentErrors[key] = null),
        );
        Object.keys(this.courseErrors).forEach((key) => (this.courseErrors[key] = null));
        Object.keys(this.createTopicErrors).forEach(
            (key) => (this.createTopicErrors[key] = null),
        );
        Object.keys(this.editTopicErrors).forEach(
            (key) => (this.editTopicErrors[key] = null),
        );

        this.createTopicData = null;
        this.courseData = null;
        this.editTopicData = null;
        this.assignmentData = null;
        this.materialData = null;

        this.courseMenuAnchor = null;
        this.contentMenuAnchor = null;

        this.editCourseOpen = false;
        this.createTopicOpen = false;
        this.createMaterialOpen = false;
        this.createAssignmentOpen = false;
        this.editTopicOpen = false;
    }
}
