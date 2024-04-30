/* eslint-disable @typescript-eslint/no-non-null-assertion */
import React, { useEffect } from 'react';
import {
    Box,
    Button,
    CircularProgress,
    FormControl,
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
import { useStore } from '../../context/RootStoreContext';
import { useTranslation } from 'react-i18next';
import Grid from '@mui/material/Unstable_Grid2';
import { useNavigate, useParams } from 'react-router-dom';
import { MoreVert } from '@mui/icons-material';
import FilesPicker from '../../components/FilesPicker';

import LinksPicker from '../../components/LinksPicker';
import LinkCard from '../../components/LinkCard';
import FileCard from '../../components/FileCard';
import DocViewer, { DocViewerRenderers } from '@cyntler/react-doc-viewer';

const MaterialPage = observer(() => {
    const { materialPageStore } = useStore();
    const theme = useTheme();
    const { t, i18n } = useTranslation();
    const navigate = useNavigate();
    const { courseId, materialId } = useParams();

    if (isNaN(Number(courseId)) || isNaN(Number(materialId))) {
        navigate('/404');
    }

    useEffect(() => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        materialPageStore.init(Number.parseInt(courseId!), Number.parseInt(materialId!));
        return () => materialPageStore.reset();
    }, [courseId, materialId]);

    if (materialPageStore.isLoading) {
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
    const editString = materialPageStore.material!.isEdited
        ? ` (${t(
              'glossary.changed',
          )}: ${materialPageStore.material!.editedTime?.toLocaleString(
              i18n.language,
          )})`
        : '';

    return (
        <Grid container mt={2}>
            <Grid xs />
            <Grid xs={12} md={10} xl={8}>
                <Stack spacing={2}>
                    <Stack
                        borderBottom={2}
                        borderColor={theme.palette.primary.light}
                        width="100%"
                        spacing={1}
                        pb={1}
                    >
                        <Stack direction="row" justifyContent="space-between">
                            <Typography variant="h5" color={theme.palette.primary.light}>
                                {materialPageStore.material!.materialName}
                            </Typography>
                            <IconButton
                                id="menu-button"
                                aria-controls={
                                    materialPageStore.materialMenuAnchor
                                        ? 'material-menu'
                                        : undefined
                                }
                                aria-haspopup="true"
                                aria-expanded={
                                    materialPageStore.materialMenuAnchor
                                        ? 'true'
                                        : undefined
                                }
                                onClick={materialPageStore.openMaterialMenu}
                                sx={{ textAlign: 'end' }}
                            >
                                <MoreVert />
                            </IconButton>
                            <Menu
                                elevation={4}
                                id="material-menu"
                                anchorEl={materialPageStore.materialMenuAnchor}
                                open={Boolean(materialPageStore.materialMenuAnchor)}
                                onClose={materialPageStore.closeMaterialMenu}
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
                                    onClick={materialPageStore.handleEditMaterialOpen}
                                >
                                    {t('glossary.editMaterial')}
                                </MenuItem>
                                <MenuItem
                                    onClick={() =>
                                        materialPageStore.deleteMaterial(
                                            Number.parseInt(courseId!),
                                            navigate,
                                        )
                                    }
                                >
                                    {t('glossary.deleteMaterial')}
                                </MenuItem>
                            </Menu>
                        </Stack>
                        <Typography variant="caption">
                            {materialPageStore.material!.materialDatePublication.toLocaleString(
                                i18n.language,
                            ) + editString}
                        </Typography>
                    </Stack>
                    <Typography>
                        {materialPageStore.material!.materialDescription}
                    </Typography>
                    <Grid container>
                        {
                            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                            materialPageStore.material!.materiallinks!.map((link) => (
                                <Grid key={link.id} xs={12} md={6} height={50}>
                                    <LinkCard
                                        link={
                                            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                            link.materialLink!
                                        }
                                    />
                                </Grid>
                            ))
                        }
                    </Grid>

                    <Grid container>
                        {
                            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                            materialPageStore.material!.materialfiles!.map(
                                (file, index) => (
                                    <Grid key={file.id} xs={12} md={6} height={50}>
                                        <FileCard
                                            file={
                                                // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                                                file.materialFile!
                                            }
                                            onClick={() =>
                                                materialPageStore.onFileClick(index)
                                            }
                                        />
                                    </Grid>
                                ),
                            )
                        }
                    </Grid>
                </Stack>
                <Modal
                    open={materialPageStore.isFileViewerOpen}
                    onClose={materialPageStore.onFileViewerClose}
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
                                visibility: materialPageStore.isFileViewerOpen
                                    ? 'visible'
                                    : 'collapse',
                            }}
                            documents={[{ uri: materialPageStore.fileLink }]}
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
                    open={materialPageStore.editMaterialOpen}
                    onClose={materialPageStore.handleEditMaterialClose}
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
                            {t('glossary.editMaterial')}
                        </Typography>
                        <TextField
                            fullWidth
                            required
                            type="text"
                            label={t('common.name')}
                            value={materialPageStore.editMaterialData?.materialName}
                            onChange={materialPageStore.onMaterialNameChange}
                            error={
                                materialPageStore.editMaterialErrors.materialName !== null
                            }
                            helperText={
                                materialPageStore.editMaterialErrors.materialName !== null
                                    ? t(
                                          materialPageStore.editMaterialErrors
                                              .materialName.errorKey,
                                          materialPageStore.editMaterialErrors
                                              .materialName.options,
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
                                materialPageStore.editMaterialData?.materialDescription
                            }
                            onChange={materialPageStore.onMaterialDescriptionChange}
                        />
                        <FormControl fullWidth>
                            <InputLabel id="materialTopicSelect">
                                {t('common.topic')}
                            </InputLabel>
                            <Select
                                labelId="materialTopicSelect"
                                value={
                                    materialPageStore.editMaterialData?.topicId
                                        ? materialPageStore.editMaterialData?.topicId.toString()
                                        : '0'
                                }
                                label={t('common.topic')}
                                onChange={materialPageStore.onMaterialTopicChange}
                                fullWidth
                            >
                                <MenuItem value={0}>{t('glossary.noTopic')}</MenuItem>
                                {materialPageStore.topics?.map((topic) => (
                                    <MenuItem key={topic.id} value={topic.id}>
                                        {topic.title}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                        <FilesPicker
                            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                            files={materialPageStore.editMaterialData!.materialFiles}
                            error={materialPageStore.editMaterialErrors.materialFiles}
                            onFileAdd={materialPageStore.onMaterialFileAdd}
                            onFileDelete={materialPageStore.onMaterialFileDelete}
                        />

                        <LinksPicker
                            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                            links={materialPageStore.editMaterialData!.materialLinks}
                            error={materialPageStore.editMaterialErrors.materialLinks}
                            onLinkAdd={materialPageStore.onMaterialLinkAdd}
                            onLinkDelete={materialPageStore.onMaterialLinkDelete}
                        />
                        <Typography
                            color="error"
                            variant="caption"
                            align="center"
                            visibility={
                                materialPageStore.editMaterialErrors.meta !== null
                                    ? 'visible'
                                    : 'collapse'
                            }
                        >
                            {materialPageStore.editMaterialErrors.meta !== null
                                ? t(
                                      materialPageStore.editMaterialErrors.meta.errorKey,
                                      materialPageStore.editMaterialErrors.meta.options,
                                  )
                                : null}
                        </Typography>
                        <Stack direction="row" width="100%" justifyContent="end">
                            <Button
                                color="inherit"
                                onClick={materialPageStore.handleEditMaterialClose}
                            >
                                {t('common.close')}
                            </Button>
                            <Button
                                color="primary"
                                onClick={materialPageStore.submitMaterialEdit}
                                disabled={!materialPageStore.isEditMaterialValid}
                            >
                                {t('common.edit')}
                            </Button>
                        </Stack>
                    </Stack>
                </Modal>
            </Grid>
            <Grid xs />
        </Grid>
    );
});

export default MaterialPage;
