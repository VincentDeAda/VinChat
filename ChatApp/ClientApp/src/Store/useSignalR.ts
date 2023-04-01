import { HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
import { ref } from "vue";
const connection = new HubConnectionBuilder()
  .withAutomaticReconnect()
  .withUrl('/chat')
  // .configureLogging(LogLevel.None)
  .build();

const online = ref(0)
const events = {
  MessageReceived: (incomingMessage: Message) => { },
  OnlineMemberCountChanged: (count: number) => online.value = count,
  MessageRemoved: (id: string) => { },
  MessageUpdated: (newMsg: Message) => { }
}
const removeMessage = async (id: string) => await connection.invoke('RemoveMessage', id);
const sendMessage = async (message: string) => await invoke('SendMessage', message)
const updateMessage = async (messageId: string, newContent: string) => await invoke('UpdateMessage', messageId, newContent);

const invoke = async (methodName: string, ...args: any[]) => {
  if (connection.state != HubConnectionState.Connected)
    return;
  await connection.invoke(methodName, ...args)
}



const startUp = async () => {
  connection.on('MessageReceived', events.MessageReceived);
  connection.on('MessageRemoved', events.MessageRemoved);
  connection.on('UpdateOnline', events.OnlineMemberCountChanged);
  connection.on('MessageUpdated', events.MessageUpdated);

  return await connection.start();
};
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