import React from 'react';
import { Outlet, Navigate } from 'react-router-dom';

const NotAuthRoutes = () => {
    const accessToken = localStorage.getItem('accessToken');
    if (accessToken) {
        return (<Navigate to="/dashboard" />);
    }
    return (
        <Outlet />
    );
}

export default NotAuthRoutes