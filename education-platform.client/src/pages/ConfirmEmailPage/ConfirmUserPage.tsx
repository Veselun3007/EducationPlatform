import {
    Avatar,
    Button,
    Divider,
    Grid,
    Paper,
    Stack,
    TextField,
    Typography,
} from '@mui/material';
import React, { useEffect } from 'react';
import { Email } from '@mui/icons-material';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../context/RootStoreContext';
import { useTranslation } from 'react-i18next';
import { useNavigate, useParams } from 'react-router-dom';

const ConfirmEmailPage = observer(() => {
    const { confirmUserPageStore } = useStore();
    const { t } = useTranslation();
    const navigate = useNavigate();
    const { email } = useParams();

    useEffect(() => () => confirmUserPageStore.reset(), []);

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
                                {t('glossary.confirmUserHeader')}
                            </Typography>
                            <Avatar sx={{ bgcolor: 'primary.main' }}>
                                <Email />
                            </Avatar>
                            <Typography variant="body2">
                                {t('glossary.confirmUser')}
                            </Typography>
                            <TextField
                                fullWidth
                                required
                                type="text"
                                label={t('common.code')}
                                value={confirmUserPageStore.data.confirmCode}
                                onChange={confirmUserPageStore.onCodeChange}
                                error={confirmUserPageStore.errors.code !== null}
                                helperText={
                                    confirmUserPageStore.errors.code !== null
                                        ? t(
                                              confirmUserPageStore.errors.code.errorKey,
                                              confirmUserPageStore.errors.code.options,
                                          )
                                        : null
                                }
                            />
                            <Typography
                                color="error"
                                variant="caption"
                                align="center"
                                visibility={
                                    confirmUserPageStore.errors.meta !== null
                                        ? 'visible'
                                        : 'collapse'
                                }
                            >
                                {confirmUserPageStore.errors.meta !== null
                                    ? t(
                                          confirmUserPageStore.errors.meta.errorKey,
                                          confirmUserPageStore.errors.meta.options,
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
                                onClick={() =>
                                    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                    confirmUserPageStore.submit(email!, navigate)
                                }
                                disabled={!confirmUserPageStore.isValid}
                            >
                                {t('common.submit')}
                            </Button>
                        </Stack>
                    </Paper>
                </Stack>
            </Grid>
        </Grid>
    );
});

export default ConfirmEmailPage;
