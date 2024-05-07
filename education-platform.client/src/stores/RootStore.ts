import AssignmentService from '../services/AssignmentService';
import AuthService from '../services/AuthService';
import CourseService from '../services/CourseService';
import CourseUserService from '../services/CourseUserService';
import MaterialService from '../services/MaterialService';
import TopicService from '../services/TopicService';
import UserService from '../services/UserService';
import AssignmentPageStore from './AssignmentPageStore';
import ConfirmUserPageStore from './ConfirmUserPageStore';
import CoursePageStore from './CoursePageStore';
import LoginPageStore from './LoginPageStore';
import MaterialPageStore from './MaterialPageStore';
import CourseStore from './CourseStore';
import SignUpPageStore from './SignUpPageStore';
import UserStore from './UserStore';
import CommonService from '../services/common/CommonService';
import UsersPageStore from './UsersPageStore';
import ChatPageStore from './ChatPageStore';

export default class RootStore {
    readonly authService: AuthService;
    readonly userService: UserService;
    readonly courseService: CourseService;
    readonly assignmentService: AssignmentService;
    readonly materialService: MaterialService;
    readonly topicService: TopicService;
    readonly courseUserService: CourseUserService;
    readonly commonService: CommonService;

    readonly signUpPageStore: SignUpPageStore;
    readonly loginPageStore: LoginPageStore;
    readonly confirmUserPageStore: ConfirmUserPageStore;
    readonly userStore: UserStore;
    readonly courseStore: CourseStore;
    readonly coursePageStore: CoursePageStore;
    readonly assignmentPageStore: AssignmentPageStore;
    readonly materialPageStore: MaterialPageStore;
    readonly usersPageStore: UsersPageStore;
    readonly chatPageStore: ChatPageStore

    constructor() {
        //Service creation
        this.authService = new AuthService();
        this.userService = new UserService(this.authService);
        this.courseService = new CourseService(this.authService);
        this.courseUserService = new CourseUserService(this.authService);
        this.assignmentService = new AssignmentService(this.authService);
        this.materialService = new MaterialService(this.authService);
        this.topicService = new TopicService(this.authService);
        this.commonService = new CommonService();
        //Store creation
        this.signUpPageStore = new SignUpPageStore(this, this.authService);
        this.loginPageStore = new LoginPageStore(this, this.authService);
        this.confirmUserPageStore = new ConfirmUserPageStore(this, this.authService);
        this.userStore = new UserStore(this, this.authService, this.userService);
        this.courseStore = new CourseStore(this, this.courseService);
        this.coursePageStore = new CoursePageStore(this, this.courseService, this.assignmentService,this.materialService, this.topicService, this.courseUserService);
        this.assignmentPageStore = new AssignmentPageStore(this, this.assignmentService, this.topicService, this.commonService);
        this.materialPageStore = new MaterialPageStore(this, this.materialService, this.topicService, this.commonService);
        this.usersPageStore =new UsersPageStore(this, this.courseUserService);
        this.chatPageStore = new ChatPageStore(this);
    }
}
