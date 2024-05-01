import React, { useEffect } from 'react';
import {
    Badge,
    Box,
    Button,
    Card,
    CardActionArea,
    CircularProgress,
    FormControl,
    IconButton,
    InputLabel,
    Menu,
    MenuItem,
    Select,
    Stack,
    Typography,
    useTheme,
} from '@mui/material';
import { observer } from 'mobx-react-lite';
import AbstractBackground from '../../components/AbstractBackground';
import { useStore } from '../../context/RootStoreContext';
import { useTranslation } from 'react-i18next';
import Grid from '@mui/material/Unstable_Grid2';
import { useNavigate, useParams } from 'react-router-dom';
import ColoredAvatar from '../../components/ColoredAvatar';
import { Delete, LibraryAdd } from '@mui/icons-material';

const UsersPage = observer(() => {
    const { usersPageStore } = useStore();
    const { t } = useTranslation();
    const navigate = useNavigate();
    const theme = useTheme();
    const { courseId } = useParams();

    if (isNaN(Number(courseId))) {
        navigate('/404');
    }

    useEffect(() => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        usersPageStore.init(Number.parseInt(courseId!), navigate);
        return () => usersPageStore.reset();
    }, []);

    if (usersPageStore.isLoading) {
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

    const admins = usersPageStore.users.filter(u => u.role === 0);
    const teachers = usersPageStore.users.filter(u => u.role === 1);
    const students = usersPageStore.users.filter(u => u.role === 2);

    const emptyUser: React.ReactNode = (
        <Typography color="InactiveCaptionText" textAlign="center">
            {t('glossary.userEmpty')}
        </Typography>
    );

    return (
        <Grid container mt={2} mb={2}>
            <Grid xs />
            <Grid xs={12} md={10} xl={8}>
                <Stack spacing={2}>
                    <Box
                        borderBottom={2}
                        borderColor={theme.palette.primary.light}
                        p={1}
                    >
                        <Typography variant="h5" color={theme.palette.primary.light}>
                            {t('glossary.admins')}
                        </Typography>
                    </Box>

                    {
                        admins.map((user, index) => (
                            <Stack direction="row" p={2} key={user.courseuserId} spacing={2}
                                borderBottom={index === admins.length - 1 ? undefined : 1}
                                borderColor="InactiveCaptionText">
                                <ColoredAvatar
                                    src={user.userImage}
                                    alt={user.userName}
                                />
                                <Stack>
                                    <Typography>
                                        {user.userName}
                                    </Typography>
                                    <Typography color="InactiveCaptionText" variant='caption'>
                                        {user.userEmail}
                                    </Typography>
                                </Stack>
                            </Stack>
                        ))
                    }

                    {
                        (admins.length === 0) && emptyUser
                    }

                    <Box
                        borderBottom={2}
                        borderColor={theme.palette.primary.light}
                        p={1}
                    >
                        <Typography variant="h5" color={theme.palette.primary.light}>
                            {t('glossary.teachers')}
                        </Typography>
                    </Box>
                    {
                        teachers.map((user, index) => (
                            <Stack direction="row" p={2} key={user.courseuserId} justifyContent="space-between"
                                borderBottom={index === teachers.length - 1 ? undefined : 1}
                                borderColor="InactiveCaptionText">
                                <Stack direction="row" spacing={2}>
                                    <ColoredAvatar
                                        src={user.userImage}
                                        alt={user.userName}
                                    />
                                    <Stack>
                                        <Typography>
                                            {user.userName}
                                        </Typography>
                                        <Typography color="InactiveCaptionText" variant='caption'>
                                            {user.userEmail}
                                        </Typography>
                                    </Stack>
                                </Stack>
                                {usersPageStore.isAdmin && <Stack direction="row" spacing={1} alignItems="center" justifyItems="center">
                                    <FormControl fullWidth>
                                        <InputLabel id="roleSelect">
                                            {t('common.role')}
                                        </InputLabel>
                                        <Select
                                            labelId="roleSelect"
                                            value={
                                                user.role
                                            }
                                            label={t('common.topic')}
                                            onChange={(e) => usersPageStore.onUserRoleChange(e, navigate, user.courseuserId)}
                                            fullWidth
                                        >
                                            <MenuItem value={0}>{t('glossary.admin')}</MenuItem>
                                            <MenuItem value={1}>{t('glossary.teacher')}</MenuItem>
                                            <MenuItem value={2}>{t('glossary.student')}</MenuItem>
                                        </Select>
                                    </FormControl>
                                    <IconButton sx={{ height: 'fit-content' }} onClick={() => usersPageStore.kickUser(user.courseuserId, navigate)}>
                                        <Delete />
                                    </IconButton>
                                </Stack>}
                            </Stack>
                        ))
                    }
                    {
                        (teachers.length === 0) && emptyUser
                    }

                    <Box
                        borderBottom={2}
                        borderColor={theme.palette.primary.light}
                        p={1}
                    >
                        <Typography variant="h5" color={theme.palette.primary.light}>
                            {t('glossary.students')}
                        </Typography>
                    </Box>
                    {
                        students.map((user, index) => (
                            <Stack direction="row" p={2} key={user.courseuserId}
                                borderBottom={index === students.length - 1 ? undefined : 1}
                                borderColor="InactiveCaptionText"
                                justifyContent="space-between"
                            >
                                <Stack direction="row" spacing={2}>
                                    <ColoredAvatar
                                        src={user.userImage}
                                        alt={user.userName}
                                    />
                                    <Stack>
                                        <Typography>
                                            {user.userName}
                                        </Typography>
                                        <Typography color="InactiveCaptionText" variant='caption'>
                                            {user.userEmail}
                                        </Typography>
                                    </Stack>
                                </Stack>
                                {usersPageStore.isAdmin &&<Stack direction="row" spacing={1} alignItems="center" justifyItems="center">
                                    <FormControl fullWidth>
                                        <InputLabel id="roleSelect">
                                            {t('common.role')}
                                        </InputLabel>
                                        <Select
                                            labelId="roleSelect"
                                            value={
                                                user.role
                                            }
                                            label={t('common.topic')}
                                            onChange={(e) => usersPageStore.onUserRoleChange(e, navigate, user.courseuserId)}
                                            fullWidth
                                        >
                                            <MenuItem value={0}>{t('glossary.admin')}</MenuItem>
                                            <MenuItem value={1}>{t('glossary.teacher')}</MenuItem>
                                            <MenuItem value={2}>{t('glossary.student')}</MenuItem>
                                        </Select>
                                    </FormControl>
                                    <IconButton sx={{ height: 'fit-content' }} onClick={() => usersPageStore.kickUser(user.courseuserId, navigate)}>
                                        <Delete />
                                    </IconButton>
                                </Stack>}
                            </Stack>
                        ))
                    }
                    {
                        (students.length === 0) && emptyUser
                    }
                </Stack>
            </Grid>
            <Grid xs />
        </Grid>


    );
});

export default UsersPage;
