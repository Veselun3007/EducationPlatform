import AuthService from './AuthService';

export default class CourseService {
    private readonly _authService: AuthService;

    constructor(authService: AuthService) {
        this._authService = authService;
    }
}
