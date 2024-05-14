import { action, computed, makeObservable, observable, runInAction } from 'mobx';
import ValidationError from '../helpers/validation/ValidationError';
import CreateCourseModel from '../models/course/CreateCourseModel';
import CourseService from '../services/CourseService';
import FormStore from './common/FormStore';
import RootStore from './RootStore';
import debounce from '../helpers/debounce';
import { NavigateFunction } from 'react-router-dom';
import LoginRequiredError from '../errors/LoginRequiredError';
import ServiceError from '../errors/ServiceError';
import { enqueueAlert } from '../components/Notification/NotificationProvider';
import CourseInfoModel from '../models/course/CourseInfoModel';

export default class CourseStore extends FormStore {
    private readonly _courseService: CourseService;
    private readonly _rootStore: RootStore;

    data: CreateCourseModel = new CreateCourseModel('');
    errors: Record<string, ValidationError | null> = {
        name: null,
        meta: null,
    };

    drawerOpen = false;
    toggled = false;
    userMenuAnchorEl = null as null | HTMLElement;
    settingsOpen = false;
    settingsTab = '1';
    createCourseOpen = false;

    coursesInfo: CourseInfoModel[] = [];
    isLoading = true;
    needRefresh = false;

    constructor(rootStore: RootStore, courseService: CourseService) {
        super();
        this._rootStore = rootStore;
        this._courseService = courseService;

        makeObservable(this, {
            needRefresh: observable,
            coursesInfo: observable,
            isLoading: observable,
            drawerOpen: observable,
            createCourseOpen: observable,
            toggled: observable,
            userMenuAnchorEl: observable,
            settingsOpen: observable,
            settingsTab: observable,
            data: observable,
            errors: observable,

            isValid: computed,

            handleDrawerOpen: action.bound,
            handleDrawerClose: action.bound,
            toggleDrawer: action.bound,
            handleUserMenuOpen: action.bound,
            handleUserMenuClose: action.bound,
            handleSettingsClose: action.bound,
            handleSettingsOpen: action.bound,
            handleSettingsTabChange: action.bound,
            handleCreateCourseOpen: action.bound,
            handleCreateCourseClose: action.bound,
            submit: action.bound,
            reset: action.bound,
            onNameChange: action.bound,
            onDescriptionChange: action.bound,
            resetCourse: action.bound,
            init: action.bound,
            setNeedRefresh: action.bound,
        });
    }

    async init(navigate: NavigateFunction) {
        try {
            const data = await this._courseService.getCourses();
            runInAction(() => {
                this.coursesInfo = data;
                this.isLoading = false;
                this.needRefresh = false;
            });
        } catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                navigate('/');
                enqueueAlert((error as ServiceError).message, 'error');
            }
        }
    }

    get isValid(): boolean {
        return this.data.validateName().length === 0;
    }

    handleDrawerOpen(): void {
        this.drawerOpen = true;
    }

    handleDrawerClose(): void {
        this.drawerOpen = false;
    }

    toggleDrawer(): void {
        this.toggled = !this.toggled;
    }

    handleUserMenuOpen(event: React.MouseEvent<HTMLElement>): void {
        this.userMenuAnchorEl = event.currentTarget;
    }

    handleUserMenuClose(): void {
        this.userMenuAnchorEl = null;
    }

    handleSettingsClose(): void {
        this.settingsOpen = false;
    }

    handleSettingsOpen(): void {
        this.settingsOpen = true;
    }

    handleSettingsTabChange(event: React.SyntheticEvent, newValue: string): void {
        this.settingsTab = newValue;
    }

    handleCreateCourseOpen(): void {
        this.createCourseOpen = true;
    }

    handleCreateCourseClose(): void {
        this.createCourseOpen = false;
        this.resetCourse();
    }

    onNameChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.data.courseName = e.target.value;
        debounce(
            action(() => {
                const nameErrors = this.data.validateName();
                this.errors.name = nameErrors.length !== 0 ? nameErrors[0] : null;
            }),
        )();
    }

    onDescriptionChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.data.courseDescription = e.target.value;
    }

    async submit(navigate: NavigateFunction) {
        try {
            const newCourse = await this._courseService.createCourse(this.data);
            runInAction(() => {
                this.coursesInfo.push(newCourse);
                this.handleCreateCourseClose();
                navigate(`/course/${newCourse.course.courseId}`);
                enqueueAlert('glossary.courseCreateSuccess', 'success');
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

    resetCourse() {
        this.data.courseName = '';
        this.data.courseDescription = '';
        super.reset();
    }

    setNeedRefresh() {
        this.needRefresh = true;
    }

    reset(): void {
        this.drawerOpen = false;
        this.toggled = false;
        this.userMenuAnchorEl = null;
        this.settingsOpen = false;
        this.settingsTab = '1';
        this.createCourseOpen = false;
        this.isLoading = true;
        this.needRefresh = false;
        this.coursesInfo = [];
        this.resetCourse();
    }
}
