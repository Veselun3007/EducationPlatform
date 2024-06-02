import React from 'react';
import { Route, Routes } from 'react-router-dom';
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
import { darkScrollbar } from '@mui/material';
import CoursePage from './pages/CoursePage/CoursePage';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import AssignmentPage from './pages/AssignmentPage/AssignmentPage';
import MaterialPage from './pages/MaterialPage/MaterialPage';
import JoinCoursePage from './pages/JoinCoursePage/JoinCoursePage';
import UsersPage from './pages/UsersPage/UsersPage';
import ChatPage from './pages/ChatPage/ChatPage';
import MarkWorksPage from './pages/MarkWorksPage/MarkWorksPage';

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
    components: {
        MuiCssBaseline: {
            styleOverrides: (themeParam) => ({
                body: themeParam.palette.mode === 'dark' ? darkScrollbar() : null,
            }),
        },
    },
});

const rootStore: RootStore = new RootStore();

function App() {
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
                            <Route
                                path="/course/:courseId/join/:courseLink"
                                element={<JoinCoursePage />}
                            />
                            <Route
                                path="/course/:courseId/material/:materialId"
                                element={<MaterialPage />}
                            />
                            <Route
                                path="/course/:courseId/assignment/:assignmentId"
                                element={<AssignmentPage />}
                            />
                            <Route path="/course/:courseId/chat" element={<ChatPage />} />

                            <Route
                                path="/course/:courseId/users"
                                element={<UsersPage />}
                            />
                            <Route
                                path="/course/:courseId/assignment/:assignmentId/mark"
                                element={<MarkWorksPage />}
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
