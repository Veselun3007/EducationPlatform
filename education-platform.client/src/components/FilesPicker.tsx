import React from 'react';

import { observer } from 'mobx-react-lite';
import Grid from '@mui/material/Unstable_Grid2';
import { Button, IconButton, Paper, Stack, Typography } from '@mui/material';
import { AttachFile, Delete } from '@mui/icons-material';
import ValidationError from '../helpers/validation/ValidationError';
import { useTranslation } from 'react-i18next';


interface FilesPickerProps {
    onFileAdd(e: React.ChangeEvent<HTMLInputElement>): void;
    onFileDelete(id: number): void;
    files: File[];
    error: ValidationError | null;
}

const FilesPicker: React.FC<FilesPickerProps> = observer(({ onFileAdd, error, onFileDelete, files
}) => {
    const { t } = useTranslation();
    return (
        <Stack direction="column" width="100%">
            <Grid container spacing={1}>
                {files.map(((file, index) => (
                    <Grid key={index} xs={12}>
                        <Paper variant="outlined">
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
                                >{file.name}</Typography>
                                <IconButton onClick={() => onFileDelete(index)}><Delete /></IconButton>
                            </Stack>
                        </Paper>
                    </Grid>
                )))}
                <Grid xs={12}>
                    <Button
                    sx={{height:50}}
                    variant='contained'
                        component="label"
                        color='primary'
                        startIcon={<AttachFile/>}
                    >
                        {t('common.addFile')}
                        <input
                            type="file"
                            hidden
                            multiple
                            onChange={onFileAdd}
                        />
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
    )

});

export default FilesPicker;
