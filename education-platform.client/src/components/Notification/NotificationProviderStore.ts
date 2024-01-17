import { action, makeObservable, observable } from 'mobx';

type NotificationVariants = 'default' | 'error' | 'success' | 'warning' | 'info';

export default class NotificationProviderStore {
    isOpen = false;
    text = '';
    variant = 'default';

    constructor() {
        makeObservable(this, {
            isOpen: observable,
            text: observable,
            variant: observable,
            enqueueAlert: action.bound,
            dequeueAlert: action.bound,
        });
    }

    enqueueAlert(text: string, variant: NotificationVariants = 'default') {
        this.isOpen = true;
        this.text = text;
        this.variant = variant;
    }

    dequeueAlert(event?: React.SyntheticEvent | Event, reason?: string) {
        if (reason === 'clickaway') {
            return;
        }
        this.isOpen = false;
        this.text = '';
        this.variant = 'default';
    }
}
