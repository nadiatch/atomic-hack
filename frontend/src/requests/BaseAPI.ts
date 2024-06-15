import AXIOS from "./_axios";
import {IMessage} from "../components/Chat";

export function send(message: IMessage | undefined, _callback: (response: any) => void) {
    AXIOS.post(`/api/v1/send`, message).then((response) => {
        _callback(response);
    })
}
