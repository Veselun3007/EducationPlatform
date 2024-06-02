/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { action, computed, makeObservable, observable, runInAction } from 'mobx';
import RootStore from './RootStore';
import MaterialModel from '../models/material/MaterialModel';
import TopicModel from '../models/topic/TopicModel';
import ValidationError from '../helpers/validation/ValidationError';
import { NavigateFunction } from 'react-router-dom';
import { enqueueAlert } from '../components/Notification/NotificationProvider';
import debounce from '../helpers/debounce';
import { SelectChangeEvent } from '@mui/material';
import MaterialService from '../services/MaterialService';
import TopicService from '../services/TopicService';
import UpdateMaterialModel from '../models/material/UpdateMaterialModel';
import LoginRequiredError from '../errors/LoginRequiredError';
import ServiceError from '../errors/ServiceError';
import FileValidator from '../helpers/validation/FileValidator';
import StringValidator from '../helpers/validation/StringValidator';

export default class MaterialPageStore {
    private readonly _rootStore: RootStore;
    private readonly _materialService: MaterialService;
    private readonly _topicService: TopicService;

    isLoading = true;

    material: MaterialModel | null = null;
    topics: TopicModel[] | null = null;

    editMaterialData: UpdateMaterialModel | null = null;
    editMaterialErrors: Record<string, ValidationError | null> = {
        materialName: null,
        materialFiles: null,
        materialLinks: null,
        meta: null,
    };

    materialMenuAnchor: null | HTMLElement = null;
    editMaterialOpen = false;

    isFileViewerOpen = false;
    fileLink = '';

    linkAdd = '';
    isLinkAddOpen = false;
    isTeacher = false;

    constructor(
        rootStore: RootStore,
        materialService: MaterialService,
        topicService: TopicService,
    ) {
        this._rootStore = rootStore;
        this._materialService = materialService;
        this._topicService = topicService;

        makeObservable(this, {
            isTeacher: observable,
            linkAdd: observable,
            isLinkAddOpen: observable,
            isLoading: observable,
            material: observable,
            topics: observable,
            editMaterialData: observable,
            editMaterialErrors: observable,
            materialMenuAnchor: observable,
            editMaterialOpen: observable,
            isFileViewerOpen: observable,
            fileLink: observable,

            isEditMaterialValid: computed,

            init: action.bound,
            onFileClick: action.bound,
            onFileViewerClose: action.bound,
            onMaterialDescriptionChange: action.bound,
            onMaterialFileAdd: action.bound,
            onMaterialFileDelete: action.bound,
            onMaterialLinkAdd: action.bound,
            onMaterialLinkDelete: action.bound,
            onMaterialNameChange: action.bound,
            onMaterialTopicChange: action.bound,
            openMaterialMenu: action.bound,
            handleEditMaterialOpen: action.bound,
            handleEditMaterialClose: action.bound,
            submitMaterialEdit: action.bound,
            deleteMaterial: action.bound,
            closeMaterialMenu: action.bound,
            reset: action.bound,
            resetEditMaterial: action.bound,
            onLinkAddChange: action.bound,
            handleLinkAddClose: action.bound,
            handleLinkAddOpen: action.bound,
        });
    }

    get isEditMaterialValid(): boolean {
        return this.editMaterialData!.validateMaterialName().length === 0;
    }

    async init(courseId: number, materialId: number, navigate: NavigateFunction) {
        try {
            const material = await this._materialService.getMaterialById(materialId);
            const topics = await this._topicService.getTopics(courseId);

            runInAction(() => {
                this.material = material;
                this.topics = topics;

                this.editMaterialData = new UpdateMaterialModel(
                    this.material.id,
                    courseId,
                    this.material.materialName,
                    this.material.materialDatePublication,
                    this.material.topicId,
                    this.material.materialDescription,
                );
                this.isLoading = false;
                const course = this._rootStore.courseStore.coursesInfo.find(
                    (c) => c.course.courseId === courseId,
                );
                if (!course) navigate('/');
                this.isTeacher =
                    course?.userInfo.role === 0 || course?.userInfo.role === 1;
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

    onMaterialNameChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.editMaterialData!.materialName = e.target.value;
        debounce(
            action(() => {
                const errors = this.editMaterialData!.validateMaterialName();
                this.editMaterialErrors.materialName =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onMaterialTopicChange(event: SelectChangeEvent) {
        const value = Number.parseInt(event.target.value);
        if (value !== 0) {
            this.editMaterialData!.topicId = value;
        } else {
            this.editMaterialData!.topicId = undefined;
        }
    }

    onMaterialDescriptionChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.editMaterialData!.materialDescription = e.target.value;
    }

    async onMaterialFileAdd(
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
            const addedFile = await this._materialService.addFile(
                e.target.files[0],
                this.material!.id,
            );
            runInAction(() => {
                this.material!.materialfiles!.push(addedFile);
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

    async onMaterialFileDelete(fileId: number, navigate: NavigateFunction) {
        try {
            await this._materialService.deleteFileById(fileId);
            runInAction(() => {
                const index = this.material!.materialfiles!.findIndex(
                    (f) => f.id == fileId,
                );
                this.material!.materialfiles!.splice(index, 1);
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

    async onMaterialLinkAdd(navigate: NavigateFunction) {
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
            const addedLink = await this._materialService.addLink(
                this.material!.id,
                this.linkAdd,
            );
            runInAction(() => {
                this.material!.materiallinks!.push(addedLink);
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

    async onMaterialLinkDelete(linkId: number, navigate: NavigateFunction) {
        try {
            await this._materialService.deleteLinkById(linkId);
            runInAction(() => {
                const index = this.material!.materiallinks!.findIndex(
                    (l) => l.id == linkId,
                );
                this.material!.materiallinks!.splice(index, 1);
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

    async submitMaterialEdit(navigate: NavigateFunction) {
        try {
            const updatedAssignment = await this._materialService.updateMaterial(
                this.editMaterialData!,
            );
            runInAction(() => {
                this.material = updatedAssignment;
                this.handleEditMaterialClose();
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

    async deleteMaterial(courseId: number, navigate: NavigateFunction) {
        try {
            await this._materialService.deleteMaterial(this.material!.id);
            runInAction(() => {
                enqueueAlert('glossary.deleteMaterialSuccess', 'success');
                this.closeMaterialMenu();
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

    async onFileClick(id: number, navigate: NavigateFunction) {
        try {
            const link = await this._materialService.getMaterialFileById(id);
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

    handleEditMaterialOpen() {
        this.editMaterialOpen = true;
        this.closeMaterialMenu();
    }

    handleEditMaterialClose() {
        this.resetEditMaterial();
        this.editMaterialOpen = false;
    }

    openMaterialMenu(event: React.MouseEvent<HTMLButtonElement>) {
        this.materialMenuAnchor = event.currentTarget;
    }

    closeMaterialMenu() {
        this.materialMenuAnchor = null;
    }

    resetEditMaterial() {
        this.editMaterialData!.materialDatePublication =
            this.material!.materialDatePublication;
        this.editMaterialData!.materialDescription = this.material!.materialDescription;
        this.editMaterialData!.materialName = this.material!.materialName;
        this.editMaterialData!.topicId = this.material!.topicId;
        Object.keys(this.editMaterialErrors).forEach(
            (key) => (this.editMaterialErrors[key] = null),
        );
    }

    reset() {
        this.isLoading = true;
        this.isTeacher = false;
        this.editMaterialData = null;
        Object.keys(this.editMaterialErrors).forEach(
            (key) => (this.editMaterialErrors[key] = null),
        );
        this.material = null;
        this.isFileViewerOpen = false;
        this.fileLink = '';
        this.editMaterialOpen = false;
        this.materialMenuAnchor = null;

        this.linkAdd = '';
        this.isLinkAddOpen = false;
    }
}
