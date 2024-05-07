import React, { useEffect, useRef, useState } from 'react';
import {
    Box,
    CircularProgress,
} from '@mui/material';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../context/RootStoreContext';
import { useNavigate, useParams } from 'react-router-dom';
import LoginRequiredError from '../../errors/LoginRequiredError';
import { enqueueAlert } from '../../components/Notification/NotificationProvider';
import ServiceError from '../../errors/ServiceError';
import { runInAction } from 'mobx';

const JoinCoursePage = observer(() => {
    const { courseUserService, courseStore } = useStore();
    const { courseId, courseLink } = useParams();
    const navigate = useNavigate();


    if (isNaN(Number(courseId)) || !courseLink) {
        navigate('/404');
    }

    useEffect(() => {
        courseUserService.createUser(courseLink!).then(() => {
            navigate(`/dashboard`);
            courseStore.setNeedRefresh();
            enqueueAlert('glossary.joinCourseSuccess', 'success');
            
        }).catch((error) => {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                navigate('/');
                enqueueAlert((error as ServiceError).message, 'error');
            }
        });
    }, [])
    return null;
});

export default JoinCoursePage;
