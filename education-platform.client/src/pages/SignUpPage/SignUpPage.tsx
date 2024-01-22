import { Login } from '@mui/icons-material';
import { Grid, Paper, Stack, Avatar, TextField, Input, Button, Typography } from '@mui/material';
import React from 'react';
import { useTranslation } from 'react-i18next';
import SignUpPageStore from './SignUpPageStore';
import AuthService from '../../services/AuthService';

const store = new SignUpPageStore(new AuthService());

const SignUpPage = () => {
    const {t} = useTranslation();
    return (
        <Grid container direction="row" justifyContent="center" alignItems="center" sx={{ minHeight: '100svh' }}>
            <Grid item xl={2} lg={4} md={6} sm={9} xs={11} >
                <Stack>
                    <Paper square={false} >
                        <Stack justifyContent="center" spacing={{ xs: 1, md: 2 }} p={3} alignItems="center">
                        <Typography variant="h2" fontSize={36} align="center">{t('glossary.signUpHeader')}</Typography>
                            <Avatar sx={{ bgcolor: 'primary.main' }}>
                                <Login />
                            </Avatar>
                            <TextField fullWidth type="email" label={t('common.email')} />
                            <TextField fullWidth label={t('common.username')}/>
                            <TextField fullWidth type="password" label={t('common.password')}/>
                            {/* <Stack direction="column" spacing={1}>
                                <Button variant="contained" component="label">
                                    {previewImageButtonText}
                                    <input
                                        type="file"
                                        hidden
                                        accept=".jpg, .jpeg, .png"
                                        onChange={onPreviewImageChange}
                                        data-testid="previewImageField"
                                    />
                                </Button>
                                <Typography
                                    color="error"
                                    variant="caption"
                                    data-testid="previewImageError"
                                    visibility={
                                        previewImageError !== '' ? 'visible' : 'collapse'
                                    }
                                >
                                    {previewImageError}
                                </Typography>
                            </Stack> */}
                            <Button variant="contained" color="success" fullWidth>Submit</Button>
                        </Stack>
                    </Paper>
                </Stack>
            </Grid>
        </Grid>
    );
};

export default SignUpPage;
