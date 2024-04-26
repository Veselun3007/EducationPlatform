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
import { Delete, Link } from '@mui/icons-material';
import ValidationError from '../helpers/validation/ValidationError';

import { useTranslation } from 'react-i18next';

import { action, observable } from 'mobx';
import { useNavigate } from 'react-router-dom';

interface LinkCardProps {
    link: string;
}

const LinkCard: React.FC<LinkCardProps> = observer(({ link }) => {
    const theme = useTheme();

    return (
        <Paper sx={{ width: '100%', height: '100%', overflow: 'hidden' }}>
            <CardActionArea
                onClick={(e) => {
                    window.location.replace(link);
                }}
                sx={{ width: '100%', height: '100%', alignContent: 'center' }}
            >
                <Stack
                    direction="row"
                    height="100%"
                    overflow="hidden"
                    spacing={1}
                    alignItems="center"
                >
                    <Avatar
                        sx={{ height: '100%', bgcolor: theme.palette.primary.main }}
                        variant="square"
                    >
                        <Link />
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
                        {link}
                    </Typography>
                </Stack>
            </CardActionArea>
        </Paper>
    );
});

export default LinkCard;
