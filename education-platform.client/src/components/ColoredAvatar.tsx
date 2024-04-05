import React from 'react';
import Avatar, { AvatarProps } from '@mui/material/Avatar';

function stringToColor(string: string) {
    let hash = 0;
    let i;

    /* eslint-disable no-bitwise */
    for (i = 0; i < string.length; i += 1) {
        hash = string.charCodeAt(i) + ((hash << 5) - hash);
    }

    let color = '#';

    for (i = 0; i < 3; i += 1) {
        const value = (hash >> (i * 8)) & 0xff;
        color += `00${value.toString(16)}`.slice(-2);
    }
    /* eslint-enable no-bitwise */

    return color;
}

function stringAvatar(name: string | undefined) {
    if (!name) {
        return {
            sx: {
                bgcolor: 'grey',
            },
            children: `img`,
        };
    }
    return {
        sx: {
            bgcolor: stringToColor(name),
        },
        children: `${name.split(' ')[0][0]}${name.split(' ')[1][0]}`,
    };
}

const ColoredAvatar: React.FC<AvatarProps> = ({ ...props }) => {
    const stringProps = stringAvatar(props.alt);
    const sx = { ...stringProps.sx, ...props.sx };
    delete props.sx;
    return (
        <Avatar sx={sx} {...props}>
            {stringProps.children}
        </Avatar>
    );
};
export default ColoredAvatar;
