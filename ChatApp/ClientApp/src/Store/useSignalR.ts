import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { ref } from "vue";
const connection = new HubConnectionBuilder()
  .withAutomaticReconnect()
  .withUrl('/chat')
  .build();

const online = ref(0)
const events = {
  MessageReceived: (incomingMessage: Message) => {
  },
  OnlineMemberCountChanged: (count: number) => online.value = count,
  MessageRemoved: (id: string) => { },
  MessageUpdated: (newMsg: Message) => { }
}
const removeMessage = async (id: string) => {
  if (connection.state != HubConnectionState.Connected)
    return;
  await connection.invoke('RemoveMessage', id)
}

const sendMessage = async (message: string) => {
  if (connection.state != HubConnectionState.Connected)
    return;
  await connection.invoke('SendMessage', message)
}

const updateMessage = async (messageId: string, newContent: string) => {
  if (connection.state != HubConnectionState.Connected)
    return;
  await connection.invoke('UpdateMessage', messageId, newContent)
}
connection.on('MessageReceived', (e) => events.MessageReceived(e));
connection.on('MessageRemoved', (e) => events.MessageRemoved(e));
connection.on('UpdateOnline', (e) => events.OnlineMemberCountChanged(e));
connection.on('MessageUpdated', (e) => events.MessageUpdated(e));



const startUp = async () => await connection.start();
export default function () {
  return {
    startUp,
    sendMessage,
    removeMessage,
    updateMessage,
    events,
    online
  }
}