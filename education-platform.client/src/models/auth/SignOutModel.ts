export default class SignOutModel {
    accessToken: string | null;

    constructor(accessToken: string | null) {
        this.accessToken = accessToken;
    }
}
