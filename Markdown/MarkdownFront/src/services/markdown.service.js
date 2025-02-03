import axios from "axios";
import authHeader from "./auth-header";

const API_URL = "http://localhost:41301/api/Markdown/";

class MarkdownService {
    async convert(text) {
        return await axios.post(API_URL + "convert",
            {
                RawText: text
            },
            { headers: await authHeader() });
    }
}

const markdownService = new MarkdownService();

export default markdownService;
