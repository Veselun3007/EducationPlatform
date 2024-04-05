import { makeObservable, observable } from 'mobx';

export default class UserModel {
    public email: string;
    public userName: string;
    public userImage?: string;

    constructor(email: string, userName: string, userImage?: string) {
        makeObservable(this, {
            email: observable,
            userName: observable,
            userImage: observable,
        });

        this.email = email;
        this.userName = userName;
        this.userImage = userImage;
    }
}
