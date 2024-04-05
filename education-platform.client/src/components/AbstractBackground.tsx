import { sha256 } from 'js-sha256';
import React from 'react';
import { Box, SxProps } from '@mui/material';

interface AbstractBackgroundProps {
    value: string;
    children?: React.ReactNode;
    sx?: SxProps;
}

const AbstractBackground: React.FC<AbstractBackgroundProps> = ({
    value,
    children,
    sx
}) => {
    const hash = sha256(value);

    const direction = parseInt(hash.slice(0, 1), 16);

    const hexNumOfColors = parseInt(hash[2], 16);
    let numOfColors;
    if (hexNumOfColors <= 4) {
        numOfColors = 2;
    } else if (hexNumOfColors <= 8) {
        numOfColors = 3;
    } else if (hexNumOfColors <= 12) {
        numOfColors = 4;
    } else {
        numOfColors = 5;
    }

    function hexToRgb(hex: string) {
        const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
        return result
            ? {
                  r: parseInt(result[1], 16),
                  g: parseInt(result[2], 16),
                  b: parseInt(result[3], 16),
              }
            : null;
    }

    let gradient = `linear-gradient(${direction}deg, `;
    for (let i = 0; i < numOfColors; i++) {
        const hex = hash.slice(i * 6, (i + 1) * 6);
        const rgb = hexToRgb(hex);
        gradient = gradient + `rgb(${rgb?.r}, ${rgb?.g}, ${rgb?.b})`;
        if (i != numOfColors - 1) {
            gradient = gradient + ', ';
        }
    }
    gradient = gradient + ')';

    return (
        <Box
            sx={{ position: 'relative', overflow: 'hidden', top: 0, left: 0, ...sx}}
        >
            <Box
                width="100%"
                height="100%"
                sx={{ zIndex: 1, display: 'flex', position: 'absolute', top: 0, left: 0 }}
            >
                {children}
            </Box>
            <Box
                width="100%"
                height="100%"
                sx={{
                    background: gradient,
                    filter: 'blur(10px) brightness(50%)',
                   
                }}
            />
        </Box>
    );
};

export default AbstractBackground;
