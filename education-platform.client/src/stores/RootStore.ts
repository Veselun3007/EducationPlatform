import AssignmentService from '../services/AssignmentService';
import AuthService from '../services/AuthService';
import CourseService from '../services/CourseService';
import MaterialService from '../services/MaterialService';
import TopicService from '../services/TopicService';
import UserService from '../services/UserService';
import ConfirmUserPageStore from './ConfirmUserPageStore';
import CoursePageStore from './CoursePageStore';
import DashboardPageStore from './DashboardPageStore';
import LoginPageStore from './LoginPageStore';
import NavigationPanelStore from './NavigationPanelStore';
import SignUpPageStore from './SignUpPageStore';
import UserStore from './UserStore';

export default class RootStore {
    readonly authService: AuthService;
    readonly userService: UserService;
    readonly courseService: CourseService;
    readonly assignmentService: AssignmentService;
    readonly materialService: MaterialService;
    readonly topicService: TopicService;

    readonly signUpPageStore: SignUpPageStore;
    readonly loginPageStore: LoginPageStore;
    readonly confirmUserPageStore: ConfirmUserPageStore;
    readonly dashboardPageStore: DashboardPageStore;
    readonly userStore: UserStore;
    readonly navigationPanelStore: NavigationPanelStore;
    readonly coursePageStore: CoursePageStore;

    constructor() {
        //Service creation
        this.authService = new AuthService();
        this.userService = new UserService(this.authService);
        this.courseService = new CourseService(this.authService);
        this.assignmentService = new AssignmentService(this.authService);
        this.materialService = new MaterialService(this.authService);
        this.topicService = new TopicService(this.authService);
        //Store creation
        this.signUpPageStore = new SignUpPageStore(this, this.authService);
        this.loginPageStore = new LoginPageStore(this, this.authService);
        this.confirmUserPageStore = new ConfirmUserPageStore(this, this.authService);
        this.dashboardPageStore = new DashboardPageStore(this, this.authService);
        this.userStore = new UserStore(this, this.authService, this.userService);
        this.navigationPanelStore = new NavigationPanelStore(this, this.courseService);
        this.coursePageStore = new CoursePageStore(this);
    }
}
