import {
    Box,
    IconButton,
    Paper,
    Stack,
    TextField,
    Typography,
    useTheme,
} from '@mui/material';
import { observer, useLocalObservable } from 'mobx-react-lite';
import React from 'react';
import CommentInfoModel from '../models/studentAssignment/CommenttInfoModel';
import { useTranslation } from 'react-i18next';
import ColoredAvatar from './ColoredAvatar';
import { action, computed, observable } from 'mobx';
import CreateCommentModel from '../models/studentAssignment/CreateComentModel';
import debounce from '../helpers/debounce';
import ValidationError from '../helpers/validation/ValidationError';
import { Send } from '@mui/icons-material';
import { NavigateFunction, useNavigate } from 'react-router-dom';

interface CommentBlockProps {
    comments: CommentInfoModel[];
    currentUser: number;
    assignmentId: number;
    sendComment(comment: CreateCommentModel, navigate: NavigateFunction): Promise<void>;
}

const CommentBlock: React.FC<CommentBlockProps> = observer(
    ({ comments, currentUser, sendComment, assignmentId }) => {
        const { t, i18n } = useTranslation();
        const navigate = useNavigate();
        const theme = useTheme();
        const store = useLocalObservable(
            () => ({
                createComment: new CreateCommentModel('', assignmentId),
                error: null as ValidationError | null,

                get isValid() {
                    return this.createComment.validateCommentText().length === 0;
                },

                reset() {
                    this.createComment.commentText = '';
                    this.error = null;
                },

                onCommentChange(e: React.ChangeEvent<HTMLInputElement>) {
                    this.createComment.commentText = e.target.value;
                    debounce(
                        action(() => {
                            const errors = this.createComment.validateCommentText();
                            this.error = errors.length !== 0 ? errors[0] : null;
                        }),
                    )();
                },
            }),
            {
                createComment: observable,
                reset: action.bound,
                error: observable,
                onCommentChange: action.bound,
                isValid: computed,
            },
        );

        return (
            <Box width="100%">
                <Stack width="100%" spacing={1}>
                    <Stack
                        width="100%"
                        spacing={1}
                        p={1}
                        bgcolor={theme.palette.background.default}
                    >
                        {comments.length > 0 ? (
                            comments.map((comment) => {
                                if (comment.comment.courseUserId === currentUser) {
                                    return (
                                        <Paper
                                            key={comment.comment.commentId}
                                            sx={{
                                                width: 'fit-content',
                                                maxWidth: '50%',
                                                alignSelf: 'end',
                                                bgcolor: theme.palette.secondary.main,
                                                p: 1,
                                            }}
                                        >
                                            <Stack height="100%" alignContent="center">
                                                <Typography>
                                                    {comment.comment.commentText}
                                                </Typography>

                                                <Typography
                                                    textAlign="left"
                                                    variant="caption"
                                                    color="InactiveCaptionText"
                                                >
                                                    {comment.comment.commentDate.toLocaleString(
                                                        i18n.language,
                                                    )}
                                                </Typography>
                                            </Stack>
                                        </Paper>
                                    );
                                }

                                return (
                                    <Stack
                                        key={comment.comment.commentId}
                                        direction="row"
                                        spacing={2}
                                        alignItems="center"
                                    >
                                        <ColoredAvatar
                                            src={comment.userInfo.imageLink}
                                            alt={comment.userInfo.userName}
                                        />
                                        <Paper
                                            sx={{
                                                width: 'fit-content',
                                                maxWidth: '50%',
                                                alignSelf: 'start',
                                                p: 1,
                                            }}
                                        >
                                            <Stack height="100%">
                                                <Typography
                                                    color={theme.palette.secondary.light}
                                                >
                                                    {comment.userInfo.userName}
                                                </Typography>
                                                <Typography>
                                                    {comment.comment.commentText}
                                                </Typography>
                                                <Typography
                                                    textAlign="left"
                                                    variant="caption"
                                                    color="InactiveCaptionText"
                                                >
                                                    {comment.comment.commentDate.toLocaleString(
                                                        i18n.language,
                                                    )}
                                                </Typography>
                                            </Stack>
                                        </Paper>
                                    </Stack>
                                );
                            })
                        ) : (
                            <Typography textAlign="center" color="InactiveCaptionText">
                                {t('glossary.noComments')}{' '}
                            </Typography>
                        )}
                    </Stack>
                    <Stack direction="row">
                        <TextField
                            variant="standard"
                            fullWidth
                            required
                            type="text"
                            placeholder={t('glossary.writeComment')}
                            value={store.createComment.commentText}
                            onChange={store.onCommentChange}
                            error={store.error !== null}
                            helperText={
                                store.error !== null
                                    ? t(store.error.errorKey, store.error.options)
                                    : null
                            }
                        />
                        <IconButton
                            color="primary"
                            onClick={async () => {
                                await sendComment(store.createComment, navigate);
                                store.reset();
                            }}
                            disabled={!store.isValid}
                        >
                            <Send />
                        </IconButton>
                    </Stack>
                </Stack>
            </Box>
        );
    },
);

export default CommentBlock;
