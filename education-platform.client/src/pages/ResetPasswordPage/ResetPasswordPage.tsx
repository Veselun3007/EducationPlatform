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

const ResetPasswordPage = observer(() => {
  const { resetPasswordStore } = useStore();
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { email } = useParams();

  useEffect(() => () => resetPasswordStore.reset(), []);

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
                {t('glossary.resetPasswordHeader')}
              </Typography>
              <Avatar sx={{ bgcolor: 'primary.main' }}>
                <Email />
              </Avatar>
              <Typography variant="body2">
                {t('glossary.resetPassword')}
              </Typography>
              <TextField
                fullWidth
                required
                type="text"
                label={t('common.code')}
                value={resetPasswordStore.data.confirmCode}
                onChange={resetPasswordStore.onConfirmCodeChange}
                error={resetPasswordStore.errors.confirmCode !== null}
                helperText={
                  resetPasswordStore.errors.confirmCode !== null
                    ? t(
                      resetPasswordStore.errors.confirmCode.errorKey,
                      resetPasswordStore.errors.confirmCode.options,
                    )
                    : null
                }
              />
              <TextField
                fullWidth
                required
                type="password"
                label={t('common.password')}
                value={resetPasswordStore.data.password}
                onChange={resetPasswordStore.onPasswordChange}
                error={resetPasswordStore.errors.password !== null}
                helperText={
                  resetPasswordStore.errors.password !== null
                    ? t(
                      resetPasswordStore.errors.password.errorKey,
                      resetPasswordStore.errors.password.options,
                    )
                    : null
                }
              />
              <Typography
                color="error"
                variant="caption"
                align="center"
                visibility={
                  resetPasswordStore.errors.meta !== null
                    ? 'visible'
                    : 'collapse'
                }
              >
                {resetPasswordStore.errors.meta !== null
                  ? t(
                    resetPasswordStore.errors.meta.errorKey,
                    resetPasswordStore.errors.meta.options,
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
                  resetPasswordStore.submit(email!, navigate)
                }
                disabled={!resetPasswordStore.isValid}
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

export default ResetPasswordPage;