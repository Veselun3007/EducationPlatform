import React from 'react';
import { Route, Routes, useNavigate } from 'react-router-dom';
import IntroductionPage from './pages/IntroductionPage/IntroductionPage';
import NotFoundPage from './pages/NotFoundPage/NotFoundPage';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import LoginPage from './pages/LoginPage/LoginPage';
import SignUpPage from './pages/SignUpPage/SignUpPage';
import NotificationProvider from './components/Notification/NotificationProvider';
import { RootStoreContext } from './context/RootStoreContext';
import RootStore from './stores/RootStore';
import ConfirmEmailPage from './pages/ConfirmEmailPage/ConfirmUserPage';
import DashboardPage from './pages/DashboardPage/DashboardPage';
import AuthRoutes from './HOC/AuthRoutes';
import NotAuthRoutes from './HOC/NotAuthRoutes';
import { Button, Typography } from '@mui/material';
import CoursePage from './pages/CoursePage/CoursePage';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import AssignmentPage from './pages/AssignmentPage/AssignmentPage';
import MaterialPage from './pages/MaterialPage/MaterialPage';
import JoinCoursePage from './pages/JoinCoursePage/JoinCoursePage';
import UsersPage from './pages/UsersPage/UsersPage';
import ChatPage from './pages/ChatPage/ChatPage';

const darkTheme = createTheme({
    palette: {
        mode: 'dark',

        primary: {
            main: '#8cccdb',
        },
        secondary: {
            main: '#312577',
        },
    },
});

// const darkTheme = createTheme({
//   palette: {
//     mode: 'dark', // Set to 'dark' for dark theme
//     background: {
//       default: '#1A1A1A', // Very dark background
//       paper: '#222222',  // Darker paper color
//     },
//     text: {
//       primary: '#F2F2F2',  // White for readability
//       secondary: '#CCCCCC', // Lighter white for less important text
//       disabled: 'rgba(255, 255, 255, 0.5)',  // Faded white for disabled elements
//     },
//     primary: {
//       main: '#FF5722', // Deep orange
//       light: '#FF8A65',  // Lighter deep orange
//       dark: '#E64A19',   // Darker deep orange
//       contrastText: '#FFFFFF',  // White for contrast
//     },
//     secondary: {
//       main: '#3F51B5', // Indigo
//       light: '#7986CB',  // Lighter indigo
//       dark: '#303F9F',   // Darker indigo
//       contrastText: '#FFFFFF',  // White for contrast
//     },
//     action: {
//       active: 'rgba(255, 255, 255, 1)',  // Slightly transparent white for active elements
//       hover: 'rgba(255, 255, 255, 0.1)',   // Very transparent white for hover state
//       selected: 'rgba(255, 255, 255, 0.3)',  // Slightly transparent white for selected elements
//     },
//     divider: 'rgba(255, 255, 255, 0.12)',  // Very faint white line for dividers

//   },
// });

// const darkTheme = createTheme({
//   palette: {
//     mode: 'dark', // Set to 'dark' for dark theme
//     background: {
//       default: '#121212', // Darker background
//       paper: '#1E1E1E',  // Even darker paper color
//     },
//     text: {
//       primary: '#FFFFFF',  // White for readability
//       secondary: '#CCCCCC', // Lighter white for less important text
//       disabled: 'rgba(255, 255, 255, 0.5)',  // Faded white for disabled elements
//     },
//     primary: {
//       main: '#6200EE', // Deep purple (Google's Material Design 3 primary color)
//       light: '#BB86FC',  // Lighter purple
//       dark: '#3700B3',   // Darker purple
//       contrastText: '#FFFFFF',  // White for contrast
//     },
//     secondary: {
//       main: '#03DAC5', // Teal (Google's Material Design 3 secondary color)
//       light: '#64FFDA',  // Lighter teal
//       dark: '#00BFA5',   // Darker teal
//       contrastText: '#000000',  // Black for contrast
//     },
//     action: {
//       active: 'rgba(255, 255, 255, 0.7)',  // Slightly transparent white for active elements
//       hover: 'rgba(255, 255, 255, 0.1)',   // Very transparent white for hover state
//       selected: 'rgba(255, 255, 255, 0.3)',  // Slightly transparent white for selected elements
//     },
//     divider: 'rgba(255, 255, 255, 0.12)',  // Very faint white line for dividers
//   },
// });

const rootStore: RootStore = new RootStore();

function App() {
    const navigate = useNavigate();
    console.log('rerendered');
    return (
        <RootStoreContext.Provider value={rootStore}>
            <ThemeProvider theme={darkTheme}>
                <LocalizationProvider dateAdapter={AdapterDayjs}>
                    <CssBaseline />
                    <NotificationProvider autoHideDuration={3000} />
                    <Routes>
                        <Route index element={<IntroductionPage />} />

                        <Route element={<NotAuthRoutes />}>
                            <Route path="/login" element={<LoginPage />} />
                            <Route path="/signup" element={<SignUpPage />} />
                            <Route
                                path="/confirmEmail/:email"
                                element={<ConfirmEmailPage />}
                            />

                        </Route>

                        <Route element={<AuthRoutes />}>
                            <Route path="/dashboard" element={<DashboardPage />} />
                            <Route path="/course/:id" element={<CoursePage />} />
                            <Route path="/course/:courseId/join/:courseLink" element={<JoinCoursePage />} />
                            <Route
                                path="/course/:courseId/material/:materialId"
                                element={<MaterialPage />}
                            />
                            <Route
                                path="/course/:courseId/assignment/:assignmentId"
                                element={<AssignmentPage />}
                            />
                            <Route
                                path="/course/:courseId/chat"
                                element={<ChatPage />}
                            />

                            <Route
                                path="/course/:courseId/users"
                                element={<UsersPage />}
                            />
                            <Route
                                path="/course/:courseId/assignment/:assignmentId/mark"
                                element={<Typography>Mark</Typography>}
                            />
                        </Route>

                        <Route path="404" element={<NotFoundPage />} />
                        <Route path="*" element={<NotFoundPage />} />
                    </Routes>
                </LocalizationProvider>
            </ThemeProvider>
        </RootStoreContext.Provider>
    );
}

export default App;
