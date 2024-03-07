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

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
        primary: {
            main: '#8cccdb',
        },
        secondary: {
            main: '#312577',
        },
        //   background: {
        //     default: '#151515',
        // },
    },
});

const rootStore: RootStore = new RootStore();

function App() {
    console.log('rerendered');
    return (
        <RootStoreContext.Provider value={rootStore}>
            <ThemeProvider theme={darkTheme}>
                <CssBaseline />
                <NotificationProvider autoHideDuration={3000} />
                <Routes>
                    <Route index element={<IntroductionPage />} />
                    
                    <Route element={<NotAuthRoutes />}>
                        <Route path="/login" element={<LoginPage />} />
                        <Route path="/signup" element={<SignUpPage />} />
                        <Route path="/confirmEmail/:email" element={<ConfirmEmailPage />} />
                    </Route>

                    <Route element={<AuthRoutes />}>
                        <Route path="/dashboard" element={<DashboardPage />} />
                    </Route>

                    <Route path="*" element={<NotFoundPage />} />
                </Routes>
            </ThemeProvider>
        </RootStoreContext.Provider>
    );
}

export default App;
