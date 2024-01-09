import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import Backend from 'i18next-http-backend';
import LanguageDetector from 'i18next-browser-languagedetector';

export const locales ={
    en:'English',
    uk:'Українська'
}

i18n
    .use(Backend)
    .use(LanguageDetector)
    .use(initReactI18next)
    .init({
        fallbackLng: 'en',
          interpolation: {
            escapeValue: false
          }
    });
    
    
    export const setLanguage = (language: string) => {
        i18n.changeLanguage(language);
    };

    export default i18n;