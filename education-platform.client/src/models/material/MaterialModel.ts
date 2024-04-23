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

    // constructor(id: number, materialName: string, 
    //     materialDatePublication: Date, isEdited: boolean,
    //     topicId?: number, materialDescription?: string,
    //     editedTime?: Date, materialfiles?: MaterialFileModel[],
    //     materiallinks?: MaterialLinkModel[]
    // ) {
    //     this.id = id;
    //     this.editedTime = editedTime;
    //     this.materialName = materialName;
    //     this.materialDescription = materialDescription;
    //     this.materialDatePublication = materialDatePublication;
    //     this.isEdited = isEdited;
    //     this.topicId = topicId;
    //     this.materialfiles = materialfiles;
    //     this.materiallinks = materiallinks
        
        
    // }

}
