import React from 'react';
import { useTranslation } from 'react-i18next';
import { locales } from '../i18n';
import Grid from '@mui/material/Unstable_Grid2';
import { Button, Card, CardActions, CardContent, CardMedia, Stack, ToggleButton, ToggleButtonGroup, Typography } from '@mui/material';
import './IntroductionPage.css';


const IntroductionPage = () => {
    const { t, i18n } = useTranslation();

    const handleLanguageChange = (event: React.MouseEvent<HTMLElement>, newLanguage: string,) => {
        i18n.changeLanguage(newLanguage);
    };

    return (
        <Stack direction="column" spacing={5} justifyContent="center">
            <div className="gradient-bg">
                <Stack className="content" spacing={{ xs: 4, sm: 3, md: 2 }} useFlexGap>
                    <Typography variant="h2" fontSize={58}>
                        {t('glossary.introductionTitle')}
                    </Typography>
                    <Typography variant="subtitle1" fontSize={18}>
                        {t('glossary.introductionSubtitle')}
                    </Typography>
                    <Stack direction="row" spacing={5} mt={3}>
                        <Button size="large" variant="contained" color='secondary'>
                            {t('common.signUp')}
                        </Button>
                        <Button size="large" variant="contained" >
                            {t('common.login')}
                        </Button>
                    </Stack>
                </Stack>
                <div className="topBar">
                    <ToggleButtonGroup

                        value={i18n.language}
                        exclusive
                        onChange={handleLanguageChange}
                        aria-label="Platform"
                    >
                        {
                            locales.map((value, id) => (<ToggleButton key={id} value={value}>{value}</ToggleButton>))
                        }
                    </ToggleButtonGroup>
                </div>
                <svg xmlns="http://www.w3.org/2000/svg">
                    <defs>
                        <filter id="goo">
                            <feGaussianBlur
                                in="SourceGraphic"
                                stdDeviation="10"
                                result="blur"
                            />
                            <feColorMatrix
                                in="blur"
                                mode="matrix"
                                values="1 0 0 0 0  0 1 0 0 0  0 0 1 0 0  0 0 0 18 -8"
                                result="goo"
                            />
                            <feBlend in="SourceGraphic" in2="goo" />
                        </filter>
                    </defs>
                </svg>
                <div className="gradients-container">
                    <div className="g1"></div>
                    <div className="g2"></div>
                    <div className="g3"></div>
                    <div className="g4"></div>
                    <div className="g5"></div>
                </div>
            </div>
            {/* <Button sx={{width:'5em', margin: '5em'}} size="large" variant="contained">
                            {t('common.login')}
                        </Button> */}
            {/* <Grid container >

            </Grid> */}
            
            <Typography align='center' variant='h3'>{t('glossary.platformCapabilities')}</Typography>
            <Stack direction={{ xs: 'column', md: 'row' }} spacing={3} justifyContent="center" alignItems="stretch">

                <Card sx={{ maxWidth: 345 }}>
                    <CardMedia
                        component="img"
                        image="/assets/communication.jpg"
                    />
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="div">
                            {t('glossary.platformCapabilityCommunication')}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            {t('glossary.platformCapabilityCommunicationDesc')}
                        </Typography>
                    </CardContent>
                </Card>
                <Card sx={{ maxWidth: 345 }}>
                    <CardMedia
                        component="img"
                        image="/assets/assessment.jpg"
                    />
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="div">
                            {t('glossary.platformCapabilityAssessment')}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            {t('glossary.platformCapabilityAssessmentDesc')}
                        </Typography>
                    </CardContent>
                </Card>
                <Card sx={{ maxWidth: 345 }}>
                    <CardMedia
                        component="img"
                        image="/assets/docs.jpg"
                    />
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="div">
                            {t('glossary.platformCapabilityExamination')}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            {t('glossary.platformCapabilityExaminationDesc')}
                        </Typography>
                    </CardContent>
                </Card>
            </Stack>
            <Typography align='center' variant='h3'>{t('glossary.developers')}</Typography>
            <Stack direction={{ xs: 'column', md: 'row' }} spacing={3} justifyContent="center" alignItems="stretch">

                <Card sx={{ maxWidth: 345 }}>
                    <CardMedia
                        component="img"
                        alt = {t('glossary.developerSobolyev') + ' image'}
                        // image="/assets/communication.jpg"
                    />
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="div">
                            {t('glossary.developerSobolyev')}
                        </Typography>
                    </CardContent>
                </Card>
                <Card sx={{ maxWidth: 345 }}>
                    <CardMedia
                        component="img"
                        alt = {t('glossary.developerFedyshen')+ ' image'}
                        // image="/assets/assessment.jpg"
                    />
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="div">
                            {t('glossary.developerFedyshen')}
                        </Typography>
                    </CardContent>
                </Card>
                <Card sx={{ maxWidth: 345 }}>
                    <CardMedia
                        component="img"
                        alt = {t('glossary.developerRenhach')+ ' image'}
                        //image="/assets/docs.jpg"
                    />
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="div">
                            {t('glossary.developerRenhach')}
                        </Typography>
                        
                    </CardContent>
                </Card>
            </Stack>
        </Stack>
    );
};

export default IntroductionPage;
