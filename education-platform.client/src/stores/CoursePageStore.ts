/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { action, computed, makeObservable, observable } from 'mobx';
import IStore from './common/IStore';
import AuthService from '../services/AuthService';
import RootStore from './RootStore';
import CourseModel from '../models/course/CourseModel';
import FormStore from './common/FormStore';
import ValidationError from '../helpers/validation/ValidationError';
import CreateUpdateCourseModel from '../models/course/CreateUpdateCourseModel';
import debounce from '../helpers/debounce';
import { enqueueAlert } from '../components/Notification/NotificationProvider';
import { NavigateFunction } from 'react-router-dom';
import CreateUpdateTopicModel from '../models/topic/CreateUpdateTopicModel';
import CreateUpdateMaterialModel from '../models/material/CreateUpdateMaterialModel';
import CreateUpdateAssignmentModel from '../models/assignment/CreateUpdateAssignmentModel';
import TopicModel from '../models/topic/TopicModel';
import { avatarClasses, SelectChangeEvent } from '@mui/material';
import { Dayjs } from 'dayjs';

export default class CoursePageStore {

    private readonly _rootStore: RootStore;

    course: CourseModel | null = null;
    topics: TopicModel[] | null = null;

    courseMenuAnchor: null | HTMLElement = null;
    contentMenuAnchor: null | HTMLElement = null;

    editCourseOpen = false;
    createTopicOpen = false;
    createMaterialOpen = false;
    createAssignmentOpen = false;


    courseData: CreateUpdateCourseModel | null = null;
    courseErrors: Record<string, ValidationError | null> = {
        name: null,
        meta: null,
    };

    topicData: CreateUpdateTopicModel | null = null;
    topicErrors: Record<string, ValidationError | null> = {
        title: null,
        meta: null
    }

    materialData: CreateUpdateMaterialModel | null = null;
    materialErrors: Record<string, ValidationError | null> = {
        materialName: null,
        materialFiles: null,
        materialLinks: null,
        meta: null
    }

    assignmentData: CreateUpdateAssignmentModel | null = null;
    assignmentErrors: Record<string, ValidationError | null> = {
        assignmentName: null,
        maxMark: null,
        minMark: null,
        isRequired: null,
        assignmentDeadline: null,
        assignmentFiles: null,
        assignmentLinks: null,
        meta: null
    }

    constructor(rootStore: RootStore) {
        this._rootStore = rootStore;

        makeObservable(this, {
            createMaterialOpen: observable,
            topics: observable,
            createAssignmentOpen: observable,
            courseData: observable,
            courseErrors: observable,
            course: observable,
            courseMenuAnchor: observable,
            contentMenuAnchor: observable,
            editCourseOpen: observable,
            topicData: observable,
            topicErrors: observable,
            createTopicOpen: observable,
            materialData: observable,
            materialErrors: observable,
            assignmentData: observable,
            assignmentErrors: observable,

            isCourseValid: computed,
            isTopicValid: computed,
            isMaterialValid: computed,
            isAssignmentValid: computed,

            getCourse: action.bound,
            onNameChange: action.bound,
            onDescriptionChange: action.bound,
            openCourseMenu: action.bound,
            closeCourseMenu: action.bound,
            handleEditCourseOpen: action.bound,
            handleEditCourseClose: action.bound,
            reset: action.bound,
            submitCourse: action.bound,
            deleteCourse: action.bound,
            onTitleChange: action.bound,
            submitTopic: action.bound,
            openContentMenu: action.bound,
            closeContentMenu: action.bound,
            handleCreateTopicOpen: action.bound,
            handleCreateTopicClose: action.bound,
            resetAssignment: action.bound,
            resetCourse: action.bound,
            resetMaterial: action.bound,
            resetTopic: action.bound,
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
            onAssignmentTopicChange: action.bound
        });
    }

    get isCourseValid(): boolean {
        return this.courseData!.validateName().length === 0;
    }

    get isTopicValid(): boolean {
        return this.topicData!.validateTitle().length === 0;
    }

    get isMaterialValid(): boolean {
        return this.materialData!.validateMaterialName().length === 0 &&
            this.materialData!.validateMaterialFiles().length === 0 &&
            this.materialData!.validateMaterialLinks().length === 0;
    }

    get isAssignmentValid(): boolean {
        return this.assignmentData!.validateAssignmentDeadline().length === 0 &&
            this.assignmentData!.validateAssignmentFiles().length === 0 &&
            this.assignmentData!.validateAssignmentLinks().length === 0 &&
            this.assignmentData!.validateAssignmentName().length === 0 &&
            this.assignmentData!.validateIsRequired().length === 0 &&
            this.assignmentData!.validateMaxMark().length === 0 &&
            this.assignmentData!.validateMinMark().length === 0;
    }

    getCourse() {
        this.course = {
            courseId: 1,
            courseName: 'Course name 1 ВАП ВФ ІАР ПВІАПВАП ВАПВА ВІПІПІВП',
            courseDescription:
                'ThiThis is description for cource 1s is description for cource 1 This is description for cource 1, This is description for cource 1 This is description for cource 1This is description for cource 1',
            courseLink: 'link 1'
        };

        this.courseData = new CreateUpdateCourseModel(this.course.courseName, this.course.courseDescription);

        this.topicData = new CreateUpdateTopicModel(this.course.courseId, '');
        this.materialData = new CreateUpdateMaterialModel(this.course.courseId, '', [], [], new Date(Date.now()));
        this.assignmentData = new CreateUpdateAssignmentModel(this.course.courseId, '', 100, 0, false, new Date(Date.now() + 86400000), [], [], new Date(Date.now()));
        this.topics = [
            {
                courseId: 1,
                id: 1,
                title: "topic1",
            },
            {
                courseId: 1,
                id: 2,
                title: "topic2",
            },
            {
                courseId: 1,
                id: 3,
                title: "topic3",
            },
        ]

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

    onTitleChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.topicData!.title = e.target.value;
        debounce(
            action(() => {
                const titleErrors = this.topicData!.validateTitle();
                this.topicErrors.title = titleErrors.length !== 0 ? titleErrors[0] : null;
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
        const value = Number.parseInt(event.target.value)
        if (value !== 0) {
            this.materialData!.topicId = value;
        } else {
            this.materialData!.topicId = undefined;
        }
    }
    onMaterialFileAdd(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.files) {
            this.materialData!.materialFiles.push(...Array.from(e.target.files))
        }
        debounce(
            action(() => {
                const errors = this.materialData!.validateMaterialFiles();
                this.materialErrors.materialFiles = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }
    onMaterialFileDelete(id: number) {
        this.materialData?.materialFiles.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.materialData!.validateMaterialFiles();
                this.materialErrors.materialFiles = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onMaterialLinkAdd(link: string) {
        this.materialData?.materialLinks.push(link)
        debounce(
            action(() => {
                const errors = this.materialData!.validateMaterialLinks();
                this.materialErrors.materialLinks = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onMaterialLinkDelete(id: number) {
        this.materialData?.materialLinks.splice(id, 1)
        debounce(
            action(() => {
                const errors = this.materialData!.validateMaterialLinks();
                this.materialErrors.materialLinks = errors.length !== 0 ? errors[0] : null;
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
                this.assignmentErrors.assignmentName = errors.length !== 0 ? errors[0] : null;
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
                    this.assignmentErrors.maxMark = errors.length !== 0 ? errors[0] : null;
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
                    this.assignmentErrors.minMark = errors.length !== 0 ? errors[0] : null;
                }),
            )();
        }
    }

    onAssignmentTopicChange(event: SelectChangeEvent) {
        const value = Number.parseInt(event.target.value)
        if (value !== 0) {
            this.assignmentData!.topicId = value;
        } else {
            this.assignmentData!.topicId = undefined;
        }
    }

    onAssignmentIsRequiredChange(event: SelectChangeEvent) {
        const value = Number.parseInt(event.target.value)
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
                    this.assignmentErrors.assignmentDeadline = errors.length !== 0 ? errors[0] : null;
                }),
            )();
        }
    }

    onAssignmentFileAdd(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.files) {
            this.assignmentData!.assignmentFiles.push(...Array.from(e.target.files))
        }
        debounce(
            action(() => {
                const errors = this.assignmentData!.validateAssignmentFiles();
                this.assignmentErrors.assignmentFiles = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }
    onAssignmentFileDelete(id: number) {
        this.assignmentData?.assignmentFiles.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.assignmentData!.validateAssignmentFiles();
                this.assignmentErrors.assignmentFiles = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onAssignmentLinkAdd(link: string) {
        this.assignmentData?.assignmentLinks.push(link)
        debounce(
            action(() => {
                const errors = this.assignmentData!.validateAssignmentLinks();
                this.assignmentErrors.assignmentLinks = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onAssignmentLinkDelete(id: number) {
        this.assignmentData?.assignmentLinks.splice(id, 1)
        debounce(
            action(() => {
                const errors = this.assignmentData!.validateAssignmentLinks();
                this.assignmentErrors.assignmentLinks = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    submitCourse() {

        this.handleEditCourseClose();

    }

    submitTopic() {
        this.handleCreateTopicClose();
    }

    submitMaterial() {
        this.handleCreateMaterialClose();
    }

    submitAssignment() {
        this.handleCreateAssignmentClose();
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
        this.resetCourse()
        this.editCourseOpen = false;
        this.closeCourseMenu();
    }

    handleCreateTopicOpen() {
        this.createTopicOpen = true;
        this.closeContentMenu();
    }

    handleCreateTopicClose() {
        this.resetTopic();
        this.createTopicOpen = false;
        this.closeContentMenu();
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

    resetTopic() {
        this.topicData!.title = '';
        Object.keys(this.topicErrors).forEach((key) => (this.topicErrors[key] = null));
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

        Object.keys(this.assignmentErrors).forEach((key) => (this.assignmentErrors[key] = null));
    }

    resetMaterial() {
        this.materialData!.materialDatePublication = new Date(Date.now());
        this.materialData!.materialDescription = '';
        this.materialData!.materialFiles = [];
        this.materialData!.materialLinks = [];
        this.materialData!.materialName = '';
        this.materialData!.topicId = undefined;
        Object.keys(this.materialErrors).forEach((key) => (this.materialErrors[key] = null));
    }

    reset(): void {
        this.resetAssignment();
        this.resetCourse();
        this.resetMaterial();
        this.resetTopic();
        this.course = null;
    }
}
