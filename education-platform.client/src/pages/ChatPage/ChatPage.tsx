/* eslint-disable @typescript-eslint/no-non-null-assertion */
import React, { useEffect } from 'react';
import {
    Avatar,
    Box,
    Button,
    CardActionArea,
    CircularProgress,
    FormControl,
    FormHelperText,
    IconButton,
    InputBase,
    InputLabel,
    ListItemIcon,
    ListItemText,
    Menu,
    MenuItem,
    Modal,
    Paper,
    Select,
    Skeleton,
    Stack,
    styled,
    TextField,
    Typography,
    useTheme,
} from '@mui/material';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../context/RootStoreContext';
import { useTranslation } from 'react-i18next';
import Grid from '@mui/material/Unstable_Grid2';
import { useNavigate, useParams } from 'react-router-dom';
import { Assessment, AttachFile, Delete, Edit, Link, MoreVert, Send } from '@mui/icons-material';
import FilesPicker from '../../components/FilesPicker';

import LinksPicker from '../../components/LinksPicker';
import { DateTimePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';
import LinkCard from '../../components/LinkCard';
import FileCard from '../../components/FileCard';
import DocViewer, { DocViewerRenderers } from '@cyntler/react-doc-viewer';
import ValidationError from '../../helpers/validation/ValidationError';
import InfiniteScroll from 'react-infinite-scroll-component';

const ChatPage = observer(() => {
    const { chatPageStore } = useStore();
    const theme = useTheme();
    const { t, i18n } = useTranslation();
    const navigate = useNavigate();
    const { courseId } = useParams();

    if (isNaN(Number(courseId)) ) {
        navigate('/404');
    }

    useEffect(() => {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        chatPageStore.init(
            Number.parseInt(courseId!),
            
            navigate
        );
        return () => chatPageStore.reset();
    }, [courseId]);

    const StyledCardActionArea = styled(CardActionArea)(({ theme }) => `
    .MuiCardActionArea-focusHighlight {
        background: transparent;
    }
`);

    if (chatPageStore.isLoading) {
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
            <Grid xs={12} md={9} xl={6}  >
                <Stack height="100%" justifyContent="flex-end">
                    <InfiniteScroll

                        height="65svh"
                        style={{ display: "flex", flexDirection: "column-reverse" }}
                        next={chatPageStore.loadNextPack}
                        hasMore={chatPageStore.hasMore}
                        loader={
                            <Box
                                display="flex"
                                alignItems="center"
                                justifyContent="center"
                                m={1}

                            >
                                <CircularProgress />
                            </Box>}
                        dataLength={chatPageStore.messages.length}
                        endMessage={
                            <Typography mb={2} color="InactiveCaptionText" textAlign="center">
                                {t('glossary.endMessage')}
                            </Typography>
                        }
                        inverse={true}
                    >

                        <Stack width="100%" direction="column-reverse" spacing={1} p={1}>
                            {chatPageStore.messages.map((message) => {
                                if (message.creatorId === chatPageStore.currentUser) {
                                    return (
                                        <Paper key={message.id} onContextMenu={(e) => chatPageStore.openMessageMenu(e, message.id)} sx={{ width: 'fit-content', maxWidth: '50%', alignSelf: 'end', bgcolor: theme.palette.secondary.main, p: 1 }}>
                                            <Stack height="100%" alignContent="center">
                                                <Typography>{message.messageText}</Typography>
                                                <Stack spacing={1}>
                                                    {message.attachedFiles?.map(file => (
                                                        <Box key={file.id} height={40}>
                                                            <FileCard file={file.mediaLink} onClick={() => chatPageStore.onFileClick(file.id, navigate)} />
                                                        </Box>
                                                    ))}
                                                </Stack>
                                                <Stack direction="row" justifyContent="end" spacing={1}>
                                                    <Typography variant="caption" color="InactiveCaptionText">
                                                        {message.isEdit ? t('glossary.edited') : ''}
                                                    </Typography>
                                                    <Typography variant="caption" color="InactiveCaptionText">
                                                        {message.createdIn.toLocaleString(i18n.language)}
                                                    </Typography>
                                                </Stack>
                                            </Stack>
                                        </Paper>
                                    );
                                }

                                return (
                                    <Paper key={message.id} sx={{ width: 'fit-content', maxWidth: '50%', alignSelf: 'start', p: 1 }}>
                                        <Stack height="100%">

                                            <Typography>{message.messageText}</Typography>
                                            <Stack spacing={1}>
                                                {message.attachedFiles?.map(file => (
                                                    <Box key={file.id} height={40}>
                                                        <FileCard file={file.mediaLink} onClick={() => chatPageStore.onFileClick(file.id, navigate)} />
                                                    </Box>
                                                ))}

                                            </Stack>
                                            <Stack direction="row" justifyContent="end" spacing={1}>
                                                <Typography variant="caption" color="InactiveCaptionText">
                                                    {message.isEdit ? t('glossary.edited') : ''}
                                                </Typography>
                                                <Typography variant="caption" color="InactiveCaptionText">
                                                    {message.createdIn.toLocaleString(i18n.language)}
                                                </Typography>
                                            </Stack>
                                        </Stack>
                                    </Paper>
                                )
                            })}


                        </Stack>
                    </InfiniteScroll>
                    <Menu
                        elevation={4}
                        disableAutoFocusItem
                        anchorEl={chatPageStore.messageMenuAnchor}
                        open={Boolean(
                            chatPageStore.messageMenuAnchor,
                        )}
                        onClose={chatPageStore.closeMessageMenu}
                        anchorOrigin={{
                            vertical: 'bottom',
                            horizontal: 'right',
                        }}
                        transformOrigin={{
                            vertical: 'top',
                            horizontal: 'right',
                        }}
                        onContextMenu={() => {
                            chatPageStore.closeMessageMenu()
                        }}
                    >
                        <MenuItem onClick={chatPageStore.handeEditMessageOpen}>
                            <ListItemIcon>
                                <Edit fontSize="small" />
                            </ListItemIcon>
                            <ListItemText>{t('glossary.edit')}</ListItemText>

                        </MenuItem>
                        <MenuItem onClick={chatPageStore.deleteMessage}>
                            <ListItemIcon>
                                <Delete fontSize="small" />
                            </ListItemIcon>
                            <ListItemText> {t('glossary.delete')}</ListItemText>
                        </MenuItem>
                    </Menu>
                    <Paper>
                        <Stack direction="row" minHeight={50} spacing={1} alignItems="center" justifyItems="center">
                            <IconButton color="primary" component="label">
                                <AttachFile />
                                <input type="file" hidden multiple onChange={chatPageStore.onCreateMessageFileAdd} />
                            </IconButton>
                            <TextField
                                variant="standard"
                                multiline
                                maxRows={7}
                                fullWidth
                                value={chatPageStore.createMessage?.messageText}
                                placeholder={t('glossary.writeMessage')}
                                sx={{
                                    maxHeight: 'fit-content',
                                }}
                                onChange={chatPageStore.onCreateMessageTextChange}
                                error={chatPageStore.createMessageErrors.messageText !== null}
                                helperText={
                                    chatPageStore.createMessageErrors.messageText !==
                                        null
                                        ? t(
                                            chatPageStore.createMessageErrors.messageText.errorKey,
                                            chatPageStore.createMessageErrors.messageText.options,
                                        )
                                        : null
                                }
                            />
                            <IconButton color="primary" onClick={() => chatPageStore.sendMessage()} disabled={!chatPageStore.isCreateMessageValid}>
                                <Send />
                            </IconButton>

                        </Stack>
                    </Paper>

                    <Stack spacing={1} mt={1}>
                        <Typography
                            color="error"
                            variant="caption"
                            visibility={chatPageStore.createMessageErrors.attachedFiles !== null ? 'visible' : 'collapse'}
                        >
                            {chatPageStore.createMessageErrors.attachedFiles !== null ? t(chatPageStore.createMessageErrors.attachedFiles.errorKey, chatPageStore.createMessageErrors.attachedFiles.options) : null}
                        </Typography>
                        <Stack spacing={1}>
                            {chatPageStore.createMessage?.attachedFiles.map((file, index) => (
                                <Paper key={index} variant="outlined" sx={{ overflow: 'hidden', width: '100%' }}>
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
                                            {file.name}
                                        </Typography>
                                        <IconButton onClick={() => chatPageStore.onCreateMessageFileDelete(index)}>
                                            <Delete />
                                        </IconButton>
                                    </Stack>
                                </Paper>

                            ))}
                        </Stack>
                    </Stack>
                </Stack>

                {chatPageStore.selectedMessage && chatPageStore.editMessage && <Modal open={chatPageStore.editMessageOpen} onClose={chatPageStore.handleEditMessageClose}>
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
                            {t('glossary.editMessage')}
                        </Typography>
                        <TextField
                            variant="standard"
                            multiline
                            maxRows={7}
                            fullWidth
                            value={chatPageStore.editMessage?.messageText}
                            placeholder={t('glossary.writeMessage')}
                            sx={{
                                maxHeight: 'fit-content',
                            }}
                            onChange={chatPageStore.onEditMessageTextChange}
                            error={chatPageStore.editMessageErrors.messageText !== null}
                            helperText={
                                chatPageStore.editMessageErrors.messageText !==
                                    null
                                    ? t(
                                        chatPageStore.editMessageErrors.messageText.errorKey,
                                        chatPageStore.editMessageErrors.messageText.options,
                                    )
                                    : null
                            }
                        />

                        <Stack direction="row" width="100%" justifyContent="end">
                            <Button
                                color="inherit"
                                onClick={
                                    chatPageStore.handleEditMessageClose
                                }
                            >
                                {t('common.close')}
                            </Button>
                            <Button
                                color="primary"
                                onClick={() => chatPageStore.submitEdit()}
                                disabled={!chatPageStore.isEditMessageValid}
                            >
                                {t('common.edit')}
                            </Button>
                        </Stack>
                        <Typography textAlign="start" width="100%" variant="h6">
                            {t('glossary.editFiles')}
                        </Typography>
                        <Grid container spacing={1}>
                            {chatPageStore.messages!.find(m => m.id === chatPageStore.selectedMessage)!.attachedFiles!.map((file) => (
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
                                                {file.mediaLink!.slice(file.mediaLink!.indexOf('_') + 1)}
                                            </Typography>
                                            {chatPageStore.canDeleteFile && <IconButton onClick={() => chatPageStore.onEditMessageFileDelete(file.id, navigate)}>
                                                <Delete />
                                            </IconButton>}
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
                                    <input type="file" hidden onChange={(e) => chatPageStore.onEditMessageFileAdd(e, navigate)} />
                                </Button>
                            </Grid>
                        </Grid>
                    </Stack>
                </Modal>}

                <Modal
                        open={chatPageStore.isFileViewerOpen}
                        onClose={chatPageStore.onFileViewerClose}
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
                                    visibility: chatPageStore.isFileViewerOpen
                                        ? 'visible'
                                        : 'collapse',
                                }}
                                documents={[{ uri: chatPageStore.fileLink }]}
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
            <Grid xs />
        </Grid>
    );
});

export default ChatPage;