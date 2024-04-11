import React from 'react';
import { useTranslation } from 'react-i18next';
import { locales } from '../../i18n';
import {
    Button,
    Card,
    CardContent,
    CardMedia,
    IconButton,
    Stack,
    ToggleButton,
    ToggleButtonGroup,
    Typography,
} from '@mui/material';
import './IntroductionPage.css';
import HoverImage from '../../components/HoverImage/HoverImage';
import GitHubIcon from '@mui/icons-material/GitHub';
import LinkedInIcon from '@mui/icons-material/LinkedIn';
import { useNavigate } from 'react-router-dom';

const IntroductionPage = () => {
    const { t, i18n } = useTranslation();
    const navigate = useNavigate();

    const handleLanguageChange = (
        event: React.MouseEvent<HTMLElement>,
        newLanguage: string,
    ) => {
        i18n.changeLanguage(newLanguage);
    };

    let actionButtons;
    if (localStorage.getItem('accessToken')) {
        actionButtons = (
            <Button
                size="large"
                variant="contained"
                color="secondary"
                onClick={() => {
                    navigate('/dashboard');
                }}
            >
                {t('common.dashboard')}
            </Button>
        );
    } else {
        actionButtons = (
            <>
                <Button
                    size="large"
                    variant="contained"
                    color="secondary"
                    onClick={() => {
                        navigate('/signup');
                    }}
                >
                    {t('common.signUp')}
                </Button>
                <Button
                    size="large"
                    variant="contained"
                    onClick={() => {
                        navigate('/login');
                    }}
                >
                    {t('common.login')}
                </Button>
            </>
        );
    }

    return (
        <Stack
            direction="column"
            spacing={5}
            mb={5}
            justifyContent="center"
            alignItems="stretch"
        >
            <div className="gradient-bg">
                <Stack className="content" spacing={{ xs: 4, sm: 3, md: 2 }} useFlexGap>
                    <Typography align="center" variant="h2" fontSize={58}>
                        {t('glossary.introductionTitle')}
                    </Typography>
                    <Typography align="center" variant="subtitle1" fontSize={18}>
                        {t('glossary.introductionSubtitle')}
                    </Typography>
                    <Stack direction="row" spacing={5} mt={3}>
                        {actionButtons}
                    </Stack>
                </Stack>
                <div className="topBar">
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

            <Typography align="center" variant="h3">
                {t('glossary.platformCapabilities')}
            </Typography>
            <Stack
                direction={{ xs: 'column', md: 'row' }}
                spacing={3}
                justifyContent="center"
                alignItems="stretch"
                alignContent="center"
            >
                <Card sx={{ maxWidth: 345, borderRadius: '5%' }}>
                    <CardMedia component="img" image="/assets/communication.jpg" />
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="div">
                            {t('glossary.platformCapabilityCommunication')}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            {t('glossary.platformCapabilityCommunicationDesc')}
                        </Typography>
                    </CardContent>
                </Card>
                <Card sx={{ maxWidth: 345, borderRadius: '5%' }}>
                    <CardMedia component="img" image="/assets/assessment.jpg" />
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="div">
                            {t('glossary.platformCapabilityAssessment')}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            {t('glossary.platformCapabilityAssessmentDesc')}
                        </Typography>
                    </CardContent>
                </Card>
                <Card sx={{ maxWidth: 345, borderRadius: '5%' }}>
                    <CardMedia component="img" image="/assets/docs.jpg" />
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
            <Typography align="center" variant="h3">
                {t('glossary.developers')}
            </Typography>
            <Stack
                direction={{ xs: 'column', md: 'row' }}
                spacing={3}
                justifyContent="center"
                alignItems="stretch"
            >
                <HoverImage
                    imageSrc="/assets/Sobolyev.jpg"
                    imageAlt={t('glossary.developerSobolyev')}
                    backgroundColor="primary.main"
                    sx={{ maxWidth: 345 }}
                >
                    <Stack direction="column" justifyContent="center" alignItems="center">
                        <Typography variant="h5">
                            {t('glossary.developerSobolyev')}
                        </Typography>
                        <Typography variant="subtitle1" fontSize={18}>
                            {t('glossary.developerSobolyevRole')}
                        </Typography>
                        <Stack direction="row">
                            <IconButton href="https://github.com/DanSoboliev">
                                <GitHubIcon />
                            </IconButton>
                            <IconButton href="https://www.linkedin.com/in/dan-sobolev-57643124b/">
                                <LinkedInIcon />
                            </IconButton>
                        </Stack>
                    </Stack>
                </HoverImage>
                <HoverImage
                    imageSrc="/assets/Fedyshen.jpg"
                    imageAlt={t('glossary.developerFefyshen')}
                    backgroundColor="primary.main"
                    sx={{ maxWidth: 345 }}
                >
                    <Stack direction="column" justifyContent="center" alignItems="center">
                        <Typography variant="h5">
                            {t('glossary.developerFedyshen')}
                        </Typography>
                        <Typography variant="subtitle1" fontSize={18}>
                            {t('glossary.developerFedyshenRole')}
                        </Typography>
                        <Stack direction="row">
                            <IconButton href="https://github.com/Veselun3007">
                                <GitHubIcon />
                            </IconButton>
                            <IconButton href="https://www.linkedin.com/in/bohdan-fedyshen-74554a24b/">
                                <LinkedInIcon />
                            </IconButton>
                        </Stack>
                    </Stack>
                </HoverImage>
                <HoverImage
                    imageSrc="/assets/Renhach.jpg"
                    imageAlt={t('glossary.developerRenhach')}
                    backgroundColor="primary.main"
                    sx={{ maxWidth: 345 }}
                >
                    <Stack direction="column" justifyContent="center" alignItems="center">
                        <Typography variant="h5">
                            {t('glossary.developerRenhach')}
                        </Typography>
                        <Typography variant="subtitle1" fontSize={18}>
                            {t('glossary.developerRenhachRole')}
                        </Typography>
                        <Stack direction="row">
                            <IconButton href="https://github.com/Right9lt">
                                <GitHubIcon />
                            </IconButton>
                            <IconButton href="https://www.linkedin.com/in/valentyn-renhach-a45952245/">
                                <LinkedInIcon />
                            </IconButton>
                        </Stack>
                    </Stack>
                </HoverImage>
            </Stack>
        </Stack>
    );
};

export default IntroductionPage;
