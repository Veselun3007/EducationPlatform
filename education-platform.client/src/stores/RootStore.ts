import AuthService from '../services/AuthService';
import UserService from '../services/UserService';
import ConfirmUserPageStore from './ConfirmUserPageStore';
import DashboardPageStore from './DashboardPageStore';
import LoginPageStore from './LoginPageStore';
import SignUpPageStore from './SignUpPageStore';
import UserStore from './UserStore';

export default class RootStore {
    readonly authService: AuthService;
    readonly userService: UserService;

    readonly signUpPageStore: SignUpPageStore;
    readonly loginPageStore: LoginPageStore;
    readonly confirmUserPageStore: ConfirmUserPageStore;
    readonly dashboardPageStore: DashboardPageStore;
    readonly userStore: UserStore;

    constructor() {
        //Service creation
        this.authService = new AuthService();
        this.userService = new UserService(this.authService);

        //Store creation
        this.signUpPageStore = new SignUpPageStore(this, this.authService);
        this.loginPageStore = new LoginPageStore(this, this.authService);
        this.confirmUserPageStore = new ConfirmUserPageStore(this, this.authService);
        this.dashboardPageStore = new DashboardPageStore(this, this.authService);
        this.userStore = new UserStore(this, this.authService)
    }
}
