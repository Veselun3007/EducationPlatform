import MediaMessage from './MediaMessage';

export default class CreateMessageModelTransport {
    public courseId: number;
    public messageText?: string;
    public creatorId: number;
    public createdIn: Date = new Date(Date.now());
    public attachedFiles: MediaMessage[];

    constructor(
        courseId: number,
        creatorId: number,
        attachedFiles: MediaMessage[],
        messageText?: string,
    ) {
        this.courseId = courseId;
        this.messageText = messageText;
        this.creatorId = creatorId;
        this.attachedFiles = attachedFiles;
    }
}
