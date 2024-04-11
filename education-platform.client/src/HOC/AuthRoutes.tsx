import React from 'react';
import { Outlet, Navigate } from 'react-router-dom';
import { enqueueAlert } from '../components/Notification/NotificationProvider';
import NavigationPanel from '../components/NavigationPanel';

const AuthRoutes = () => {
    const accessToken = localStorage.getItem('accessToken');
    if (!accessToken) {
        enqueueAlert('glossary.loginToContinue', 'warning');
        return <Navigate to="/login" />;
    }
    return (
        <NavigationPanel>
            <Outlet />
        </NavigationPanel>
    );
};

export default AuthRoutes;
