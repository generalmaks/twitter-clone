export interface CreateUser {
  username: string;
  email: string;
  displayUsername: string;
  password: string;
  bio?: string | null;
}

export interface GetUser {
  id: number;
  username?: string | null;
  displayUsername?: string | null;
  bio?: string | null;
  createdAt: string;
}

export interface UpdateUser {
  id: number;
  username?: string | null;
  displayUsername?: string | null;
  bio?: string | null;
  password?: string | null;
}