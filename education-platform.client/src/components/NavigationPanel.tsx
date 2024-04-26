
import * as React from 'react';
import { styled, useTheme, Theme, CSSObject } from '@mui/material/styles';
import Box from '@mui/material/Box';
import MuiDrawer from '@mui/material/Drawer';
import MuiAppBar, { AppBarProps as MuiAppBarProps } from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import List from '@mui/material/List';
import CssBaseline from '@mui/material/CssBaseline';
import Typography from '@mui/material/Typography';
import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import { useStore } from '../context/RootStoreContext';
import HomeIcon from '@mui/icons-material/Home';
import { Button, CircularProgress, Drawer, Menu, MenuItem, Modal, Paper, Stack, Tab, TextField, ToggleButton, ToggleButtonGroup, useMediaQuery } from '@mui/material';
import { action, observable } from 'mobx';
import { observer, useLocalObservable } from 'mobx-react-lite';
import { useLocation, useNavigate } from 'react-router-dom';
import debounce from '../helpers/debounce';
import AbstractBackground from './AbstractBackground';
import { useTranslation } from 'react-i18next';
import ColoredAvatar from './ColoredAvatar';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { locales } from '../i18n';
import { Close, Edit } from '@mui/icons-material';
import { useEffect } from 'react';


const drawerWidth = 240;

const openedMixin = (theme: Theme): CSSObject => ({
    width: drawerWidth,
    transition: theme.transitions.create('width', {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.enteringScreen,
    }),
    overflowX: 'hidden',
});

const closedMixin = (theme: Theme): CSSObject => ({
    transition: theme.transitions.create('width', {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.leavingScreen,
    }),
    overflowX: 'hidden',
    width: `calc(${theme.spacing(6)} + 1px)`,
    [theme.breakpoints.up('md')]: {
        width: `calc(${theme.spacing(8)} + 1px)`,
    },
});

const DrawerHeader = styled('div')(({ theme }) => ({
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'flex-end',
    padding: theme.spacing(0, 1),
    ...theme.mixins.toolbar,
}));

interface AppBarProps extends MuiAppBarProps {
    isSmallScreen?: boolean;
}

const AppBar = styled(MuiAppBar, {
    shouldForwardProp: (prop) => prop !== 'isSmallScreen',
})<AppBarProps>(({ theme, isSmallScreen }) => ({
    zIndex: isSmallScreen ? theme.zIndex.drawer : theme.zIndex.drawer + 1,
    // transition: theme.transitions.create(['width', 'margin'], {
    //     easing: theme.transitions.easing.sharp,
    //     duration: theme.transitions.duration.leavingScreen,
    // }),
    // ...(open && {
    //     marginLeft: drawerWidth,
    //     width: `calc(100% - ${drawerWidth}px)`,
    //     transition: theme.transitions.create(['width', 'margin'], {
    //         easing: theme.transitions.easing.sharp,
    //         duration: theme.transitions.duration.enteringScreen,
    //     }),
    // }),
}));

const PermanentDrawer = styled(MuiDrawer, {
    shouldForwardProp: (prop) => prop !== 'open',
})(({ theme, open }) => ({
    width: drawerWidth,
    flexShrink: 0,
    whiteSpace: 'nowrap',
    boxSizing: 'border-box',
    ...(open && {
        ...openedMixin(theme),
        '& .MuiDrawer-paper': openedMixin(theme),
    }),
    ...(!open && {
        ...closedMixin(theme),
        '& .MuiDrawer-paper': closedMixin(theme),
    }),
}));

interface NavigationPanelProps {
    children?: React.ReactNode;
}

const NavigationPanel: React.FC<NavigationPanelProps> = observer(({ children }) => {
    const theme = useTheme();
    const navigate = useNavigate();
    const { pathname } = useLocation();
    const isSmallScreen = useMediaQuery(theme.breakpoints.down('md'));
    const { t, i18n } = useTranslation();
    const { dashboardPageStore, userStore } = useStore();

    useEffect(() => {
        userStore.getUser(navigate);
        return () => userStore.reset()
    }, [])

    const courseTopLevelPath = pathname.replace(/(\/course\/\d+).*/, '$1');

    const localStore = useLocalObservable(
        () => ({
            drawerOpen: false,
            toggled: false,
            anchorEl: null as null | HTMLElement,
            settingsOpen: false,
            settingsTab: "1",

            handleDrawerOpen() {
                this.drawerOpen = true;
            },

            handleDrawerClose() {
                this.drawerOpen = false;
            },

            toggleDrawer() {
                this.toggled = !this.toggled;
            },

            handleMenuOpen(event: React.MouseEvent<HTMLElement>) {
                this.anchorEl = event.currentTarget;
            },
            handleMenuClose() {
                this.anchorEl = null;
            },

            handleSettingsClose() {
                this.settingsOpen = false;

            },

            handleSettingsOpen() {
                this.settingsOpen = true;
            },

            handleSettingsTabChange(event: React.SyntheticEvent, newValue: string) {
                this.settingsTab = newValue;
            },
        }),
        {
            drawerOpen: observable,
            toggled: observable,
            anchorEl: observable,
            settingsOpen: observable,
            settingsTab: observable,
            handleDrawerOpen: action.bound,
            handleDrawerClose: action.bound,
            toggleDrawer: action.bound,
            handleMenuOpen: action.bound,
            handleMenuClose: action.bound,
            handleSettingsClose: action.bound,
            handleSettingsOpen: action.bound,
            handleSettingsTabChange: action.bound,
        },
    );

    const content = (
        <>
            <ListItem
                disablePadding
                sx={{ display: 'block', marginTop: isSmallScreen ? 0 : '4rem' }}
            >
                <ListItemButton
                    selected={pathname === '/dashboard'}
                    sx={{
                        minHeight: 48,
                        justifyContent:
                            localStore.drawerOpen || localStore.toggled ? 'initial' : 'center',
                        px: 2.5,
                        borderBottomRightRadius: 24,
                        borderTopRightRadius: 24,
                        mr: 1,
                        maxWidth: drawerWidth,
                    }}
                    onClick={() => navigate('/dashboard')}
                >
                    <ListItemIcon
                        sx={{
                            minWidth: 0,
                            mr: localStore.drawerOpen || localStore.toggled ? 3 : 'auto',
                            justifyContent: 'center',
                        }}
                    >
                        <HomeIcon fontSize="large" />
                    </ListItemIcon>
                    <ListItemText
                        primary={'Dashboard'}
                        sx={{ opacity: localStore.drawerOpen || localStore.toggled ? 1 : 0 }}
                    />
                </ListItemButton>
            </ListItem>
            <Divider />
            <List>
                {dashboardPageStore.courses.map((cource) => (
                    <ListItem
                        key={cource.courseId}
                        disablePadding
                        sx={{ display: 'block' }}
                        onClick={() => navigate(`/course/${cource.courseId}`)}
                    >
                        <ListItemButton
                            sx={{
                                minHeight: 48,
                                justifyContent:
                                    localStore.drawerOpen || localStore.toggled
                                        ? 'initial'
                                        : 'center',
                                px: 2.5,
                                borderBottomRightRadius: 24,
                                borderTopRightRadius: 24,
                                maxWidth: drawerWidth,
                                mr: 1,
                            }}
                            selected={`/course/${cource.courseId}` === courseTopLevelPath}
                        >
                            <ListItemIcon
                                sx={{
                                    minWidth: 0,
                                    mr:
                                        localStore.drawerOpen || localStore.toggled
                                            ? 3
                                            : 'auto',
                                    justifyContent: 'center',
                                }}
                            >
                                <AbstractBackground value={cource.courseName} sx={{ borderRadius: '50%', height: '2.1875rem', width: '2.1875rem' }}>
                                    <Typography textAlign="center" alignSelf="center" width="100%">{cource.courseName[0]}</Typography>
                                </AbstractBackground>
                            </ListItemIcon>
                            <ListItemText
                                primary={cource.courseName}
                                sx={{
                                    opacity:
                                        localStore.drawerOpen || localStore.toggled ? 1 : 0,
                                    display: '-webkit-box',
                                    overflow: 'hidden',
                                    WebkitBoxOrient: 'vertical',
                                    WebkitLineClamp: 1,
                                    textOverflow: 'ellipsis',
                                    whiteSpace: 'normal',
                                }}
                            />
                        </ListItemButton>
                    </ListItem>
                ))}
            </List>
        </>
    );

    const permanentDrawer = (
        <>
            <PermanentDrawer
                variant="permanent"
                open={localStore.drawerOpen || localStore.toggled}
                onMouseOverCapture={() => {
                    if (!localStore.toggled) debounce(localStore.handleDrawerOpen, 100)();
                }}
                onMouseLeave={() => {
                    if (!localStore.toggled)
                        debounce(localStore.handleDrawerClose, 100)();
                }}
            >
                {content}
            </PermanentDrawer>
            <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
                <DrawerHeader />
                {children}
            </Box>
        </>
    );

    const temporaryDrawer = (
        <>
            <Drawer
                sx={{ width: drawerWidth }}
                open={localStore.drawerOpen}
                onClose={localStore.handleDrawerClose}
            >
                {content}
            </Drawer>
            <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
                <DrawerHeader />
                {children}
            </Box>
        </>
    );

    const handleLanguageChange = (
        event: React.MouseEvent<HTMLElement>,
        newLanguage: string,
    ) => {
        i18n.changeLanguage(newLanguage);
    };

    if (userStore.user === null || userStore.data === null) {
        return (
            <Box display="flex" height="100svh" width="100%" alignItems="center"  justifyContent="center">
                <CircularProgress/>
            </Box>
        );
    }

    return (
        <>
            <Box sx={{ display: 'flex' }}>
                <CssBaseline />
                <AppBar position="fixed" isSmallScreen={isSmallScreen}>
                    <Toolbar>
                        <IconButton
                            color="inherit"
                            aria-label="open drawer"
                            onClick={() =>
                                isSmallScreen
                                    ? localStore.handleDrawerOpen()
                                    : localStore.toggleDrawer()
                            }
                            edge="start"
                            sx={{
                                marginRight: 5,
                                // ...(localStore.open && { display: 'none' }),

                            }}
                        >
                            <MenuIcon />
                        </IconButton>
                        <Typography variant="h6" noWrap component="div" sx={{ flexGrow: 1 }}>
                            {t('glossary.educationPlatform')}
                        </Typography>
                        <ColoredAvatar sx={{ cursor: 'pointer' }} src={userStore.user.userImage} alt={userStore.user.userName} onClick={localStore.handleMenuOpen} />
                        <Menu
                            id="menu-appbar"
                            anchorEl={localStore.anchorEl}
                            anchorOrigin={{
                                vertical: 'bottom',
                                horizontal: 'center',
                            }}
                            keepMounted
                            transformOrigin={{
                                vertical: 'top',
                                horizontal: 'right',
                            }}
                            sx={{ mt: 1 }}
                            open={Boolean(localStore.anchorEl)}
                            onClose={localStore.handleMenuClose}

                        >
                            <Stack width={300} height="100%" alignItems="center" p={2}>
                                <ColoredAvatar sx={{ width: 120, height: 120 }} src={userStore.user.userImage} alt={userStore.user.userName} />
                                <Typography variant="h5" mt={1}>{userStore.user.userName}</Typography>
                                <Typography variant="caption" color="text.secondary">{userStore.user.email}</Typography>
                                <Paper sx={{ width: 250, overflow: 'hidden', marginTop: 1, bgcolor: theme.palette.background.default, borderRadius: 5 }}>
                                    <MenuItem onClick={localStore.handleSettingsOpen} sx={{ height: 50 }}>{t('common.settings')}</MenuItem>
                                    <MenuItem onClick={() => userStore.signOut(navigate)} sx={{ height: 50 }}>{t('common.logout')}</MenuItem>
                                </Paper>

                            </Stack>

                        </Menu>
                    </Toolbar>
                </AppBar>
                {isSmallScreen ? temporaryDrawer : permanentDrawer}
            </Box>
            <Modal open={localStore.settingsOpen} onClose={localStore.handleSettingsClose}
            >
                <Box bgcolor={theme.palette.background.paper} width="90%" height='70%'
                    sx={{ position: 'absolute', top: '50%', left: '50%', transform: 'translate(-50%, -50%)' }} overflow='auto'>
                    <TabContext value={localStore.settingsTab}>
                        <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                            <Stack direction="row" justifyContent="space-between" overflow="visible">
                                <TabList onChange={localStore.handleSettingsTabChange}>
                                    <Tab label={t('common.commonSettings')} value="1" />
                                    <Tab label={t('common.userSettings')} value="2" />
                                </TabList>
                                <IconButton size="large" onClick={localStore.handleSettingsClose}>
                                    <Close />
                                </IconButton>
                            </Stack>

                        </Box>
                        <TabPanel value="1">
                            <Stack spacing={1}>
                                <Stack direction="row" justifyContent="space-between" alignItems="center">
                                    <Typography>{t('glossary.appLanguage')}</Typography>
                                    <ToggleButtonGroup
                                        value={i18n.language}
                                        exclusive
                                        onChange={handleLanguageChange}
                                        aria-label="Platform"
                                    >
                                        {locales.map((value, id) => (
                                            <ToggleButton key={id} value={value}>
                                                {value}
                                            </ToggleButton>
                                        ))}
                                    </ToggleButtonGroup>
                                </Stack>
                                <Divider />
                            </Stack>
                        </TabPanel>
                        <TabPanel value="2">

                            <Stack width={300} height="100%" spacing={2}>
                                <Typography variant="h5">
                                    {t('glossary.editUser')}
                                </Typography>
                                <Stack
                                    justifyContent="center"
                                    spacing={{ xs: 1, md: 2 }}
                                    p={3}
                                    alignItems="center"
                                    width={300}
                                >
                                    <Stack direction="column" spacing={1} width="100%" alignItems="center">
                                        <Box position="relative" height={100} width={100} overflow="hidden" sx={{
                                            '&:hover': {
                                                cursor: "pointer",
                                                "& .darkener": {
                                                    visibility: 'visible',
                                                    bgcolor: 'rgba(0,0,0,0.5)'
                                                }
                                            }
                                        }} borderRadius="50%">
                                            <ColoredAvatar sx={{ width: '100%', height: '100%', objectFit: 'cover' }} src={userStore.previewImage} alt={userStore.data.userName} />
                                            <Box className="darkener" visibility="hidden" position="absolute" left="0%" top="0%" width="100%" height="100%" display="flex" justifyContent="center" alignItems="center">
                                                <IconButton sx={{ width: '100%', height: '100%' }} component="label">
                                                    <Edit />
                                                    <input
                                                        width="100%"
                                                        height="100%"
                                                        type="file"
                                                        hidden
                                                        accept=".jpg, .jpeg, .png"
                                                        onChange={userStore.onUserImageChange}
                                                    />

                                                </IconButton>
                                            </Box>
                                        </Box>

                                        <Typography
                                            color="error"
                                            variant="caption"
                                            visibility={
                                                userStore.errors.userImage !== null
                                                    ? 'visible'
                                                    : 'collapse'
                                            }
                                        >
                                            {userStore.errors.userImage !== null
                                                ? t(
                                                    userStore.errors.userImage.errorKey,
                                                    userStore.errors.userImage.options,
                                                )
                                                : null}
                                        </Typography>
                                    </Stack>
                                    <TextField
                                        fullWidth
                                        required
                                        type="email"
                                        label={t('common.email')}
                                        value={userStore.data.email}
                                        onChange={userStore.onEmailChange}
                                        error={userStore.errors.email !== null}
                                        helperText={
                                            userStore.errors.email !== null
                                                ? t(
                                                    userStore.errors.email.errorKey,
                                                    userStore.errors.email.options,
                                                )
                                                : null
                                        }
                                    />
                                    <TextField
                                        fullWidth
                                        required
                                        label={t('common.username')}
                                        value={userStore.data.userName}
                                        onChange={userStore.onUserNameChange}
                                        error={userStore.errors.userName !== null}
                                        helperText={
                                            userStore.errors.userName !== null
                                                ? t(
                                                    userStore.errors.userName.errorKey,
                                                    userStore.errors.userName.options,
                                                )
                                                : null
                                        }
                                    />

                                    <Typography
                                        color="error"
                                        variant="caption"
                                        align="center"
                                        visibility={
                                            userStore.errors.meta !== null
                                                ? 'visible'
                                                : 'collapse'
                                        }
                                    >
                                        {userStore.errors.meta !== null
                                            ? t(
                                                userStore.errors.meta.errorKey,
                                                userStore.errors.meta.options,
                                            )
                                            : null}
                                    </Typography>
                                    <Button
                                        variant="contained"
                                        color="success"
                                        fullWidth
                                        onClick={() => userStore.submit(navigate)}
                                        disabled={!userStore.isValid}
                                    >
                                        {t('common.submit')}
                                    </Button>

                                </Stack>
                                <Typography variant="h5">
                                    {t('glossary.deleteUser')}
                                </Typography>
                                <Button
                                    variant="contained"
                                    color="error"

                                    onClick={() => userStore.deleteUser(navigate)}
                                >
                                    {t('glossary.deleteUser')}
                                </Button>


                            </Stack>
                        </TabPanel>
                    </TabContext>
                </Box>
            </Modal>
        </>
    );
});

export default NavigationPanel;