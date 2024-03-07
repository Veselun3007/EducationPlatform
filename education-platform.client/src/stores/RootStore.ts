import AuthService from '../services/AuthService';
import UserService from '../services/UserService';
import ConfirmUserPageStore from './ConfirmUserPageStore';
import LoginPageStore from './LoginPageStore';
import SignUpPageStore from './SignUpPageStore';

export default class RootStore {
    readonly authService: AuthService;
    readonly userService: UserService;

    readonly signUpPageStore: SignUpPageStore;
    readonly loginPageStore: LoginPageStore;
    readonly confirmUserPageStore: ConfirmUserPageStore;

    constructor() {
        //Service creation
        this.authService = new AuthService();
        this.userService = new UserService(this.authService);

        //Store creation
        this.signUpPageStore = new SignUpPageStore(this, this.authService);
        this.loginPageStore = new LoginPageStore(this, this.authService);
        this.confirmUserPageStore = new ConfirmUserPageStore(this, this.authService);
    }
}
