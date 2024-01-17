import { Button } from '@mui/material';
import React from 'react';
import { enqueueAlert } from '../../components/Notification/NotificationProvider';

const LoginPage = () => {
    return (
        <>
            <Button onClick={() => enqueueAlert('hui v dupe')}>Click</Button>
            <Button onClick={() => enqueueAlert('hui v dupe', 'success')}>Click</Button>
        </>
    );
};

export default LoginPage;
