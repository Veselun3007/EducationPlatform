import React from 'react';

import { observer, useLocalObservable } from 'mobx-react-lite';
import Grid from '@mui/material/Unstable_Grid2';
import {
    Avatar,
    Button,
    CardActionArea,
    IconButton,
    Modal,
    Paper,
    Stack,
    TextField,
    Typography,
    useTheme,
} from '@mui/material';
import { AttachFile, Delete, Link } from '@mui/icons-material';
import ValidationError from '../helpers/validation/ValidationError';

import { useTranslation } from 'react-i18next';

import { action, observable } from 'mobx';
import { useNavigate } from 'react-router-dom';

interface FileCardProps {
    file: string;
    onClick(): void;
    bgColor?: string
}

const FileCard: React.FC<FileCardProps> = observer(({ file, onClick, bgColor }) => {
    const theme = useTheme();
    return (
        <Paper sx={{maxWidth: '100%', height: '100%', overflow: 'hidden', bgcolor:bgColor }}>
            <CardActionArea
                onClick={onClick}
                sx={{ width: '100%', height: '100%', alignContent: 'center' }}
            >
                <Stack
                    direction="row"
                    height="100%"
                    overflow="hidden"
                    spacing={1}
                    alignItems="center"
                    pr={1}
                >
                    <Avatar
                        sx={{ height: '100%', bgcolor: theme.palette.primary.main }}
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
                        
                    >
                        {file}
                    </Typography>
                </Stack>
            </CardActionArea>
        </Paper>
    );
});

export default FileCard;
