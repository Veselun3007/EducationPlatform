import React from 'react';
import { Stack, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';

const NotFoundPage = () => {
    const { t } = useTranslation();

    return (
        <Stack direction="column" justifyContent="center" alignItems="center">
            <Typography fontSize={175} sx={{ opacity: 0.2 }} color="primary.main">
                404
            </Typography>
            <Typography variant="h2" fontSize={58}>
                {t('glossary.404error')}
            </Typography>
        </Stack>
    );
};

export default NotFoundPage;
