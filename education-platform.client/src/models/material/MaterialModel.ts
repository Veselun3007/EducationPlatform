import MaterialFileModel from './MaterialFileModel';
import MaterialLinkModel from './MaterialLinkModel';

export default interface MaterialModel {
    id: number;
    topicId?: number;
    materialName: string;
    materialDescription?: string;
    materialDatePublication: Date;
    isEdited: boolean;
    editedTime?: Date;
    materialfiles?: MaterialFileModel[];
    materiallinks?: MaterialLinkModel[];
}
