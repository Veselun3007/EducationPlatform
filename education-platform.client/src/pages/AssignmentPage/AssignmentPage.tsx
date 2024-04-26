/* eslint-disable @typescript-eslint/no-non-null-assertion */
import React, { useEffect } from 'react';
import {
    Box,
    Button,
    CircularProgress,
    FormControl,
    FormHelperText,
    IconButton,
    InputLabel,
    Menu,
    MenuItem,
    Modal,
    Paper,
    Select,
    Stack,
    TextField,
    Typography,
    useTheme,
} from '@mui/material';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../context/RootStoreContext';
import { useTranslation } from 'react-i18next';
import Grid from '@mui/material/Unstable_Grid2';
import { useNavigate, useParams } from 'react-router-dom';
import { MoreVert } from '@mui/icons-material';
import FilesPicker from '../../components/FilesPicker';

import LinksPicker from '../../components/LinksPicker';
import { DateTimePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';
import LinkCard from '../../components/LinkCard';
import FileCard from '../../components/FileCard';
import DocViewer, { DocViewerRenderers } from '@cyntler/react-doc-viewer';

const AssignmentPage = observer(() => {
    const { assignmentPageStore } = useStore();
    const theme = useTheme();
    const { t, i18n } = useTranslation();
    const navigate = useNavigate();
    const { courseId, assignmentId } = useParams();

    if (isNaN(Number(courseId)) || isNaN(Number(assignmentId))) {
        navigate('/404');
    }

    useEffect(() => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        assignmentPageStore.init(
            Number.parseInt(courseId!),
            Number.parseInt(assignmentId!),
        );
        return () => assignmentPageStore.reset();
    }, []);

    if (assignmentPageStore.isLoading) {
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
    const editString = assignmentPageStore.assignment!.isEdited
        ? ` (${t(
              'glossary.changed',
          )}: ${assignmentPageStore.assignment!.editedTime?.toLocaleDateString(
              i18n.language,
          )})`
        : '';

    return (
        <Grid container mt={2}>
            <Grid xs />
            <Grid container xs={12} md={10} xl={8} spacing={2}>
                <Grid xs={12} lg={8}>
                    <Stack spacing={2}>
                        <Stack
                            borderBottom={2}
                            borderColor={theme.palette.primary.light}
                            width="100%"
                            spacing={1}
                            pb={1}
                        >
                            <Stack direction="row" justifyContent="space-between">
                                <Typography
                                    variant="h5"
                                    color={theme.palette.primary.light}
                                >
                                    {assignmentPageStore.assignment!.assignmentName}
                                </Typography>
                                <IconButton
                                    id="menu-button"
                                    aria-controls={
                                        assignmentPageStore.assignmentMenuAnchor
                                            ? 'assignment-menu'
                                            : undefined
                                    }
                                    aria-haspopup="true"
                                    aria-expanded={
                                        assignmentPageStore.assignmentMenuAnchor
                                            ? 'true'
                                            : undefined
                                    }
                                    onClick={assignmentPageStore.openAssignmentMenu}
                                    sx={{ textAlign: 'end' }}
                                >
                                    <MoreVert />
                                </IconButton>
                                <Menu
                                    elevation={4}
                                    id="assignment-menu"
                                    anchorEl={assignmentPageStore.assignmentMenuAnchor}
                                    open={Boolean(
                                        assignmentPageStore.assignmentMenuAnchor,
                                    )}
                                    onClose={assignmentPageStore.closeAssignmentMenu}
                                    MenuListProps={{
                                        'aria-labelledby': 'menu-button',
                                    }}
                                    anchorOrigin={{
                                        vertical: 'bottom',
                                        horizontal: 'right',
                                    }}
                                    transformOrigin={{
                                        vertical: 'top',
                                        horizontal: 'right',
                                    }}
                                >
                                    <MenuItem
                                        onClick={
                                            assignmentPageStore.handleEditAssignmentOpen
                                        }
                                    >
                                        {t('glossary.editAssignment')}
                                    </MenuItem>
                                    <MenuItem
                                        onClick={() =>
                                            assignmentPageStore.deleteAssignment(
                                                Number.parseInt(courseId!),
                                                navigate,
                                            )
                                        }
                                    >
                                        {t('glossary.deleteAssignment')}
                                    </MenuItem>
                                    <MenuItem
                                        onClick={() => {
                                            assignmentPageStore.closeAssignmentMenu();
                                            navigate(
                                                `/course/${courseId}/assignment/${assignmentId}/mark`,
                                            );
                                        }}
                                    >
                                        {t('glossary.markWorks')}
                                    </MenuItem>
                                </Menu>
                            </Stack>
                            <Typography variant="caption">
                                {assignmentPageStore.assignment!.assignmentDatePublication.toLocaleDateString(
                                    i18n.language,
                                ) + editString}
                            </Typography>
                            <Typography variant="body2" textAlign="end">
                                {`${t(
                                    'common.deadline',
                                )}: ${assignmentPageStore.assignment!.assignmentDeadline.toLocaleDateString(
                                    i18n.language,
                                )}`}
                            </Typography>
                        </Stack>
                        <Typography>
                            {assignmentPageStore.assignment!.assignmentDescription}
                        </Typography>
                        <Grid container>
                            {
                                // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                assignmentPageStore.assignment!.assignmentlinks!.map(
                                    (link) => (
                                        <Grid key={link.id} xs={12} md={6} height={50}>
                                            <LinkCard
                                                link={
                                                    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                                    link.assignmentLink!
                                                }
                                            />
                                        </Grid>
                                    ),
                                )
                            }
                        </Grid>

                        <Grid container>
                            {
                                // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                assignmentPageStore.assignment!.assignmentfiles!.map(
                                    (file, index) => (
                                        <Grid key={file.id} xs={12} md={6} height={50}>
                                            <FileCard
                                                file={
                                                    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                                    file.assignmentFile!
                                                }
                                                onClick={() =>
                                                    assignmentPageStore.onFileClick(index)
                                                }
                                            />
                                        </Grid>
                                    ),
                                )
                            }
                        </Grid>
                    </Stack>
                    <Modal
                        open={assignmentPageStore.isFileViewerOpen}
                        onClose={assignmentPageStore.onFileViewerClose}
                    >
                        <Box
                            maxWidth="80%"
                            maxHeight="90%"
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
                                    visibility: assignmentPageStore.isFileViewerOpen
                                        ? 'visible'
                                        : 'collapse',
                                }}
                                documents={[{ uri: assignmentPageStore.fileLink }]}
                                pluginRenderers={DocViewerRenderers}
                            />
                        </Box>
                    </Modal>
                    <Modal
                        open={assignmentPageStore.editAssignmentOpen}
                        onClose={assignmentPageStore.handleEditAssignmentClose}
                    >
                        <Stack
                            bgcolor={theme.palette.background.paper}
                            width={{ xs: '90%', md: '40%' }}
                            maxHeight={{ xs: '90%' }}
                            sx={{
                                position: 'absolute',
                                top: '50%',
                                left: '50%',
                                transform: 'translate(-50%, -50%)',
                            }}
                            overflow="auto"
                            spacing={{ xs: 1, md: 2 }}
                            p={3}
                        >
                            <Typography textAlign="start" width="100%" variant="h6">
                                {t('glossary.editAssignment')}
                            </Typography>
                            <TextField
                                fullWidth
                                required
                                type="text"
                                label={t('common.name')}
                                value={
                                    assignmentPageStore.editAssignmentData!.assignmentName
                                }
                                onChange={assignmentPageStore.onAssignmentNameChange}
                                error={
                                    assignmentPageStore.editAssignmentErrors
                                        .assignmentName !== null
                                }
                                helperText={
                                    assignmentPageStore.editAssignmentErrors
                                        .assignmentName !== null
                                        ? t(
                                              assignmentPageStore.editAssignmentErrors
                                                  .assignmentName.errorKey,
                                              assignmentPageStore.editAssignmentErrors
                                                  .assignmentName.options,
                                          )
                                        : null
                                }
                            />

                            <TextField
                                fullWidth
                                multiline
                                rows={4}
                                label={t('common.description')}
                                value={
                                    assignmentPageStore.editAssignmentData
                                        ?.assignmentDescription
                                }
                                onChange={
                                    assignmentPageStore.onAssignmentDescriptionChange
                                }
                            />
                            <FormControl fullWidth>
                                <InputLabel id="assignmentTopicSelect">
                                    {t('common.topic')}
                                </InputLabel>
                                <Select
                                    labelId="assignmentTopicSelect"
                                    value={
                                        assignmentPageStore.editAssignmentData?.topicId
                                            ? assignmentPageStore.editAssignmentData?.topicId.toString()
                                            : '0'
                                    }
                                    label={t('common.topic')}
                                    onChange={assignmentPageStore.onAssignmentTopicChange}
                                    fullWidth
                                >
                                    <MenuItem value={0}>{t('glossary.noTopic')}</MenuItem>
                                    {assignmentPageStore.topics?.map((topic) => (
                                        <MenuItem key={topic.id} value={topic.id}>
                                            {topic.title}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                            <FormControl fullWidth>
                                <InputLabel id="assignmentIsRequiredSelect">
                                    {t('common.isRequired')}
                                </InputLabel>
                                <Select
                                    labelId="assignmentIsRequiredSelect"
                                    value={
                                        assignmentPageStore.editAssignmentData?.isRequired
                                            ? '1'
                                            : '0'
                                    }
                                    label={t('common.isRequired')}
                                    onChange={
                                        assignmentPageStore.onAssignmentIsRequiredChange
                                    }
                                    fullWidth
                                >
                                    <MenuItem value={0}>
                                        {t('glossary.notRequired')}
                                    </MenuItem>
                                    <MenuItem value={1}>
                                        {t('glossary.required')}
                                    </MenuItem>
                                </Select>
                            </FormControl>
                            <FormControl fullWidth>
                                <DateTimePicker
                                    sx={{ width: '100%' }}
                                    label={t('common.deadline')}
                                    value={dayjs(
                                        assignmentPageStore.editAssignmentData
                                            ?.assignmentDeadline,
                                    )}
                                    onChange={(newValue) =>
                                        assignmentPageStore.onAssignmentDeadlineChange(
                                            newValue,
                                        )
                                    }
                                    minDateTime={dayjs()}
                                    ampm={false}
                                    disablePast
                                />
                                <FormHelperText error>
                                    {assignmentPageStore.editAssignmentErrors
                                        .assignmentDeadline !== null
                                        ? t(
                                              assignmentPageStore.editAssignmentErrors
                                                  .assignmentDeadline.errorKey,
                                              assignmentPageStore.editAssignmentErrors
                                                  .assignmentDeadline.options,
                                          )
                                        : null}
                                </FormHelperText>
                            </FormControl>
                            <TextField
                                fullWidth
                                required
                                type="number"
                                label={t('common.minMark')}
                                value={assignmentPageStore.editAssignmentData?.minMark}
                                onChange={assignmentPageStore.onAssignmentMinMarkChange}
                                error={
                                    assignmentPageStore.editAssignmentErrors.minMark !==
                                    null
                                }
                                helperText={
                                    assignmentPageStore.editAssignmentErrors.minMark !==
                                    null
                                        ? t(
                                              assignmentPageStore.editAssignmentErrors
                                                  .minMark.errorKey,
                                              assignmentPageStore.editAssignmentErrors
                                                  .minMark.options,
                                          )
                                        : null
                                }
                            />

                            <TextField
                                fullWidth
                                required
                                type="number"
                                label={t('common.maxMark')}
                                value={assignmentPageStore.editAssignmentData?.maxMark}
                                onChange={assignmentPageStore.onAssignmentMaxMarkChange}
                                error={
                                    assignmentPageStore.editAssignmentErrors.maxMark !==
                                    null
                                }
                                helperText={
                                    assignmentPageStore.editAssignmentErrors.maxMark !==
                                    null
                                        ? t(
                                              assignmentPageStore.editAssignmentErrors
                                                  .maxMark.errorKey,
                                              assignmentPageStore.editAssignmentErrors
                                                  .maxMark.options,
                                          )
                                        : null
                                }
                            />
                            <FilesPicker
                                // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                files={
                                    assignmentPageStore.editAssignmentData!
                                        .assignmentFiles
                                }
                                error={
                                    assignmentPageStore.editAssignmentErrors
                                        .assignmentFiles
                                }
                                onFileAdd={assignmentPageStore.onAssignmentFileAdd}
                                onFileDelete={assignmentPageStore.onAssignmentFileDelete}
                            />

                            <LinksPicker
                                // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                links={
                                    assignmentPageStore.editAssignmentData!
                                        .assignmentLinks
                                }
                                error={
                                    assignmentPageStore.editAssignmentErrors
                                        .assignmentLinks
                                }
                                onLinkAdd={assignmentPageStore.onAssignmentLinkAdd}
                                onLinkDelete={assignmentPageStore.onAssignmentLinkDelete}
                            />
                            <Typography
                                color="error"
                                variant="caption"
                                align="center"
                                visibility={
                                    assignmentPageStore.editAssignmentErrors.meta !== null
                                        ? 'visible'
                                        : 'collapse'
                                }
                            >
                                {assignmentPageStore.editAssignmentErrors.meta !== null
                                    ? t(
                                          assignmentPageStore.editAssignmentErrors.meta
                                              .errorKey,
                                          assignmentPageStore.editAssignmentErrors.meta
                                              .options,
                                      )
                                    : null}
                            </Typography>
                            <Stack direction="row" width="100%" justifyContent="end">
                                <Button
                                    color="inherit"
                                    onClick={
                                        assignmentPageStore.handleEditAssignmentClose
                                    }
                                >
                                    {t('common.close')}
                                </Button>
                                <Button
                                    color="primary"
                                    onClick={assignmentPageStore.submitEditAssignment}
                                    disabled={!assignmentPageStore.isEditAssignmentValid}
                                >
                                    {t('common.edit')}
                                </Button>
                            </Stack>
                        </Stack>
                    </Modal>
                </Grid>
                <Grid xs={12} lg={4}>
                    <Paper sx={{ p: 2 }}>
                        <Stack direction="column" spacing={2}>
                            <Typography variant="h6">{t('glossary.myWork')}</Typography>

                            <FilesPicker
                                onFileAdd={assignmentPageStore.onWorkFileAdd}
                                fullWidth={true}
                                onFileDelete={assignmentPageStore.onWorkFileDelete}
                                files={assignmentPageStore.work.workFiles}
                                error={assignmentPageStore.workErrors.workFiles}
                            />
                            <Button
                                variant="outlined"
                                disabled={!assignmentPageStore.isWorkValid}
                                fullWidth
                                sx={{ height: 50 }}
                            >
                                {t('common.send')}
                            </Button>
                        </Stack>
                    </Paper>
                </Grid>
            </Grid>
            <Grid xs />
        </Grid>
    );
});

export default AssignmentPage;
