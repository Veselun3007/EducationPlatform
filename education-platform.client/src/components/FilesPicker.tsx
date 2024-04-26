import React from 'react';

import { observer } from 'mobx-react-lite';
import Grid from '@mui/material/Unstable_Grid2';
import {
    Avatar,
    Button,
    IconButton,
    Paper,
    Stack,
    Typography,
    useTheme,
} from '@mui/material';
import { AttachFile, Delete } from '@mui/icons-material';
import ValidationError from '../helpers/validation/ValidationError';
import { useTranslation } from 'react-i18next';

interface FilesPickerProps {
    onFileAdd(e: React.ChangeEvent<HTMLInputElement>): void;
    onFileDelete(id: number): void;
    files: File[];
    error: ValidationError | null;
    fullWidth?: boolean;
    variant?: string;
}

const FilesPicker: React.FC<FilesPickerProps> = observer(
    ({ onFileAdd, error, onFileDelete, files, fullWidth, variant }) => {
        const { t } = useTranslation();
        const theme = useTheme();
        return (
            <Stack direction="column" width="100%">
                <Grid container spacing={1}>
                    {files.map((file, index) => (
                        <Grid key={index} xs={12}>
                            <Paper variant="outlined" sx={{ overflow: 'hidden' }}>
                                <Stack
                                    direction="row"
                                    height={50}
                                    alignItems="center"
                                    spacing={1}
                                    overflow="hidden"
                                    pr={1}
                                >
                                    <Avatar
                                        sx={{
                                            height: '100%',
                                            bgcolor: theme.palette.primary.main,
                                        }}
                                        variant="square"
                                    >
                                        <AttachFile />
                                    </Avatar>
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
                                        flexGrow={1}
                                    >
                                        {file.name}
                                    </Typography>
                                    <IconButton onClick={() => onFileDelete(index)}>
                                        <Delete />
                                    </IconButton>
                                </Stack>
                            </Paper>
                        </Grid>
                    ))}
                    <Grid xs={12}>
                        <Button
                            sx={{ height: 50 }}
                            variant="contained"
                            component="label"
                            color="secondary"
                            startIcon={<AttachFile />}
                            fullWidth={fullWidth ? fullWidth : false}
                        >
                            {t('common.addFile')}
                            <input type="file" hidden multiple onChange={onFileAdd} />
                        </Button>
                    </Grid>
                </Grid>
                <Typography
                    color="error"
                    variant="caption"
                    visibility={error !== null ? 'visible' : 'collapse'}
                >
                    {error !== null ? t(error.errorKey, error.options) : null}
                </Typography>
            </Stack>
        );
    },
);

export default FilesPicker;
