import { AttachFile, Login } from '@mui/icons-material';
import {
    Grid,
    Paper,
    Stack,
    Avatar,
    TextField,
    Button,
    Typography,
    Divider,
    Link,
} from '@mui/material';
import React, { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useStore } from '../../context/RootStoreContext';
import { observer } from 'mobx-react-lite';
import { useNavigate } from 'react-router-dom';

const SignUpPage = observer(() => {
    const { signUpPageStore } = useStore();
    const { t } = useTranslation();
    const navigate = useNavigate();

    const userImageButtonText = signUpPageStore.data.userImage
        ? signUpPageStore.data.userImage.name
        : t('common.userImage');

    useEffect(() => () => signUpPageStore.reset(), []);

    return (
        <Grid
            container
            direction="row"
            justifyContent="center"
            alignItems="center"
            sx={{ minHeight: '100svh' }}
        >
            <Grid item xl={2} lg={4} md={6} sm={9} xs={11}>
                <Stack>
                    <Paper square={false} variant="elevation">
                        <Stack
                            justifyContent="center"
                            spacing={{ xs: 1, md: 2 }}
                            p={3}
                            alignItems="center"
                        >
                            <Typography variant="h2" fontSize={36} align="center">
                                {t('glossary.signUpHeader')}
                            </Typography>
                            <Avatar sx={{ bgcolor: 'primary.main' }}>
                                <Login />
                            </Avatar>
                            <TextField
                                fullWidth
                                required
                                type="email"
                                label={t('common.email')}
                                value={signUpPageStore.data.email}
                                onChange={signUpPageStore.onEmailChange}
                                error={signUpPageStore.errors.email !== null}
                                helperText={
                                    signUpPageStore.errors.email !== null
                                        ? t(
                                              signUpPageStore.errors.email.errorKey,
                                              signUpPageStore.errors.email.options,
                                          )
                                        : null
                                }
                            />
                            <TextField
                                fullWidth
                                required
                                label={t('common.username')}
                                value={signUpPageStore.data.userName}
                                onChange={signUpPageStore.onUserNameChange}
                                error={signUpPageStore.errors.userName !== null}
                                helperText={
                                    signUpPageStore.errors.userName !== null
                                        ? t(
                                              signUpPageStore.errors.userName.errorKey,
                                              signUpPageStore.errors.userName.options,
                                          )
                                        : null
                                }
                            />
                            <TextField
                                fullWidth
                                required
                                type="password"
                                label={t('common.password')}
                                value={signUpPageStore.data.password}
                                onChange={signUpPageStore.onPasswordChange}
                                error={signUpPageStore.errors.password !== null}
                                helperText={
                                    signUpPageStore.errors.password !== null
                                        ? t(
                                              signUpPageStore.errors.password.errorKey,
                                              signUpPageStore.errors.password.options,
                                          )
                                        : null
                                }
                            />
                            <Stack direction="column" spacing={1} width="100%">
                                <Button
                                    variant="contained"
                                    component="label"
                                    startIcon={<AttachFile />}
                                    fullWidth
                                >
                                    <Typography
                                        variant="button"
                                        textOverflow="ellipsis"
                                        whiteSpace="nowrap"
                                        overflow="hidden"
                                    >
                                        {userImageButtonText}
                                    </Typography>
                                    <input
                                        type="file"
                                        hidden
                                        accept=".jpg, .jpeg, .png"
                                        onChange={signUpPageStore.onUserImageChange}
                                    />
                                </Button>
                                <Typography
                                    color="error"
                                    variant="caption"
                                    visibility={
                                        signUpPageStore.errors.userImage !== null
                                            ? 'visible'
                                            : 'collapse'
                                    }
                                >
                                    {signUpPageStore.errors.userImage !== null
                                        ? t(
                                              signUpPageStore.errors.userImage.errorKey,
                                              signUpPageStore.errors.userImage.options,
                                          )
                                        : null}
                                </Typography>
                            </Stack>
                            <Typography
                                color="error"
                                variant="caption"
                                align="center"
                                visibility={
                                    signUpPageStore.errors.meta !== null
                                        ? 'visible'
                                        : 'collapse'
                                }
                            >
                                {signUpPageStore.errors.meta !== null
                                    ? t(
                                          signUpPageStore.errors.meta.errorKey,
                                          signUpPageStore.errors.meta.options,
                                      )
                                    : null}
                            </Typography>
                            <Divider
                                orientation="horizontal"
                                variant="fullWidth"
                                style={{ width: '100%' }}
                            />
                            <Button
                                variant="contained"
                                color="success"
                                fullWidth
                                onClick={() => signUpPageStore.submit(navigate)}
                                disabled={!signUpPageStore.isValid}
                            >
                                {t('common.submit')}
                            </Button>
                            <Stack direction="row" spacing={1}>
                                <Typography variant="body2">
                                    {t('glossary.haveAccount')}
                                </Typography>
                                <Link
                                    component="button"
                                    onClick={() => navigate('/login')}
                                    variant="body2"
                                    underline="none"
                                >
                                    {t('common.login')}
                                </Link>
                            </Stack>
                        </Stack>
                    </Paper>
                </Stack>
            </Grid>
        </Grid>
    );
});

export default SignUpPage;
