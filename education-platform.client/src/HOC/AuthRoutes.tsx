import React from 'react';
import { Outlet, Navigate } from 'react-router-dom';
import { enqueueAlert } from '../components/Notification/NotificationProvider';

const AuthRoutes = () => {
    const accessToken = localStorage.getItem('accessToken');
    if (!accessToken) {
        enqueueAlert('glossary.loginToContinue', 'warning');
        return (<Navigate to="/login" />);
    }
    return (
        <Outlet />
    );
}

export default AuthRoutes