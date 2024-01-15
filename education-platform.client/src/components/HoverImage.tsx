import React from 'react';
import { Box, SxProps, Theme, Typography } from '@mui/material';
import './HoverImage.css';

type HoverImageProps = {
    imageSrc: string;
    imageAlt: string;
    backgroundColor: string;
    sx: SxProps<Theme>;
    children?: React.ReactNode;
};

const HoverImage: React.FC<HoverImageProps> = ({
    imageSrc,
    imageAlt,
    backgroundColor,
    sx,
    children,
}) => {
    return (
        <Box className="container" sx={sx}>
            <img className="image" src={imageSrc} alt={imageAlt} />
            <Box className="overlay" sx={{ bgcolor: backgroundColor }}>
                {children}
            </Box>
        </Box>
    );
};

export default HoverImage;
