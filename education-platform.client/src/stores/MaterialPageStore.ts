/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { action, computed, makeObservable, observable } from 'mobx';
import RootStore from './RootStore';
import MaterialModel from '../models/material/MaterialModel';
import TopicModel from '../models/topic/TopicModel';
import ValidationError from '../helpers/validation/ValidationError';
import CreateUpdateMaterialModel from '../models/material/CreateUpdateMaterialModel';
import { NavigateFunction } from 'react-router-dom';
import { enqueueAlert } from '../components/Notification/NotificationProvider';
import debounce from '../helpers/debounce';
import { avatarClasses, SelectChangeEvent } from '@mui/material';

export default class MaterialPageStore {
    private readonly _rootStore: RootStore;

    isLoading = true;

    material: MaterialModel | null = null;
    topics: TopicModel[] | null = null;

    editMaterialData: CreateUpdateMaterialModel | null = null;
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

    constructor(rootStore: RootStore) {
        this._rootStore = rootStore;
        makeObservable(this, {
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
        });
    }

    get isEditMaterialValid(): boolean {
        return (
            this.editMaterialData!.validateMaterialName().length === 0 &&
            this.editMaterialData!.validateMaterialFiles().length === 0 &&
            this.editMaterialData!.validateMaterialLinks().length === 0
        );
    }

    init(courseId: number, materialId: number) {
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

        this.material = {
            id: 1,
            topicId: 1,
            materialName: 'Introduction to Algorithms',
            materialDescription: 'Overview of basic algorithms and their analysis.',
            materialDatePublication: new Date('2024-04-20'),
            isEdited: true,
            editedTime: new Date('2024-04-23'),
            materialfiles: [
                {
                    id: 1,
                    materialFile:
                        'https://educationplatform.s3.amazonaws.com/a274a45d-3d68-4a5e-8a97-41475043552f_trust%20me%20cooper.jpg?AWSAccessKeyId=AKIA6GBMGKKRTD7XQ7W6&Expires=1714034789&Signature=7FVpHNgWlhgXnDfl31o4Jn6w%2FFQ%3D',
                },
            ],
            materiallinks: [
                {
                    id: 1,
                    materialLink:
                        'https://mitpress.mit.edu/books/introduction-algorithms',
                },
            ],
        };

        this.editMaterialData = new CreateUpdateMaterialModel(
            courseId,
            this.material.materialName,
            [],
            [],
            this.material.materialDatePublication,
            this.material.topicId,
            this.material.materialDescription,
        );

        //add autofill for files

        this.isLoading = false;
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
    onMaterialFileAdd(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.files) {
            this.editMaterialData!.materialFiles.push(...Array.from(e.target.files));
        }
        debounce(
            action(() => {
                const errors = this.editMaterialData!.validateMaterialFiles();
                this.editMaterialErrors.materialFiles =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }
    onMaterialFileDelete(id: number) {
        this.editMaterialData?.materialFiles.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.editMaterialData!.validateMaterialFiles();
                this.editMaterialErrors.materialFiles =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onMaterialLinkAdd(link: string) {
        this.editMaterialData?.materialLinks.push(link);
        debounce(
            action(() => {
                const errors = this.editMaterialData!.validateMaterialLinks();
                this.editMaterialErrors.materialLinks =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onMaterialLinkDelete(id: number) {
        this.editMaterialData?.materialLinks.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.editMaterialData!.validateMaterialLinks();
                this.editMaterialErrors.materialLinks =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onMaterialDescriptionChange(e: React.ChangeEvent<HTMLInputElement>) {
        this.editMaterialData!.materialDescription = e.target.value;
    }

    submitMaterialEdit() {
        this.handleEditMaterialClose();
    }

    deleteMaterial(courseId: number, navigate: NavigateFunction) {
        enqueueAlert('glossary.deleteMaterialSuccess', 'success');
        navigate(`/course/${courseId}`);
        this.closeMaterialMenu();
    }

    onFileClick(id: number) {
        this.isFileViewerOpen = true;
        this.fileLink = this.material!.materialfiles![id].materialFile!;
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
        this.editMaterialData!.materialFiles = [];
        this.editMaterialData!.materialLinks = []; //change it with real files
        this.editMaterialData!.materialName = this.material!.materialName;
        this.editMaterialData!.topicId = this.material!.topicId;
        Object.keys(this.editMaterialErrors).forEach(
            (key) => (this.editMaterialErrors[key] = null),
        );
    }

    reset() {
        this.resetEditMaterial();
        this.isLoading = true;

        this.isFileViewerOpen = false;
        this.fileLink = '';
        this.editMaterialOpen = false;
        this.materialMenuAnchor = null;
        this.material = null;
    }
}
