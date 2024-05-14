import React from 'react';

import { observer } from 'mobx-react-lite';
import {
    Avatar,
    CardActionArea,
    Paper,
    Stack,
    Typography,
    useTheme,
} from '@mui/material';
import { Link } from '@mui/icons-material';

interface LinkCardProps {
    link: string;
}

const LinkCard: React.FC<LinkCardProps> = observer(({ link }) => {
    const theme = useTheme();

    return (
        <Paper sx={{ width: '100%', height: '100%', overflow: 'hidden' }}>
            <CardActionArea
                // eslint-disable-next-line @typescript-eslint/no-unused-vars
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
