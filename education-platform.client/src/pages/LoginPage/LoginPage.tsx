import {
    Avatar,
    Button,
    Divider,
    Grid,
    Paper,
    Stack,
    TextField,
    Typography,
    Link,
} from '@mui/material';
import React, { useEffect } from 'react';
import { Login } from '@mui/icons-material';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../context/RootStoreContext';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

const LoginPage = observer(() => {
    const { loginPageStore } = useStore();
    const { t } = useTranslation();
    const navigate = useNavigate();

    useEffect(() => () => loginPageStore.reset(), []);

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
                                {t('glossary.loginHeader')}
                            </Typography>
                            <Avatar sx={{ bgcolor: 'primary.main' }}>
                                <Login />
                            </Avatar>
                            <TextField
                                fullWidth
                                required
                                type="email"
                                label={t('common.email')}
                                value={loginPageStore.data.email}
                                onChange={loginPageStore.onEmailChange}
                                error={loginPageStore.errors.email !== null}
                                helperText={
                                    loginPageStore.errors.email !== null
                                        ? t(
                                            loginPageStore.errors.email.errorKey,
                                            loginPageStore.errors.email.options,
                                        )
                                        : null
                                }
                            />
                            <TextField
                                fullWidth
                                required
                                type="password"
                                label={t('common.password')}
                                value={loginPageStore.data.password}
                                onChange={loginPageStore.onPasswordChange}
                                error={loginPageStore.errors.password !== null}
                                helperText={
                                    loginPageStore.errors.password !== null
                                        ? t(
                                            loginPageStore.errors.password.errorKey,
                                            loginPageStore.errors.password.options,
                                        )
                                        : null
                                }
                            />
                            <Typography
                                color="error"
                                variant="caption"
                                align="center"
                                visibility={
                                    loginPageStore.errors.meta !== null
                                        ? 'visible'
                                        : 'collapse'
                                }
                            >
                                {loginPageStore.errors.meta !== null
                                    ? t(
                                        loginPageStore.errors.meta.errorKey,
                                        loginPageStore.errors.meta.options,
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
                                onClick={() => loginPageStore.submit(navigate)}
                                disabled={!loginPageStore.isValid}
                            >
                                {t('common.submit')}
                            </Button>
                            <Stack direction="row" spacing={1}>
                                <Typography variant="body2">
                                    {t('glossary.noAccount')}
                                </Typography>
                                <Link
                                    component="button"
                                    onClick={() => navigate('/signup')}
                                    variant="body2"
                                    underline="none"
                                >
                                    {t('common.signUp')}
                                </Link>
                                <Typography variant="body2">
                                    {t('glossary.forgotPassword')}
                                </Typography>
                                <Link
                                    component="button"
                                    onClick={() => navigate('/forgot-password')}
                                    variant="body2"
                                    underline="none"
                                >
                                    {t('common.forgotPassword')}
                                </Link>
                            </Stack>
                        </Stack>
                    </Paper>
                </Stack>
            </Grid>
        </Grid>
    );
});

export default LoginPage;
