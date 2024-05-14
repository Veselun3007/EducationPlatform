/* eslint-disable @typescript-eslint/no-non-null-assertion */
import React, { useEffect } from 'react';
import { Box, CircularProgress, Modal, Stack, Typography, useTheme } from '@mui/material';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../context/RootStoreContext';
import { useTranslation } from 'react-i18next';
import Grid from '@mui/material/Unstable_Grid2';
import { useNavigate, useParams } from 'react-router-dom';
import DocViewer, { DocViewerRenderers } from '@cyntler/react-doc-viewer';
import MarkCard from '../../components/MarkCard';

const MarkWorksPage = observer(() => {
    const { markWorksPageStore } = useStore();
    const theme = useTheme();
    const { t } = useTranslation();
    const navigate = useNavigate();
    const { courseId, assignmentId } = useParams();

    if (isNaN(Number(courseId))) {
        navigate('/404');
    }

    useEffect(() => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        markWorksPageStore.init(
            Number.parseInt(courseId!),
            Number.parseInt(assignmentId!),
            navigate,
        );
        return () => markWorksPageStore.reset();
    }, [courseId, assignmentId]);

    if (markWorksPageStore.isLoading) {
        return (
            <Box
                display="flex"
                height="100svh"
                width="100%"
                alignItems="center"
                justifyContent="center"
            >
                <CircularProgress />
            </Box>
        );
    }
    return (
        <Grid container mt={2}>
            <Grid xs />
            <Grid xs={12} md={9} xl={6}>
                <Stack spacing={2}>
                    <Typography variant="h5">
                        {t('glossary.assignmentGrade', {
                            name: markWorksPageStore.assignment!.assignmentName,
                        })}
                    </Typography>
                    {markWorksPageStore.saInfo!.map((info) => (
                        <MarkCard
                            key={info.studentAssignment.studentassignmentId}
                            assignment={markWorksPageStore.assignment!}
                            saInfo={info}
                            onSubmitMark={markWorksPageStore.submitMark}
                            sendComment={markWorksPageStore.sendComment}
                            currentUser={markWorksPageStore.currentUser!}
                            onFileClick={markWorksPageStore.onWorkFileClick}
                        />
                    ))}
                </Stack>
            </Grid>
            <Grid xs />

            <Modal
                open={markWorksPageStore.isFileViewerOpen}
                onClose={markWorksPageStore.onFileViewerClose}
            >
                <Box
                    width="80%"
                    height="90%"
                    sx={{
                        position: 'absolute',
                        top: '50%',
                        left: '50%',
                        transform: 'translate(-50%, -50%)',
                    }}
                    overflow="auto"
                >
                    <DocViewer
                        prefetchMethod="GET"
                        style={{
                            visibility: markWorksPageStore.isFileViewerOpen
                                ? 'visible'
                                : 'collapse',
                        }}
                        documents={[{ uri: markWorksPageStore.fileLink }]}
                        pluginRenderers={DocViewerRenderers}
                        theme={{
                            primary: theme.palette.background.default,
                            secondary: theme.palette.secondary.main,
                            textPrimary: theme.palette.text.primary,
                            textSecondary: theme.palette.text.secondary,
                        }}
                    />
                </Box>
            </Modal>
        </Grid>
    );
});

export default MarkWorksPage;
