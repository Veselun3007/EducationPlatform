import MessageMediaModel from "./MessageMediaModel";

export default interface MessageModel {
    id: number;
    courseId: number;
    messageText: string | null;
    creatorId: number;
    createdIn: Date;
    isEdit: boolean | null;
    editedIn: Date | null;
    attachedFiles: MessageMediaModel[] | null;
}