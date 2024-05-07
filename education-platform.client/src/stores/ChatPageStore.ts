/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { action, computed, makeObservable, observable, runInAction } from "mobx";
import RootStore from "./RootStore";
import MessageModel from "../models/message/MessageModel";
import { NavigateFunction } from "react-router-dom";
import CreateMessageModel from "../models/message/CreateMessageModel";
import ValidationError from "../helpers/validation/ValidationError";
import debounce from "../helpers/debounce";
import UpdateMessageModel from "../models/message/UpdateMessageModel";
import FileValidator from "../helpers/validation/FileValidator";
import { enqueueAlert } from "../components/Notification/NotificationProvider";

export default class ChatPageStore {
    private readonly _rootStore: RootStore;

    messages: MessageModel[] = [];
    hasMore = true;

    currentUser: number | null = null;

    isLoading = true;

    messageMenuAnchor: null | HTMLElement = null;
    selectedMessage: null | number = null;

    editMessageOpen = false;

    createMessage: CreateMessageModel | null = null;
    createMessageErrors: Record<string, ValidationError | null> = {
        messageText: null,
        attachedFiles: null,
    }

    editMessage: UpdateMessageModel | null = null;
    editMessageErrors: Record<string, ValidationError | null> = {
        messageText: null
    }

    isFileViewerOpen = false;
    fileLink=''

    constructor(rootStore: RootStore) {

        this._rootStore = rootStore;

        makeObservable(this, {
            fileLink: observable,
            isFileViewerOpen: observable,
            editMessage: observable,
            editMessageErrors: observable,
            editMessageOpen: observable,
            messageMenuAnchor: observable,
            messages: observable,
            isLoading: observable,
            currentUser: observable,
            selectedMessage: observable,
            hasMore: observable,
            createMessageErrors: observable,
            createMessage: observable,

            messageLength: computed,
            isCreateMessageValid: computed,
            isEditMessageValid: computed,
            canDeleteFile: computed,

            init: action.bound,
            loadNextPack: action.bound,
            reset: action.bound,
            openMessageMenu: action.bound,
            closeMessageMenu: action.bound,
            handeEditMessageOpen: action.bound,
            handleEditMessageClose: action.bound,
            resetEditMessage: action.bound,
            resetCreateMessage: action.bound,
            onCreateMessageTextChange: action.bound,
            sendMessage: action.bound,
            onCreateMessageFileAdd: action.bound,
            onCreateMessageFileDelete: action.bound,
            deleteMessage: action.bound,
            onEditMessageTextChange: action.bound,
            submitEdit: action.bound,
            onEditMessageFileAdd: action.bound,
            onEditMessageFileDelete: action.bound,
            onFileClick: action.bound,
            onFileViewerClose: action.bound
        });
    }

    get messageLength() {
        return this.messages.length;
    }

    async init(courseId: number, chatId: number, navigate: NavigateFunction) {
        runInAction(() => {
            this.messages = [
                {
                    id: 1,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'Good morning!',
                    creatorId: 21,
                    createdIn: new Date('2022-01-01T06:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: [
                        {
                            id: 1,
                            messageId: 1,
                            mediaLink: 'https://example.com/image1.jpg'
                        },
                        {
                            id: 2,
                            messageId: 1,
                            mediaLink: 'https://example.com/image2.jpg'
                        }
                    ]
                },
                {
                    id: 2,
                    chatId: 1,
                    replyToMessageId: 1,
                    messageText: 'What are your plans for today?',
                    creatorId: 3,
                    createdIn: new Date('2022-01-01T08:30:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: []
                },
                {
                    id: 3,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'I\'m going to the gym and then to work.',
                    creatorId: 21,
                    createdIn: new Date('2022-01-01T08:45:00.000Z'),
                    isEdit: true,
                    editedIn: new Date('2022-01-01T08:50:00.000Z'),
                    attachedFiles: []
                },
                {
                    id: 4,
                    chatId: 1,
                    replyToMessageId: 3,
                    messageText: null,
                    creatorId: 21,
                    createdIn: new Date('2022-01-01T09:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: [
                        {
                            id: 1,
                            messageId: 4,
                            mediaLink: 'https://example.com/image1.jpg'
                        },
                        {
                            id: 2,
                            messageId: 4,
                            mediaLink: 'https://example.com/image2.jpg'
                        },
                        {
                            id: 3,
                            messageId: 4,
                            mediaLink: 'https://example.com/image3.jpg'
                        }
                    ]
                },
                {
                    id: 5,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'Have a great day!',
                    creatorId: 2,
                    createdIn: new Date('2022-01-01T17:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: null
                },
                {
                    id: 6,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'Good morning!',
                    creatorId: 2,
                    createdIn: new Date('2022-01-01T06:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: [
                        {
                            id: 1,
                            messageId: 1,
                            mediaLink: 'https://example.com/image1.jpg'
                        },
                        {
                            id: 2,
                            messageId: 1,
                            mediaLink: 'https://example.com/image2.jpg'
                        }
                    ]
                },
                {
                    id: 7,
                    chatId: 1,
                    replyToMessageId: 1,
                    messageText: 'What are your plans for today?',
                    creatorId: 3,
                    createdIn: new Date('2022-01-01T08:30:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: []
                },
                {
                    id: 8,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'I\'m going to the gym and then to work.',
                    creatorId: 2,
                    createdIn: new Date('2022-01-01T08:45:00.000Z'),
                    isEdit: true,
                    editedIn: new Date('2022-01-01T08:50:00.000Z'),
                    attachedFiles: []
                },
                {
                    id: 9,
                    chatId: 1,
                    replyToMessageId: 3,
                    messageText: null,
                    creatorId: 3,
                    createdIn: new Date('2022-01-01T09:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: [
                        {
                            id: 1,
                            messageId: 4,
                            mediaLink: 'https://example.com/image1.jpg'
                        },
                        {
                            id: 2,
                            messageId: 4,
                            mediaLink: 'https://example.com/image2.jpg'
                        },
                        {
                            id: 3,
                            messageId: 4,
                            mediaLink: 'https://example.com/image3.jpg'
                        }
                    ]
                },
                {
                    id: 10,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'Have a great day!',
                    creatorId: 2,
                    createdIn: new Date('2022-01-01T17:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: null
                },
                {
                    id: 11,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'Good morning!',
                    creatorId: 2,
                    createdIn: new Date('2022-01-01T06:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: [
                        {
                            id: 1,
                            messageId: 1,
                            mediaLink: 'https://example.com/image1.jpg'
                        },
                        {
                            id: 2,
                            messageId: 1,
                            mediaLink: 'https://example.com/image2.jpg'
                        }
                    ]
                },
                {
                    id: 12,
                    chatId: 1,
                    replyToMessageId: 1,
                    messageText: 'What are your plans for today?',
                    creatorId: 3,
                    createdIn: new Date('2022-01-01T08:30:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: []
                },
                {
                    id: 13,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'I\'m going to the gym and then to work.',
                    creatorId: 2,
                    createdIn: new Date('2022-01-01T08:45:00.000Z'),
                    isEdit: true,
                    editedIn: new Date('2022-01-01T08:50:00.000Z'),
                    attachedFiles: []
                },
                {
                    id: 14,
                    chatId: 1,
                    replyToMessageId: 3,
                    messageText: null,
                    creatorId: 3,
                    createdIn: new Date('2022-01-01T09:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: [
                        {
                            id: 1,
                            messageId: 4,
                            mediaLink: 'https://example.com/image1.jpg'
                        },
                        {
                            id: 2,
                            messageId: 4,
                            mediaLink: 'https://example.com/image2.jpg'
                        },
                        {
                            id: 3,
                            messageId: 4,
                            mediaLink: 'https://example.com/image3.jpg'
                        }
                    ]
                },
                {
                    id: 15,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'Have a great day!',
                    creatorId: 2,
                    createdIn: new Date('2022-01-01T17:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: null
                },
                {
                    id: 16,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'Good morning!',
                    creatorId: 2,
                    createdIn: new Date('2022-01-01T06:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: [
                        {
                            id: 1,
                            messageId: 1,
                            mediaLink: 'https://example.com/image1.jpg'
                        },
                        {
                            id: 2,
                            messageId: 1,
                            mediaLink: 'https://example.com/image2.jpg'
                        }
                    ]
                },
                {
                    id: 17,
                    chatId: 1,
                    replyToMessageId: 1,
                    messageText: 'What are your plans for today?',
                    creatorId: 3,
                    createdIn: new Date('2022-01-01T08:30:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: []
                },
                {
                    id: 18,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'I\'m going to the gym and then to work.',
                    creatorId: 2,
                    createdIn: new Date('2022-01-01T08:45:00.000Z'),
                    isEdit: true,
                    editedIn: new Date('2022-01-01T08:50:00.000Z'),
                    attachedFiles: []
                },
                {
                    id: 19,
                    chatId: 1,
                    replyToMessageId: 3,
                    messageText: null,
                    creatorId: 3,
                    createdIn: new Date('2022-01-01T09:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: [
                        {
                            id: 1,
                            messageId: 4,
                            mediaLink: 'https://example.com/image1.jpg'
                        },
                        {
                            id: 2,
                            messageId: 4,
                            mediaLink: 'https://example.com/image2.jpg'
                        },
                        {
                            id: 3,
                            messageId: 4,
                            mediaLink: 'https://example.com/image3.jpg'
                        }
                    ]
                },
                {
                    id: 20,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: 'Have a great day!',
                    creatorId: 2,
                    createdIn: new Date('2022-01-01T17:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: null
                }

            ]
            const course = this._rootStore.courseStore.coursesInfo
                .find(c => c.course.courseId === courseId);
            if (!course) {
                navigate('/');
            }
            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
            this.currentUser = course!.userInfo.courseuserId;

            this.isLoading = false;
            const newMessages: MessageModel[] = []
            for (let i = 0; i < 80; i++) {
                newMessages.push({
                    id: this.messages[this.messages.length - 1]!.id + i + 1,
                    chatId: 1,
                    replyToMessageId: null,
                    messageText: (this.messages[this.messages.length - 1]!.id + i + 1).toString(),
                    creatorId: 2,
                    createdIn: new Date('2022-01-01T17:00:00.000Z'),
                    isEdit: false,
                    editedIn: null,
                    attachedFiles: null
                });
            }

            this.messages = [...this.messages, ...newMessages]
            if (this.messages.length > 100) this.hasMore = false;

            this.createMessage = new CreateMessageModel(courseId, this.currentUser);

        });

    }

    async loadNextPack() {
        setTimeout(() => {
            runInAction(() => {
                const newMessages: MessageModel[] = []
                for (let i = 0; i < 100; i++) {
                    newMessages.push({
                        id: this.messages[this.messages.length - 1]!.id + i + 1,
                        chatId: 1,
                        replyToMessageId: null,
                        messageText: (this.messages[this.messages.length - 1]!.id + i + 1).toString(),
                        creatorId: 2,
                        createdIn: new Date('2022-01-01T17:00:00.000Z'),
                        isEdit: false,
                        editedIn: null,
                        attachedFiles: null
                    });
                }

                this.messages = [...this.messages, ...newMessages]
                if (this.messages.length > 1000) this.hasMore = false;
            })
        }, 500)

    }

    get isCreateMessageValid(): boolean {
        return this.createMessage!.validateMessage()
            && this.createMessage!.validateMessageText().length === 0
            && this.createMessage!.validateAttachedFiles().length === 0
    }

    get isEditMessageValid(): boolean {
        const message = this.messages.find(m => m.id === this.selectedMessage);
        return this.editMessage!.validateMessageText().length === 0
            && (
                (message!.attachedFiles!.length === 0 && this.editMessage!.messageText && this.editMessage!.messageText!.length > 0)
                || (message!.attachedFiles!.length > 0)
            );
    }

    get canDeleteFile(): boolean {
        const message = this.messages.find(m => m.id === this.selectedMessage);

        return message !== undefined && ((message.messageText !== null && message.messageText.length > 0) || (message.attachedFiles !== null && message.attachedFiles.length > 1))
    }

    async onFileClick(id: number, navigate: NavigateFunction){
        ///add behaviour
        runInAction(()=>{
            this.isFileViewerOpen = true;
            this.fileLink ='';////
        })
    }

    onFileViewerClose(){
        this.isFileViewerOpen = false;
        this.fileLink = '';
    }

    onCreateMessageTextChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.createMessage!.messageText = e.target.value;
        debounce(
            action(() => {
                const errors = this.createMessage!.validateMessageText();
                this.createMessageErrors.messageText = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onEditMessageTextChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.editMessage!.messageText = e.target.value;
        debounce(
            action(() => {
                const errors = this.editMessage!.validateMessageText();
                this.editMessageErrors.messageText = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    handeEditMessageOpen(courseId: number) {
        const message = this.messages.find(m => m.id === this.selectedMessage);
        const messagetext = message!.messageText === null ? '' : message!.messageText;
        this.editMessage = new UpdateMessageModel(this.selectedMessage!, courseId, this.currentUser!, messagetext)
        this.editMessageOpen = true;
        this.closeMessageMenu();
    }

    handleEditMessageClose() {
        this.editMessageOpen = false;
        this.resetEditMessage();
    }

    openMessageMenu(event: React.MouseEvent<HTMLDivElement>, messageId: number) {
        event.preventDefault();
        if (event.button === 2) {
            this.messageMenuAnchor = event.currentTarget;
            this.selectedMessage = messageId;
        }
    }

    closeMessageMenu() {
        this.messageMenuAnchor = null;
    }

    onCreateMessageFileAdd(e: React.ChangeEvent<HTMLInputElement>) {
        if (e.target.files) {
            this.createMessage!.attachedFiles.push(...Array.from(e.target.files));
        }
        debounce(
            action(() => {
                const errors = this.createMessage!.validateAttachedFiles();
                this.createMessageErrors.attachedFiles =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    onCreateMessageFileDelete(id: number) {
        this.createMessage?.attachedFiles.splice(id, 1);
        debounce(
            action(() => {
                const errors = this.createMessage!.validateAttachedFiles();
                this.createMessageErrors.attachedFiles =
                    errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    async onEditMessageFileAdd(e: React.ChangeEvent<HTMLInputElement>, navigate: NavigateFunction) {
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
            enqueueAlert(validator.errors[0].errorKey, 'error', validator.errors[0].options);
            return;
        }

        // try {
        //     const addedFile = await this._assignmentService.addFile(e.target.files[0], this.assignment!.id);
        //     runInAction(() => {
        //         this.assignment!.assignmentfiles!.push(addedFile);
        //         enqueueAlert('glossary.editSuccess', 'success');
        //     })
        // } catch (error) {
        //     if (error instanceof LoginRequiredError) {
        //         navigate('/login');
        //         enqueueAlert(error.message, 'error');
        //     } else {
        //         enqueueAlert((error as ServiceError).message, 'error');
        //     }
        // }
    }
    async onEditMessageFileDelete(fileId: number, navigate: NavigateFunction) {
        // try {
        //     await this._assignmentService.deleteFileById(fileId);
        //     runInAction(() => {
        //         const index = this.assignment!.assignmentfiles!.findIndex(f => f.id == fileId);
        //         this.assignment!.assignmentfiles!.splice(index, 1);
        //         enqueueAlert('glossary.deleteFileSuccess', 'success');
        //     })
        // } catch (error) {
        //     if (error instanceof LoginRequiredError) {
        //         navigate('/login');
        //         enqueueAlert(error.message, 'error');
        //     } else {
        //         enqueueAlert((error as ServiceError).message, 'error');
        //     }
        // }
    }

    sendMessage() {
        this.resetCreateMessage();
    }

    deleteMessage() {
        //
    }

    submitEdit() {
        this.resetEditMessage();
    }

    resetCreateMessage() {
        this.createMessage!.attachedFiles = [];
        this.createMessage!.messageText = '';
        this.createMessage!.createdIn = new Date(Date.now());

        Object.keys(this.createMessageErrors).forEach(
            (key) => (this.createMessageErrors[key] = null),
        );
    }

    resetEditMessage() {
        // const message = this.messages.find(m=>m.id === this.selectedMessage)
        // this.editMessage!.messageText = message!.messageText === null? '': message!.messageText;
        this.editMessage = null;
        this.selectedMessage = null;

        Object.keys(this.editMessageErrors).forEach(
            (key) => (this.editMessageErrors[key] = null),
        );
    }

    reset() {
        this.messages = [];
        this.isLoading = true;
        this.currentUser = null;
        this.hasMore = true;
        this.messageMenuAnchor = null;
        this.selectedMessage = null;

        this.editMessageOpen = false;
        this.createMessage = null;
        this.editMessage = null;

        Object.keys(this.createMessageErrors).forEach(
            (key) => (this.createMessageErrors[key] = null),
        );

        Object.keys(this.editMessageErrors).forEach(
            (key) => (this.editMessageErrors[key] = null),
        );
    }


}