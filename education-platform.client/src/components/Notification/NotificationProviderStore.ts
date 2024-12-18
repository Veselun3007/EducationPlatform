import { action, makeObservable, observable } from 'mobx';

type NotificationVariants = 'default' | 'error' | 'success' | 'warning' | 'info';

export default class NotificationProviderStore {
    isOpen = false;
    key = '';
    variant = 'default';
    options?: Record<string, unknown> = undefined;

    constructor() {
        makeObservable(this, {
            isOpen: observable,
            key: observable,
            variant: observable,
            enqueueAlert: action.bound,
            dequeueAlert: action.bound,
        });
    }

    enqueueAlert(
        text: string,
        variant: NotificationVariants = 'default',
        options?: Record<string, unknown>,
    ) {
        this.isOpen = true;
        this.key = text;
        this.variant = variant;
        this.options = options;
    }

    dequeueAlert(event?: React.SyntheticEvent | Event, reason?: string) {
        if (reason === 'clickaway') {
            return;
        }
        this.isOpen = false;
        this.key = '';
        this.variant = 'default';
    }
}
