import axios from "axios";
import authHeader from "./auth-header";

const API_URL = "http://localhost:41301/api/Document/";

class DocumentsService {
    async getAll() {
        return await axios.get(API_URL + "get", { headers: await authHeader() });
    }

    async getById(id) {
        return await axios.get(API_URL  + id, { headers: await authHeader() });
    }

    async createNewDocument(document) {
        let authHeaders = await authHeader();
        console.info(document);
        return await axios.post(
            API_URL + "create",
            {
                title: document.title,
                content: document.content,
                isPublic: document.isPublic,
                link: "http://localhost:3000/documents/",
            },
            { headers: authHeaders }
        );
    }

    async updateDocument(document) {
        let authHeaders = await authHeader();
        console.info(document);
        return await axios.put(
            API_URL + document.id + "/change",
            {
                Id: document.id,
                Title: document.title,
                Content: document.content,
                IsPublic: document.isPublic,
                Link: document.link,
            },
            { headers: authHeaders }
        ).then((response)=>{
            return response;
        });
    }

    async deleteById(id) {
        return await axios.delete(API_URL + id + "/delete", {
            headers: await authHeader(),
        });
    }
}

const documentsService = new DocumentsService();

export default documentsService;
