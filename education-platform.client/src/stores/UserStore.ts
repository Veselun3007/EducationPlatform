import { action, makeObservable, observable } from "mobx";
import AuthService from "../services/AuthService";
import IStore from "./common/IStore";
import RootStore from "./RootStore";
import UserModel from "../models/user/UserModel";
import { enqueueAlert } from "../components/Notification/NotificationProvider";
import ServiceError from "../errors/ServiceError";
import { NavigateFunction } from "react-router-dom";


export default class UserStore implements IStore{
    private readonly _rootStore: RootStore;
    private readonly _authService: AuthService;

    user = new UserModel('renhach.valentyn@gmail.com','Valentyn Renhach', '/assets/Renhach.jpg')

    constructor(rootStore: RootStore, authService: AuthService) {
        this._rootStore = rootStore;
        this._authService = authService;

        makeObservable(this, {
            user: observable,
            reset: action.bound,
            signOut: action.bound,
        });
    }

    reset(): void {
        throw new Error("Method not implemented.");
    }

    signOut (navigate: NavigateFunction){
        try{
            //yield this._authService.signOut();
            navigate('/login');
            enqueueAlert('glossary.signOutSuccess', 'success');
        } 
        catch(error){
            enqueueAlert((error as ServiceError).message, 'error');
        }
    }


}