import React from 'react';
import { useTranslation } from 'react-i18next';
import { setLanguage, locales } from '../i18n';

const IntroductionPage = () => {
    
    const {t} = useTranslation();
    return (
        <>
        <h1>{t('hello')}</h1>
        {
            Object.entries(locales)
            .map( ([key, value]) => <button key={key} onClick={()=>setLanguage(key)}>{value}</button> )
        }
        </>
    );
}

export default IntroductionPage;