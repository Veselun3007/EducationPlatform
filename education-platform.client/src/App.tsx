import React from 'react';
import { Route, Routes } from 'react-router-dom';
import IntroductionPage from './pages/IntroductionPage';
import NotFoundPage from './pages/NotFoundPage';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
        primary: {
            main: '#f55951',
        },
        secondary:{
            main:'#008080'
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
            <Routes>
                <Route index element={<IntroductionPage />} />

                <Route path="*" element={<NotFoundPage />} />
            </Routes>
        </ThemeProvider>
    );
}

export default App;
