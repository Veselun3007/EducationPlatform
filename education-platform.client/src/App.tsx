import React from 'react';
import { Route, Routes } from 'react-router-dom';
import IntroductionPage from './pages/IntroductionPage';
import NotFoundPage from './pages/NotFoundPage';


function App() {
    return (
        <Routes>
            <Route index element={<IntroductionPage/>}/>

            <Route path="*" element={<NotFoundPage/>}/>
        </Routes>
    );
}

export default App;
