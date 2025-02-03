import DocumentsService from "../services/documents.service";
import { CLEAR_MESSAGE, SET_MESSAGE } from "./types";

export const createDocument = (document) => (dispatch) => {
    return DocumentsService.createNewDocument(document).then(
        () => {
            dispatch({
                type: CLEAR_MESSAGE,
            });

            return Promise.resolve();
        },
        (error) => {
            const message =
                (error.response && error.response.data.error) ||
                error.message ||
                error.toString();

            dispatch({
                type: SET_MESSAGE,
                payload: message,
            });
            return Promise.reject();
        }
    );
};

export const updateDocument = (document) => (dispatch) => {
    return DocumentsService.updateDocument(document).then(
        () => {
            dispatch({
                type: CLEAR_MESSAGE,
            });

            return Promise.resolve();
        },
        (error) => {
            const message =
                (error.response && error.response.data.error) ||
                error.message ||
                error.toString();

            dispatch({
                type: SET_MESSAGE,
                payload: message,
            });
            return Promise.reject();
        }
    );
};
