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
    Select,
    Stack,
    TextField,
    Typography,
    useTheme,
} from '@mui/material';
import { observer } from 'mobx-react-lite';
import AbstractBackground from '../../components/AbstractBackground';
import { useStore } from '../../context/RootStoreContext';
import { useTranslation } from 'react-i18next';
import Grid from '@mui/material/Unstable_Grid2';
import { useNavigate } from 'react-router-dom';
import { Add, ContentCopy, MoreVert } from '@mui/icons-material';
import { enqueueAlert } from '../../components/Notification/NotificationProvider';
import FilesPicker from '../../components/FilesPicker';

import LinksPicker from '../../components/LinksPicker';
import { DateTimePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';

const CoursePage = observer(() => {
    const { coursePageStore } = useStore();
    const theme = useTheme();
    const { t } = useTranslation();
    const navigate = useNavigate();

    useEffect(() => {
        coursePageStore.getCourse();
        return () => coursePageStore.reset();
    }, []);

    if (coursePageStore.course === null ||
        coursePageStore.courseData === null ||
        coursePageStore.topicData === null

    ) {
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
        )
    }

    return (
        <Grid container>
            <Grid xs />
            <Grid xs={12} md={10} xl={8}>
                <Stack spacing={2}>
                    <AbstractBackground
                        value={coursePageStore.course.courseName}
                        sx={{ width: '100%', height: '15rem', borderRadius: 3 }}
                    >
                        <Grid container width="100%" sx={{ display: 'flex', flexDirection: 'column', alignItems: 'stretch' }}>
                            <Grid xs={12} flexGrow={1} textAlign="end">
                                <IconButton
                                    id="menu-button"

                                    aria-controls={coursePageStore.courseMenuAnchor ? 'course-menu' : undefined}
                                    aria-haspopup="true"
                                    aria-expanded={coursePageStore.courseMenuAnchor ? 'true' : undefined}
                                    onClick={coursePageStore.openCourseMenu}
                                    sx={{ textAlign: 'end' }}
                                >
                                    <MoreVert />
                                </IconButton>
                                <Menu
                                    elevation={4}
                                    id="course-menu"
                                    anchorEl={coursePageStore.courseMenuAnchor}
                                    open={Boolean(coursePageStore.courseMenuAnchor)}
                                    onClose={coursePageStore.closeCourseMenu}
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
                                    <MenuItem onClick={coursePageStore.handleEditCourseOpen}>{t('glossary.editCourse')}</MenuItem>
                                    <MenuItem onClick={() => coursePageStore.deleteCourse(navigate)}>{t('glossary.deleteCourse')}</MenuItem>
                                </Menu>
                            </Grid>
                            <Grid xs={12} flexGrow={0}>
                                <Stack justifyContent="center" p={2} spacing={1} width="100%">

                                    <Typography
                                        variant="h5"
                                        textAlign="left"
                                        sx={{
                                            display: '-webkit-box',
                                            overflow: 'hidden',
                                            WebkitBoxOrient: 'vertical',
                                            WebkitLineClamp: 1,
                                            textOverflow: 'ellipsis',
                                            whiteSpace: 'normal',
                                        }}
                                    >
                                        {coursePageStore.course.courseName}
                                    </Typography>
                                    <Stack direction="row" spacing={1} alignItems="center">
                                        <Typography>
                                            {t('glossary.joinLink')}
                                        </Typography>
                                        <Typography sx={{ fontStyle: 'italic' }}>
                                            {coursePageStore.course.courseLink}
                                        </Typography>
                                        <IconButton onClick={() => {
                                            enqueueAlert('glossary.copiedToClipboard', 'success')
                                            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                            navigator.clipboard.writeText(coursePageStore.course!.courseLink)
                                        }}>
                                            <ContentCopy />
                                        </IconButton>
                                    </Stack>
                                </Stack>
                            </Grid>
                            <Grid xs={12} flexGrow={1}>

                            </Grid>
                        </Grid>
                    </AbstractBackground>
                    <Button
                        id="contentMenu-button"

                        aria-controls={coursePageStore.contentMenuAnchor ? 'content-menu' : undefined}
                        aria-haspopup="true"
                        aria-expanded={coursePageStore.contentMenuAnchor ? 'true' : undefined}
                        onClick={coursePageStore.openContentMenu}
                        sx={{ width: 'fit-content' }}
                        startIcon={<Add />}
                        variant="contained"
                    >
                        {t('common.create')}
                    </Button>
                    <Menu
                        elevation={4}
                        id="content-menu"
                        anchorEl={coursePageStore.contentMenuAnchor}
                        open={Boolean(coursePageStore.contentMenuAnchor)}
                        onClose={coursePageStore.closeContentMenu}
                        MenuListProps={{
                            'aria-labelledby': 'contentMenu-button',
                        }}
                        anchorOrigin={{
                            vertical: 'bottom',
                            horizontal: 'center',
                        }}
                        transformOrigin={{
                            vertical: 'top',
                            horizontal: 'center',
                        }}
                    >
                        <MenuItem onClick={coursePageStore.handleCreateTopicOpen}>{t('glossary.topic')}</MenuItem>
                        <MenuItem onClick={coursePageStore.handleCreateMaterialOpen}>{t('glossary.material')}</MenuItem>
                        <MenuItem onClick={coursePageStore.handleCreateAssignmentOpen}>{t('glossary.assignment')}</MenuItem>
                    </Menu>
                </Stack>
                <Modal
                    open={coursePageStore.editCourseOpen}
                    onClose={coursePageStore.handleEditCourseClose}
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
                            {t('glossary.editCourse')}
                        </Typography>
                        <TextField
                            fullWidth
                            required
                            type="text"
                            label={t('common.name')}
                            value={coursePageStore.courseData.name}
                            onChange={coursePageStore.onNameChange}
                            error={coursePageStore.courseErrors.name !== null}
                            helperText={
                                coursePageStore.courseErrors.name !== null
                                    ? t(
                                        coursePageStore.courseErrors.name.errorKey,
                                        coursePageStore.courseErrors.name.options,
                                    )
                                    : null
                            }
                        />
                        <TextField
                            fullWidth
                            multiline
                            rows={4}
                            label={t('common.description')}
                            value={coursePageStore.courseData.description}
                            onChange={coursePageStore.onDescriptionChange}
                        />
                        <Typography
                            color="error"
                            variant="caption"
                            align="center"
                            visibility={
                                coursePageStore.courseErrors.meta !== null
                                    ? 'visible'
                                    : 'collapse'
                            }
                        >
                            {coursePageStore.courseErrors.meta !== null
                                ? t(
                                    coursePageStore.courseErrors.meta.errorKey,
                                    coursePageStore.courseErrors.meta.options,
                                )
                                : null}
                        </Typography>
                        <Stack direction="row" width="100%" justifyContent="end">
                            <Button
                                color="inherit"
                                onClick={coursePageStore.handleEditCourseClose}
                            >
                                {t('common.close')}
                            </Button>
                            <Button
                                color="primary"
                                onClick={coursePageStore.submitCourse}
                                disabled={!coursePageStore.isCourseValid}
                            >
                                {t('common.edit')}
                            </Button>
                        </Stack>
                    </Stack>
                </Modal>
                <Modal
                    open={coursePageStore.createTopicOpen}
                    onClose={coursePageStore.handleCreateTopicClose}
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
                            {t('glossary.createTopic')}
                        </Typography>
                        <TextField
                            fullWidth
                            required
                            type="text"
                            label={t('common.title')}
                            value={coursePageStore.topicData?.title}
                            onChange={coursePageStore.onTitleChange}
                            error={coursePageStore.topicErrors.title !== null}
                            helperText={
                                coursePageStore.topicErrors.title !== null
                                    ? t(
                                        coursePageStore.topicErrors.title.errorKey,
                                        coursePageStore.topicErrors.title.options,
                                    )
                                    : null
                            }
                        />

                        <Typography
                            color="error"
                            variant="caption"
                            align="center"
                            visibility={
                                coursePageStore.topicErrors.meta !== null
                                    ? 'visible'
                                    : 'collapse'
                            }
                        >
                            {coursePageStore.topicErrors.meta !== null
                                ? t(
                                    coursePageStore.topicErrors.meta.errorKey,
                                    coursePageStore.topicErrors.meta.options,
                                )
                                : null}
                        </Typography>
                        <Stack direction="row" width="100%" justifyContent="end">
                            <Button
                                color="inherit"
                                onClick={coursePageStore.handleCreateTopicClose}
                            >
                                {t('common.close')}
                            </Button>
                            <Button
                                color="primary"
                                onClick={coursePageStore.submitTopic}
                                disabled={!coursePageStore.isTopicValid}
                            >
                                {t('common.create')}
                            </Button>
                        </Stack>
                    </Stack>
                </Modal>

                <Modal
                    open={coursePageStore.createMaterialOpen}
                    onClose={coursePageStore.handleCreateMaterialClose}
                >
                    <Stack
                        bgcolor={theme.palette.background.paper}
                        width={{ xs: '90%', md: '40%' }}
                        maxHeight={{ xs: '90%'}}
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
                            {t('glossary.createMaterial')}
                        </Typography>
                        <TextField
                            fullWidth
                            required
                            type="text"
                            label={t('common.name')}
                            value={coursePageStore.materialData?.materialName}
                            onChange={coursePageStore.onMaterialNameChange}
                            error={coursePageStore.materialErrors.materialName !== null}
                            helperText={
                                coursePageStore.materialErrors.materialName !== null
                                    ? t(
                                        coursePageStore.materialErrors.materialName.errorKey,
                                        coursePageStore.materialErrors.materialName.options,
                                    )
                                    : null
                            }
                        />

                        <TextField
                            fullWidth
                            multiline
                            rows={4}
                            label={t('common.description')}
                            value={coursePageStore.materialData?.materialDescription}
                            onChange={coursePageStore.onMaterialDescriptionChange}
                        />
                        <FormControl fullWidth>
                            <InputLabel id="materialTopicSelect">{t('common.topic')}</InputLabel>
                            <Select
                                labelId="materialTopicSelect"

                                value={coursePageStore.materialData?.topicId ? coursePageStore.materialData?.topicId.toString() : "0"}
                                label={t('common.topic')}
                                onChange={coursePageStore.onMaterialTopicChange}
                                fullWidth
                            >
                                <MenuItem value={0}>{t('glossary.noTopic')}</MenuItem>
                                {coursePageStore.topics?.map((topic) => (
                                    <MenuItem key={topic.id} value={topic.id}>{topic.title}</MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                        <FilesPicker
                            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                            files={coursePageStore.materialData!.materialFiles}
                            error={coursePageStore.materialErrors.materialFiles}
                            onFileAdd={coursePageStore.onMaterialFileAdd}
                            onFileDelete={coursePageStore.onMaterialFileDelete}
                        />

                        <LinksPicker
                            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                            links={coursePageStore.materialData!.materialLinks}
                            error={coursePageStore.materialErrors.materialLinks}
                            onLinkAdd={coursePageStore.onMaterialLinkAdd}
                            onLinkDelete={coursePageStore.onMaterialLinkDelete} />
                        <Typography
                            color="error"
                            variant="caption"
                            align="center"
                            visibility={
                                coursePageStore.materialErrors.meta !== null
                                    ? 'visible'
                                    : 'collapse'
                            }
                        >
                            {coursePageStore.materialErrors.meta !== null
                                ? t(
                                    coursePageStore.materialErrors.meta.errorKey,
                                    coursePageStore.materialErrors.meta.options,
                                )
                                : null}
                        </Typography>
                        <Stack direction="row" width="100%" justifyContent="end">
                            <Button
                                color="inherit"
                                onClick={coursePageStore.handleCreateMaterialClose}
                            >
                                {t('common.close')}
                            </Button>
                            <Button
                                color="primary"
                                onClick={coursePageStore.submitMaterial}
                                disabled={!coursePageStore.isMaterialValid}
                            >
                                {t('common.create')}
                            </Button>
                        </Stack>
                    </Stack>
                </Modal>

                <Modal
                    open={coursePageStore.createAssignmentOpen}
                    onClose={coursePageStore.handleCreateAssignmentClose}
                >   
                    <Stack
                        bgcolor={theme.palette.background.paper}
                        width={{ xs: '90%', md: '40%' }}
                        maxHeight={{ xs: '90%'}}
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
                            {t('glossary.createAssignment')}
                        </Typography>
                        <TextField
                            fullWidth
                            required
                            type="text"
                            label={t('common.name')}
                            value={coursePageStore.assignmentData?.assignmentName}
                            onChange={coursePageStore.onAssignmentNameChange}
                            error={coursePageStore.assignmentErrors.assignmentName !== null}
                            helperText={
                                coursePageStore.assignmentErrors.assignmentName !== null
                                    ? t(
                                        coursePageStore.assignmentErrors.assignmentName.errorKey,
                                        coursePageStore.assignmentErrors.assignmentName.options,
                                    )
                                    : null
                            }
                        />

                        <TextField
                            fullWidth
                            multiline
                            rows={4}
                            label={t('common.description')}
                            value={coursePageStore.assignmentData?.assignmentDescription}
                            onChange={coursePageStore.onAssignmentDescriptionChange}
                        />
                        <FormControl fullWidth>
                            <InputLabel id="assignmentTopicSelect">{t('common.topic')}</InputLabel>
                            <Select
                                labelId="assignmentTopicSelect"

                                value={coursePageStore.assignmentData?.topicId ? coursePageStore.assignmentData?.topicId.toString() : "0"}
                                label={t('common.topic')}
                                onChange={coursePageStore.onAssignmentTopicChange}
                                fullWidth
                            >
                                <MenuItem value={0}>{t('glossary.noTopic')}</MenuItem>
                                {coursePageStore.topics?.map((topic) => (
                                    <MenuItem key={topic.id} value={topic.id}>{topic.title}</MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                        <FormControl fullWidth>
                            <InputLabel id="assignmentIsRequiredSelect">{t('common.isRequired')}</InputLabel>
                            <Select
                                labelId="assignmentIsRequiredSelect"

                                value={coursePageStore.assignmentData?.isRequired ? "1":"0"}
                                label={t('common.isRequired')}
                                onChange={coursePageStore.onAssignmentIsRequiredChange}
                                fullWidth
                            >
                                <MenuItem value={0}>{t('glossary.notRequired')}</MenuItem>
                                <MenuItem value={1}>{t('glossary.required')}</MenuItem>
                            </Select>
                        </FormControl>
                        <FormControl fullWidth>
                            <DateTimePicker
                                sx={{ width: '100%' }}
                                label={t('common.deadline')}
                                value={dayjs(coursePageStore.assignmentData?.assignmentDeadline)}
                                onChange={(newValue) => coursePageStore.onAssignmentDeadlineChange(newValue)}
                                minDateTime={dayjs()}
                                ampm={false}
                                disablePast
                            />
                            <FormHelperText error>
                                {coursePageStore.assignmentErrors.assignmentDeadline !== null
                                    ? t(
                                        coursePageStore.assignmentErrors.assignmentDeadline.errorKey,
                                        coursePageStore.assignmentErrors.assignmentDeadline.options,
                                    )
                                    : null
                                }
                            </FormHelperText>
                        </FormControl>
                        <TextField
                            fullWidth
                            required
                            type="number"
                            label={t('common.minMark')}
                            value={coursePageStore.assignmentData?.minMark}
                            onChange={coursePageStore.onAssignmentMinMarkChange}
                            error={coursePageStore.assignmentErrors.minMark !== null}
                            helperText={
                                coursePageStore.assignmentErrors.minMark !== null
                                    ? t(
                                        coursePageStore.assignmentErrors.minMark.errorKey,
                                        coursePageStore.assignmentErrors.minMark.options,
                                    )
                                    : null
                            }
                        />

                        <TextField
                            fullWidth
                            required
                            type="number"
                            label={t('common.maxMark')}
                            value={coursePageStore.assignmentData?.maxMark}
                            onChange={coursePageStore.onAssignmentMaxMarkChange}
                            error={coursePageStore.assignmentErrors.maxMark !== null}
                            helperText={
                                coursePageStore.assignmentErrors.maxMark !== null
                                    ? t(
                                        coursePageStore.assignmentErrors.maxMark.errorKey,
                                        coursePageStore.assignmentErrors.maxMark.options,
                                    )
                                    : null
                            }
                        />
                        <FilesPicker
                            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                            files={coursePageStore.assignmentData!.assignmentFiles}
                            error={coursePageStore.assignmentErrors.assignmentFiles}
                            onFileAdd={coursePageStore.onAssignmentFileAdd}
                            onFileDelete={coursePageStore.onAssignmentFileDelete}
                        />

                        <LinksPicker
                            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                            links={coursePageStore.assignmentData!.assignmentLinks}
                            error={coursePageStore.assignmentErrors.assignmentLinks}
                            onLinkAdd={coursePageStore.onAssignmentLinkAdd}
                            onLinkDelete={coursePageStore.onAssignmentLinkDelete} />
                        <Typography
                            color="error"
                            variant="caption"
                            align="center"
                            visibility={
                                coursePageStore.assignmentErrors.meta !== null
                                    ? 'visible'
                                    : 'collapse'
                            }
                        >
                            {coursePageStore.assignmentErrors.meta !== null
                                ? t(
                                    coursePageStore.assignmentErrors.meta.errorKey,
                                    coursePageStore.assignmentErrors.meta.options,
                                )
                                : null}
                        </Typography>
                        <Stack direction="row" width="100%" justifyContent="end">
                            <Button
                                color="inherit"
                                onClick={coursePageStore.handleCreateAssignmentClose}
                            >
                                {t('common.close')}
                            </Button>
                            <Button
                                color="primary"
                                onClick={coursePageStore.submitAssignment}
                                disabled={!coursePageStore.isAssignmentValid}
                            >
                                {t('common.create')}
                            </Button>
                        </Stack>
                    </Stack>
                </Modal>
            </Grid>
            <Grid xs />
        </Grid>


    );
});

export default CoursePage;
