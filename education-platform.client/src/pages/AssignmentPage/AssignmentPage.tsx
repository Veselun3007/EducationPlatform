/* eslint-disable @typescript-eslint/no-non-null-assertion */
import React, { useEffect } from 'react';
import {
    Avatar,
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
    Skeleton,
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
import { Assessment, AttachFile, Delete, Link, MoreVert } from '@mui/icons-material';
import FilesPicker from '../../components/FilesPicker';

import LinksPicker from '../../components/LinksPicker';
import { DateTimePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';
import LinkCard from '../../components/LinkCard';
import FileCard from '../../components/FileCard';
import DocViewer, { DocViewerRenderers } from '@cyntler/react-doc-viewer';
import ValidationError from '../../helpers/validation/ValidationError';

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
            navigate
        );
        return () => assignmentPageStore.reset();
    }, [courseId, assignmentId]);

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
        )}: ${assignmentPageStore.assignment!.editedTime?.toLocaleString(
            i18n.language,
        )})`
        : '';

    return (
        <Grid container mt={2}>
            <Grid xs />
            <Grid container xs={12} md={10} xl={8} spacing={2}>
                <Grid xs={12} lg={!assignmentPageStore.isTeacher? 8: 12}>
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
                                {assignmentPageStore.isTeacher && <><IconButton
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
                                    </Menu></>}
                            </Stack>
                            <Typography variant="caption">
                                {assignmentPageStore.assignment!.assignmentDatePublication.toLocaleString(
                                    i18n.language,
                                ) + editString}
                            </Typography>
                            <Typography variant="body2" textAlign="end">
                                {`${t(
                                    'common.deadline',
                                )}: ${assignmentPageStore.assignment!.assignmentDeadline.toLocaleString(
                                    i18n.language,
                                )}`}
                            </Typography>
                        </Stack>
                        <Typography>
                            {assignmentPageStore.assignment!.assignmentDescription}
                        </Typography>
                        <Grid container spacing={1}>
                            {
                                // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                assignmentPageStore.assignment!.assignmentlinks!.map(
                                    (link) => (
                                        <Grid key={link.id} xs={12} md={6} height={55}>
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

                        <Grid container spacing={1}>
                            {
                                // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                assignmentPageStore.assignment!.assignmentfiles!.map(
                                    (file) => (
                                        <Grid key={file.id} xs={12} md={6} height={55}>
                                            <FileCard
                                                file={
                                                    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                                    file.assignmentFile!.slice(file.assignmentFile!.indexOf('_') + 1)
                                                }
                                                onClick={() =>
                                                    assignmentPageStore.onFileClick(file.id, navigate)
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
                                    visibility: assignmentPageStore.isFileViewerOpen
                                        ? 'visible'
                                        : 'collapse',
                                }}
                                documents={[{ uri: assignmentPageStore.fileLink }]}
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
                                    onClick={() => assignmentPageStore.submitEditAssignment(navigate)}
                                    disabled={!assignmentPageStore.isEditAssignmentValid}
                                >
                                    {t('common.edit')}
                                </Button>
                            </Stack>
                            <Typography textAlign="start" width="100%" variant="h6">
                                {t('glossary.editFiles')}
                            </Typography>
                            <Grid container spacing={1}>
                                {assignmentPageStore.assignment!.assignmentfiles!.map((file) => (
                                    <Grid key={file.id} xs={12}>
                                        <Paper variant="outlined" sx={{ overflow: 'hidden' }}>
                                            <Stack
                                                direction="row"
                                                height={50}
                                                alignItems="center"
                                                spacing={1}
                                                overflow="hidden"
                                                pr={1}
                                            >
                                                <Avatar
                                                    sx={{
                                                        height: '100%',
                                                        bgcolor: theme.palette.primary.main,
                                                    }}
                                                    variant="square"
                                                >
                                                    <AttachFile />
                                                </Avatar>
                                                <Typography
                                                    textAlign="left"
                                                    sx={{
                                                        display: '-webkit-box',
                                                        overflow: 'hidden',
                                                        WebkitBoxOrient: 'vertical',
                                                        WebkitLineClamp: 1,
                                                        textOverflow: 'ellipsis',
                                                        whiteSpace: 'normal',
                                                    }}
                                                    flexGrow={1}
                                                >
                                                    {file.assignmentFile!.slice(file.assignmentFile!.indexOf('_') + 1)}
                                                </Typography>
                                                <IconButton onClick={() => assignmentPageStore.onAssignmentFileDelete(file.id, navigate)}>
                                                    <Delete />
                                                </IconButton>
                                            </Stack>
                                        </Paper>
                                    </Grid>
                                ))}
                                <Grid xs={12}>
                                    <Button
                                        sx={{ height: 50 }}
                                        variant="contained"
                                        component="label"
                                        color="secondary"
                                        startIcon={<AttachFile />}
                                    >
                                        {t('common.addFile')}
                                        <input type="file" hidden onChange={(e) => assignmentPageStore.onAssignmentFileAdd(e, navigate)} />
                                    </Button>
                                </Grid>
                            </Grid>
                            <Typography textAlign="start" width="100%" variant="h6">
                                {t('glossary.editLinks')}
                            </Typography>

                            <Stack direction="column" width="100%">
                                <Grid container spacing={1}>
                                    {assignmentPageStore.assignment!.assignmentlinks!.map((link) => (
                                        <Grid key={link.id} xs={12}>
                                            <Paper variant="outlined" sx={{ overflow: 'hidden' }}>
                                                <Stack
                                                    direction="row"
                                                    pr={1}
                                                    height={50}
                                                    alignItems="center"
                                                    spacing={1}
                                                >
                                                    <Avatar
                                                        sx={{
                                                            height: '100%',
                                                            bgcolor: theme.palette.primary.main,
                                                        }}
                                                        variant="square"
                                                    >
                                                        <Link />
                                                    </Avatar>
                                                    <Typography
                                                        textAlign="left"
                                                        sx={{
                                                            display: '-webkit-box',
                                                            overflow: 'hidden',
                                                            WebkitBoxOrient: 'vertical',
                                                            WebkitLineClamp: 1,
                                                            textOverflow: 'ellipsis',
                                                            whiteSpace: 'normal',
                                                        }}
                                                        flexGrow={1}
                                                    >
                                                        {link.assignmentLink}
                                                    </Typography>
                                                    <IconButton onClick={() => assignmentPageStore.onAssignmentLinkDelete(link.id, navigate)}>
                                                        <Delete />
                                                    </IconButton>
                                                </Stack>
                                            </Paper>
                                        </Grid>
                                    ))}
                                    <Grid xs={12}>
                                        <Button
                                            sx={{ height: 50 }}
                                            variant="contained"
                                            component="label"
                                            color="secondary"
                                            startIcon={<Link />}
                                            onClick={assignmentPageStore.handleLinkAddOpen}
                                        >
                                            {t('common.addLink')}
                                        </Button>
                                    </Grid>
                                </Grid>
                            </Stack>
                            <Modal
                                open={assignmentPageStore.isLinkAddOpen}
                                onClose={assignmentPageStore.handleLinkAddClose}
                                sx={{ height: '100%' }}
                            >
                                <Stack
                                    bgcolor={theme.palette.background.paper}
                                    width={{ xs: '90%', md: '40%' }}
                                    height="fit-content"
                                    sx={{
                                        position: 'absolute',
                                        top: '50%',
                                        left: '50%',
                                        transform: 'translate(-50%, -50%)',
                                    }}
                                    overflow="auto"
                                    justifyContent="center"
                                    spacing={{ xs: 1, md: 2 }}
                                    p={3}
                                    alignItems="center"
                                >
                                    <Typography textAlign="start" width="100%" variant="h6">
                                        {t('common.addLink')}
                                    </Typography>
                                    <TextField
                                        fullWidth
                                        required
                                        type="text"
                                        label={t('common.link')}
                                        value={assignmentPageStore.linkAdd}
                                        onChange={assignmentPageStore.onLinkAddChange}
                                    />
                                    <Stack direction="row" width="100%" justifyContent="end">
                                        <Button color="inherit" onClick={assignmentPageStore.handleLinkAddClose}>
                                            {t('common.close')}
                                        </Button>
                                        <Button
                                            color="primary"
                                            onClick={() => assignmentPageStore.onAssignmentLinkAdd(navigate)}
                                        >
                                            {t('common.add')}
                                        </Button>
                                    </Stack>
                                </Stack>
                            </Modal>
                        </Stack>
                    </Modal>
                </Grid>
                {!assignmentPageStore.isTeacher && <Grid xs={12} lg={4}>
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
                </Grid>}
            </Grid>
            <Grid xs />
        </Grid>
    );
});

export default AssignmentPage;
