import React, { useEffect } from 'react';
import { Badge, Card, CardActionArea, Stack, Typography } from '@mui/material';
import { observer } from 'mobx-react-lite';
import AbstractBackground from '../../components/AbstractBackground';
import { useStore } from '../../context/RootStoreContext';
import { useTranslation } from 'react-i18next';
import Grid from '@mui/material/Unstable_Grid2';
import { useNavigate } from 'react-router-dom';
import ColoredAvatar from '../../components/ColoredAvatar';

const DashboardPage = observer(() => {
    const { dashboardPageStore } = useStore();
    const { t } = useTranslation();
    const navigate = useNavigate();

    useEffect(() => {
        return () => dashboardPageStore.reset();
    }, []);

    return (
        <Grid container alignItems="center" justifyItems="center">
            {dashboardPageStore.courses.map((course) => (
                <Grid
                    key={course.courseId}
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
                            onClick={() => navigate(`/course/${course.courseId}`)}
                        >
                            <Stack direction="column" height="100%">
                                <Badge
                                    anchorOrigin={{
                                        vertical: 'bottom',
                                        horizontal: 'right',
                                    }}
                                    badgeContent={
                                        <ColoredAvatar
                                            alt="Bohdan Fedyshen"
                                            sx={{ width: 70, height: 70, right: 45 }}
                                            src="/assets/Sobolyev.jpg"
                                        />
                                    }
                                >
                                    <AbstractBackground
                                        value={course.courseName}
                                        sx={{ width:"100%",height:"7rem"}}

                                    >
                                        <Typography
                                            variant="h5"
                                            textAlign="left"
                                            ml={2}
                                            alignSelf="center"
                                            sx={{
                                                display: '-webkit-box',
                                                overflow: 'hidden',
                                                WebkitBoxOrient: 'vertical',
                                                WebkitLineClamp: 1,
                                                textOverflow: 'ellipsis',
                                                whiteSpace: 'normal',
                                            }}
                                        >
                                            {course.courseName}
                                        </Typography>
                                    </AbstractBackground>
                                </Badge>
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
                                    {course.courseDescription}
                                </Typography>
                            </Stack>
                        </CardActionArea>
                    </Card>
                </Grid>
            ))}
        </Grid>
    );
});

export default DashboardPage;
