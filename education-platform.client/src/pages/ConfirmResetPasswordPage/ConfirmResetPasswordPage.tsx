import {
  Avatar,
  Button,
  Divider,
  Grid,
  Link,
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
import { useNavigate } from 'react-router-dom';

const ConfirmResetPasswordPage = observer(() => {
  const { confirmResetPasswordStore } = useStore();
  const { t } = useTranslation();
  const navigate = useNavigate();

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
                {t('glossary.confirmResetPasswordHeader')}
              </Typography>
              <Avatar sx={{ bgcolor: 'primary.main' }}>
                <Email />
              </Avatar>
              <TextField
                fullWidth
                required
                type="email"
                label={t('common.email')}
                value={confirmResetPasswordStore.data.email}
                onChange={confirmResetPasswordStore.onEmailChange}
                error={confirmResetPasswordStore.errors.email !== null}
                helperText={
                  confirmResetPasswordStore.errors.email !== null
                    ? t(
                      confirmResetPasswordStore.errors.email.errorKey,
                      confirmResetPasswordStore.errors.email.options,
                    )
                    : null
                }
              />
              <Typography
                color="error"
                variant="caption"
                align="center"
                visibility={
                  confirmResetPasswordStore.errors.meta !== null
                    ? 'visible'
                    : 'collapse'
                }
              >
                {confirmResetPasswordStore.errors.meta !== null
                  ? t(
                    confirmResetPasswordStore.errors.meta.errorKey,
                    confirmResetPasswordStore.errors.meta.options,
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
                onClick={() => {
                    confirmResetPasswordStore.submit(navigate);                 
                }}
                disabled={!confirmResetPasswordStore.isValid}
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

export default ConfirmResetPasswordPage;