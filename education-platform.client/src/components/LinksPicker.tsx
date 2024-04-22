import React from 'react';

import { observer, useLocalObservable } from 'mobx-react-lite';
import Grid from '@mui/material/Unstable_Grid2';
import { Button, IconButton, Modal, Paper, Stack, TextField, Typography, useTheme } from '@mui/material';
import { Delete, Link } from '@mui/icons-material';
import ValidationError from '../helpers/validation/ValidationError';

import { useTranslation } from 'react-i18next';

import { action, observable } from 'mobx';


interface LinksPickerProps {
    onLinkAdd(link: string): void;
    onLinkDelete(id: number): void;
    links: string[];
    error: ValidationError | null;
}

const LinksPicker: React.FC<LinksPickerProps> = observer(({ onLinkAdd, error, onLinkDelete, links
}) => {
    const { t } = useTranslation();
    const theme = useTheme()
    const store = useLocalObservable(() => ({
        link: '',
        isOpen: false,
        handleOpen() {
            this.isOpen = true;
        },
        handleClose() {
            this.link = '';
            this.isOpen = false
        },
        onLinkChange(e: React.ChangeEvent<HTMLInputElement>) {
            this.link = e.target.value;
        },

    }), {
        link: observable,
        isOpen: observable,
        handleOpen: action.bound,
        handleClose: action.bound,
        onLinkChange: action.bound,
    })
    return (
        <>
            <Stack direction="column" spacing={2} width="100%">
                <Grid container spacing={1}>
                    {links.map(((link, index) => (
                        <Grid key={index} xs={12} >
                            <Paper variant="outlined" >
                                <Stack direction="row" p={1} height={50} alignItems="center" justifyContent="space-between">
                                    <Typography

                                        textAlign="left"
                                        sx={{
                                            display: '-webkit-box',
                                            overflow: 'hidden',
                                            WebkitBoxOrient: 'vertical',
                                            WebkitLineClamp: 1,
                                            textOverflow: 'ellipsis',
                                            whiteSpace: 'normal',
                                        }}
                                    >{link}</Typography>
                                    <IconButton onClick={() => onLinkDelete(index)}><Delete /></IconButton>
                                </Stack>
                            </Paper>
                        </Grid>
                    )))}
                    <Grid xs={12}>
                        <Button
                            sx={{ height: 50 }}
                            variant='contained'
                            component="label"
                            color='primary'
                            startIcon={<Link />}
                            onClick={store.handleOpen}
                        >
                            {t('common.addLink')}
                        </Button>
                    </Grid>
                </Grid>
                <Typography
                    color="error"
                    variant="caption"
                    visibility={
                        error !== null
                            ? 'visible'
                            : 'collapse'
                    }
                >
                    {error !== null
                        ? t(
                            error.errorKey,
                            error.options,
                        )
                        : null}
                </Typography>
            </Stack>
            <Modal open={store.isOpen} onClose={store.handleClose} sx={{ height: '100%' }}>
                <Stack
                    bgcolor={theme.palette.background.paper}
                    width={{ xs: '90%', md: '40%' }}
                    height="fit-content"
                    sx={{
                        position: 'absolute',
                        top: '50%',
                        left: '50%',
                        transform: 'translate(-50%, -50%)',
                    }}
                    overflow="auto"
                    justifyContent="center"
                    spacing={{ xs: 1, md: 2 }}
                    p={3}
                    alignItems="center"
                >
                    <Typography textAlign="start" width="100%" variant="h6">
                        {t('glossary.createTopic')}
                    </Typography>
                    <TextField
                        fullWidth
                        required
                        type="text"
                        label={t('common.link')}
                        value={store.link}
                        onChange={store.onLinkChange}
                    />
                    <Stack direction="row" width="100%" justifyContent="end">
                        <Button
                            color="inherit"
                            onClick={store.handleClose}
                        >
                            {t('common.close')}
                        </Button>
                        <Button
                            color="primary"
                            onClick={() => {
                                if (store.link !== '') {
                                    onLinkAdd(store.link)
                                    store.handleClose()
                                } else{
                                    store.handleClose();
                                }

                            }}
                        >
                            {t('common.add')}
                        </Button>
                    </Stack>
                </Stack>
            </Modal>
        </>
    )

});

export default LinksPicker;
