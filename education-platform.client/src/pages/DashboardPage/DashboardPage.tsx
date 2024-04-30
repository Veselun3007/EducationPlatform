import React, { useEffect } from 'react';
import {
    Badge,
    Box,
    Button,
    Card,
    CardActionArea,
    CircularProgress,
    Menu,
    MenuItem,
    Stack,
    Typography,
} from '@mui/material';
import { observer } from 'mobx-react-lite';
import AbstractBackground from '../../components/AbstractBackground';
import { useStore } from '../../context/RootStoreContext';
import { useTranslation } from 'react-i18next';
import Grid from '@mui/material/Unstable_Grid2';
import { useNavigate } from 'react-router-dom';
import ColoredAvatar from '../../components/ColoredAvatar';

const DashboardPage = observer(() => {
    const { courseStore } = useStore();
    const { t } = useTranslation();
    const navigate = useNavigate();

    useEffect(() => {
        if(courseStore.needRefresh){
            courseStore.init(navigate);
        }
        return () => courseStore.setNeedRefresh();
    }, []);

    if (courseStore.isLoading) {
        return (
            <Box
                display="flex"
                height="100svh"
                width="100%"
                alignItems="center"
                justifyContent="center"
            >
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Grid container alignItems="center" justifyItems="center">
            {courseStore.coursesInfo.map((info) => (
                <Grid
                    key={info.course.courseId}
                    sx={{ borderRadius: 2 }}
                    marginInline={2}
                    marginBlock={2}
                    width="18.75rem"
                    height="18.375rem"
                    overflow="hidden"
                >
                    <Card sx={{ height: '100%' }}>
                        <CardActionArea
                            sx={{ height: '100%' }}
                            onClick={() => navigate(`/course/${info.course.courseId}`)}
                        >
                            <Stack direction="column" height="100%">
                                <Badge
                                    anchorOrigin={{
                                        vertical: 'bottom',
                                        horizontal: 'right',
                                    }}
                                    badgeContent={
                                        <ColoredAvatar
                                            alt={info.adminInfo.adminName}
                                            sx={{ width: 70, height: 70, right: 45 }}
                                            src={info.adminInfo.imageLink}
                                        />
                                    }
                                >
                                    <AbstractBackground
                                        value={info.course.courseName}
                                        sx={{ width: '100%', height: '7rem' }}
                                    >
                                        <Stack alignSelf="center" pl={2}>
                                            <Typography
                                                variant="h5"
                                                textAlign="left"
                                                alignSelf="start"

                                                sx={{
                                                    display: '-webkit-box',
                                                    overflow: 'hidden',
                                                    WebkitBoxOrient: 'vertical',
                                                    WebkitLineClamp: 1,
                                                    textOverflow: 'ellipsis',
                                                    whiteSpace: 'normal',
                                                }}
                                            >
                                                {info.course.courseName}
                                            </Typography>
                                            <Typography

                                                textAlign="left"
                                                justifyItems="start"
                                                sx={{
                                                    display: '-webkit-box',
                                                    overflow: 'hidden',
                                                    WebkitBoxOrient: 'vertical',
                                                    WebkitLineClamp: 1,
                                                    textOverflow: 'ellipsis',
                                                    whiteSpace: 'normal',
                                                }}
                                            >
                                                {info.adminInfo.adminName}
                                            </Typography>
                                        </Stack>
                                    </AbstractBackground>
                                </Badge>
                                {info.course.courseDescription &&
                                    <>
                                        <Typography mt={2} ml={2} variant="h6">
                                            {t('glossary.courseDescription')}
                                        </Typography>
                                        <Typography
                                            ml={2}
                                            mr={2}
                                            sx={{
                                                display: '-webkit-box',
                                                overflow: 'hidden',
                                                WebkitBoxOrient: 'vertical',
                                                WebkitLineClamp: 5,
                                                textOverflow: 'ellipsis',
                                                whiteSpace: 'normal',
                                            }}
                                        >
                                            {info.course.courseDescription}
                                        </Typography>
                                    </>
                                }

                            </Stack>
                        </CardActionArea>
                    </Card>
                </Grid>
            ))}
        </Grid>
    );
});

export default DashboardPage;
