import React from 'react';
import { observer } from 'mobx-react-lite';
import NotificationProviderStore from './NotificationProviderStore';
import { Alert, AlertColor, Snackbar } from '@mui/material';

interface NotificationProviderProps {
    autoHideDuration?: number;
}

const store = new NotificationProviderStore();

const NotificationProvider: React.FC<NotificationProviderProps> = observer(
    ({ autoHideDuration = 3000 }) => {
        const { text, variant, isOpen, dequeueAlert } = store;
        if (variant == 'default') {
            return (
                <Snackbar
                    open={isOpen}
                    autoHideDuration={autoHideDuration}
                    message={text}
                    onClose={dequeueAlert}
                />
            );
        } else {
            return (
                <Snackbar
                    open={isOpen}
                    autoHideDuration={autoHideDuration}
                    onClose={dequeueAlert}
                >
                    <Alert severity={variant as AlertColor} sx={{ width: '100%' }}>
                        {text}
                    </Alert>
                </Snackbar>
            );
        }
    },
);

export default NotificationProvider;
export const enqueueAlert = store.enqueueAlert;
