interface LoginRequest {
  username: string,
  password: string
}
interface Author {
  id: string,
  username: string
}
interface UserInfo {
  id: string,
  username: string
}
interface Message {
  id: string,
  message: string,
  author: Author,
  date: Date,
  isEdited: boolean,
  lastEditDate?: Date
}