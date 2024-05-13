/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { Box, Button, IconButton, Paper, Stack, TextField, Typography, useTheme } from '@mui/material';
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
import SAInfoModel from '../models/studentAssignment/SAInfoModel';
import UpdateMarkModel from '../models/studentAssignment/UpdateMarkModel';
import FileCard from './FileCard';
import AssignmentModel from '../models/assignment/AssignmentModel';
import Grid2 from '@mui/material/Unstable_Grid2/Grid2';
import CommentBlock from './CommentBlock';

interface MarkCardProps {

    saInfo: SAInfoModel;
    onSubmitMark(mark: UpdateMarkModel, navigate: NavigateFunction): Promise<void>;
    sendComment(comment: CreateCommentModel, navigate: NavigateFunction): Promise<void>;
    currentUser: number;
    onFileClick(id: number, navigate: NavigateFunction): Promise<void>,
    assignment: AssignmentModel;
}

const MarkCard: React.FC<MarkCardProps> = observer(({ saInfo, onSubmitMark, onFileClick, currentUser, sendComment, assignment }) => {
    const { t, i18n } = useTranslation();
    const navigate = useNavigate();
    const theme = useTheme();
    const store = useLocalObservable(
        () => ({
            updateMark: new UpdateMarkModel(saInfo.studentAssignment.currentMark ? saInfo.studentAssignment.currentMark : assignment.minMark, saInfo.studentAssignment.studentassignmentId),
            error: null as ValidationError | null,

            get isValid() {
                return this.updateMark.validateNewMark(assignment.maxMark, assignment.minMark).length === 0
            },

            reset() {
                this.updateMark.newMark = saInfo.studentAssignment.currentMark ? saInfo.studentAssignment.currentMark : assignment.minMark;
                this.error = null;
            },

            onMarkChange(e: React.ChangeEvent<HTMLInputElement>) {
                if (e.target.value) {
                    this.updateMark.newMark = Number.parseInt(e.target.value);
                    debounce(
                        action(() => {
                            const errors = this.updateMark.validateNewMark(assignment.maxMark, assignment.minMark)
                            this.error =
                                errors.length !== 0 ? errors[0] : null;
                        }),
                    )();
                }
            },


        }),
        {
            updateMark: observable,
            reset: action.bound,
            error: observable,
            onMarkChange: action.bound,
            isValid: computed,
        },
    );



    return (
        <Paper sx={{ width: '100%', p: 1 }}>
            <Stack width="100%" spacing={2} >
                <Stack direction="row" alignItems="center" spacing={1}>
                    <ColoredAvatar
                        src={saInfo.userInfo.imageLink}
                        alt={saInfo.userInfo.userName}
                    />
                    <Typography flexGrow={1}>
                        {saInfo.userInfo.userName}
                    </Typography>
                    {saInfo?.studentAssignment.isDone
                        && saInfo?.studentAssignment.currentMark &&
                        <Stack direction="column">
                            <Typography color={theme.palette.success.main}>
                                {t('glossary.graded')}
                            </Typography>
                            <Typography color={theme.palette.success.main}>
                                {`${saInfo.studentAssignment.currentMark}/${assignment!.maxMark}`}
                            </Typography>
                        </Stack>
                    }

                    {saInfo?.studentAssignment.isDone
                        && !saInfo?.studentAssignment.currentMark
                        && saInfo.studentAssignment.submissionDate
                        && saInfo.studentAssignment.submissionDate < assignment!.assignmentDeadline
                        && saInfo.files.length > 0
                        &&

                        <Typography>
                            {t('glossary.submitted')}
                        </Typography>

                    }

                    {saInfo?.studentAssignment.isDone
                        && !saInfo?.studentAssignment.currentMark
                        && saInfo.studentAssignment.submissionDate
                        && saInfo.studentAssignment.submissionDate > assignment!.assignmentDeadline
                        &&

                        <Typography color={theme.palette.error.light}>
                            {t('glossary.submittedExpired')}
                        </Typography>

                    }

                    {!saInfo?.studentAssignment.isDone
                        && new Date(Date.now()) > assignment!.assignmentDeadline
                        &&

                        <Typography color={theme.palette.error.main}>
                            {t('glossary.expired')}
                        </Typography>

                    }

                    {!saInfo?.studentAssignment.isDone
                        && new Date(Date.now()) < assignment!.assignmentDeadline
                        &&

                        <Typography color={theme.palette.success.main}>
                            {t('glossary.assigned')}
                        </Typography>

                    }

                    {saInfo?.studentAssignment.isDone
                        && !saInfo?.studentAssignment.currentMark
                        && saInfo.studentAssignment.submissionDate
                        && saInfo.studentAssignment.submissionDate < assignment!.assignmentDeadline
                        && saInfo.files.length === 0
                        &&

                        <Typography >
                            {t('glossary.markedAsDone')}
                        </Typography>

                    }
                </Stack>
                <Typography variant='h6'>
                    {t('glossary.attachedFiles')}
                </Typography>

                {saInfo.files.length > 0 ?
                    <Grid2 container spacing={1}>


                        {saInfo.files.map((file) =>

                            <Grid2 key={file.attachedFileId} xs={12} md={6} height={55}>
                                <FileCard file={file.attachedFileName!.slice(file.attachedFileName!.indexOf('_') + 1)}
                                    onClick={() => onFileClick(file.attachedFileId, navigate)} />
                            </Grid2>
                        )}
                    </Grid2>
                    : <Typography textAlign="center" color="InactiveCaptionText">
                        {t('glossary.noAttachedFiles')}
                    </Typography>
                }
                <TextField type="number"
                    InputProps={{ inputProps: { min: assignment.minMark, max: assignment.maxMark } }}
                    fullWidth
                    required
                    label={t('common.mark')}
                    value={
                        store.updateMark.newMark
                    }
                    onChange={store.onMarkChange}
                    error={
                        store.error !== null
                    }
                    helperText={
                        store.error !== null
                            ? t(
                                store.error.errorKey,
                                store.error.options,
                            )
                            : null
                    }
                />
                <Button variant='contained' color="success" onClick={() => onSubmitMark(store.updateMark, navigate)}>{saInfo.studentAssignment.currentMark ? t('glossary.changeGrade') : t('glossary.gradeWork')}</Button>
                <CommentBlock comments={saInfo.comments} currentUser={currentUser} assignmentId={saInfo.studentAssignment.studentassignmentId} sendComment={sendComment} />
            </Stack>
        </Paper>
    );

});

export default MarkCard;