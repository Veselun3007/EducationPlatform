import { makeObservable, observable, action } from "mobx";
import AuthService from "../../services/AuthService";
import { error } from "console";

export default class SignUpPageStore {

    private _authService: AuthService;
    form = {
        fields:{
            email:{
                value:'',
                error:null,
            },
        }
    }

    constructor(authService: AuthService) {
        this._authService = authService
        makeObservable(this, {
            c1:action.bound,
            c2:action.bound
        });
    }

    c1(){
        this.form.fields.email.value = 'few';
    }
    c2(){
        console.log(this.form.fields.email.value);
    }
}