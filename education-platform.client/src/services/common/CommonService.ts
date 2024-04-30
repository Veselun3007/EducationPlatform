import axios from "axios";

export default class CommonService {
    async loadFile(link: string) {
        try {
            const response = await axios.get(link, { responseType: 'blob' });
            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
            const name = link.split('?')[0].split('/').pop();

            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
            const file = new File([response.data], name!);
            return file;
        } catch {
            return undefined;
        }
    }
}