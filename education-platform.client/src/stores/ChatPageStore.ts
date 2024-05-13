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
import CourseUserInfoModel from "../models/courseUser/CourseUserInfoModel";
import CourseUserService from "../services/CourseUserService";
import * as signalR from "@microsoft/signalr";
import { HUB_URL } from "../services/common/routesAPI";
import LoginRequiredError from "../errors/LoginRequiredError";
import MediaMessage from "../models/message/MediaMessage";
import CreateMessageModelTransport from "../models/message/CreateMessageModelTransport";
import { fileToBase64 } from "../helpers/fileToBase64";
import MessageMediaModel from "../models/message/MessageMediaModel";



export default class ChatPageStore {
    private readonly _rootStore: RootStore;
    private readonly _courseUserService: CourseUserService;
    private _hubConnection: signalR.HubConnection | null = null;

    courseId: number | null = null;
    messages: MessageModel[] = [];
    users: CourseUserInfoModel[] = [];
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
    fileLink = '';


    constructor(rootStore: RootStore, courseUserService: CourseUserService) {

        this._rootStore = rootStore;
        this._courseUserService = courseUserService;

        makeObservable(this, {
            courseId: observable,
            users: observable,
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

    async init(courseId: number, navigate: NavigateFunction) {
        try {
            this._hubConnection = new signalR.HubConnectionBuilder().withUrl(HUB_URL, { withCredentials: false })
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.Critical)
                .build();

            await this._hubConnection.start();

            await this._hubConnection.send('JoinRoom', courseId);

            const users = await this._courseUserService.getUsers(courseId);

            await this._hubConnection.send("GetFirstPackMessage", courseId);

            this._hubConnection.onreconnected(async ()=>{
                await this._hubConnection!.send('JoinRoom', courseId);
            });

            this._hubConnection.on("ReceiveMessages", (response) => {
                if (Object.hasOwn(response, 'statusCode')) {
                    enqueueAlert('glossary.somethingWentWrong', 'error');
                } else {
                    runInAction(() => {
                        const messages = (response as MessageModel[])
                        this.messages = this.messages.concat(messages).sort((a, b) => b.id - a.id);
                        if (messages.length < 100) this.hasMore = false;
                    });
                }
            });

            this._hubConnection.on('ReceiveMessage', (response) => {
                if (Object.hasOwn(response, 'statusCode')) {
                    enqueueAlert('glossary.somethingWentWrong', 'error');
                } else {
                    runInAction(() => {
                        this.messages = this.messages.concat(response as MessageModel).sort((a, b) => b.id - a.id);
                    });
                }

            });

            this._hubConnection.on("BroadCastDeleteMessage", (response) => {
                if (Object.hasOwn(response, 'statusCode')) {
                    switch (response.statusCode as number) {
                        case 404:
                            enqueueAlert('glossary.messageNotFound', 'error');
                            break;
                        default:
                            enqueueAlert('glossary.somethingWentWrong', 'error');
                    }
                } else {
                    runInAction(() => {
                        const id = Number.parseInt(response as string);
                        if (this.messages.some(m => m.id === id)) {
                            const index = this.messages.findIndex(m => m.id === id);
                            this.messages.splice(index, 1);
                        }
                    });
                }
            });

            this._hubConnection.on('EditMedia', (response) => {
                if (Object.hasOwn(response, 'statusCode')) {
                    switch (response.statusCode as number) {
                        case 404:
                            enqueueAlert('glossary.messageNotFound', 'error');
                            break;
                        default:
                            enqueueAlert('glossary.somethingWentWrong', 'error');
                    }
                } else {
                    runInAction(() => {
                        const message = response as MessageModel;
                        if (this.messages.some(m => m.id === message.id)) {
                            const index = this.messages.findIndex(m => m.id === message.id);
                            this.messages[index] = message;
                        }
                    });
                }
            });

            this._hubConnection.on('DeleteMedia', (response) => {
                if (Object.hasOwn(response, 'statusCode')) {
                    switch (response.statusCode as number) {
                        case 404:
                            enqueueAlert('glossary.fileNotFound', 'error');
                            break;
                        default:
                            enqueueAlert('glossary.somethingWentWrong', 'error');
                    }
                } else {
                    runInAction(() => {
                        const fileId = Number.parseInt(response);
                        const message = this.messages.find(message => message.attachedFiles?.some(file => file.id === fileId));
                        if (message) {
                            const attachments = message.attachedFiles;
                            const index = attachments?.findIndex(file => file.id === fileId);
                            if (index !== -1) {
                                attachments!.splice(index!, 1);
                            }
                        }
                    });
                }
            });

            this._hubConnection.on('AddMedia', (response) => {
                if (Object.hasOwn(response, 'statusCode')) {
                    switch (response.statusCode as number) {
                        case 404:
                            enqueueAlert('glossary.messageNotFound', 'error');
                            break;
                        default:
                            enqueueAlert('glossary.somethingWentWrong', 'error');
                    }
                } else {
                    runInAction(() => {
                        const messageMedia = response as MessageMediaModel;
                        const message = this.messages.find(m => m.id === messageMedia.messageId);
                        if (message) {
                            message.attachedFiles?.push(messageMedia);
                        }
                    });
                }
            });

            this._hubConnection.on('GetFile', (response) => {
                if (Object.hasOwn(response, 'statusCode')) {
                    switch (response.statusCode as number) {
                        case 404:
                            enqueueAlert('glossary."fileNotFound', 'error');
                            break;
                        default:
                            enqueueAlert('glossary.somethingWentWrong', 'error');
                    }
                } else {
                    runInAction(() => {
                        this.fileLink = response as string;
                        this.isFileViewerOpen = true;
                    });
                }
            });

            runInAction(() => {
                this.users = users;
                this.courseId = courseId;

                const course = this._rootStore.courseStore.coursesInfo
                    .find(c => c.course.courseId === courseId);
                if (!course) {
                    navigate('/');
                }
                this.currentUser = course!.userInfo.courseuserId;

                this.isLoading = false;

                this.createMessage = new CreateMessageModel(courseId, this.currentUser);

            });
        } catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            }
            else {
                enqueueAlert('glossary.somethingWentWrong', 'error');
                navigate(`/course/${courseId}`)
            }
        }
    }

    async loadNextPack() {
        try {
            const lastId = this.messages.at(-1)!.id;
            await this._hubConnection!.send('GetNextPackMessage', this.courseId, lastId);
        } catch (error) {
            enqueueAlert('glossary.somethingWentWrong', 'error')
        }
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

    async onFileClick(id: number) {
        try {
            await this._hubConnection!.send('GetFileById', id);
        } catch (error) {
            enqueueAlert('glossary.somethingWentWrong', 'error');
        }
        
    }

    onFileViewerClose() {
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
                if(!this.editMessage) return;
                const errors = this.editMessage!.validateMessageText();
                this.editMessageErrors.messageText = errors.length !== 0 ? errors[0] : null;
            }),
        )();
    }

    handeEditMessageOpen() {
        const message = this.messages.find(m => m.id === this.selectedMessage);
        const messagetext = message!.messageText === null ? '' : message!.messageText;
        this.editMessage = new UpdateMessageModel(this.selectedMessage!, this.courseId!, this.currentUser!, message!.createdIn, messagetext);
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

    async onEditMessageFileAdd(e: React.ChangeEvent<HTMLInputElement>) {
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

        try {
            const base64File = await fileToBase64(e.target.files[0]);
            const media = new MediaMessage(base64File, e.target.files[0].name)
            await this._hubConnection!.send('AddMessageMedia', this.courseId, media, this.selectedMessage);
        } catch (error) {
            enqueueAlert('glossary.somethingWentWrong', 'error');
        }
    }

    async onEditMessageFileDelete(fileId: number) {
        try {
            await this._hubConnection!.send('DeleteMessageMedia', this.courseId, fileId);

        } catch (error) {
            enqueueAlert('glossary.somethingWentWrong', 'error');
        }
    }

    async sendMessage() {
        try {
            const mediaMessages: MediaMessage[] = [];

            for (const file of this.createMessage!.attachedFiles) {
                const byteFile = await fileToBase64(file);
                mediaMessages.push(new MediaMessage(byteFile, file.name))
            }

            const message = new CreateMessageModelTransport(this.createMessage!.courseId, this.createMessage!.creatorId, mediaMessages, this.createMessage!.messageText)
            await this._hubConnection?.send('SendMessage', message);

            runInAction(() => {
                this.resetCreateMessage();
            });
        } catch (error) {
            enqueueAlert('glossary.somethingWentWrong', 'error');
        }
    }

    async deleteMessage() {
        try {
            await this._hubConnection?.send('DeleteMessage', this.courseId, this.selectedMessage, 1)
            runInAction(() => {
                this.selectedMessage = null;
                this.closeMessageMenu();
            })
        }
        catch (error) {
            enqueueAlert('glossary.somethingWentWrong', 'error');
        }
    }

    async submitEdit() {
        try {
            await this._hubConnection?.send('EditMessage', this.courseId, this.editMessage);
            runInAction(() => {
                this.handleEditMessageClose();
            })
        } catch (error) {
            enqueueAlert('glossary.somethingWentWrong', 'error');
        }
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
        this.editMessage = null;
        this.selectedMessage = null;

        Object.keys(this.editMessageErrors).forEach(
            (key) => (this.editMessageErrors[key] = null),
        );
    }

    reset() {
        this._hubConnection?.stop();
        this.courseId = null;
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