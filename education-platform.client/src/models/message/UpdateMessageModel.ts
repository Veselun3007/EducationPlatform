import { makeObservable, observable } from 'mobx';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class UpdateMessageModel {
    public id: number;
    public courseId: number;
    public messageText?: string;
    public creatorId: number;
    public createdIn: Date;

    constructor(
        id: number,
        courseId: number,
        creatorId: number,
        createdIn: Date,
        messageText = '',
    ) {
        this.id = id;
        this.courseId = courseId;
        this.messageText = messageText;
        this.creatorId = creatorId;
        this.createdIn = createdIn;

        makeObservable(this, {
            createdIn: observable,
            id: observable,
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
