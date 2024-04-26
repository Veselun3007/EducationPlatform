import { action, computed, makeObservable, observable } from 'mobx';
import ValidationError from '../helpers/validation/ValidationError';
import CreateUpdateCourseModel from '../models/course/CreateUpdateCourseModel';
import CourseService from '../services/CourseService';
import FormStore from './common/FormStore';
import RootStore from './RootStore';
import debounce from '../helpers/debounce';

export default class NavigationPanelStore extends FormStore {
    private readonly _courseService: CourseService;
    private readonly _rootStore: RootStore;

    data: CreateUpdateCourseModel = new CreateUpdateCourseModel('');
    errors: Record<string, ValidationError | null> = {
        name: null,
        meta: null,
    };

    drawerOpen = false;
    toggled = false;
    userMenuAnchorEl = null as null | HTMLElement;
    settingsOpen = false;
    settingsTab = '1';
    courseMenuAnchorEl = null as null | HTMLElement;
    createCourseOpen = false;

    constructor(rootStore: RootStore, courseService: CourseService) {
        super();
        this._rootStore = rootStore;
        this._courseService = courseService;

        makeObservable(this, {
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
        });
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

    handleCourseMenuOpen(event: React.MouseEvent<HTMLButtonElement>): void {
        this.courseMenuAnchorEl = event.currentTarget;
    }

    handleCourseMenuClose(): void {
        this.courseMenuAnchorEl = null;
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
        this.reset();
    }

    onNameChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.data.name = e.target.value;
        debounce(
            action(() => {
                const nameErrors = this.data.validateName();
                this.errors.name = nameErrors.length !== 0 ? nameErrors[0] : null;
            }),
        )();
    }

    onDescriptionChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.data.description = e.target.value;
    }

    submit(...params: unknown[]): void {
        throw new Error('Method not implemented.');
    }

    reset(): void {
        this.data.name = '';
        this.data.description = '';

        super.reset();
    }
}
