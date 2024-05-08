export default class MediaMessage{
    public fileBase64: string;
    public fileName: string;

    constructor(fileBase64:string, fileName: string) { 
        this.fileBase64 = fileBase64;
        this.fileName = fileName;
    }
}