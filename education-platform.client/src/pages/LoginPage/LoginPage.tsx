import { Avatar, Button, Card, Container, Paper, Stack, TextField } from '@mui/material';
import React from 'react';
import { enqueueAlert } from '../../components/Notification/NotificationProvider';
import Grid from '@mui/material/Unstable_Grid2'
import { Login } from '@mui/icons-material';

const LoginPage = () => {
    return (
        <Grid container direction="column" justifyContent="center" alignItems="center" sx={{ minHeight: '100svh' }}>
            <Grid lg={2} md={4} sm={6} xs={8}>
                <Paper square={false} >
                    <Stack justifyContent="center" spacing={{ xs: 1, md: 2 }} p={3} alignItems="center">
                        <Avatar sx={{ bgcolor: 'primary.main' }}>
                            <Login/>
                        </Avatar>
                        <TextField fullWidth type="email" label="da" />
                        <TextField fullWidth/>
                        <TextField fullWidth type="password"/>
                        <TextField fullWidth type="file"/>
                        <input type="file" />
                    </Stack>
                </Paper>
            </Grid>
        </Grid>
    );
};

export default LoginPage;
