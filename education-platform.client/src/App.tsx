import React from 'react';
import { Route, Routes } from 'react-router-dom';
import IntroductionPage from './pages/IntroductionPage/IntroductionPage';
import NotFoundPage from './pages/NotFoundPage/NotFoundPage';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import LoginPage from './pages/LoginPage/LoginPage';
import SignUpPage from './pages/SignUpPage/SignUpPage';
import NotificationProvider from './components/Notification/NotificationProvider';

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
        primary: {
            main: '#f55951',
        },
        secondary: {
            main: '#008080',
        },
        //   background: {
        //     default: '#151515',
        // },
    },
});

function App() {
    return (
        <ThemeProvider theme={darkTheme}>
            <CssBaseline />
            <NotificationProvider autoHideDuration={3000} />
            <Routes>
                <Route index element={<IntroductionPage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/signup" element={<SignUpPage />} />
                <Route path="*" element={<NotFoundPage />} />
            </Routes>
        </ThemeProvider>
    );
}

export default App;
