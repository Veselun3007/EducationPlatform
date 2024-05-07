import { makeObservable, observable } from "mobx";
import FileValidator from "../../helpers/validation/FileValidator";
import StringValidator from "../../helpers/validation/StringValidator";
import ValidationError from "../../helpers/validation/ValidationError";

export default class UpdateMessageModel {
    public id: number;
    public courseId: number;
    public messageText?: string;
    public creatorId: number;

    constructor(id: number,courseId: number, creatorId: number, messageText = '') {
        this.id = id;
        this.courseId = courseId;
        this.messageText = messageText;
        this.creatorId = creatorId;

        makeObservable(this, {
            id:observable,
            courseId: observable,
            messageText: observable,
            creatorId: observable,
        });
    }


    validateMessageText(): ValidationError[] {
        const validator = new StringValidator(this.messageText);
        validator.smallerThan(450);

        return validator.errors;
    }
}